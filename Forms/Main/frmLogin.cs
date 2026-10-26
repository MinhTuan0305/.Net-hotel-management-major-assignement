using System;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Main
{
    /// <summary>
    /// Đăng nhập (mọi vai trò). Đặc tả: KE_HOACH mục 1; nghiệp vụ mục 1.
    /// Session.TryLogin đọc tài khoản qua sp_GetEmployeeForLogin và so mật khẩu BCrypt ở C#.
    /// Sai tên, sai mật khẩu đều báo chung một câu để không lộ tài khoản nào tồn tại;
    /// tài khoản bị khóa thì báo riêng. Đúng thì DialogResult = OK để Program.cs mở frmMain.
    /// </summary>
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            lblTitle.Text = HotelInfo.Name;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireText(txtUsername, "tên đăng nhập")) return;
            if (txtPassword.Text.Length == 0)
            {
                UiHelper.Warn("Hãy nhập mật khẩu.", this);
                txtPassword.Focus();
                return;
            }

            bool ok;
            string error;
            try
            {
                using (UiHelper.Busy(this))
                {
                    ok = Session.TryLogin(txtUsername.Text, txtPassword.Text, out error);
                }
            }
            catch (Exception ex)
            {
                // Lỗi kết nối DB... (không phải sai mật khẩu)
                ErrorHandler.Show(ex, this);
                return;
            }

            if (!ok)
            {
                UiHelper.Warn(error, this);
                txtPassword.SelectAll();
                txtPassword.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }
    }
}
