// =============================================================================
// Session.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Giữ thông tin nhân viên ĐANG ĐĂNG NHẬP trong suốt thời gian chạy chương trình
//   (EmployeeId, Username, FullName, RoleName), để:
//     - truyền @EmployeeId cho mọi procedure (DB dùng nó kiểm tra quyền qua
//       fn_HasRole và ghi lại ai đã check-in, thu tiền, lập hóa đơn...);
//     - ẩn/hiện menu và nút theo vai trò (Session.HasRole / IsManager...);
//     - hiện tên nhân viên trên thanh trạng thái của frmMain.
//   Kèm 2 nghiệp vụ tài khoản dùng chung:
//     - TryLogin: kiểm tra đăng nhập (gọi sp_GetEmployeeForLogin + BCrypt);
//     - ChangePassword: đổi mật khẩu của chính mình (dlgChangePassword).
//
// CÁCH DÙNG
//   frmLogin:
//       string error;
//       if (Session.TryLogin(txtUser.Text, txtPass.Text, out error)) { DialogResult = DialogResult.OK; }
//       else UiHelper.Warn(error);
//   Gọi procedure:
//       DbHelper.Exec("dbo.sp_ExtendStay", ..., Session.EmployeeParam());
//   Phân quyền giao diện:
//       btnVoid.Visible = Session.IsManager;            // Admin hoặc Manager
//       mnuReports.Visible = Session.HasRole(Roles.Admin, Roles.Manager);
//
// LƯU Ý
//   Ẩn nút chỉ để giao diện gọn. Quyền thật vẫn do procedure kiểm tra (lỗi 50001),
//   nên quên ẩn một nút cũng không làm sai dữ liệu.
// =============================================================================
using System;
using System.Data;
using System.Data.SqlClient;

namespace HotelManagement.Core
{
    public static class Session
    {
        private const string WrongLogin = "Sai tên đăng nhập hoặc mật khẩu.";

        public static int EmployeeId { get; private set; }
        public static string Username { get; private set; }
        public static string FullName { get; private set; }
        public static string RoleName { get; private set; }
        public static DateTime LoginAt { get; private set; }

        public static bool IsLoggedIn
        {
            get { return EmployeeId > 0; }
        }

        public static bool IsAdmin { get { return HasRole(Roles.Admin); } }

        /// <summary>Admin hoặc Manager – nhóm được void hóa đơn, giảm giá trên 10%, xem báo cáo...</summary>
        public static bool IsManager { get { return HasRole(Roles.Admin, Roles.Manager); } }

        public static bool IsReceptionist { get { return HasRole(Roles.Receptionist); } }
        public static bool IsHousekeeping { get { return HasRole(Roles.Housekeeping); } }

        /// <summary>Tên vai trò bằng tiếng Việt để hiển thị.</summary>
        public static string RoleDisplayName
        {
            get { return Roles.ToVietnamese(RoleName); }
        }

        /// <summary>True nếu người đang đăng nhập có MỘT trong các vai trò truyền vào.</summary>
        public static bool HasRole(params string[] roles)
        {
            if (!IsLoggedIn) return false;
            if (roles == null || roles.Length == 0) return true;
            foreach (string r in roles)
            {
                if (string.Equals(r, RoleName, StringComparison.OrdinalIgnoreCase)) return true;
            }
            return false;
        }

        /// <summary>Tham số @EmployeeId của người đang đăng nhập, truyền thẳng vào procedure.</summary>
        public static SqlParameter EmployeeParam()
        {
            RequireLogin();
            return DbHelper.P("@EmployeeId", EmployeeId);
        }

        /// <summary>Ném lỗi nếu chưa đăng nhập (phòng trường hợp mở form khi chưa login).</summary>
        public static void RequireLogin()
        {
            if (!IsLoggedIn)
                throw new InvalidOperationException("Chưa đăng nhập. Hãy đăng nhập lại.");
        }

        /// <summary>Ghi nhận đăng nhập thành công (TryLogin tự gọi).</summary>
        public static void Login(int employeeId, string username, string fullName, string roleName)
        {
            EmployeeId = employeeId;
            Username = username;
            FullName = fullName;
            RoleName = roleName;
            LoginAt = DateTime.Now;
        }

        /// <summary>Xóa thông tin đăng nhập (khi bấm Đăng xuất).</summary>
        public static void Logout()
        {
            EmployeeId = 0;
            Username = null;
            FullName = null;
            RoleName = null;
            LoginAt = DateTime.MinValue;
        }

        /// <summary>
        /// Kiểm tra đăng nhập. Đúng thì lưu vào Session và trả về true; sai thì trả
        /// về false kèm câu báo lỗi. Lỗi kết nối DB vẫn ném ra để form đưa cho ErrorHandler.
        /// </summary>
        public static bool TryLogin(string username, string password, out string error)
        {
            error = null;
            username = (username ?? "").Trim();
            if (username.Length == 0 || string.IsNullOrEmpty(password))
            {
                error = "Hãy nhập tên đăng nhập và mật khẩu.";
                return false;
            }

            DataTable dt = DbHelper.Query("dbo.sp_GetEmployeeForLogin", DbHelper.P("@Username", username));
            if (dt.Rows.Count == 0)
            {
                error = WrongLogin;
                return false;
            }

            DataRow row = dt.Rows[0];
            string hash = DbHelper.Value<string>(row, "PasswordHash");
            if (!PasswordHasher.Verify(password, hash))
            {
                error = WrongLogin;
                return false;
            }

            if (!DbHelper.Value<bool>(row, "IsActive"))
            {
                error = "Tài khoản đã bị khóa. Hãy liên hệ Admin.";
                return false;
            }

            Login(DbHelper.Value<int>(row, "EmployeeId"),
                  DbHelper.Value<string>(row, "Username"),
                  DbHelper.Value<string>(row, "FullName"),
                  DbHelper.Value<string>(row, "RoleName"));
            return true;
        }

        /// <summary>Đổi mật khẩu của người đang đăng nhập (dlgChangePassword gọi).</summary>
        public static bool ChangePassword(string oldPassword, string newPassword, string confirmPassword, out string error)
        {
            RequireLogin();
            error = null;

            if (string.IsNullOrEmpty(oldPassword))
            {
                error = "Hãy nhập mật khẩu hiện tại.";
                return false;
            }
            if (!PasswordHasher.IsValidNewPassword(newPassword, out error)) return false;
            if (newPassword != confirmPassword)
            {
                error = "Mật khẩu nhập lại không khớp.";
                return false;
            }
            if (newPassword == oldPassword)
            {
                error = "Mật khẩu mới phải khác mật khẩu hiện tại.";
                return false;
            }

            string currentHash = DbHelper.Scalar<string>(
                "SELECT PasswordHash FROM dbo.Employees WHERE EmployeeId = @Id",
                DbHelper.P("@Id", EmployeeId));
            if (!PasswordHasher.Verify(oldPassword, currentHash))
            {
                error = "Mật khẩu hiện tại không đúng.";
                return false;
            }

            DbHelper.ExecText(
                "UPDATE dbo.Employees SET PasswordHash = @Hash WHERE EmployeeId = @Id",
                DbHelper.P("@Hash", PasswordHasher.Hash(newPassword)),
                DbHelper.P("@Id", EmployeeId));
            return true;
        }
    }
}
