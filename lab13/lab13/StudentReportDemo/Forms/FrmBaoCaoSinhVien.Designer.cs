namespace StudentReportDemo.Forms;

partial class FrmBaoCaoSinhVien
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblLop = new Label();
        cboLop = new ComboBox();
        lblKetQua = new Label();
        cboKetQua = new ComboBox();
        lblTuKhoa = new Label();
        txtTuKhoa = new TextBox();
        btnXemBaoCao = new Button();
        btnThongKe = new Button();
        btnXuatFile = new Button();
        dgvBaoCao = new DataGridView();
        statusStrip1 = new StatusStrip();
        lblThongKe = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)dgvBaoCao).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // lblLop
        lblLop.AutoSize = true; lblLop.Location = new Point(15, 20); lblLop.Name = "lblLop"; lblLop.Text = "Lớp:";
        // cboLop
        cboLop.DropDownStyle = ComboBoxStyle.DropDownList; cboLop.Location = new Point(50, 17);
        cboLop.Name = "cboLop"; cboLop.Size = new Size(200, 23); cboLop.TabIndex = 0;
        // lblKetQua
        lblKetQua.AutoSize = true; lblKetQua.Location = new Point(270, 20); lblKetQua.Name = "lblKetQua"; lblKetQua.Text = "Kết quả:";
        // cboKetQua
        cboKetQua.DropDownStyle = ComboBoxStyle.DropDownList; cboKetQua.Location = new Point(330, 17);
        cboKetQua.Name = "cboKetQua"; cboKetQua.Size = new Size(130, 23); cboKetQua.TabIndex = 1;
        // lblTuKhoa
        lblTuKhoa.AutoSize = true; lblTuKhoa.Location = new Point(480, 20); lblTuKhoa.Name = "lblTuKhoa"; lblTuKhoa.Text = "Từ khóa:";
        // txtTuKhoa
        txtTuKhoa.Location = new Point(545, 17); txtTuKhoa.Name = "txtTuKhoa";
        txtTuKhoa.PlaceholderText = "Mã hoặc họ tên..."; txtTuKhoa.Size = new Size(210, 23); txtTuKhoa.TabIndex = 2;
        // btnXemBaoCao
        btnXemBaoCao.Location = new Point(15, 55); btnXemBaoCao.Name = "btnXemBaoCao"; btnXemBaoCao.Size = new Size(130, 34);
        btnXemBaoCao.TabIndex = 3; btnXemBaoCao.Text = "Xem báo cáo"; btnXemBaoCao.UseVisualStyleBackColor = true;
        btnXemBaoCao.Click += btnXemBaoCao_Click;
        // btnThongKe
        btnThongKe.Location = new Point(155, 55); btnThongKe.Name = "btnThongKe"; btnThongKe.Size = new Size(130, 34);
        btnThongKe.TabIndex = 4; btnThongKe.Text = "Thống kê"; btnThongKe.UseVisualStyleBackColor = true;
        btnThongKe.Click += btnThongKe_Click;
        // btnXuatFile
        btnXuatFile.Location = new Point(295, 55); btnXuatFile.Name = "btnXuatFile"; btnXuatFile.Size = new Size(130, 34);
        btnXuatFile.TabIndex = 5; btnXuatFile.Text = "Xuất file"; btnXuatFile.UseVisualStyleBackColor = true;
        btnXuatFile.Click += btnXuatFile_Click;
        // dgvBaoCao
        dgvBaoCao.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dgvBaoCao.AllowUserToAddRows = false; dgvBaoCao.AllowUserToDeleteRows = false; dgvBaoCao.ReadOnly = true;
        dgvBaoCao.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvBaoCao.BackgroundColor = SystemColors.Window; dgvBaoCao.RowHeadersVisible = false;
        dgvBaoCao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvBaoCao.Location = new Point(15, 100); dgvBaoCao.Name = "dgvBaoCao"; dgvBaoCao.Size = new Size(855, 385); dgvBaoCao.TabIndex = 6;
        // statusStrip1
        statusStrip1.Items.AddRange(new ToolStripItem[] { lblThongKe });
        statusStrip1.Name = "statusStrip1"; statusStrip1.Location = new Point(0, 498); statusStrip1.Size = new Size(885, 22);
        // lblThongKe
        lblThongKe.Name = "lblThongKe"; lblThongKe.Text = "Sẵn sàng";
        // FrmBaoCaoSinhVien
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(885, 520);
        Controls.Add(dgvBaoCao); Controls.Add(btnXuatFile); Controls.Add(btnThongKe); Controls.Add(btnXemBaoCao);
        Controls.Add(txtTuKhoa); Controls.Add(lblTuKhoa); Controls.Add(cboKetQua); Controls.Add(lblKetQua);
        Controls.Add(cboLop); Controls.Add(lblLop); Controls.Add(statusStrip1);
        MinimumSize = new Size(760, 400);
        Name = "FrmBaoCaoSinhVien";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Báo cáo sinh viên";
        Load += FrmBaoCaoSinhVien_Load;
        ((System.ComponentModel.ISupportInitialize)dgvBaoCao).EndInit();
        statusStrip1.ResumeLayout(false); statusStrip1.PerformLayout();
        ResumeLayout(false); PerformLayout();
    }

    private Label lblLop = null!;
    private ComboBox cboLop = null!;
    private Label lblKetQua = null!;
    private ComboBox cboKetQua = null!;
    private Label lblTuKhoa = null!;
    private TextBox txtTuKhoa = null!;
    private Button btnXemBaoCao = null!;
    private Button btnThongKe = null!;
    private Button btnXuatFile = null!;
    private DataGridView dgvBaoCao = null!;
    private StatusStrip statusStrip1 = null!;
    private ToolStripStatusLabel lblThongKe = null!;
}
