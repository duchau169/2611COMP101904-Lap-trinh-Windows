using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.Data.SqlClient;
using QLSinhVienLab12.DAL;

namespace QLSinhVienLab12.Forms;

/// <summary>
/// Form nhập điểm theo lớp. Form KHÔNG viết SQL: mọi truy cập database đi qua KetQuaDAL.
/// Luồng chính: chọn lớp + môn -> Tải danh sách (DataTable -> BindingSource -> DataGridView)
/// -> nhập điểm, ghi chú -> Lưu tất cả (kiểm tra dữ liệu -> KetQuaDAL.SaveAll trong một transaction).
/// </summary>
public partial class FrmNhapDiemTheoLop : Form
{
    private readonly KetQuaDAL ketQuaDAL = new KetQuaDAL();
    private readonly BindingSource ketQuaSource = new BindingSource();

    // Lớp và môn của danh sách ĐANG HIỂN THỊ. Lưu dùng đúng cặp này (không đọc lại từ ComboBox)
    // để tránh lưu nhầm môn khi người dùng đổi ComboBox mà chưa bấm "Tải danh sách".
    private string? loadedMaLop;
    private string? loadedMaMon;

    /// <summary>Một lỗi của một dòng dữ liệu, dùng để báo lỗi và tô đỏ ô điểm trên lưới.</summary>
    private sealed record RowError(DataRow Row, string Message);

    public FrmNhapDiemTheoLop()
    {
        InitializeComponent();
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        Load += FrmNhapDiemTheoLop_Load;
        FormClosing += FrmNhapDiemTheoLop_FormClosing;

        btnTaiDanhSach.Click += btnTaiDanhSach_Click;
        btnLuuTatCa.Click += btnLuuTatCa_Click;
        txtLoc.TextChanged += txtLoc_TextChanged;
        cboLop.SelectionChangeCommitted += ComboBox_SelectionChangeCommitted;
        cboMonHoc.SelectionChangeCommitted += ComboBox_SelectionChangeCommitted;

        dgvKetQua.CellParsing += dgvKetQua_CellParsing;
        dgvKetQua.DataError += dgvKetQua_DataError;
        dgvKetQua.CellEndEdit += dgvKetQua_CellEndEdit;
    }

    // =====================================================================
    // CHỨC NĂNG 1 - LOAD FORM: nạp lớp và môn học vào ComboBox
    // =====================================================================
    private void FrmNhapDiemTheoLop_Load(object? sender, EventArgs e)
    {
        ConfigureGrid();
        dgvKetQua.DataSource = ketQuaSource;
        LoadComboBox();
    }

    private void LoadComboBox()
    {
        try
        {
            // Đặt DisplayMember/ValueMember TRƯỚC khi gán DataSource,
            // nếu không ComboBox sẽ hiện chữ "System.Data.DataRowView".
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
            cboLop.DataSource = ketQuaDAL.GetLop();

            cboMonHoc.DisplayMember = "TenMon";
            cboMonHoc.ValueMember = "MaMon";
            cboMonHoc.DataSource = ketQuaDAL.GetMonHoc();

            SetStatus("Đã tải danh sách lớp và môn học. Chọn lớp, môn học rồi bấm \"Tải danh sách\".");
        }
        catch (SqlException ex)
        {
            ShowDbError("tải danh sách lớp và môn học", ex);
            SetStatus("Không kết nối được SQL Server.");
        }
    }

    private void ConfigureGrid()
    {
        dgvKetQua.AutoGenerateColumns = false;
        dgvKetQua.AllowUserToAddRows = false;
        dgvKetQua.AllowUserToDeleteRows = false;
        dgvKetQua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvKetQua.EditMode = DataGridViewEditMode.EditOnEnter;   // chọn ô là gõ được ngay

        dgvKetQua.Columns.Clear();

        var colMaSV = new DataGridViewTextBoxColumn
        {
            Name = "colMaSV", DataPropertyName = "MaSV", HeaderText = "Mã SV", FillWeight = 15, ReadOnly = true
        };
        var colHoTen = new DataGridViewTextBoxColumn
        {
            Name = "colHoTen", DataPropertyName = "HoTen", HeaderText = "Họ tên", FillWeight = 35, ReadOnly = true
        };
        var colDiem = new DataGridViewTextBoxColumn
        {
            Name = "colDiem", DataPropertyName = "Diem", HeaderText = "Điểm", FillWeight = 15
        };
        var colGhiChu = new DataGridViewTextBoxColumn
        {
            Name = "colGhiChu", DataPropertyName = "GhiChu", HeaderText = "Ghi chú", FillWeight = 35,
            MaxInputLength = 255   // khớp NVARCHAR(255) của cột GhiChu
        };

        colMaSV.DefaultCellStyle.BackColor = SystemColors.Control;    // ô chỉ đọc có nền xám
        colHoTen.DefaultCellStyle.BackColor = SystemColors.Control;
        colDiem.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colDiem.DefaultCellStyle.Format = "0.##";
        colDiem.DefaultCellStyle.NullValue = string.Empty;

        dgvKetQua.Columns.AddRange(colMaSV, colHoTen, colDiem, colGhiChu);
    }

    // =====================================================================
    // CHỨC NĂNG 2 - TẢI DANH SÁCH SINH VIÊN (binding vào DataGridView)
    // =====================================================================
    private void btnTaiDanhSach_Click(object? sender, EventArgs e)
    {
        string maLop = cboLop.SelectedValue?.ToString() ?? string.Empty;
        string maMon = cboMonHoc.SelectedValue?.ToString() ?? string.Empty;

        if (maLop.Length == 0 || maMon.Length == 0)
        {
            Warn("Vui lòng chọn lớp và môn học.");
            return;
        }

        if (!ConfirmDiscardChanges()) return;

        LoadKetQua(maLop, maMon);
    }

    /// <summary>Nạp danh sách sinh viên của lớp kèm điểm môn học lên lưới. Trả về false nếu lỗi.</summary>
    private bool LoadKetQua(string maLop, string maMon)
    {
        try
        {
            DataTable table = ketQuaDAL.GetKetQuaByLopMon(maLop, maMon);
            table.Columns["Diem"]!.AllowDBNull = true;   // sinh viên chưa có điểm -> ô trống

            ketQuaSource.DataSource = table;
            txtLoc.Clear();

            loadedMaLop = maLop;
            loadedMaMon = maMon;
            lblDangXem.Text = $"Đang hiển thị: lớp {cboLop.Text}  -  môn {cboMonHoc.Text}";

            UpdateStatistics(table);
            SetStatus($"Đã tải {table.Rows.Count} sinh viên. Nhập điểm rồi bấm \"Lưu tất cả\".");
            return true;
        }
        catch (SqlException ex)
        {
            ShowDbError("tải danh sách sinh viên", ex);
            return false;
        }
    }

    private void ComboBox_SelectionChangeCommitted(object? sender, EventArgs e)
    {
        if (loadedMaLop == null) return;

        bool changed = cboLop.SelectedValue?.ToString() != loadedMaLop
                       || cboMonHoc.SelectedValue?.ToString() != loadedMaMon;
        if (changed)
            SetStatus("Bạn vừa đổi lớp hoặc môn học. Bấm \"Tải danh sách\" để tải lại " +
                      "(nút Lưu vẫn lưu danh sách đang hiển thị ở trên).");
    }

    // =====================================================================
    // CHỨC NĂNG 3 - NHẬP / SỬA ĐIỂM VÀ GHI CHÚ TRÊN LƯỚI
    // =====================================================================

    /// <summary>
    /// Chuẩn hóa chữ người dùng gõ vào ô điểm: chấp nhận cả dấu chấm lẫn dấu phẩy thập phân
    /// (không phụ thuộc cấu hình ngôn ngữ của Windows); ô rỗng được hiểu là chưa nhập điểm (NULL).
    /// </summary>
    private void dgvKetQua_CellParsing(object? sender, DataGridViewCellParsingEventArgs e)
    {
        if (dgvKetQua.Columns[e.ColumnIndex].Name != "colDiem") return;

        string text = (e.Value?.ToString() ?? string.Empty).Trim();
        if (text.Length == 0)
        {
            e.Value = DBNull.Value;
            e.ParsingApplied = true;
            return;
        }

        text = text.Replace(',', '.');
        if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double diem))
        {
            e.Value = diem;
            e.ParsingApplied = true;
        }
        // Không phải số: để ParsingApplied = false, lưới sẽ phát sinh DataError (xử lý bên dưới).
    }

    /// <summary>Nhập chữ vào ô điểm (không phải số): báo lỗi nhẹ nhàng thay vì hộp thoại lỗi mặc định.</summary>
    private void dgvKetQua_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;   // không cho DataGridView tự hiện hộp thoại lỗi

        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
        if (dgvKetQua.Columns[e.ColumnIndex].Name == "colDiem")
        {
            const string message = "Điểm phải là số (ví dụ 8 hoặc 7.5). Sửa lại hoặc nhấn Esc để hủy.";
            dgvKetQua.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = message;
            SetStatus(message);
            e.Cancel = true;   // giữ con trỏ ở ô để người dùng sửa
        }
    }

    private void dgvKetQua_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

        // Ô vừa sửa xong: bỏ dấu đỏ cũ (nếu có), đẩy giá trị về DataTable rồi tính lại thống kê.
        DataGridViewCell cell = dgvKetQua.Rows[e.RowIndex].Cells[e.ColumnIndex];
        cell.ErrorText = string.Empty;
        cell.Style.BackColor = Color.Empty;

        ketQuaSource.EndEdit();
        if (ketQuaSource.DataSource is DataTable table)
            UpdateStatistics(table);
    }

    // =====================================================================
    // CHỨC NĂNG 4, 5, 6, 7 - KIỂM TRA VÀ LƯU TẤT CẢ BẰNG TRANSACTION
    // =====================================================================
    private void btnLuuTatCa_Click(object? sender, EventArgs e)
    {
        if (loadedMaMon == null || ketQuaSource.DataSource is not DataTable table)
        {
            Warn("Vui lòng chọn lớp, môn học và bấm \"Tải danh sách\" trước khi lưu.");
            return;
        }

        if (table.Rows.Count == 0)
        {
            Warn("Danh sách sinh viên đang trống, không có gì để lưu.");
            return;
        }

        // Đẩy giá trị ô đang gõ dở về DataTable (nếu không, ô cuối cùng vừa nhập sẽ bị bỏ sót).
        dgvKetQua.EndEdit();
        ketQuaSource.EndEdit();
        ClearRowErrors();

        // Chế độ thử rollback: bỏ qua kiểm tra trên Form để dữ liệu sai đi tới SQL Server,
        // khi đó stored procedure báo lỗi ở giữa chừng và transaction phải Rollback tất cả.
        if (!chkBoQuaKiemTra.Checked)
        {
            List<RowError> errors = ValidateData(table);
            if (errors.Count > 0)
            {
                ShowValidationErrors(errors);
                return;
            }
        }

        try
        {
            int count = ketQuaDAL.SaveAll(loadedMaMon, table);   // commit nếu tất cả dòng thành công
            table.AcceptChanges();                               // dữ liệu trên lưới đã khớp database

            LoadKetQua(loadedMaLop!, loadedMaMon);               // tải lại để thấy đúng dữ liệu trong database
            SetStatus($"Đã lưu thành công {count} dòng (commit).");
            MessageBox.Show($"Đã lưu thành công {count} dòng.", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            // SaveAll đã Rollback: database KHÔNG có dòng nào của lần lưu này.
            SetStatus("Lưu thất bại: toàn bộ thao tác đã được hoàn tác (rollback).");
            MessageBox.Show(
                $"{ex.Message}\n\nToàn bộ thao tác lưu đã được hoàn tác (rollback), " +
                "không có dòng nào được ghi vào database.",
                "Không thể lưu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Kiểm tra TẤT CẢ dòng và gom đủ mọi lỗi (không dừng ở lỗi đầu tiên):
    /// điểm không được rỗng, phải là số và nằm trong khoảng 0 đến 10; ghi chú tối đa 255 ký tự.
    /// </summary>
    private List<RowError> ValidateData(DataTable table)
    {
        var errors = new List<RowError>();

        foreach (DataRow row in table.Rows)
        {
            string who = $"{row["MaSV"]} - {row["HoTen"]}";

            if (row.IsNull("Diem"))
            {
                errors.Add(new RowError(row, $"{who}: chưa nhập điểm."));
            }
            else
            {
                double diem = Convert.ToDouble(row["Diem"]);
                if (double.IsNaN(diem) || double.IsInfinity(diem) || diem < 0 || diem > 10)
                {
                    string shown = diem.ToString("0.##", CultureInfo.CurrentCulture);
                    errors.Add(new RowError(row, $"{who}: điểm phải từ 0 đến 10 (đang nhập {shown})."));
                }
            }

            string ghiChu = Convert.ToString(row["GhiChu"]) ?? string.Empty;
            if (ghiChu.Length > 255)
                errors.Add(new RowError(row, $"{who}: ghi chú dài quá 255 ký tự."));
        }

        return errors;
    }

    /// <summary>Tô đỏ ô điểm của các dòng lỗi, nhảy tới dòng lỗi đầu tiên và liệt kê lỗi trong một hộp thoại.</summary>
    private void ShowValidationErrors(List<RowError> errors)
    {
        txtLoc.Clear();   // bỏ lọc để mọi dòng đều hiện ra và đánh dấu được

        DataGridViewRow? firstErrorRow = null;
        foreach (DataGridViewRow gridRow in dgvKetQua.Rows)
        {
            if (gridRow.DataBoundItem is not DataRowView view) continue;

            List<RowError> rowErrors = errors.Where(err => ReferenceEquals(err.Row, view.Row)).ToList();
            if (rowErrors.Count == 0) continue;

            DataGridViewCell cell = gridRow.Cells["colDiem"];
            cell.ErrorText = string.Join(" ", rowErrors.Select(err => err.Message));
            cell.Style.BackColor = Color.MistyRose;
            firstErrorRow ??= gridRow;
        }

        if (firstErrorRow != null)
            dgvKetQua.CurrentCell = firstErrorRow.Cells["colDiem"];

        const int maxShown = 10;
        var sb = new StringBuilder();
        sb.AppendLine($"Có {errors.Count} lỗi nên chưa lưu dòng nào. Vui lòng sửa các lỗi sau:");
        sb.AppendLine();
        foreach (RowError err in errors.Take(maxShown))
            sb.AppendLine("- " + err.Message);
        if (errors.Count > maxShown)
            sb.AppendLine($"... và {errors.Count - maxShown} lỗi khác.");

        SetStatus($"Dữ liệu chưa hợp lệ ({errors.Count} lỗi), chưa lưu gì vào database.");
        MessageBox.Show(sb.ToString(), "Dữ liệu chưa hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void ClearRowErrors()
    {
        foreach (DataGridViewRow gridRow in dgvKetQua.Rows)
        {
            foreach (DataGridViewCell cell in gridRow.Cells)
            {
                cell.ErrorText = string.Empty;
                cell.Style.BackColor = Color.Empty;
            }
        }
    }

    // =====================================================================
    // MỞ RỘNG: LỌC SINH VIÊN VÀ THỐNG KÊ
    // =====================================================================
    private void txtLoc_TextChanged(object? sender, EventArgs e)
    {
        if (ketQuaSource.DataSource is not DataTable) return;

        string keyword = txtLoc.Text.Trim();
        // BindingSource.Filter chỉ lọc phần HIỂN THỊ; "Lưu tất cả" vẫn lưu toàn bộ DataTable.
        ketQuaSource.Filter = keyword.Length == 0
            ? null
            : $"MaSV LIKE '%{EscapeFilterValue(keyword)}%' OR HoTen LIKE '%{EscapeFilterValue(keyword)}%'";
    }

    /// <summary>Thoát các ký tự đặc biệt trong biểu thức lọc của DataView để gõ gì cũng không gây lỗi.</summary>
    private static string EscapeFilterValue(string text)
    {
        var sb = new StringBuilder();
        foreach (char c in text)
        {
            switch (c)
            {
                case '\'': sb.Append("''"); break;
                case '[': sb.Append("[[]"); break;
                case ']': sb.Append("[]]"); break;
                case '*': sb.Append("[*]"); break;
                case '%': sb.Append("[%]"); break;
                default: sb.Append(c); break;
            }
        }
        return sb.ToString();
    }

    /// <summary>Thống kê trên dữ liệu đang nhập: sĩ số, số người đã có điểm, điểm trung bình, cao nhất, thấp nhất.</summary>
    private void UpdateStatistics(DataTable table)
    {
        int total = 0, graded = 0;
        double sum = 0, max = double.MinValue, min = double.MaxValue;

        foreach (DataRow row in table.Rows)
        {
            total++;
            if (row.IsNull("Diem")) continue;

            double diem = Convert.ToDouble(row["Diem"]);
            graded++;
            sum += diem;
            max = Math.Max(max, diem);
            min = Math.Min(min, diem);
        }

        string text = $"Sĩ số: {total}  |  Đã có điểm: {graded}";
        if (graded > 0)
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            text += $"  |  TB: {(sum / graded).ToString("0.##", culture)}" +
                    $"  |  Cao nhất: {max.ToString("0.##", culture)}" +
                    $"  |  Thấp nhất: {min.ToString("0.##", culture)}";
        }
        lblThongKe.Text = text;
    }

    // =====================================================================
    // HÀM HỖ TRỢ
    // =====================================================================
    private bool HasUnsavedChanges()
    {
        dgvKetQua.EndEdit();
        ketQuaSource.EndEdit();
        return ketQuaSource.DataSource is DataTable table && table.GetChanges() != null;
    }

    private bool ConfirmDiscardChanges()
    {
        if (!HasUnsavedChanges()) return true;

        DialogResult result = MessageBox.Show(
            "Có điểm đã nhập nhưng chưa lưu. Bạn có muốn bỏ các thay đổi này không?",
            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        return result == DialogResult.Yes;
    }

    private void FrmNhapDiemTheoLop_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (!ConfirmDiscardChanges())
            e.Cancel = true;
    }

    private void SetStatus(string message) => lblStatus.Text = message;

    private static void Warn(string message) =>
        MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

    /// <summary>Hiện lỗi SQL kèm gợi ý cách khắc phục cho các lỗi kết nối thường gặp.</summary>
    private static void ShowDbError(string action, SqlException ex)
    {
        string hint = ex.Number switch
        {
            -1 or 2 or 53 or 233 =>
                "\n\nKhông kết nối được SQL Server. Kiểm tra: dịch vụ SQL Server đang chạy, tên server trong " +
                "chuỗi kết nối (file DAL/Database.cs hoặc biến môi trường QLSV_LAB12_CONNSTR).",
            4060 or 18456 =>
                "\n\nKhông đăng nhập được database QLSinhVienLab12. Kiểm tra database đã được tạo " +
                "(chạy Database/QLSinhVienLab12.sql) và tài khoản có quyền truy cập.",
            2812 or 208 =>
                "\n\nThiếu stored procedure hoặc bảng. Hãy chạy file Database/QLSinhVienLab12.sql trước.",
            _ => string.Empty
        };

        MessageBox.Show($"Lỗi khi {action}: {ex.Message}{hint}", "Lỗi cơ sở dữ liệu",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
