// =============================================================================
// Constants.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Gom mọi giá trị "cứng" dùng chung về một chỗ, đúng y như ràng buộc CHECK
//   trong database HotelManagement:
//     - trạng thái phòng, phiếu đặt, lượt lưu trú, hóa đơn, việc dọn phòng;
//     - loại khoản tiền, phương thức thanh toán, vai trò, giới tính;
//     - mã lỗi 50001–50007 do procedure THROW;
//     - các mốc nghiệp vụ (giờ nhận/trả phòng, VAT, giới hạn giảm giá...);
//     - thông tin khách sạn in trên hóa đơn.
//   Mỗi nhóm trạng thái có hàm ToVietnamese() để hiển thị chữ tiếng Việt,
//   riêng trạng thái phòng có thêm ToColor() để tô màu sơ đồ phòng.
//
// QUY TẮC DÙNG
//   - Không gõ tay chuỗi "Available", "CheckedIn"... trong form. Luôn viết
//     RoomStatus.Available, StayStatus.CheckedIn... để tránh gõ sai chính tả
//     (gõ sai thì lọc không ra dữ liệu mà không báo lỗi).
//   - Giá trị ở đây phải khớp file 01_HotelManagement_TaoDB.sql. Đổi CHECK
//     trong DB thì sửa file này cùng lúc.
//   - Các con số trong BusinessRules chỉ để HIỂN THỊ và KIỂM TRA SƠ BỘ trên
//     form. Quy tắc thật nằm trong procedure; DB vẫn là nơi quyết định cuối.
// =============================================================================
using System;
using System.Drawing;

namespace HotelManagement.Core
{
    /// <summary>Vai trò – bảng Roles.RoleName (CK_Roles_RoleName).</summary>
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Manager = "Manager";
        public const string Receptionist = "Receptionist";
        public const string Housekeeping = "Housekeeping";

        public static readonly string[] All = { Admin, Manager, Receptionist, Housekeeping };

        public static string ToVietnamese(string role)
        {
            switch (role)
            {
                case Admin: return "Quản trị";
                case Manager: return "Quản lý";
                case Receptionist: return "Lễ tân";
                case Housekeeping: return "Buồng phòng";
                default: return role ?? "";
            }
        }
    }

    /// <summary>Trạng thái phòng vật lý – Rooms.Status (CK_Rooms_Status). Không có Reserved.</summary>
    public static class RoomStatus
    {
        public const string Available = "Available";
        public const string Occupied = "Occupied";
        public const string Cleaning = "Cleaning";
        public const string Maintenance = "Maintenance";

        public static readonly string[] All = { Available, Occupied, Cleaning, Maintenance };

        public static string ToVietnamese(string status)
        {
            switch (status)
            {
                case Available: return "Trống";
                case Occupied: return "Đang có khách";
                case Cleaning: return "Đang dọn";
                case Maintenance: return "Bảo trì";
                default: return status ?? "";
            }
        }

        /// <summary>Màu nền dùng cho sơ đồ phòng và lưới danh sách phòng.</summary>
        public static Color ToColor(string status)
        {
            switch (status)
            {
                case Available: return Color.FromArgb(198, 239, 206);   // xanh lá nhạt
                case Occupied: return Color.FromArgb(255, 199, 206);    // đỏ nhạt
                case Cleaning: return Color.FromArgb(255, 235, 156);    // vàng nhạt
                case Maintenance: return Color.FromArgb(217, 217, 217); // xám
                default: return Color.Empty;
            }
        }
    }

    /// <summary>Trạng thái phiếu đặt – Bookings.Status (do DB tự tính, form chỉ đọc).</summary>
    public static class BookingStatus
    {
        public const string Pending = "Pending";
        public const string Confirmed = "Confirmed";
        public const string CheckedIn = "CheckedIn";
        public const string CheckedOut = "CheckedOut";
        public const string Cancelled = "Cancelled";

        public static readonly string[] All = { Pending, Confirmed, CheckedIn, CheckedOut, Cancelled };

        public static string ToVietnamese(string status)
        {
            switch (status)
            {
                case Pending: return "Chờ cọc";
                case Confirmed: return "Đã xác nhận";
                case CheckedIn: return "Đang lưu trú";
                case CheckedOut: return "Đã trả phòng";
                case Cancelled: return "Đã hủy";
                default: return status ?? "";
            }
        }
    }

    /// <summary>Trạng thái lượt lưu trú (từng phòng) – BookingRooms.Status.</summary>
    public static class StayStatus
    {
        public const string Booked = "Booked";
        public const string CheckedIn = "CheckedIn";
        public const string CheckedOut = "CheckedOut";
        public const string Cancelled = "Cancelled";

        public static readonly string[] All = { Booked, CheckedIn, CheckedOut, Cancelled };

        public static string ToVietnamese(string status)
        {
            switch (status)
            {
                case Booked: return "Chờ nhận";
                case CheckedIn: return "Đang ở";
                case CheckedOut: return "Đã trả";
                case Cancelled: return "Đã hủy";
                default: return status ?? "";
            }
        }
    }

    /// <summary>Trạng thái hóa đơn – Invoices.Status. Chỉ có Paid và Void.</summary>
    public static class InvoiceStatus
    {
        public const string Paid = "Paid";
        public const string Void = "Void";

        public static readonly string[] All = { Paid, Void };

        public static string ToVietnamese(string status)
        {
            switch (status)
            {
                case Paid: return "Hiệu lực";
                case Void: return "Đã hủy (void)";
                default: return status ?? "";
            }
        }
    }

    /// <summary>Loại khoản tiền – Payments.Type.</summary>
    public static class PaymentType
    {
        public const string Deposit = "Deposit";
        public const string Payment = "Payment";
        public const string Refund = "Refund";

        public static readonly string[] All = { Deposit, Payment, Refund };

        public static string ToVietnamese(string type)
        {
            switch (type)
            {
                case Deposit: return "Tiền cọc";
                case Payment: return "Thanh toán";
                case Refund: return "Hoàn tiền";
                default: return type ?? "";
            }
        }
    }

    /// <summary>Phương thức thanh toán – Payments.Method.</summary>
    public static class PayMethod
    {
        public const string Cash = "Cash";
        public const string Card = "Card";
        public const string BankTransfer = "BankTransfer";
        public const string EWallet = "EWallet";

        public static readonly string[] All = { Cash, Card, BankTransfer, EWallet };

        public static string ToVietnamese(string method)
        {
            switch (method)
            {
                case Cash: return "Tiền mặt";
                case Card: return "Thẻ";
                case BankTransfer: return "Chuyển khoản";
                case EWallet: return "Ví điện tử";
                default: return method ?? "";
            }
        }

        /// <summary>Chuyển khoản và ví điện tử bắt buộc có mã giao dịch (CK_Payments_Reference).</summary>
        public static bool NeedsReference(string method)
        {
            return method == BankTransfer || method == EWallet;
        }
    }

    /// <summary>Trạng thái việc dọn phòng – HousekeepingTasks.Status.</summary>
    public static class HousekeepingStatus
    {
        public const string Pending = "Pending";
        public const string InProgress = "InProgress";
        public const string Done = "Done";

        public static readonly string[] All = { Pending, InProgress, Done };

        public static string ToVietnamese(string status)
        {
            switch (status)
            {
                case Pending: return "Chờ dọn";
                case InProgress: return "Đang dọn";
                case Done: return "Hoàn tất";
                default: return status ?? "";
            }
        }
    }

    /// <summary>Kết quả dọn phòng – HousekeepingTasks.Result.</summary>
    public static class HousekeepingResult
    {
        public const string Clean = "Clean";
        public const string NeedMaintenance = "NeedMaintenance";

        public static string ToVietnamese(string result)
        {
            switch (result)
            {
                case Clean: return "Sạch";
                case NeedMaintenance: return "Cần bảo trì";
                default: return result ?? "";
            }
        }
    }

    /// <summary>Nguồn phát sinh việc dọn phòng – HousekeepingTasks.Source.</summary>
    public static class HousekeepingSource
    {
        public const string CheckOut = "CheckOut";
        public const string AfterMaintenance = "AfterMaintenance";

        public static string ToVietnamese(string source)
        {
            switch (source)
            {
                case CheckOut: return "Sau trả phòng";
                case AfterMaintenance: return "Sau bảo trì";
                default: return source ?? "";
            }
        }
    }

    /// <summary>Giới tính – Customers.Gender (giá trị lưu trong DB là tiếng Việt).</summary>
    public static class Genders
    {
        public const string Male = "Nam";
        public const string Female = "Nữ";
        public const string Other = "Khác";

        public static readonly string[] All = { Male, Female, Other };
    }

    /// <summary>Tỷ lệ cọc cho phép – Bookings.DepositRate.</summary>
    public static class DepositRates
    {
        public const byte Half = 50;
        public const byte Full = 100;

        public static readonly byte[] All = { Half, Full };
    }

    /// <summary>Chế độ mở hộp thoại dlgPayment.</summary>
    public enum PaymentMode
    {
        /// <summary>Thu cọc cho cả phiếu (id = BookingId) – sp_RecordBookingDeposit.</summary>
        Deposit,
        /// <summary>Thu tiền khi đang ở (id = BookingRoomId) – sp_RecordPayment Type=Payment.</summary>
        StayPayment,
        /// <summary>Hoàn tiền thủ công (id = BookingRoomId) – sp_RecordPayment Type=Refund, chỉ A/M.</summary>
        Refund
    }

    /// <summary>Mã lỗi nghiệp vụ do procedure THROW (xem ErrorHandler).</summary>
    public static class SqlErrorCodes
    {
        public const int Permission = 50001;       // không có quyền
        public const int NotFound = 50002;         // không tìm thấy
        public const int InvalidState = 50003;     // sai trạng thái
        public const int InvalidData = 50004;      // dữ liệu không hợp lệ
        public const int ScheduleConflict = 50005; // trùng lịch
        public const int Money = 50006;            // lỗi số tiền
        public const int DateTimeRule = 50007;     // lỗi ngày giờ

        public static bool IsBusiness(int number)
        {
            return number >= Permission && number <= DateTimeRule;
        }
    }

    /// <summary>
    /// Các mốc nghiệp vụ theo tài liệu thống nhất. Chỉ dùng để hiển thị, gợi ý
    /// và kiểm tra sơ bộ; procedure mới là nơi chặn thật.
    /// </summary>
    public static class BusinessRules
    {
        public static readonly TimeSpan CheckInTime = new TimeSpan(14, 0, 0);
        public static readonly TimeSpan CheckOutTime = new TimeSpan(12, 0, 0);
        /// <summary>Sau giờ này của ngày trả phải gia hạn thêm 1 đêm mới check-out được.</summary>
        public static readonly TimeSpan LateCheckOutCutoff = new TimeSpan(18, 0, 0);

        /// <summary>Trễ tối đa bấy nhiêu phút thì miễn phụ thu.</summary>
        public const int LateFreeMinutes = 30;
        public const decimal DefaultTaxRate = 10m;
        /// <summary>Lễ tân giảm tối đa bấy nhiêu % trên (tiền phòng + dịch vụ).</summary>
        public const decimal ReceptionistMaxDiscountPercent = 10m;
        /// <summary>Số lượng dịch vụ lớn hơn mức này thì hỏi xác nhận.</summary>
        public const int ServiceQuantityWarning = 20;
        /// <summary>Phiếu Pending quá bấy nhiêu giờ thì tự hủy.</summary>
        public const int PendingExpireHours = 24;
        /// <summary>Hủy trước giờ nhận phòng ít nhất bấy nhiêu giờ thì được hoàn cọc.</summary>
        public const int FreeCancelHours = 48;
        /// <summary>Bảng tính check-out quá bấy nhiêu phút thì phải xem trước lại.</summary>
        public const int CheckOutQuoteValidMinutes = 10;
        public const int MinPasswordLength = 6;
    }

    /// <summary>Thông tin khách sạn in trên hóa đơn. Nhóm sửa theo ý.</summary>
    public static class HotelInfo
    {
        public const string Name = "KHÁCH SẠN DEMO";
        public const string Address = "123 Đường ABC, Quận 1, TP. Hồ Chí Minh";
        public const string Phone = "028 1234 5678";
        public const string TaxCode = "0123456789";
    }
}
