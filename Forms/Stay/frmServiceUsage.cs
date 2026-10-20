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

namespace Hotel_management_system.Forms.Stay
{
    /// <summary>frmServiceUsage – người làm: Tuấn. Quyền: A,M,R. Đặc tả: docs/KE_HOACH_TRIEN_KHAI.md mục 4.</summary>
    public partial class frmServiceUsage : Form
    {
        // Constructor đã chốt (CLAUDE.md mục 7) – không đổi tham số.
        public frmServiceUsage(int bookingRoomId)
        {
            InitializeComponent();
        }
    }
}