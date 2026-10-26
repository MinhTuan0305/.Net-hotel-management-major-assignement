using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Billing
{
    /// <summary>
    /// Xem trước và in một hóa đơn (Admin, Manager, Receptionist). Đặc tả: KE_HOACH mục 5; nghiệp vụ mục 12.
    /// Dữ liệu: vw_InvoiceDetail (đầu hóa đơn) + vw_InvoiceLines (dòng tiền phòng + từng dịch vụ, kể cả phụ thu trễ)
    /// + tổng các khoản đã cọc/đã thu/đã hoàn của phòng (Payments).
    /// Vẽ bằng PrintDocument; PrintPreviewControl hiện ngay trên form. Hóa đơn Void in chữ "ĐÃ HỦY".
    /// </summary>
    public partial class frmInvoicePrint : Form
    {
        private const string SqlHeader = "SELECT * FROM dbo.vw_InvoiceDetail WHERE InvoiceId = @Id;";
        private const string SqlLines = "SELECT * FROM dbo.vw_InvoiceLines WHERE InvoiceId = @Id ORDER BY LineNumber;";
        private const string SqlPayments = @"
SELECT Type, CAST(SUM(Amount) AS DECIMAL(14,0)) AS Total
FROM dbo.Payments WHERE BookingRoomId = @StayId GROUP BY Type;";
        private const string SqlReplaces = "SELECT InvoiceNo FROM dbo.Invoices WHERE InvoiceId = @Id;";

        private readonly int _invoiceId;
        private DataRow _header;
        private DataTable _lines;
        private decimal _deposit, _paid, _refunded;
        private string _replacesNo;
        private int _nextLine;          // dòng hóa đơn sẽ in tiếp ở trang sau
        private bool _totalsPending;    // đã in hết dòng nhưng phần tổng phải sang trang mới

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public frmInvoicePrint(int invoiceId)
        {
            InitializeComponent();
            _invoiceId = invoiceId;
        }

        private void frmInvoicePrint_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager, Roles.Receptionist)) return;
            try
            {
                DataTable header = DbHelper.QueryText(SqlHeader, DbHelper.P("@Id", _invoiceId));
                if (header.Rows.Count == 0)
                {
                    UiHelper.Warn("Không tìm thấy hóa đơn.", this);
                    BeginInvoke(new Action(Close));
                    return;
                }
                _header = header.Rows[0];
                _lines = DbHelper.QueryText(SqlLines, DbHelper.P("@Id", _invoiceId));

                DataTable pays = DbHelper.QueryText(SqlPayments,
                    DbHelper.P("@StayId", DbHelper.Value<int>(_header, "BookingRoomId")));
                foreach (DataRow p in pays.Rows)
                {
                    decimal total = DbHelper.Value<decimal>(p, "Total");
                    switch (DbHelper.Value<string>(p, "Type"))
                    {
                        case PaymentType.Deposit: _deposit = total; break;
                        case PaymentType.Payment: _paid = total; break;
                        case PaymentType.Refund: _refunded = total; break;
                    }
                }

                int? replaces = DbHelper.Value<int?>(_header, "ReplacesInvoiceId");
                if (replaces.HasValue)
                    _replacesNo = DbHelper.Scalar<string>(SqlReplaces, DbHelper.P("@Id", replaces.Value));

                string no = DbHelper.Value<string>(_header, "InvoiceNo");
                string status = DbHelper.Value<string>(_header, "Status");
                Text = "Hóa đơn " + no;
                printDocument.DocumentName = no;
                lblInfo.Text = no + " – " + InvoiceStatus.ToVietnamese(status) + " – tổng " +
                               Formatters.Money(DbHelper.Value<decimal>(_header, "TotalAmount"));
                lblInfo.ForeColor = status == InvoiceStatus.Void ? Color.FromArgb(192, 0, 0) : SystemColors.ControlText;

                ppcInvoice.Document = printDocument;
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
                BeginInvoke(new Action(Close));
            }
        }

        // ==================================================================
        // Vẽ hóa đơn
        // ==================================================================

        private void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            // Mỗi lần xem trước / in đều vẽ lại từ đầu.
            _nextLine = 0;
            _totalsPending = false;
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_header == null) return;
            Graphics g = e.Graphics;
            Rectangle m = e.MarginBounds;
            float x = m.Left, right = m.Right, width = m.Width;
            float y = m.Top;
            bool firstPage = _nextLine == 0;

            using (var fTitle = new Font("Times New Roman", 17, FontStyle.Bold))
            using (var fHotel = new Font("Times New Roman", 13, FontStyle.Bold))
            using (var fNormal = new Font("Times New Roman", 11))
            using (var fBold = new Font("Times New Roman", 11, FontStyle.Bold))
            using (var fSmall = new Font("Times New Roman", 9.5f, FontStyle.Italic))
            using (var pen = new Pen(Color.Black, 1))
            {
                float lh = fNormal.GetHeight(g) + 2;
                var rightAlign = new StringFormat { Alignment = StringAlignment.Far };
                var center = new StringFormat { Alignment = StringAlignment.Center };

                if (firstPage)
                {
                    // ---- Thông tin khách sạn + số hóa đơn ----
                    g.DrawString(HotelInfo.Name, fHotel, Brushes.Black, x, y);
                    g.DrawString("Số: " + DbHelper.Value<string>(_header, "InvoiceNo"), fBold, Brushes.Black, right, y, rightAlign);
                    y += fHotel.GetHeight(g) + 2;
                    g.DrawString("Địa chỉ: " + HotelInfo.Address, fNormal, Brushes.Black, x, y);
                    g.DrawString("Ngày: " + Formatters.DateTimeText(_header["IssuedAt"]), fNormal, Brushes.Black, right, y, rightAlign);
                    y += lh;
                    g.DrawString("Điện thoại: " + HotelInfo.Phone + "    MST: " + HotelInfo.TaxCode, fNormal, Brushes.Black, x, y);
                    y += lh + 10;

                    g.DrawString("HÓA ĐƠN THANH TOÁN", fTitle, Brushes.Black, x + width / 2, y, center);
                    y += fTitle.GetHeight(g) + 2;
                    if (_replacesNo != null)
                    {
                        g.DrawString("(Thay thế hóa đơn " + _replacesNo + " đã hủy)", fSmall, Brushes.Black, x + width / 2, y, center);
                        y += lh;
                    }
                    y += 8;

                    // ---- Khách / người mua ----
                    y = DrawField(g, "Khách lưu trú: ", DbHelper.Value<string>(_header, "GuestName"), fBold, fNormal, x, y, lh);
                    y = DrawField(g, "Người đặt: ", DbHelper.Value<string>(_header, "CustomerName") + "  –  " +
                                  DbHelper.Value<string>(_header, "CustomerPhone", ""), fBold, fNormal, x, y, lh);
                    string buyer = DbHelper.Value<string>(_header, "BuyerName");
                    string company = DbHelper.Value<string>(_header, "BuyerCompanyName");
                    string taxCode = DbHelper.Value<string>(_header, "BuyerTaxCode");
                    string address = DbHelper.Value<string>(_header, "BuyerAddress");
                    if (buyer != null) y = DrawField(g, "Người mua hàng: ", buyer, fBold, fNormal, x, y, lh);
                    if (company != null) y = DrawField(g, "Đơn vị: ", company, fBold, fNormal, x, y, lh);
                    if (taxCode != null) y = DrawField(g, "Mã số thuế: ", taxCode, fBold, fNormal, x, y, lh);
                    if (address != null) y = DrawField(g, "Địa chỉ: ", address, fBold, fNormal, x, y, lh);

                    y = DrawField(g, "Phòng: ", DbHelper.Value<string>(_header, "RoomNumber") + " (" +
                                  DbHelper.Value<string>(_header, "TypeName") + ")   ·   Nhận: " +
                                  Formatters.DateTimeText(_header["ActualCheckIn"]) + "   ·   Trả: " +
                                  Formatters.DateTimeText(_header["ActualCheckOut"]) + "   ·   " +
                                  Formatters.Nights(DbHelper.Value<int>(_header, "Nights")), fBold, fNormal, x, y, lh);
                    y += 8;
                }

                // ---- Bảng dòng hóa đơn (bỏ qua nếu trang này chỉ còn phần tổng) ----
                if (!_totalsPending)
                {
                    float[] colX = { x, x + 36, x + width * 0.56f, x + width * 0.66f, x + width * 0.74f, right };
                    float priceX = colX[4] + (colX[5] - colX[4]) * 0.45f;
                    g.DrawLine(pen, x, y, right, y);
                    g.DrawString("STT", fBold, Brushes.Black, colX[0] + 2, y + 2);
                    g.DrawString("Nội dung", fBold, Brushes.Black, colX[1] + 2, y + 2);
                    g.DrawString("ĐVT", fBold, Brushes.Black, colX[2] + 2, y + 2);
                    g.DrawString("SL", fBold, Brushes.Black, colX[4] - 4, y + 2, rightAlign);
                    g.DrawString("Đơn giá", fBold, Brushes.Black, priceX, y + 2, rightAlign);
                    g.DrawString("Thành tiền", fBold, Brushes.Black, colX[5] - 2, y + 2, rightAlign);
                    y += lh + 4;
                    g.DrawLine(pen, x, y, right, y);
                    y += 2;

                    while (_nextLine < _lines.Rows.Count)
                    {
                        // Hết chỗ (chừa 1 dòng ghi chú) mà vẫn còn dòng: sang trang sau.
                        if (y + lh > m.Bottom - lh)
                        {
                            g.DrawLine(pen, x, y + 2, right, y + 2);
                            g.DrawString("(xem tiếp trang sau)", fSmall, Brushes.Black, right, y + 6, rightAlign);
                            DrawVoidStamp(g, m);
                            e.HasMorePages = true;
                            return;
                        }

                        DataRow r = _lines.Rows[_nextLine];
                        string desc = DbHelper.Value<string>(r, "Description");
                        DateTime? usedAt = DbHelper.Value<DateTime?>(r, "UsedAt");
                        if (usedAt.HasValue) desc += " (" + usedAt.Value.ToString("dd/MM HH:mm") + ")";

                        g.DrawString((_nextLine + 1).ToString(), fNormal, Brushes.Black, colX[0] + 2, y);
                        g.DrawString(Fit(g, desc, fNormal, colX[2] - colX[1] - 6), fNormal, Brushes.Black, colX[1] + 2, y);
                        g.DrawString(DbHelper.Value<string>(r, "Unit"), fNormal, Brushes.Black, colX[2] + 2, y);
                        g.DrawString(DbHelper.Value<int>(r, "Quantity").ToString(), fNormal, Brushes.Black, colX[4] - 4, y, rightAlign);
                        g.DrawString(Formatters.Number(DbHelper.Value<decimal>(r, "UnitPrice")), fNormal, Brushes.Black, priceX, y, rightAlign);
                        g.DrawString(Formatters.Number(DbHelper.Value<decimal>(r, "Amount")), fNormal, Brushes.Black, colX[5] - 2, y, rightAlign);
                        y += lh;
                        _nextLine++;
                    }
                    g.DrawLine(pen, x, y + 2, right, y + 2);
                    y += 10;

                    // Không đủ chỗ cho phần tổng + chữ ký (khoảng 14 dòng): đẩy sang trang mới.
                    if (y + lh * 14 > m.Bottom)
                    {
                        _totalsPending = true;
                        DrawVoidStamp(g, m);
                        e.HasMorePages = true;
                        return;
                    }
                }

                // ---- Tổng tiền ----
                decimal roomCharge = DbHelper.Value<decimal>(_header, "RoomCharge");
                decimal serviceCharge = DbHelper.Value<decimal>(_header, "ServiceCharge");
                decimal discount = DbHelper.Value<decimal>(_header, "Discount");
                decimal taxRate = DbHelper.Value<decimal>(_header, "TaxRate");
                decimal tax = DbHelper.Value<decimal>(_header, "TaxAmount");
                decimal total = DbHelper.Value<decimal>(_header, "TotalAmount");

                float labelX = x + width * 0.45f;
                y = DrawTotal(g, "Tiền phòng:", roomCharge, fNormal, labelX, right, y, lh, rightAlign);
                y = DrawTotal(g, "Tiền dịch vụ:", serviceCharge, fNormal, labelX, right, y, lh, rightAlign);
                y = DrawTotal(g, "Cộng:", roomCharge + serviceCharge, fNormal, labelX, right, y, lh, rightAlign);
                if (discount > 0)
                {
                    y = DrawTotal(g, "Giảm giá:", -discount, fNormal, labelX, right, y, lh, rightAlign);
                    string reason = DbHelper.Value<string>(_header, "DiscountReason");
                    if (reason != null)
                    {
                        g.DrawString("(" + reason + ")", fSmall, Brushes.Black, right, y, rightAlign);
                        y += lh;
                    }
                }
                y = DrawTotal(g, "Thuế GTGT " + Formatters.Percent(taxRate) + ":", tax, fNormal, labelX, right, y, lh, rightAlign);
                g.DrawLine(pen, labelX, y + 1, right, y + 1);
                y += 4;
                y = DrawTotal(g, "TỔNG CỘNG THANH TOÁN:", total, fBold, labelX, right, y, lh, rightAlign);
                g.DrawString("Bằng chữ: " + Formatters.MoneyToWords(total) + ".", fSmall, Brushes.Black,
                             new RectangleF(x, y, width, lh * 2));
                y += lh * 1.6f;

                // ---- Đã thu / đã hoàn ----
                string paidLine = "Đã cọc: " + Formatters.Money(_deposit) + "    Đã thanh toán: " + Formatters.Money(_paid);
                if (_refunded > 0) paidLine += "    Đã hoàn: " + Formatters.Money(_refunded);
                g.DrawString(paidLine, fNormal, Brushes.Black, x, y);
                y += lh;
                g.DrawString("Thực thu của phòng: " + Formatters.Money(DbHelper.Value<decimal>(_header, "StayNetPaid")),
                             fNormal, Brushes.Black, x, y);
                y += lh;

                if (DbHelper.Value<string>(_header, "Status") == InvoiceStatus.Void)
                {
                    g.DrawString("Hóa đơn đã hủy lúc " + Formatters.DateTimeText(_header["VoidedAt"]) + " bởi " +
                                 DbHelper.Value<string>(_header, "VoidedByName", "") + ". Lý do: " +
                                 DbHelper.Value<string>(_header, "VoidReason", ""),
                                 fSmall, Brushes.DarkRed, new RectangleF(x, y, width, lh * 2));
                    y += lh * 1.6f;
                }
                y += 14;

                // ---- Chữ ký ----
                float half = width / 2;
                g.DrawString("Khách hàng", fBold, Brushes.Black, x + half / 2, y, center);
                g.DrawString("Người lập hóa đơn", fBold, Brushes.Black, x + half + half / 2, y, center);
                y += lh;
                g.DrawString("(Ký, ghi rõ họ tên)", fSmall, Brushes.Black, x + half / 2, y, center);
                g.DrawString("(Ký, ghi rõ họ tên)", fSmall, Brushes.Black, x + half + half / 2, y, center);
                y += lh * 3;
                g.DrawString(DbHelper.Value<string>(_header, "IssuedByName"), fNormal, Brushes.Black, x + half + half / 2, y, center);

                DrawVoidStamp(g, m);
                e.HasMorePages = false;
            }
        }

        private static float DrawField(Graphics g, string label, string value, Font fLabel, Font fValue,
                                       float x, float y, float lh)
        {
            g.DrawString(label, fLabel, Brushes.Black, x, y);
            float w = g.MeasureString(label, fLabel).Width;
            g.DrawString(value ?? "", fValue, Brushes.Black, x + w, y);
            return y + lh;
        }

        private static float DrawTotal(Graphics g, string label, decimal amount, Font f, float labelX, float right,
                                       float y, float lh, StringFormat rightAlign)
        {
            g.DrawString(label, f, Brushes.Black, labelX, y);
            g.DrawString(Formatters.Money(amount), f, Brushes.Black, right, y, rightAlign);
            return y + lh;
        }

        /// <summary>Cắt bớt chữ quá dài cho vừa cột, thêm "...".</summary>
        private static string Fit(Graphics g, string text, Font f, float width)
        {
            if (g.MeasureString(text, f).Width <= width) return text;
            while (text.Length > 3 && g.MeasureString(text + "...", f).Width > width)
                text = text.Substring(0, text.Length - 1);
            return text + "...";
        }

        /// <summary>Chữ "ĐÃ HỦY" chéo giữa trang cho hóa đơn Void.</summary>
        private void DrawVoidStamp(Graphics g, Rectangle m)
        {
            if (DbHelper.Value<string>(_header, "Status") != InvoiceStatus.Void) return;
            GraphicsState state = g.Save();
            g.TranslateTransform(m.Left + m.Width / 2f, m.Top + m.Height / 2f);
            g.RotateTransform(-30);
            using (var f = new Font("Times New Roman", 72, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.FromArgb(70, 200, 0, 0)))
            {
                var center = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("ĐÃ HỦY", f, brush, 0, 0, center);
            }
            g.Restore(state);
        }

        // ==================================================================
        // Nút
        // ==================================================================

        private void btnPrint_Click(object sender, EventArgs e)
        {
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                printDocument.Print();
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ppcInvoice.AutoZoom = false;
            ppcInvoice.Zoom = Math.Min(3.0, ppcInvoice.Zoom + 0.25);
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ppcInvoice.AutoZoom = false;
            ppcInvoice.Zoom = Math.Max(0.25, ppcInvoice.Zoom - 0.25);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
