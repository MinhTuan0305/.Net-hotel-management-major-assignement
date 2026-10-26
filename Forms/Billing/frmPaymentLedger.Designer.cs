namespace Hotel_management_system.Forms.Billing
{
    partial class frmPaymentLedger
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
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.cboEmployee = new System.Windows.Forms.ComboBox();
            this.lblEmployee = new System.Windows.Forms.Label();
            this.cboMethod = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cboType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dgvLedger = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnRefund = new System.Windows.Forms.Button();
            this.lblByMethod = new System.Windows.Forms.Label();
            this.lblSummary = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).BeginInit();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.Controls.Add(this.btnRefresh);
            this.pnlTop.Controls.Add(this.cboEmployee);
            this.pnlTop.Controls.Add(this.lblEmployee);
            this.pnlTop.Controls.Add(this.cboMethod);
            this.pnlTop.Controls.Add(this.lblMethod);
            this.pnlTop.Controls.Add(this.cboType);
            this.pnlTop.Controls.Add(this.lblType);
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
            // btnRefresh
            //
            this.btnRefresh.Location = new System.Drawing.Point(1040, 9);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 10;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            //
            // cboEmployee
            //
            this.cboEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEmployee.Location = new System.Drawing.Point(850, 11);
            this.cboEmployee.Name = "cboEmployee";
            this.cboEmployee.Size = new System.Drawing.Size(170, 23);
            this.cboEmployee.TabIndex = 9;
            this.cboEmployee.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblEmployee
            //
            this.lblEmployee.AutoSize = true;
            this.lblEmployee.Location = new System.Drawing.Point(784, 14);
            this.lblEmployee.Name = "lblEmployee";
            this.lblEmployee.Size = new System.Drawing.Size(63, 15);
            this.lblEmployee.TabIndex = 8;
            this.lblEmployee.Text = "Người thu:";
            //
            // cboMethod
            //
            this.cboMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMethod.Location = new System.Drawing.Point(640, 11);
            this.cboMethod.Name = "cboMethod";
            this.cboMethod.Size = new System.Drawing.Size(130, 23);
            this.cboMethod.TabIndex = 7;
            this.cboMethod.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblMethod
            //
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(564, 14);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(73, 15);
            this.lblMethod.TabIndex = 6;
            this.lblMethod.Text = "Phương thức:";
            //
            // cboType
            //
            this.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboType.Location = new System.Drawing.Point(420, 11);
            this.cboType.Name = "cboType";
            this.cboType.Size = new System.Drawing.Size(130, 23);
            this.cboType.TabIndex = 5;
            this.cboType.SelectedIndexChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblType
            //
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(386, 14);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 15);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "Loại:";
            //
            // dtpTo
            //
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(256, 11);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(110, 23);
            this.dtpTo.TabIndex = 3;
            this.dtpTo.ValueChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblTo
            //
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(222, 14);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 15);
            this.lblTo.TabIndex = 2;
            this.lblTo.Text = "đến";
            //
            // dtpFrom
            //
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(104, 11);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(110, 23);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.ValueChanged += new System.EventHandler(this.filter_Changed);
            //
            // lblFrom
            //
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(12, 14);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(86, 15);
            this.lblFrom.TabIndex = 0;
            this.lblFrom.Text = "Ngày thu từ:";
            //
            // dgvLedger
            //
            this.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLedger.Location = new System.Drawing.Point(0, 44);
            this.dgvLedger.Name = "dgvLedger";
            this.dgvLedger.Size = new System.Drawing.Size(1184, 507);
            this.dgvLedger.TabIndex = 1;
            this.dgvLedger.SelectionChanged += new System.EventHandler(this.dgvLedger_SelectionChanged);
            //
            // pnlBottom
            //
            this.pnlBottom.Controls.Add(this.btnExport);
            this.pnlBottom.Controls.Add(this.btnRefund);
            this.pnlBottom.Controls.Add(this.lblByMethod);
            this.pnlBottom.Controls.Add(this.lblSummary);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 551);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1184, 70);
            this.pnlBottom.TabIndex = 2;
            //
            // btnExport
            //
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(1072, 18);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 34);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Xuất CSV";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            //
            // btnRefund
            //
            this.btnRefund.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefund.Location = new System.Drawing.Point(906, 18);
            this.btnRefund.Name = "btnRefund";
            this.btnRefund.Size = new System.Drawing.Size(160, 34);
            this.btnRefund.TabIndex = 2;
            this.btnRefund.Text = "Ghi hoàn thủ công...";
            this.btnRefund.UseVisualStyleBackColor = true;
            this.btnRefund.Click += new System.EventHandler(this.btnRefund_Click);
            //
            // lblByMethod
            //
            this.lblByMethod.Location = new System.Drawing.Point(12, 38);
            this.lblByMethod.Name = "lblByMethod";
            this.lblByMethod.Size = new System.Drawing.Size(880, 22);
            this.lblByMethod.TabIndex = 1;
            //
            // lblSummary
            //
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary.Location = new System.Drawing.Point(12, 10);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(880, 24);
            this.lblSummary.TabIndex = 0;
            //
            // frmPaymentLedger
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 621);
            this.Controls.Add(this.dgvLedger);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(1100, 500);
            this.Name = "frmPaymentLedger";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sổ thu";
            this.Load += new System.EventHandler(this.frmPaymentLedger_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).EndInit();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cboType;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cboMethod;
        private System.Windows.Forms.Label lblEmployee;
        private System.Windows.Forms.ComboBox cboEmployee;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.DataGridView dgvLedger;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.Label lblByMethod;
        private System.Windows.Forms.Button btnRefund;
        private System.Windows.Forms.Button btnExport;
    }
}
