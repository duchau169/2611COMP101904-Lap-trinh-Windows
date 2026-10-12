using System.ComponentModel;
using System.Globalization;

namespace ProductManager;

public partial class FrmProduct : Form
{
    private readonly List<Product> _allProducts = new();   // toàn bộ dữ liệu trong bộ nhớ
    private readonly BindingSource _bindingSource = new(); // dữ liệu đang hiển thị (có thể đã lọc)
    private string _imagePath = string.Empty;
    private bool _isLoadingForm;
    private static readonly CultureInfo ViCulture = new("vi-VN");

    public FrmProduct()
    {
        InitializeComponent();
        SetupGrid();
    }

    // ================= KHỞI TẠO =================

    private void FrmProduct_Load(object? sender, EventArgs e)
    {
        cboLoaiSP.Items.AddRange(new object[] { "Điện tử", "Văn phòng", "Gia dụng", "Thời trang" });
        LoadSampleData();
        ApplyFilter();
        ClearInput();
        UpdateStatus("Đã tải dữ liệu mẫu.");
    }

    private void SetupGrid()
    {
        dgvProducts.Columns.Clear();
        dgvProducts.Columns.AddRange(
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.MaSP), HeaderText = "Mã sản phẩm", FillWeight = 13 },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.TenSP), HeaderText = "Tên sản phẩm", FillWeight = 20 },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.LoaiSP), HeaderText = "Loại sản phẩm", FillWeight = 15 },
            new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Product.DonGia), HeaderText = "Đơn giá", FillWeight = 13,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", FormatProvider = ViCulture }
            },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.SoLuong), HeaderText = "Số lượng", FillWeight = 11 },
            new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Product.NgayNhap), HeaderText = "Ngày nhập", FillWeight = 13,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(Product.ConKinhDoanh), HeaderText = "Còn KD", FillWeight = 15 }
        );
        dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvProducts.DataSource = _bindingSource;
    }

    /// <summary>Cột cuối (bool) hiển thị chữ thay vì True/False.</summary>
    private void DgvProducts_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex != dgvProducts.Columns.Count - 1) return;
        if (e.Value is bool b)
        {
            e.Value = b ? "Còn kinh doanh" : "Ngừng kinh doanh";
            e.FormattingApplied = true;
        }
    }

    private void LoadSampleData()
    {
        _allProducts.AddRange(new[]
        {
            new Product { MaSP = "SP001", TenSP = "Laptop Dell",        LoaiSP = "Điện tử",   DonGia = 15000000, SoLuong = 10,  NgayNhap = new DateTime(2026, 9, 7),  ConKinhDoanh = true },
            new Product { MaSP = "SP002", TenSP = "Bút bi Thiên Long",  LoaiSP = "Văn phòng", DonGia = 5000,     SoLuong = 200, NgayNhap = new DateTime(2026, 9, 27), ConKinhDoanh = true },
            new Product { MaSP = "SP003", TenSP = "Nồi cơm điện",       LoaiSP = "Gia dụng",  DonGia = 850000,   SoLuong = 25,  NgayNhap = new DateTime(2026, 10, 2), ConKinhDoanh = false },
        });
    }

    // ================= HIỂN THỊ / TÌM KIẾM =================

    /// <summary>Lọc theo từ khóa ở ô tìm kiếm (rỗng = hiện tất cả).</summary>
    private void ApplyFilter()
    {
        string kw = txtTimKiem.Text.Trim();
        IEnumerable<Product> query = _allProducts;
        if (kw.Length > 0)
        {
            query = _allProducts.Where(p =>
                p.MaSP.Contains(kw, StringComparison.OrdinalIgnoreCase) ||
                p.TenSP.Contains(kw, StringComparison.OrdinalIgnoreCase));
        }
        _isLoadingForm = true;   // tránh tự nạp dòng đầu lên form khi gán lại dữ liệu
        try
        {
            _bindingSource.DataSource = new BindingList<Product>(query.ToList());
            dgvProducts.ClearSelection();
            dgvProducts.CurrentCell = null;
        }
        finally
        {
            _isLoadingForm = false;
        }
    }

    private void OnSearch()
    {
        ApplyFilter();
        string kw = txtTimKiem.Text.Trim();
        UpdateStatus(kw.Length == 0
            ? "Hiển thị toàn bộ sản phẩm."
            : $"Tìm thấy {_bindingSource.Count} sản phẩm với từ khóa \"{kw}\".");
    }

    private void TxtTimKiem_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            OnSearch();
        }
    }

    // ================= CRUD =================

    private void OnAdd()
    {
        if (!ValidateInput(null)) return;

        var p = new Product();
        FillProductFromForm(p);
        _allProducts.Add(p);

        txtTimKiem.Clear();
        ApplyFilter();
        ClearInput();
        UpdateStatus($"Đã thêm sản phẩm {p.MaSP}.");
    }

    private void OnEdit()
    {
        var p = GetSelectedProduct();
        if (p == null)
        {
            MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách để sửa.", "Chưa chọn dòng",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ValidateInput(p)) return;

        FillProductFromForm(p);
        ApplyFilter();
        ClearInput();
        UpdateStatus($"Đã cập nhật sản phẩm {p.MaSP}.");
    }

    private void OnDelete()
    {
        var p = GetSelectedProduct();
        if (p == null)
        {
            MessageBox.Show("Vui lòng chọn một sản phẩm trong danh sách để xóa.", "Chưa chọn dòng",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm \"{p.TenSP}\" ({p.MaSP})?",
            "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result != DialogResult.Yes) return;

        _allProducts.Remove(p);
        ApplyFilter();
        ClearInput();
        UpdateStatus($"Đã xóa sản phẩm {p.MaSP}.");
    }

    private void OnRefresh()
    {
        txtTimKiem.Clear();
        errorProvider.Clear();
        ApplyFilter();
        ClearInput();
        UpdateStatus("Đã làm mới dữ liệu.");
    }

    private void OnViewDetail()
    {
        var p = GetSelectedProduct();
        if (p == null)
        {
            MessageBox.Show("Vui lòng chọn một sản phẩm để xem chi tiết.", "Chưa chọn dòng",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        string detail =
            $"Mã SP: {p.MaSP}\n" +
            $"Tên SP: {p.TenSP}\n" +
            $"Loại: {p.LoaiSP}\n" +
            $"Đơn giá: {p.DonGia.ToString("N0", ViCulture)} đ\n" +
            $"Số lượng: {p.SoLuong}\n" +
            $"Ngày nhập: {p.NgayNhap:dd/MM/yyyy}\n" +
            $"Trạng thái: {(p.ConKinhDoanh ? "Còn kinh doanh" : "Ngừng kinh doanh")}\n" +
            $"Ảnh: {(string.IsNullOrEmpty(p.DuongDanAnh) ? "(không có)" : p.DuongDanAnh)}";
        MessageBox.Show(detail, "Chi tiết sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // ================= ẢNH / XUẤT FILE =================

    private void OnChooseImage()
    {
        if (dlgOpenImage.ShowDialog(this) != DialogResult.OK) return;

        if (ShowImage(dlgOpenImage.FileName))
        {
            _imagePath = dlgOpenImage.FileName;
            UpdateStatus("Đã chọn ảnh sản phẩm.");
        }
        else
        {
            MessageBox.Show("Không thể mở file ảnh đã chọn.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>Đọc ảnh bằng FileStream rồi copy ra Bitmap để không khóa file.</summary>
    private bool ShowImage(string path)
    {
        var old = picAnh.Image;
        picAnh.Image = null;
        old?.Dispose();

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return false;
        try
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using var img = Image.FromStream(fs);
            picAnh.Image = new Bitmap(img);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void OnExport()
    {
        if (dlgSaveFile.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            var lines = _allProducts.Select(p =>
                $"{p.MaSP}\t{p.TenSP}\t{p.LoaiSP}\t{p.DonGia:N0}\t{p.SoLuong}\t{p.NgayNhap:dd/MM/yyyy}\t{(p.ConKinhDoanh ? "Còn KD" : "Ngừng KD")}");
            File.WriteAllLines(dlgSaveFile.FileName,
                new[] { "MaSP\tTenSP\tLoaiSP\tDonGia\tSoLuong\tNgayNhap\tTrangThai" }.Concat(lines),
                System.Text.Encoding.UTF8);
            UpdateStatus($"Đã xuất {_allProducts.Count} sản phẩm ra file.");
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể ghi file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    // ================= LƯỚI =================

    private Product? GetSelectedProduct() =>
        dgvProducts.SelectedRows.Count > 0 ? dgvProducts.SelectedRows[0].DataBoundItem as Product : null;

    private void OnGridSelectionChanged()
    {
        if (_isLoadingForm) return;
        var p = GetSelectedProduct();
        if (p != null) LoadProductToForm(p);
    }

    /// <summary>Nhấn chuột phải: chọn dòng dưới con trỏ trước khi hiện ContextMenu.</summary>
    private void DgvProducts_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right) return;
        var hit = dgvProducts.HitTest(e.X, e.Y);
        if (hit.RowIndex >= 0)
        {
            dgvProducts.ClearSelection();
            dgvProducts.Rows[hit.RowIndex].Selected = true;
            dgvProducts.CurrentCell = dgvProducts.Rows[hit.RowIndex].Cells[0];
        }
    }

    // ================= FORM <-> DỮ LIỆU =================

    private void LoadProductToForm(Product p)
    {
        errorProvider.Clear();
        txtMaSP.Text = p.MaSP;
        txtTenSP.Text = p.TenSP;
        cboLoaiSP.SelectedItem = p.LoaiSP;
        numDonGia.Value = Math.Clamp(p.DonGia, numDonGia.Minimum, numDonGia.Maximum);
        numSoLuong.Value = Math.Clamp(p.SoLuong, (int)numSoLuong.Minimum, (int)numSoLuong.Maximum);
        dtpNgayNhap.Value = p.NgayNhap < dtpNgayNhap.MinDate ? dtpNgayNhap.MinDate : p.NgayNhap;
        chkConKinhDoanh.Checked = p.ConKinhDoanh;
        _imagePath = p.DuongDanAnh;
        ShowImage(_imagePath);
    }

    private void FillProductFromForm(Product p)
    {
        p.MaSP = txtMaSP.Text.Trim();
        p.TenSP = txtTenSP.Text.Trim();
        p.LoaiSP = cboLoaiSP.SelectedItem?.ToString() ?? string.Empty;
        p.DonGia = numDonGia.Value;
        p.SoLuong = (int)numSoLuong.Value;
        p.NgayNhap = dtpNgayNhap.Value.Date;
        p.ConKinhDoanh = chkConKinhDoanh.Checked;
        p.DuongDanAnh = _imagePath;
    }

    private void ClearInput()
    {
        _isLoadingForm = true;
        try
        {
            errorProvider.Clear();
            txtMaSP.Clear();
            txtTenSP.Clear();
            cboLoaiSP.SelectedIndex = -1;
            numDonGia.Value = 0;
            numSoLuong.Value = 0;
            dtpNgayNhap.Value = DateTime.Today;
            chkConKinhDoanh.Checked = true;
            _imagePath = string.Empty;
            ShowImage(string.Empty);
            dgvProducts.ClearSelection();
            dgvProducts.CurrentCell = null;
        }
        finally
        {
            _isLoadingForm = false;
        }
        txtMaSP.Focus();
    }

    // ================= VALIDATION =================

    /// <param name="editing">Sản phẩm đang sửa (null khi thêm mới) để bỏ qua khi kiểm tra trùng mã.</param>
    private bool ValidateInput(Product? editing)
    {
        errorProvider.Clear();
        var errors = new List<(Control control, string message)>();

        string ma = txtMaSP.Text.Trim();
        if (ma.Length == 0)
            errors.Add((txtMaSP, "Mã sản phẩm không được để trống."));
        else if (_allProducts.Any(x => !ReferenceEquals(x, editing) &&
                                       string.Equals(x.MaSP, ma, StringComparison.OrdinalIgnoreCase)))
            errors.Add((txtMaSP, "Mã sản phẩm đã tồn tại."));

        if (txtTenSP.Text.Trim().Length == 0)
            errors.Add((txtTenSP, "Tên sản phẩm không được để trống."));

        if (cboLoaiSP.SelectedIndex < 0)
            errors.Add((cboLoaiSP, "Vui lòng chọn loại sản phẩm."));

        if (numDonGia.Value <= 0)
            errors.Add((numDonGia, "Đơn giá phải lớn hơn 0."));

        if (numSoLuong.Value < 0)
            errors.Add((numSoLuong, "Số lượng phải lớn hơn hoặc bằng 0."));

        if (dtpNgayNhap.Value.Date > DateTime.Today)
            errors.Add((dtpNgayNhap, "Ngày nhập không được lớn hơn ngày hiện tại."));

        foreach (var (control, message) in errors)
            errorProvider.SetError(control, message);

        if (errors.Count > 0)
        {
            MessageBox.Show("Dữ liệu chưa hợp lệ:\n- " + string.Join("\n- ", errors.Select(x => x.message)),
                "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            errors[0].control.Focus();
            UpdateStatus("Dữ liệu nhập chưa hợp lệ.");
            return false;
        }
        return true;
    }

    // ================= STATUS =================

    private void UpdateStatus(string message)
    {
        lblStatus.Text = message;
        lblCount.Text = _bindingSource.Count == _allProducts.Count
            ? $"Số lượng: {_allProducts.Count}"
            : $"Hiển thị: {_bindingSource.Count}/{_allProducts.Count}";
        decimal total = _allProducts.Sum(p => p.DonGia * p.SoLuong);
        lblTotal.Text = $"Tổng giá trị tồn kho: {total.ToString("N0", ViCulture)} đ";
    }
}
