using Bài47;
using System;
using System.Windows.Forms;

namespace Bài47
{
    public partial class Form1 : Form
    {
        // ⭐ SỬA LỖI CS0104: dùng tên đầy đủ System.Windows.Forms.Timer
        private System.Windows.Forms.Timer timerClock;

        public Form1()
        {
            InitializeComponent();
            SetupClock();
            SetStatusMessage("Chào mừng đến với hệ thống MDI");
        }

        // =====================================================
        // ⭐ GIAO TIẾP PARENT <-> CHILD
        // Form con gọi 2 hàm public này để cập nhật StatusStrip
        // =====================================================
        public void SetStatusMessage(string message)
        {
            lblMessage.Text = message;
        }

        public void SetStatusUser(string user)
        {
            lblUser.Text = "Người dùng: " + user;
        }

        // =====================================================
        // ⭐ THUẬT TOÁN QUẢN LÝ MDI CHILD (không mở trùng Form)
        // =====================================================
        private void OpenChildForm(Type formType)
        {
            // 1) Nếu Form đã mở -> chỉ Activate, KHÔNG tạo mới
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == formType)
                {
                    if (f.WindowState == FormWindowState.Minimized)
                        f.WindowState = FormWindowState.Normal;
                    f.Activate();
                    SetStatusMessage($"Đã kích hoạt: {f.Text}");
                    return;
                }
            }

            // 2) Chưa mở -> khởi tạo mới
            Form child = (Form)Activator.CreateInstance(formType);
            child.MdiParent = this;
            child.FormClosed += (s, e) =>
                SetStatusMessage($"Đã đóng: {((Form)s).Text}");
            child.Show();
            SetStatusMessage($"Đã mở: {child.Text}");
        }

        // =====================================================
        // MENU HANDLERS
        // =====================================================
        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            var rs = MessageBox.Show("Bạn có chắc muốn đăng xuất?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                foreach (Form f in this.MdiChildren) f.Close();
                SetStatusUser("guest");
                SetStatusMessage("Đã đăng xuất");
            }
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            var rs = MessageBox.Show("Thoát ứng dụng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes) Application.Exit();
        }

        private void mnuSinhVien_Click(object sender, EventArgs e)
            => OpenChildForm(typeof(FormStudent));

        private void mnuSanPham_Click(object sender, EventArgs e)
            => OpenChildForm(typeof(FormProduct));

        private void mnuCascade_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
            SetStatusMessage("Sắp xếp cửa sổ: Cascade");
        }

        private void mnuTileH_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
            SetStatusMessage("Sắp xếp cửa sổ: Tile Horizontal");
        }

        private void mnuTileV_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
            SetStatusMessage("Sắp xếp cửa sổ: Tile Vertical");
        }

        // =====================================================
        // TOOLSTRIP HANDLERS
        // =====================================================
        private void tsbSinhVien_Click(object sender, EventArgs e)
            => OpenChildForm(typeof(FormStudent));

        private void tsbSanPham_Click(object sender, EventArgs e)
            => OpenChildForm(typeof(FormProduct));

        // =====================================================
        // ĐỒNG HỒ HỆ THỐNG  ⭐ ĐÃ SỬA LỖI Timer
        // =====================================================
        private void SetupClock()
        {
            timerClock = new System.Windows.Forms.Timer { Interval = 1000 };
            timerClock.Tick += (s, e) =>
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            timerClock.Start();
        }
    }
}