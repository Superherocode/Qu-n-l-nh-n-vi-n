using Login_sign.C_;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login_sign
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frm_login_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            frmRegister register = new frmRegister();
            register.ShowDialog();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            frmFogotPassword fogotPassword = new frmFogotPassword();
            fogotPassword.ShowDialog();
        }


        Modify modify = new Modify();
        private void button1_Click(object sender, EventArgs e)
        {
            string tentk = txtUsername.Text;
            string mk = txtpassword.Text;
            if (tentk.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập tên tài khoản!");
            }
            else if (mk.Trim() == "")
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!");
            }
            else
            {
                string query = "Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "' and MatKhau = '" + mk + "' ";
                string query1 = "Select * from TaiKhoan where TenTaiKhoan = '" + tentk + "' and MatKhau = '" + mk + "' ";
                if (modify.TaiKhoans(query).Count() != 0)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Tên tài khoản và mật khẩu không chính xác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            txtpassword.Text = "";
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            if(checkbxShowPas.Checked) {
                txtpassword.PasswordChar = '\0' ;
            }
            else
            {
                txtpassword.PasswordChar = '•';
            }
        }
    }
}
