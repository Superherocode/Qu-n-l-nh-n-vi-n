using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Login_sign.C_;

namespace Login_sign
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        public bool CheckAccount(string account)
        {
            return Regex.IsMatch(account, @"^[a-zA-Z0-9]{6,24}$");
        }

        public bool CheckEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z0-9_.]{3,20}@gmail.com(.vn|)$");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void frmRegister_Load(object sender, EventArgs e)
        {

        }

        Modify modify = new Modify();
        private void button1_Click(object sender, EventArgs e)
        {
            string tentk = txtUsername.Text;
            string mk = txtpassword.Text;
            string email = textBox_Email.Text;
            string xnmk = txtComPassword.Text;

            if (!CheckAccount(tentk))
            {
                MessageBox.Show("Vui lòng đặt tên tài khoản đúng định dạng từ 6-24 kí tự, các kí tự bao gồm [0-9], [a-z], [A-Z]");
                return;
            }

            if (mk.Length < 6 || mk.Length > 24 || !Regex.IsMatch(mk, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Vui lòng điền mật khẩu từ 6-24 kí tự, các kí tự bao gồm [0-9], [a-z], [A-Z]");
                return;
            }

            if (xnmk != mk)
            {
                MessageBox.Show("Xác nhận mật khẩu chưa trùng khớp!");
                return;
            }

            if (!CheckEmail(email))
            {
                MessageBox.Show("Email không đúng định dạng");
                return;
            }

            if (modify.TaiKhoans($"Select * from Taikhoan Where Email ='{email}'").Count > 0)
            {
                MessageBox.Show("Email đã tồn tại");
                return;
            }

            try
            {
                string query = $"Insert into Taikhoan values ('"+tentk+ "','"+mk+"','"+email+"')";
                modify.Command(query);

                if (MessageBox.Show("Đăng ký tài khoản thành công! Bạn có muốn đăng nhập không?", "Thông báo",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    this.Hide();
                    frmLogin dangNhap = new frmLogin();
                    dangNhap.ShowDialog();
                }
            }
            catch
            {
                MessageBox.Show("Tên tài khoản này đã được đăng ký!\nVui lòng đăng ký tên tài khoản khác!");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            frmLogin frmLogin = new frmLogin();
            frmLogin.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtpassword.Text = "";
            txtUsername.Text = "";
            txtComPassword.Text = "";
            textBox_Email.Text = "";
        }

        private void checkbxShowPas_CheckedChanged(object sender, EventArgs e)
        {
            if (checkbxShowPas.Checked)
            {
                txtpassword.PasswordChar = '\0';
                txtComPassword.PasswordChar = '\0';
            }
            else
            {
                txtpassword.PasswordChar = '•';
                txtComPassword.PasswordChar = '•';
            }
        }
    }
}
