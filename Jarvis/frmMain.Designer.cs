namespace Jarvis
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelMessage = new System.Windows.Forms.Label();
            this.btnTrain = new System.Windows.Forms.Button();
            this.btnSpeech = new System.Windows.Forms.Button();
            this.btnFaceRecognition = new System.Windows.Forms.Button();
            this.btnBallRecognition = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.imgBoxFace = new Emgu.CV.UI.ImageBox();
            this.boxSpeech = new System.Windows.Forms.TextBox();
            this.boxApiKey = new System.Windows.Forms.TextBox();
            this.boxFacePosition = new System.Windows.Forms.TextBox();
            this.boxBallPosition = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.comboBoxCOM = new System.Windows.Forms.ComboBox();
            this.boxDebugMyo = new System.Windows.Forms.RichTextBox();
            this.btnUpdateKey = new System.Windows.Forms.Button();
            this.btnStopSpeech = new System.Windows.Forms.Button();
            this.boxCommand = new System.Windows.Forms.TextBox();
            this.labelCommand = new System.Windows.Forms.Label();
            this.btnWeb = new System.Windows.Forms.Button();
            this.btnWeather = new System.Windows.Forms.Button();
            this.timerFollowFace = new System.Windows.Forms.Timer(this.components);
            this.boxChangeImg = new System.Windows.Forms.CheckBox();
            this.boxManualHue = new System.Windows.Forms.CheckBox();
            this.boxHue = new System.Windows.Forms.NumericUpDown();
            this.labelUser = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxFace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxHue)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMessage
            // 
            this.labelMessage.AutoSize = true;
            this.labelMessage.Location = new System.Drawing.Point(21, 505);
            this.labelMessage.Name = "labelMessage";
            this.labelMessage.Size = new System.Drawing.Size(53, 13);
            this.labelMessage.TabIndex = 0;
            this.labelMessage.Text = "Message:";
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(20, 479);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(75, 23);
            this.btnTrain.TabIndex = 1;
            this.btnTrain.Text = "Train Faces";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // btnSpeech
            // 
            this.btnSpeech.Location = new System.Drawing.Point(830, 479);
            this.btnSpeech.Name = "btnSpeech";
            this.btnSpeech.Size = new System.Drawing.Size(88, 23);
            this.btnSpeech.TabIndex = 2;
            this.btnSpeech.Text = "Start Speaking";
            this.btnSpeech.UseVisualStyleBackColor = true;
            this.btnSpeech.Click += new System.EventHandler(this.btnSpeech_Click);
            // 
            // btnFaceRecognition
            // 
            this.btnFaceRecognition.Location = new System.Drawing.Point(101, 479);
            this.btnFaceRecognition.Name = "btnFaceRecognition";
            this.btnFaceRecognition.Size = new System.Drawing.Size(130, 23);
            this.btnFaceRecognition.TabIndex = 3;
            this.btnFaceRecognition.Text = "Start Face Recognition";
            this.btnFaceRecognition.UseVisualStyleBackColor = true;
            this.btnFaceRecognition.Click += new System.EventHandler(this.btnFaceRecognition_Click);
            // 
            // btnBallRecognition
            // 
            this.btnBallRecognition.Location = new System.Drawing.Point(246, 479);
            this.btnBallRecognition.Name = "btnBallRecognition";
            this.btnBallRecognition.Size = new System.Drawing.Size(123, 23);
            this.btnBallRecognition.TabIndex = 4;
            this.btnBallRecognition.Text = "Start Ball Recognition";
            this.btnBallRecognition.UseVisualStyleBackColor = true;
            this.btnBallRecognition.Click += new System.EventHandler(this.btnBallRecognition_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(18, 540);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // imgBoxFace
            // 
            this.imgBoxFace.Location = new System.Drawing.Point(18, 23);
            this.imgBoxFace.Name = "imgBoxFace";
            this.imgBoxFace.Size = new System.Drawing.Size(640, 360);
            this.imgBoxFace.TabIndex = 2;
            this.imgBoxFace.TabStop = false;
            this.imgBoxFace.Click += new System.EventHandler(this.imgBoxFace_Click);
            // 
            // boxSpeech
            // 
            this.boxSpeech.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.boxSpeech.Location = new System.Drawing.Point(691, 23);
            this.boxSpeech.Multiline = true;
            this.boxSpeech.Name = "boxSpeech";
            this.boxSpeech.Size = new System.Drawing.Size(225, 360);
            this.boxSpeech.TabIndex = 6;
            // 
            // boxApiKey
            // 
            this.boxApiKey.Location = new System.Drawing.Point(564, 481);
            this.boxApiKey.Name = "boxApiKey";
            this.boxApiKey.Size = new System.Drawing.Size(244, 20);
            this.boxApiKey.TabIndex = 7;
            // 
            // boxFacePosition
            // 
            this.boxFacePosition.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.boxFacePosition.Location = new System.Drawing.Point(18, 399);
            this.boxFacePosition.Multiline = true;
            this.boxFacePosition.Name = "boxFacePosition";
            this.boxFacePosition.Size = new System.Drawing.Size(262, 60);
            this.boxFacePosition.TabIndex = 8;
            // 
            // boxBallPosition
            // 
            this.boxBallPosition.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.boxBallPosition.Location = new System.Drawing.Point(320, 399);
            this.boxBallPosition.Multiline = true;
            this.boxBallPosition.Name = "boxBallPosition";
            this.boxBallPosition.Size = new System.Drawing.Size(262, 60);
            this.boxBallPosition.TabIndex = 9;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(101, 540);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // comboBoxCOM
            // 
            this.comboBoxCOM.FormattingEnabled = true;
            this.comboBoxCOM.Location = new System.Drawing.Point(20, 580);
            this.comboBoxCOM.Name = "comboBoxCOM";
            this.comboBoxCOM.Size = new System.Drawing.Size(156, 21);
            this.comboBoxCOM.TabIndex = 11;
            // 
            // boxDebugMyo
            // 
            this.boxDebugMyo.Location = new System.Drawing.Point(195, 540);
            this.boxDebugMyo.Name = "boxDebugMyo";
            this.boxDebugMyo.Size = new System.Drawing.Size(247, 64);
            this.boxDebugMyo.TabIndex = 12;
            this.boxDebugMyo.Text = "";
            // 
            // btnUpdateKey
            // 
            this.btnUpdateKey.Location = new System.Drawing.Point(652, 507);
            this.btnUpdateKey.Name = "btnUpdateKey";
            this.btnUpdateKey.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateKey.TabIndex = 13;
            this.btnUpdateKey.Text = "UpdateKey";
            this.btnUpdateKey.UseVisualStyleBackColor = true;
            this.btnUpdateKey.Click += new System.EventHandler(this.btnUpdateKey_Click);
            // 
            // btnStopSpeech
            // 
            this.btnStopSpeech.Location = new System.Drawing.Point(830, 508);
            this.btnStopSpeech.Name = "btnStopSpeech";
            this.btnStopSpeech.Size = new System.Drawing.Size(75, 23);
            this.btnStopSpeech.TabIndex = 14;
            this.btnStopSpeech.Text = "Stop recognition";
            this.btnStopSpeech.UseVisualStyleBackColor = true;
            this.btnStopSpeech.Click += new System.EventHandler(this.btnStopSpeech_Click);
            // 
            // boxCommand
            // 
            this.boxCommand.Location = new System.Drawing.Point(676, 542);
            this.boxCommand.Name = "boxCommand";
            this.boxCommand.Size = new System.Drawing.Size(244, 20);
            this.boxCommand.TabIndex = 15;
            // 
            // labelCommand
            // 
            this.labelCommand.AutoSize = true;
            this.labelCommand.Location = new System.Drawing.Point(544, 545);
            this.labelCommand.Name = "labelCommand";
            this.labelCommand.Size = new System.Drawing.Size(114, 13);
            this.labelCommand.TabIndex = 16;
            this.labelCommand.Text = "Recognised Command";
            // 
            // btnWeb
            // 
            this.btnWeb.Location = new System.Drawing.Point(652, 580);
            this.btnWeb.Name = "btnWeb";
            this.btnWeb.Size = new System.Drawing.Size(99, 23);
            this.btnWeb.TabIndex = 18;
            this.btnWeb.Text = "Web Browser";
            this.btnWeb.UseVisualStyleBackColor = true;
            this.btnWeb.Click += new System.EventHandler(this.btnWeb_Click);
            // 
            // btnWeather
            // 
            this.btnWeather.Location = new System.Drawing.Point(510, 580);
            this.btnWeather.Name = "btnWeather";
            this.btnWeather.Size = new System.Drawing.Size(99, 23);
            this.btnWeather.TabIndex = 19;
            this.btnWeather.Text = "Get Weather";
            this.btnWeather.UseVisualStyleBackColor = true;
            this.btnWeather.Click += new System.EventHandler(this.btnWeather_Click);
            // 
            // timerFollowFace
            // 
            this.timerFollowFace.Tick += new System.EventHandler(this.timerFollowFace_Tick);
            // 
            // boxChangeImg
            // 
            this.boxChangeImg.AutoSize = true;
            this.boxChangeImg.Location = new System.Drawing.Point(600, 442);
            this.boxChangeImg.Name = "boxChangeImg";
            this.boxChangeImg.Size = new System.Drawing.Size(83, 17);
            this.boxChangeImg.TabIndex = 23;
            this.boxChangeImg.Text = "Change Img";
            this.boxChangeImg.UseVisualStyleBackColor = true;
            // 
            // boxManualHue
            // 
            this.boxManualHue.AutoSize = true;
            this.boxManualHue.Location = new System.Drawing.Point(600, 402);
            this.boxManualHue.Name = "boxManualHue";
            this.boxManualHue.Size = new System.Drawing.Size(84, 17);
            this.boxManualHue.TabIndex = 24;
            this.boxManualHue.Text = "Manual Hue";
            this.boxManualHue.UseVisualStyleBackColor = true;
            // 
            // boxHue
            // 
            this.boxHue.Location = new System.Drawing.Point(700, 401);
            this.boxHue.Maximum = new decimal(new int[] {
            170,
            0,
            0,
            0});
            this.boxHue.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.boxHue.Name = "boxHue";
            this.boxHue.Size = new System.Drawing.Size(68, 20);
            this.boxHue.TabIndex = 25;
            this.boxHue.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(802, 399);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(32, 13);
            this.labelUser.TabIndex = 26;
            this.labelUser.Text = "User:";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 613);
            this.Controls.Add(this.labelUser);
            this.Controls.Add(this.boxHue);
            this.Controls.Add(this.boxManualHue);
            this.Controls.Add(this.boxChangeImg);
            this.Controls.Add(this.btnWeather);
            this.Controls.Add(this.btnWeb);
            this.Controls.Add(this.labelCommand);
            this.Controls.Add(this.boxCommand);
            this.Controls.Add(this.btnStopSpeech);
            this.Controls.Add(this.btnUpdateKey);
            this.Controls.Add(this.boxDebugMyo);
            this.Controls.Add(this.comboBoxCOM);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.boxBallPosition);
            this.Controls.Add(this.boxFacePosition);
            this.Controls.Add(this.boxApiKey);
            this.Controls.Add(this.boxSpeech);
            this.Controls.Add(this.imgBoxFace);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.btnBallRecognition);
            this.Controls.Add(this.btnFaceRecognition);
            this.Controls.Add(this.btnSpeech);
            this.Controls.Add(this.btnTrain);
            this.Controls.Add(this.labelMessage);
            this.Name = "frmMain";
            this.Text = "Rocket";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgBoxFace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.boxHue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label labelMessage;
        public System.Windows.Forms.Button btnTrain;
        public System.Windows.Forms.Button btnSpeech;
        public System.Windows.Forms.Button btnFaceRecognition;
        public System.Windows.Forms.Button btnBallRecognition;
        public System.Windows.Forms.Button btnConnect;
        public Emgu.CV.UI.ImageBox imgBoxFace;
        public System.Windows.Forms.TextBox boxSpeech;
        public System.Windows.Forms.TextBox boxApiKey;
        public System.Windows.Forms.TextBox boxFacePosition;
        public System.Windows.Forms.TextBox boxBallPosition;
        public System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox comboBoxCOM;
        private System.Windows.Forms.RichTextBox boxDebugMyo;
        public System.Windows.Forms.Button btnUpdateKey;
        public System.Windows.Forms.Button btnStopSpeech;
        public System.Windows.Forms.TextBox boxCommand;
        public System.Windows.Forms.Label labelCommand;
        public System.Windows.Forms.Button btnWeb;
        public System.Windows.Forms.Button btnWeather;
        private System.Windows.Forms.Timer timerFollowFace;
        private System.Windows.Forms.CheckBox boxChangeImg;
        private System.Windows.Forms.CheckBox boxManualHue;
        private System.Windows.Forms.NumericUpDown boxHue;
        private System.Windows.Forms.Label labelUser;
    }
}

