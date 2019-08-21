using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ASM_PC
{
    public partial class Form1 : Form
    {
        Connector ct=null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ct == null)
            {
                ct = new Connector();
                ct.imageArrived += ImageArrived; //link event handler
                ct.disconnected += Disconnected; //link event handler
            }
            else
            {
                if (!ct.IsConnected)
                {
                    ct = null;
                    ct = new Connector();
                    ct.imageArrived += ImageArrived; //link event handler 
                    ct.disconnected += Disconnected; //link event handler
                    pictureBox1.Image = null;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ct.MirroringStart();
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
        /// disconnected event handler 
        /// </summary>
        private void Disconnected()
        {
            pictureBox1.Image = null;
            ct = null;
        }
    }
}
