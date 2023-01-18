using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System_WareHouse
{
    public partial class Login : Form
    {

        List<Account> listAccoubt = ListAccount.Instance.ListAccounts;
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (KiemTraDangNhap(txtUserName.Text, txtPassWord.Text))
            {
                Home f = new Home();
                f.Show();
                this.Hide();
                f.DangXuat += F_DangXuat;
            }
            else
            {
                MessageBox.Show("Sai ten khoan hoac mat khau", "Loi");
                txtUserName.Focus();
            }
        }

        private void F_DangXuat(object sender, EventArgs e)
        {
            (sender as Home).isExit = false;
            (sender as Home).Close();
            this.Show();
        }

        //function kiemtra
        bool KiemTraDangNhap(string username, string passworld)
        {
            for (int i = 0; i < listAccoubt.Count; i++)
            {
                if (username == listAccoubt[i].UserName && passworld == listAccoubt[i].PassWorld)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
