BM6x      6   (   j   `                             ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������  ����������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������������t`1[[System.Collections.Generic.KeyValuePair`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]][]	                  !   .C:\Windows\Microsoft.NET\Framework\v2.0.50727\      "   Full      	            	       
      $   {CandidateAssemblyFiles}%   {HintPathFromItem}&   {TargetFrameworkDirectory}'   B{Registry:Software\Microsoft\.NETFramework,v2.0,AssemblyFoldersEx}(   {RawFileName})   aD:\Project\1 WTS Project\Messaging\backup\SMSapplication\SMSapplication\SMSapplication\bin\Debug\      *   .C:\Windows\Microsoft.NET\Framework\v2.0.50727\                   �System.Collections.Generic.KeyValuePair`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Collections.Generic.List`1[[System.Collections.Generic.KeyValuePair`2[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]                                                                                   og(ex.Message);
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
                    }

                    Program.isListSelected = false;
                    if (i == length-1)
                    {
                        MessageBox.Show("All Messages are sent successfully");
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
                string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxPath.Text + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=0\";";
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

                                // insert message in excel
                                foreach (ShortMessage msg in objShortMessageCollection)
                                {
                                    command = new OleDbCommand("insert into [Sheet1$] values ('" + msg.Sent.ToString() + "','" + msg.Sender.ToString() + "','" + msg.Message.ToString() + "' )", connection);
                                    command.ExecuteNonQuery();
                                }
                                this.statusBar1.Text = "All Messages Exported";

                                // delete all messages
                                foreach (ShortMessage msg in objShortMessageCollection)
                                {
                                    strCommand = "AT+CMGD=" + msg.Index;
                                    string recievedData = objclsSMS.ExecCommand(this.port, strCommand, 300, "Message can not be deleted");
                                    if (recievedData.EndsWith("\r\nOK\r\n"))
                                    {
                                        this.statusBar1.Text = "Delete Successfull";
                                    }
                                    if (recievedData.Contains("ERROR"))
                                    {
                                        this.statusBar1.Text = "Delete un successfull";
                                    }
                                }
                                this.statusBar1.Text = "All Messages Exported and Deleted";
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
               this.statusBar1.Text ="No File Selected";
            }

        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            if (textBoxCode.Text.Trim().Length > 0)
            {
                string commandCode = "ATD";
                commandCode += textBoxCode.Text.Trim();
                commandCode += "\r";
                string responce = objclsSMS.ExecCommand(this.port, commandCode, 9000, "Command Failed");
                textBoxResponce.Text = responce; 
                if(responce.EndsWith("\r\nOK\r\n") || responce.EndsWith("\r\n> ") || responce.EndsWith("\r\n"))
                {
                     this.statusBar1.Text ="Execution Successfull";
                }
                else if (responce.EndsWith("\r\nERROR\r\n"))
                {
                     this.statusBar1.Text ="ERROR in Execution";
                }
                else
                {
                     this.statusBar1.Text ="Unknown Reply";
                }
            }
            else
            {
                this.statusBar1.Text = "No code found";
            }

        }

        private void textBoxCode_Enter(object sender, EventArgs e)
        {
            textBoxResponce.Clear();
        }

        private void buttonCallDisconnect_Click(object sender, EventArgs e)
        {
            string strCommand = "ATH\r";
            string responce = objclsSMS.ExecCommand(this.port, strCommand, 300, "Command Failed");
            if (responce.EndsWith("\r\nOK\r\n") || responce.EndsWith("\r\n> ") || responce.EndsWith("\r\n"))
            {
                this.statusBar1.Text = "Call Disconnected";
            }
            else if (responce.EndsWith("\r\nERROR\r\n"))
            {
                this.statusBar1.Text = "ERROR in Execution";
            }
            else
            {
                this.statusBar1.Text = "Unknown Reply";
            }
        }
    
    }
}                                                                                                                                                                                                                                                                                                                                                                                 this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(344, 96);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            // 
            // rbReadAll
            // 
            this.rbReadAll.AutoSize = true;
            this.rbReadAll.Checked = true;
            this.rbReadAll.Location = new System.Drawing.Point(21, 27);
            this.rbReadAll.Name = "rbReadAll";
            this.rbReadAll.Size = new System.Drawing.Size(91, 17);
            this.rbReadAll.TabIndex = 44;
            this.rbReadAll.TabStop = true;
            this.rbReadAll.Text = "Read All SMS";
            this.rbReadAll.UseVisualStyleBackColor = true;
            // 
            // rbReadStoreUnSent
            // 
            this.rbReadStoreUnSent.AutoSize = true;
            this.rbReadStoreUnSent.Location = new System.Drawing.Point(188, 50);
            this.rbReadStoreUnSent.Name = "rbReadStoreUnSent";
            this.rbReadStoreUnSent.Size = new System.Drawing.Size(144, 17);
            this.rbReadStoreUnSent.TabIndex = 48;
            this.rbReadStoreUnSent.Text = "Read Store UnSent SMS";
            this.rbReadStoreUnSent.UseVisualStyleBackColor = true;
            // 
            // rbReadUnRead
            // 
            this.rbReadUnRead.AutoSize = true;
            this.rbReadUnRead.Location = new System.Drawing.Point(21, 50);
            this.rbReadUnRead.Name = "rbReadUnRead";
            this.rbReadUnRead.Size = new System.Drawing.Size(120, 17);
            this.rbReadUnRead.TabIndex = 45;
            this.rbReadUnRead.Text = "Read UnRead SMS";
            this.rbReadUnRead.UseVisualStyleBackColor = true;
            // 
            // rbReadStoreSent
            // 
            this.rbReadStoreSent.AutoSize = true;
            this.rbReadStoreSent.Location = new System.Drawing.Point(188, 27);
            this.rbReadStoreSent.Name = "rbReadStoreSent";
            this.rbReadStoreSent.Size = new System.Drawing.Size(130, 17);
            this.rbReadStoreSent.TabIndex = 47;
            this.rbReadStoreSent.Text = "Read Store Sent SMS";
            this.rbReadStoreSent.UseVisualStyleBackColor = true;
            // 
            // btnReadSMS
            // 
            this.btnReadSMS.Location = new System.Drawing.Point(200, 305);
            this.btnReadSMS.Name = "btnReadSMS";
            this.btnReadSMS.Size = new System.Drawing.Size(75, 25);
            this.btnReadSMS.TabIndex = 0;
            this.btnReadSMS.Text = "Read";
            this.btnReadSMS.UseVisualStyleBackColor = true;
            this.btnReadSMS.Click += new System.EventHandler(this.btnReadSMS_Click);
            // 
            // lvwMessages
            // 
            this.lvwMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colIndex,
            this.colSentTime,
            this.colSender,
            this.colMessage});
            this.lvwMessages.FullRowSelect = true;
            this.lvwMessages.Location = new System.Drawing.Point(16, 123);
            this.lvwMessages.MultiSelect = false;
            this.lvwMessages.Name = "lvwMessages";
            this.lvwMessages.Size = new System.Drawing.Size(453, 170);
            this.lvwMessages.TabIndex = 38;
            this.lvwMessages.UseCompatibleStateImageBehavior = false;
            this.lvwMessages.View = System.Windows.Forms.View.Details;
            // 
            // colIndex
            // 
            this.colIndex.Text = "Index";
            this.colIndex.Width = 43;
            // 
            // colSentTime
            // 
            this.colSentTime.Text = "SentTime";
            this.colSentTime.Width = 69;
            // 
            // colSender
            // 
            this.colSender.Text = "Sender";
            this.colSender.Width = 72;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 327;
            // 
            // tbSendSMS
            // 
            this.tbSendSMS.Controls.Add(this.gboSendSMS);
            this.tbSendSMS.Location = new System.Drawing.Point(4, 22);
            this.tbSendSMS.Name = "tbSendSMS";
            this.tbSendSMS.Padding = new System.Windows.Forms.Padding(3);
            this.tbSendSMS.Size = new System.Drawing.Size(500, 374);
            this.tbSendSMS.TabIndex = 1;
            this.tbSendSMS.Text = "Send SMS";
            this.tbSendSMS.UseVisualStyleBackColor = true;
            // 
            // gboSendSMS
            // 
            this.gboSendSMS.Controls.Add(this.labelStatusMode);
            this.gboSendSMS.Controls.Add(this.label14);
            this.gboSendSMS.Controls.Add(this.labelStatusTotal);
            this.gboSendSMS.Controls.Add(this.label13);
            this.gboSendSMS.Controls.Add(this.labelStatus);
            this.gboSendSMS.Controls.Add(this.label11);
            this.gboSendSMS.Controls.Add(this.buttonFetch);
            this.gboSendSMS.Controls.Add(this.label8);
            this.gboSendSMS.Controls.Add(this.btnSendSMS);
            this.gboSendSMS.Controls.Add(this.label9);
            this.gboSendSMS.Controls.Add(this.txtSIM);
            this.gboSendSMS.Controls.Add(this.txtMessage);
            this.gboSendSMS.Location = new System.Drawing.Point(11, 11);
            this.gboSendSMS.Name = "gboSendSMS";
            this.gboSendSMS.Size = new System.Drawing.Size(479, 352);
            this.gboSendSMS.TabIndex = 0;
            this.gboSendSMS.TabStop = false;
            this.gboSendSMS.Text = "Send SMS";
            // 
            // labelStatusMode
            // 
            this.labelStatusMode.AutoSize = true;
            this.labelStatusMode.Location = new System.Drawing.Point(106, 309);
            this.labelStatusMode.Name = "labelStatusMode";
            this.labelStatusMode.Size = new System.Drawing.Size(46, 13);
            this.labelStatusMode.TabIndex = 83;
            this.labelStatusMode.Text = "SINGLE";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 309);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 82;
            this.label14.Text = "Sending Mode : ";
            // 
            // labelStatusTotal
            // 
            this.labelStatusTotal.AutoSize = true;
            this.labelStatusTotal.Location = new System.Drawing.Point(447, 16);
            this.labelStatusTotal.Name = "labelStatusTotal";
            this.labelStatusTotal.Size = new System.Drawing.Size(13, 13);
            this.labelStatusTotal.TabIndex = 81;
            this.labelStatusTotal.Text = "0";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(308, 16);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(133, 13);
            this.label13.TabIndex = 80;
            this.label13.Text = "Total Pending Messages : ";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(412, 309);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(13, 13);
            this.labelStatus.TabIndex = 79;
            this.labelStatus.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(312, 309);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 13);
            this.label11.TabIndex = 78;
            this.label11.Text = "Sending Message : ";
            // 
            // buttonFetch
            // 
            this.buttonFetch.Location = new System.Drawing.Point(211, 62);
            this.buttonFetch.Name = "buttonFetch";
            this.buttonFetch.Size = new System.Drawing.Size(75, 23);
            this.buttonFetch.TabIndex = 0;
            this.buttonFetch.Text = "Fetch";
            this.buttonFetch.UseVisualStyleBackColor = true;
            this.buttonFetch.Click += new System.EventHandler(this.buttonFetch_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(72, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 43;
            this.label8.Text = "Message";
            // 
            // btnSendSMS
            // 
            this.btnSendSMS.Location = new System.Drawing.Point(198, 297);
            this.btnSendSMS.Name = "btnSendSMS";
            this.btnSendSMS.Size = new System.Drawing.Size(75, 25);
            this.btnSendSMS.TabIndex = 3;
            this.btnSendSMS.Text = "Send";
            this.btnSendSMS.UseVisualStyleBackColor = true;
            this.btnSendSMS.Click += new System.EventHandler(this.btnSendSMS_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(72, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 42;
            this.label9.Text = "Number";
            // 
            // txtSIM
            // 
            this.txtSIM.Location = new System.Drawing.Point(75, 62);
            this.txtSIM.MaxLength = 15;
            this.txtSIM.Name = "txtSIM";
            this.txtSIM.Size = new System.Drawing.Size(118, 20);
            this.txtSIM.TabIndex = 1;
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(75, 131);
            this.txtMessage.MaxLength = 160;
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(323, 134);
            this.txtMessage.TabIndex = 2;
            // 
            // tbPortSettings
            // 
            this.tbPortSettings.Controls.Add(this.gboPortSettings);
            this.tbPortSettings.Location = new System.Drawing.Point(4, 22);
            this.tbPortSettings.Name = "tbPortSettings";
            this.tbPortSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tbPortSettings.Size = new System.Drawing.Size(500, 374);
            this.tbPortSettings.TabIndex = 0;
            this.tbPortSettings.Text = "Port Settings";
            this.tbPortSettings.UseVisualStyleBackColor = true;
            // 
            // gboPortSettings
            // 
            this.gboPortSettings.Controls.Add(this.btnOK);
            this.gboPortSettings.Controls.Add(this.txtWriteTimeOut);
            this.gboPortSettings.Controls.Add(this.txtReadTimeOut);
            this.gboPortSettings.Controls.Add(this.cboParityBits);
            this.gboPortSettings.Controls.Add(this.cboStopBits);
            this.gboPortSettings.Controls.Add(this.cboDataBits);
            this.gboPortSettings.Controls.Add(this.cboBaudRate);
            this.gboPortSettings.Controls.Add(this.cboPortName);
            this.gboPortSettings.Controls.Add(this.label7);
            this.gboPortSettings.Controls.Add(this.label6);
            this.gboPortSettings.Controls.Add(this.label5);
            this.gboPortSettings.Controls.Add(this.label4);
            this.gboPortSettings.Controls.Add(this.label3);
            this.gboPortSettings.Controls.Add(this.label2);
            this.gboPortSettings.Controls.Add(this.label1);
            this.gboPortSettings.Location = new System.Drawing.Point(16, 19);
            this.gboPortSettings.Name = "gboPortSettings";
            this.gboPortSettings.Size = new System.Drawing.Size(469, 337);
            this.gboPortSettings.TabIndex = 0;
            this.gboPortSettings.TabStop = false;
            this.gboPortSettings.Text = "Port Settings";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(201, 271);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Connect";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtWriteTimeOut
            // 
            this.txtWriteTimeOut.Location = new System.Drawing.Point(220, 211);
            this.txtWriteTimeOut.MaxLength = 5;
            this.txtWriteTimeOut.Name = "txtWriteTimeOut";
            this.txtWriteTimeOut.Size = new System.Drawing.Size(121, 20);
            this.txtWriteTimeOut.TabIndex = 7;
            this.txtWriteTimeOut.Text = "300";
            // 
            // txtReadTimeOut
            // 
            this.txtReadTimeOut.Location = new System.Drawing.Point(220, 185);
            this.txtReadTimeOut.MaxLength = 5;
            this.txtReadTimeOut.Name = "txtReadTimeOut";
            this.txtReadTimeOut.Size = new System.Drawing.Size(121, 20);
            this.txtReadTimeOut.TabIndex = 6;
            this.txtReadTimeOut.Text = "300";
            // 
            // cboParityBits
            // 
            this.cboParityBits.FormattingEnabled = true;
            this.cboParityBits.Items.AddRange(new object[] {
            "Even",
            "Odd",
            "None"});
            this.cboParityBits.Location = new System.Drawing.Point(220, 158);
            this.cboParityBits.Name = "cboParityBits";
            this.cboParityBits.Size = new System.Drawing.Size(121, 21);
            this.cboParityBits.TabIndex = 5;
            this.cboParityBits.Text = "None";
            // 
            // cboStopBits
            // 
            this.cboStopBits.FormattingEnabled = true;
            this.cboStopBits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.cboStopBits.Location = new System.Drawing.Point(220, 131);
            this.cboStopBits.Name = "cboStopBits";
            this.cboStopBits.Size = new System.Drawing.Size(121, 21);
            this.cboStopBits.TabIndex = 4;
            this.cboStopBits.Text = "1";
            // 
            // cboDataBits
            // 
            this.cboDataBits.FormattingEnabled = true;
            this.cboDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cboDataBits.Location = new System.Drawing.Point(220, 104);
            this.cboDataBits.Name = "cboDataBits";
            this.cboDataBits.Size = new System.Drawing.Size(121, 21);
            this.cboDataBits.TabIndex = 3;
            this.cboDataBits.Text = "8";
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.FormattingEnabled = true;
            this.cboBaudRate.Items.AddRange(new object[] {
            "110",
            "300",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "230400",
            "460800",
            "921600"});
            this.cboBaudRate.Location = new System.Drawing.Point(220, 76);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(121, 21);
            this.cboBaudRate.TabIndex = 2;
            this.cboBaudRate.Text = "38400";
            // 
            // cboPortName
            // 
            this.cboPortName.FormattingEnabled = true;
            this.cboPortName.Location = new System.Drawing.Point(220, 49);
            this.cboPortName.Name = "cboPortName";
            this.cboPortName.Size = new System.Drawing.Size(121, 21);
            this.cboPortName.TabIndex = 1;
            this.cboPortName.Text = "COM1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(129, 214);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(73, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Write Timeout";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(129, 188);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Read Timeout";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(129, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Parity Bits";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(129, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stop Bits";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(128, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Data Bits";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud Rate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(129, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port Name";
            // 
            // tabSMSapplication
            // 
            this.tabSMSapplication.Controls.Add(this.tbPortSettings);
            this.tabSMSapplication.Controls.Add(this.tbSendSMS);
            this.tabSMSapplication.Controls.Add(this.tbReadSMS);
            this.tabSMSapplication.Controls.Add(this.tbDeleteSMS);
            this.tabSMSapplication.Controls.Add(this.tbAbout);
            this.tabSMSapplication.Location = new System.Drawing.Point(12, 12);
            this.tabSMSapplication.Name = "tabSMSapplication";
            this.tabSMSapplication.SelectedIndex = 0;
            this.tabSMSapplication.Size = new System.Drawing.Size(508, 400);
            this.tabSMSapplication.TabIndex = 0;
            // 
            // tbAbout
            // 
            this.tbAbout.Controls.Add(this.label19);
            this.tbAbout.Controls.Add(this.label18);
            this.tbAbout.Location = new System.Drawing.Point(4, 22);
            this.tbAbout.Name = "tbAbout";
            this.tbAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tbAbout.Size = new System.Drawing.Size(500, 374);
            this.tbAbout.TabIndex = 4;
            this.tbAbout.Text = "About";
            this.tbAbout.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(172, 174);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(165, 13);
            this.label19.TabIndex = 1;
            this.label19.Text = "E-mail: somesh.ballia@gmail..com";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(172, 150);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(157, 13);
            this.label18.TabIndex = 0;
            this.label18.Text = "Developed by : Somesh Pathak";
            // 
            // buttonCallDisconnect
            // 
            this.buttonCallDisconnect.Enabled = false;
            this.buttonCallDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCallDisconnect.Location = new System.Drawing.Point(409, 434);
            this.buttonCallDisconnect.Name = "buttonCallDisconnect";
            this.buttonCallDisconnect.Size = new System.Drawing.Size(107, 31);
            this.buttonCallDisconnect.TabIndex = 4;
            this.buttonCallDisconnect.Text = "Disconnect Call";
            this.buttonCallDisconnect.UseVisualStyleBackColor = true;
            this.buttonCallDisconnect.Click += new System.EventHandler(this.buttonCallDisconnect_Click);
            // 
            // eHedwig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(531, 500);
            this.Controls.Add(this.buttonCallDisconnect);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.gboConnectionStatus);
            this.Controls.Add(this.tabSMSapplication);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "eHedwig";
            this.Text = "SMS Application";
            this.Load += new System.EventHandler(this.SMSapplication_Load);
            this.gboConnectionStatus.ResumeLayout(false);
            this.gboConnectionStatus.PerformLayout();
            this.tbDeleteSMS.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gboDeleteSMS.ResumeLayout(false);
            this.gboDeleteSMS.PerformLayout();
            this.tbReadSMS.ResumeLayout(false);
            this.gboReadSMS.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tbSendSMS.ResumeLayout(false);
            this.gboSendSMS.ResumeLayout(false);
            this.gboSendSMS.PerformLayout();
            this.tbPortSettings.ResumeLayout(false);
            this.gboPortSettings.ResumeLayout(false);
            this.gboPortSettings.PerformLayout();
            this.tabSMSapplication.ResumeLayout(false);
            this.tbAbout.ResumeLayout(false);
            this.tbAbout.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gboConnectionStatus;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label lblConnectionStatus;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.StatusBar statusBar1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TabPage tbDeleteSMS;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCountSMS;
        private System.Windows.Forms.TextBox txtCountSMS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox gboDeleteSMS;
        private System.Windows.Forms.RadioButton rbDeleteReadSMS;
        private System.Windows.Forms.Button btnDeleteSMS;
        private System.Windows.Forms.RadioButton rbDeleteAllSMS;
        private System.Windows.Forms.TabPage tbReadSMS;
        private System.Windows.Forms.GroupBox gboReadSMS;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbReadAll;
        private System.Windows.Forms.RadioButton rbReadStoreUnSent;
        private System.Windows.Forms.RadioButton rbReadUnRead;
        private System.Windows.Forms.RadioButton rbReadStoreSent;
        private System.Windows.Forms.Button btnReadSMS;
        private System.Windows.Forms.ListView lvwMessages;
        private System.Windows.Forms.ColumnHeader colIndex;
        private System.Windows.Forms.ColumnHeader colSentTime;
        private System.Windows.Forms.ColumnHeader colSender;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.TabPage tbSendSMS;
        private System.Windows.Forms.GroupBox gboSendSMS;
        private System.Windows.Forms.Label labelStatusMode;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelStatusTotal;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button buttonFetch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSendSMS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSIM;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TabPage tbPortSettings;
        private System.Windows.Forms.GroupBox gboPortSettings;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtWriteTimeOut;
        private System.Windows.Forms.TextBox txtReadTimeOut;
        private System.Windows.Forms.ComboBox cboParityBits;
        private System.Windows.Forms.ComboBox cboStopBits;
        private System.Windows.Forms.ComboBox cboDataBits;
        private System.Windows.Forms.ComboBox cboBaudRate;
        private System.Windows.Forms.ComboBox cboPortName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabSMSapplication;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBoxResponce;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox textBoxCode;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button buttonCallDisconnect;
        private System.Windows.Forms.TabPage tbAbout;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
    }
}

