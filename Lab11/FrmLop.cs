using System.Data;
using Microsoft.Data.SqlClient;

namespace COMP1019_Lab11_AdoNet
{
    /// <summary>Form quản lý lớp học: CRUD trên bảng LOP của SQL Server.</summary>
    public class FrmLop : Form
    {
        // ----- Control theo đúng tên trong đề -----
        private Label lblMaLop, lblTenLop, lblTimKiem;
        private TextBox txtMaLop, txtTenLop, txtTimKiem;
        private DataGridView dgvLop;
        private Button btnThem, btnSua, btnXoa, btnLamMoi, btnTimKiem;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblTrangThai;

        private readonly LopRepository _repo = new LopRepository();
        private bool _isLoading;           // true khi đang nạp lưới, để bỏ qua SelectionChanged
        private string _selectedMaLop;     // mã lớp đang được chọn trong lưới (null nếu chưa chọn)

        public FrmLop()
        {
            Text = "Quản lý lớp học - EduManagerDB";
            ClientSize = new Size(820, 560);
            MinimumSize = new Size(760, 440);
            StartPosition = FormStartPosition.CenterScreen;

            BuildUi();
            WireEvents();
        }

        // ================= GIAO DIỆN =================
        private void BuildUi()
        {
            var pnlTop = new Panel { Name = "pnlTop", Dock = DockStyle.Top, Height = 160 };

            lblMaLop = new Label { Name = "lblMaLop", Text = "Mã lớp:", Left = 20, Top = 23, AutoSize = true };
            txtMaLop = new TextBox { Name = "txtMaLop", Left = 90, Top = 20, Width = 200, MaxLength = LopValidator.MaLopMaxLength };
            lblTenLop = new Label { Name = "lblTenLop", Text = "Tên lớp:", Left = 320, Top = 23, AutoSize = true };
            txtTenLop = new TextBox { Name = "txtTenLop", Left = 390, Top = 20, Width = 320, MaxLength = LopValidator.TenLopMaxLength };

            btnThem = MakeButton("btnThem", "Thêm", 20, 65);
            btnSua = MakeButton("btnSua", "Sửa", 110, 65);
            btnXoa = MakeButton("btnXoa", "Xóa", 200, 65);
            btnLamMoi = MakeButton("btnLamMoi", "Làm mới", 290, 65);

            lblTimKiem = new Label { Name = "lblTimKiem", Text = "Tìm kiếm:", Left = 20, Top = 119, AutoSize = true };
            txtTimKiem = new TextBox { Name = "txtTimKiem", Left = 90, Top = 116, Width = 300 };
            btnTimKiem = MakeButton("btnTimKiem", "Tìm kiếm", 400, 112);

            pnlTop.Controls.AddRange(new Control[]
            {
                lblMaLop, txtMaLop, lblTenLop, txtTenLop,
                btnThem, btnSua, btnXoa, btnLamMoi,
                lblTimKiem, txtTimKiem, btnTimKiem
            });

            dgvLop = new DataGridView
            {
                Name = "dgvLop",
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window
            };

            statusStrip = new StatusStrip { Name = "statusStrip1" };
            lblTrangThai = new ToolStripStatusLabel { Name = "lblTrangThai", Text = "Đang kết nối..." };
            statusStrip.Items.Add(lblTrangThai);

            // Fill trước, rồi Top/Bottom để không bị che
            Controls.Add(dgvLop);
            Controls.Add(pnlTop);
            Controls.Add(statusStrip);
        }

        private static Button MakeButton(string name, string text, int x, int y) =>
            new Button { Name = name, Text = text, Left = x, Top = y, Width = 80, Height = 32 };

        private void WireEvents()
        {
            Load += FrmLop_Load;                                // F1
            dgvLop.SelectionChanged += DgvLop_SelectionChanged; // F2
            btnThem.Click += BtnThem_Click;                     // F3
            btnSua.Click += BtnSua_Click;                       // F4
            btnXoa.Click += BtnXoa_Click;                       // F5
            btnTimKiem.Click += BtnTimKiem_Click;               // F6
            btnLamMoi.Click += BtnLamMoi_Click;                 // F7

            txtTimKiem.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    btnTimKiem.PerformClick();
                    e.SuppressKeyPress = true;
                }
            };
        }

        // ================= NẠP DỮ LIỆU (F1) =================
        private void FrmLop_Load(object sender, EventArgs e)
        {
            if (!EnsureSchemaCompatible()) return;
            LoadData();
        }

        /// <summary>
        /// Bảng LOP của buổi 10 có thể có thêm cột bắt buộc (ví dụ MaKhoa) khiến việc thêm lớp bị lỗi.
        /// Nếu phát hiện, hỏi người dùng có cho phép các cột đó nhận NULL hay không.
        /// Trả về false nếu không kết nối được database.
        /// </summary>
        private bool EnsureSchemaCompatible()
        {
            try
            {
                List<SchemaFix> fixes = _repo.FindExtraRequiredColumns();
                if (fixes.Count == 0) return true;

                string names = string.Join(", ", fixes.Select(f => f.ColumnName));
                var answer = MessageBox.Show(
                    $"Bảng LOP trong database của bạn có thêm cột bắt buộc nhập: {names}.\n" +
                    "Bài Lab 11 chỉ nhập Mã lớp và Tên lớp nên sẽ không thêm được lớp mới.\n\n" +
                    "Cho phép các cột này nhận giá trị trống (NULL)?\n" +
                    "Dữ liệu hiện có không bị mất hay thay đổi.",
                    "Điều chỉnh cấu trúc bảng LOP", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (answer == DialogResult.Yes)
                {
                    _repo.AllowNulls(fixes);
                    Info($"Đã cho phép NULL cho cột: {names}.");
                }
                return true;
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                lblTrangThai.Text = "Không kết nối được cơ sở dữ liệu";
                ShowDbError(ex, "kiểm tra cấu trúc bảng LOP");
                return false;
            }
        }

        /// <summary>Nạp danh sách lớp lên lưới. keyword rỗng thì lấy tất cả, ngược lại tìm theo từ khóa.</summary>
        private void LoadData(string keyword = null)
        {
            try
            {
                DataTable table = string.IsNullOrEmpty(keyword) ? _repo.GetAll() : _repo.Search(keyword);
                BindGrid(table);
                UpdateStatus(table.Rows.Count, keyword);
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                lblTrangThai.Text = "Không kết nối được cơ sở dữ liệu";
                ShowDbError(ex, "tải danh sách lớp");
            }
        }

        private void BindGrid(DataTable table)
        {
            _isLoading = true;
            try
            {
                dgvLop.DataSource = table;
                dgvLop.Columns["MaLop"].HeaderText = "Mã lớp";
                dgvLop.Columns["TenLop"].HeaderText = "Tên lớp";
                dgvLop.Columns["SoSinhVien"].HeaderText = "Số sinh viên";
                dgvLop.ClearSelection();
                ClearInput();
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void UpdateStatus(int count, string keyword)
        {
            var b = new SqlConnectionStringBuilder(DbConfig.ConnectionString);
            string mode = string.IsNullOrEmpty(keyword) ? "Số lớp" : $"Kết quả tìm '{keyword}'";
            lblTrangThai.Text = $"Server: {b.DataSource} | Database: {b.InitialCatalog} | {mode}: {count}";
        }

        private void ClearInput()
        {
            txtMaLop.Clear();
            txtTenLop.Clear();
            txtMaLop.ReadOnly = false;
            _selectedMaLop = null;
        }

        // ================= F2: CHỌN DÒNG =================
        private void DgvLop_SelectionChanged(object sender, EventArgs e)
        {
            if (_isLoading || dgvLop.SelectedRows.Count == 0) return;

            DataGridViewRow row = dgvLop.SelectedRows[0];
            _selectedMaLop = Convert.ToString(row.Cells["MaLop"].Value);
            txtMaLop.Text = _selectedMaLop;
            txtTenLop.Text = Convert.ToString(row.Cells["TenLop"].Value);
            txtMaLop.ReadOnly = true; // không cho đổi mã khi đang sửa
        }

        // ================= F3: THÊM =================
        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (_selectedMaLop != null)
            {
                Warn("Đang chọn một lớp. Hãy bấm 'Làm mới' rồi nhập lớp mới.");
                return;
            }

            string maLop = txtMaLop.Text.Trim();
            string tenLop = txtTenLop.Text.Trim();

            string error = LopValidator.Validate(maLop, tenLop);
            if (error != null)
            {
                Warn(error);
                return;
            }

            try
            {
                if (_repo.Exists(maLop))
                {
                    Warn($"Mã lớp '{maLop}' đã tồn tại!");
                    txtMaLop.Focus();
                    return;
                }

                _repo.Insert(new Lop { MaLop = maLop, TenLop = tenLop });
                Info("Thêm lớp thành công!");
                ReloadAll();
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                // Phòng trường hợp trùng khóa chính xảy ra giữa lúc kiểm tra và lúc thêm
                Warn($"Mã lớp '{maLop}' đã tồn tại!");
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                ShowDbError(ex, "thêm lớp");
            }
        }

        // ================= F4: SỬA =================
        private void BtnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMaLop == null)
            {
                Warn("Vui lòng chọn lớp cần sửa trong danh sách!");
                return;
            }

            string tenLop = txtTenLop.Text.Trim();
            string error = LopValidator.ValidateTenLop(tenLop);
            if (error != null)
            {
                Warn(error);
                return;
            }

            try
            {
                int rows = _repo.Update(new Lop { MaLop = _selectedMaLop, TenLop = tenLop });
                if (rows == 0)
                {
                    Warn("Lớp này không còn tồn tại (có thể đã bị xóa). Danh sách sẽ được tải lại.");
                }
                else
                {
                    Info("Cập nhật lớp thành công!");
                }
                ReloadAll();
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                ShowDbError(ex, "cập nhật lớp");
            }
        }

        // ================= F5: XÓA =================
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMaLop == null)
            {
                Warn("Vui lòng chọn lớp cần xóa trong danh sách!");
                return;
            }

            try
            {
                int soSinhVien = _repo.CountStudents(_selectedMaLop);
                if (soSinhVien > 0)
                {
                    Warn($"Không thể xóa lớp '{_selectedMaLop}' vì đang có {soSinhVien} sinh viên.");
                    return;
                }

                var confirm = MessageBox.Show($"Bạn có chắc muốn xóa lớp '{_selectedMaLop}'?", "Xác nhận xóa",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;

                _repo.Delete(_selectedMaLop);
                Info("Xóa lớp thành công!");
                ReloadAll();
            }
            catch (SqlException ex) when (ex.Number == 547)
            {
                // Lỗi khóa ngoại: lớp vừa được gán sinh viên giữa lúc kiểm tra và lúc xóa
                Warn("Không thể xóa lớp vì đang có sinh viên thuộc lớp này (ràng buộc khóa ngoại).");
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                ShowDbError(ex, "xóa lớp");
            }
        }

        // ================= F6: TÌM KIẾM =================
        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txtTimKiem.Text.Trim();
            LoadData(keyword);

            if (keyword != "" && dgvLop.Rows.Count == 0)
                Info("Không tìm thấy lớp phù hợp.");
        }

        // ================= F7: LÀM MỚI =================
        private void BtnLamMoi_Click(object sender, EventArgs e)
        {
            ReloadAll();
            txtMaLop.Focus();
        }

        /// <summary>Xóa ô tìm kiếm, xóa nội dung đang nhập (mở khóa mã lớp) và nạp lại toàn bộ danh sách.</summary>
        private void ReloadAll()
        {
            txtTimKiem.Clear();
            LoadData();
        }

        // ================= HÀM HỖ TRỢ =================
        private static bool IsDbException(Exception ex) =>
            ex is SqlException || ex is InvalidOperationException;

        private static void ShowDbError(Exception ex, string action)
        {
            string hint = "";
            if (ex is SqlException sql)
            {
                if (sql.Number == 4060)
                    hint = $"\n\nKhông tìm thấy database {DbConfig.DatabaseName}. Hãy chạy file Database/EduManagerDB.sql trong SSMS.";
                else if (sql.Number == 515)
                    hint = "\n\nBảng LOP có cột bắt buộc nhập ngoài Mã lớp và Tên lớp. Hãy chạy file Database/EduManagerDB.sql trong SSMS " +
                           "hoặc mở lại chương trình và chọn Yes khi được hỏi điều chỉnh bảng.";
            }

            MessageBox.Show($"Lỗi cơ sở dữ liệu khi {action}:\n{ex.Message}{hint}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void Warn(string message) =>
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static void Info(string message) =>
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
