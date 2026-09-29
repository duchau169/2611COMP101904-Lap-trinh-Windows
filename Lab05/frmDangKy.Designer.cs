namespace CourseRegistrationApp
{
    partial class frmDangKy
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTieuDe = new Label();
            grpHocVien = new GroupBox();
            lblHoTen = new Label();
            txtHoTen = new TextBox();
            lblDemKyTu = new Label();
            lblSoDienThoai = new Label();
            txtSoDienThoai = new TextBox();
            lblNgaySinh = new Label();
            dtpNgaySinh = new DateTimePicker();
            chkNhanEmail = new CheckBox();
            lblTrangThaiEmail = new Label();
            grpKhoaHoc = new GroupBox();
            lblKhoaHoc = new Label();
            cboKhoaHoc = new ComboBox();
            lblHinhThuc = new Label();
            radOnline = new RadioButton();
            radOffline = new RadioButton();
            lblSoThang = new Label();
            numSoThang = new NumericUpDown();
            lblHocPhiThangTitle = new Label();
            lblHocPhiThang = new Label();
            lblTongTienTitle = new Label();
            lblTongTien = new Label();
            btnDangKy = new Button();
            btnLamMoi = new Button();
            btnThoat = new Button();
            grpHocVien.SuspendLayout();
            grpKhoaHoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSoThang).BeginInit();
            SuspendLayout();
            //
            // lblTieuDe
            //
            lblTieuDe.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(30, 90, 160);
            lblTieuDe.Location = new Point(0, 20);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(920, 65);
            lblTieuDe.Text = "ĐĂNG KÝ KHÓA HỌC";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            //
            // grpHocVien
            //
            grpHocVien.Controls.Add(lblHoTen);
            grpHocVien.Controls.Add(txtHoTen);
            grpHocVien.Controls.Add(lblDemKyTu);
            grpHocVien.Controls.Add(lblSoDienThoai);
            grpHocVien.Controls.Add(txtSoDienThoai);
            grpHocVien.Controls.Add(lblNgaySinh);
            grpHocVien.Controls.Add(dtpNgaySinh);
            grpHocVien.Controls.Add(chkNhanEmail);
            grpHocVien.Controls.Add(lblTrangThaiEmail);
            grpHocVien.ForeColor = Color.FromArgb(30, 90, 160);
            grpHocVien.Location = new Point(25, 100);
            grpHocVien.Name = "grpHocVien";
            grpHocVien.Size = new Size(420, 330);
            grpHocVien.TabIndex = 0;
            grpHocVien.TabStop = false;
            grpHocVien.Text = "Thông tin học viên";
            //
            // lblHoTen
            //
            lblHoTen.AutoSize = true;
            lblHoTen.ForeColor = Color.DimGray;
            lblHoTen.Location = new Point(30, 58);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Text = "Họ tên";
            //
            // txtHoTen
            //
            txtHoTen.Location = new Point(140, 54);
            txtHoTen.MaxLength = 50;
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(200, 32);
            txtHoTen.TabIndex = 0;
            txtHoTen.TextChanged += txtHoTen_TextChanged;
            //
            // lblDemKyTu
            //
            lblDemKyTu.AutoSize = true;
            lblDemKyTu.ForeColor = Color.Gray;
            lblDemKyTu.Location = new Point(348, 58);
            lblDemKyTu.Name = "lblDemKyTu";
            lblDemKyTu.Text = "0/50";
            //
            // lblSoDienThoai
            //
            lblSoDienThoai.AutoSize = true;
            lblSoDienThoai.ForeColor = Color.DimGray;
            lblSoDienThoai.Location = new Point(30, 118);
            lblSoDienThoai.Name = "lblSoDienThoai";
            lblSoDienThoai.Text = "SĐT";
            //
            // txtSoDienThoai
            //
            txtSoDienThoai.Location = new Point(140, 114);
            txtSoDienThoai.MaxLength = 15;
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.Size = new Size(200, 32);
            txtSoDienThoai.TabIndex = 1;
            //
            // lblNgaySinh
            //
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.ForeColor = Color.DimGray;
            lblNgaySinh.Location = new Point(30, 178);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Text = "Ngày sinh";
            //
            // dtpNgaySinh
            //
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.Location = new Point(140, 174);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(200, 32);
            dtpNgaySinh.TabIndex = 2;
            //
            // chkNhanEmail
            //
            chkNhanEmail.AutoSize = true;
            chkNhanEmail.ForeColor = Color.DimGray;
            chkNhanEmail.Location = new Point(140, 230);
            chkNhanEmail.Name = "chkNhanEmail";
            chkNhanEmail.TabIndex = 3;
            chkNhanEmail.Text = "Nhận email thông báo";
            chkNhanEmail.UseVisualStyleBackColor = true;
            chkNhanEmail.CheckedChanged += chkNhanEmail_CheckedChanged;
            //
            // lblTrangThaiEmail
            //
            lblTrangThaiEmail.AutoSize = true;
            lblTrangThaiEmail.ForeColor = Color.DimGray;
            lblTrangThaiEmail.Location = new Point(140, 275);
            lblTrangThaiEmail.Name = "lblTrangThaiEmail";
            lblTrangThaiEmail.Text = "Không nhận email thông báo";
            //
            // grpKhoaHoc
            //
            grpKhoaHoc.Controls.Add(lblKhoaHoc);
            grpKhoaHoc.Controls.Add(cboKhoaHoc);
            grpKhoaHoc.Controls.Add(lblHinhThuc);
            grpKhoaHoc.Controls.Add(radOnline);
            grpKhoaHoc.Controls.Add(radOffline);
            grpKhoaHoc.Controls.Add(lblSoThang);
            grpKhoaHoc.Controls.Add(numSoThang);
            grpKhoaHoc.Controls.Add(lblHocPhiThangTitle);
            grpKhoaHoc.Controls.Add(lblHocPhiThang);
            grpKhoaHoc.Controls.Add(lblTongTienTitle);
            grpKhoaHoc.Controls.Add(lblTongTien);
            grpKhoaHoc.ForeColor = Color.FromArgb(30, 90, 160);
            grpKhoaHoc.Location = new Point(465, 100);
            grpKhoaHoc.Name = "grpKhoaHoc";
            grpKhoaHoc.Size = new Size(430, 330);
            grpKhoaHoc.TabIndex = 1;
            grpKhoaHoc.TabStop = false;
            grpKhoaHoc.Text = "Thông tin khóa học";
            //
            // lblKhoaHoc
            //
            lblKhoaHoc.AutoSize = true;
            lblKhoaHoc.ForeColor = Color.DimGray;
            lblKhoaHoc.Location = new Point(30, 58);
            lblKhoaHoc.Name = "lblKhoaHoc";
            lblKhoaHoc.Text = "Khóa học";
            //
            // cboKhoaHoc
            //
            cboKhoaHoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhoaHoc.FormattingEnabled = true;
            cboKhoaHoc.Location = new Point(150, 54);
            cboKhoaHoc.Name = "cboKhoaHoc";
            cboKhoaHoc.Size = new Size(250, 33);
            cboKhoaHoc.TabIndex = 0;
            cboKhoaHoc.SelectedIndexChanged += cboKhoaHoc_SelectedIndexChanged;
            //
            // lblHinhThuc
            //
            lblHinhThuc.AutoSize = true;
            lblHinhThuc.ForeColor = Color.DimGray;
            lblHinhThuc.Location = new Point(30, 118);
            lblHinhThuc.Name = "lblHinhThuc";
            lblHinhThuc.Text = "Hình thức";
            //
            // radOnline
            //
            radOnline.AutoSize = true;
            radOnline.Checked = true;
            radOnline.ForeColor = Color.DimGray;
            radOnline.Location = new Point(150, 116);
            radOnline.Name = "radOnline";
            radOnline.TabIndex = 1;
            radOnline.TabStop = true;
            radOnline.Text = "Online";
            radOnline.UseVisualStyleBackColor = true;
            //
            // radOffline
            //
            radOffline.AutoSize = true;
            radOffline.ForeColor = Color.DimGray;
            radOffline.Location = new Point(280, 116);
            radOffline.Name = "radOffline";
            radOffline.TabIndex = 2;
            radOffline.Text = "Trực tiếp";
            radOffline.UseVisualStyleBackColor = true;
            //
            // lblSoThang
            //
            lblSoThang.AutoSize = true;
            lblSoThang.ForeColor = Color.DimGray;
            lblSoThang.Location = new Point(30, 178);
            lblSoThang.Name = "lblSoThang";
            lblSoThang.Text = "Số tháng";
            //
            // numSoThang
            //
            numSoThang.Location = new Point(150, 174);
            numSoThang.Maximum = 12;
            numSoThang.Minimum = 1;
            numSoThang.Name = "numSoThang";
            numSoThang.Size = new Size(90, 32);
            numSoThang.TabIndex = 3;
            numSoThang.Value = 1;
            numSoThang.ValueChanged += numSoThang_ValueChanged;
            //
            // lblHocPhiThangTitle
            //
            lblHocPhiThangTitle.AutoSize = true;
            lblHocPhiThangTitle.ForeColor = Color.DimGray;
            lblHocPhiThangTitle.Location = new Point(30, 228);
            lblHocPhiThangTitle.Name = "lblHocPhiThangTitle";
            lblHocPhiThangTitle.Text = "Học phí/tháng";
            //
            // lblHocPhiThang
            //
            lblHocPhiThang.AutoSize = true;
            lblHocPhiThang.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblHocPhiThang.ForeColor = Color.DimGray;
            lblHocPhiThang.Location = new Point(180, 228);
            lblHocPhiThang.Name = "lblHocPhiThang";
            lblHocPhiThang.Text = "0 VNĐ";
            //
            // lblTongTienTitle
            //
            lblTongTienTitle.AutoSize = true;
            lblTongTienTitle.ForeColor = Color.DimGray;
            lblTongTienTitle.Location = new Point(30, 278);
            lblTongTienTitle.Name = "lblTongTienTitle";
            lblTongTienTitle.Text = "Tổng tiền";
            //
            // lblTongTien
            //
            lblTongTien.AutoSize = true;
            lblTongTien.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTongTien.ForeColor = Color.Firebrick;
            lblTongTien.Location = new Point(180, 278);
            lblTongTien.Name = "lblTongTien";
            lblTongTien.Text = "0 VNĐ";
            //
            // btnDangKy
            //
            btnDangKy.BackColor = Color.SeaGreen;
            btnDangKy.FlatAppearance.BorderSize = 0;
            btnDangKy.FlatStyle = FlatStyle.Flat;
            btnDangKy.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnDangKy.ForeColor = Color.White;
            btnDangKy.Location = new Point(85, 470);
            btnDangKy.Name = "btnDangKy";
            btnDangKy.Size = new Size(200, 52);
            btnDangKy.TabIndex = 2;
            btnDangKy.Text = "Đăng ký";
            btnDangKy.UseVisualStyleBackColor = false;
            btnDangKy.Click += btnDangKy_Click;
            //
            // btnLamMoi
            //
            btnLamMoi.BackColor = Color.RoyalBlue;
            btnLamMoi.FlatAppearance.BorderSize = 0;
            btnLamMoi.FlatStyle = FlatStyle.Flat;
            btnLamMoi.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLamMoi.ForeColor = Color.White;
            btnLamMoi.Location = new Point(360, 470);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(200, 52);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = false;
            btnLamMoi.Click += btnLamMoi_Click;
            //
            // btnThoat
            //
            btnThoat.BackColor = Color.IndianRed;
            btnThoat.FlatAppearance.BorderSize = 0;
            btnThoat.FlatStyle = FlatStyle.Flat;
            btnThoat.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnThoat.ForeColor = Color.White;
            btnThoat.Location = new Point(635, 470);
            btnThoat.Name = "btnThoat";
            btnThoat.Size = new Size(200, 52);
            btnThoat.TabIndex = 4;
            btnThoat.Text = "Thoát";
            btnThoat.UseVisualStyleBackColor = false;
            btnThoat.Click += btnThoat_Click;
            //
            // frmDangKy
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(240, 246, 255);
            ClientSize = new Size(920, 560);
            Controls.Add(lblTieuDe);
            Controls.Add(grpHocVien);
            Controls.Add(grpKhoaHoc);
            Controls.Add(btnDangKy);
            Controls.Add(btnLamMoi);
            Controls.Add(btnThoat);
            Font = new Font("Segoe UI", 11F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "frmDangKy";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ĐĂNG KÝ KHÓA HỌC";
            Load += frmDangKy_Load;
            grpHocVien.ResumeLayout(false);
            grpHocVien.PerformLayout();
            grpKhoaHoc.ResumeLayout(false);
            grpKhoaHoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSoThang).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblTieuDe;
        private GroupBox grpHocVien;
        private Label lblHoTen;
        private TextBox txtHoTen;
        private Label lblDemKyTu;
        private Label lblSoDienThoai;
        private TextBox txtSoDienThoai;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private CheckBox chkNhanEmail;
        private Label lblTrangThaiEmail;
        private GroupBox grpKhoaHoc;
        private Label lblKhoaHoc;
        private ComboBox cboKhoaHoc;
        private Label lblHinhThuc;
        private RadioButton radOnline;
        private RadioButton radOffline;
        private Label lblSoThang;
        private NumericUpDown numSoThang;
        private Label lblHocPhiThangTitle;
        private Label lblHocPhiThang;
        private Label lblTongTienTitle;
        private Label lblTongTien;
        private Button btnDangKy;
        private Button btnLamMoi;
        private Button btnThoat;
    }
}
