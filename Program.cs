using System;
using System.Windows.Forms;
using HotelManagement.Core;

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
        // thay dòng Application.Run(new Form1()) bằng vòng lặp sau:
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

            Application.Run(new Form1());
        }
    }
}
