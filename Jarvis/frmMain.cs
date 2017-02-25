using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using System.IO.Ports;
using MyoSharp.Device;
using MyoSharp.Communication;
using MyoSharp.Exceptions;
using MyoSharp.Poses;
using System.Speech.Synthesis;
using Microsoft.CognitiveServices.SpeechRecognition;
using System.Net;
using WeatherAssignment;
using Emgu.CV.Util;
using TTSSample;
using Microsoft.ProjectOxford.SpeakerRecognition;
using Microsoft.ProjectOxford.SpeakerRecognition.Contract.Identification;

namespace Jarvis
{
    public partial class frmMain : Form
    {

        #region InitialLoad

        SpeechSynthesizer synthesizer;

        public frmMain()
        {
            InitializeComponent();
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            faceClassifier = new CascadeClassifier(Application.StartupPath + "/Cascades/haarcascade_frontalface_default.xml");
            eigenRecognition = new ClassifierTrain();
            faceRecognition = false;
            ballRecognition = false;

            synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = -2;     // -10...10

            if (eigenRecognition.IsTrained)
            {
                labelMessage.Text = "Training Data loaded";
            }
            else
            {
                labelMessage.Text = "No training data found, please train program using Train menu option";
            }

            InitializeMyo();

            for (int i = 0; i < 5; i++)
            {
                maxedOut[i] = new bool[2];
            }
            resetMaxedOut();

            boxApiKey.Text = "04473eab9ed146139f04fc5d9b6bf724";

        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopMyo();

        }

        #endregion

        #region Video

        Mat currentFrame; //current image aquired from webcam for display
        Mat grayFrame;
        Image<Gray, byte> result;
        VideoCapture grabber; //This is our capture variable
        public CascadeClassifier faceClassifier;
        ClassifierTrain eigenRecognition;
        bool faceRecognition;
        bool ballRecognition;

        bool trackFace = true;
        string trackName = "Andrei";
        bool foundFace = false;
        bool foundBall = false;
        bool trackBall = true;
        int middleX;
        int middleY;
        double maxDist;

        int framePause;
        int frameLimit = 3;

        int hueRed = 160;
        int hueBlue = 110;
        bool blueBallMode = false;

        #region GuiInteraction
        private void imgBoxFace_Click(object sender, EventArgs e)
        {

        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            stopCapture();

            //OpenForm
            frmTrain TF = new frmTrain(this);
            TF.Show();
        }

        private void btnFaceRecognition_Click(object sender, EventArgs e)
        {
            if (faceRecognition)
            {
                faceRecognition = false;
                btnFaceRecognition.Enabled = true;
                stopCapture();
            }
            else
            {
                faceRecognition = true;
                btnFaceRecognition.Enabled = false;
                if (ballRecognition)
                {
                    ballRecognition = false;
                    btnBallRecognition.Enabled = true;
                    stopCaptureBall();
                }
                initialiseCapture();
            }
        }
        private void btnBallRecognition_Click(object sender, EventArgs e)
        {
            if (ballRecognition)
            {
                ballRecognition = false;
                btnBallRecognition.Enabled = true;
                stopCaptureBall();
            }
            else
            {
                ballRecognition = true;
                btnBallRecognition.Enabled = false;
                if (faceRecognition)
                {
                    faceRecognition = false;
                    btnFaceRecognition.Enabled = true;
                    stopCapture();
                }
                initialiseCaptureBall();
            }
        }
        #endregion

        #region FaceRecognition
        public void initialiseCapture()
        {
            grabber = new VideoCapture();
            grabber.QueryFrame();

            framePause = 0;
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber_Parrellel);

            sendCode(RESET, RESET, RESET_BALL, RESET);
        }
        private void stopCapture()
        {
            Application.Idle -= new EventHandler(FrameGrabber_Parrellel);
            if (grabber != null)
            {
                grabber.Dispose();
            }
        }

        void FrameGrabber_Parrellel(object sender, EventArgs e)
        {
            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame();
            grayFrame = new Mat(currentFrame.Size, DepthType.Cv8U, 3);

            //Convert it to Grayscale
            //Clear_Faces_Found();

            if (currentFrame != null)
            {
                CvInvoke.CvtColor(currentFrame, grayFrame, ColorConversion.Bgr2Gray);

                //Face Detector
                Rectangle[] facesDetected = faceClassifier.DetectMultiScale(grayFrame, 1.2, 10);
                //Rectangle[] facesDetected = Face.DetectMultiScale(gray_frame, 1.2, 10, new Size(50, 50), Size.Empty);

                if (trackFace && !foundFace && !maxedOut[0][0] && !maxedOut[0][1])
                {
                    sendCode(MOVE_LEFT, PAUSE, PAUSE, PAUSE);
                }

                //Action for each element detected
                Parallel.For(0, facesDetected.Length, i =>
                {
                    try
                    {
                        facesDetected[i].X += (int)(facesDetected[i].Height * 0.15);
                        facesDetected[i].Y += (int)(facesDetected[i].Width * 0.22);
                        facesDetected[i].Height -= (int)(facesDetected[i].Height * 0.3);
                        facesDetected[i].Width -= (int)(facesDetected[i].Width * 0.35);

                        result = new Mat(currentFrame, facesDetected[i]).ToImage<Gray, byte>();
                        result._EqualizeHist();
                        //draw the face detected in the 0th (gray) channel with blue color
                        CvInvoke.Rectangle(currentFrame, facesDetected[i], new MCvScalar(255, 0, 0), 2);


                        // reset foundface
                        //foundFace = false;

                        if (eigenRecognition.IsTrained)
                        {
                            string name = eigenRecognition.Recognise(result);
                            int match_value = (int)eigenRecognition.Get_Eigen_Distance;
                            //Draw the label for each face detected and recognized
                            CvInvoke.PutText(currentFrame, name + " ", new Point(facesDetected[i].X, facesDetected[i].Y), FontFace.HersheyComplex, 1, new MCvScalar(0, 255, 0), 1);
                            Console.WriteLine(name);
                            if (trackFace && name.Equals(trackName))
                            {
                                foundFace = true;
                                middleX = facesDetected[i].X + facesDetected[i].Width / 2;
                                middleY = facesDetected[i].Y + facesDetected[i].Height / 2;
                                CvInvoke.Circle(currentFrame, new Point((int)middleX, (int)middleY), 3, new MCvScalar(0, 255, 0), -1);

                                if (framePause++ == frameLimit)
                                {
                                    rotateHand(middleX, middleY);
                                    framePause = 0;
                                }
                            }
                            else
                            {
                                if (framePause++ == frameLimit)
                                {
                                    sendCode(MOVE_LEFT, PAUSE, PAUSE, PAUSE);
                                    framePause = 0;
                                }
                            }

                        }

                    }
                    catch
                    {
                        //do nothing as parrellel loop buggy
                        //No action as the error is useless, it is simply an error in 
                        //no data being there to process and this occurss sporadically 
                    }
                });
                //Show the faces procesed and recognized
                imgBoxFace.Image = currentFrame;
            }
        }
        public void retrain()
        {
            eigenRecognition = new ClassifierTrain();
            if (eigenRecognition.IsTrained)
            {
                labelMessage.Text = "Training Data loaded";
            }
            else
            {
                labelMessage.Text = "No training data found, please train program using Train menu option";
            }
        }



        #endregion

        #region BallRecognition
        public void initialiseCaptureBall()
        {
            grabber = new VideoCapture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(processFrameAndUpdateGUI);

            sendCode(RESET, RESET, RESET_BALL, RESET);
        }

        public void stopCaptureBall()
        {
            Application.Idle -= new EventHandler(processFrameAndUpdateGUI);
            if (grabber != null)
            {
                grabber.Dispose();
            }
        }

        void processFrameAndUpdateGUI(object sender, EventArgs arg)
        {

            currentFrame = grabber.QueryFrame();

            if (currentFrame == null)
            {
                MessageBox.Show("unable to read from webcam" + Environment.NewLine + Environment.NewLine +
                                "exiting program");
                Environment.Exit(0);
                return;
            }

            Mat imgHSV = new Mat(currentFrame.Size, DepthType.Cv8U, 3);

            Mat imgThreshLow = new Mat(currentFrame.Size, DepthType.Cv8U, 1);
            Mat imgThreshHigh = new Mat(currentFrame.Size, DepthType.Cv8U, 1);

            Mat imgThresh = new Mat(currentFrame.Size, DepthType.Cv8U, 1);

            CvInvoke.CvtColor(currentFrame, imgHSV, ColorConversion.Bgr2Hsv);

            //CvInvoke.InRange(imgHSV, new ScalarArray(new MCvScalar(0, 155, 155)), new ScalarArray(new MCvScalar(18, 255, 255)), imgThreshLow);
            //CvInvoke.InRange(imgHSV, new ScalarArray(new MCvScalar(165, 155, 155)), new ScalarArray(new MCvScalar(179, 255, 255)), imgThreshHigh);

            //CvInvoke.Add(imgThreshLow, imgThreshHigh, imgThresh);

            //CvInvoke.InRange(imgHSV, new ScalarArray(new MCvScalar(int.Parse(boxHue.Text) - 10, 100, 100)), new ScalarArray(new MCvScalar(int.Parse(boxHue.Text) + 10, 255, 255)), imgThresh);

            CvInvoke.InRange(imgHSV, new ScalarArray(new MCvScalar(getHue()-10, 130, 130)), new ScalarArray(new MCvScalar(getHue()+10, 255, 255)), imgThresh);

            CvInvoke.GaussianBlur(imgThresh, imgThresh, new Size(3, 3), 0);

            Mat structuringElement = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new Size(3, 3), new Point(-1, -1));

            CvInvoke.Dilate(imgThresh, imgThresh, structuringElement, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));
            CvInvoke.Erode(imgThresh, imgThresh, structuringElement, new Point(-1, -1), 1, BorderType.Default, new MCvScalar(0, 0, 0));


            // circles way



            //CircleF[] circles = CvInvoke.HoughCircles(imgThresh, HoughType.Gradient, 2.0, imgThresh.Rows / 4, 100, 50, 20, 200);

            ////CvInvoke.FindContours()
            //float max = 0;
            //CircleF f = new CircleF(new PointF(0,0), -1);

            //foreach (CircleF circle in circles)
            //{
            //    if (circle.Radius > max)
            //    {
            //        max = circle.Radius;
            //        f = circle;
            //    }

            //}


            ////foreach (CircleF circle in circles)
            ////{
            ////    if (boxBallPosition.Text != "")
            ////    {                         // if we are not on the first line in the text box
            ////        boxBallPosition.AppendText(Environment.NewLine);         // then insert a new line char
            ////    }

            ////    boxBallPosition.AppendText("ball position x = " + circle.Center.X.ToString().PadLeft(4) + ", y = " + circle.Center.Y.ToString().PadLeft(4) + ", radius = " + circle.Radius.ToString("###.000").PadLeft(7));
            ////    boxBallPosition.ScrollToCaret();             // scroll down in text box so most recent line added (at the bottom) will be shown

            ////    CvInvoke.Circle(currentFrame, new Point((int)circle.Center.X, (int)circle.Center.Y), (int)circle.Radius, new MCvScalar(0, 0, 255), 2);
            ////    CvInvoke.Circle(currentFrame, new Point((int)circle.Center.X, (int)circle.Center.Y), 3, new MCvScalar(0, 255, 0), -1);
            ////}

            //if (f.Radius > 0)
            //{
            //    if (boxBallPosition.Text != "")
            //    {                         // if we are not on the first line in the text box
            //        boxBallPosition.AppendText(Environment.NewLine);         // then insert a new line char
            //    }

            //    boxBallPosition.AppendText("ball position x = " + f.Center.X.ToString().PadLeft(4) + ", y = " + f.Center.Y.ToString().PadLeft(4) + ", radius = " + f.Radius.ToString("###.000").PadLeft(7));
            //    boxBallPosition.ScrollToCaret();             // scroll down in text box so most recent line added (at the bottom) will be shown

            //    CvInvoke.Circle(currentFrame, new Point((int)f.Center.X, (int)f.Center.Y), (int)f.Radius, new MCvScalar(0, 0, 255), 2);
            //    CvInvoke.Circle(currentFrame, new Point((int)f.Center.X, (int)f.Center.Y), 3, new MCvScalar(0, 255, 0), -1);

            //    if (trackBall)
            //    {
            //        foundBall = true;
            //        rotateHand((int)f.Center.X, (int)f.Center.Y);
            //    }

            //}

            if (trackBall && !foundBall && !maxedOut[0][0] && !maxedOut[0][1])
            {
                if (framePause++ == frameLimit)
                {
                    sendCode(MOVE_LEFT, PAUSE, PAUSE, PAUSE);
                    framePause = 0;
                }
            }

            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            IOutputArray hierarchy = null;
            CvInvoke.FindContours(imgThresh, contours, hierarchy, RetrType.External, ChainApproxMethod.ChainApproxSimple);
            double largest_area = 30;
            int largest_contour_index = 0;

            foundBall = false;
            int count = contours.Size;
            for (int i = 0; i < count; i++)
            {
                double a = CvInvoke.ContourArea(contours[i], false);  //  Find the area of contour
                if (a > largest_area)
                {
                    largest_area = a;
                    largest_contour_index = i;                //Store the index of largest contour
                    foundBall = true;
                }
            }

            if (trackBall && foundBall)
            {
                middleX = 0;
                middleY = 0;
                maxDist = 0;
                double newDist;
                int x1, x2, y1, y2;
                int size = contours[largest_contour_index].Size;
                for (int i = 0; i < size; i++)
                {
                    middleX += contours[largest_contour_index][i].X;
                    middleY += contours[largest_contour_index][i].Y;

                }
                for (int i = 0; i < size / 2; i++)
                {
                    x1 = contours[largest_contour_index][i].X;
                    x2 = contours[largest_contour_index][i + size / 2].X;
                    y1 = contours[largest_contour_index][i].Y;
                    y2 = contours[largest_contour_index][i + size / 2].Y;
                    newDist = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
                    if (maxDist < newDist)
                    {
                        maxDist = newDist;
                    }
                }

                middleX = middleX / contours[largest_contour_index].Size;
                middleY = middleY / contours[largest_contour_index].Size;

                CvInvoke.Circle(currentFrame, new Point((int)middleX, (int)middleY), 3, new MCvScalar(0, 255, 0), -1);

                CvInvoke.Circle(currentFrame, new Point((int)middleX, (int)middleY), (int)maxDist / 2, new MCvScalar(0, 255, 0), 2);

                boxBallPosition.AppendText("maxDist = " + maxDist + "   largest Area: " + largest_area);
                boxBallPosition.ScrollToCaret();

                if (framePause++ == frameLimit)
                {
                    rotateHand(middleX, middleY);
                    framePause = 0;
                }
            }

            CvInvoke.DrawContours(imgThresh, contours, largest_contour_index, new MCvScalar(255, 0, 0));
            CvInvoke.DrawContours(currentFrame, contours, largest_contour_index, new MCvScalar(255, 0, 0));

            if (boxChangeImg.Checked)
            {
                imgBoxFace.Image = imgThresh;

            }
            else
            {
                imgBoxFace.Image = currentFrame;

            }


        }

        private int getHue()
        {
            if (boxManualHue.Checked)
            {
                return (int)boxHue.Value;
            }
            else
            {
                if (blueBallMode)
                {
                    return hueBlue;

                }
                else
                {
                    return hueRed;
                }
            }
        }

        #endregion

        #endregion

        #region SerialPort

        SerialPort port = new SerialPort();
        const byte MOVE_LEFT = '0' - 0;
        const byte MOVE_DOWN = '0' - 0;
        const byte PAUSE = '1' - 0;
        const byte MOVE_RIGHT = '2' - 0;
        const byte MOVE_UP = '2' - 0;
        const byte RESET = '3' - 0;
        const byte RESET_BALL = '4' - 0;

        bool[][] maxedOut = new bool[5][];

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;             //incercam sa evitam dublu apel

            if (!comboBoxCOM.Enabled)
            {                 //este conectat -> exc. deconectare

                //trecem - vizual - in starea deconectat
                port.Close();
                btnConnect.Text = "Connect";
                comboBoxCOM.Enabled = true;
            }
            else
            {
                try
                {
                    port = new SerialPort((string)comboBoxCOM.SelectedItem, 9600);
                    port.Open();
                    port.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
                    btnConnect.Text = "Disconnect";
                    comboBoxCOM.Enabled = false;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error at port opening", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            btnConnect.Enabled = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPorts();

        }

        private void LoadPorts()
        {
            comboBoxCOM.DataSource = SerialPort.GetPortNames();
        }

        void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            //acest artificiu este necesar pt. cross thread call - o prostie nu-ti bate capul cu el
            if (this.InvokeRequired)
            {
                Invoke(new MethodInvoker(() => {
                    process(port.ReadLine());
                }));
            }
            else
            {
                process(port.ReadLine());
            }

        }

        private void process(string data)
        {
            //MessageBox.Show("Am primit ceva pe seriala");

            switch (data)
            {
                case "a":
                    maxedOut[0][0] = true;
                    break;
                case "b":
                    maxedOut[0][1] = true;
                    break;
                case "c":
                    maxedOut[1][0] = true;
                    break;
                case "d":
                    maxedOut[1][1] = true;
                    break;
                case "e":
                    maxedOut[2][0] = true;
                    break;
                case "f":
                    maxedOut[2][1] = true;
                    break;
                case "g":
                    maxedOut[3][0] = true;
                    break;
                case "h":
                    maxedOut[3][1] = true;
                    break;
                default:
                    break;
            }

        }

        private void rotateHand(int x, int y)
        {
            int centerX = 320;
            int centerY = 180;
            int error = 50;
            int errorX = (x - centerX);
            int errorY = (y - centerY);

            if (Math.Abs(errorX) < error)
            {
                if (Math.Abs(errorY) < error)
                {
                    Console.WriteLine("you rock");
                }
                else
                {
                    if (errorY > 0)
                    {
                        sendCode(PAUSE, PAUSE, MOVE_DOWN, PAUSE);
                    }
                    else
                    {
                        sendCode(PAUSE, PAUSE, MOVE_UP, PAUSE);

                    }
                }

            }
            else
            {
                if (Math.Abs(errorY) < error)
                {
                    if (errorX > 0)
                    {
                        sendCode(MOVE_RIGHT, PAUSE, PAUSE, PAUSE);
                    }
                    else
                    {
                        sendCode(MOVE_LEFT, PAUSE, PAUSE, PAUSE);

                    }
                }
                else
                {
                    if (errorY > 0)
                    {
                        if (errorX > 0)
                        {
                            sendCode(MOVE_RIGHT, PAUSE, MOVE_DOWN, PAUSE);
                        }
                        else
                        {
                            sendCode(MOVE_LEFT, PAUSE, MOVE_DOWN, PAUSE);
                        }
                    }
                    else
                    {
                        if (errorX > 0)
                        {
                            sendCode(MOVE_RIGHT, PAUSE, MOVE_UP, PAUSE);
                        }
                        else
                        {
                            sendCode(MOVE_LEFT, PAUSE, MOVE_UP, PAUSE);
                        }
                    }
                }
            }

        }

        private void sendCode(byte servo1, byte servo2, byte servo3, byte servo4)
        {
            Console.WriteLine("send code");
            Console.WriteLine(port.IsOpen + " aici");
            if (port.IsOpen)
            {
                byte[] command = new byte[8];
                command[0] = 'A' - 0;
                command[1] = servo1;
                command[2] = 'B' - 0;
                command[3] = servo2;
                command[4] = 'C' - 0;
                command[5] = servo3;
                command[6] = 'D' - 0;
                command[7] = servo4;

                port.Write(command, 0, 8);
            }

        }

        private void resetMaxedOut()
        {
            for (int i = 0; i < 5; i++)
            {
                maxedOut[i][0] = false;
                maxedOut[i][1] = false;
            }
        }

        #endregion

        #region Myo
        IChannel myoChannel;
        IHub myoHub;

        #region InitializeMyo

        private void InitializeMyo()
        {
            myoChannel = Channel.Create(ChannelDriver.Create(ChannelBridge.Create(), MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create())));
            myoHub = Hub.Create(myoChannel);

            myoHub.MyoConnected += myoHub_MyoConnected;
            myoHub.MyoDisconnected += myoHub_MyoDisconnected;

            myoChannel.StartListening();
        }

        private void StopMyo()
        {
            myoChannel.StopListening();
            myoChannel.Dispose();

        }

        void myoHub_MyoConnected(object sender, MyoEventArgs e)
        {
            //throw new NotImplementedException();
            MessageBox.Show("Myo connected");
            e.Myo.Vibrate(VibrationType.Long);

            //1. Poses
            e.Myo.Unlock(UnlockType.Hold);
            var pose = HeldPose.Create(e.Myo, Pose.FingersSpread, Pose.DoubleTap, Pose.Fist, Pose.WaveIn, Pose.WaveOut);
            pose.Interval = TimeSpan.FromSeconds(0.5);
            pose.Start();
            pose.Triggered += pose_Triggered;

            //2. Angles and Quaternions
            e.Myo.OrientationDataAcquired += Myo_OrientationDataAcquired;

        }

        void myoHub_MyoDisconnected(object sender, MyoEventArgs e)
        {
            e.Myo.OrientationDataAcquired -= Myo_OrientationDataAcquired;
            MessageBox.Show("Myo disconnected");
        }

        #endregion

        #region Myo event handlers
        void Myo_OrientationDataAcquired(object sender, OrientationDataEventArgs e)
        {
            //Invoke Roll, Pitch, Yaw
            var roll = (e.Roll + 3.14) / (3.14 + 2.0f) * 100;
            var pitch = (e.Pitch + 3.14) / (3.14 + 2.0f) * 100;
            var yaw = (e.Yaw + 3.14) / (3.14 + 2.0f) * 100;
            string data = "Roll:  " + roll.ToString() + Environment.NewLine + "Pitch:  " + pitch.ToString() + Environment.NewLine + "Yaw:  " + yaw.ToString() + Environment.NewLine;
            InvokeData(data);
        }
        void pose_Triggered(object sender, PoseEventArgs e)
        {
            InvokeData(e.Pose.ToString());
        }

        private void InvokeData(string data)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(InvokeData), new object[] { data });
                return;
            }
            boxDebugMyo.Text = "";
            boxDebugMyo.AppendText(data + Environment.NewLine);
        }
        #endregion

        #endregion

        #region Speech
        private DataRecognitionClient dataClient;

        private MicrophoneRecognitionClient micClient = null;

        private const string DefaultSubscriptionKeyPromptMessage = "Paste your subscription key here to start";

        private SpeechRecognitionMode speechMode = SpeechRecognitionMode.LongDictation;

        private string DefaultLocale = "en-US";

        private SpeechRecognitionMode Mode = SpeechRecognitionMode.LongDictation;

        private string subscriptionKey = "";

        private string authenticationUri = "";

        NAudio.Wave.WaveFileWriter waveWriter = null;
        NAudio.Wave.DirectSoundOut waveOut = null;
        NAudio.Wave.WaveIn sourceStream = null;

        private void btnSpeech_Click(object sender, EventArgs e)
        {
            if (this.checkSubscriptionKey())
            {
                this.btnSpeech.Enabled = false;

                if (this.micClient == null)
                {
                    this.CreateMicrophoneRecoClient();
                }

                this.micClient.StartMicAndRecognition();

                //if (identify)
                //{
                    string fileName = "C:\\Users\\Mac\\Desktop\\check.wav";

                    int deviceNumber = 0;

                    sourceStream = new NAudio.Wave.WaveIn();

                    sourceStream.DeviceNumber = deviceNumber;

                    sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);

                    sourceStream.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(sourceStream_DataAvailable);
                    waveWriter = new NAudio.Wave.WaveFileWriter(fileName, sourceStream.WaveFormat);

                    sourceStream.StartRecording();
                //}
                
            }
        }
        private void sourceStream_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            if (waveWriter == null) return;

            waveWriter.WriteData(e.Buffer, 0, e.BytesRecorded);
            waveWriter.Flush();
        }

        private void CreateMicrophoneRecoClient()
        {
            this.micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(
                this.Mode,
                this.DefaultLocale,
                this.subscriptionKey);
            this.micClient.AuthenticationUri = this.authenticationUri;

            // Event handlers for speech recognition results
            this.micClient.OnMicrophoneStatus += this.OnMicrophoneStatus;
            this.micClient.OnPartialResponseReceived += this.OnPartialResponseReceivedHandler;
            this.micClient.OnResponseReceived += this.OnMicDictationResponseReceivedHandler;

            this.micClient.OnConversationError += this.OnConversationErrorHandler;
        }

        private bool checkSubscriptionKey()
        {
            if (this.subscriptionKey.Length == 0)
            {
                if (boxApiKey.TextLength == 0)
                {
                    MessageBox.Show(DefaultSubscriptionKeyPromptMessage);
                    return false;
                }
                else
                {
                    this.subscriptionKey = boxApiKey.Text;
                    return true;
                }
            }
            return true;
        }

        private void btnUpdateKey_Click(object sender, EventArgs e)
        {
            this.subscriptionKey = boxApiKey.Text;

        }

        private void WriteLine(string format, params object[] args)
        {
            var formattedStr = string.Format(format, args);

            if (boxSpeech.InvokeRequired)
            {
                boxSpeech.Invoke((MethodInvoker)delegate
                {
                    boxSpeech.AppendText(formattedStr);
                });
            }
            else
            {
                boxSpeech.AppendText(formattedStr);
            };

        }

        private void OnMicrophoneStatus(object sender, MicrophoneEventArgs e)
        {
            WriteLine("--- Microphone status change received by OnMicrophoneStatus() ---");
            WriteLine("********* Microphone status: {0} *********", e.Recording);
            if (e.Recording)
            {
                WriteLine("Please start speaking.");
            }
            WriteLine("\n");
        }
        private void OnPartialResponseReceivedHandler(object sender, PartialSpeechResponseEventArgs e)
        {
            this.WriteLine("--- Partial result received by OnPartialResponseReceivedHandler() ---");
            this.WriteLine("{0}", e.PartialResult);
            this.WriteLine("\n");
        }
        private void OnMicDictationResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {

            this.WriteLine("--- OnMicDictationResponseReceivedHandler ---");
            if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.EndOfDictation ||
                e.PhraseResponse.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout)
            {
                // we got the final result, so it we can end the mic reco.  No need to do this
                // for dataReco, since we already called endAudio() on it as soon as we were done
                // sending all the data.
                this.micClient.EndMicAndRecognition();

                if (btnSpeech.InvokeRequired)
                {
                    boxSpeech.Invoke((MethodInvoker)delegate
                    {
                        btnSpeech.Enabled = true;
                    });
                }
                else
                {
                    btnSpeech.Enabled = true;
                };
            }

            this.WriteResponseResult(e);

            if (e.PhraseResponse.Results.Length > 0)
            {
                this.checkSpeechCommand(e.PhraseResponse.Results[0].DisplayText);
            }

        }

        private void WriteResponseResult(SpeechResponseEventArgs e)
        {
            if (e.PhraseResponse.Results.Length == 0)
            {
                this.WriteLine("No phrase response is available.");
            }
            else
            {
                this.WriteLine("********* Final n-BEST Results *********");
                for (int i = 0; i < e.PhraseResponse.Results.Length; i++)
                {
                    this.WriteLine(
                        "[{0}] Confidence={1}, Text=\"{2}\"",
                        i,
                        e.PhraseResponse.Results[i].Confidence,
                        e.PhraseResponse.Results[i].DisplayText);
                }

                this.WriteLine("\n");
            }
        }
        private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
            if (btnSpeech.InvokeRequired)
            {
                boxSpeech.Invoke((MethodInvoker)delegate
                {
                    btnSpeech.Enabled = true;
                });
            }
            else
            {
                btnSpeech.Enabled = true;
            };
            this.WriteLine("--- Error received by OnConversationErrorHandler() ---");
            this.WriteLine("Error code: {0}", e.SpeechErrorCode.ToString());
            this.WriteLine("Error text: {0}", e.SpeechErrorText);
            this.WriteLine("\n");
        }

        private void btnStopSpeech_Click(object sender, EventArgs e)
        {
            //this.micClient.EndMicAndRecognition();
            //this.micClient = null;
            boxSpeech.Text = "";
            btnSpeech.Enabled = true;
            
            
                if (waveOut != null)
                {
                    waveOut.Stop();
                    waveOut.Dispose();
                    waveOut = null;
                }
                if (sourceStream != null)
                {
                    sourceStream.StopRecording();
                    sourceStream.Dispose();
                    sourceStream = null;
                }
                if (waveWriter != null)
                {
                    waveWriter.Dispose();
                    waveWriter = null;
                }

                string _selectedFile = "C:\\Users\\Mac\\Desktop\\check.wav";

            if (identify)
            {
                identifySpeaker(_selectedFile);
            }
            

        }
        #endregion

        #region Speech Commands
        private void checkSpeechCommand(string command)
        {
            switch (command)
            {
                case "Say hello to the jury.":
                    writeCommand("Say hello to the jury");
                    TextToSpeech.Speak("Hello everyone");
                    identify = false;

                    break;
                case "Pick up the blue ball.":
                    blueBallMode = true;
                    identify = false;

                    writeCommand("Pick up the blue ball");
                    initialiseCaptureBall();
                    break;
                case "Pick up the red ball.":
                    blueBallMode = false;
                    identify = false;

                    writeCommand("Pick up the red ball");
                    initialiseCaptureBall();
                    break;
                case "Give me the ball.":
                    identify = false;

                    writeCommand("Give me the ball");
                    break;
                case "Give the ball to Andrei.":
                    identify = false;

                    writeCommand("Give Andrei the ball");

                    break;
                case "Give the ball to Andre.":
                    identify = false;

                    writeCommand("Give Andrei the ball");

                    break;
                case "Give the ball to Vlad.":
                    identify = false;

                    writeCommand("Give Vlad the ball");

                    break;
                case "Drop the ball.":
                    identify = false;

                    writeCommand("Drop the ball");

                    break;

                case "Play Shakira.":
                    identify = false;

                    writeCommand("Play Shakira");
                    break;

                case "Who am I?":
                    writeCommand("Who am I?");
                    identify = true;
                    break;

                default:
                    identify = false;

                    writeCommand("Command not recognised");
                    break;
            }
        }
        bool identify = false;
        private async void identifySpeaker(string _selectedFile)
        {
            SpeakerIdentificationServiceClient _serviceClient;
            OperationLocation processPollingLocation;

            _serviceClient = new SpeakerIdentificationServiceClient("e5404f463d1242ad8ce61c5422afc4bf");


            Profile[] allProfiles = await _serviceClient.GetProfilesAsync();
            Guid[] testProfileIds = new Guid[allProfiles.Length];
            for (int i = 0; i < testProfileIds.Length; i++)
            {
                testProfileIds[i] = allProfiles[i].ProfileId;
            }
            using (Stream audioStream = File.OpenRead(_selectedFile))
            {
                _selectedFile = "";
                processPollingLocation = await _serviceClient.IdentifyAsync(audioStream, testProfileIds, true);
            }

            IdentificationOperation identificationResponse = null;
            int numOfRetries = 10;
            TimeSpan timeBetweenRetries = TimeSpan.FromSeconds(5.0);
            while (numOfRetries > 0)
            {
                await Task.Delay(timeBetweenRetries);
                identificationResponse = await _serviceClient.CheckIdentificationStatusAsync(processPollingLocation);

                if (identificationResponse.Status == Status.Succeeded)
                {
                    writeUser("User: " + getUser(identificationResponse.ProcessingResult.IdentifiedProfileId.ToString()));
                    break;
                }
                else if (identificationResponse.Status == Status.Failed)
                {
                    writeUser("User: unknown");
                    break;
                }
                numOfRetries--;
            }
            if (numOfRetries <= 0)
            {
                writeUser("User: unknown");
            }



        }
        private string getUser(string id)
        {
            if (id.Equals("33d3a04c-88a2-4dfd-bb96-772e73ed49f9"))
            {
                return "Andrei";
            }
            else
            {
                return "unknown";
            }
        }
        private void writeCommand(string command)
        {
            Invoke(new MethodInvoker(() =>
            {
                boxCommand.Text = command;
            }));
        }

        private void writeUser(string user)
        {
            Invoke(new MethodInvoker(() =>
            {
                labelUser.Text = user;
            }));
        }
        #endregion

        #region ManualMode

        private void btnWeb_Click(object sender, EventArgs e)
        {
            frmWeb MM = new frmWeb(this);
            MM.Show();
            btnWeb.Enabled = false;
        }

        private void btnWeather_Click(object sender, EventArgs e)
        {
            WeatherDataServiceFactory obj = WeatherDataServiceFactory.Instance;                     //get instance of weather data service factory

            Location location = new Location();                                                     //create location object
            location.city = "Boston";

            var tmp = obj.GetWeatherDataService(location);
            System.Console.WriteLine("Country: " + tmp.Sys.Country.ToString() + "\nCity: " + tmp.Name.ToString() + "\nTempreture: " + tmp.Main.Temp.ToString() + " (Kelvin)" + "\nWind Speed: " + tmp.Wind.Speed.ToString() + "\nPressure: " + tmp.Main.Pressure.ToString() + " (Kelvin)" + "\nHumidity: " + tmp.Main.Humidity.ToString());
            synthesizer.Speak("The temperature in " + location.city + " is " + KelvinToCelsius((int)tmp.Main.Temp).ToString() + " degrees Celsius");
        }

        private int KelvinToCelsius(int kelvin)
        {
            int celsius = (int)(kelvin - 273);
            return celsius;
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timerFollowFace_Tick(object sender, EventArgs e)
        {

        }

    }


}
