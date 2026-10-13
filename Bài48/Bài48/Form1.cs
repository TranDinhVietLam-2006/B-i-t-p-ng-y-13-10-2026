using System;
using System.Windows.Forms;

namespace Bài48
{
    public partial class Form1 : Form
    {
        // Dùng tên đầy đủ để tránh lỗi CS0104 (ambiguous Timer)
        private System.Windows.Forms.Timer timerClock;

        public Form1()
        {
            InitializeComponent();
            SetupClock();
            ResetProgress();
            SetStatusMessage("Chào mừng đến với hệ thống MDI (Bài 48)");
        }

        // =====================================================
        // ⭐ CẬP NHẬT STATUSSTRIP (Form con gọi qua MDI Parent)
        // =====================================================
        public void SetStatusMessage(string message)
        {
            if (InvokeRequired) { Invoke(new Action(() => SetStatusMessage(message))); return; }
            lblMessage.Text = message;
        }

        public void SetStatusUser(string user)
        {
            lblUser.Text = "Người dùng: " + user;
        }

        
        public IProgress<int> GetProgressReporter()
        {
            return new Progress<int>(percent =>
            {
                int p = Math.Max(0, Math.Min(100, percent));
                pbLoad.Value = p;
                lblPercent.Text = p + "%";
                lblMessage.Text = $"Đang nạp dữ liệu... {p}%";
            });
        }

        public void ResetProgress()
        {
            pbLoad.Value = 0;
            lblPercent.Text = "0%";
        }

        // =====================================================
        // ⭐ QUẢN LÝ MDI CHILD – KHÔNG MỞ TRÙNG FORM
        // =====================================================
        private void OpenChildForm(Type formType)
        {
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

            Form child = (Form)Activator.CreateInstance(formType);
            child.MdiParent = this;
            child.FormClosed += (s, e) =>
                SetStatusMessage($"Đã đóng: {((Form)s).Text}");
            child.Show();
            SetStatusMessage($"Đã mở: {child.Text}");
        }

      
        private void mnuDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            foreach (Form f in this.MdiChildren) f.Close();
            SetStatusUser("guest");
            SetStatusMessage("Đã đăng xuất");
        }

        private void mnuThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Thoát ứng dụng?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void mnuSinhVien_Click(object sender, EventArgs e) => OpenChildForm(typeof(FormStudent));
        private void mnuSanPham_Click(object sender, EventArgs e) => OpenChildForm(typeof(FormProduct));

        private void mnuCascade_Click(object sender, EventArgs e)
        { this.LayoutMdi(MdiLayout.Cascade); SetStatusMessage("Sắp xếp: Cascade"); }
        private void mnuTileH_Click(object sender, EventArgs e)
        { this.LayoutMdi(MdiLayout.TileHorizontal); SetStatusMessage("Sắp xếp: Tile Horizontal"); }
        private void mnuTileV_Click(object sender, EventArgs e)
        { this.LayoutMdi(MdiLayout.TileVertical); SetStatusMessage("Sắp xếp: Tile Vertical"); }

        
        private void tsbSinhVien_Click(object sender, EventArgs e) => OpenChildForm(typeof(FormStudent));
        private void tsbSanPham_Click(object sender, EventArgs e) => OpenChildForm(typeof(FormProduct));

       
        private void SetupClock()
        {
            timerClock = new System.Windows.Forms.Timer { Interval = 1000 };
            timerClock.Tick += (s, e) =>
                lblTime.Text = DateTime.Now.ToString("HH:mm:ss dd/MM/yyyy");
            timerClock.Start();
        }
    }
}