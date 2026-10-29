namespace QLSinhVienLab12.Forms;

partial class FrmNhapDiemTheoLop
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        grbChon = new GroupBox();
        lblLop = new Label();
        cboLop = new ComboBox();
        lblMonHoc = new Label();
        cboMonHoc = new ComboBox();
        btnTaiDanhSach = new Button();
        lblDangXem = new Label();
        lblLoc = new Label();
        txtLoc = new TextBox();
        chkBoQuaKiemTra = new CheckBox();
        dgvKetQua = new DataGridView();
        btnLuuTatCa = new Button();
        lblGoiY = new Label();
        statusStrip1 = new StatusStrip();
        lblStatus = new ToolStripStatusLabel();
        lblThongKe = new ToolStripStatusLabel();
        grbChon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // grbChon
        // 
        grbChon.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        grbChon.Controls.Add(lblLop);
        grbChon.Controls.Add(cboLop);
        grbChon.Controls.Add(lblMonHoc);
        grbChon.Controls.Add(cboMonHoc);
        grbChon.Controls.Add(btnTaiDanhSach);
        grbChon.Location = new Point(12, 10);
        grbChon.Name = "grbChon";
        grbChon.Size = new Size(876, 70);
        grbChon.TabIndex = 0;
        grbChon.TabStop = false;
        grbChon.Text = "Chọn lớp và môn học";
        // 
        // lblLop
        // 
        lblLop.AutoSize = true;
        lblLop.Location = new Point(15, 33);
        lblLop.Name = "lblLop";
        lblLop.Text = "Lớp:";
        // 
        // cboLop
        // 
        cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLop.Location = new Point(55, 30);
        cboLop.Name = "cboLop";
        cboLop.Size = new Size(250, 23);
        cboLop.TabIndex = 0;
        // 
        // lblMonHoc
        // 
        lblMonHoc.AutoSize = true;
        lblMonHoc.Location = new Point(325, 33);
        lblMonHoc.Name = "lblMonHoc";
        lblMonHoc.Text = "Môn học:";
        // 
        // cboMonHoc
        // 
        cboMonHoc.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMonHoc.Location = new Point(395, 30);
        cboMonHoc.Name = "cboMonHoc";
        cboMonHoc.Size = new Size(250, 23);
        cboMonHoc.TabIndex = 1;
        // 
        // btnTaiDanhSach
        // 
        btnTaiDanhSach.Location = new Point(670, 25);
        btnTaiDanhSach.Name = "btnTaiDanhSach";
        btnTaiDanhSach.Size = new Size(185, 34);
        btnTaiDanhSach.TabIndex = 2;
        btnTaiDanhSach.Text = "Tải danh sách";
        btnTaiDanhSach.UseVisualStyleBackColor = true;
        // 
        // lblDangXem
        // 
        lblDangXem.AutoSize = true;
        lblDangXem.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblDangXem.ForeColor = Color.Navy;
        lblDangXem.Location = new Point(14, 92);
        lblDangXem.Name = "lblDangXem";
        lblDangXem.Text = "Chưa tải danh sách sinh viên.";
        // 
        // lblLoc
        // 
        lblLoc.AutoSize = true;
        lblLoc.Location = new Point(14, 125);
        lblLoc.Name = "lblLoc";
        lblLoc.Text = "Lọc sinh viên:";
        // 
        // txtLoc
        // 
        txtLoc.Location = new Point(105, 122);
        txtLoc.Name = "txtLoc";
        txtLoc.PlaceholderText = "Nhập mã hoặc họ tên...";
        txtLoc.Size = new Size(240, 23);
        txtLoc.TabIndex = 3;
        // 
        // chkBoQuaKiemTra
        // 
        chkBoQuaKiemTra.AutoSize = true;
        chkBoQuaKiemTra.ForeColor = Color.DarkRed;
        chkBoQuaKiemTra.Location = new Point(400, 124);
        chkBoQuaKiemTra.Name = "chkBoQuaKiemTra";
        chkBoQuaKiemTra.TabIndex = 4;
        chkBoQuaKiemTra.Text = "Bỏ qua kiểm tra trên Form (chỉ để thử rollback)";
        chkBoQuaKiemTra.UseVisualStyleBackColor = true;
        // 
        // dgvKetQua
        // 
        dgvKetQua.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvKetQua.BackgroundColor = SystemColors.Window;
        dgvKetQua.Location = new Point(12, 155);
        dgvKetQua.Name = "dgvKetQua";
        dgvKetQua.RowHeadersVisible = false;
        dgvKetQua.Size = new Size(876, 340);
        dgvKetQua.TabIndex = 5;
        // 
        // btnLuuTatCa
        // 
        btnLuuTatCa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        btnLuuTatCa.Location = new Point(12, 505);
        btnLuuTatCa.Name = "btnLuuTatCa";
        btnLuuTatCa.Size = new Size(180, 38);
        btnLuuTatCa.TabIndex = 6;
        btnLuuTatCa.Text = "Lưu tất cả";
        btnLuuTatCa.UseVisualStyleBackColor = true;
        // 
        // lblGoiY
        // 
        lblGoiY.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        lblGoiY.AutoSize = true;
        lblGoiY.ForeColor = SystemColors.GrayText;
        lblGoiY.Location = new Point(205, 516);
        lblGoiY.Name = "lblGoiY";
        lblGoiY.Text = "Nhập điểm (0 đến 10, dùng dấu . hoặc , đều được) và ghi chú rồi bấm Lưu tất cả.";
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus, lblThongKe });
        statusStrip1.Location = new Point(0, 558);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(900, 22);
        statusStrip1.TabIndex = 7;
        // 
        // lblStatus
        // 
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(600, 17);
        lblStatus.Spring = true;
        lblStatus.Text = "Sẵn sàng";
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblThongKe
        // 
        lblThongKe.BorderSides = ToolStripStatusLabelBorderSides.Left;
        lblThongKe.Name = "lblThongKe";
        lblThongKe.Size = new Size(280, 17);
        lblThongKe.Text = "Sĩ số: 0";
        // 
        // FrmNhapDiemTheoLop
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 580);
        Controls.Add(lblGoiY);
        Controls.Add(btnLuuTatCa);
        Controls.Add(dgvKetQua);
        Controls.Add(chkBoQuaKiemTra);
        Controls.Add(txtLoc);
        Controls.Add(lblLoc);
        Controls.Add(lblDangXem);
        Controls.Add(grbChon);
        Controls.Add(statusStrip1);
        MinimumSize = new Size(860, 520);
        Name = "FrmNhapDiemTheoLop";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Nhập điểm theo lớp - QLSinhVienLab12";
        grbChon.ResumeLayout(false);
        grbChon.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private GroupBox grbChon = null!;
    private Label lblLop = null!;
    private ComboBox cboLop = null!;
    private Label lblMonHoc = null!;
    private ComboBox cboMonHoc = null!;
    private Button btnTaiDanhSach = null!;
    private Label lblDangXem = null!;
    private Label lblLoc = null!;
    private TextBox txtLoc = null!;
    private CheckBox chkBoQuaKiemTra = null!;
    private DataGridView dgvKetQua = null!;
    private Button btnLuuTatCa = null!;
    private Label lblGoiY = null!;
    private StatusStrip statusStrip1 = null!;
    private ToolStripStatusLabel lblStatus = null!;
    private ToolStripStatusLabel lblThongKe = null!;
}
