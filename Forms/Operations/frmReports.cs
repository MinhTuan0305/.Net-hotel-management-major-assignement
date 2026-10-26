using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Operations
{
    /// <summary>
    /// Báo cáo (Admin, Manager). Đặc tả: KE_HOACH mục 6; nghiệp vụ mục 15.
    /// - Doanh thu: dbo.sp_RevenueReport trả 2 bảng (QueryMulti):
    ///     bảng 0 = THỰC THU theo ngày nhận tiền (cọc + thanh toán − hoàn, chia theo loại và phương thức);
    ///     bảng 1 = DOANH SỐ theo ngày lập hóa đơn hiệu lực. Hai số ghi nhãn riêng, không cần bằng nhau.
    ///   Biểu đồ cột thực thu theo kỳ.
    /// - Dịch vụ: vw_ServiceUsageStats cộng trong khoảng ngày, xếp theo doanh thu.
    /// - Phòng: vw_RoomStatusSummary (phòng đang khai thác theo trạng thái) + công suất.
    /// Lọc khoảng ngày phải hợp lệ (từ ngày ≤ đến ngày); DB lọc [từ, đến + 1 ngày) để không mất ngày cuối.
    /// </summary>
    public partial class frmReports : Form
    {
        private const string SqlServices = @"
SELECT ServiceName, Category, IsSystem,
       SUM(UsageCount) AS UsageCount, SUM(TotalQuantity) AS TotalQuantity,
       CAST(SUM(TotalAmount) AS DECIMAL(14,0)) AS TotalAmount
FROM dbo.vw_ServiceUsageStats
WHERE UsedDate >= @From AND UsedDate <= @To
GROUP BY ServiceName, Category, IsSystem
ORDER BY SUM(TotalAmount) DESC, ServiceName;";

        private const string SqlRooms = "SELECT * FROM dbo.vw_RoomStatusSummary;";

        public frmReports()
        {
            InitializeComponent();
            UiHelper.StyleGrid(dgvCash);
            UiHelper.StyleGrid(dgvSales);
            UiHelper.StyleGrid(dgvServices);
            UiHelper.StyleGrid(dgvRooms);
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            if (!UiHelper.RequireRole(this, Roles.Admin, Roles.Manager)) return;

            Lookups.Bind(cboGroupBy, new List<ComboItem>
            {
                new ComboItem("DAY", "Ngày"),
                new ComboItem("MONTH", "Tháng")
            });
            // Mặc định: từ đầu tháng đến hôm nay (khoảng thời gian dễ hiểu).
            dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtpTo.Value = DateTime.Today;
            LoadAll();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void LoadAll()
        {
            if (dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                UiHelper.Warn("Khoảng ngày không hợp lệ: \"từ ngày\" phải trước hoặc bằng \"đến ngày\".", this);
                return;
            }
            try
            {
                using (UiHelper.Busy(this))
                {
                    LoadRevenue();
                    LoadServices();
                    LoadRooms();
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.Show(ex, this);
            }
        }

        // ==================================================================
        // Doanh thu
        // ==================================================================

        private void LoadRevenue()
        {
            string groupBy = Lookups.SelectedString(cboGroupBy) ?? "DAY";
            DataSet ds = DbHelper.QueryMulti("dbo.sp_RevenueReport",
                DbHelper.P("@FromDate", SqlDbType.Date, dtpFrom.Value.Date),
                DbHelper.P("@ToDate", SqlDbType.Date, dtpTo.Value.Date),
                DbHelper.P("@GroupBy", groupBy),
                Session.EmployeeParam());
            DataTable cash = ds.Tables[0];
            DataTable sales = ds.Tables[1];
            string periodFormat = groupBy == "MONTH" ? "MM/yyyy" : Formatters.DateFormat;

            UiHelper.Bind(dgvCash, cash);
            UiHelper.ShowColumns(dgvCash,
                "Period", "Kỳ",
                "Deposits", "Cọc",
                "Payments", "Thanh toán",
                "Refunds", "Hoàn",
                "NetRevenue", "Thực thu",
                "Cash", "Tiền mặt",
                "Card", "Thẻ",
                "BankTransfer", "Chuyển khoản",
                "EWallet", "Ví điện tử");
            UiHelper.FormatDateColumns(dgvCash, "Period");
            dgvCash.Columns["Period"].DefaultCellStyle.Format = periodFormat;

            UiHelper.Bind(dgvSales, sales);
            UiHelper.ShowColumns(dgvSales,
                "Period", "Kỳ",
                "InvoiceCount", "Số HĐ",
                "RoomCharge", "Tiền phòng",
                "ServiceCharge", "Dịch vụ",
                "Discount", "Giảm giá",
                "TaxAmount", "VAT",
                "TotalAmount", "Doanh số");
            dgvSales.Columns["Period"].DefaultCellStyle.Format = periodFormat;

            // Biểu đồ cột thực thu theo kỳ.
            Series s = chartRevenue.Series[0];
            s.Points.Clear();
            s.ChartType = SeriesChartType.Column;
            s.IsValueShownAsLabel = cash.Rows.Count <= 12;
            s.LabelFormat = "#,##0";
            decimal net = 0, deposits = 0, payments = 0, refunds = 0;
            foreach (DataRow r in cash.Rows)
            {
                DateTime period = DbHelper.Value<DateTime>(r, "Period");
                decimal value = DbHelper.Value<decimal>(r, "NetRevenue");
                s.Points.AddXY(period.ToString(periodFormat), (double)value);
                net += value;
                deposits += DbHelper.Value<decimal>(r, "Deposits");
                payments += DbHelper.Value<decimal>(r, "Payments");
                refunds += DbHelper.Value<decimal>(r, "Refunds");
            }

            decimal salesTotal = 0;
            int invoiceCount = 0;
            foreach (DataRow r in sales.Rows)
            {
                salesTotal += DbHelper.Value<decimal>(r, "TotalAmount");
                invoiceCount += DbHelper.Value<int>(r, "InvoiceCount");
            }

            lblRevenueSummary.Text = "THỰC THU: " + Formatters.Money(net) + "  (cọc " + Formatters.Money(deposits) +
                                     " + thanh toán " + Formatters.Money(payments) + " − hoàn " + Formatters.Money(refunds) +
                                     ")     ·     DOANH SỐ HÓA ĐƠN: " + Formatters.Money(salesTotal) +
                                     " (" + invoiceCount + " hóa đơn)";
        }

        // ==================================================================
        // Dịch vụ
        // ==================================================================

        private void LoadServices()
        {
            DataTable dt = DbHelper.QueryText(SqlServices,
                DbHelper.P("@From", SqlDbType.Date, dtpFrom.Value.Date),
                DbHelper.P("@To", SqlDbType.Date, dtpTo.Value.Date));

            // Thêm cột hạng để thấy top dịch vụ.
            dt.Columns.Add("Rank", typeof(int));
            for (int i = 0; i < dt.Rows.Count; i++) dt.Rows[i]["Rank"] = i + 1;

            UiHelper.Bind(dgvServices, dt);
            UiHelper.ShowColumns(dgvServices,
                "Rank", "Hạng",
                "ServiceName", "Dịch vụ",
                "Category", "Nhóm",
                "UsageCount", "Số lượt",
                "TotalQuantity", "Tổng số lượng",
                "TotalAmount", "Doanh thu",
                "IsSystem", "Hệ thống");

            decimal total = 0;
            foreach (DataRow r in dt.Rows) total += DbHelper.Value<decimal>(r, "TotalAmount");
            lblServicesSummary.Text = dt.Rows.Count + " dịch vụ được dùng   ·   Tổng tiền dịch vụ (theo ngày dùng, chưa VAT): " +
                                      Formatters.Money(total);
        }

        // ==================================================================
        // Phòng
        // ==================================================================

        private void LoadRooms()
        {
            DataTable summary = DbHelper.QueryText(SqlRooms);
            DataRow r = summary.Rows[0];
            int total = DbHelper.Value<int>(r, "TotalRooms");
            int available = DbHelper.Value<int>(r, "AvailableRooms");
            int occupied = DbHelper.Value<int>(r, "OccupiedRooms");
            int cleaning = DbHelper.Value<int>(r, "CleaningRooms");
            int maintenance = DbHelper.Value<int>(r, "MaintenanceRooms");

            var dt = new DataTable();
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Rooms", typeof(int));
            dt.Columns.Add("Percent", typeof(string));
            AddRoomRow(dt, RoomStatus.Available, available, total);
            AddRoomRow(dt, RoomStatus.Occupied, occupied, total);
            AddRoomRow(dt, RoomStatus.Cleaning, cleaning, total);
            AddRoomRow(dt, RoomStatus.Maintenance, maintenance, total);

            UiHelper.Bind(dgvRooms, dt);
            UiHelper.ShowColumns(dgvRooms, "Status", "Trạng thái", "Rooms", "Số phòng", "Percent", "Tỷ lệ");

            Series s = chartRooms.Series[0];
            s.Points.Clear();
            foreach (DataRow row in dt.Rows)
            {
                int count = DbHelper.Value<int>(row, "Rooms");
                if (count == 0) continue;
                string code = StatusCode(DbHelper.Value<string>(row, "Status"));
                int index = s.Points.AddXY(RoomStatus.ToVietnamese(code) + " (" + count + ")", count);
                s.Points[index].Color = RoomStatus.ToColor(code);
            }

            // Công suất = phòng có khách / (phòng khai thác − phòng bảo trì); mẫu số 0 thì 0 (nghiệp vụ mục 15).
            int usable = total - maintenance;
            decimal occupancy = usable == 0 ? 0 : Math.Round(100m * occupied / usable, 1);
            lblRoomsSummary.Text = "Tổng " + total + " phòng đang khai thác   ·   Công suất hiện tại: " +
                                   Formatters.Percent(occupancy) + " (" + occupied + "/" + usable + " phòng không bảo trì)";
        }

        private static void AddRoomRow(DataTable dt, string status, int count, int total)
        {
            string percent = total == 0 ? "0%" : Formatters.Percent(Math.Round(100m * count / total, 1));
            dt.Rows.Add(RoomStatus.ToVietnamese(status), count, percent);
        }

        private static string StatusCode(string vietnamese)
        {
            foreach (string code in RoomStatus.All)
                if (RoomStatus.ToVietnamese(code) == vietnamese) return code;
            return vietnamese;
        }

        // ==================================================================
        // Xuất CSV
        // ==================================================================

        private void btnExport_Click(object sender, EventArgs e)
        {
            string range = dtpFrom.Value.ToString("yyyyMMdd") + "_" + dtpTo.Value.ToString("yyyyMMdd");
            if (tabMain.SelectedTab == tabServices)
                UiHelper.ExportCsv(dgvServices, "BaoCaoDichVu_" + range + ".csv", this);
            else if (tabMain.SelectedTab == tabRooms)
                UiHelper.ExportCsv(dgvRooms, "BaoCaoPhong_" + DateTime.Today.ToString("yyyyMMdd") + ".csv", this);
            else if (UiHelper.ExportCsv(dgvCash, "ThucThu_" + range + ".csv", this))
                UiHelper.ExportCsv(dgvSales, "DoanhSoHoaDon_" + range + ".csv", this);
        }
    }
}
