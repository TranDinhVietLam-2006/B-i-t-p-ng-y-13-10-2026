using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài48
{
    // Model
    public class SanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public decimal Gia { get; set; }
        public int SoLuong { get; set; }
    }

    public partial class FormProduct : Form
    {
        private Button btnLoad;
        private DataGridView dgv;
        private ContextMenuStrip ctx;

        public FormProduct()
        {
            InitializeComponent();
            this.Text = "Quản lý Sản phẩm";
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(750, 440);

            btnLoad = new Button
            {
                Text = "🔄 Nạp dữ liệu (Async)",
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = System.Drawing.Color.LightGreen,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            };
            btnLoad.Click += BtnLoad_Click;

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
            mnuRefresh.Click += async (s, e) => await ReloadAsync();
            mnuDetail.Click += (s, e) =>
            {
                if (dgv.CurrentRow == null) return;
                MessageBox.Show(
                    $"{dgv.CurrentRow.Cells["MaSP"].Value} - {dgv.CurrentRow.Cells["TenSP"].Value}\n" +
                    $"Giá: {dgv.CurrentRow.Cells["Gia"].Value:N0} VNĐ",
                    "Chi tiết sản phẩm");
                NotifyParent("Xem chi tiết sản phẩm");
            };

            ctx.Items.AddRange(new ToolStripItem[] { mnuXoa, mnuRefresh, mnuDetail });
            dgv.ContextMenuStrip = ctx;

            this.Controls.Add(dgv);
            this.Controls.Add(btnLoad);
        }

     
        private async void BtnLoad_Click(object sender, EventArgs e) => await ReloadAsync();

        private async Task ReloadAsync()
        {
            var parent = this.MdiParent as Form1;
            IProgress<int> progress = parent?.GetProgressReporter();

            // ⭐ Khóa nút (tránh bấm trùng)
            btnLoad.Enabled = false;
            btnLoad.Text = "Đang nạp dữ liệu...";
            try
            {
                var list = await LoadDataAsync(progress);

                dgv.DataSource = null;
                dgv.DataSource = list;

                parent?.SetStatusMessage($" Đã nạp xong {list.Count} sản phẩm");
                MessageBox.Show($"Nạp thành công {list.Count} sản phẩm!",
                    "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                parent?.SetStatusMessage(" Lỗi nạp dữ liệu sản phẩm");
            }
            finally
            {
                
                btnLoad.Enabled = true;
                btnLoad.Text = " Nạp dữ liệu (Async)";
                parent?.ResetProgress();
            }
        }

      
        private async Task<List<SanPham>> LoadDataAsync(IProgress<int> progress)
        {
            return await Task.Run(async () =>
            {
                var list = new List<SanPham>();
                var rnd = new Random();

                string[] tenSP = { "Laptop Dell", "Chuột Logitech", "Bàn phím cơ",
                                   "Màn hình LG", "Tai nghe Sony", "USB 32GB",
                                   "Ổ cứng SSD", "Webcam HD" };

                const int TOTAL = 100;
                for (int i = 1; i <= TOTAL; i++)
                {
                    await Task.Delay(50);   

                    list.Add(new SanPham
                    {
                        MaSP = $"SP{i:D3}",
                        TenSP = tenSP[rnd.Next(tenSP.Length)] + " #" + i,
                        Gia = (rnd.Next(20, 500)) * 100000m,
                        SoLuong = rnd.Next(1, 100)
                    });

                    progress?.Report(i * 100 / TOTAL);
                }

                return list;
            });
        }

        private void NotifyParent(string msg) => (this.MdiParent as Form1)?.SetStatusMessage(msg);
    }
}