using Bài47;
using System;
using System.Data;
using System.Windows.Forms;

namespace Bài47
{
    public partial class FormProduct : Form
    {
        private DataGridView dgv;
        private ContextMenuStrip ctx;
        private DataTable dt;

        public FormProduct()
        {
            InitializeComponent();
            this.Text = "Quản lý Sản phẩm";
            LoadData();
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(700, 420);

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            ctx = new ContextMenuStrip();
            var mnuXoa = new ToolStripMenuItem("Xóa dòng");
            var mnuRefresh = new ToolStripMenuItem("Làm mới dữ liệu");
            var mnuDetail = new ToolStripMenuItem("Xem chi tiết");

            mnuXoa.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null) return;
                var ten = dgv.CurrentRow.Cells["TenSP"].Value?.ToString();
                if (MessageBox.Show($"Xóa [{ten}]?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    dgv.Rows.Remove(dgv.CurrentRow);
                    NotifyParent($"Đã xóa sản phẩm: {ten}");
                }
            };
            mnuRefresh.Click += (s, e) =>
            {
                LoadData();
                NotifyParent("Đã làm mới danh sách sản phẩm");
            };
            mnuDetail.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null) return;
                MessageBox.Show(
                    $"{dgv.CurrentRow.Cells["MaSP"].Value} - {dgv.CurrentRow.Cells["TenSP"].Value}\n" +
                    $"Giá: {dgv.CurrentRow.Cells["Gia"].Value}",
                    "Chi tiết sản phẩm");
                NotifyParent("Xem chi tiết sản phẩm");
            };

            ctx.Items.AddRange(new ToolStripItem[] { mnuXoa, mnuRefresh, mnuDetail });
            dgv.ContextMenuStrip = ctx;

            this.Controls.Add(dgv);
        }

        private void LoadData()
        {
            dt = new DataTable();
            dt.Columns.Add("MaSP", typeof(string));
            dt.Columns.Add("TenSP", typeof(string));
            dt.Columns.Add("Gia", typeof(decimal));
            dt.Columns.Add("SoLuong", typeof(int));

            dt.Rows.Add("SP01", "Laptop Dell", 15000000m, 5);
            dt.Rows.Add("SP02", "Chuột Logitech", 350000m, 50);
            dt.Rows.Add("SP03", "Bàn phím cơ", 1200000m, 20);

            dgv.DataSource = dt;
        }

        private void NotifyParent(string message)
        {
            if (this.MdiParent is Form1 parent)
                parent.SetStatusMessage(message);
        }
    }
}