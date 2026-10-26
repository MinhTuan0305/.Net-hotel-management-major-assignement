using System;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Main
{
    /// <summary>
    /// Đổi mật khẩu của chính người đang đăng nhập (mọi vai trò). Đặc tả: KE_HOACH mục 1.
    /// Session.ChangePassword kiểm tra mật khẩu cũ, độ dài, nhập lại khớp rồi lưu bản băm BCrypt.
    /// </summary>
    public partial class dlgChangePassword : Form
    {
        public dlgChangePassword()
        {
            InitializeComponent();
            UiHelper.SetupDialog(this, btnOk, btnCancel);
            lblAccount.Text = "Tài khoản: " + Session.Username + " (" + Session.FullName + ")";
            lblHint.Text = "Ít nhất " + BusinessRules.MinPasswordLength + " ký tự, khác mật khẩu cũ.";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            bool ok;
            string error;
            try
            {
                using (UiHelper.Busy(this))
                {
                    ok = Session.ChangePassword(txtOld.Text, txtNew.Text, txtConfirm.Text, out error);
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                return;
            }

            if (!ok)
            {
                UiHelper.Warn(error, this);
                return;
            }

            UiHelper.Info("Đã đổi mật khẩu. Lần đăng nhập sau hãy dùng mật khẩu mới.", this);
            DialogResult = DialogResult.OK;
        }
    }
}
