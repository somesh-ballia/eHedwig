using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;

namespace SMSapplication
{
    public partial class FetchExcel : Form
    {
        public FetchExcel()
        {
            InitializeComponent();
        }

        OleDbConnection connection;
        OleDbDataAdapter DA;
        DataSet DS = new DataSet();         
        string querry = "select * from [sheet1$]";

        private void buttonFetch_Click(object sender, EventArgs e)
        {            
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName.Length > 0)
            {
                try
                {
                    textBoxPath.Text = openFileDialog1.FileName;
                    string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + textBoxPath.Text + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
                    connection = new OleDbConnection(connectionString);
                    DA = new OleDbDataAdapter(querry, connection);
                    DA.Fill(DS);
                    dataGridView1.DataSource = DS.Tables[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No File found");
            }
        }

        private void FetchExcel_Load(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel (*.xls)|*.xls";
            openFileDialog1.Title = "Save an Excel File";
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                int index = e.ColumnIndex;
                int length = dataGridView1.RowCount;
                if (length > 1)
                {
                    Program.PhoneNumberList = new string[length];
                    for (int i = 0; i <length-1; i++)
                    {
                        DataGridViewCell cell = dataGridView1[index, i];
                        Program.PhoneNumberList[i] = cell.Value.ToString();                        
                    }
                }
                Program.isListSelected = true;
                MessageBox.Show("Phone Numbers Fetched");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
