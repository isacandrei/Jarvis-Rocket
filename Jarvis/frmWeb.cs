using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jarvis
{
    public partial class frmWeb : Form
    {
        frmMain parent;
        const string youtube = "https://www.youtube.com/";

        public frmWeb(frmMain parent)
        {
            this.parent = parent;
            InitializeComponent();

        }

        private void frmWeb_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.parent.btnWeb.Enabled = true;
        }

        private void frmWeb_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(new Uri(youtube));

            webBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(webBrowser_DocumentCompleted);


            //HtmlElement btnElement = webBrowser1.Document.All.GetElementsByName("btnG")[0];
            //btnElement.InvokeMember("click"); 

        }

        void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser1.Url.OriginalString.Equals(youtube))
            {

                HtmlElement textElement = webBrowser1.Document.GetElementById("masthead-search-term");
                textElement.SetAttribute("value", "shakira");
                SendKeys.Send("{ENTER}");
            }
            if (isSearch(webBrowser1.Url.OriginalString))
            {
                var links = webBrowser1.Document.GetElementsByTagName("a");
                foreach (HtmlElement link in links)
                {
                    if (link.GetAttribute("className") == "yt-uix-tile-link yt-ui-ellipsis yt-ui-ellipsis-2 yt-uix-sessionlink      spf-link ")
                    {
                        link.InvokeMember("click");
                        break;
                    }
                }
            }
        }

        bool isSearch(string url)
        {
            string[] strArr = null;

            strArr = url.Split('?');

            return strArr[0].Equals(youtube + "results");
        }
    }

}
