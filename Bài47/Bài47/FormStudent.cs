using Bài47;
using System;
using System.Data;
using System.Windows.Forms;

namespace Bài47
{
    public partial class FormStudent : Form
    {
        private DataGridView dgv;
        private ContextMenuStrip ctx;
        private DataTable dt;

        public FormStudent()
        {
            InitializeComponent();
            this.Text = "Quản lý Sinh viên";
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
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            // Menu ngữ cảnh trên DataGridView
            ctx = new ContextMenuStrip();
            var mnuXoa = new ToolStripMenuItem("Xóa dòng");
            var mnuRefresh = new ToolStripMenuItem("Làm mới dữ liệu");
            var mnuDetail = new ToolStripMenuItem("Xem chi tiết");

            mnuXoa.Click += MnuXoa_Click;
            mnuRefresh.Click += MnuRefresh_Click;
            mnuDetail.Click += MnuDetail_Click;

            ctx.Items.AddRange(new ToolStripItem[] { mnuXoa, mnuRefresh, mnuDetail });
            dgv.ContextMenuStrip = ctx;

            this.Controls.Add(dgv);
        }

        private void LoadData()
        {
            dt = new DataTable();
            dt.Columns.Add("MaSV", typeof(string));
            dt.Columns.Add("HoTen", typeof(string));
            dt.Columns.Add("Lop", typeof(string));
            dt.Columns.Add("DiemTB", typeof(double));

            dt.Rows.Add("SV01", "Nguyễn Văn A", "CNTT1", 8.5);
            dt.Rows.Add("SV02", "Trần Thị B", "CNTT1", 7.8);
            dt.Rows.Add("SV03", "Lê Văn C", "CNTT2", 9.1);
            dt.Rows.Add("SV04", "Phạm Thị D", "CNTT2", 6.9);

            dgv.DataSource = dt;
        }

        // =====================================================
        // ⭐ GỌI NGƯỢC LÊN Form1 (PARENT) CẬP NHẬT STATUSSTRIP
        // =====================================================
        private void NotifyParent(string message)
        {
            if (this.MdiParent is Form1 parent)
                parent.SetStatusMessage(message);
        }

        private void MnuXoa_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;

            var ma = dgv.CurrentRow.Cells["MaSV"].Value?.ToString();
            var rs = MessageBox.Show($"Xóa sinh viên [{ma}]?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (rs != DialogResult.Yes) return;

            dgv.Rows.Remove(dgv.CurrentRow);
            NotifyParent($"Đã xóa sinh viên: {ma}");
        }

        private void MnuRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            NotifyParent("Đã làm mới danh sách sinh viên");
        }

        private void MnuDetail_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var info = string.Join(" | ", new[]
            {
                dgv.CurrentRow.Cells["MaSV"].Value,
                dgv.CurrentRow.Cells["HoTen"].Value,
                dgv.CurrentRow.Cells["Lop"].Value,
                dgv.CurrentRow.Cells["DiemTB"].Value
            });
            MessageBox.Show(info, "Chi tiết Sinh viên",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            NotifyParent("Xem chi tiết sinh viên");
        }
    }
}