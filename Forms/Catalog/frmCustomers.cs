using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HotelManagement.Core;

namespace Hotel_management_system.Forms.Catalog
{
    /// <summary>frmCustomers – người làm: B. Quyền: A,M,R. Đặc tả: docs/KE_HOACH_TRIEN_KHAI.md mục 2.</summary>
    public partial class frmCustomers : Form
    {
        /// <summary>Khách được chọn khi mở ở chế độ chọn (pickMode = true).</summary>
        public int? SelectedCustomerId { get; private set; }

        public frmCustomers() : this(false)
        {
        }

        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public frmCustomers(bool pickMode)
        {
            InitializeComponent();
        }
    }
}