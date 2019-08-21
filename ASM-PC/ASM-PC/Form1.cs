using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ASM_PC
{
    public partial class Form1 : Form
    {
        Connector ct=null;
        string web;

        public Form1()
        {
            InitializeComponent();
            //input your web server url
            web = "http://~/InsertServerInfo.php?";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ct == null)
            {
                ct = new Connector();
                //link event handler
                ct.connected += Connected;
                ct.imageArrived += ImageArrived; 
                ct.disconnected += Disconnected;
                ct.ConnectionStart();
            }
        }

        /// <summary>
        /// Arriving image event handler 
        /// </summary>
        private void ImageArrived()
        {
            if (ct != null)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    try
                    {
                        pictureBox1.Image = ct.AndroidScreenImage;
                    }catch(NullReferenceException nre)
                    {
                        
                    }
                }));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (ct != null)
            {
                ct.MirroringEnd();
            }
        }

        /// <summary>
        /// connected event handler 
        /// </summary>
        private void Connected()
        {
            ct.MirroringStart();
        }

        /// <summary>
        /// disconnected event handler 
        /// </summary>
        private void Disconnected()
        {
            pictureBox1.Image = null;
            ct = null;
            MessageBox.Show("Disconnected");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string code = textBox1.Text;
            if (code == "")
            {
                MessageBox.Show("Input your code");
                return;
            }
                
            string port = "9876";

            //create get url
            string temp= web + "code="+code+"&port=" + port;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(temp);
            request.Method = "GET";
            request.GetResponse();
            MessageBox.Show("uploaded");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ct != null)
            {
                ct.dispose();
            }
        }
    }
}
