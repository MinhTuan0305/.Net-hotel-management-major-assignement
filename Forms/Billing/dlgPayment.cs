using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Billing
{
    /// <summary>
    /// Ghi một khoản tiền (Admin, Manager, Receptionist; hoàn tiền chỉ Admin, Manager).
    /// Đặc tả: KE_HOACH mục 3; nghiệp vụ mục 13.
    ///   Deposit     (id = BookingId)     : cọc cả phiếu  -> sp_RecordBookingDeposit (DB tự chia cho từng phòng).
    ///   StayPayment (id = BookingRoomId) : thu khi đang ở -> sp_RecordPayment @Type = 'Payment'.
    ///   Refund      (id = BookingRoomId) : hoàn thủ công  -> sp_RecordPayment @Type = 'Refund', ghi chú bắt buộc.
    /// Thu/hoàn lúc check-out và lập lại hóa đơn KHÔNG dùng hộp thoại này (sp_CheckOutRoom tự làm).
    /// </summary>
    public partial class dlgPayment : Form
    {
        private readonly PaymentMode _mode;
        private readonly int _id;
        private string _target;   // "phiếu #12" / "phòng 301" để ghi vào câu thông báo

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public dlgPayment(PaymentMode mode, int id)
        {
            InitializeComponent();
            _mode = mode;
            _id = id;
            UiHelper.SetupDialog(this, btnOk, btnCancel);
            UiHelper.SetupMoneyInput(nudAmount);
        }

        private void dlgPayment_Load(object sender, EventArgs e)
        {
            if (_mode == PaymentMode.Refund)
            {
                // Hoàn tiền ngoài chính sách chỉ Admin/Manager (ma trận quyền I.1).
                if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager)) return;
            }
            else if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist))
            {
                return;
            }

            try
            {
                Lookups.LoadPaymentMethods(cboMethod);
                bool ok;
                switch (_mode)
                {
                    case PaymentMode.Deposit: ok = LoadDeposit(); break;
                    case PaymentMode.StayPayment: ok = LoadStayPayment(); break;
                    default: ok = LoadRefund(); break;
                }
                if (!ok) BeginInvoke(new Action(Close));
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                BeginInvoke(new Action(Close));
            }
            UpdateReferenceLabel();
        }

        // ==================================================================
        // Nạp thông tin theo chế độ
        // ==================================================================

        private bool LoadDeposit()
        {
            DataTable dt = DbHelper.QueryText(
                "SELECT BookingId, CustomerName, Status, DepositRate, RequiredDeposit, NetPaid, MissingDeposit, BookedCount " +
                "FROM dbo.vw_BookingSummary WHERE BookingId = @Id", DbHelper.P("@Id", _id));
            if (dt.Rows.Count == 0)
            {
                UiHelper.Warn("Không tìm thấy phiếu đặt #" + _id + ".", this);
                return false;
            }
            DataRow r = dt.Rows[0];
            if (DbHelper.Value<int>(r, "BookedCount") == 0)
            {
                UiHelper.Warn("Phiếu #" + _id + " không còn phòng nào đang chờ nhận để thu cọc.", this);
                return false;
            }

            // Tối đa: tiền phòng còn lại của các phòng chưa nhận (cùng giới hạn với procedure).
            decimal capacity = DbHelper.Scalar<decimal>(
                "SELECT SUM(CASE WHEN PlannedRoomCharge > NetPaid THEN PlannedRoomCharge - NetPaid ELSE 0 END) " +
                "FROM dbo.vw_RoomStayFolio WHERE BookingId = @Id AND StayStatus = 'Booked'", DbHelper.P("@Id", _id));
            decimal missing = DbHelper.Value<decimal>(r, "MissingDeposit");

            _target = "phiếu #" + _id;
            Text = "Thu cọc phiếu #" + _id;
            lblTitle.Text = "Thu cọc phiếu #" + _id;
            lblSubtitle.Text = DbHelper.Value<string>(r, "CustomerName") + " – cọc " +
                               DbHelper.Value<int>(r, "DepositRate") + "% – " +
                               BookingStatus.ToVietnamese(DbHelper.Value<string>(r, "Status"));
            SetInfo("Cọc yêu cầu:", DbHelper.Value<decimal>(r, "RequiredDeposit"),
                    "Đã thu:", DbHelper.Value<decimal>(r, "NetPaid"),
                    "Còn thiếu cọc:", missing);

            if (capacity > 0) nudAmount.Maximum = capacity;
            nudAmount.Value = Math.Min(missing, nudAmount.Maximum);
            lblAmountHint.Text = "Tối đa " + Formatters.Money(capacity);
            btnOk.Text = "Thu cọc";
            return true;
        }

        private bool LoadStayPayment()
        {
            DataRow r = LoadFolio();
            if (r == null) return false;

            string status = DbHelper.Value<string>(r, "StayStatus");
            if (status != StayStatus.CheckedIn && !Session.HasRole(Roles.Admin, Roles.Manager))
            {
                UiHelper.Warn("Chỉ thu tiền cho phòng đang ở (phòng này: " + StayStatus.ToVietnamese(status) + ").", this);
                return false;
            }

            decimal balance = DbHelper.Value<decimal>(r, "EstimatedBalance");
            Text = "Thu tiền phòng " + DbHelper.Value<string>(r, "RoomNumber");
            lblTitle.Text = Text;
            SetInfo("Tạm tính (phòng + dịch vụ + VAT):", DbHelper.Value<decimal>(r, "EstimatedTotal"),
                    "Đã thu:", DbHelper.Value<decimal>(r, "NetPaid"),
                    balance >= 0 ? "Còn lại (tạm tính):" : "Khách đã trả dư:", Math.Abs(balance));
            nudAmount.Value = balance > 0 ? Math.Min(balance, nudAmount.Maximum) : 0;
            lblAmountHint.Text = "Số chính thức tính khi check-out.";
            if (status != StayStatus.CheckedIn)
                lblNote.Text = "Ghi chú:*";   // Admin/Manager ghi điều chỉnh ngoài lúc ở phải có ghi chú
            return true;
        }

        private bool LoadRefund()
        {
            DataRow r = LoadFolio();
            if (r == null) return false;

            decimal netPaid = DbHelper.Value<decimal>(r, "NetPaid");
            if (netPaid <= 0)
            {
                UiHelper.Warn("Phòng này chưa có tiền thu nào để hoàn.", this);
                return false;
            }

            Text = "Hoàn tiền phòng " + DbHelper.Value<string>(r, "RoomNumber");
            lblTitle.Text = Text + " (ngoài chính sách)";
            SetInfo("Đã cọc:", DbHelper.Value<decimal>(r, "DepositPaid"),
                    "Đã thanh toán khác / đã hoàn:",
                    DbHelper.Value<decimal>(r, "OtherPaid") - DbHelper.Value<decimal>(r, "Refunded"),
                    "Thực thu của phòng (tối đa hoàn):", netPaid);
            nudAmount.Maximum = netPaid;
            nudAmount.Value = 0;
            lblAmountHint.Text = "Tối đa " + Formatters.Money(netPaid);
            lblNote.Text = "Lý do hoàn:*";
            lblMethod.Text = "Hoàn bằng:";
            btnOk.Text = "Ghi hoàn";
            return true;
        }

        /// <summary>Sổ tiền của một lượt lưu trú (vw_RoomStayFolio) kèm tên khách.</summary>
        private DataRow LoadFolio()
        {
            DataTable dt = DbHelper.QueryText(
                "SELECT f.*, s.CustomerName, s.GuestName FROM dbo.vw_RoomStayFolio f " +
                "JOIN dbo.vw_RoomStays s ON s.BookingRoomId = f.BookingRoomId WHERE f.BookingRoomId = @Id",
                DbHelper.P("@Id", _id));
            if (dt.Rows.Count == 0)
            {
                UiHelper.Warn("Không tìm thấy lượt lưu trú.", this);
                return null;
            }
            DataRow r = dt.Rows[0];
            _target = "phòng " + DbHelper.Value<string>(r, "RoomNumber");
            lblSubtitle.Text = "Phiếu #" + DbHelper.Value<int>(r, "BookingId") + " – " +
                               DbHelper.Value<string>(r, "GuestName") + " – " +
                               StayStatus.ToVietnamese(DbHelper.Value<string>(r, "StayStatus"));
            return r;
        }

        private void SetInfo(string c1, decimal v1, string c2, decimal v2, string c3, decimal v3)
        {
            lblCaption1.Text = c1;
            lblValue1.Text = Formatters.Money(v1);
            lblCaption2.Text = c2;
            lblValue2.Text = Formatters.Money(v2);
            lblCaption3.Text = c3;
            lblValue3.Text = Formatters.Money(v3);
        }

        // ==================================================================
        // Nhập liệu
        // ==================================================================

        private void cboMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateReferenceLabel();
        }

        /// <summary>Chuyển khoản / ví điện tử bắt buộc có mã giao dịch (CK_Payments_Reference).</summary>
        private void UpdateReferenceLabel()
        {
            lblReference.Text = PayMethod.NeedsReference(Lookups.SelectedString(cboMethod))
                ? "Mã giao dịch:*"
                : "Mã giao dịch:";
        }

        private bool ValidateInput()
        {
            if (nudAmount.Value <= 0)
            {
                UiHelper.Warn("Số tiền phải lớn hơn 0.", this);
                nudAmount.Focus();
                return false;
            }
            if (!UiHelper.RequireSelection(cboMethod, "phương thức thanh toán")) return false;
            if (PayMethod.NeedsReference(Lookups.SelectedString(cboMethod)) &&
                !UiHelper.RequireText(txtReference, "mã giao dịch (chuyển khoản/ví điện tử)")) return false;
            if (lblNote.Text.EndsWith("*", StringComparison.Ordinal) && !UiHelper.RequireText(txtNote, "ghi chú / lý do"))
                return false;
            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            string method = Lookups.SelectedString(cboMethod);

            try
            {
                using (UiHelper.Busy(this))
                {
                    if (_mode == PaymentMode.Deposit)
                    {
                        DbHelper.Exec("dbo.sp_RecordBookingDeposit",
                            DbHelper.P("@BookingId", _id),
                            DbHelper.P("@Amount", nudAmount.Value),
                            DbHelper.P("@Method", method),
                            DbHelper.P("@ReferenceNo", txtReference.Text),
                            Session.EmployeeParam());
                    }
                    else
                    {
                        SqlParameter paymentId = DbHelper.Out("@PaymentId", SqlDbType.Int);
                        DbHelper.Exec("dbo.sp_RecordPayment",
                            DbHelper.P("@BookingRoomId", _id),
                            DbHelper.P("@Type", _mode == PaymentMode.Refund ? PaymentType.Refund : PaymentType.Payment),
                            DbHelper.P("@Amount", nudAmount.Value),
                            DbHelper.P("@Method", method),
                            DbHelper.P("@ReferenceNo", txtReference.Text),
                            DbHelper.P("@Note", txtNote.Text),
                            Session.EmployeeParam(),
                            paymentId);
                    }
                }
            }
            catch (Exception ex)
            {
                // Vượt số tối đa, thiếu mã giao dịch, sai quyền... procedure báo câu tiếng Việt.
                ErrorHandler.Show(ex, this);
                return;
            }

            FormManager.NotifyDataChanged(DataArea.Payments);
            FormManager.NotifyDataChanged(DataArea.Bookings);
            FormManager.NotifyDataChanged(DataArea.Stays);

            string action = _mode == PaymentMode.Refund ? "Đã ghi hoàn " : "Đã thu ";
            UiHelper.Info(action + Formatters.Money(nudAmount.Value) + " (" + PayMethod.ToVietnamese(method) +
                          ") cho " + _target + ".", this);
            DialogResult = DialogResult.OK;
        }
    }
}
