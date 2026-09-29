using System.Globalization;

namespace CourseRegistrationApp
{
    public partial class frmDangKy : Form
    {
        private static readonly CultureInfo ViCulture = new CultureInfo("vi-VN");

        public frmDangKy()
        {
            InitializeComponent();
        }

        // ===== 5.1. FORM LOAD =====
        private void frmDangKy_Load(object sender, EventArgs e)
        {
            // Nạp danh sách khóa học
            cboKhoaHoc.Items.Clear();
            cboKhoaHoc.Items.Add(new Course("C# WinForms cơ bản", 800000));
            cboKhoaHoc.Items.Add(new Course("SQL Server cơ bản", 700000));
            cboKhoaHoc.Items.Add(new Course("Web Frontend cơ bản", 750000));
            cboKhoaHoc.Items.Add(new Course("Lập trình Python cơ bản", 650000));
            cboKhoaHoc.SelectedIndex = 0;              // khóa học đầu tiên

            radOnline.Checked = true;                  // mặc định Online

            numSoThang.Minimum = 1;                    // số tháng 1..12
            numSoThang.Maximum = 12;
            numSoThang.Value = 1;

            CapNhatTongTien();                         // tổng học phí ban đầu
        }

        // ===== TÍNH HỌC PHÍ =====
        private decimal TinhTongTien()
        {
            if (cboKhoaHoc.SelectedItem is Course course)
                return course.FeePerMonth * numSoThang.Value;
            return 0;
        }

        private void CapNhatTongTien()
        {
            if (cboKhoaHoc.SelectedItem is Course course)
                lblHocPhiThang.Text = FormatTien(course.FeePerMonth);
            else
                lblHocPhiThang.Text = FormatTien(0);

            lblTongTien.Text = FormatTien(TinhTongTien());
        }

        // Đếm số ký tự họ tên (x/50)
        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {
            lblDemKyTu.Text = $"{txtHoTen.TextLength}/{txtHoTen.MaxLength}";
        }

        // Hiển thị trạng thái nhận email
        private void chkNhanEmail_CheckedChanged(object sender, EventArgs e)
        {
            lblTrangThaiEmail.Text = chkNhanEmail.Checked
                ? "Sẽ nhận email thông báo"
                : "Không nhận email thông báo";
        }

        private static string FormatTien(decimal soTien)
            => soTien.ToString("N0", ViCulture) + " VNĐ";

        private void cboKhoaHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        private void numSoThang_ValueChanged(object sender, EventArgs e)
        {
            CapNhatTongTien();
        }

        // ===== 5.2. NÚT ĐĂNG KÝ =====
        private void btnDangKy_Click(object sender, EventArgs e)
        {
            string hoTen = txtHoTen.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (hoTen == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập họ tên học viên.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            if (soDienThoai == string.Empty)
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoDienThoai.Focus();
                return;
            }

            if (cboKhoaHoc.SelectedItem is not Course course)
            {
                MessageBox.Show("Vui lòng chọn khóa học.", "Thiếu thông tin",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboKhoaHoc.Focus();
                return;
            }

            string hinhThuc = radOnline.Checked ? "Online" : "Trực tiếp";
            int soThang = (int)numSoThang.Value;
            decimal tongTien = course.FeePerMonth * soThang;
            string nhanEmail = chkNhanEmail.Checked ? "Có" : "Không";

            string phieu =
                $"Họ tên: {hoTen}{Environment.NewLine}" +
                $"Số điện thoại: {soDienThoai}{Environment.NewLine}" +
                $"Ngày sinh: {dtpNgaySinh.Value:dd/MM/yyyy}{Environment.NewLine}" +
                $"Khóa học: {course.Name}{Environment.NewLine}" +
                $"Hình thức học: {hinhThuc}{Environment.NewLine}" +
                $"Số tháng: {soThang}{Environment.NewLine}" +
                $"Tổng tiền: {FormatTien(tongTien)}{Environment.NewLine}" +
                $"Nhận email thông báo: {nhanEmail}";

            MessageBox.Show(phieu, "PHIẾU ĐĂNG KÝ KHÓA HỌC",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== 5.3. NÚT LÀM MỚI =====
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtSoDienThoai.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            chkNhanEmail.Checked = false;
            cboKhoaHoc.SelectedIndex = 0;
            radOnline.Checked = true;
            numSoThang.Value = 1;
            CapNhatTongTien();
            txtHoTen.Focus();
        }

        // ===== 5.4. NÚT THOÁT =====
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult kq = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?",
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (kq == DialogResult.Yes)
                Close();
        }
    }
}
