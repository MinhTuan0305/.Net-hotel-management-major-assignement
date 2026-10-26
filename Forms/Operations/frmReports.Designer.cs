namespace Hotel_management_system.Forms.Operations
{
    partial class frmReports
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartAreaRevenue = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legendRevenue = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series seriesRevenue = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartAreaRooms = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legendRooms = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series seriesRooms = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.cboGroupBy = new System.Windows.Forms.ComboBox();
            this.lblGroupBy = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabRevenue = new System.Windows.Forms.TabPage();
            this.chartRevenue = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlRevenueLeft = new System.Windows.Forms.Panel();
            this.dgvSales = new System.Windows.Forms.DataGridView();
            this.lblSales = new System.Windows.Forms.Label();
            this.dgvCash = new System.Windows.Forms.DataGridView();
            this.lblCash = new System.Windows.Forms.Label();
            this.lblRevenueSummary = new System.Windows.Forms.Label();
            this.lblRevenueNote = new System.Windows.Forms.Label();
            this.tabServices = new System.Windows.Forms.TabPage();
            this.dgvServices = new System.Windows.Forms.DataGridView();
            this.lblServicesSummary = new System.Windows.Forms.Label();
            this.tabRooms = new System.Windows.Forms.TabPage();
            this.chartRooms = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvRooms = new System.Windows.Forms.DataGridView();
            this.lblRoomsSummary = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabRevenue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).BeginInit();
            this.pnlRevenueLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCash)).BeginInit();
            this.tabServices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();
            this.tabRooms.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRooms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.btnExport);
            this.pnlTop.Controls.Add(this.btnView);
            this.pnlTop.Controls.Add(this.cboGroupBy);
            this.pnlTop.Controls.Add(this.lblGroupBy);
            this.pnlTop.Controls.Add(this.dtpTo);
            this.pnlTop.Controls.Add(this.lblTo);
            this.pnlTop.Controls.Add(this.dtpFrom);
            this.pnlTop.Controls.Add(this.lblFrom);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1184, 44);
            this.pnlTop.TabIndex = 0;
            //
            // btnExport
            //
            this.btnExport.Location = new System.Drawing.Point(620, 9);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 27);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Xuất CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            //
            // btnView
            //
            this.btnView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnView.Location = new System.Drawing.Point(510, 9);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(100, 27);
            this.btnView.TabIndex = 6;
            this.btnView.Text = "Xem báo cáo";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            //
            // cboGroupBy
            //
            this.cboGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroupBy.Location = new System.Drawing.Point(396, 11);
            this.cboGroupBy.Name = "cboGroupBy";
            this.cboGroupBy.Size = new System.Drawing.Size(100, 23);
            this.cboGroupBy.TabIndex = 5;
            //
            // lblGroupBy
            //
            this.lblGroupBy.AutoSize = true;
            this.lblGroupBy.Location = new System.Drawing.Point(334, 14);
            this.lblGroupBy.Name = "lblGroupBy";
            this.lblGroupBy.Size = new System.Drawing.Size(58, 15);
            this.lblGroupBy.TabIndex = 4;
            this.lblGroupBy.Text = "Nhóm theo:";
            //
            // dtpTo
            //
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(210, 11);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 23);
            this.dtpTo.TabIndex = 3;
            //
            // lblTo
            //
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(178, 14);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 15);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "đến";
            //
            // dtpFrom
            //
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(60, 11);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 23);
            this.dtpFrom.TabIndex = 1;
            //
            // lblFrom
            //
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(12, 14);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(45, 15);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Từ ngày:";
            //
            // tabMain
            //
            this.tabMain.Controls.Add(this.tabRevenue);
            this.tabMain.Controls.Add(this.tabServices);
            this.tabMain.Controls.Add(this.tabRooms);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 44);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1184, 617);
            this.tabMain.TabIndex = 1;
            //
            // tabRevenue
            //
            this.tabRevenue.Controls.Add(this.chartRevenue);
            this.tabRevenue.Controls.Add(this.pnlRevenueLeft);
            this.tabRevenue.Controls.Add(this.lblRevenueSummary);
            this.tabRevenue.Controls.Add(this.lblRevenueNote);
            this.tabRevenue.Location = new System.Drawing.Point(4, 24);
            this.tabRevenue.Name = "tabRevenue";
            this.tabRevenue.Padding = new System.Windows.Forms.Padding(3);
            this.tabRevenue.Size = new System.Drawing.Size(1176, 589);
            this.tabRevenue.TabIndex = 0;
            this.tabRevenue.Text = "Doanh thu";
            this.tabRevenue.UseVisualStyleBackColor = true;
            //
            // chartRevenue
            //
            chartAreaRevenue.AxisX.MajorGrid.Enabled = false;
            chartAreaRevenue.AxisY.LabelStyle.Format = "#,##0";
            chartAreaRevenue.Name = "AreaRevenue";
            this.chartRevenue.ChartAreas.Add(chartAreaRevenue);
            this.chartRevenue.Dock = System.Windows.Forms.DockStyle.Fill;
            legendRevenue.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legendRevenue.Name = "LegendRevenue";
            this.chartRevenue.Legends.Add(legendRevenue);
            this.chartRevenue.Location = new System.Drawing.Point(623, 25);
            this.chartRevenue.Name = "chartRevenue";
            seriesRevenue.ChartArea = "AreaRevenue";
            seriesRevenue.Legend = "LegendRevenue";
            seriesRevenue.Name = "Thực thu";
            this.chartRevenue.Series.Add(seriesRevenue);
            this.chartRevenue.Size = new System.Drawing.Size(550, 533);
            this.chartRevenue.TabIndex = 2;
            //
            // pnlRevenueLeft
            //
            this.pnlRevenueLeft.Controls.Add(this.dgvSales);
            this.pnlRevenueLeft.Controls.Add(this.lblSales);
            this.pnlRevenueLeft.Controls.Add(this.dgvCash);
            this.pnlRevenueLeft.Controls.Add(this.lblCash);
            this.pnlRevenueLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlRevenueLeft.Location = new System.Drawing.Point(3, 25);
            this.pnlRevenueLeft.Name = "pnlRevenueLeft";
            this.pnlRevenueLeft.Size = new System.Drawing.Size(620, 533);
            this.pnlRevenueLeft.TabIndex = 1;
            //
            // dgvSales
            //
            this.dgvSales.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSales.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSales.Location = new System.Drawing.Point(0, 300);
            this.dgvSales.Name = "dgvSales";
            this.dgvSales.Size = new System.Drawing.Size(620, 233);
            this.dgvSales.TabIndex = 3;
            //
            // lblSales
            //
            this.lblSales.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSales.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSales.Location = new System.Drawing.Point(0, 276);
            this.lblSales.Name = "lblSales";
            this.lblSales.Size = new System.Drawing.Size(620, 24);
            this.lblSales.TabIndex = 2;
            this.lblSales.Text = "Doanh số theo hóa đơn (ngày lập, chỉ hóa đơn hiệu lực)";
            this.lblSales.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // dgvCash
            //
            this.dgvCash.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCash.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvCash.Location = new System.Drawing.Point(0, 24);
            this.dgvCash.Name = "dgvCash";
            this.dgvCash.Size = new System.Drawing.Size(620, 252);
            this.dgvCash.TabIndex = 1;
            //
            // lblCash
            //
            this.lblCash.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCash.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCash.Location = new System.Drawing.Point(0, 0);
            this.lblCash.Name = "lblCash";
            this.lblCash.Size = new System.Drawing.Size(620, 24);
            this.lblCash.TabIndex = 0;
            this.lblCash.Text = "Thực thu (theo ngày nhận tiền, gồm cả cọc, trừ hoàn)";
            this.lblCash.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            //
            // lblRevenueSummary
            //
            this.lblRevenueSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRevenueSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblRevenueSummary.Location = new System.Drawing.Point(3, 558);
            this.lblRevenueSummary.Name = "lblRevenueSummary";
            this.lblRevenueSummary.Size = new System.Drawing.Size(1170, 28);
            this.lblRevenueSummary.TabIndex = 3;
            this.lblRevenueSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // lblRevenueNote
            //
            this.lblRevenueNote.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRevenueNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lblRevenueNote.Location = new System.Drawing.Point(3, 3);
            this.lblRevenueNote.Name = "lblRevenueNote";
            this.lblRevenueNote.Size = new System.Drawing.Size(1170, 22);
            this.lblRevenueNote.TabIndex = 0;
            this.lblRevenueNote.Text = "Thực thu tính theo ngày nhận tiền (gồm cả cọc); doanh số tính theo ngày lập hóa đơn. Hai số khác nhau là bình thường.";
            //
            // tabServices
            //
            this.tabServices.Controls.Add(this.dgvServices);
            this.tabServices.Controls.Add(this.lblServicesSummary);
            this.tabServices.Location = new System.Drawing.Point(4, 24);
            this.tabServices.Name = "tabServices";
            this.tabServices.Padding = new System.Windows.Forms.Padding(3);
            this.tabServices.Size = new System.Drawing.Size(1176, 589);
            this.tabServices.TabIndex = 1;
            this.tabServices.Text = "Dịch vụ";
            this.tabServices.UseVisualStyleBackColor = true;
            //
            // dgvServices
            //
            this.dgvServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvServices.Location = new System.Drawing.Point(3, 3);
            this.dgvServices.Name = "dgvServices";
            this.dgvServices.Size = new System.Drawing.Size(1170, 555);
            this.dgvServices.TabIndex = 0;
            //
            // lblServicesSummary
            //
            this.lblServicesSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblServicesSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblServicesSummary.Location = new System.Drawing.Point(3, 558);
            this.lblServicesSummary.Name = "lblServicesSummary";
            this.lblServicesSummary.Size = new System.Drawing.Size(1170, 28);
            this.lblServicesSummary.TabIndex = 1;
            this.lblServicesSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // tabRooms
            //
            this.tabRooms.Controls.Add(this.chartRooms);
            this.tabRooms.Controls.Add(this.dgvRooms);
            this.tabRooms.Controls.Add(this.lblRoomsSummary);
            this.tabRooms.Location = new System.Drawing.Point(4, 24);
            this.tabRooms.Name = "tabRooms";
            this.tabRooms.Padding = new System.Windows.Forms.Padding(3);
            this.tabRooms.Size = new System.Drawing.Size(1176, 589);
            this.tabRooms.TabIndex = 2;
            this.tabRooms.Text = "Phòng";
            this.tabRooms.UseVisualStyleBackColor = true;
            //
            // chartRooms
            //
            chartAreaRooms.Name = "AreaRooms";
            this.chartRooms.ChartAreas.Add(chartAreaRooms);
            this.chartRooms.Dock = System.Windows.Forms.DockStyle.Fill;
            legendRooms.Name = "LegendRooms";
            this.chartRooms.Legends.Add(legendRooms);
            this.chartRooms.Location = new System.Drawing.Point(423, 3);
            this.chartRooms.Name = "chartRooms";
            seriesRooms.ChartArea = "AreaRooms";
            seriesRooms.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            seriesRooms.Legend = "LegendRooms";
            seriesRooms.Name = "Phòng";
            this.chartRooms.Series.Add(seriesRooms);
            this.chartRooms.Size = new System.Drawing.Size(750, 555);
            this.chartRooms.TabIndex = 1;
            //
            // dgvRooms
            //
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRooms.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvRooms.Location = new System.Drawing.Point(3, 3);
            this.dgvRooms.Name = "dgvRooms";
            this.dgvRooms.Size = new System.Drawing.Size(420, 555);
            this.dgvRooms.TabIndex = 0;
            //
            // lblRoomsSummary
            //
            this.lblRoomsSummary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblRoomsSummary.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblRoomsSummary.Location = new System.Drawing.Point(3, 558);
            this.lblRoomsSummary.Name = "lblRoomsSummary";
            this.lblRoomsSummary.Size = new System.Drawing.Size(1170, 28);
            this.lblRoomsSummary.TabIndex = 2;
            this.lblRoomsSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // frmReports
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1000, 560);
            this.Name = "frmReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Báo cáo";
            this.Load += new System.EventHandler(this.frmReports_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tabRevenue.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenue)).EndInit();
            this.pnlRevenueLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCash)).EndInit();
            this.tabServices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
            this.tabRooms.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartRooms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblGroupBy;
        private System.Windows.Forms.ComboBox cboGroupBy;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabRevenue;
        private System.Windows.Forms.Label lblRevenueNote;
        private System.Windows.Forms.Panel pnlRevenueLeft;
        private System.Windows.Forms.Label lblCash;
        private System.Windows.Forms.DataGridView dgvCash;
        private System.Windows.Forms.Label lblSales;
        private System.Windows.Forms.DataGridView dgvSales;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenue;
        private System.Windows.Forms.Label lblRevenueSummary;
        private System.Windows.Forms.TabPage tabServices;
        private System.Windows.Forms.DataGridView dgvServices;
        private System.Windows.Forms.Label lblServicesSummary;
        private System.Windows.Forms.TabPage tabRooms;
        private System.Windows.Forms.DataGridView dgvRooms;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRooms;
        private System.Windows.Forms.Label lblRoomsSummary;
    }
}
