using System;

namespace Lab03_QuanLySinhVienOOP
{
    // Class con: SinhVien ke thua tu Nguoi
    public class SinhVien : Nguoi
    {
        public string MaSinhVien { get; set; }
        public string MaLop { get; set; }

        private double _diemTrungBinh;

        // Property co kiem tra du lieu: chi nhan gia tri tu 0 den 10
        public double DiemTrungBinh
        {
            get { return _diemTrungBinh; }
            set
            {
                if (value < 0 || value > 10)
                {
                    throw new ArgumentOutOfRangeException(nameof(DiemTrungBinh), "Diem trung binh phai nam trong khoang 0 - 10.");
                }
                _diemTrungBinh = value;
            }
        }

        public SinhVien() : base()
        {
            MaSinhVien = string.Empty;
            MaLop = string.Empty;
            _diemTrungBinh = 0;
        }

        public SinhVien(string maSinhVien, string hoTen, DateTime ngaySinh, string maLop, double diemTrungBinh)
            : base(hoTen, ngaySinh)
        {
            MaSinhVien = maSinhVien;
            MaLop = maLop;
            DiemTrungBinh = diemTrungBinh; // qua property de duoc kiem tra
        }

        // Xep loai dua theo diem trung binh
        public string XepLoai()
        {
            if (DiemTrungBinh >= 8.5) return "Xuat sac";
            if (DiemTrungBinh >= 7.0) return "Gioi";
            if (DiemTrungBinh >= 5.5) return "Kha";
            if (DiemTrungBinh >= 5.0) return "Trung binh";
            return "Yeu";
        }

        // Override phuong thuc LayThongTin cua lop cha
        public override string LayThongTin()
        {
            return $"Ma SV: {MaSinhVien,-8} | Ho ten: {HoTen,-25} | Lop: {MaLop,-10} | Diem: {DiemTrungBinh,-5:0.0} | Xep loai: {XepLoai()}";
        }
    }
}
