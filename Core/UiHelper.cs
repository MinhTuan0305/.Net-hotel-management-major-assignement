// =============================================================================
// UiHelper.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Các tiện ích giao diện dùng chung để 23 form trông và hoạt động giống nhau:
//     1. Lưới DataGridView: StyleGrid (kiểu chung), Bind (gán dữ liệu + tự định
//        dạng cột tiền/ngày + giữ dòng đang chọn), ShowColumns (chỉ hiện cột cần
//        thiết và đặt tiêu đề tiếng Việt), TranslateColumn (đổi "CheckedIn" ->
//        "Đang ở"), ColorRows (tô màu theo trạng thái), SelectedRow / SelectedId.
//     2. Hộp thông báo: Info, Warn, Error, Confirm.
//     3. Kiểm tra nhập liệu: RequireText, RequireSelection, RequireSelectedRow.
//     4. Phân quyền giao diện: VisibleFor / EnabledFor (control và mục menu).
//     5. Khác: Busy (con trỏ chờ), SetupMoneyInput (NumericUpDown cho tiền),
//        SetupDialog (cấu hình chuẩn cho hộp thoại dlg...), ExportCsv.
//
// CÁCH DÙNG (ví dụ frmFrontDesk, tab Đang ở)
//   // Trong Load – chỉ gọi MỘT lần cho mỗi lưới:
//   UiHelper.StyleGrid(dgvInHouse);
//   UiHelper.TranslateColumn(dgvInHouse, "StayStatus", StayStatus.ToVietnamese);
//   // Mỗi lần nạp lại:
//   UiHelper.Bind(dgvInHouse, DbHelper.QueryText("SELECT * FROM dbo.vw_InHouseStays"), "BookingRoomId");
//   UiHelper.ShowColumns(dgvInHouse,
//       "RoomNumber", "Phòng",
//       "GuestName", "Khách",
//       "CheckOutDate", "Ngày trả");
//   int? id = UiHelper.SelectedId(dgvInHouse, "BookingRoomId");
// =============================================================================
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HotelManagement.Core
{
    public static class UiHelper
    {
        public const string AppTitle = "Quản lý khách sạn";

        // ==================================================================
        // 1. DataGridView
        // ==================================================================

        /// <summary>Kiểu chung cho mọi lưới danh sách: chỉ đọc, chọn cả dòng, xen màu dòng.</summary>
        public static void StyleGrid(DataGridView dgv, bool fillColumns = true)
        {
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.MultiSelect = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.AutoGenerateColumns = true;
            dgv.BackgroundColor = SystemColors.Window;
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font(dgv.Font, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 239, 245);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 251);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 228, 247);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgv.AutoSizeColumnsMode = fillColumns
                ? DataGridViewAutoSizeColumnsMode.Fill
                : DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }

        /// <summary>
        /// Gán dữ liệu cho lưới và tự định dạng cột. keyColumn != null thì sau khi
        /// nạp lại sẽ chọn lại đúng dòng đang chọn trước đó (theo giá trị khóa).
        /// </summary>
        public static void Bind(DataGridView dgv, DataTable data, string keyColumn = null)
        {
            int? keep = keyColumn == null ? null : SelectedId(dgv, keyColumn);
            dgv.DataSource = data;
            AutoFormat(dgv);
            if (keep.HasValue) SelectRowById(dgv, keyColumn, keep.Value);
        }

        /// <summary>
        /// Định dạng tự động: cột decimal -> tiền "1.250.000" canh phải; cột ngày
        /// (tên có chữ "Date" hoặc "Period") -> dd/MM/yyyy; cột DateTime khác -> dd/MM/yyyy HH:mm.
        /// </summary>
        public static void AutoFormat(DataGridView dgv)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                Type t = col.ValueType;
                if (t == typeof(decimal))
                {
                    SetMoneyStyle(col);
                }
                else if (t == typeof(DateTime))
                {
                    bool dateOnly = col.Name.IndexOf("Date", StringComparison.OrdinalIgnoreCase) >= 0 ||
                                    col.Name.Equals("Period", StringComparison.OrdinalIgnoreCase);
                    col.DefaultCellStyle.Format = dateOnly ? Formatters.DateFormat : Formatters.DateTimeFormat;
                }
            }
        }

        /// <summary>
        /// Chỉ hiện các cột được liệt kê, theo đúng thứ tự, với tiêu đề tiếng Việt.
        /// Tham số là các cặp (tên cột, tiêu đề). Cột không liệt kê bị ẩn (vẫn đọc
        /// được bằng SelectedRow/SelectedId). Gọi sau Bind.
        /// </summary>
        public static void ShowColumns(DataGridView dgv, params string[] nameHeaderPairs)
        {
            if (nameHeaderPairs == null || nameHeaderPairs.Length % 2 != 0)
                throw new ArgumentException("ShowColumns cần các cặp (tên cột, tiêu đề).", "nameHeaderPairs");

            foreach (DataGridViewColumn col in dgv.Columns) col.Visible = false;

            int order = 0;
            for (int i = 0; i < nameHeaderPairs.Length; i += 2)
            {
                DataGridViewColumn col = dgv.Columns[nameHeaderPairs[i]];
                if (col == null) continue;   // view không có cột này: bỏ qua, không làm văng form
                col.Visible = true;
                col.HeaderText = nameHeaderPairs[i + 1];
                col.DisplayIndex = order++;
            }
        }

        /// <summary>Đặt tiêu đề cho các cột (không ẩn cột nào). Tham số là các cặp (tên cột, tiêu đề).</summary>
        public static void SetHeaders(DataGridView dgv, params string[] nameHeaderPairs)
        {
            if (nameHeaderPairs == null || nameHeaderPairs.Length % 2 != 0)
                throw new ArgumentException("SetHeaders cần các cặp (tên cột, tiêu đề).", "nameHeaderPairs");
            for (int i = 0; i < nameHeaderPairs.Length; i += 2)
            {
                DataGridViewColumn col = dgv.Columns[nameHeaderPairs[i]];
                if (col != null) col.HeaderText = nameHeaderPairs[i + 1];
            }
        }

        public static void HideColumns(DataGridView dgv, params string[] names)
        {
            foreach (string n in names)
            {
                DataGridViewColumn col = dgv.Columns[n];
                if (col != null) col.Visible = false;
            }
        }

        public static void FormatMoneyColumns(DataGridView dgv, params string[] names)
        {
            foreach (string n in names)
            {
                DataGridViewColumn col = dgv.Columns[n];
                if (col != null) SetMoneyStyle(col);
            }
        }

        public static void FormatDateColumns(DataGridView dgv, params string[] names)
        {
            SetFormat(dgv, Formatters.DateFormat, names);
        }

        public static void FormatDateTimeColumns(DataGridView dgv, params string[] names)
        {
            SetFormat(dgv, Formatters.DateTimeFormat, names);
        }

        /// <summary>
        /// Hiển thị chữ tiếng Việt thay cho mã trạng thái trong một cột
        /// (dữ liệu gốc không đổi). Gọi MỘT lần trong Load, không gọi lại mỗi lần nạp.
        ///   UiHelper.TranslateColumn(dgv, "RoomStatus", RoomStatus.ToVietnamese);
        /// </summary>
        public static void TranslateColumn(DataGridView dgv, string columnName, Func<string, string> translate)
        {
            dgv.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
                if (dgv.Columns[e.ColumnIndex].Name != columnName) return;
                if (e.Value == null || e.Value == DBNull.Value) return;
                e.Value = translate(e.Value.ToString());
                e.FormattingApplied = true;
            };
        }

        /// <summary>
        /// Tô màu nền cả dòng theo giá trị của một cột (đọc giá trị gốc, không phải chữ đã dịch).
        /// Gọi MỘT lần trong Load.
        ///   UiHelper.ColorRows(dgvRooms, "Status", RoomStatus.ToColor);
        /// </summary>
        public static void ColorRows(DataGridView dgv, string columnName, Func<string, Color> colorOf)
        {
            dgv.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex < 0) return;
                var drv = dgv.Rows[e.RowIndex].DataBoundItem as DataRowView;
                if (drv == null || !drv.Row.Table.Columns.Contains(columnName)) return;
                object v = drv[columnName];
                if (v == null || v == DBNull.Value) return;
                Color c = colorOf(v.ToString());
                if (c != Color.Empty) e.CellStyle.BackColor = c;
            };
        }

        /// <summary>DataRow của dòng đang chọn; null nếu chưa chọn.</summary>
        public static DataRow SelectedRow(DataGridView dgv)
        {
            if (dgv == null || dgv.CurrentRow == null) return null;
            var drv = dgv.CurrentRow.DataBoundItem as DataRowView;
            return drv == null ? null : drv.Row;
        }

        /// <summary>Giá trị khóa (int) của dòng đang chọn; null nếu chưa chọn.</summary>
        public static int? SelectedId(DataGridView dgv, string keyColumn)
        {
            DataRow row = SelectedRow(dgv);
            if (row == null || !row.Table.Columns.Contains(keyColumn)) return null;
            return DbHelper.Value<int?>(row, keyColumn);
        }

        /// <summary>Đọc một cột bất kỳ của dòng đang chọn.</summary>
        public static T SelectedValue<T>(DataGridView dgv, string column, T defaultValue = default(T))
        {
            DataRow row = SelectedRow(dgv);
            if (row == null || !row.Table.Columns.Contains(column)) return defaultValue;
            return DbHelper.Value(row, column, defaultValue);
        }

        /// <summary>Chọn dòng có cột khóa = id (dùng sau khi nạp lại lưới).</summary>
        public static bool SelectRowById(DataGridView dgv, string keyColumn, int id)
        {
            int firstVisible = -1;
            foreach (DataGridViewColumn c in dgv.Columns)
            {
                if (c.Visible && (firstVisible < 0 || c.DisplayIndex < dgv.Columns[firstVisible].DisplayIndex))
                    firstVisible = c.Index;
            }
            if (firstVisible < 0) return false;

            foreach (DataGridViewRow r in dgv.Rows)
            {
                var drv = r.DataBoundItem as DataRowView;
                if (drv == null || !drv.Row.Table.Columns.Contains(keyColumn)) continue;
                object v = drv[keyColumn];
                if (v != DBNull.Value && Convert.ToInt32(v) == id)
                {
                    dgv.CurrentCell = r.Cells[firstVisible];
                    return true;
                }
            }
            return false;
        }

        // ==================================================================
        // 2. Hộp thông báo
        // ==================================================================

        public static void Info(string message, IWin32Window owner = null)
        {
            MessageBox.Show(owner, message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void Warn(string message, IWin32Window owner = null)
        {
            MessageBox.Show(owner, message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void Error(string message, IWin32Window owner = null)
        {
            MessageBox.Show(owner, message, AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Hỏi Có/Không. defaultNo = true cho thao tác nguy hiểm (void, hủy phiếu, xóa)
        /// để lỡ bấm Enter cũng không thực hiện.
        /// </summary>
        public static bool Confirm(string message, bool defaultNo = false, IWin32Window owner = null)
        {
            DialogResult r = MessageBox.Show(owner, message, "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                defaultNo ? MessageBoxDefaultButton.Button2 : MessageBoxDefaultButton.Button1);
            return r == DialogResult.Yes;
        }

        // ==================================================================
        // 3. Kiểm tra nhập liệu (kiểm tra sơ bộ; procedure vẫn kiểm tra lại)
        // ==================================================================

        /// <summary>Ô bắt buộc nhập. Trống thì báo "Hãy nhập {label}." và đặt con trỏ vào ô.</summary>
        public static bool RequireText(TextBox txt, string label)
        {
            if (!string.IsNullOrWhiteSpace(txt.Text)) return true;
            Warn("Hãy nhập " + label + ".", txt.FindForm());
            txt.Focus();
            return false;
        }

        /// <summary>ComboBox bắt buộc chọn (dòng "-- Chọn --" coi như chưa chọn).</summary>
        public static bool RequireSelection(ComboBox cbo, string label)
        {
            var item = cbo.SelectedItem as ComboItem;
            bool ok = item != null ? item.Value != null : cbo.SelectedIndex >= 0;
            if (ok) return true;
            Warn("Hãy chọn " + label + ".", cbo.FindForm());
            cbo.Focus();
            return false;
        }

        /// <summary>Lưới phải có dòng đang chọn.</summary>
        public static bool RequireSelectedRow(DataGridView dgv, string what = "một dòng trong danh sách")
        {
            if (SelectedRow(dgv) != null) return true;
            Warn("Hãy chọn " + what + ".", dgv.FindForm());
            return false;
        }

        // ==================================================================
        // 4. Phân quyền giao diện
        // ==================================================================

        /// <summary>Chỉ hiện control cho các vai trò truyền vào: UiHelper.VisibleFor(btnVoid, Roles.Admin, Roles.Manager);</summary>
        public static void VisibleFor(Control control, params string[] roles)
        {
            control.Visible = Session.HasRole(roles);
        }

        public static void VisibleFor(ToolStripItem item, params string[] roles)
        {
            item.Visible = Session.HasRole(roles);
        }

        public static void EnabledFor(Control control, params string[] roles)
        {
            control.Enabled = Session.HasRole(roles);
        }

        public static void SetEnabled(bool enabled, params Control[] controls)
        {
            foreach (Control c in controls)
            {
                if (c != null) c.Enabled = enabled;
            }
        }

        // ==================================================================
        // 5. Khác
        // ==================================================================

        /// <summary>
        /// Hiện con trỏ chờ trong lúc chạy thao tác lâu:
        ///   using (UiHelper.Busy(this)) { ... }
        /// </summary>
        public static IDisposable Busy(Control owner = null)
        {
            return new BusyScope(owner);
        }

        /// <summary>Cấu hình NumericUpDown để nhập tiền: số nguyên, có dấu ngăn nghìn, bước 1.000.</summary>
        public static void SetupMoneyInput(NumericUpDown nud, decimal maximum = 99999999999999m)
        {
            nud.DecimalPlaces = 0;
            nud.ThousandsSeparator = true;
            nud.Minimum = 0;
            nud.Maximum = maximum;
            nud.Increment = 1000;
            nud.TextAlign = HorizontalAlignment.Right;
        }

        /// <summary>
        /// Cấu hình chuẩn cho hộp thoại (dlgCheckIn, dlgPayment...): khung cố định, giữa
        /// form cha, Enter = nút OK, Esc = nút Hủy. Gọi trong constructor sau InitializeComponent().
        /// Nút OK tự đặt DialogResult = OK khi lưu thành công; nút Hủy có DialogResult = Cancel.
        /// </summary>
        public static void SetupDialog(Form dlg, Button okButton, Button cancelButton)
        {
            dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
            dlg.MaximizeBox = false;
            dlg.MinimizeBox = false;
            dlg.ShowInTaskbar = false;
            dlg.StartPosition = FormStartPosition.CenterParent;
            if (okButton != null) dlg.AcceptButton = okButton;
            if (cancelButton != null)
            {
                cancelButton.DialogResult = DialogResult.Cancel;
                dlg.CancelButton = cancelButton;
            }
        }

        /// <summary>
        /// Xuất các cột đang hiện của lưới ra file CSV (mở được bằng Excel, giữ dấu tiếng Việt).
        /// Trả về true nếu đã lưu.
        /// </summary>
        public static bool ExportCsv(DataGridView dgv, string defaultFileName, IWin32Window owner = null)
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.Filter = "CSV (*.csv)|*.csv";
                dlg.FileName = defaultFileName;
                if (dlg.ShowDialog(owner) != DialogResult.OK) return false;

                var columns = new System.Collections.Generic.List<DataGridViewColumn>();
                foreach (DataGridViewColumn c in dgv.Columns)
                {
                    if (c.Visible) columns.Add(c);
                }
                columns.Sort((a, b) => a.DisplayIndex.CompareTo(b.DisplayIndex));

                var sb = new StringBuilder();
                for (int i = 0; i < columns.Count; i++)
                {
                    if (i > 0) sb.Append(',');
                    sb.Append(CsvEscape(columns[i].HeaderText));
                }
                sb.AppendLine();

                foreach (DataGridViewRow r in dgv.Rows)
                {
                    if (r.IsNewRow) continue;
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (i > 0) sb.Append(',');
                        object v = r.Cells[columns[i].Index].FormattedValue;
                        sb.Append(CsvEscape(v == null ? "" : v.ToString()));
                    }
                    sb.AppendLine();
                }

                File.WriteAllText(dlg.FileName, sb.ToString(), new UTF8Encoding(true));
                return true;
            }
        }

        // ------------------------------------------------------------------
        // Nội bộ
        // ------------------------------------------------------------------

        private static void SetMoneyStyle(DataGridViewColumn col)
        {
            col.DefaultCellStyle.Format = Formatters.MoneyFormat;
            col.DefaultCellStyle.FormatProvider = Formatters.Vi;
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private static void SetFormat(DataGridView dgv, string format, string[] names)
        {
            foreach (string n in names)
            {
                DataGridViewColumn col = dgv.Columns[n];
                if (col != null) col.DefaultCellStyle.Format = format;
            }
        }

        private static string CsvEscape(string s)
        {
            if (s.IndexOfAny(new[] { ',', '"', '\n', '\r' }) < 0) return s;
            return "\"" + s.Replace("\"", "\"\"") + "\"";
        }

        private sealed class BusyScope : IDisposable
        {
            private readonly Control _owner;
            private readonly Cursor _previous;

            public BusyScope(Control owner)
            {
                _owner = owner;
                _previous = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                if (_owner != null) _owner.UseWaitCursor = true;
            }

            public void Dispose()
            {
                if (_owner != null && !_owner.IsDisposed) _owner.UseWaitCursor = false;
                Cursor.Current = _previous ?? Cursors.Default;
            }
        }
    }
}
