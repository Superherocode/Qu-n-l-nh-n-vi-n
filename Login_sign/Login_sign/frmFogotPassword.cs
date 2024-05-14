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
    public partial class frmFogotPassword : Form
    {
        public frmFogotPassword()
        {
            InitializeComponent();
            label5.Text = "";
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void frmFogotPassword_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Modify modify = new Modify();
            string email = txtUsername.Text;
            if (email.Trim() == "")
            {
                MessageBox.Show("Mời bạn điền email để khôi phục mật khẩu");
            }
            else
            {
                string query = $"Select * from TaiKhoan Where Email = '{email}'";
                if (modify.TaiKhoans(query).Count() > 0)
                {
                    label5.ForeColor = Color.Black;
                    label5.Text = modify.TaiKhoans(query)[0].MatKhau;
                }
                else
                {
                    label5.ForeColor = Color.Red;
                    label5.Text = "Email không tồn tại!!";
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtUsername.Text = "";
            label5.Text = "";
        }

        private void label4_Click(object sender, EventArgs e)
        {
            frmRegister register = new frmRegister();
            register.ShowDialog();
        }
    }
}
