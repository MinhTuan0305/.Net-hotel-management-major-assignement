// =============================================================================
// ErrorHandler.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Biến mọi lỗi (đặc biệt là lỗi từ SQL Server) thành thông báo tiếng Việt dễ
//   hiểu, để form nào cũng báo lỗi giống nhau:
//     - Lỗi nghiệp vụ 50001–50007 do procedure THROW: hiện nguyên câu tiếng Việt
//       đã viết sẵn trong procedure, kèm tiêu đề theo loại lỗi.
//     - Lỗi ràng buộc (trùng UNIQUE 2627/2601, khóa ngoại/CHECK 547): đổi tên
//       ràng buộc (vd. UQ_Customers_IdNumber) thành câu dễ hiểu.
//     - Lỗi kết nối, sai cấu hình, DB cũ thiếu procedure: hướng dẫn cách sửa.
//     - Lỗi khác: ghi vào file log (thư mục Logs cạnh file .exe) để nhóm tra lại.
//   Install() bắt luôn các lỗi "lọt lưới" để chương trình không bị văng.
//
// CÁCH DÙNG TRONG FORM
//   private void btnCheckIn_Click(object sender, EventArgs e)
//   {
//       try
//       {
//           DbHelper.Exec("dbo.sp_CheckInRoom", ...);
//           UiHelper.Info("Nhận phòng thành công.");
//           DialogResult = DialogResult.OK;
//       }
//       catch (Exception ex)
//       {
//           ErrorHandler.Show(ex, this);
//       }
//   }
//   Hoặc gọn hơn:
//       if (ErrorHandler.Try(() => DbHelper.Exec("dbo.sp_CheckInRoom", ...), this)) { ... }
//
//   Muốn xử lý riêng một mã lỗi (vd. chưa đủ cọc thì mở dlgPayment):
//       catch (Exception ex) when (ErrorHandler.GetSqlNumber(ex) == SqlErrorCodes.Money) { ... }
// =============================================================================
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace HotelManagement.Core
{
    public static class ErrorHandler
    {
        /// <summary>Tên ràng buộc trong DB -> câu thông báo. Khớp với file 01_HotelManagement_TaoDB.sql.</summary>
        private static readonly Dictionary<string, string> ConstraintMessages =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // Trùng giá trị duy nhất
            { "UQ_Amenities_AmenityName", "Tên tiện nghi đã tồn tại." },
            { "UQ_Customers_IdNumber", "Số CCCD/hộ chiếu này đã thuộc về một khách hàng khác." },
            { "UQ_Employees_Username", "Tên đăng nhập đã được dùng." },
            { "UQ_Roles_RoleName", "Tên vai trò đã tồn tại." },
            { "UQ_RoomTypes_TypeName", "Tên loại phòng đã tồn tại." },
            { "UQ_Rooms_RoomNumber", "Số phòng đã tồn tại." },
            { "UQ_Services_ServiceName", "Tên dịch vụ đã tồn tại." },
            { "UX_Invoices_ActivePerStay", "Phòng này đã có hóa đơn hiệu lực. Muốn lập lại phải void hóa đơn cũ trước." },
            { "UX_HousekeepingTasks_OpenTask", "Phòng này đang có việc dọn chưa hoàn tất." },

            // CHECK
            { "CK_Customers_DateOfBirth", "Ngày sinh không được ở tương lai." },
            { "CK_Customers_Gender", "Giới tính chỉ được là Nam, Nữ hoặc Khác." },
            { "CK_RoomTypes_BasePrice", "Giá phòng phải lớn hơn 0." },
            { "CK_RoomTypes_MaxOccupancy", "Sức chứa phải từ 1 đến 10 người." },
            { "CK_Services_Price", "Giá dịch vụ không được âm." },
            { "CK_Roles_RoleName", "Tên vai trò chỉ được là Admin, Manager, Receptionist hoặc Housekeeping." },
            { "CK_Rooms_Status", "Trạng thái phòng không hợp lệ." },
            { "CK_BookingRooms_Dates", "Ngày trả phòng phải sau ngày nhận phòng." },
            { "CK_BookingRooms_Adults", "Mỗi phòng phải có ít nhất 1 người lớn." },
            { "CK_BookingRooms_Price", "Giá phòng phải lớn hơn 0." },
            { "CK_Bookings_DepositRate", "Tỷ lệ cọc chỉ được là 50% hoặc 100%." },
            { "CK_ServiceUsages_Quantity", "Số lượng phải lớn hơn 0." },
            { "CK_ServiceUsages_UnitPrice", "Đơn giá không được âm." },
            { "CK_Payments_Amount", "Số tiền phải lớn hơn 0." },
            { "CK_Payments_Method", "Phương thức thanh toán không hợp lệ." },
            { "CK_Payments_Reference", "Chuyển khoản/ví điện tử phải có mã giao dịch." },
            { "CK_Invoices_Buyer", "Hóa đơn có mã số thuế phải có tên đơn vị." },
            { "CK_Invoices_Discount", "Giảm giá không được âm và không vượt tổng tiền phòng + dịch vụ." },
            { "CK_Invoices_DiscountReason", "Có giảm giá thì phải ghi lý do." },
            { "CK_Invoices_TaxRate", "Thuế suất phải từ 0 đến 100." },
            { "CK_HousekeepingTasks_Note", "Báo phòng cần bảo trì thì phải ghi chú." },

            // Khóa ngoại – thường gặp khi XÓA bản ghi đang được dùng
            { "FK_Employees_Roles", "Vai trò này đang có nhân viên." },
            { "FK_Rooms_RoomTypes", "Loại phòng này đang có phòng. Hãy ngừng dùng thay vì xóa." },
            { "FK_RTA_Amenities", "Tiện nghi này đang được gán cho loại phòng. Bỏ gán trước khi xóa." },
            { "FK_RTA_RoomTypes", "Loại phòng này đang được gán tiện nghi." },
            { "FK_Bookings_Customers", "Khách hàng đã có phiếu đặt phòng nên không xóa được." },
            { "FK_BookingRooms_Rooms", "Phòng đã có lịch sử đặt phòng. Hãy ngừng khai thác thay vì xóa." },
            { "FK_BookingRooms_Bookings", "Phiếu đặt đang có phòng." },
            { "FK_ServiceUsages_Services", "Dịch vụ đã được sử dụng. Hãy ngừng bán thay vì xóa." },
            { "FK_HousekeepingTasks_Rooms", "Phòng đã có lịch sử dọn phòng nên không xóa được." },
            { "FK_Bookings_CreatedBy", EmployeeInUse },
            { "FK_BookingRooms_CheckInBy", EmployeeInUse },
            { "FK_BookingRooms_CheckOutBy", EmployeeInUse },
            { "FK_ServiceUsages_CreatedBy", EmployeeInUse },
            { "FK_Invoices_IssuedBy", EmployeeInUse },
            { "FK_Invoices_VoidedBy", EmployeeInUse },
            { "FK_Payments_Employees", EmployeeInUse },
            { "FK_HousekeepingTasks_AssignedTo", EmployeeInUse }
        };

        private const string EmployeeInUse = "Nhân viên đã phát sinh giao dịch nên không xóa được. Hãy khóa tài khoản.";

        private const string ConnectionHelp =
            "Không kết nối được SQL Server.\n\n" +
            "Kiểm tra:\n" +
            "  1. Dịch vụ SQL Server trên máy đã chạy chưa (SQL Server Configuration Manager).\n" +
            "  2. Tên server trong file connectionStrings.config có đúng không (vd. .\\SQLEXPRESS).";

        private const string OldDatabaseHelp =
            "Database trên máy bạn có thể là bản cũ (thiếu bảng, cột hoặc procedure).\n" +
            "Hãy kéo code mới nhất và chạy lại database\\reset_db.bat.";

        // ------------------------------------------------------------------
        // API dùng trong form
        // ------------------------------------------------------------------

        /// <summary>Hiện thông báo lỗi phù hợp và ghi log nếu là lỗi bất thường.</summary>
        public static void Show(Exception ex, IWin32Window owner = null)
        {
            if (ex == null) return;
            string title;
            MessageBoxIcon icon;
            string message = GetMessage(ex, out title, out icon);
            if (!IsBusinessError(ex)) Log(ex);
            MessageBox.Show(owner, message, title, MessageBoxButtons.OK, icon);
        }

        /// <summary>Chạy một thao tác; lỗi thì hiện thông báo và trả về false.</summary>
        public static bool Try(Action action, IWin32Window owner = null)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                Show(ex, owner);
                return false;
            }
        }

        /// <summary>
        /// Chạy một thao tác có kết quả; lỗi thì hiện thông báo và trả về false.
        ///   DataTable dt;
        ///   if (ErrorHandler.Try(() => DbHelper.QueryText("SELECT ..."), out dt, this)) UiHelper.Bind(dgv, dt);
        /// </summary>
        public static bool Try<T>(Func<T> func, out T result, IWin32Window owner = null)
        {
            try
            {
                result = func();
                return true;
            }
            catch (Exception ex)
            {
                result = default(T);
                Show(ex, owner);
                return false;
            }
        }

        /// <summary>Mã lỗi SQL Server của exception (0 nếu không phải lỗi SQL).</summary>
        public static int GetSqlNumber(Exception ex)
        {
            SqlException sql = FindSqlException(ex);
            return sql == null ? 0 : sql.Number;
        }

        /// <summary>True nếu là lỗi nghiệp vụ 50001–50007 (lỗi "bình thường", không cần ghi log).</summary>
        public static bool IsBusinessError(Exception ex)
        {
            return SqlErrorCodes.IsBusiness(GetSqlNumber(ex));
        }

        /// <summary>Tạo câu thông báo, tiêu đề và biểu tượng cho một lỗi.</summary>
        public static string GetMessage(Exception ex, out string title, out MessageBoxIcon icon)
        {
            title = "Lỗi";
            icon = MessageBoxIcon.Error;
            if (ex == null) return "";

            ex = Unwrap(ex);

            SqlException sql = FindSqlException(ex);
            if (sql != null) return GetSqlMessage(sql, out title, out icon);

            if (ex is ConfigurationErrorsException)
            {
                title = "Lỗi cấu hình";
                return "Thiếu hoặc sai file connectionStrings.config.\n" +
                       "Hãy chép connectionStrings.example.config thành connectionStrings.config, " +
                       "sửa tên server rồi build lại.\n\nChi tiết: " + ex.Message;
            }

            if (ex is InvalidOperationException && ex.Message.StartsWith("Chưa đăng nhập", StringComparison.Ordinal))
            {
                title = "Phiên đăng nhập";
                icon = MessageBoxIcon.Warning;
                return ex.Message;
            }

            return ex.Message;
        }

        /// <summary>
        /// Gắn bộ bắt lỗi toàn cục. Gọi MỘT lần trong Program.Main, TRƯỚC khi tạo form đầu tiên.
        /// </summary>
        public static void Install()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => Show(e.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex == null) return;
                Log(ex);
                MessageBox.Show("Chương trình gặp lỗi nghiêm trọng và sẽ đóng.\n\n" + ex.Message,
                                "Lỗi nghiêm trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }

        /// <summary>Ghi lỗi vào Logs\error_yyyyMMdd.log cạnh file .exe. Không bao giờ ném lỗi.</summary>
        public static void Log(Exception ex)
        {
            if (ex == null) return;
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
                Directory.CreateDirectory(dir);
                string file = Path.Combine(dir, "error_" + DateTime.Now.ToString("yyyyMMdd") + ".log");

                var sb = new StringBuilder();
                sb.AppendLine("==== " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                              " | NV: " + (Session.IsLoggedIn ? Session.Username : "(chưa đăng nhập)"));
                SqlException sql = FindSqlException(ex);
                if (sql != null) sb.AppendLine("SQL error " + sql.Number + " | procedure: " + sql.Procedure + " | line " + sql.LineNumber);
                sb.AppendLine(ex.ToString());
                sb.AppendLine();
                File.AppendAllText(file, sb.ToString(), Encoding.UTF8);
            }
            catch
            {
                // Ghi log hỏng thì bỏ qua, không làm phiền người dùng.
            }
        }

        // ------------------------------------------------------------------
        // Nội bộ
        // ------------------------------------------------------------------

        private static string GetSqlMessage(SqlException sql, out string title, out MessageBoxIcon icon)
        {
            icon = MessageBoxIcon.Warning;
            string mapped;

            switch (sql.Number)
            {
                // ---- Lỗi nghiệp vụ do procedure THROW: dùng nguyên câu tiếng Việt ----
                case SqlErrorCodes.Permission: title = "Không có quyền"; return sql.Message;
                case SqlErrorCodes.NotFound: title = "Không tìm thấy"; return sql.Message;
                case SqlErrorCodes.InvalidState: title = "Trạng thái không phù hợp"; return sql.Message;
                case SqlErrorCodes.InvalidData: title = "Dữ liệu không hợp lệ"; return sql.Message;
                case SqlErrorCodes.ScheduleConflict: title = "Trùng lịch"; return sql.Message;
                case SqlErrorCodes.Money: title = "Lỗi số tiền"; return sql.Message;
                case SqlErrorCodes.DateTimeRule: title = "Lỗi ngày giờ"; return sql.Message;

                // ---- Ràng buộc ----
                case 2627:
                case 2601:
                    title = "Dữ liệu bị trùng";
                    return TryMapConstraint(sql.Message, out mapped)
                        ? mapped
                        : "Giá trị này đã tồn tại, hãy nhập giá trị khác.";

                case 547:
                    if (TryMapConstraint(sql.Message, out mapped))
                    {
                        title = sql.Message.IndexOf("DELETE", StringComparison.OrdinalIgnoreCase) >= 0
                            ? "Dữ liệu đang được sử dụng"
                            : "Dữ liệu không hợp lệ";
                        return mapped;
                    }
                    if (sql.Message.IndexOf("DELETE", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        title = "Dữ liệu đang được sử dụng";
                        return "Không xóa được vì dữ liệu này đang được dùng ở nơi khác.";
                    }
                    title = "Dữ liệu không hợp lệ";
                    return "Dữ liệu không thỏa ràng buộc của hệ thống.\n\nChi tiết: " + sql.Message;

                case 515:
                    title = "Thiếu dữ liệu";
                    return "Chưa nhập đủ thông tin bắt buộc.";

                case 8152:
                case 2628:
                    title = "Dữ liệu quá dài";
                    return "Có ô nhập vượt quá số ký tự cho phép.";

                case 1205:
                    title = "Hệ thống bận";
                    return "Thao tác bị trùng lúc với người khác. Hãy thử lại.";

                case -2:
                    title = "Quá thời gian chờ";
                    icon = MessageBoxIcon.Error;
                    return "Máy chủ phản hồi quá lâu. Hãy thử lại.";

                // ---- Kết nối / cấu hình ----
                case -1:
                case 2:
                case 26:
                case 40:
                case 53:
                case 10060:
                case 10061:
                case 11001:
                    title = "Mất kết nối";
                    icon = MessageBoxIcon.Error;
                    return ConnectionHelp;

                case 4060:
                    title = "Không mở được database";
                    icon = MessageBoxIcon.Error;
                    return "Không mở được database HotelManagement. Đã chạy script 01_HotelManagement_TaoDB.sql chưa?\n\n" +
                           "Chi tiết: " + sql.Message;

                case 18456:
                    title = "Đăng nhập SQL Server thất bại";
                    icon = MessageBoxIcon.Error;
                    return "SQL Server từ chối đăng nhập. Kiểm tra Integrated Security hoặc user/password " +
                           "trong connectionStrings.config.\n\nChi tiết: " + sql.Message;

                // ---- DB trên máy là bản cũ ----
                case 208:   // Invalid object name
                case 207:   // Invalid column name
                case 2812:  // Could not find stored procedure
                    title = "Database chưa cập nhật";
                    icon = MessageBoxIcon.Error;
                    return OldDatabaseHelp + "\n\nChi tiết: " + sql.Message;

                // ---- Lỗi lập trình (gọi sai tham số) ----
                case 201:   // Procedure expects parameter ... which was not supplied
                case 8144:  // Procedure has too many arguments
                    title = "Lỗi gọi procedure";
                    icon = MessageBoxIcon.Error;
                    return "Code gọi procedure thiếu hoặc thừa tham số (lỗi lập trình).\n\nChi tiết: " + sql.Message;

                default:
                    title = "Lỗi cơ sở dữ liệu";
                    icon = MessageBoxIcon.Error;
                    return sql.Message + "\n\n(Mã lỗi SQL: " + sql.Number + ")";
            }
        }

        private static bool TryMapConstraint(string sqlMessage, out string mapped)
        {
            mapped = null;
            if (string.IsNullOrEmpty(sqlMessage)) return false;
            foreach (KeyValuePair<string, string> kv in ConstraintMessages)
            {
                // SQL Server đặt tên ràng buộc trong dấu "..." (lỗi 547) hoặc '...' (lỗi 2627/2601).
                // So cả dấu nháy để CK_Invoices_Discount không khớp nhầm CK_Invoices_DiscountReason.
                if (sqlMessage.IndexOf("\"" + kv.Key + "\"", StringComparison.OrdinalIgnoreCase) >= 0 ||
                    sqlMessage.IndexOf("'" + kv.Key + "'", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    mapped = kv.Value;
                    return true;
                }
            }
            return false;
        }

        private static Exception Unwrap(Exception ex)
        {
            while (ex.InnerException != null && (ex is TargetInvocationException || ex is AggregateException))
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        private static SqlException FindSqlException(Exception ex)
        {
            while (ex != null)
            {
                var sql = ex as SqlException;
                if (sql != null) return sql;
                ex = ex.InnerException;
            }
            return null;
        }
    }
}
