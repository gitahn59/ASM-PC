﻿using System;
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
           ct = new Connector();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ct != null)
            {
                ct.GetImage();
                pictureBox1.Image = ct.i;
            }
        }
    }
}
