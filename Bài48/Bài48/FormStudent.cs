using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài48
{
    // Model
    public class SinhVien
    {
        public string MaSV { get; set; }
        public string HoTen { get; set; }
        public string Lop { get; set; }
        public double DiemTB { get; set; }
    }

    public partial class FormStudent : Form
    {
        private Button btnLoad;
        private DataGridView dgv;
        private ContextMenuStrip ctx;

        public FormStudent()
        {
            InitializeComponent();
            this.Text = "Quản lý Sinh viên";
        }

        private void InitializeComponent()
        {
            this.ClientSize = new System.Drawing.Size(750, 440);

            // Nút Nạp dữ liệu (Async)
            btnLoad = new Button
            {
                Text = "🔄 Nạp dữ liệu (Async)",
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = System.Drawing.Color.LightSkyBlue,
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold)
            };
            btnLoad.Click += BtnLoad_Click;   // async void handler

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            // ContextMenuStrip trên DataGridView
            ctx = new ContextMenuStrip();
            var mnuXoa = new ToolStripMenuItem("Xóa dòng");
            var mnuRefresh = new ToolStripMenuItem("Làm mới dữ liệu");
            var mnuDetail = new ToolStripMenuItem("Xem chi tiết");

            mnuXoa.Click += MnuXoa_Click;
            mnuRefresh.Click += async (s, e) => await ReloadAsync();
            mnuDetail.Click += MnuDetail_Click;

            ctx.Items.AddRange(new ToolStripItem[] { mnuXoa, mnuRefresh, mnuDetail });
            dgv.ContextMenuStrip = ctx;

            // Thứ tự add: Fill trước, Top sau (Top được xử lý trước)
            this.Controls.Add(dgv);
            this.Controls.Add(btnLoad);
        }

        // =====================================================
        // ⭐ NÚT NẠP DỮ LIỆU – ASYNC HANDLER
        // =====================================================
        private async void BtnLoad_Click(object sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private async Task ReloadAsync()
        {
            var parent = this.MdiParent as Form1;
            IProgress<int> progress = parent?.GetProgressReporter();

            // ⭐ Khóa nút để tránh bấm trùng lặp
            btnLoad.Enabled = false;
            btnLoad.Text = "Đang nạp dữ liệu...";
            try
            {
                List<SinhVien> list = await LoadDataAsync(progress);

                // Bind vào grid (UI thread – không cần Invoke vì await đã quay về UI)
                dgv.DataSource = null;
                dgv.DataSource = list;

                parent?.SetStatusMessage($"✅ Đã nạp xong {list.Count} sinh viên");
                MessageBox.Show($"Nạp thành công {list.Count} sinh viên!",
                    "Hoàn tất", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi nạp dữ liệu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                parent?.SetStatusMessage("❌ Lỗi nạp dữ liệu sinh viên");
            }
            finally
            {
                // ⭐ Mở lại nút + reset ProgressBar
                btnLoad.Enabled = true;
                btnLoad.Text = "🔄 Nạp dữ liệu (Async)";
                parent?.ResetProgress();
            }
        }

        // =====================================================
        // ⭐ PHƯƠNG THỨC BẤT ĐỒNG BỘ: async Task<List<SinhVien>>
        // Dùng Task.Run + Task.Delay để giả lập nạp dữ liệu nặng
        // =====================================================
        private async Task<List<SinhVien>> LoadDataAsync(IProgress<int> progress)
        {
            return await Task.Run(async () =>
            {
                var list = new List<SinhVien>();
                var rnd = new Random();

                string[] ho = { "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Vũ", "Đặng" };
                string[] ten = { "An", "Bình", "Cường", "Dung", "Em", "Phong", "Giang" };
                string[] lop = { "CNTT1", "CNTT2", "CNTT3", "KT1", "KT2" };

                const int TOTAL = 100;   // 100 bước → 0% → 100%
                for (int i = 1; i <= TOTAL; i++)
                {
                    // ⭐ Giả lập xử lý nặng từng bước (50ms)
                    await Task.Delay(50);

                    list.Add(new SinhVien
                    {
                        MaSV = $"SV{i:D3}",
                        HoTen = ho[rnd.Next(ho.Length)] + " " + ten[rnd.Next(ten.Length)],
                        Lop = lop[rnd.Next(lop.Length)],
                        DiemTB = Math.Round(4 + rnd.NextDouble() * 6, 2)
                    });

                    // ⭐ Báo cáo tiến trình (marshal về UI Thread nhờ Progress<T>)
                    progress?.Report(i * 100 / TOTAL);
                }

                return list;
            });
        }

       
        private void NotifyParent(string msg)
        {
            (this.MdiParent as Form1)?.SetStatusMessage(msg);
        }

        private void MnuXoa_Click(object sender, EventArgs e)
        {
            if (dgv.CurrentRow == null) return;
            var ma = dgv.CurrentRow.Cells["MaSV"].Value?.ToString();
            if (MessageBox.Show($"Xóa sinh viên [{ma}]?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            dgv.Rows.Remove(dgv.CurrentRow);
            NotifyParent($"Đã xóa sinh viên: {ma}");
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
            MessageBox.Show(info, "Chi tiết Sinh viên");
            NotifyParent("Xem chi tiết sinh viên");
        }
    }
}