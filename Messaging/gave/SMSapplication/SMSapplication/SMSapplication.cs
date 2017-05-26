/*
 * Created by: Syeda Anila Nusrat. 
 * Date: 1st August 2009
 * Time: 2:54 PM 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Data.OleDb;
using System.Threading;

namespace SMSapplication
{
    public partial class SMSapplication : Form
    {      

        #region Constructor
        public SMSapplication()
        {
            InitializeComponent();
        }
        #endregion

        #region Private Variables
        SerialPort port = new SerialPort();
        clsSMS objclsSMS = new clsSMS();
        ShortMessageCollection objShortMessageCollection = new ShortMessageCollection();
        #endregion

        #region Private Methods

        #region Write StatusBar
        private void WriteStatusBar(string status)
        {
            try
            {
                statusBar1.Text = "Message: " + status;
            }
            catch (Exception ex)
            {
                
            }
        }
        #endregion
        
        #endregion

        #region Private Events

        private void SMSapplication_Load(object sender, EventArgs e)
        {
            try
            {
                #region Display all available COM Ports
                string[] ports = SerialPort.GetPortNames();

                // Add all port names to the combo box:
                foreach (string port in ports)
                {
                    this.cboPortName.Items.Add(port);
                }
                #endregion

                //Remove tab pages
                this.tabSMSapplication.TabPages.Remove(tbSendSMS);
                this.tabSMSapplication.TabPages.Remove(tbReadSMS);
                this.tabSMSapplication.TabPages.Remove(tbDeleteSMS);

                this.btnDisconnect.Enabled = false;
                openFileDialog1.Filter = "Excel (*.xls)|*.xls";
                openFileDialog1.Title = "Save an Excel File";
            }
            catch(Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //Open communication port 
                this.port = objclsSMS.OpenPort(this.cboPortName.Text, Convert.ToInt32(this.cboBaudRate.Text), Convert.ToInt32(this.cboDataBits.Text), Convert.ToInt32(this.txtReadTimeOut.Text), Convert.ToInt32(this.txtWriteTimeOut.Text));

                if (this.port != null)
                {
                    this.gboPortSettings.Enabled = false;

                    //MessageBox.Show("Modem is connected at PORT " + this.cboPortName.Text);
                    this.statusBar1.Text = "Modem is connected at PORT " + this.cboPortName.Text;

                    //Add tab pages
                    this.tabSMSapplication.TabPages.Add(tbSendSMS);
                    this.tabSMSapplication.TabPages.Add(tbReadSMS);
                    this.tabSMSapplication.TabPages.Add(tbDeleteSMS);

                    this.lblConnectionStatus.Text = "Connected at " + this.cboPortName.Text;
                    this.btnDisconnect.Enabled = true;
                }

                else
                {
                    //MessageBox.Show("Invalid port settings");
                    this.statusBar1.Text = "Invalid port settings";
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.gboPortSettings.Enabled = true;
                objclsSMS.ClosePort(this.port);

                //Remove tab pages
                this.tabSMSapplication.TabPages.Remove(tbSendSMS);
                this.tabSMSapplication.TabPages.Remove(tbReadSMS);
                this.tabSMSapplication.TabPages.Remove(tbDeleteSMS);

                this.lblConnectionStatus.Text = "Not Connected";
                this.btnDisconnect.Enabled = false;

            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        private void btnSendSMS_Click(object sender, EventArgs e)
        {

            //.............................................. Send SMS ....................................................
            if (Program.isListSelected == false)
            {
                labelStatusMode.Text = "SINGLE";
                if (txtSIM.Text.Trim().Length > 9 && txtMessage.Text.Trim().Length > 0)
                {
                    try
                    {

                        if (objclsSMS.sendMsg(this.port, this.txtSIM.Text, this.txtMessage.Text))
                        {
                            //MessageBox.Show("Message has sent successfully");
                            this.statusBar1.Text = "Message has sent successfully";
                        }
                        else
                        {
                            //MessageBox.Show("Failed to send message");
                            this.statusBar1.Text = "Failed to send message";
                        }

                    }
                    catch (Exception ex)
                    {
                        ErrorLog(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Empty Fields Found");
                }
            }
            else
            {
                labelStatusMode.Text = "GROUP";
                if (txtMessage.Text.Length > 0)
                {
                    int i;
                    int length = Program.PhoneNumberList.Length;
                    for (i = 0; i < length - 1; i++)
                    {
                        this.txtSIM.Text = Program.PhoneNumberList[i].ToString();
                        labelStatus.Text = (i+1).ToString();
                        labelStatusTotal.Text = (length - i-1).ToString();
                        //MessageBox.Show(Program.PhoneNumberList[i].ToString());
                        try
                        {
                            if (objclsSMS.sendMsg(this.port, this.txtSIM.Text, this.txtMessage.Text))
                            {
                                //MessageBox.Show("Message has sent successfully");
                                this.statusBar1.Text = "Message has sent successfully";
                            }
                            else
                            {
                                //MessageBox.Show("Failed to send message");
                                this.statusBar1.Text = "Failed to send message";
                            }

                        }
                        catch (Exception ex)
                        {
                            ErrorLog(ex.Message);
                        }

                        Thread.Sleep(5000);
                    }

                    Program.isListSelected = false;
                    if (i == length-1)
                    {
                        MessageBox.Show("All Messages are successfully");
                        labelStatusTotal.Text = "0";
                        txtMessage.Clear();
                        txtSIM.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("No Message Found");
                }
            }
        }
        private void btnReadSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //count SMS 
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                if (uCountSMS > 0)
                {
                    lvwMessages.Items.Clear();                  
                    #region Command
                    string strCommand = "AT+CMGL=\"ALL\"";

                    if (this.rbReadAll.Checked)
                    {
                        strCommand = "AT+CMGL=\"ALL\"";
                    }
                    else if (this.rbReadUnRead.Checked)
                    {
                        strCommand = "AT+CMGL=\"REC UNREAD\"";
                    }
                    else if (this.rbReadStoreSent.Checked)
                    {
                        strCommand = "AT+CMGL=\"STO SENT\"";
                    }
                    else if (this.rbReadStoreUnSent.Checked)
                    {
                        strCommand = "AT+CMGL=\"STO UNSENT\"";
                    }
                    #endregion

                    // If SMS exist then read SMS
                    #region Read SMS
                    //.............................................. Read all SMS ....................................................
                    objShortMessageCollection = objclsSMS.ReadSMS(this.port, strCommand);
                    try
                    {
                       
                        foreach (ShortMessage msg in objShortMessageCollection)
                        {
                            ListViewItem item = new ListViewItem(new string[] { msg.Index, msg.Sent, msg.Sender, msg.Message });
                            item.Tag = msg;
                            lvwMessages.Items.Add(item);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    #endregion

                }
                else
                {
                    lvwMessages.Clear();
                    //MessageBox.Show("There is no message in SIM");
                    this.statusBar1.Text = "There is no message in SIM";
                    
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }
        private void btnDeleteSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //Count SMS 
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                if (uCountSMS > 0)
                {
                    DialogResult dr = MessageBox.Show("Are u sure u want to delete the SMS?", "Delete confirmation", MessageBoxButtons.YesNo);

                    if (dr.ToString() == "Yes")
                    {
                        #region Delete SMS

                        if (this.rbDeleteAllSMS.Checked)
                        {                           
                            //...............................................Delete all SMS ....................................................

                            #region Delete all SMS
                            string strCommand = "AT+CMGD=1,4";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                //MessageBox.Show("Messages has deleted successfuly ");
                                this.statusBar1.Text = "Messages has deleted successfuly";
                            }
                            else
                            {
                                //MessageBox.Show("Failed to delete messages ");
                                this.statusBar1.Text = "Failed to delete messages";
                            }
                            #endregion
                            
                        }
                        else if (this.rbDeleteReadSMS.Checked)
                        {                          
                            //...............................................Delete Read SMS ....................................................

                            #region Delete Read SMS
                            string strCommand = "AT+CMGD=1,3";
                            if (objclsSMS.DeleteMsg(this.port, strCommand))
                            {
                                //MessageBox.Show("Messages has deleted successfuly");
                                this.statusBar1.Text = "Messages has deleted successfuly";
                            }
                            else
                            {
                                //MessageBox.Show("Failed to delete messages ");
                                this.statusBar1.Text = "Failed to delete messages";
                            }
                            #endregion

                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }

        }
        private void btnCountSMS_Click(object sender, EventArgs e)
        {
            try
            {
                //Count SMS
                int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                this.txtCountSMS.Text = uCountSMS.ToString();
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message);
            }
        }

        #endregion

        #region Error Log
        public void ErrorLog(string Message)
        {
            StreamWriter sw = null;

            try
            {
                WriteStatusBar(Message);

                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                //string sPathName = @"E:\";
                string sPathName = @"SMSapplicationErrorLog_";

                string sYear = DateTime.Now.Year.ToString();
                string sMonth = DateTime.Now.Month.ToString();
                string sDay = DateTime.Now.Day.ToString();

                string sErrorTime = sDay + "-" + sMonth + "-" + sYear;

                sw = new StreamWriter(sPathName + sErrorTime + ".txt", true);

                sw.WriteLine(sLogFormat + Message);
                sw.Flush();

            }
            catch (Exception ex)
            {
                //ErrorLog(ex.ToString());
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw.Close();
                }
            }
            
        }
        #endregion 

        private void buttonFetch_Click(object sender, EventArgs e)
        {
            txtSIM.Clear();
            FetchExcel obj = new FetchExcel();
            obj.Show();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName.Length > 0)
            {
                textBoxPath.Text = openFileDialog1.FileName;
                OleDbConnection connection;
                OleDbCommand command;
                try
                {
                    string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxPath.Text + " ;Extended Properties=\"Excel 8.0;";
                    connection = new OleDbConnection(connectionString);
                    connection.Open();


                    try
                    {
                        //count SMS 
                        int uCountSMS = objclsSMS.CountSMSmessages(this.port);
                        if (uCountSMS > 0)
                        {                                                       
                            string strCommand = "AT+CMGL=\"ALL\"";  
                            objShortMessageCollection = objclsSMS.ReadSMS(this.port, strCommand);
                            try
                            {
                                foreach (ShortMessage msg in objShortMessageCollection)
                                {
                                    //ListViewItem item = new ListViewItem(new string[] { msg.Index, msg.Sent, msg.Sender, msg.Message });
                                    //item.Tag = msg;
                                    //lvwMessages.Items.Add(item);
                                    command = new OleDbCommand("insert into [Sheet1$] values ('" + msg.Sent.ToString() + "','" + msg.Sender.ToString() + "','" + msg.Message.ToString() + "' )", connection);
                                    command.ExecuteNonQuery();
                                }
                                this.statusBar1.Text = "All Messages Exported";

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {                            
                            this.statusBar1.Text = "There is no message in SIM";
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog(ex.Message);
                    }

                    connection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                MessageBox.Show("No File Selected");
            }

        }
    
    }
}