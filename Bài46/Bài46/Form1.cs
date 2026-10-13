
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Bài46
{
    public partial class Form1 : Form
    {
        private readonly BindingList<Student> students =
            new BindingList<Student>();

        private readonly BindingSource bs = new BindingSource();

        public Form1()
        {
            InitializeComponent();
            SetupDataBinding();
            AddSampleData();
        }

        // Cau hinh BindingSource va lien ket du lieu
        private void SetupDataBinding()
        {
            cboGender.Items.Clear();
            cboGender.Items.Add("Nam");
            cboGender.Items.Add("Nu");
            cboGender.DropDownStyle =
                ComboBoxStyle.DropDownList;

            dgvStudents.AutoGenerateColumns = true;
            dgvStudents.ReadOnly = true;
            dgvStudents.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvStudents.MultiSelect = false;
            dgvStudents.AllowUserToAddRows = false;
            dgvStudents.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudents.RowHeadersVisible = false;

            bs.DataSource = students;
            dgvStudents.DataSource = bs;

            txtStudentId.DataBindings.Add(
                "Text", bs, nameof(Student.StudentId),
                true, DataSourceUpdateMode.OnPropertyChanged);

            txtFullName.DataBindings.Add(
                "Text", bs, nameof(Student.FullName),
                true, DataSourceUpdateMode.OnPropertyChanged);

            txtAge.DataBindings.Add(
                "Text", bs, nameof(Student.Age),
                true, DataSourceUpdateMode.OnPropertyChanged);

            cboGender.DataBindings.Add(
                "SelectedItem", bs, nameof(Student.Gender),
                true, DataSourceUpdateMode.OnPropertyChanged);

            bs.PositionChanged += Bs_PositionChanged;
            bs.CurrentChanged += Bs_CurrentChanged;
        }

        // Them du lieu mau
        private void AddSampleData()
        {
            students.Add(new Student
            {
                StudentId = "SV001",
                FullName = "Nguyen Van An",
                Age = "20",
                Gender = "Nam"
            });

            students.Add(new Student
            {
                StudentId = "SV002",
                FullName = "Tran Thi Binh",
                Age = "21",
                Gender = "Nu"
            });

            if (bs.Count > 0)
                bs.Position = 0;

            LoadAvatar();
        }

        // Tu dong hien thi avatar khi chuyen sinh vien
        private void Bs_PositionChanged(object sender, EventArgs e)
        {
            LoadAvatar();
        }

        private void Bs_CurrentChanged(object sender, EventArgs e)
        {
            LoadAvatar();
        }

        // Tai anh dai dien
        private void LoadAvatar()
        {
            Image oldImage = picAvatar.Image;
            picAvatar.Image = null;

            if (oldImage != null)
                oldImage.Dispose();

            Student sv = bs.Current as Student;

            if (sv == null ||
                string.IsNullOrWhiteSpace(sv.AvatarPath))
                return;

            if (!File.Exists(sv.AvatarPath))
                return;

            try
            {
                using (Image temp = Image.FromFile(sv.AvatarPath))
                {
                    picAvatar.Image = new Bitmap(temp);
                }
            }
            catch (Exception)
            {
                picAvatar.Image = null;
            }
        }

        // Them sinh vien
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Student sv = new Student();

            students.Add(sv);
            bs.Position = bs.Count - 1;

            txtStudentId.Focus();
        }

        // Xoa sinh vien dang chon
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (bs.Current == null)
            {
                MessageBox.Show(
                    "Vui long chon sinh vien can xoa!");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Ban co chac muon xoa sinh vien dang chon?",
                "Xac nhan xoa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                int index = bs.Position;

                students.RemoveAt(index);

                if (students.Count > 0)
                    bs.Position = Math.Min(index, students.Count - 1);

                LoadAvatar();
            }
        }

        // Cap nhat sinh vien
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (bs.Current == null)
            {
                MessageBox.Show(
                    "Vui long chon hoac them sinh vien!");
                return;
            }

            Validate();
            bs.EndEdit();

            Student sv = bs.Current as Student;

            if (sv == null)
                return;

            if (string.IsNullOrWhiteSpace(sv.StudentId) ||
                string.IsNullOrWhiteSpace(sv.FullName))
            {
                MessageBox.Show(
                    "Ma sinh vien va ho ten khong duoc de trong!");
                return;
            }

            int ageValue;

            if (!string.IsNullOrWhiteSpace(sv.Age) &&
                (!int.TryParse(sv.Age, out ageValue) ||
                 ageValue < 0))
            {
                MessageBox.Show(
                    "Tuoi phai la so nguyen khong am!");
                txtAge.Focus();
                return;
            }

            dgvStudents.Refresh();

            MessageBox.Show(
                "Cap nhat thong tin thanh cong!");
        }

        // Chon anh avatar
        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            if (bs.Current == null)
            {
                MessageBox.Show(
                    "Vui long them sinh vien truoc khi chon anh!");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Chon anh dai dien";

                ofd.Filter =
                    "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All files (*.*)|*.*";

                ofd.Multiselect = false;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Student sv = bs.Current as Student;

                    if (sv != null)
                    {
                        sv.AvatarPath = ofd.FileName;
                        LoadAvatar();
                    }
                }
            }
        }

        // Xuat danh sach sinh vien ra file CSV
        private void btnExportCsv_Click(object sender, EventArgs e)
        {
            Validate();
            bs.EndEdit();

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Title = "Xuat danh sach sinh vien ra CSV";
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.DefaultExt = "csv";
                sfd.AddExtension = true;
                sfd.FileName = "DanhSachSinhVien.csv";
                sfd.OverwritePrompt = true;

                if (sfd.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (StreamWriter writer = new StreamWriter(
                        sfd.FileName, false, new UTF8Encoding(true)))
                    {
                        writer.WriteLine(
                            "StudentId,FullName,Age,Gender,AvatarPath");

                        foreach (Student sv in students)
                        {
                            writer.WriteLine(
                                CsvField(sv.StudentId) + "," +
                                CsvField(sv.FullName) + "," +
                                CsvField(sv.Age) + "," +
                                CsvField(sv.Gender) + "," +
                                CsvField(sv.AvatarPath));
                        }
                    }

                    MessageBox.Show(
                        "Xuat file CSV thanh cong!",
                        "Thong bao",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Khong the xuat file CSV: " + ex.Message);
                }
            }
        }

        // Xu ly dau phay va dau ngoac kep trong file CSV
        private string CsvField(string value)
        {
            if (value == null)
                value = "";

            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        // Giai phong anh khi dong form
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (picAvatar.Image != null)
            {
                picAvatar.Image.Dispose();
                picAvatar.Image = null;
            }

            base.OnFormClosed(e);
        }
    }
}
