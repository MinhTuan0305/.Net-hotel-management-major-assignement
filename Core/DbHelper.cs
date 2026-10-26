// =============================================================================
// DbHelper.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Là cửa DUY NHẤT để chương trình nói chuyện với SQL Server. Form không tự
//   tạo SqlConnection / SqlCommand mà gọi các hàm ở đây:
//     Query      – gọi procedure trả về 1 bảng          (vd. sp_PreviewCheckOut)
//     QueryText  – chạy câu SELECT từ view / function   (vd. vw_InHouseStays)
//     QueryMulti – gọi procedure trả về nhiều bảng       (vd. sp_RevenueReport)
//     Exec       – gọi procedure không cần lấy bảng về   (vd. sp_CheckInRoom)
//     ExecText   – chạy INSERT/UPDATE/DELETE cho danh mục (vd. Amenities)
//     Scalar     – lấy 1 giá trị                         (vd. COUNT(*))
//     InTransaction + ExecTextTx – nhiều câu lệnh trong 1 transaction
//   Kèm các hàm tạo tham số: P (thường), Out (OUTPUT), Tvp (tham số bảng
//   dbo.BookingRoomInput) và Value<T> để đọc giá trị an toàn khi gặp NULL.
//
// CÁCH HOẠT ĐỘNG
//   - Chuỗi kết nối đọc từ App.config -> connectionStrings.config, tên "HotelDb".
//     Mỗi máy có file connectionStrings.config riêng (không đưa lên Git).
//   - Mỗi lần gọi mở kết nối mới và đóng ngay (ADO.NET có connection pool nên
//     không chậm). Không giữ kết nối dùng chung toàn chương trình.
//   - Không bắt lỗi ở đây: lỗi SQL được ném ra để form bắt và đưa cho
//     ErrorHandler hiển thị (vd. THROW 50005 "trùng lịch").
//   - Các procedure tự mở transaction và tự kiểm tra quyền, nên form KHÔNG cần
//     SqlTransaction khi gọi procedure.
//
// VÍ DỤ
//   DbHelper.Exec("dbo.sp_CheckInRoom",
//       DbHelper.P("@BookingRoomId", id),
//       DbHelper.P("@GuestName", txtGuestName.Text),
//       DbHelper.P("@GuestIdNumber", txtGuestId.Text),
//       Session.EmployeeParam());
//
//   DataTable dt = DbHelper.QueryText(
//       "SELECT * FROM dbo.fn_AvailableRooms(@In, @Out, NULL) ORDER BY RoomNumber",
//       DbHelper.P("@In", dtpIn.Value.Date), DbHelper.P("@Out", dtpOut.Value.Date));
//
//   SqlParameter invoiceId = DbHelper.Out("@InvoiceId", SqlDbType.Int);
//   DbHelper.Exec("dbo.sp_CheckOutRoom", ..., invoiceId);
//   int newId = DbHelper.OutValue<int>(invoiceId);
// =============================================================================
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace HotelManagement.Core
{
    public static class DbHelper
    {
        /// <summary>Tên chuỗi kết nối trong connectionStrings.config.</summary>
        public const string ConnectionName = "HotelDb";

        /// <summary>Thời gian chờ tối đa của một câu lệnh (giây).</summary>
        public static int CommandTimeoutSeconds = 30;

        // ------------------------------------------------------------------
        // Kết nối
        // ------------------------------------------------------------------

        public static string ConnectionString
        {
            get
            {
                ConnectionStringSettings cs = ConfigurationManager.ConnectionStrings[ConnectionName];
                if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
                {
                    throw new ConfigurationErrorsException(
                        "Chưa có chuỗi kết nối '" + ConnectionName + "'. Hãy chép file " +
                        "connectionStrings.example.config thành connectionStrings.config và sửa tên server.");
                }
                return cs.ConnectionString;
            }
        }

        /// <summary>Mở một kết nối mới. Người gọi phải đặt trong using.</summary>
        public static SqlConnection OpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>Thử kết nối khi khởi động chương trình. Trả về false kèm thông báo dễ hiểu.</summary>
        public static bool TestConnection(out string error)
        {
            try
            {
                using (SqlConnection conn = OpenConnection())
                using (var cmd = new SqlCommand("SELECT COUNT(*) FROM dbo.Roles", conn))
                {
                    cmd.CommandTimeout = CommandTimeoutSeconds;
                    cmd.ExecuteScalar();
                }
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                string title;
                System.Windows.Forms.MessageBoxIcon icon;
                error = ErrorHandler.GetMessage(ex, out title, out icon);
                return false;
            }
        }

        // ------------------------------------------------------------------
        // Đọc dữ liệu
        // ------------------------------------------------------------------

        /// <summary>Gọi procedure trả về một bảng.</summary>
        public static DataTable Query(string procName, params SqlParameter[] ps)
        {
            return FillTable(procName, CommandType.StoredProcedure, ps);
        }

        /// <summary>Chạy câu SELECT (từ view, function...) có tham số.</summary>
        public static DataTable QueryText(string sql, params SqlParameter[] ps)
        {
            return FillTable(sql, CommandType.Text, ps);
        }

        /// <summary>
        /// Gọi procedure trả về nhiều bảng. ds.Tables[0] là bảng thứ nhất,
        /// ds.Tables[1] là bảng thứ hai... (vd. sp_RevenueReport).
        /// </summary>
        public static DataSet QueryMulti(string procName, params SqlParameter[] ps)
        {
            using (SqlConnection conn = OpenConnection())
            using (SqlCommand cmd = CreateCommand(conn, null, procName, CommandType.StoredProcedure, ps))
            using (var da = new SqlDataAdapter(cmd))
            {
                var ds = new DataSet();
                try
                {
                    da.Fill(ds);
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
                return ds;
            }
        }

        /// <summary>Lấy một giá trị (ô đầu tiên của dòng đầu tiên). NULL trả về null.</summary>
        public static object Scalar(string sql, params SqlParameter[] ps)
        {
            using (SqlConnection conn = OpenConnection())
            using (SqlCommand cmd = CreateCommand(conn, null, sql, CommandType.Text, ps))
            {
                try
                {
                    object v = cmd.ExecuteScalar();
                    return v == DBNull.Value ? null : v;
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
            }
        }

        /// <summary>Lấy một giá trị và đổi sang kiểu T. NULL trả về defaultValue.</summary>
        public static T Scalar<T>(string sql, params SqlParameter[] ps)
        {
            return ConvertValue<T>(Scalar(sql, ps), default(T));
        }

        // ------------------------------------------------------------------
        // Ghi dữ liệu
        // ------------------------------------------------------------------

        /// <summary>
        /// Gọi procedure không trả bảng. Giá trị trả về là số dòng bị ảnh hưởng
        /// (procedure có SET NOCOUNT ON sẽ trả -1, không cần quan tâm).
        /// Muốn lấy id mới thì truyền tham số Out(...) rồi đọc bằng OutValue.
        /// </summary>
        public static int Exec(string procName, params SqlParameter[] ps)
        {
            return NonQuery(procName, CommandType.StoredProcedure, ps);
        }

        /// <summary>Chạy INSERT/UPDATE/DELETE (dùng cho các form danh mục).</summary>
        public static int ExecText(string sql, params SqlParameter[] ps)
        {
            return NonQuery(sql, CommandType.Text, ps);
        }

        /// <summary>
        /// Chạy nhiều câu lệnh trong cùng một transaction; lỗi ở bất kỳ câu nào
        /// thì hủy toàn bộ. Ví dụ lưu tiện nghi của loại phòng:
        ///   DbHelper.InTransaction((conn, tx) =>
        ///   {
        ///       DbHelper.ExecTextTx(conn, tx, "DELETE FROM dbo.RoomTypeAmenities WHERE RoomTypeId = @Id", DbHelper.P("@Id", id));
        ///       foreach (int a in amenityIds)
        ///           DbHelper.ExecTextTx(conn, tx, "INSERT INTO dbo.RoomTypeAmenities (RoomTypeId, AmenityId) VALUES (@Id, @A)",
        ///               DbHelper.P("@Id", id), DbHelper.P("@A", a));
        ///   });
        /// </summary>
        public static void InTransaction(Action<SqlConnection, SqlTransaction> work)
        {
            if (work == null) throw new ArgumentNullException("work");
            using (SqlConnection conn = OpenConnection())
            using (SqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    work(conn, tx);
                    tx.Commit();
                }
                catch
                {
                    try { tx.Rollback(); } catch { /* transaction có thể đã bị SQL Server hủy */ }
                    throw;
                }
            }
        }

        /// <summary>Chạy một câu lệnh bên trong InTransaction.</summary>
        public static int ExecTextTx(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = CreateCommand(conn, tx, sql, CommandType.Text, ps))
            {
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
            }
        }

        /// <summary>
        /// Lấy một giá trị bên trong InTransaction (vd. id vừa INSERT bằng SELECT SCOPE_IDENTITY()).
        /// NULL trả về defaultValue.
        /// </summary>
        public static T ScalarTx<T>(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] ps)
        {
            using (SqlCommand cmd = CreateCommand(conn, tx, sql, CommandType.Text, ps))
            {
                try
                {
                    return ConvertValue<T>(cmd.ExecuteScalar(), default(T));
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
            }
        }

        // ------------------------------------------------------------------
        // Tạo tham số
        // ------------------------------------------------------------------

        /// <summary>
        /// Tham số thường. null -> NULL. Chuỗi được cắt khoảng trắng hai đầu;
        /// chuỗi rỗng -> NULL (để ô bỏ trống trên form thành NULL trong DB,
        /// procedure sẽ báo "phải nhập..." nếu trường đó bắt buộc).
        /// </summary>
        public static SqlParameter P(string name, object value)
        {
            object v = value;
            string s = value as string;
            if (s != null)
            {
                s = s.Trim();
                v = s.Length == 0 ? null : s;
            }
            return new SqlParameter(name, v ?? DBNull.Value);
        }

        /// <summary>Tham số có chỉ định kiểu (khi cần ép kiểu rõ, vd. SqlDbType.Date).</summary>
        public static SqlParameter P(string name, SqlDbType type, object value)
        {
            SqlParameter p = P(name, value);
            p.SqlDbType = type;
            return p;
        }

        /// <summary>
        /// Tham số OUTPUT (vd. @BookingId, @InvoiceId, @UsageId, @PaymentId,
        /// @RefundAmount, @TotalRefund). Gọi xong đọc bằng OutValue&lt;T&gt;(p).
        /// </summary>
        public static SqlParameter Out(string name, SqlDbType type)
        {
            var p = new SqlParameter(name, type) { Direction = ParameterDirection.Output };
            switch (type)
            {
                case SqlDbType.Decimal:
                    p.Precision = 14;   // tiền trong DB là DECIMAL(14,0)
                    p.Scale = 0;
                    break;
                case SqlDbType.NVarChar:
                case SqlDbType.VarChar:
                    p.Size = 4000;
                    break;
            }
            return p;
        }

        /// <summary>
        /// Chuỗi tìm kiếm "chứa" cho LIKE: thoát ký tự đặc biệt [ % _ rồi bọc %...%.
        /// Ô trống trả null, dùng với điều kiện "@Kw IS NULL OR ...":
        ///   "WHERE (@Kw IS NULL OR FullName COLLATE Vietnamese_CI_AI LIKE @Kw)",
        ///   DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text))
        /// </summary>
        public static string LikeContains(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return null;
            string s = text.Trim().Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]");
            return "%" + s + "%";
        }

        /// <summary>Tham số kiểu bảng (table-valued parameter), vd. @Rooms dbo.BookingRoomInput.</summary>
        public static SqlParameter Tvp(string name, string typeName, DataTable rows)
        {
            return new SqlParameter(name, SqlDbType.Structured)
            {
                TypeName = typeName,
                Value = rows
            };
        }

        /// <summary>
        /// Tạo DataTable đúng cấu trúc và ĐÚNG THỨ TỰ CỘT của type dbo.BookingRoomInput
        /// (SQL Server ghép cột theo thứ tự, không theo tên). Dùng cho sp_CreateBooking:
        ///   DataTable rooms = DbHelper.CreateBookingRoomInput();
        ///   DbHelper.AddBookingRoom(rooms, roomId, dtpIn.Value, dtpOut.Value, 2, 0, null, null);
        ///   SqlParameter bookingId = DbHelper.Out("@BookingId", SqlDbType.Int);
        ///   DbHelper.Exec("dbo.sp_CreateBooking", DbHelper.P("@CustomerId", customerId),
        ///       DbHelper.P("@DepositRate", (byte)50), DbHelper.P("@Note", txtNote.Text),
        ///       Session.EmployeeParam(), DbHelper.Tvp("@Rooms", "dbo.BookingRoomInput", rooms), bookingId);
        /// </summary>
        public static DataTable CreateBookingRoomInput()
        {
            var dt = new DataTable("BookingRoomInput");
            dt.Columns.Add("RoomId", typeof(int));
            dt.Columns.Add("CheckInDate", typeof(DateTime));
            dt.Columns.Add("CheckOutDate", typeof(DateTime));
            dt.Columns.Add("Adults", typeof(byte));
            dt.Columns.Add("Children", typeof(byte));
            dt.Columns.Add("GuestName", typeof(string));
            dt.Columns.Add("GuestIdNumber", typeof(string));
            return dt;
        }

        /// <summary>Thêm một phòng vào bảng tạo bởi CreateBookingRoomInput.</summary>
        public static void AddBookingRoom(DataTable rooms, int roomId, DateTime checkInDate, DateTime checkOutDate,
                                          int adults, int children, string guestName, string guestIdNumber)
        {
            if (rooms == null) throw new ArgumentNullException("rooms");
            rooms.Rows.Add(roomId, checkInDate.Date, checkOutDate.Date,
                           (byte)adults, (byte)children,
                           NullIfEmpty(guestName), NullIfEmpty(guestIdNumber));
        }

        // ------------------------------------------------------------------
        // Đọc giá trị an toàn
        // ------------------------------------------------------------------

        /// <summary>
        /// Đọc một ô của DataRow và đổi sang kiểu T; ô NULL trả về defaultValue.
        ///   int id        = DbHelper.Value&lt;int&gt;(row, "BookingRoomId");
        ///   decimal? paid = DbHelper.Value&lt;decimal?&gt;(row, "PaidAmount");
        ///   string note   = DbHelper.Value&lt;string&gt;(row, "Note", "");
        /// </summary>
        public static T Value<T>(DataRow row, string column, T defaultValue = default(T))
        {
            if (row == null) throw new ArgumentNullException("row");
            if (!row.Table.Columns.Contains(column))
                throw new ArgumentException("Không có cột '" + column + "' trong kết quả truy vấn.", "column");
            return ConvertValue(row[column], defaultValue);
        }

        /// <summary>Đọc giá trị của tham số OUTPUT sau khi gọi procedure.</summary>
        public static T OutValue<T>(SqlParameter p, T defaultValue = default(T))
        {
            if (p == null) throw new ArgumentNullException("p");
            return ConvertValue(p.Value, defaultValue);
        }

        /// <summary>Đổi một giá trị bất kỳ (có thể là DBNull) sang kiểu T.</summary>
        public static T ConvertValue<T>(object value, T defaultValue = default(T))
        {
            if (value == null || value == DBNull.Value) return defaultValue;
            if (value is T) return (T)value;

            Type target = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (target.IsEnum)
                return (T)Enum.Parse(target, value.ToString(), true);
            return (T)Convert.ChangeType(value, target, CultureInfo.InvariantCulture);
        }

        // ------------------------------------------------------------------
        // Nội bộ
        // ------------------------------------------------------------------

        private static object NullIfEmpty(string s)
        {
            if (s == null) return DBNull.Value;
            s = s.Trim();
            return s.Length == 0 ? (object)DBNull.Value : s;
        }

        private static SqlCommand CreateCommand(SqlConnection conn, SqlTransaction tx, string text,
                                                CommandType type, SqlParameter[] ps)
        {
            var cmd = new SqlCommand(text, conn, tx)
            {
                CommandType = type,
                CommandTimeout = CommandTimeoutSeconds
            };
            if (ps != null)
            {
                foreach (SqlParameter p in ps)
                {
                    if (p != null) cmd.Parameters.Add(p);
                }
            }
            return cmd;
        }

        private static DataTable FillTable(string text, CommandType type, SqlParameter[] ps)
        {
            using (SqlConnection conn = OpenConnection())
            using (SqlCommand cmd = CreateCommand(conn, null, text, type, ps))
            using (var da = new SqlDataAdapter(cmd))
            {
                var dt = new DataTable();
                try
                {
                    da.Fill(dt);
                }
                finally
                {
                    // Gỡ tham số khỏi command để có thể dùng lại cùng đối tượng
                    // SqlParameter ở lần gọi sau (giá trị OUTPUT vẫn đọc được).
                    cmd.Parameters.Clear();
                }
                return dt;
            }
        }

        private static int NonQuery(string text, CommandType type, SqlParameter[] ps)
        {
            using (SqlConnection conn = OpenConnection())
            using (SqlCommand cmd = CreateCommand(conn, null, text, type, ps))
            {
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                finally
                {
                    cmd.Parameters.Clear();
                }
            }
        }
    }
}
