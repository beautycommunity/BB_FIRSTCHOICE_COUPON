using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using k.libary;


namespace BB_FIRSTCHOICE_COUPON
{
    public partial class frmUse : frmSub
    {
        TextBox txtBl = new TextBox();
        TextBox txtAbbno = new TextBox();
        string CvcNo;
        string StrConn;
        string Stcode;
        string Whcode;

        public frmUse()
        {
            InitializeComponent();
            txtBl.Text = "false";
            CvcNo = "1234";
        }

        public frmUse(string _strconn,ref TextBox _txt_abbno,string _cvc_no,ref TextBox _txt_bl,string _stcode,string _whcode)
        {
            InitializeComponent();

            txtAbbno = _txt_abbno;
            txtBl = _txt_bl;
            CvcNo = _cvc_no;
            StrConn = _strconn;
            Stcode = _stcode;
            Whcode = _whcode;

          
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            txtBl.Text = "false";
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            txtBl.Text = "false";
            try
            {
                DataSet ds = new DataSet();
                string sql = "select count(*) cnt from [cmd-fx]..pos_pt where abbno = '" + txtDocno.Text.Trim() + "' and ptstatus = 'S' and workdate >= dateadd(day,-5,getdate())";
                ds = cData.getDataSetWithSqlCommand(StrConn, sql, 1000000, true);

                int cnt = Convert.ToInt32( ds.Tables[0].Rows[0]["cnt"]);

                if (cnt==0)
                {
                    MessageBox.Show("ไม่พบเลขที่บิล : " + txtDocno.Text );
                    txtDocno.Focus();
                    txtBl.Text = "false";
                }
                else
                {
                    txtAbbno.Text = "insert [dbBeautyCommsupport]..DAT_FIRSTCHOICE " +
                                    " (whcode,stcode,cvcno,abbno,CREATEDATE,CFLAG) "+
                                    " values('"+Whcode+"', '"+Stcode+"','"+CvcNo+"', '"+txtDocno.Text.Trim()+"', getdate(), 0) ";

                    txtBl.Text = "true";

                    this.Close();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("เกิดข้อผิดพลาด\n" + ex.Message);
            }
        }

        private void frmUse_Load(object sender, EventArgs e)
        {
            txtBl.Text = "false";
        }
    }
}
