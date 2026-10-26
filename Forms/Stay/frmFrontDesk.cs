using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Billing;
using Hotel_management_system.Forms.Booking;

namespace Hotel_management_system.Forms.Stay
{
    /// <summary>
    /// Màn hình lễ tân (A, M, R). 3 tab: Khách đến hôm nay, Đang ở, Dự kiến trả hôm nay.
    /// Form chỉ ĐỌC dữ liệu; mọi thao tác ghi (check-in, dịch vụ, gia hạn, check-out...)
    /// đi qua form/hộp thoại riêng, mở theo BookingRoomId của dòng đang chọn.
    /// </summary>
    public partial class frmFrontDesk : Form
    {
        // Khóa của mọi dòng: một lượt lưu trú của một phòng.
        private const string KeyColumn = "BookingRoomId";

        // Mỗi tab đọc một view, nối thêm vw_RoomStayFolio để có cột tiền.
        // Câu SQL cố định, không chứa dữ liệu người dùng gõ, nên không cần tham số.
        private const string SqlArrivals = @"
SELECT s.*, f.RequiredDeposit, f.NetPaid,
       CAST(CASE WHEN f.RequiredDeposit > f.NetPaid THEN f.RequiredDeposit - f.NetPaid ELSE 0 END AS DECIMAL(14,0)) AS MissingDeposit
FROM dbo.vw_TodayArrivals s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
ORDER BY s.RoomNumber;";

        private const string SqlInHouse = @"
SELECT s.*, f.NetPaid, f.EstimatedTotal, f.EstimatedBalance
FROM dbo.vw_InHouseStays s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
ORDER BY s.RoomNumber;";

        // LateMinutes: số phút trễ so với 12:00 ngày trả, tính bằng giờ của SQL Server
        // (cùng công thức với dbo.fn_LateCheckoutFee). Âm nghĩa là chưa đến giờ trả.
        private const string SqlDepartures = @"
SELECT s.*, f.NetPaid, f.EstimatedTotal, f.EstimatedBalance,
       DATEDIFF(MINUTE, DATEADD(HOUR, 12, CAST(s.CheckOutDate AS DATETIME2(0))), SYSDATETIME()) AS LateMinutes
FROM dbo.vw_TodayDepartures s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
ORDER BY s.CheckOutDate, s.RoomNumber;";

        private static readonly Color PendingColor = Color.FromArgb(255, 224, 178); // cam nhạt: phiếu chưa đủ cọc
        private static readonly Color LateColor = Color.FromArgb(255, 170, 170);    // đỏ: đã quá giờ trả phòng

        // Dữ liệu đang hiển thị của 3 tab (giữ lại để lọc bằng ô tìm mà không phải gọi DB).
        private DataTable _arrivals;
        private DataTable _inHouse;
        private DataTable _departures;

        // Đã hẹn nạp lại hay chưa (gộp nhiều thông báo DataChanged liên tiếp thành một lần nạp).
        private bool _reloadPending;

        public frmFrontDesk()
        {
            InitializeComponent();
            SetupGrids();
        }

        // ==================================================================
        // Khởi tạo
        // ==================================================================

        /// <summary>Kiểu lưới, dịch trạng thái, tô màu. Chỉ gọi MỘT lần (các hàm này gắn sự kiện).</summary>
        private void SetupGrids()
        {
            UiHelper.StyleGrid(dgvArrivals);
            UiHelper.StyleGrid(dgvInHouse);
            UiHelper.StyleGrid(dgvDepartures);

            // Tab Khách đến: phiếu chưa đủ cọc tô cam cả dòng.
            UiHelper.TranslateColumn(dgvArrivals, "BookingStatus", BookingStatus.ToVietnamese);
            UiHelper.TranslateColumn(dgvArrivals, "RoomStatus", RoomStatus.ToVietnamese);
            UiHelper.ColorRows(dgvArrivals, "BookingStatus", ColorByBookingStatus);
            // Gắn SAU ColorRows để màu ô "Tình trạng phòng" không bị màu cả dòng đè lên.
            dgvArrivals.CellFormatting += dgvArrivals_CellFormatting;

            // Tab Dự kiến trả: hiện số phút trễ dạng "1 giờ 30 phút" và tô đỏ dòng đã quá 12:00.
            UiHelper.TranslateColumn(dgvDepartures, "LateMinutes", FormatLate);
            UiHelper.ColorRows(dgvDepartures, "LateMinutes", ColorByLate);
        }

        private void frmFrontDesk_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            // Form khác lưu xong (check-in, thu tiền, check-out...) thì 3 tab tự nạp lại.
            FormManager.Subscribe(this, OnDataChanged);
            ShowButtonsForCurrentTab();
            RefreshData();
            timerRefresh.Start();
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        /// <summary>
        /// Nạp lại cả 3 tab. fromTimer = true: lỗi thì chỉ ghi log và báo ở góc trên,
        /// không bật hộp thoại (tránh mỗi phút hiện một thông báo khi mất kết nối).
        /// </summary>
        private void RefreshData(bool fromTimer = false)
        {
            try
            {
                using (fromTimer ? null : UiHelper.Busy(this))
                {
                    LoadAll();
                }
            }
            catch (Exception ex)
            {
                if (!fromTimer)
                {
                    ErrorHandler.Show(ex, this);
                    return;
                }
                ErrorHandler.Log(ex);
                lblUpdated.Text = "Không nạp được dữ liệu lúc " + Formatters.Time(DateTime.Now);
            }
        }

        private void LoadAll()
        {
            // Đọc đủ 3 bảng rồi mới gán lên lưới: một câu lỗi thì lưới vẫn giữ dữ liệu cũ.
            DataTable arrivals = DbHelper.QueryText(SqlArrivals);
            DataTable inHouse = DbHelper.QueryText(SqlInHouse);
            DataTable departures = DbHelper.QueryText(SqlDepartures);

            _arrivals = arrivals;
            _inHouse = inHouse;
            _departures = departures;

            // Lọc theo ô tìm TRƯỚC khi gán, để Bind chọn lại đúng dòng cũ trong số dòng còn hiện.
            string filter = BuildFilter();
            _arrivals.DefaultView.RowFilter = filter;
            _inHouse.DefaultView.RowFilter = filter;
            _departures.DefaultView.RowFilter = filter;

            // Gán DataSource mới thì lưới tạo lại cột, nên phải gọi ShowColumns sau mỗi lần Bind.
            BindKeepScroll(dgvArrivals, _arrivals);
            UiHelper.ShowColumns(dgvArrivals,
                "BookingId", "Mã phiếu",
                "RoomNumber", "Phòng",
                "TypeName", "Loại phòng",
                "GuestName", "Khách",
                "CustomerPhone", "SĐT",
                "CheckInDate", "Ngày nhận",
                "CheckOutDate", "Ngày trả",
                "Nights", "Số đêm",
                "BookingStatus", "Phiếu",
                "RoomStatus", "Tình trạng phòng",
                "RequiredDeposit", "Cọc yêu cầu",
                "NetPaid", "Đã thu",
                "MissingDeposit", "Thiếu cọc");

            BindKeepScroll(dgvInHouse, _inHouse);
            UiHelper.ShowColumns(dgvInHouse,
                "RoomNumber", "Phòng",
                "TypeName", "Loại phòng",
                "GuestName", "Khách",
                "CustomerPhone", "SĐT",
                "BookingId", "Mã phiếu",
                "ActualCheckIn", "Nhận lúc",
                "CheckOutDate", "Ngày trả",
                "Nights", "Số đêm",
                "PricePerNight", "Giá/đêm",
                "NetPaid", "Đã thu",
                "EstimatedTotal", "Tạm tính (VAT)",
                "EstimatedBalance", "Còn lại");

            BindKeepScroll(dgvDepartures, _departures);
            UiHelper.ShowColumns(dgvDepartures,
                "RoomNumber", "Phòng",
                "GuestName", "Khách",
                "CustomerPhone", "SĐT",
                "BookingId", "Mã phiếu",
                "ActualCheckIn", "Nhận lúc",
                "CheckOutDate", "Ngày trả",
                "LateMinutes", "Trễ",
                "NetPaid", "Đã thu",
                "EstimatedTotal", "Tạm tính (VAT)",
                "EstimatedBalance", "Còn lại");

            UpdateTabTitles();
            UpdateButtonsEnabled();
            lblUpdated.Text = "Cập nhật lúc " + Formatters.Time(DateTime.Now);
        }

        /// <summary>UiHelper.Bind (giữ dòng đang chọn) + giữ vị trí cuộn, để tự làm mới không làm nhảy lưới.</summary>
        private static void BindKeepScroll(DataGridView dgv, DataTable data)
        {
            int firstRow = dgv.FirstDisplayedScrollingRowIndex;
            UiHelper.Bind(dgv, data, KeyColumn);
            if (firstRow >= 0 && firstRow < dgv.Rows.Count)
                dgv.FirstDisplayedScrollingRowIndex = firstRow;
        }

        /// <summary>
        /// Có form khác báo dữ liệu đổi. Check-out báo liền 5 vùng, nên hẹn nạp lại bằng
        /// BeginInvoke: mọi thông báo trong cùng một lượt chỉ gây ra MỘT lần nạp.
        /// </summary>
        private void OnDataChanged(string area)
        {
            if (area != DataArea.Stays && area != DataArea.Bookings && area != DataArea.Rooms &&
                area != DataArea.Payments && area != DataArea.Services)
                return;
            RequestReload();
        }

        private void RequestReload()
        {
            if (_reloadPending) return;
            _reloadPending = true;
            BeginInvoke(new Action(() =>
            {
                _reloadPending = false;
                if (!IsDisposed) RefreshData();
            }));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            // Mỗi 60 giây: để dòng "quá 12:00" tự chuyển đỏ và thấy thay đổi từ máy khác.
            RefreshData(fromTimer: true);
        }

        // ==================================================================
        // Tìm kiếm (lọc trên dữ liệu đã nạp, không gọi DB)
        // ==================================================================

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_arrivals == null) return;   // chưa nạp lần nào (vd. lỗi kết nối khi mở form)
            string filter = BuildFilter();
            _arrivals.DefaultView.RowFilter = filter;
            _inHouse.DefaultView.RowFilter = filter;
            _departures.DefaultView.RowFilter = filter;
            UpdateTabTitles();
            UpdateButtonsEnabled();
        }

        /// <summary>
        /// Biểu thức RowFilter lọc cả 3 tab theo số phòng, tên khách, người đặt, SĐT, mã phiếu.
        /// RowFilter không gửi xuống SQL Server, nhưng vẫn phải thoát ký tự đặc biệt
        /// (gõ "O'Neil" mà không thoát dấu ' thì văng lỗi cú pháp).
        /// </summary>
        private string BuildFilter()
        {
            string text = txtSearch.Text.Trim();
            if (text.Length == 0) return "";
            string v = EscapeLike(text);
            return string.Format(
                "RoomNumber LIKE '%{0}%' OR GuestName LIKE '%{0}%' OR CustomerName LIKE '%{0}%'" +
                " OR CustomerPhone LIKE '%{0}%' OR Convert(BookingId, 'System.String') LIKE '%{0}%'", v);
        }

        /// <summary>Thoát chuỗi cho LIKE của RowFilter: ' thành '', còn * % [ ] bọc trong [ ].</summary>
        private static string EscapeLike(string text)
        {
            var sb = new StringBuilder(text.Length);
            foreach (char c in text)
            {
                if (c == '\'') sb.Append("''");
                else if (c == '*' || c == '%' || c == '[' || c == ']') sb.Append('[').Append(c).Append(']');
                else sb.Append(c);
            }
            return sb.ToString();
        }

        /// <summary>Tiêu đề tab kèm số dòng: "Đang ở (4)", khi đang lọc thì "Đang ở (1/4)".</summary>
        private void UpdateTabTitles()
        {
            SetTabTitle(tabArrivals, "Khách đến hôm nay", _arrivals);
            SetTabTitle(tabInHouse, "Đang ở", _inHouse);
            SetTabTitle(tabDepartures, "Dự kiến trả hôm nay", _departures);
        }

        private static void SetTabTitle(TabPage tab, string title, DataTable data)
        {
            if (data == null)
            {
                tab.Text = title;
                return;
            }
            int total = data.Rows.Count;
            int shown = data.DefaultView.Count;
            tab.Text = shown == total
                ? title + " (" + total + ")"
                : title + " (" + shown + "/" + total + ")";
        }

        // ==================================================================
        // Nút theo tab
        // ==================================================================

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowButtonsForCurrentTab();
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            UpdateButtonsEnabled();
        }

        /// <summary>Mỗi tab chỉ hiện các nút dùng được cho các phòng trong tab đó.</summary>
        private void ShowButtonsForCurrentTab()
        {
            bool arrivals = tabMain.SelectedTab == tabArrivals;
            bool inHouse = tabMain.SelectedTab == tabInHouse;
            bool departures = tabMain.SelectedTab == tabDepartures;

            btnCheckIn.Visible = arrivals;
            btnChangeRoom.Visible = arrivals;
            btnDeposit.Visible = arrivals;
            btnCancelBooking.Visible = arrivals;

            btnServices.Visible = inHouse;
            btnPayment.Visible = inHouse;
            btnExtend.Visible = inHouse || departures;
            btnCheckOut.Visible = inHouse || departures;

            UpdateButtonsEnabled();
        }

        /// <summary>Chưa chọn dòng nào thì làm mờ các nút thao tác.</summary>
        private void UpdateButtonsEnabled()
        {
            bool hasRow = UiHelper.SelectedRow(CurrentGrid()) != null;
            UiHelper.SetEnabled(hasRow, btnCheckIn, btnChangeRoom, btnDeposit, btnCancelBooking,
                btnServices, btnPayment, btnExtend, btnCheckOut);
        }

        private DataGridView CurrentGrid()
        {
            if (tabMain.SelectedTab == tabInHouse) return dgvInHouse;
            if (tabMain.SelectedTab == tabDepartures) return dgvDepartures;
            return dgvArrivals;
        }

        /// <summary>Nhấp đúp một dòng = thao tác chính của tab.</summary>
        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;   // nhấp đúp vào tiêu đề cột
            if (sender == dgvArrivals) btnCheckIn_Click(sender, e);
            else if (sender == dgvInHouse) btnServices_Click(sender, e);
            else btnCheckOut_Click(sender, e);
        }

        /// <summary>BookingRoomId của dòng đang chọn; chưa chọn thì nhắc và trả về null.</summary>
        private int? SelectedStayId()
        {
            DataGridView dgv = CurrentGrid();
            if (!UiHelper.RequireSelectedRow(dgv, "một phòng trong danh sách")) return null;
            return UiHelper.SelectedId(dgv, KeyColumn);
        }

        /// <summary>BookingId (mã phiếu) của dòng đang chọn; chưa chọn thì nhắc và trả về null.</summary>
        private int? SelectedBookingId()
        {
            DataGridView dgv = CurrentGrid();
            if (!UiHelper.RequireSelectedRow(dgv, "một phòng trong danh sách")) return null;
            return UiHelper.SelectedId(dgv, "BookingId");
        }

        // ==================================================================
        // Thao tác. Hộp thoại lưu xong tự gọi FormManager.NotifyDataChanged, form này nghe được
        // và tự nạp lại (OnDataChanged); vẫn gọi RequestReload khi trả OK để chắc chắn nạp lại.
        // ==================================================================

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            int? id = SelectedStayId();
            if (id == null) return;
            if (FormManager.ShowDialog(new dlgCheckIn(id.Value), this)) RequestReload();
        }

        private void btnChangeRoom_Click(object sender, EventArgs e)
        {
            int? bookingId = SelectedBookingId();
            if (bookingId == null) return;
            // Mở không chặn (không phải hộp thoại); mỗi phiếu một cửa sổ. Sửa xong form đó tự báo DataChanged.
            int id = bookingId.Value;
            FormManager.Open("booking-" + id, () => new frmBookingEdit(id));
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            int? bookingId = SelectedBookingId();
            if (bookingId == null) return;
            // Cọc tính theo cả phiếu nên truyền BookingId.
            if (FormManager.ShowDialog(new dlgPayment(PaymentMode.Deposit, bookingId.Value), this)) RequestReload();
        }

        private void btnCancelBooking_Click(object sender, EventArgs e)
        {
            int? bookingId = SelectedBookingId();
            int? id = SelectedStayId();
            if (bookingId == null || id == null) return;
            // Truyền cả BookingRoomId để chỉ hủy phòng đang chọn (có ô no-show trong hộp thoại).
            if (FormManager.ShowDialog(new dlgCancel(bookingId.Value, id), this)) RequestReload();
        }

        private void btnServices_Click(object sender, EventArgs e)
        {
            int? id = SelectedStayId();
            if (id == null) return;
            // Form lớn, mỗi phòng một cửa sổ (mở song song được phòng 101 và 205).
            int stayId = id.Value;
            FormManager.Open("stay-" + stayId, () => new frmServiceUsage(stayId));
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            int? id = SelectedStayId();
            if (id == null) return;
            if (FormManager.ShowDialog(new dlgPayment(PaymentMode.StayPayment, id.Value), this)) RequestReload();
        }

        private void btnExtend_Click(object sender, EventArgs e)
        {
            int? id = SelectedStayId();
            if (id == null) return;
            if (FormManager.ShowDialog(new dlgExtendStay(id.Value), this)) RequestReload();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            int? id = SelectedStayId();
            if (id == null) return;
            // Mở dạng hộp thoại để lễ tân làm xong một phòng rồi mới sang phòng khác.
            if (FormManager.ShowDialog(new frmCheckOut(id.Value), this)) RequestReload();
        }

        // ==================================================================
        // Tô màu, hiển thị
        // ==================================================================

        private static Color ColorByBookingStatus(string status)
        {
            return status == BookingStatus.Pending ? PendingColor : Color.Empty;
        }

        private static string FormatLate(string value)
        {
            int minutes;
            if (!int.TryParse(value, out minutes) || minutes <= 0) return "";
            return Formatters.Duration(minutes);
        }

        private static Color ColorByLate(string value)
        {
            int minutes;
            return int.TryParse(value, out minutes) && minutes > 0 ? LateColor : Color.Empty;
        }

        /// <summary>
        /// Tab Khách đến: tô riêng ô "Tình trạng phòng" khi phòng chưa sẵn sàng (đang dọn,
        /// bảo trì, khách cũ chưa trả) để lễ tân biết trước là chưa check-in được.
        /// Đọc giá trị gốc từ dòng dữ liệu, vì e.Value đã bị TranslateColumn đổi sang tiếng Việt.
        /// </summary>
        private void dgvArrivals_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvArrivals.Columns[e.ColumnIndex].Name != "RoomStatus") return;
            var drv = dgvArrivals.Rows[e.RowIndex].DataBoundItem as DataRowView;
            if (drv == null) return;
            string status = drv["RoomStatus"] as string;
            if (status != null && status != RoomStatus.Available)
                e.CellStyle.BackColor = RoomStatus.ToColor(status);
        }
    }
}
