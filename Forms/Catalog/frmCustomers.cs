using System;
using System.Data;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Catalog
{
    /// <summary>
    /// Khách hàng (Admin, Manager, Receptionist; xóa chỉ Admin, Manager). Đặc tả: KE_HOACH mục 2; nghiệp vụ mục 4.
    /// - Bắt buộc: họ tên, CCCD/hộ chiếu (duy nhất), SĐT. Ngày sinh không ở tương lai. Quốc tịch mặc định "Việt Nam".
    /// - Tìm theo tên/SĐT/CCCD, không phân biệt dấu (COLLATE Vietnamese_CI_AI).
    /// - Lưới dưới: lịch sử phiếu đặt của khách (vw_BookingSummary).
    /// - pickMode = true (frmBookingEdit mở để chọn khách): hiện nút "Chọn khách này", nhấp đúp để chọn;
    ///   thêm khách mới xong thì chọn luôn. Kết quả ở SelectedCustomerId, DialogResult = OK.
    /// </summary>
    public partial class frmCustomers : Form
    {
        private const string SqlList = @"
SELECT c.CustomerId, c.FullName, c.Gender, c.DateOfBirth, c.IdNumber, c.Phone, c.Email, c.Address, c.Nationality,
       c.CreatedAt,
       (SELECT COUNT(*) FROM dbo.Bookings b WHERE b.CustomerId = c.CustomerId) AS BookingCount
FROM dbo.Customers c
WHERE (@Kw IS NULL OR c.FullName COLLATE Vietnamese_CI_AI LIKE @Kw
                   OR c.Phone LIKE @Kw OR c.IdNumber LIKE @Kw)
ORDER BY c.FullName;";

        private const string SqlHistory = @"
SELECT BookingId, Status, CreatedAt, FirstCheckIn, LastCheckOut, RoomCount, RequiredDeposit, NetPaid, Note
FROM dbo.vw_BookingSummary
WHERE CustomerId = @Id
ORDER BY CreatedAt DESC;";

        private const string DefaultNationality = "Việt Nam";

        private enum EditMode { View, Add, Edit }

        private readonly bool _pickMode;
        private EditMode _mode = EditMode.View;

        /// <summary>Khách được chọn khi mở ở chế độ chọn (pickMode = true).</summary>
        public int? SelectedCustomerId { get; private set; }

        // Open<T>() của FormManager cần constructor không tham số, nên tách 2 constructor thay vì (bool pickMode = false).
        public frmCustomers() : this(false)
        {
        }

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public frmCustomers(bool pickMode)
        {
            InitializeComponent();
            _pickMode = pickMode;
            UiHelper.StyleGrid(dgvCustomers);
            UiHelper.StyleGrid(dgvHistory);
            UiHelper.TranslateColumn(dgvHistory, "Status", BookingStatus.ToVietnamese);

            if (pickMode)
            {
                Text = "Chọn khách hàng";
                StartPosition = FormStartPosition.CenterParent;
                ShowInTaskbar = false;
                btnPick.Visible = true;
                AcceptButton = btnPick;
            }
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            try
            {
                Lookups.LoadGenders(cboGender, Lookups.ChooseText);
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
            LoadData();
            SetMode(EditMode.View);
            txtSearch.Focus();
        }

        // ==================================================================
        // Nạp dữ liệu
        // ==================================================================

        private void LoadData(int? selectId = null)
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlList, DbHelper.P("@Kw", DbHelper.LikeContains(txtSearch.Text)));
                UiHelper.Bind(dgvCustomers, dt, "CustomerId");
                UiHelper.ShowColumns(dgvCustomers,
                    "FullName", "Họ tên",
                    "Phone", "SĐT",
                    "IdNumber", "CCCD/Hộ chiếu",
                    "Gender", "Giới tính",
                    "DateOfBirth", "Ngày sinh",
                    "Nationality", "Quốc tịch",
                    "BookingCount", "Số phiếu");
                if (selectId.HasValue) UiHelper.SelectRowById(dgvCustomers, "CustomerId", selectId.Value);
                ShowDetail();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ShowDetail()
        {
            DataRow r = UiHelper.SelectedRow(dgvCustomers);
            if (r == null)
            {
                ClearFields();
                dgvHistory.DataSource = null;
            }
            else
            {
                txtFullName.Text = DbHelper.Value<string>(r, "FullName");
                Lookups.SelectValue(cboGender, r["Gender"]);
                DateTime? dob = DbHelper.Value<DateTime?>(r, "DateOfBirth");
                dtpDateOfBirth.Checked = dob.HasValue;
                dtpDateOfBirth.Value = dob ?? DateTime.Today.AddYears(-30);
                txtIdNumber.Text = DbHelper.Value<string>(r, "IdNumber");
                txtPhone.Text = DbHelper.Value<string>(r, "Phone");
                txtEmail.Text = DbHelper.Value<string>(r, "Email", "");
                txtAddress.Text = DbHelper.Value<string>(r, "Address", "");
                txtNationality.Text = DbHelper.Value<string>(r, "Nationality", DefaultNationality);
                LoadHistory(DbHelper.Value<int>(r, "CustomerId"));
            }
            UpdateButtons();
        }

        private void LoadHistory(int customerId)
        {
            try
            {
                DataTable dt = DbHelper.QueryText(SqlHistory, DbHelper.P("@Id", customerId));
                UiHelper.Bind(dgvHistory, dt);
                UiHelper.ShowColumns(dgvHistory,
                    "BookingId", "Mã phiếu",
                    "Status", "Trạng thái",
                    "CreatedAt", "Ngày tạo",
                    "FirstCheckIn", "Nhận",
                    "LastCheckOut", "Trả",
                    "RoomCount", "Số phòng",
                    "RequiredDeposit", "Cọc yêu cầu",
                    "NetPaid", "Đã thu",
                    "Note", "Ghi chú");
                // Cột ngày nhận/trả không có chữ "Date" trong tên nên định dạng thủ công.
                UiHelper.FormatDateColumns(dgvHistory, "FirstCheckIn", "LastCheckOut");
                lblHistory.Text = "Lịch sử đặt phòng của khách (" + dt.Rows.Count + " phiếu)";
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void ClearFields()
        {
            txtFullName.Text = "";
            Lookups.SelectValue(cboGender, null);
            dtpDateOfBirth.Value = DateTime.Today.AddYears(-30);
            dtpDateOfBirth.Checked = false;
            txtIdNumber.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtNationality.Text = DefaultNationality;
        }

        // ==================================================================
        // Chế độ Xem / Thêm / Sửa
        // ==================================================================

        private void SetMode(EditMode mode)
        {
            _mode = mode;
            bool editing = mode != EditMode.View;
            foreach (TextBox t in new[] { txtFullName, txtIdNumber, txtPhone, txtEmail, txtAddress, txtNationality })
                t.ReadOnly = !editing;
            cboGender.Enabled = editing;
            dtpDateOfBirth.Enabled = editing;
            dgvCustomers.Enabled = !editing;
            pnlTop.Enabled = !editing;
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            bool editing = _mode != EditMode.View;
            bool hasRow = UiHelper.SelectedRow(dgvCustomers) != null;
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && hasRow;
            // Xóa khách: chỉ Admin/Manager (ma trận quyền I.1).
            btnDelete.Visible = Session.HasRole(Roles.Admin, Roles.Manager);
            btnDelete.Enabled = !editing && hasRow;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
            btnPick.Enabled = !editing && hasRow;
        }

        private void dgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) ShowDetail();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (_mode == EditMode.View) LoadData();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearFields();
            dgvHistory.DataSource = null;
            SetMode(EditMode.Add);
            txtFullName.Focus();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!UiHelper.RequireSelectedRow(dgvCustomers, "một khách hàng")) return;
            SetMode(EditMode.Edit);
            txtFullName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetMode(EditMode.View);
            ShowDetail();
        }

        // ==================================================================
        // Lưu / Xóa / Chọn
        // ==================================================================

        private bool ValidateInput()
        {
            if (!UiHelper.RequireText(txtFullName, "họ tên khách")) return false;
            if (!UiHelper.RequireText(txtIdNumber, "số CCCD/hộ chiếu")) return false;
            if (!UiHelper.IsValidIdNumber(txtIdNumber.Text))
            {
                UiHelper.Warn("CCCD/hộ chiếu chỉ gồm chữ cái không dấu và chữ số, dài 6–20 ký tự.", this);
                txtIdNumber.Focus();
                return false;
            }
            if (!UiHelper.RequireText(txtPhone, "số điện thoại")) return false;
            if (!UiHelper.IsValidPhone(txtPhone.Text))
            {
                UiHelper.Warn("Số điện thoại không hợp lệ (8–15 chữ số).", this);
                txtPhone.Focus();
                return false;
            }
            if (!UiHelper.IsValidEmail(txtEmail.Text))
            {
                UiHelper.Warn("Email không đúng định dạng (vd. ten@gmail.com).", this);
                txtEmail.Focus();
                return false;
            }
            if (dtpDateOfBirth.Checked && dtpDateOfBirth.Value.Date > DateTime.Today)
            {
                UiHelper.Warn("Ngày sinh không được ở tương lai.", this);
                dtpDateOfBirth.Focus();
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string nationality = string.IsNullOrWhiteSpace(txtNationality.Text) ? DefaultNationality : txtNationality.Text;
            var ps = new[]
            {
                DbHelper.P("@FullName", txtFullName.Text),
                DbHelper.P("@Gender", Lookups.SelectedString(cboGender)),
                DbHelper.P("@Dob", dtpDateOfBirth.Checked ? (object)dtpDateOfBirth.Value.Date : null),
                DbHelper.P("@IdNumber", txtIdNumber.Text),
                DbHelper.P("@Phone", txtPhone.Text),
                DbHelper.P("@Email", txtEmail.Text),
                DbHelper.P("@Address", txtAddress.Text),
                DbHelper.P("@Nationality", nationality)
            };

            try
            {
                bool adding = _mode == EditMode.Add;
                int id;
                if (adding)
                {
                    id = DbHelper.Scalar<int>(
                        "INSERT INTO dbo.Customers (FullName, Gender, DateOfBirth, IdNumber, Phone, Email, Address, Nationality) " +
                        "VALUES (@FullName, @Gender, @Dob, @IdNumber, @Phone, @Email, @Address, @Nationality); " +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);", ps);
                }
                else
                {
                    id = UiHelper.SelectedId(dgvCustomers, "CustomerId").Value;
                    DbHelper.ExecText(
                        "UPDATE dbo.Customers SET FullName = @FullName, Gender = @Gender, DateOfBirth = @Dob, " +
                        "IdNumber = @IdNumber, Phone = @Phone, Email = @Email, Address = @Address, " +
                        "Nationality = @Nationality WHERE CustomerId = @Id",
                        ps[0], ps[1], ps[2], ps[3], ps[4], ps[5], ps[6], ps[7], DbHelper.P("@Id", id));
                }

                FormManager.NotifyDataChanged(DataArea.Catalog);
                SetMode(EditMode.View);

                // Chế độ chọn: thêm khách mới xong thì chọn luôn, khỏi tìm lại.
                if (_pickMode && adding)
                {
                    SelectedCustomerId = id;
                    DialogResult = DialogResult.OK;
                    return;
                }

                // Khách mới có thể không khớp ô tìm hiện tại: xóa ô tìm để thấy dòng vừa lưu.
                if (adding && txtSearch.Text.Length > 0) txtSearch.Text = "";
                LoadData(id);
            }
            catch (Exception ex)
            {
                // Trùng CCCD (UQ_Customers_IdNumber) -> câu tiếng Việt; giữ nguyên dữ liệu đã nhập để sửa.
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataRow r = UiHelper.SelectedRow(dgvCustomers);
            if (r == null) return;
            if (DbHelper.Value<int>(r, "BookingCount") > 0)
            {
                UiHelper.Warn("Khách đã có phiếu đặt phòng nên không xóa được (giữ lịch sử giao dịch).", this);
                return;
            }
            if (!UiHelper.Confirm("Xóa khách hàng \"" + DbHelper.Value<string>(r, "FullName") + "\"?",
                    defaultNo: true, owner: this)) return;

            try
            {
                DbHelper.ExecText("DELETE FROM dbo.Customers WHERE CustomerId = @Id",
                    DbHelper.P("@Id", DbHelper.Value<int>(r, "CustomerId")));
                FormManager.NotifyDataChanged(DataArea.Catalog);
                LoadData();
            }
            catch (Exception ex)
            {
                // Có phiếu phát sinh cùng lúc -> FK_Bookings_Customers.
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnPick_Click(object sender, EventArgs e)
        {
            int? id = UiHelper.SelectedId(dgvCustomers, "CustomerId");
            if (id == null)
            {
                UiHelper.Warn("Hãy chọn một khách hàng.", this);
                return;
            }
            SelectedCustomerId = id;
            DialogResult = DialogResult.OK;
        }

        private void dgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (_pickMode && e.RowIndex >= 0) btnPick_Click(sender, e);
        }
    }
}
