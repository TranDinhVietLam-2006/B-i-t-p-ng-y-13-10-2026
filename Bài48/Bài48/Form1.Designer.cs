namespace Bài48
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuStripMain;
        private ToolStripMenuItem mnuHeThong, mnuDangXuat, mnuThoat;
        private ToolStripMenuItem mnuDanhMuc, mnuSinhVien, mnuSanPham;
        private ToolStripMenuItem mnuCuaSo, mnuCascade, mnuTileH, mnuTileV;

        private ToolStrip toolStripMain;
        private ToolStripButton tsbSinhVien, tsbSanPham;
        private ToolStripSeparator tsSep1;

        private StatusStrip statusStripMain;
        private ToolStripStatusLabel lblUser, lblSpring, lblMessage, lblTime;

        // ⭐ Panel chứa ProgressBar + Label phần trăm (Bài 4.8)
        private Panel panelProgress;
        private ProgressBar pbLoad;
        private Label lblPercent;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.mnuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDanhMuc = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSinhVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSanPham = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCuaSo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCascade = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTileH = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTileV = new System.Windows.Forms.ToolStripMenuItem();

            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.tsbSinhVien = new System.Windows.Forms.ToolStripButton();
            this.tsbSanPham = new System.Windows.Forms.ToolStripButton();
            this.tsSep1 = new System.Windows.Forms.ToolStripSeparator();

            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.lblUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();

            this.panelProgress = new System.Windows.Forms.Panel();
            this.pbLoad = new System.Windows.Forms.ProgressBar();
            this.lblPercent = new System.Windows.Forms.Label();

            // ===== MenuStrip =====
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuHeThong, this.mnuDanhMuc, this.mnuCuaSo});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(950, 24);

            this.mnuHeThong.Text = "Hệ thống";
            this.mnuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuDangXuat, this.mnuThoat});
            this.mnuDangXuat.Text = "Đăng xuất";
            this.mnuDangXuat.Click += new System.EventHandler(this.mnuDangXuat_Click);
            this.mnuThoat.Text = "Thoát";
            this.mnuThoat.Click += new System.EventHandler(this.mnuThoat_Click);

            this.mnuDanhMuc.Text = "Danh mục";
            this.mnuDanhMuc.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuSinhVien, this.mnuSanPham});
            this.mnuSinhVien.Text = "Quản lý Sinh viên";
            this.mnuSinhVien.Click += new System.EventHandler(this.mnuSinhVien_Click);
            this.mnuSanPham.Text = "Quản lý Sản phẩm";
            this.mnuSanPham.Click += new System.EventHandler(this.mnuSanPham_Click);

            this.mnuCuaSo.Text = "Cửa sổ";
            this.mnuCuaSo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.mnuCascade, this.mnuTileH, this.mnuTileV});
            this.mnuCascade.Text = "Cascade";
            this.mnuCascade.Click += new System.EventHandler(this.mnuCascade_Click);
            this.mnuTileH.Text = "Tile Horizontal";
            this.mnuTileH.Click += new System.EventHandler(this.mnuTileH_Click);
            this.mnuTileV.Text = "Tile Vertical";
            this.mnuTileV.Click += new System.EventHandler(this.mnuTileV_Click);

            // ===== ToolStrip =====
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.tsbSinhVien, this.tsbSanPham, this.tsSep1});
            this.toolStripMain.Location = new System.Drawing.Point(0, 24);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(950, 25);

            this.tsbSinhVien.Text = "Sinh viên";
            this.tsbSinhVien.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSinhVien.Click += new System.EventHandler(this.tsbSinhVien_Click);

            this.tsbSanPham.Text = "Sản phẩm";
            this.tsbSanPham.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSanPham.Click += new System.EventHandler(this.tsbSanPham_Click);

            // ===== Panel ProgressBar =====
            this.panelProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelProgress.Height = 28;
            this.panelProgress.Padding = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.panelProgress.Name = "panelProgress";

            this.pbLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbLoad.Minimum = 0;
            this.pbLoad.Maximum = 100;
            this.pbLoad.Value = 0;
            this.pbLoad.Style = System.Windows.Forms.ProgressBarStyle.Continuous;

            this.lblPercent.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblPercent.Width = 60;
            this.lblPercent.Text = "0%";
            this.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblPercent.Font = new System.Drawing.Font("Segoe UI", 9F,
                                    System.Drawing.FontStyle.Bold);
            this.lblPercent.ForeColor = System.Drawing.Color.DarkBlue;

            // Thứ tự add: Fill trước, Right sau → Right được xử lý trước
            this.panelProgress.Controls.Add(this.pbLoad);
            this.panelProgress.Controls.Add(this.lblPercent);

            // ===== StatusStrip =====
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.lblUser, this.lblSpring, this.lblMessage, this.lblTime});
            this.statusStripMain.Location = new System.Drawing.Point(0, 540);
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Size = new System.Drawing.Size(950, 22);

            this.lblUser.Text = "Người dùng: guest";
            this.lblSpring.Spring = true;
            this.lblMessage.Text = "Sẵn sàng";
            this.lblMessage.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.lblTime.Text = "00:00:00";
            this.lblTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;

            // ===== Form1 =====
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 562);
            // Thứ tự add: statusStrip → panelProgress (panel nằm TRÊN statusStrip)
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.panelProgress);
            this.Controls.Add(this.toolStripMain);
            this.Controls.Add(this.menuStripMain);
            this.IsMdiContainer = true;                       // ⭐ MDI Parent
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "Form1";
            this.Text = "BÀI 48 - MDI + ASYNC/AWAIT + PROGRESSBAR";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }
    }
}