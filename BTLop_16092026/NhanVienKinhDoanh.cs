namespace QuanLyNhanVien
{
    /// <summary>
    /// Nhân viên kinh doanh: Lương = Lương cơ bản + 5% x Doanh số
    /// </summary>
    public class NhanVienKinhDoanh : NhanVien
    {
        public const double TyLeHoaHong = 0.05;

        private double doanhSo;
        public double DoanhSo
        {
            get => doanhSo;
            set => doanhSo = (value >= 0) ? value : 0;
        }

        // Sử dụng base(...) để gọi constructor của lớp cha
        public NhanVienKinhDoanh(string maNhanVien, string hoTen, double luongCoBan, double doanhSo)
            : base(maNhanVien, hoTen, luongCoBan)
        {
            DoanhSo = doanhSo;
        }

        public override double TinhLuong()
        {
            return LuongCoBan + TyLeHoaHong * DoanhSo;
        }

        public override void HienThiThongTin()
        {
            Console.WriteLine(
                $"Mã NV: {MaNhanVien,-6} | Loại: Kinh doanh   | Họ tên: {HoTen,-20} | " +
                $"Lương cơ bản: {LuongCoBan,12:N0} | Doanh số: {DoanhSo,12:N0} | " +
                $"Lương thực nhận: {TinhLuong(),12:N0}");
        }
    }
}
