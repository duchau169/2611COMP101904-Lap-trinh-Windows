namespace StudentInfoApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Xử lý sự kiện Click nút "Hiển thị":
        /// Kiểm tra dữ liệu nhập, nếu hợp lệ thì tính tuổi và hiển thị thông tin tổng hợp.
        /// </summary>
        private void btnHienThi_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra họ tên không được rỗng
            string hoTen = txtHoTen.Text.Trim();
            if (string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập họ tên.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return;
            }

            // 2. Kiểm tra năm sinh không rỗng, phải là số nguyên, nằm trong khoảng hợp lệ
            string namSinhText = txtNamSinh.Text.Trim();
            if (string.IsNullOrEmpty(namSinhText))
            {
                MessageBox.Show("Vui lòng nhập năm sinh.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamSinh.Focus();
                return;
            }

            if (!int.TryParse(namSinhText, out int namSinh))
            {
                MessageBox.Show("Năm sinh phải là số nguyên.", "Dữ liệu không hợp lệ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamSinh.Focus();
                return;
            }

            int namHienTai = DateTime.Now.Year;
            if (namSinh < 1900 || namSinh > namHienTai)
            {
                MessageBox.Show($"Năm sinh phải nằm trong khoảng từ 1900 đến {namHienTai}.",
                    "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNamSinh.Focus();
                return;
            }

            // 3. Kiểm tra email không được rỗng
            string email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập email.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            // 4. Kiểm tra đã chọn giới tính chưa
            if (!radNam.Checked && !radNu.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string gioiTinh = radNam.Checked ? "Nam" : "Nữ";

            // 5. Kiểm tra đã chọn khoa/lớp chưa
            if (cboKhoa.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn khoa hoặc lớp.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string khoa = cboKhoa.SelectedItem.ToString();

            // Tất cả dữ liệu hợp lệ -> tính tuổi và hiển thị kết quả
            int tuoi = namHienTai - namSinh;

            string ketQua =
                "THÔNG TIN SINH VIÊN" + Environment.NewLine +
                $"Họ tên: {hoTen}" + Environment.NewLine +
                $"Tuổi: {tuoi}" + Environment.NewLine +
                $"Email: {email}" + Environment.NewLine +
                $"Giới tính: {gioiTinh}" + Environment.NewLine +
                $"Khoa/Lớp: {khoa}";

            // Hiển thị kết quả lên TextBox trên Form
            txtKetQua.Text = ketQua;

            // Đồng thời hiển thị bằng MessageBox theo yêu cầu đề bài
            MessageBox.Show(ketQua, "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Xử lý sự kiện Click nút "Xóa":
        /// Đưa các control về trạng thái rỗng/ban đầu.
        /// </summary>
        private void btnXoa_Click(object sender, EventArgs e)
        {
            txtHoTen.Clear();
            txtNamSinh.Clear();
            txtEmail.Clear();

            radNam.Checked = false;
            radNu.Checked = false;

            cboKhoa.SelectedIndex = -1;

            txtKetQua.Clear();

            txtHoTen.Focus();
        }

        /// <summary>
        /// Xử lý sự kiện Click nút "Thoát":
        /// Hỏi xác nhận trước khi đóng chương trình.
        /// </summary>
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn thoát chương trình không?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
