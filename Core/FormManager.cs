// =============================================================================
// FormManager.cs
// -----------------------------------------------------------------------------
// FILE NÀY ĐỂ LÀM GÌ
//   Quy ước chung cho việc MỞ FORM, để form của người này mở form của người kia
//   theo cùng một cách:
//     - Open<T>()      : mở form lớn (frmBookings, frmFrontDesk...) thành cửa sổ
//                        riêng thuộc frmMain; nếu đã mở rồi thì chỉ đưa lên trước,
//                        không mở thêm bản thứ hai.
//     - Open<T>(key, ()=>new T(...)) : form cần tham số; mỗi key một cửa sổ
//                        (vd. frmServiceUsage của phòng 101 và phòng 205 mở song song).
//     - ShowDialog(dlg): mở hộp thoại dlg..., trả true nếu người dùng đã lưu
//                        (dlg đặt DialogResult = OK), rồi tự giải phóng hộp thoại.
//     - CloseAll()     : đóng mọi form con khi đăng xuất.
//   Kèm cơ chế báo "dữ liệu đã đổi" (DataChanged) để các màn hình đang mở tự nạp
//   lại: vd. check-out xong thì sơ đồ phòng ở frmMain và hàng đợi dọn phòng tự cập nhật.
//
// CÁCH DÙNG
//   frmMain (Load):     FormManager.Init(this);
//   Menu:               FormManager.Open<frmFrontDesk>();
//   Form có tham số:    FormManager.Open("room-" + id, () => new frmServiceUsage(id));
//   Hộp thoại:          if (FormManager.ShowDialog(new dlgCheckIn(id), this)) LoadData();
//   Sau khi lưu:        FormManager.NotifyDataChanged(DataArea.Stays);
//   Nghe thay đổi:      FormManager.Subscribe(this, area => { if (area == DataArea.Rooms) LoadRoomBoard(); });
//                       (Subscribe tự hủy đăng ký khi form đóng, không bị rò bộ nhớ.)
// =============================================================================
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HotelManagement.Core
{
    /// <summary>Tên các vùng dữ liệu dùng với FormManager.NotifyDataChanged.</summary>
    public static class DataArea
    {
        public const string Rooms = "Rooms";               // trạng thái phòng, sơ đồ phòng
        public const string Bookings = "Bookings";         // phiếu đặt, cọc
        public const string Stays = "Stays";               // check-in, gia hạn, check-out
        public const string Services = "Services";         // dịch vụ đã dùng
        public const string Payments = "Payments";         // thu / hoàn tiền
        public const string Invoices = "Invoices";         // lập, void, lập lại hóa đơn
        public const string Housekeeping = "Housekeeping"; // việc dọn phòng
        public const string Catalog = "Catalog";           // danh mục (loại phòng, dịch vụ...)
    }

    public static class FormManager
    {
        private static readonly Dictionary<string, Form> OpenedForms = new Dictionary<string, Form>();

        /// <summary>Form chính (frmMain). Mọi form con thuộc về form này.</summary>
        public static Form MainForm { get; private set; }

        /// <summary>Phát ra khi có thao tác làm đổi dữ liệu; tham số là một giá trị của DataArea.</summary>
        public static event Action<string> DataChanged;

        /// <summary>frmMain gọi trong sự kiện Load.</summary>
        public static void Init(Form mainForm)
        {
            MainForm = mainForm;
        }

        /// <summary>Mở form không cần tham số; đã mở thì đưa lên trước.</summary>
        public static T Open<T>() where T : Form, new()
        {
            return Open(null, () => new T());
        }

        /// <summary>
        /// Mở form có tham số. key phân biệt các bản mở song song của cùng một loại form
        /// (null = chỉ một bản). create chỉ được gọi khi form chưa mở.
        /// </summary>
        public static T Open<T>(string key, Func<T> create) where T : Form
        {
            if (create == null) throw new ArgumentNullException("create");
            string fullKey = typeof(T).FullName + (string.IsNullOrEmpty(key) ? "" : "#" + key);

            Form existing;
            if (OpenedForms.TryGetValue(fullKey, out existing) && !existing.IsDisposed)
            {
                if (existing.WindowState == FormWindowState.Minimized)
                    existing.WindowState = FormWindowState.Normal;
                existing.Activate();
                return (T)existing;
            }

            T form = create();
            OpenedForms[fullKey] = form;
            form.FormClosed += (sender, e) =>
            {
                Form current;
                if (OpenedForms.TryGetValue(fullKey, out current) && ReferenceEquals(current, form))
                    OpenedForms.Remove(fullKey);
            };

            if (MainForm != null && !ReferenceEquals(form, MainForm) && !MainForm.IsDisposed)
                form.Owner = MainForm;
            form.Show();
            return form;
        }

        /// <summary>True nếu đang có ít nhất một bản form loại T mở.</summary>
        public static bool IsOpen<T>() where T : Form
        {
            string prefix = typeof(T).FullName;
            foreach (KeyValuePair<string, Form> kv in OpenedForms)
            {
                if ((kv.Key == prefix || kv.Key.StartsWith(prefix + "#", StringComparison.Ordinal)) &&
                    !kv.Value.IsDisposed)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Mở hộp thoại, trả true nếu hộp thoại kết thúc với DialogResult.OK.
        /// Hộp thoại được Dispose sau khi đóng, nên đọc kết quả (vd. SelectedCustomerId)
        /// TRƯỚC khi gọi hàm này thì không được; muốn đọc kết quả thì dùng overload có onOk.
        /// </summary>
        public static bool ShowDialog(Form dialog, IWin32Window owner = null)
        {
            return ShowDialog(dialog, owner, null);
        }

        /// <summary>
        /// Như trên, nhưng chạy onOk(dialog) khi người dùng bấm OK, trước khi hộp thoại bị giải phóng:
        ///   FormManager.ShowDialog(new frmCustomers(true), this,
        ///       f => _customerId = ((frmCustomers)f).SelectedCustomerId);
        /// </summary>
        public static bool ShowDialog(Form dialog, IWin32Window owner, Action<Form> onOk)
        {
            if (dialog == null) throw new ArgumentNullException("dialog");
            using (dialog)
            {
                IWin32Window parent = owner ?? MainForm;
                DialogResult result = parent != null ? dialog.ShowDialog(parent) : dialog.ShowDialog();
                bool ok = result == DialogResult.OK;
                if (ok && onOk != null) onOk(dialog);
                return ok;
            }
        }

        /// <summary>Đóng mọi form con đang mở (khi đăng xuất). Không đóng frmMain.</summary>
        public static void CloseAll()
        {
            var forms = new List<Form>(OpenedForms.Values);
            foreach (Form f in forms)
            {
                if (!f.IsDisposed && !ReferenceEquals(f, MainForm)) f.Close();
            }
            OpenedForms.Clear();
        }

        /// <summary>Báo cho các màn hình đang mở rằng dữ liệu vùng area đã thay đổi.</summary>
        public static void NotifyDataChanged(string area)
        {
            Action<string> handler = DataChanged;
            if (handler != null) handler(area);
        }

        /// <summary>
        /// Đăng ký nghe DataChanged cho một form và tự hủy đăng ký khi form đóng.
        /// Nên dùng hàm này thay cho "FormManager.DataChanged += ..." trực tiếp.
        /// </summary>
        public static void Subscribe(Form form, Action<string> handler)
        {
            if (form == null) throw new ArgumentNullException("form");
            if (handler == null) throw new ArgumentNullException("handler");

            Action<string> safe = area =>
            {
                if (!form.IsDisposed && form.IsHandleCreated) handler(area);
            };
            DataChanged += safe;
            form.FormClosed += (sender, e) => DataChanged -= safe;
        }
    }
}
