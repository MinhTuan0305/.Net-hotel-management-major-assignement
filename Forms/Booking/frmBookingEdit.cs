using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using HotelManagement.Core;
using Hotel_management_system.Forms.Billing;
using Hotel_management_system.Forms.Catalog;

namespace Hotel_management_system.Forms.Booking
{
    /// <summary>
    /// Tạo / sửa phiếu đặt phòng (Admin, Manager, Receptionist). Đặc tả: KE_HOACH mục 3; nghiệp vụ mục 8.
    ///
    /// TẠO MỚI (bookingId = null):
    ///   chọn khách (frmCustomers chế độ chọn) -> chọn ngày, loại, số người -> tìm phòng trống
    ///   (dbo.fn_AvailableRooms) -> thêm vào giỏ -> "Lưu phiếu" gọi dbo.sp_CreateBooking với tham số
    ///   bảng dbo.BookingRoomInput. Giá mỗi phòng do DB chốt theo RoomTypes.BasePrice lúc lưu.
    ///   Lưu xong hỏi thu cọc ngay (dlgPayment Deposit) rồi mở lại phiếu ở chế độ sửa.
    ///
    /// SỬA (bookingId có giá trị):
    ///   thông tin phiếu chỉ xem (không có procedure sửa đầu phiếu). Chọn một phòng còn "Chờ nhận"
    ///   ở lưới dưới -> đổi ngày / số người / chọn phòng khác ở lưới trên -> dbo.sp_UpdateBookingRoom.
    ///   Tìm phòng thay thế truyền @ExcludeBookingRoomId để phòng hiện tại không tự "trùng lịch" với chính nó.
    ///   "Giữ giá cũ" (khách sạn đổi phòng do sự cố) bắt buộc có lý do.
    ///
    /// Số tiền trên form chỉ là tạm tính; số chính thức (giá, cọc) lấy từ DB.
    /// </summary>
    public partial class frmBookingEdit : Form
    {
        private const string SqlAvailable = @"
SELECT a.RoomId, a.RoomNumber, a.Floor, a.RoomStatus, a.RoomTypeId, a.TypeName, a.BedType, a.MaxOccupancy, a.BasePrice
FROM dbo.fn_AvailableRooms(@In, @Out, @Exclude) a
WHERE (@TypeId IS NULL OR a.RoomTypeId = @TypeId) AND a.MaxOccupancy >= @Guests
ORDER BY a.BasePrice, a.RoomNumber;";

        private const string SqlHeader = @"
SELECT BookingId, CustomerId, CustomerName, CustomerPhone, Status, DepositRate, Note, CreatedAt,
       RequiredDeposit, NetPaid, MissingDeposit, BookedCount
FROM dbo.vw_BookingSummary WHERE BookingId = @Id;";

        private const string SqlStays = @"
SELECT s.BookingRoomId, s.RoomId, s.RoomNumber, s.TypeName, s.CheckInDate, s.CheckOutDate, s.Nights,
       s.Adults, s.Children, s.PricePerNight, CAST(s.PricePerNight * s.Nights AS DECIMAL(14,0)) AS Amount,
       s.StayStatus, s.GuestName, f.RequiredDeposit, f.NetPaid
FROM dbo.vw_RoomStays s
JOIN dbo.vw_RoomStayFolio f ON f.BookingRoomId = s.BookingRoomId
WHERE s.BookingId = @Id
ORDER BY s.RoomNumber;";

        private int? _bookingId;
        private int? _customerId;
        private DataTable _cart;        // giỏ phòng khi tạo mới
        private bool _adjustingDates;
        private bool _reloadPending;

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số. null = tạo phiếu mới.
        public frmBookingEdit(int? bookingId)
        {
            InitializeComponent();
            _bookingId = bookingId;
            UiHelper.StyleGrid(dgvAvailable);
            UiHelper.StyleGrid(dgvRooms);
            UiHelper.TranslateColumn(dgvAvailable, "RoomStatus", RoomStatus.ToVietnamese);
            UiHelper.TranslateColumn(dgvRooms, "StayStatus", StayStatus.ToVietnamese);
            UiHelper.ColorRows(dgvRooms, "StayStatus",
                s => s == StayStatus.Cancelled ? System.Drawing.SystemColors.ControlLight : System.Drawing.Color.Empty);
        }

        private bool IsEditMode
        {
            get { return _bookingId.HasValue; }
        }

        private void frmBookingEdit_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;

            try
            {
                Lookups.LoadDepositRates(cboDepositRate);
                Lookups.LoadRoomTypes(cboRoomType, true, Lookups.AllText);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }

            _adjustingDates = true;
            dtpCheckIn.Value = DateTime.Today;
            dtpCheckOut.Value = DateTime.Today.AddDays(1);
            _adjustingDates = false;
            UpdateNights();

            ApplyMode();
            if (IsEditMode)
            {
                LoadBooking();
                FormManager.Subscribe(this, OnDataChanged);
            }
            else
            {
                CreateCart();
                UpdateTotals();
            }
        }

        /// <summary>Ẩn/hiện control theo chế độ tạo mới / sửa.</summary>
        private void ApplyMode()
        {
            bool edit = IsEditMode;
            btnPickCustomer.Visible = !edit;
            cboDepositRate.Enabled = !edit;
            txtNote.ReadOnly = edit;

            lblGuestName.Visible = txtGuestName.Visible = !edit;
            lblGuestIdNumber.Visible = txtGuestIdNumber.Visible = !edit;
            btnAddRoom.Visible = !edit;
            btnSave.Visible = !edit;

            chkKeepPrice.Visible = txtReason.Visible = edit;
            btnUpdateRoom.Visible = edit;
            btnDeposit.Visible = edit;
            btnRemoveRoom.Text = edit ? "Hủy phòng đã chọn" : "Bỏ phòng đã chọn";
            grpRooms.Text = edit ? "Phòng trong phiếu (chọn phòng \"Chờ nhận\" để sửa)" : "Giỏ phòng sẽ đặt";
            Text = edit ? "Phiếu đặt #" + _bookingId : "Tạo phiếu đặt phòng";
            UpdateRoomButtons();
        }

        // ==================================================================
        // Khách hàng
        // ==================================================================

        private void btnPickCustomer_Click(object sender, EventArgs e)
        {
            int? picked = null;
            FormManager.ShowDialog(new frmCustomers(true), this, f => picked = ((frmCustomers)f).SelectedCustomerId);
            if (picked == null) return;

            try
            {
                DataTable dt = DbHelper.QueryText(
                    "SELECT FullName, Phone FROM dbo.Customers WHERE CustomerId = @Id", DbHelper.P("@Id", picked.Value));
                if (dt.Rows.Count == 0) return;
                _customerId = picked;
                txtCustomer.Text = DbHelper.Value<string>(dt.Rows[0], "FullName") + " – " +
                                   DbHelper.Value<string>(dt.Rows[0], "Phone");
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        // ==================================================================
        // Ngày / tìm phòng trống
        // ==================================================================

        private void dates_ValueChanged(object sender, EventArgs e)
        {
            if (_adjustingDates) return;
            // Ngày trả luôn sau ngày nhận ít nhất 1 đêm.
            if (dtpCheckOut.Value.Date <= dtpCheckIn.Value.Date)
            {
                _adjustingDates = true;
                dtpCheckOut.Value = dtpCheckIn.Value.Date.AddDays(1);
                _adjustingDates = false;
            }
            UpdateNights();
            // Kết quả tìm cũ không còn đúng với ngày mới.
            dgvAvailable.DataSource = null;
        }

        private int Nights()
        {
            return (dtpCheckOut.Value.Date - dtpCheckIn.Value.Date).Days;
        }

        private void UpdateNights()
        {
            lblNights.Text = Formatters.Nights(Nights());
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchRooms();
        }

        private void SearchRooms()
        {
            if (!IsEditMode && dtpCheckIn.Value.Date < DateTime.Today)
            {
                UiHelper.Warn("Ngày nhận phải từ hôm nay trở đi.", this);
                return;
            }

            try
            {
                int? exclude = IsEditMode ? UiHelper.SelectedId(dgvRooms, "BookingRoomId") : null;
                DataTable dt = DbHelper.QueryText(SqlAvailable,
                    DbHelper.P("@In", SqlDbType.Date, dtpCheckIn.Value.Date),
                    DbHelper.P("@Out", SqlDbType.Date, dtpCheckOut.Value.Date),
                    DbHelper.P("@Exclude", exclude),
                    DbHelper.P("@TypeId", Lookups.SelectedInt(cboRoomType)),
                    DbHelper.P("@Guests", (int)(nudAdults.Value + nudChildren.Value)));

                // Khi tạo mới: bỏ các phòng đã có trong giỏ trùng khoảng ngày (tránh thêm 2 lần).
                if (!IsEditMode && _cart != null)
                {
                    for (int i = dt.Rows.Count - 1; i >= 0; i--)
                    {
                        if (CartHasOverlap(DbHelper.Value<int>(dt.Rows[i], "RoomId"),
                                           dtpCheckIn.Value.Date, dtpCheckOut.Value.Date))
                            dt.Rows.RemoveAt(i);
                    }
                }

                UiHelper.Bind(dgvAvailable, dt, "RoomId");
                UiHelper.ShowColumns(dgvAvailable,
                    "RoomNumber", "Phòng",
                    "Floor", "Tầng",
                    "TypeName", "Loại phòng",
                    "BedType", "Giường",
                    "MaxOccupancy", "Sức chứa",
                    "BasePrice", "Giá/đêm",
                    "RoomStatus", "Tình trạng hiện tại");
                grpSearch.Text = "Tìm phòng trống – " + dt.Rows.Count + " phòng phù hợp " +
                                 Formatters.Date(dtpCheckIn.Value) + " → " + Formatters.Date(dtpCheckOut.Value);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void dgvAvailable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (IsEditMode) btnUpdateRoom_Click(sender, e);
            else btnAddRoom_Click(sender, e);
        }

        // ==================================================================
        // TẠO MỚI: giỏ phòng
        // ==================================================================

        private void CreateCart()
        {
            _cart = new DataTable();
            _cart.Columns.Add("RowKey", typeof(int)).AutoIncrement = true;
            _cart.Columns.Add("RoomId", typeof(int));
            _cart.Columns.Add("RoomNumber", typeof(string));
            _cart.Columns.Add("TypeName", typeof(string));
            _cart.Columns.Add("CheckInDate", typeof(DateTime));
            _cart.Columns.Add("CheckOutDate", typeof(DateTime));
            _cart.Columns.Add("Nights", typeof(int));
            _cart.Columns.Add("Adults", typeof(int));
            _cart.Columns.Add("Children", typeof(int));
            _cart.Columns.Add("PricePerNight", typeof(decimal));
            _cart.Columns.Add("Amount", typeof(decimal));
            _cart.Columns.Add("GuestName", typeof(string));
            _cart.Columns.Add("GuestIdNumber", typeof(string));
            BindCart();
        }

        private void BindCart()
        {
            UiHelper.Bind(dgvRooms, _cart, "RowKey");
            UiHelper.ShowColumns(dgvRooms,
                "RoomNumber", "Phòng",
                "TypeName", "Loại phòng",
                "CheckInDate", "Ngày nhận",
                "CheckOutDate", "Ngày trả",
                "Nights", "Số đêm",
                "Adults", "Người lớn",
                "Children", "Trẻ em",
                "PricePerNight", "Giá/đêm (tạm)",
                "Amount", "Thành tiền (tạm)",
                "GuestName", "Người ở");
            UpdateRoomButtons();
        }

        private bool CartHasOverlap(int roomId, DateTime checkIn, DateTime checkOut)
        {
            foreach (DataRow c in _cart.Rows)
            {
                if (DbHelper.Value<int>(c, "RoomId") == roomId &&
                    DbHelper.Value<DateTime>(c, "CheckInDate") < checkOut &&
                    DbHelper.Value<DateTime>(c, "CheckOutDate") > checkIn)
                    return true;
            }
            return false;
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            DataRow room = UiHelper.SelectedRow(dgvAvailable);
            if (room == null)
            {
                UiHelper.Warn("Hãy bấm \"Tìm phòng trống\" rồi chọn một phòng ở lưới trên.", this);
                return;
            }

            DateTime checkIn = dtpCheckIn.Value.Date;
            DateTime checkOut = dtpCheckOut.Value.Date;
            int adults = (int)nudAdults.Value;
            int children = (int)nudChildren.Value;
            int roomId = DbHelper.Value<int>(room, "RoomId");

            if (checkIn < DateTime.Today)
            {
                UiHelper.Warn("Ngày nhận phải từ hôm nay trở đi.", this);
                return;
            }
            if (adults + children > DbHelper.Value<int>(room, "MaxOccupancy"))
            {
                UiHelper.Warn("Số khách vượt sức chứa của phòng (" + DbHelper.Value<int>(room, "MaxOccupancy") + " người).", this);
                return;
            }
            if (txtGuestIdNumber.Text.Trim().Length > 0)
            {
                if (txtGuestName.Text.Trim().Length == 0)
                {
                    UiHelper.Warn("Nhập CCCD người ở thì phải nhập tên người ở.", this);
                    txtGuestName.Focus();
                    return;
                }
                if (!UiHelper.IsValidIdNumber(txtGuestIdNumber.Text))
                {
                    UiHelper.Warn("CCCD/hộ chiếu chỉ gồm chữ cái không dấu và chữ số, dài 6–20 ký tự.", this);
                    txtGuestIdNumber.Focus();
                    return;
                }
            }
            if (CartHasOverlap(roomId, checkIn, checkOut))
            {
                UiHelper.Warn("Phòng này đã có trong giỏ với khoảng ngày trùng.", this);
                return;
            }

            decimal price = DbHelper.Value<decimal>(room, "BasePrice");
            int nights = (checkOut - checkIn).Days;
            _cart.Rows.Add(null, roomId, DbHelper.Value<string>(room, "RoomNumber"), DbHelper.Value<string>(room, "TypeName"),
                           checkIn, checkOut, nights, adults, children, price, price * nights,
                           NullIfBlank(txtGuestName.Text), NullIfBlank(txtGuestIdNumber.Text));

            // Phòng đã vào giỏ thì bỏ khỏi danh sách phòng trống.
            room.Delete();
            room.Table.AcceptChanges();
            txtGuestName.Text = "";
            txtGuestIdNumber.Text = "";
            UpdateTotals();
            UpdateRoomButtons();
        }

        private static string NullIfBlank(string s)
        {
            return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }

        private void btnRemoveRoom_Click(object sender, EventArgs e)
        {
            if (IsEditMode)
            {
                // Sửa phiếu: "bỏ phòng" nghĩa là hủy lượt lưu trú đó (theo chính sách hoàn cọc).
                DataRow stay = UiHelper.SelectedRow(dgvRooms);
                if (stay == null || DbHelper.Value<string>(stay, "StayStatus") != StayStatus.Booked) return;
                if (FormManager.ShowDialog(new dlgCancel(_bookingId.Value, DbHelper.Value<int>(stay, "BookingRoomId")), this))
                    LoadBooking();
                return;
            }

            DataRow row = UiHelper.SelectedRow(dgvRooms);
            if (row == null) return;
            row.Delete();
            _cart.AcceptChanges();
            UpdateTotals();
            UpdateRoomButtons();
        }

        private void cboDepositRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsEditMode) UpdateTotals();
        }

        /// <summary>Tạm tính: tổng tiền phòng và cọc = Σ ROUND(tiền phòng từng phòng × tỷ lệ) (cùng cách DB tính).</summary>
        private void UpdateTotals()
        {
            if (_cart == null) return;
            int rate = Lookups.SelectedInt(cboDepositRate) ?? DepositRates.Half;
            decimal total = 0, deposit = 0;
            foreach (DataRow r in _cart.Rows)
            {
                decimal amount = DbHelper.Value<decimal>(r, "Amount");
                total += amount;
                deposit += Math.Round(amount * rate / 100m, 0, MidpointRounding.AwayFromZero);
            }
            lblTotals.Text = _cart.Rows.Count + " phòng   ·   Tiền phòng (tạm tính, chưa VAT): " + Formatters.Money(total) +
                             "   ·   Cọc " + rate + "%: " + Formatters.Money(deposit);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_customerId == null)
            {
                UiHelper.Warn("Hãy chọn khách đặt phòng (nút \"Chọn...\").", this);
                btnPickCustomer.Focus();
                return;
            }
            if (_cart.Rows.Count == 0)
            {
                UiHelper.Warn("Phiếu phải có ít nhất một phòng. Tìm phòng trống rồi bấm \"Thêm phòng vào phiếu\".", this);
                return;
            }

            DataTable rooms = DbHelper.CreateBookingRoomInput();
            foreach (DataRow r in _cart.Rows)
            {
                DbHelper.AddBookingRoom(rooms,
                    DbHelper.Value<int>(r, "RoomId"),
                    DbHelper.Value<DateTime>(r, "CheckInDate"),
                    DbHelper.Value<DateTime>(r, "CheckOutDate"),
                    DbHelper.Value<int>(r, "Adults"),
                    DbHelper.Value<int>(r, "Children"),
                    DbHelper.Value<string>(r, "GuestName"),
                    DbHelper.Value<string>(r, "GuestIdNumber"));
            }

            int newId;
            try
            {
                using (UiHelper.Busy(this))
                {
                    SqlParameter bookingId = DbHelper.Out("@BookingId", SqlDbType.Int);
                    DbHelper.Exec("dbo.sp_CreateBooking",
                        DbHelper.P("@CustomerId", _customerId.Value),
                        DbHelper.P("@DepositRate", (byte)(Lookups.SelectedInt(cboDepositRate) ?? DepositRates.Half)),
                        DbHelper.P("@Note", txtNote.Text),
                        Session.EmployeeParam(),
                        DbHelper.Tvp("@Rooms", "dbo.BookingRoomInput", rooms),
                        bookingId);
                    newId = DbHelper.OutValue<int>(bookingId);
                }
            }
            catch (Exception ex)
            {
                // Trùng lịch (50005), vượt sức chứa, ngày sai... -> câu tiếng Việt; giỏ vẫn giữ để sửa.
                ErrorHandler.Show(ex, this);
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Bookings);
            FormManager.NotifyDataChanged(DataArea.Stays);

            if (UiHelper.Confirm("Đã tạo phiếu #" + newId + " (chờ cọc). Phiếu tự hủy nếu quá " +
                                 BusinessRules.PendingExpireHours + " giờ chưa đủ cọc.\n\nThu cọc ngay bây giờ?", owner: this))
            {
                FormManager.ShowDialog(new dlgPayment(PaymentMode.Deposit, newId), this);
            }

            // Mở phiếu vừa tạo ở chế độ sửa, đóng form tạo mới.
            FormManager.Open("booking-" + newId, () => new frmBookingEdit(newId));
            Close();
        }

        // ==================================================================
        // SỬA: nạp phiếu và cập nhật từng phòng
        // ==================================================================

        private void LoadBooking()
        {
            try
            {
                DataTable header = DbHelper.QueryText(SqlHeader, DbHelper.P("@Id", _bookingId.Value));
                if (header.Rows.Count == 0)
                {
                    UiHelper.Warn("Không tìm thấy phiếu đặt #" + _bookingId + ".", this);
                    BeginInvoke(new Action(Close));
                    return;
                }
                DataRow h = header.Rows[0];
                _customerId = DbHelper.Value<int>(h, "CustomerId");
                txtCustomer.Text = DbHelper.Value<string>(h, "CustomerName") + " – " + DbHelper.Value<string>(h, "CustomerPhone");
                Lookups.SelectValue(cboDepositRate, h["DepositRate"]);
                txtNote.Text = DbHelper.Value<string>(h, "Note", "");
                string status = DbHelper.Value<string>(h, "Status");
                lblBookingInfo.Text = "Phiếu #" + _bookingId + " – " + BookingStatus.ToVietnamese(status) +
                                      " – tạo " + Formatters.DateTimeText(h["CreatedAt"]);
                lblTotals.Text = "Cọc yêu cầu: " + Formatters.Money(DbHelper.Value<decimal>(h, "RequiredDeposit")) +
                                 "   ·   Đã thu: " + Formatters.Money(DbHelper.Value<decimal>(h, "NetPaid")) +
                                 "   ·   Còn thiếu cọc: " + Formatters.Money(DbHelper.Value<decimal>(h, "MissingDeposit"));
                btnDeposit.Enabled = DbHelper.Value<int>(h, "BookedCount") > 0;

                DataTable stays = DbHelper.QueryText(SqlStays, DbHelper.P("@Id", _bookingId.Value));
                UiHelper.Bind(dgvRooms, stays, "BookingRoomId");
                UiHelper.ShowColumns(dgvRooms,
                    "RoomNumber", "Phòng",
                    "TypeName", "Loại phòng",
                    "CheckInDate", "Ngày nhận",
                    "CheckOutDate", "Ngày trả",
                    "Nights", "Số đêm",
                    "Adults", "Người lớn",
                    "Children", "Trẻ em",
                    "PricePerNight", "Giá/đêm (đã chốt)",
                    "Amount", "Tiền phòng",
                    "StayStatus", "Trạng thái",
                    "RequiredDeposit", "Cọc yêu cầu",
                    "NetPaid", "Đã thu",
                    "GuestName", "Người ở");
                FillFromSelectedStay();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void dgvRooms_SelectionChanged(object sender, EventArgs e)
        {
            if (IsEditMode) FillFromSelectedStay();
            UpdateRoomButtons();
        }

        /// <summary>Chọn một phòng "Chờ nhận": điền ngày, số người vào ô tìm và tìm phòng thay thế.</summary>
        private void FillFromSelectedStay()
        {
            DataRow stay = UiHelper.SelectedRow(dgvRooms);
            if (stay == null || DbHelper.Value<string>(stay, "StayStatus") != StayStatus.Booked)
            {
                dgvAvailable.DataSource = null;
                UpdateRoomButtons();
                return;
            }

            _adjustingDates = true;
            dtpCheckIn.Value = DbHelper.Value<DateTime>(stay, "CheckInDate");
            dtpCheckOut.Value = DbHelper.Value<DateTime>(stay, "CheckOutDate");
            _adjustingDates = false;
            UpdateNights();
            nudAdults.Value = Math.Max(nudAdults.Minimum, DbHelper.Value<int>(stay, "Adults"));
            nudChildren.Value = DbHelper.Value<int>(stay, "Children");
            chkKeepPrice.Checked = false;
            txtReason.Text = "";
            SearchRooms();
            UiHelper.SelectRowById(dgvAvailable, "RoomId", DbHelper.Value<int>(stay, "RoomId"));
            UpdateRoomButtons();
        }

        private void btnUpdateRoom_Click(object sender, EventArgs e)
        {
            DataRow stay = UiHelper.SelectedRow(dgvRooms);
            if (stay == null || DbHelper.Value<string>(stay, "StayStatus") != StayStatus.Booked)
            {
                UiHelper.Warn("Chỉ sửa được phòng đang \"Chờ nhận\". Hãy chọn phòng đó ở lưới dưới.", this);
                return;
            }
            DataRow room = UiHelper.SelectedRow(dgvAvailable);
            if (room == null)
            {
                UiHelper.Warn("Hãy bấm \"Tìm phòng trống\" rồi chọn phòng (giữ phòng cũ thì chọn lại chính nó).", this);
                return;
            }
            if (chkKeepPrice.Checked && !UiHelper.RequireText(txtReason, "lý do giữ giá cũ")) return;

            int stayId = DbHelper.Value<int>(stay, "BookingRoomId");
            string oldRoom = DbHelper.Value<string>(stay, "RoomNumber");
            string newRoom = DbHelper.Value<string>(room, "RoomNumber");
            if (!UiHelper.Confirm("Cập nhật phòng " + oldRoom + (oldRoom != newRoom ? " → " + newRoom : "") + ": " +
                                  Formatters.Date(dtpCheckIn.Value) + " → " + Formatters.Date(dtpCheckOut.Value) + ", " +
                                  nudAdults.Value + " người lớn, " + nudChildren.Value + " trẻ em?", owner: this)) return;

            try
            {
                using (UiHelper.Busy(this))
                {
                    DbHelper.Exec("dbo.sp_UpdateBookingRoom",
                        DbHelper.P("@BookingRoomId", stayId),
                        DbHelper.P("@NewRoomId", DbHelper.Value<int>(room, "RoomId")),
                        DbHelper.P("@CheckInDate", SqlDbType.Date, dtpCheckIn.Value.Date),
                        DbHelper.P("@CheckOutDate", SqlDbType.Date, dtpCheckOut.Value.Date),
                        DbHelper.P("@Adults", (byte)nudAdults.Value),
                        DbHelper.P("@Children", (byte)nudChildren.Value),
                        DbHelper.P("@KeepPrice", chkKeepPrice.Checked),
                        DbHelper.P("@Reason", txtReason.Text),
                        Session.EmployeeParam());
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Bookings);
            FormManager.NotifyDataChanged(DataArea.Stays);
            UiHelper.Info("Đã cập nhật phòng. Đổi sang loại phòng khác thì giá theo loại mới (trừ khi giữ giá cũ).", this);
            LoadBooking();
            UiHelper.SelectRowById(dgvRooms, "BookingRoomId", stayId);
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            if (!_bookingId.HasValue) return;
            if (FormManager.ShowDialog(new dlgPayment(PaymentMode.Deposit, _bookingId.Value), this)) LoadBooking();
        }

        private void UpdateRoomButtons()
        {
            DataRow r = UiHelper.SelectedRow(dgvRooms);
            if (IsEditMode)
            {
                bool booked = r != null && DbHelper.Value<string>(r, "StayStatus") == StayStatus.Booked;
                btnUpdateRoom.Enabled = booked;
                btnRemoveRoom.Enabled = booked;
            }
            else
            {
                btnRemoveRoom.Enabled = r != null;
            }
        }

        private void OnDataChanged(string area)
        {
            if (area != DataArea.Bookings && area != DataArea.Payments && area != DataArea.Stays) return;
            if (_reloadPending) return;
            _reloadPending = true;
            BeginInvoke(new Action(() =>
            {
                _reloadPending = false;
                if (!IsDisposed) LoadBooking();
            }));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!IsEditMode && _cart != null && _cart.Rows.Count > 0 &&
                !UiHelper.Confirm("Giỏ đang có " + _cart.Rows.Count + " phòng chưa lưu. Đóng và bỏ phiếu này?",
                    defaultNo: true, owner: this)) return;
            Close();
        }
    }
}
