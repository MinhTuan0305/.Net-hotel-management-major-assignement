using System;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Stay;

namespace Hotel_management_system
{
    internal static class Program
    {
        // Điểm khởi động chương trình:
        //   1. Gắn bộ bắt lỗi toàn cục (phải làm TRƯỚC khi tạo form đầu tiên).
        //   2. Thử kết nối DB; lỗi thì báo cách sửa rồi thoát.
        //   3. Mở form đầu tiên.
        //
        // Khi đã có frmLogin và frmMain (frmMain có thuộc tính public bool LoggedOut),
        // thay đoạn CODE TẠM bên dưới bằng vòng lặp sau:
        //
        //   while (true)
        //   {
        //       using (var login = new frmLogin())
        //       {
        //           if (login.ShowDialog() != DialogResult.OK) return;
        //       }
        //       var main = new frmMain();
        //       Application.Run(main);
        //       if (!main.LoggedOut) return;
        //   }
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

            // ===== CODE TẠM (Tuấn) – XÓA khi đã có frmLogin và frmMain =====
            // Tự đăng nhập bằng tài khoản admin mẫu rồi mở thẳng màn hình lễ tân để thử.
            // Đăng nhập thật qua DB (sp_GetEmployeeForLogin) nên Session.EmployeeParam() dùng được.
            string loginError;
            if (!Session.TryLogin("admin", "Admin@123", out loginError))
            {
                MessageBox.Show(loginError, UiHelper.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Application.Run(new frmFrontDesk());
            // ===== HẾT CODE TẠM =====
        }
    }
}
