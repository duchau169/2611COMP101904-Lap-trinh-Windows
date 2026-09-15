namespace StudentInfoApp
{
    partial class Form1
    {
        /// <summary>
        /// Biến cần thiết cho designer.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Dọn dẹp tài nguyên đang sử dụng.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new Label();

            this.lblHoTen = new Label();
            this.txtHoTen = new TextBox();

            this.lblNamSinh = new Label();
            this.txtNamSinh = new TextBox();

            this.lblEmail = new Label();
            this.txtEmail = new TextBox();

            this.grpGioiTinh = new GroupBox();
            this.radNam = new RadioButton();
            this.radNu = new RadioButton();

            this.lblKhoa = new Label();
            this.cboKhoa = new ComboBox();

            this.btnHienThi = new Button();
            this.btnXoa = new Button();
            this.btnThoat = new Button();

            this.lblKetQuaTitle = new Label();
            this.txtKetQua = new TextBox();

            this.SuspendLayout();

            // ===== lblTitle =====
            this.lblTitle.AutoSize = false;
            this.lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitle.Location = new Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(460, 35);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTitle.Text = "ỨNG DỤNG THÔNG TIN CÁ NHÂN SINH VIÊN";

            // ===== Họ tên =====
            this.lblHoTen.AutoSize = true;
            this.lblHoTen.Location = new Point(20, 65);
            this.lblHoTen.Name = "lblHoTen";
            this.lblHoTen.Size = new Size(60, 15);
            this.lblHoTen.Text = "Họ tên:";

            this.txtHoTen.Location = new Point(150, 62);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new Size(330, 23);

            // ===== Năm sinh =====
            this.lblNamSinh.AutoSize = true;
            this.lblNamSinh.Location = new Point(20, 100);
            this.lblNamSinh.Name = "lblNamSinh";
            this.lblNamSinh.Size = new Size(70, 15);
            this.lblNamSinh.Text = "Năm sinh:";

            this.txtNamSinh.Location = new Point(150, 97);
            this.txtNamSinh.Name = "txtNamSinh";
            this.txtNamSinh.Size = new Size(150, 23);

            // ===== Email =====
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new Point(20, 135);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new Size(45, 15);
            this.lblEmail.Text = "Email:";

            this.txtEmail.Location = new Point(150, 132);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(330, 23);

            // ===== GroupBox Giới tính =====
            this.grpGioiTinh.Location = new Point(20, 170);
            this.grpGioiTinh.Name = "grpGioiTinh";
            this.grpGioiTinh.Size = new Size(460, 50);
            this.grpGioiTinh.Text = "Giới tính";
            this.grpGioiTinh.Controls.Add(this.radNam);
            this.grpGioiTinh.Controls.Add(this.radNu);

            this.radNam.AutoSize = true;
            this.radNam.Location = new Point(130, 20);
            this.radNam.Name = "radNam";
            this.radNam.Size = new Size(48, 19);
            this.radNam.Text = "Nam";

            this.radNu.AutoSize = true;
            this.radNu.Location = new Point(280, 20);
            this.radNu.Name = "radNu";
            this.radNu.Size = new Size(38, 19);
            this.radNu.Text = "Nữ";

            // ===== Khoa / Lớp =====
            this.lblKhoa.AutoSize = true;
            this.lblKhoa.Location = new Point(20, 235);
            this.lblKhoa.Name = "lblKhoa";
            this.lblKhoa.Size = new Size(65, 15);
            this.lblKhoa.Text = "Khoa/Lớp:";

            this.cboKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboKhoa.Location = new Point(150, 232);
            this.cboKhoa.Name = "cboKhoa";
            this.cboKhoa.Size = new Size(330, 23);
            this.cboKhoa.Items.AddRange(new object[] {
                "Công nghệ thông tin",
                "Kỹ thuật phần mềm",
                "Hệ thống thông tin quản lý",
                "Khoa học máy tính"
            });

            // ===== Các nút lệnh =====
            this.btnHienThi.Location = new Point(20, 275);
            this.btnHienThi.Name = "btnHienThi";
            this.btnHienThi.Size = new Size(140, 32);
            this.btnHienThi.Text = "Hiển thị";
            this.btnHienThi.UseVisualStyleBackColor = true;
            this.btnHienThi.Click += new EventHandler(this.btnHienThi_Click);

            this.btnXoa.Location = new Point(170, 275);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new Size(140, 32);
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new EventHandler(this.btnXoa_Click);

            this.btnThoat.Location = new Point(320, 275);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new Size(160, 32);
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new EventHandler(this.btnThoat_Click);

            // ===== Kết quả =====
            this.lblKetQuaTitle.AutoSize = true;
            this.lblKetQuaTitle.Location = new Point(20, 320);
            this.lblKetQuaTitle.Name = "lblKetQuaTitle";
            this.lblKetQuaTitle.Size = new Size(110, 15);
            this.lblKetQuaTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            this.lblKetQuaTitle.Text = "Kết quả tổng hợp:";

            this.txtKetQua.Location = new Point(20, 340);
            this.txtKetQua.Name = "txtKetQua";
            this.txtKetQua.Size = new Size(460, 140);
            this.txtKetQua.Multiline = true;
            this.txtKetQua.ReadOnly = true;
            this.txtKetQua.ScrollBars = ScrollBars.Vertical;
            this.txtKetQua.BackColor = Color.WhiteSmoke;

            // ===== Form1 =====
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(500, 500);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblHoTen);
            this.Controls.Add(this.txtHoTen);
            this.Controls.Add(this.lblNamSinh);
            this.Controls.Add(this.txtNamSinh);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.grpGioiTinh);
            this.Controls.Add(this.lblKhoa);
            this.Controls.Add(this.cboKhoa);
            this.Controls.Add(this.btnHienThi);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.lblKetQuaTitle);
            this.Controls.Add(this.txtKetQua);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Ứng dụng thông tin cá nhân";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblTitle;

        private Label lblHoTen;
        private TextBox txtHoTen;

        private Label lblNamSinh;
        private TextBox txtNamSinh;

        private Label lblEmail;
        private TextBox txtEmail;

        private GroupBox grpGioiTinh;
        private RadioButton radNam;
        private RadioButton radNu;

        private Label lblKhoa;
        private ComboBox cboKhoa;

        private Button btnHienThi;
        private Button btnXoa;
        private Button btnThoat;

        private Label lblKetQuaTitle;
        private TextBox txtKetQua;
    }
}
