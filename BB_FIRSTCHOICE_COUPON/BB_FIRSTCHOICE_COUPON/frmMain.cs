using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using k.libary;

namespace BB_FIRSTCHOICE_COUPON
{
    public partial class frmMain : frmSub
    {
        string StrConn_cli;
        string StrConn_ser;
        string Stcode;
        string Whcode;
  
        TextBox txtAbbno = new TextBox();
        TextBox txtBl = new TextBox();

        public frmMain()
        {
            InitializeComponent();

            StrConn_cli = "Data Source=cenl.dyndns.info,1401;Initial Catalog=CMD-FX;User ID=sa;password=0000";
            StrConn_ser = "Data Source=192.168.1.220,1433;Initial Catalog=CMD-BX;User ID=sa;password=0211";
            Stcode = "2558";
            Whcode = "1001";
        }

        public frmMain(string _strConn_cli, string _strConn_ser, string _stcode, string _whcode)
        {
            InitializeComponent();

            StrConn_cli = _strConn_cli;
            StrConn_ser = _strConn_ser;
            Stcode = _stcode;
            Whcode = _whcode;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            SqlCommand comm = new SqlCommand();
            SqlConnection conn = new SqlConnection(StrConn_ser);

            try
            {
                

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (txtSMS.Text != "")
                {
                    int cnt = 0;
                    string sql = "select count(*) cnt from [dbBeautyCommsupport]..DAT_FIRSTCHOICE where CVCNO = '" + txtSMS.Text + "'";
                    using (cWaitIndicator cw = new cWaitIndicator())
                    {
                        DataSet ds = new DataSet();


                        ds = cData.getDataSetWithSqlCommand(StrConn_ser, sql, 1000000, true);
                        cnt = Convert.ToInt32(ds.Tables[0].Rows[0]["cnt"]);
                    }

                    if (cnt == 0)
                    {


                        frmUse frm = new frmUse(StrConn_cli,ref txtAbbno,txtSMS.Text, ref txtBl,Stcode,Whcode);
                        frm.ShowDialog();

                        if (txtBl.Text == "true")
                        {

                            comm.Connection = conn;
                            comm.CommandText = txtAbbno.Text;
                            comm.CommandTimeout = 1000000;

                            comm.ExecuteNonQuery();

                            
                            MessageBox.Show("บันทึกสำเร็จ");
                            txtSMS.Text = "";
                            txtSMS.Focus();

                        }




                    }

                    else
                    {
                        frmNo frm = new frmNo();
                        frm.ShowDialog();
                        txtSMS.Focus();
                    }

                }
                else
                {
                    MessageBox.Show(" กรุณากรอก รหัสจาก SMS ");
                    txtSMS.Focus();
                }

             
            }
            catch(Exception ex)
            {
                
                MessageBox.Show("เกิดข้อผิดพลาด\n" + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void txtSMS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                //MessageBox.Show("Enter key pressed");
                btnOK_Click(sender,e);
            }
        }
    }
}
