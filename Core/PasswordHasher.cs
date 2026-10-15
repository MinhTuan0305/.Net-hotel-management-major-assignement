// =============================================================================
// PasswordHasher.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Băm và kiểm tra mật khẩu bằng BCrypt. Database chỉ lưu chuỗi băm
//   (Employees.PasswordHash, dạng "$2a$11$..."), không bao giờ lưu mật khẩu gốc.
//     - Hash(pw)           : khi tạo nhân viên mới, đặt lại mật khẩu, đổi mật khẩu.
//     - Verify(pw, hash)   : khi đăng nhập, khi kiểm tra mật khẩu cũ.
//     - IsValidNewPassword : kiểm tra mật khẩu mới đủ điều kiện (tối thiểu 6 ký tự).
//
// CẦN CÀI
//   NuGet: BCrypt.Net-Next (chạy được trên .NET Framework 4.8).
//
// TÀI KHOẢN CÓ SẴN TRONG DỮ LIỆU MẪU
//   admin / Admin@123 ; các nhân viên mẫu (quanlm, hapt, lantt, ...) / 123456.
// =============================================================================
using System;
using BCryptNet = BCrypt.Net.BCrypt;

namespace HotelManagement.Core
{
    public static class PasswordHasher
    {
        /// <summary>Độ khó băm. 11 là mức cân bằng (khoảng 0,1–0,2 giây mỗi lần).</summary>
        public const int WorkFactor = 11;

        /// <summary>Mật khẩu mặc định khi Admin bấm "Đặt lại mật khẩu" trong frmEmployees.</summary>
        public const string DefaultResetPassword = "123456";

        public static string Hash(string password)
        {
            if (password == null) throw new ArgumentNullException("password");
            return BCryptNet.HashPassword(password, WorkFactor);
        }

        /// <summary>True nếu mật khẩu khớp chuỗi băm. Chuỗi băm hỏng hoặc rỗng trả về false.</summary>
        public static bool Verify(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash)) return false;
            try
            {
                return BCryptNet.Verify(password, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        /// <summary>Kiểm tra mật khẩu mới: không rỗng, không toàn khoảng trắng, đủ độ dài.</summary>
        public static bool IsValidNewPassword(string password, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(password))
            {
                error = "Mật khẩu mới không được để trống.";
                return false;
            }
            if (password.Length < BusinessRules.MinPasswordLength)
            {
                error = "Mật khẩu mới phải có ít nhất " + BusinessRules.MinPasswordLength + " ký tự.";
                return false;
            }
            if (password.Trim() != password)
            {
                error = "Mật khẩu không được bắt đầu hoặc kết thúc bằng khoảng trắng.";
                return false;
            }
            return true;
        }
    }
}
