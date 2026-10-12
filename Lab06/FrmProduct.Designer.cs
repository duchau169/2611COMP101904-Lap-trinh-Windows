namespace ProductManager;

partial class FrmProduct
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    // ===== MenuStrip =====
    private readonly MenuStrip menuStrip = new();
    private readonly ToolStripMenuItem mnuFile = new("File");
    private readonly ToolStripMenuItem mnuExport = new("Xuất danh sách ra .txt");
    private readonly ToolStripMenuItem mnuExit = new("Thoát");
    private readonly ToolStripMenuItem mnuManage = new("Quản lý");
    private readonly ToolStripMenuItem mnuAdd = new("Thêm");
    private readonly ToolStripMenuItem mnuEdit = new("Sửa");
    private readonly ToolStripMenuItem mnuDelete = new("Xóa");
    private readonly ToolStripMenuItem mnuRefresh = new("Làm mới");
    private readonly ToolStripMenuItem mnuSearch = new("Tìm kiếm");
    private readonly ToolStripMenuItem mnuHelp = new("Trợ giúp");
    private readonly ToolStripMenuItem mnuAbout = new("Giới thiệu");

    // ===== ToolStrip =====
    private readonly ToolStrip toolStrip = new();
    private readonly ToolStripButton tsbAdd = new("Thêm");
    private readonly ToolStripButton tsbEdit = new("Sửa");
    private readonly ToolStripButton tsbDelete = new("Xóa");
    private readonly ToolStripButton tsbRefresh = new("Làm mới");
    private readonly ToolStripButton tsbSearch = new("Tìm kiếm");

    // ===== StatusStrip =====
    private readonly StatusStrip statusStrip = new();
    private readonly ToolStripStatusLabel lblStatus = new("Sẵn sàng");
    private readonly ToolStripStatusLabel lblCount = new();
    private readonly ToolStripStatusLabel lblTotal = new();

    // ===== Khung trái: nhập liệu =====
    private readonly Panel pnlLeft = new();
    private readonly GroupBox grpInfo = new();
    private readonly TextBox txtMaSP = new();
    private readonly TextBox txtTenSP = new();
    private readonly ComboBox cboLoaiSP = new();
    private readonly NumericUpDown numDonGia = new();
    private readonly NumericUpDown numSoLuong = new();
    private readonly DateTimePicker dtpNgayNhap = new();
    private readonly CheckBox chkConKinhDoanh = new();
    private readonly PictureBox picAnh = new();
    private readonly Button btnChonAnh = new();

    // ===== Khung phải: tìm kiếm + lưới =====
    private readonly Panel pnlRight = new();
    private readonly Panel pnlSearch = new();
    private readonly Label lblTimKiem = new();
    private readonly TextBox txtTimKiem = new();
    private readonly DataGridView dgvProducts = new();

    // ===== Khác =====
    private readonly ContextMenuStrip ctxMenu = new();
    private readonly ToolStripMenuItem ctxEdit = new("Sửa");
    private readonly ToolStripMenuItem ctxDelete = new("Xóa");
    private readonly ToolStripMenuItem ctxDetail = new("Xem chi tiết");
    private readonly ErrorProvider errorProvider = new();
    private readonly OpenFileDialog dlgOpenImage = new();
    private readonly SaveFileDialog dlgSaveFile = new();

    private static Label MakeLabel(string text, int y) => new()
    {
        Text = text, AutoSize = false, TextAlign = ContentAlignment.MiddleRight,
        Left = 8, Top = y, Width = 108, Height = 27
    };

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        SuspendLayout();
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Font = new Font("Segoe UI", 10F);

        // --- MenuStrip ---
        mnuFile.DropDownItems.AddRange(new ToolStripItem[] { mnuExport, new ToolStripSeparator(), mnuExit });
        mnuManage.DropDownItems.AddRange(new ToolStripItem[] { mnuAdd, mnuEdit, mnuDelete, mnuRefresh, mnuSearch });
        mnuHelp.DropDownItems.Add(mnuAbout);
        menuStrip.Items.AddRange(new ToolStripItem[] { mnuFile, mnuManage, mnuHelp });
        mnuAdd.ShortcutKeys = Keys.Control | Keys.N;
        mnuEdit.ShortcutKeys = Keys.Control | Keys.E;
        mnuDelete.ShortcutKeys = Keys.Control | Keys.D;
        mnuRefresh.ShortcutKeys = Keys.F5;
        mnuSearch.ShortcutKeys = Keys.Control | Keys.F;
        mnuExport.ShortcutKeys = Keys.Control | Keys.S;

        // --- ToolStrip (icon vẽ bằng code) ---
        toolStrip.ImageScalingSize = new Size(16, 16);
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        tsbAdd.Image = IconFactory.Add();
        tsbEdit.Image = IconFactory.Edit();
        tsbDelete.Image = IconFactory.Delete();
        tsbRefresh.Image = IconFactory.Refresh();
        tsbSearch.Image = IconFactory.Search();
        foreach (var b in new[] { tsbAdd, tsbEdit, tsbDelete, tsbRefresh, tsbSearch })
        {
            b.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            b.ImageScaling = ToolStripItemImageScaling.SizeToFit;
        }
        toolStrip.Items.AddRange(new ToolStripItem[] { tsbAdd, tsbEdit, tsbDelete, tsbRefresh, tsbSearch });

        // --- StatusStrip ---
        lblStatus.Name = "lblStatus";
        lblStatus.Spring = true;
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        lblCount.BorderSides = ToolStripStatusLabelBorderSides.Left;
        lblTotal.BorderSides = ToolStripStatusLabelBorderSides.Left;
        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus, lblCount, lblTotal });

        // --- GroupBox nhập liệu (bên trái) ---
        grpInfo.Text = "Thông tin sản phẩm";
        grpInfo.Dock = DockStyle.Fill;
        grpInfo.Font = new Font("Segoe UI Semibold", 12.5F);

        txtMaSP.Name = "txtMaSP";           txtMaSP.SetBounds(120, 40, 170, 27);
        txtTenSP.Name = "txtTenSP";         txtTenSP.SetBounds(120, 78, 170, 27);
        cboLoaiSP.Name = "cboLoaiSP";       cboLoaiSP.SetBounds(120, 116, 170, 28);
        cboLoaiSP.DropDownStyle = ComboBoxStyle.DropDownList;
        numDonGia.Name = "numDonGia";       numDonGia.SetBounds(120, 154, 170, 27);
        numDonGia.Maximum = 1_000_000_000;  numDonGia.Increment = 1000; numDonGia.ThousandsSeparator = true;
        numSoLuong.Name = "numSoLuong";     numSoLuong.SetBounds(120, 192, 100, 27);
        numSoLuong.Maximum = 1_000_000;
        dtpNgayNhap.Name = "dtpNgayNhap";   dtpNgayNhap.SetBounds(120, 230, 130, 27);
        dtpNgayNhap.Format = DateTimePickerFormat.Short;
        chkConKinhDoanh.Name = "chkConKinhDoanh"; chkConKinhDoanh.Text = "Còn kinh doanh";
        chkConKinhDoanh.SetBounds(120, 268, 170, 27); chkConKinhDoanh.Checked = true;

        picAnh.Name = "picAnh"; picAnh.SetBounds(45, 306, 215, 150);
        picAnh.BorderStyle = BorderStyle.FixedSingle; picAnh.SizeMode = PictureBoxSizeMode.Zoom;
        btnChonAnh.Name = "btnChonAnh"; btnChonAnh.Text = "Chọn ảnh"; btnChonAnh.SetBounds(100, 466, 105, 30);

        grpInfo.Controls.AddRange(new Control[] {
            MakeLabel("Mã sản phẩm :", 40), txtMaSP,
            MakeLabel("Tên sản phẩm :", 78), txtTenSP,
            MakeLabel("Loại sản phẩm :", 116), cboLoaiSP,
            MakeLabel("Đơn giá :", 154), numDonGia,
            MakeLabel("Số lượng :", 192), numSoLuong,
            MakeLabel("Ngày nhập :", 230), dtpNgayNhap,
            chkConKinhDoanh, picAnh, btnChonAnh });
        // Các control con dùng font thường (tiêu đề GroupBox mới in đậm, to)
        foreach (Control c in grpInfo.Controls) c.Font = new Font("Segoe UI", 10F);

        pnlLeft.Dock = DockStyle.Left;
        pnlLeft.Width = 320;
        pnlLeft.Padding = new Padding(10);
        pnlLeft.Controls.Add(grpInfo);

        // --- Khung phải: ô tìm kiếm + DataGridView ---
        lblTimKiem.Text = "Tìm kiếm :";
        lblTimKiem.AutoSize = true;
        lblTimKiem.Location = new Point(2, 11);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.SetBounds(82, 8, 240, 27);
        pnlSearch.Dock = DockStyle.Top;
        pnlSearch.Height = 44;
        pnlSearch.Controls.AddRange(new Control[] { lblTimKiem, txtTimKiem });

        ctxMenu.Items.AddRange(new ToolStripItem[] { ctxEdit, ctxDelete, new ToolStripSeparator(), ctxDetail });
        dgvProducts.Name = "dgvProducts";
        dgvProducts.Dock = DockStyle.Fill;
        dgvProducts.ReadOnly = true;
        dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvProducts.MultiSelect = false;
        dgvProducts.AllowUserToAddRows = false;
        dgvProducts.AllowUserToDeleteRows = false;
        dgvProducts.AllowUserToResizeRows = false;
        dgvProducts.AutoGenerateColumns = false;
        dgvProducts.RowHeadersVisible = false;
        dgvProducts.BackgroundColor = Color.White;
        dgvProducts.ContextMenuStrip = ctxMenu;

        pnlRight.Dock = DockStyle.Fill;
        pnlRight.Padding = new Padding(10, 10, 10, 10);
        pnlRight.Controls.Add(dgvProducts);   // Fill thêm trước
        pnlRight.Controls.Add(pnlSearch);

        // --- Dialog ---
        dlgOpenImage.Title = "Chọn ảnh sản phẩm";
        dlgOpenImage.Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
        dlgSaveFile.Filter = "Text (*.txt)|*.txt";
        dlgSaveFile.FileName = "DanhSachSanPham.txt";
        errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

        // --- Form (Fill thêm trước; các control Dock khác thêm sau) ---
        Controls.Add(pnlRight);
        Controls.Add(pnlLeft);
        Controls.Add(statusStrip);
        Controls.Add(toolStrip);
        Controls.Add(menuStrip);
        MainMenuStrip = menuStrip;
        Name = "FrmProduct";
        Text = "Quản lý sản phẩm";
        ClientSize = new Size(1000, 620);
        MinimumSize = new Size(1020, 680);
        StartPosition = FormStartPosition.CenterScreen;

        // --- Sự kiện ---
        Load += FrmProduct_Load;
        tsbAdd.Click += (s, e) => OnAdd();
        tsbEdit.Click += (s, e) => OnEdit();
        tsbDelete.Click += (s, e) => OnDelete();
        tsbRefresh.Click += (s, e) => OnRefresh();
        tsbSearch.Click += (s, e) => { OnSearch(); txtTimKiem.Focus(); };
        mnuAdd.Click += (s, e) => OnAdd();
        mnuEdit.Click += (s, e) => OnEdit();
        mnuDelete.Click += (s, e) => OnDelete();
        mnuRefresh.Click += (s, e) => OnRefresh();
        mnuSearch.Click += (s, e) => { OnSearch(); txtTimKiem.Focus(); };
        mnuExport.Click += (s, e) => OnExport();
        mnuExit.Click += (s, e) => Close();
        mnuAbout.Click += (s, e) => MessageBox.Show("ProductManager - Lab 05\nCOMP1019 - Lập trình trên Windows",
            "Giới thiệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        ctxEdit.Click += (s, e) => OnEdit();
        ctxDelete.Click += (s, e) => OnDelete();
        ctxDetail.Click += (s, e) => OnViewDetail();
        btnChonAnh.Click += (s, e) => OnChooseImage();
        txtTimKiem.TextChanged += (s, e) => OnSearch();      // tìm ngay khi gõ
        txtTimKiem.KeyDown += TxtTimKiem_KeyDown;
        dgvProducts.SelectionChanged += (s, e) => OnGridSelectionChanged();
        dgvProducts.MouseDown += DgvProducts_MouseDown;
        dgvProducts.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) OnViewDetail(); };
        dgvProducts.CellFormatting += DgvProducts_CellFormatting;

        ResumeLayout(false);
        PerformLayout();
    }
}
