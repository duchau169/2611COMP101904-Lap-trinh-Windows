namespace QuanLyNhanVien
{
    /// <summary>
    /// (Bonus) Nhân viên thời vụ: Lương = Số giờ làm x Lương theo giờ.
    /// Chứng minh việc mở rộng thêm loại nhân viên mới KHÔNG cần sửa
    /// thuật toán tìm lương cao nhất / tính tổng lương ở Program.cs,
    /// nhờ vào tính đa hình (chỉ cần override TinhLuong() và HienThiThongTin()).
    /// </summary>
    public class NhanVienThoiVu : NhanVien
    {
        private double soGioLam;
        public double SoGioLam
        {
            get => soGioLam;
            set => soGioLam = (value >= 0) ? value : 0;
        }

        private double luongTheoGio;
        public double LuongTheoGio
        {
            get => luongTheoGio;
            set => luongTheoGio = (value >= 0) ? value : 0;
        }

        // Nhân viên thời vụ không có "lương cơ bản" cố định nên truyền 0 cho lớp cha
        public NhanVienThoiVu(string maNhanVien, string hoTen, double soGioLam, double luongTheoGio)
            : base(maNhanVien, hoTen, 1) // truyền tạm 1 để không bị chặn bởi ràng buộc > 0 của lớp cha
        {
            SoGioLam = soGioLam;
            LuongTheoGio = luongTheoGio;
        }

        public override double TinhLuong()
        {
            return SoGioLam * LuongTheoGio;
        }

        public override void HienThiThongTin()
        {
            Console.WriteLine(
                $"Mã NV: {MaNhanVien,-6} | Loại: Thời vụ      | Họ tên: {HoTen,-20} | " +
                $"Số giờ làm: {SoGioLam,6:N1} | Lương/giờ: {LuongTheoGio,10:N0} | " +
                $"Lương thực nhận: {TinhLuong(),12:N0}");
        }
    }
}
