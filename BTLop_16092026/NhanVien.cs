namespace QuanLyNhanVien
{
    /// <summary>
    /// Lớp cơ sở đại diện cho một nhân viên.
    /// Áp dụng Encapsulation: các trường dữ liệu được đóng gói qua Property,
    /// có kiểm tra hợp lệ khi gán giá trị.
    /// </summary>
    public class NhanVien
    {
        // ----- Encapsulation: field private, truy cập qua property -----
        private string maNhanVien;
        private string hoTen;
        private double luongCoBan;

        public string MaNhanVien
        {
            get => maNhanVien;
            set => maNhanVien = value;
        }

        public string HoTen
        {
            get => hoTen;
            set => hoTen = value;
        }

        // Lương cơ bản phải > 0, nếu nhập sai sẽ mặc định về 0
        public double LuongCoBan
        {
            get => luongCoBan;
            set => luongCoBan = (value > 0) ? value : 0;
        }

        // ----- Constructor -----
        public NhanVien(string maNhanVien, string hoTen, double luongCoBan)
        {
            MaNhanVien = maNhanVien;
            HoTen = hoTen;
            LuongCoBan = luongCoBan;
        }

        /// <summary>
        /// Tính lương - phương thức virtual để các lớp con override (Đa hình).
        /// </summary>
        public virtual double TinhLuong()
        {
            return LuongCoBan;
        }

        /// <summary>
        /// Hiển thị thông tin nhân viên - phương thức virtual để các lớp con override (Đa hình).
        /// </summary>
        public virtual void HienThiThongTin()
        {
            Console.WriteLine(
                $"Mã NV: {MaNhanVien,-6} | Loại: Nhân viên     | Họ tên: {HoTen,-20} | " +
                $"Lương cơ bản: {LuongCoBan,12:N0} | Lương thực nhận: {TinhLuong(),12:N0}");
        }
    }
}
