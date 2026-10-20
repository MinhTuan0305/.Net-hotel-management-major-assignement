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

namespace Hotel_management_system.Forms.Main
{
    /// <summary>frmMain – người làm: A. Quyền: tất cả. Đặc tả: docs/KE_HOACH_TRIEN_KHAI.md mục 1.</summary>
    public partial class frmMain : Form
    {
        /// <summary>True khi người dùng bấm Đăng xuất (Program.cs dùng để mở lại frmLogin).</summary>
        public bool LoggedOut { get; private set; }

        public frmMain()
        {
            InitializeComponent();
        }
    }
}