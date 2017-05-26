using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string data = "";

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                serialPort1.Open();
               // serialPort1.WriteLine("AT+CMGF=1");
                serialPort1.WriteLine("AT+CMGR=1");
                
                serialPort1.DiscardInBuffer();
                serialPort1.DiscardOutBuffer();
                //serialPort1.WriteLine("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            data += serialPort1.ReadExisting();
            if (data.Trim() == "RING")
            {
                serialPort1.WriteLine("ATH");
            }
            else
            {
                show();
            }
            serialPort1.DiscardInBuffer();
            serialPort1.DiscardOutBuffer();
            data = "";
        }

        private void show()
        {
            MessageBox.Show(data);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {

            }
            else
            {
                serialPort1.Open();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.NewLine = "\r";
        }

        
    }
}
