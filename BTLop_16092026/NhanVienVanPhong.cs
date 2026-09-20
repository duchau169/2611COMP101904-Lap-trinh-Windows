namespace QuanLyNhanVien
{
    /// <summary>
    /// Nhân viên văn phòng: Lương = Lương cơ bản + Số ngày làm việc x 200.000
    /// </summary>
    public class NhanVienVanPhong : NhanVien
    {
        public const double DonGiaMoiNgay = 200000;

        private int soNgayLamViec;
        public int SoNgayLamViec
        {
            get => soNgayLamViec;
            set => soNgayLamViec = (value >= 0 && value <= 31) ? value : 0;
        }

        // Sử dụng base(...) để gọi constructor của lớp cha
        public NhanVienVanPhong(string maNhanVien, string hoTen, double luongCoBan, int soNgayLamViec)
            : base(maNhanVien, hoTen, luongCoBan)
        {
            SoNgayLamViec = soNgayLamViec;
        }

        public override double TinhLuong()
        {
            return LuongCoBan + SoNgayLamViec * DonGiaMoiNgay;
        }

        public override void HienThiThongTin()
        {
            Console.WriteLine(
                $"Mã NV: {MaNhanVien,-6} | Loại: Văn phòng    | Họ tên: {HoTen,-20} | " +
                $"Lương cơ bản: {LuongCoBan,12:N0} | Số ngày làm: {SoNgayLamViec,3} | " +
                $"Lương thực nhận: {TinhLuong(),12:N0}");
        }
    }
}
