using System.Data;
using Microsoft.Data.SqlClient;

namespace QLSinhVienLab12
{
    /// <summary>Form nhập điểm theo lớp: chọn lớp, môn học, nhập điểm cho cả lớp và lưu bằng transaction.</summary>
    public class FrmNhapDiemTheoLop : Form
    {
        // ----- Control theo đúng tên trong đề -----
        private ComboBox cboLop, cboMonHoc;
        private Button btnTaiDanhSach, btnLuuTatCa;
        private DataGridView dgvKetQua;
        private TextBox txtTimKiem;
        private Label lblThongKe;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblTrangThai;

        // ----- Dữ liệu và binding -----
        private readonly KetQuaDAL _dal = new KetQuaDAL();
        private readonly BindingSource _bsLop = new BindingSource();
        private readonly BindingSource _bsMonHoc = new BindingSource();
        private readonly BindingSource _bsKetQua = new BindingSource();
        private DataTable _tableKetQua;

        // Lớp và môn đã tải lên lưới (dùng khi lưu, không phụ thuộc việc người dùng đổi ComboBox sau đó)
        private string _loadedMaLop, _loadedMaMon, _loadedTenLop, _loadedTenMon;

        public FrmNhapDiemTheoLop()
        {
            Text = "Quản lý kết quả học tập - Nhập điểm theo lớp";
            ClientSize = new Size(980, 620);
            MinimumSize = new Size(860, 480);
            StartPosition = FormStartPosition.CenterScreen;

            BuildUi();
            WireEvents();
        }

        // ================= GIAO DIỆN =================
        private void BuildUi()
        {
            var pnlTop = new Panel { Name = "pnlTop", Dock = DockStyle.Top, Height = 135 };

            cboLop = new ComboBox { Name = "cboLop", Left = 85, Top = 17, Width = 280, DropDownStyle = ComboBoxStyle.DropDownList };
            cboMonHoc = new ComboBox { Name = "cboMonHoc", Left = 480, Top = 17, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };
            btnTaiDanhSach = new Button { Name = "btnTaiDanhSach", Text = "Tải danh sách", Left = 820, Top = 14, Width = 130, Height = 30 };

            txtTimKiem = new TextBox { Name = "txtTimKiem", Left = 85, Top = 62, Width = 280 };
            btnLuuTatCa = new Button
            {
                Name = "btnLuuTatCa", Text = "Lưu tất cả", Left = 480, Top = 58, Width = 150, Height = 32,
                Enabled = false, Font = new Font(Font, FontStyle.Bold)
            };
            lblThongKe = new Label
            {
                Name = "lblThongKe", Left = 20, Top = 105, Width = 930, Height = 22,
                ForeColor = Color.FromArgb(31, 78, 121), Text = "Thống kê: (chưa tải danh sách)"
            };

            pnlTop.Controls.AddRange(new Control[]
            {
                Lbl("Lớp:", 20, 20), cboLop,
                Lbl("Môn học:", 410, 20), cboMonHoc, btnTaiDanhSach,
                Lbl("Lọc SV:", 20, 65), txtTimKiem, btnLuuTatCa,
                Lbl("(lọc theo mã hoặc họ tên)", 640, 65), lblThongKe
            });

            dgvKetQua = new DataGridView
            {
                Name = "dgvKetQua",
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = SystemColors.Window,
                EditMode = DataGridViewEditMode.EditOnEnter
            };

            statusStrip = new StatusStrip { Name = "statusStrip1" };
            lblTrangThai = new ToolStripStatusLabel { Name = "lblTrangThai", Text = "Sẵn sàng." };
            statusStrip.Items.Add(lblTrangThai);

            // Fill trước, rồi Top/Bottom để không bị che
            Controls.Add(dgvKetQua);
            Controls.Add(pnlTop);
            Controls.Add(statusStrip);
        }

        private static Label Lbl(string text, int x, int y) =>
            new Label { Text = text, Left = x, Top = y + 3, AutoSize = true };

        private void WireEvents()
        {
            Load += (s, e) => LoadComboBoxes();
            btnTaiDanhSach.Click += (s, e) => LoadStudents(false);
            btnLuuTatCa.Click += (s, e) => LuuTatCa();
            txtTimKiem.TextChanged += (s, e) => ApplyFilter();

            dgvKetQua.CellValidating += DgvKetQua_CellValidating;
            dgvKetQua.CellParsing += DgvKetQua_CellParsing;
            dgvKetQua.CellEndEdit += (s, e) =>
            {
                if (e.RowIndex >= 0) dgvKetQua.Rows[e.RowIndex].ErrorText = "";
            };
            dgvKetQua.DataError += (s, e) =>
            {
                e.ThrowException = false; // không hiện hộp thoại lỗi mặc định
                SetStatus("Dữ liệu nhập không hợp lệ.");
            };

            FormClosing += (s, e) =>
            {
                if (!ConfirmDiscardChanges()) e.Cancel = true;
            };
        }

        // ================= 1. LOAD COMBOBOX =================
        /// <summary>Binding danh sách lớp và môn học vào ComboBox qua BindingSource.</summary>
        private void LoadComboBoxes()
        {
            try
            {
                DataTable lop = _dal.GetLopList();
                lop.Columns.Add("HienThi", typeof(string), "MaLop + ' - ' + TenLop");
                cboLop.DisplayMember = "HienThi";
                cboLop.ValueMember = "MaLop";
                _bsLop.DataSource = lop;
                cboLop.DataSource = _bsLop;

                DataTable mon = _dal.GetMonHocList();
                mon.Columns.Add("HienThi", typeof(string),
                    "MaMon + ' - ' + TenMon + ' (' + Convert(SoTinChi, 'System.String') + ' TC)'");
                cboMonHoc.DisplayMember = "HienThi";
                cboMonHoc.ValueMember = "MaMon";
                _bsMonHoc.DataSource = mon;
                cboMonHoc.DataSource = _bsMonHoc;

                SetStatus($"Đã tải {lop.Rows.Count} lớp và {mon.Rows.Count} môn học. Chọn lớp, môn học rồi bấm 'Tải danh sách'.");
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                SetStatus("Không kết nối được cơ sở dữ liệu.");
                ShowDbError(ex, "tải danh sách lớp và môn học");
            }
        }

        // ================= 2. TẢI DANH SÁCH SINH VIÊN =================
        /// <param name="force">true: bỏ qua việc hỏi khi có thay đổi chưa lưu (dùng sau khi lưu xong).</param>
        private void LoadStudents(bool force)
        {
            if (cboLop.SelectedValue == null || cboMonHoc.SelectedValue == null)
            {
                Warn("Vui lòng chọn lớp và môn học!");
                return;
            }
            if (!force && !ConfirmDiscardChanges()) return;

            string maLop = Convert.ToString(cboLop.SelectedValue);
            string maMon = Convert.ToString(cboMonHoc.SelectedValue);

            try
            {
                DataTable table = _dal.GetByLopMon(maLop, maMon);

                _loadedMaLop = maLop;
                _loadedMaMon = maMon;
                _loadedTenLop = ((DataRowView)cboLop.SelectedItem)["TenLop"].ToString();
                _loadedTenMon = ((DataRowView)cboMonHoc.SelectedItem)["TenMon"].ToString();

                txtTimKiem.Clear();
                _tableKetQua = table;
                _bsKetQua.DataSource = table;
                dgvKetQua.DataSource = _bsKetQua;
                ConfigureGrid();

                btnLuuTatCa.Enabled = table.Rows.Count > 0;
                RefreshThongKe();

                SetStatus(table.Rows.Count == 0
                    ? $"Lớp {_loadedTenLop} chưa có sinh viên."
                    : $"Đang nhập điểm môn '{_loadedTenMon}' cho lớp '{_loadedTenLop}': {table.Rows.Count} sinh viên.");
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                SetStatus("Không tải được danh sách sinh viên.");
                ShowDbError(ex, "tải danh sách sinh viên");
            }
        }

        private void ConfigureGrid()
        {
            dgvKetQua.Columns["MaSV"].HeaderText = "Mã SV";
            dgvKetQua.Columns["MaSV"].ReadOnly = true;
            dgvKetQua.Columns["MaSV"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvKetQua.Columns["MaSV"].FillWeight = 15;

            dgvKetQua.Columns["HoTen"].HeaderText = "Họ tên";
            dgvKetQua.Columns["HoTen"].ReadOnly = true;
            dgvKetQua.Columns["HoTen"].DefaultCellStyle.BackColor = Color.WhiteSmoke;
            dgvKetQua.Columns["HoTen"].FillWeight = 35;

            // Hai cột được nhập/sửa được tô nền vàng nhạt
            DataGridViewColumn diem = dgvKetQua.Columns["Diem"];
            diem.HeaderText = "Điểm";
            diem.FillWeight = 12;
            diem.DefaultCellStyle.Format = "0.##";
            diem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            diem.DefaultCellStyle.BackColor = Color.LemonChiffon;
            ((DataGridViewTextBoxColumn)diem).MaxInputLength = 6;

            DataGridViewColumn ghiChu = dgvKetQua.Columns["GhiChu"];
            ghiChu.HeaderText = "Ghi chú";
            ghiChu.FillWeight = 38;
            ghiChu.DefaultCellStyle.BackColor = Color.LemonChiffon;
            ((DataGridViewTextBoxColumn)ghiChu).MaxInputLength = KetQuaValidator.GhiChuMaxLength;
        }

        // ================= 3. NHẬP ĐIỂM: KIỂM TRA NGAY KHI NHẬP =================
        private void DgvKetQua_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvKetQua.Columns[e.ColumnIndex].Name != "Diem") return;

            string error = KetQuaValidator.ValidateDiemText(Convert.ToString(e.FormattedValue));
            DataGridViewRow row = dgvKetQua.Rows[e.RowIndex];
            if (error != null)
            {
                row.ErrorText = error + " (Nhấn Esc để hủy thay đổi.)";
                e.Cancel = true;
            }
            else
            {
                row.ErrorText = "";
            }
        }

        /// <summary>Tự đọc điểm theo KetQuaValidator để chấp nhận cả "7.5" và "7,5", không phụ thuộc cấu hình vùng.</summary>
        private void DgvKetQua_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (dgvKetQua.Columns[e.ColumnIndex].Name != "Diem") return;

            string text = Convert.ToString(e.Value);
            if (string.IsNullOrWhiteSpace(text))
            {
                e.Value = DBNull.Value;
                e.ParsingApplied = true;
            }
            else if (KetQuaValidator.TryParseDiem(text, out decimal diem))
            {
                e.Value = diem;
                e.ParsingApplied = true;
            }
        }

        // ================= 4. LƯU TẤT CẢ (TRANSACTION) =================
        private void LuuTatCa()
        {
            if (_tableKetQua == null || _tableKetQua.Rows.Count == 0)
            {
                Warn("Chưa có danh sách sinh viên để lưu. Hãy chọn lớp, môn học và bấm 'Tải danh sách'.");
                return;
            }

            if (!dgvKetQua.EndEdit())
            {
                Warn("Ô điểm đang nhập chưa hợp lệ. Hãy sửa lại hoặc nhấn Esc.");
                return;
            }

            txtTimKiem.Clear(); // hiện lại mọi dòng để kiểm tra đủ cả lớp

            if (!TryCollectItems(out List<KetQuaItem> items)) return;

            try
            {
                int saved = _dal.SaveAll(_loadedMaMon, items);
                Info($"Lưu thành công {saved} dòng điểm.");
                LoadStudents(true); // nạp lại để thấy dữ liệu đã lưu trong database
                SetStatus($"Đã lưu {saved} dòng điểm môn '{_loadedTenMon}' cho lớp '{_loadedTenLop}'.");
            }
            catch (KetQuaSaveException ex)
            {
                SetStatus("Lưu thất bại, đã rollback. Không có dòng nào được lưu.");
                MessageBox.Show(
                    $"Không lưu được điểm của sinh viên {ex.MaSV}.\nLý do: {ex.InnerException?.Message}\n\n" +
                    "Toàn bộ thao tác lưu đã bị hủy (rollback), không có dòng nào được ghi vào database.",
                    "Lưu thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                SetStatus("Lưu thất bại, không có dòng nào được lưu.");
                ShowDbError(ex, "lưu điểm");
            }
        }

        /// <summary>
        /// Đọc toàn bộ lưới và kiểm tra từng dòng. Nếu có dòng lỗi: đánh dấu lỗi, báo danh sách lỗi, trả về false (chưa lưu gì).
        /// </summary>
        private bool TryCollectItems(out List<KetQuaItem> items)
        {
            items = new List<KetQuaItem>();
            var errors = new List<string>();
            DataGridViewRow firstErrorRow = null;

            foreach (DataGridViewRow row in dgvKetQua.Rows)
            {
                row.ErrorText = "";
                if (!(row.DataBoundItem is DataRowView view)) continue;

                string maSV = Convert.ToString(view["MaSV"]);
                string hoTen = Convert.ToString(view["HoTen"]);
                string ghiChu = Convert.ToString(view["GhiChu"])?.Trim();

                string error = KetQuaValidator.ValidateDiemValue(view["Diem"], out decimal diem)
                               ?? KetQuaValidator.ValidateGhiChu(ghiChu);
                if (error != null)
                {
                    row.ErrorText = error;
                    errors.Add($"- {maSV} ({hoTen}): {error}");
                    firstErrorRow ??= row;
                    continue;
                }

                items.Add(new KetQuaItem { MaSV = maSV, HoTen = hoTen, Diem = diem, GhiChu = ghiChu });
            }

            if (errors.Count == 0) return true;

            const int maxShown = 8;
            string detail = string.Join("\n", errors.Take(maxShown));
            if (errors.Count > maxShown) detail += $"\n... và {errors.Count - maxShown} dòng lỗi khác.";

            SetStatus($"Có {errors.Count} dòng chưa hợp lệ, chưa lưu gì.");
            MessageBox.Show($"Có {errors.Count} dòng chưa hợp lệ nên chưa lưu:\n\n{detail}",
                "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            dgvKetQua.CurrentCell = firstErrorRow.Cells["Diem"];
            return false;
        }

        // ================= MỞ RỘNG: LỌC, THỐNG KÊ =================
        private void ApplyFilter()
        {
            string text = txtTimKiem.Text.Trim();
            if (text == "")
            {
                _bsKetQua.RemoveFilter();
                return;
            }

            // Escape ký tự đặc biệt của bộ lọc DataView
            string safe = text.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("*", "[*]");
            _bsKetQua.Filter = $"MaSV LIKE '%{safe}%' OR HoTen LIKE '%{safe}%'";
        }

        private void RefreshThongKe()
        {
            try
            {
                ThongKe tk = _dal.GetThongKe(_loadedMaLop, _loadedMaMon);
                string tb = tk.DiemTB.HasValue ? tk.DiemTB.Value.ToString("0.##") : "-";
                string max = tk.DiemMax.HasValue ? tk.DiemMax.Value.ToString("0.##") : "-";
                string min = tk.DiemMin.HasValue ? tk.DiemMin.Value.ToString("0.##") : "-";
                lblThongKe.Text = $"Thống kê (dữ liệu đã lưu trong database): Sĩ số {tk.SoSinhVien} | Đã có điểm {tk.SoCoDiem} | " +
                                  $"Điểm TB {tb} | Cao nhất {max} | Thấp nhất {min} | Đạt (từ 5) {tk.SoDat}/{tk.SoCoDiem}";
            }
            catch (Exception ex) when (IsDbException(ex))
            {
                lblThongKe.Text = "Thống kê: không lấy được dữ liệu.";
            }
        }

        // ================= HÀM HỖ TRỢ =================
        private bool HasUnsavedChanges() => _tableKetQua != null && _tableKetQua.GetChanges() != null;

        private bool ConfirmDiscardChanges()
        {
            if (!HasUnsavedChanges()) return true;

            return MessageBox.Show("Có điểm đã nhập nhưng chưa lưu. Bạn có muốn bỏ các thay đổi này không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void SetStatus(string message) => lblTrangThai.Text = message;

        private static bool IsDbException(Exception ex) =>
            ex is SqlException || ex is InvalidOperationException;

        private static void ShowDbError(Exception ex, string action)
        {
            string hint = "";
            if (ex is SqlException sql && sql.Number == 4060)
                hint = $"\n\nKhông tìm thấy database {Database.DatabaseName}. Hãy chạy file Database/QLSinhVienLab12.sql trong SSMS.";
            else if (ex is SqlException sql2 && sql2.Number == 2812)
                hint = "\n\nChưa có stored procedure. Hãy chạy lại file Database/QLSinhVienLab12.sql trong SSMS.";

            MessageBox.Show($"Lỗi cơ sở dữ liệu khi {action}:\n{ex.Message}{hint}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void Warn(string message) =>
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        private static void Info(string message) =>
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
