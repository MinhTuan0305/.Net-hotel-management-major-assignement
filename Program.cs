using System;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Main;

namespace Hotel_management_system
{
    internal static class Program
    {
        // Điểm khởi động chương trình:
        //   1. Gắn bộ bắt lỗi toàn cục (phải làm TRƯỚC khi tạo form đầu tiên).
        //   2. Thử kết nối DB; lỗi thì báo cách sửa rồi thoát.
        //   3. Vòng lặp đăng nhập: frmLogin -> frmMain. Bấm Đăng xuất trong frmMain
        //      (LoggedOut = true) thì quay lại frmLogin; bấm Thoát hoặc đóng cửa sổ thì kết thúc.
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ErrorHandler.Install();

            string error;
            if (!DbHelper.TestConnection(out error))
            {
                MessageBox.Show(error, UiHelper.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            while (true)
            {
                using (var login = new frmLogin())
                {
                    if (login.ShowDialog() != DialogResult.OK) return;
                }

                var main = new frmMain();
                Application.Run(main);
                if (!main.LoggedOut) return;
            }
        }
    }
}
