// =============================================================================
// Lookups.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Nạp dữ liệu cho ComboBox theo một kiểu chung, để các form không phải tự viết
//   lại câu SELECT và cách gán DataSource:
//     - Từ DB: vai trò, loại phòng, dịch vụ đang bán, nhóm dịch vụ, tầng, nhân viên.
//     - Danh sách cố định (không cần DB): trạng thái phòng / phiếu / lượt lưu trú /
//       hóa đơn / việc dọn, loại tiền, phương thức thanh toán, giới tính, tỷ lệ cọc.
//   Mọi ComboBox đều dùng ComboItem (Value = giá trị lưu DB, Text = chữ hiển thị),
//   nên cách đọc giá trị đã chọn luôn giống nhau: Lookups.SelectedInt(cbo),
//   Lookups.SelectedString(cbo); chọn sẵn một giá trị: Lookups.SelectValue(cbo, x).
//
// CÁCH DÙNG
//   Lookups.LoadRoomTypes(cboRoomType, true, Lookups.AllText);     // có dòng "-- Tất cả --"
//   Lookups.LoadPaymentMethods(cboMethod);                          // không có dòng trống
//   int? typeId = Lookups.SelectedInt(cboRoomType);                 // null nếu chọn "Tất cả"
//   string method = Lookups.SelectedString(cboMethod);              // "Cash", "Card"...
//
// LƯU Ý
//   Gán DataSource sẽ bắn sự kiện SelectedIndexChanged. Nếu form lọc lưới trong
//   sự kiện đó, hãy dùng một cờ _loading = true trong lúc nạp ComboBox.
// =============================================================================
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace HotelManagement.Core
{
    /// <summary>Một dòng trong ComboBox: giá trị lưu DB + chữ hiển thị.</summary>
    public class ComboItem
    {
        public ComboItem(object value, string text)
        {
            Value = value;
            Text = text;
        }

        public object Value { get; private set; }
        public string Text { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public static class Lookups
    {
        public const string AllText = "-- Tất cả --";
        public const string ChooseText = "-- Chọn --";

        // ------------------------------------------------------------------
        // Gán dữ liệu
        // ------------------------------------------------------------------

        /// <summary>
        /// Gán danh sách cho ComboBox. emptyText != null thì thêm một dòng đầu có
        /// Value = null (dùng cho "-- Tất cả --" hoặc "-- Chọn --").
        /// </summary>
        public static void Bind(ComboBox cbo, IEnumerable<ComboItem> items, string emptyText = null)
        {
            if (cbo == null) throw new ArgumentNullException("cbo");
            var list = new List<ComboItem>();
            if (emptyText != null) list.Add(new ComboItem(null, emptyText));
            if (items != null) list.AddRange(items);

            cbo.DataSource = null;
            cbo.DisplayMember = "Text";
            cbo.ValueMember = "Value";
            cbo.DataSource = list;
            cbo.SelectedIndex = list.Count > 0 ? 0 : -1;
        }

        /// <summary>Đổi một DataTable thành danh sách ComboItem.</summary>
        public static List<ComboItem> FromTable(DataTable dt, string valueColumn, string textColumn)
        {
            var list = new List<ComboItem>();
            if (dt == null) return list;
            foreach (DataRow r in dt.Rows)
            {
                list.Add(new ComboItem(r[valueColumn], Convert.ToString(r[textColumn], CultureInfo.CurrentCulture)));
            }
            return list;
        }

        // ------------------------------------------------------------------
        // Đọc / chọn giá trị
        // ------------------------------------------------------------------

        /// <summary>Giá trị đang chọn dạng int; null nếu chưa chọn hoặc chọn dòng trống.</summary>
        public static int? SelectedInt(ComboBox cbo)
        {
            object v = SelectedRaw(cbo);
            if (v == null) return null;
            return Convert.ToInt32(v, CultureInfo.InvariantCulture);
        }

        /// <summary>Giá trị đang chọn dạng chuỗi; null nếu chưa chọn hoặc chọn dòng trống.</summary>
        public static string SelectedString(ComboBox cbo)
        {
            object v = SelectedRaw(cbo);
            return v == null ? null : Convert.ToString(v, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Chọn dòng có Value bằng value (so theo chuỗi nên int/byte/long đều khớp).
        /// value = null thì chọn dòng đầu nếu đó là dòng trống, ngược lại bỏ chọn.
        /// </summary>
        public static void SelectValue(ComboBox cbo, object value)
        {
            if (cbo == null) return;
            string wanted = (value == null || value == DBNull.Value)
                ? null
                : Convert.ToString(value, CultureInfo.InvariantCulture);

            for (int i = 0; i < cbo.Items.Count; i++)
            {
                var item = cbo.Items[i] as ComboItem;
                if (item == null) continue;
                string v = item.Value == null ? null : Convert.ToString(item.Value, CultureInfo.InvariantCulture);
                if (string.Equals(v, wanted, StringComparison.OrdinalIgnoreCase))
                {
                    cbo.SelectedIndex = i;
                    return;
                }
            }
            cbo.SelectedIndex = -1;
        }

        private static object SelectedRaw(ComboBox cbo)
        {
            if (cbo == null) return null;
            var item = cbo.SelectedItem as ComboItem;
            if (item == null) return null;
            return item.Value == DBNull.Value ? null : item.Value;
        }

        // ------------------------------------------------------------------
        // Nạp từ database
        // ------------------------------------------------------------------

        /// <summary>4 vai trò, hiển thị "Lễ tân (Receptionist)".</summary>
        public static void LoadRoles(ComboBox cbo, string emptyText = null)
        {
            DataTable dt = DbHelper.QueryText("SELECT RoleId, RoleName FROM dbo.Roles ORDER BY RoleId");
            var items = new List<ComboItem>();
            foreach (DataRow r in dt.Rows)
            {
                string name = DbHelper.Value<string>(r, "RoleName");
                items.Add(new ComboItem(r["RoleId"], Roles.ToVietnamese(name) + " (" + name + ")"));
            }
            Bind(cbo, items, emptyText);
        }

        /// <summary>Loại phòng. activeOnly = true: chỉ loại đang dùng (khi thêm phòng, đặt phòng).</summary>
        public static void LoadRoomTypes(ComboBox cbo, bool activeOnly = true, string emptyText = null)
        {
            DataTable dt = DbHelper.QueryText(
                "SELECT RoomTypeId, TypeName FROM dbo.RoomTypes " +
                "WHERE (@ActiveOnly = 0 OR IsActive = 1) ORDER BY BasePrice, TypeName",
                DbHelper.P("@ActiveOnly", activeOnly));
            Bind(cbo, FromTable(dt, "RoomTypeId", "TypeName"), emptyText);
        }

        /// <summary>
        /// Dịch vụ, hiển thị "Nước suối – 15.000 đ/chai".
        /// includeSystem = false: bỏ dịch vụ hệ thống (Phụ thu trả phòng trễ) – dùng khi ghi dịch vụ.
        /// </summary>
        public static void LoadServices(ComboBox cbo, bool activeOnly = true, bool includeSystem = false,
                                        string emptyText = null)
        {
            DataTable dt = DbHelper.QueryText(
                "SELECT ServiceId, ServiceName, Unit, Price FROM dbo.Services " +
                "WHERE (@ActiveOnly = 0 OR IsActive = 1) AND (@IncludeSystem = 1 OR IsSystem = 0) " +
                "ORDER BY Category, ServiceName",
                DbHelper.P("@ActiveOnly", activeOnly),
                DbHelper.P("@IncludeSystem", includeSystem));
            var items = new List<ComboItem>();
            foreach (DataRow r in dt.Rows)
            {
                string text = DbHelper.Value<string>(r, "ServiceName") + " – " +
                              Formatters.Money(DbHelper.Value<decimal>(r, "Price")) + "/" +
                              DbHelper.Value<string>(r, "Unit");
                items.Add(new ComboItem(r["ServiceId"], text));
            }
            Bind(cbo, items, emptyText);
        }

        /// <summary>Các nhóm dịch vụ đang có (Services.Category).</summary>
        public static void LoadServiceCategories(ComboBox cbo, string emptyText = null)
        {
            DataTable dt = DbHelper.QueryText(
                "SELECT DISTINCT Category FROM dbo.Services WHERE Category IS NOT NULL ORDER BY Category");
            Bind(cbo, FromTable(dt, "Category", "Category"), emptyText);
        }

        /// <summary>Các tầng đang có phòng, hiển thị "Tầng 3".</summary>
        public static void LoadFloors(ComboBox cbo, string emptyText = null)
        {
            DataTable dt = DbHelper.QueryText("SELECT DISTINCT Floor FROM dbo.Rooms ORDER BY Floor");
            var items = new List<ComboItem>();
            foreach (DataRow r in dt.Rows)
            {
                items.Add(new ComboItem(r["Floor"], "Tầng " + r["Floor"]));
            }
            Bind(cbo, items, emptyText);
        }

        /// <summary>Nhân viên; role = null là mọi vai trò (vd. lọc sổ thu theo người thu).</summary>
        public static void LoadEmployees(ComboBox cbo, string role = null, bool activeOnly = true,
                                         string emptyText = null)
        {
            DataTable dt = DbHelper.QueryText(
                "SELECT e.EmployeeId, e.FullName FROM dbo.Employees e " +
                "JOIN dbo.Roles r ON r.RoleId = e.RoleId " +
                "WHERE (@Role IS NULL OR r.RoleName = @Role) AND (@ActiveOnly = 0 OR e.IsActive = 1) " +
                "ORDER BY e.FullName",
                DbHelper.P("@Role", role),
                DbHelper.P("@ActiveOnly", activeOnly));
            Bind(cbo, FromTable(dt, "EmployeeId", "FullName"), emptyText);
        }

        // ------------------------------------------------------------------
        // Danh sách cố định (khớp CHECK trong DB, không cần truy vấn)
        // ------------------------------------------------------------------

        public static void LoadRoomStatuses(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(RoomStatus.All, RoomStatus.ToVietnamese), emptyText);
        }

        public static void LoadBookingStatuses(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(BookingStatus.All, BookingStatus.ToVietnamese), emptyText);
        }

        public static void LoadStayStatuses(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(StayStatus.All, StayStatus.ToVietnamese), emptyText);
        }

        public static void LoadInvoiceStatuses(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(InvoiceStatus.All, InvoiceStatus.ToVietnamese), emptyText);
        }

        public static void LoadHousekeepingStatuses(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(HousekeepingStatus.All, HousekeepingStatus.ToVietnamese), emptyText);
        }

        public static void LoadPaymentTypes(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(PaymentType.All, PaymentType.ToVietnamese), emptyText);
        }

        public static void LoadPaymentMethods(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(PayMethod.All, PayMethod.ToVietnamese), emptyText);
        }

        public static void LoadGenders(ComboBox cbo, string emptyText = null)
        {
            Bind(cbo, FromCodes(Genders.All, null), emptyText);
        }

        /// <summary>Tỷ lệ cọc 50 / 100, hiển thị "Cọc 50%".</summary>
        public static void LoadDepositRates(ComboBox cbo)
        {
            var items = new List<ComboItem>();
            foreach (byte rate in DepositRates.All)
            {
                items.Add(new ComboItem(rate, "Cọc " + rate + "%"));
            }
            Bind(cbo, items);
        }

        private static List<ComboItem> FromCodes(IEnumerable<string> codes, Func<string, string> toText)
        {
            var list = new List<ComboItem>();
            foreach (string code in codes)
            {
                list.Add(new ComboItem(code, toText == null ? code : toText(code)));
            }
            return list;
        }
    }
}
