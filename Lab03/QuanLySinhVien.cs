using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab03_QuanLySinhVienOOP
{
    // Class quan ly danh sach SinhVien - Main khong duoc xu ly truc tiep danh sach
    public class QuanLySinhVien
    {
        private List<SinhVien> danhSach;

        public QuanLySinhVien()
        {
            danhSach = new List<SinhVien>();
        }

        // Them sinh vien - tra ve true neu thanh cong, false neu ma da ton tai
        public bool Them(SinhVien sv)
        {
            if (TimTheoMa(sv.MaSinhVien) != null)
            {
                return false;
            }
            danhSach.Add(sv);
            return true;
        }

        // Sua diem trung binh theo ma sinh vien
        public bool Sua(string maSinhVien, double diemMoi)
        {
            SinhVien sv = TimTheoMa(maSinhVien);
            if (sv == null)
            {
                return false;
            }
            sv.DiemTrungBinh = diemMoi;
            return true;
        }

        // Xoa sinh vien theo ma
        public bool Xoa(string maSinhVien)
        {
            SinhVien sv = TimTheoMa(maSinhVien);
            if (sv == null)
            {
                return false;
            }
            danhSach.Remove(sv);
            return true;
        }

        // Tim theo ma (dung LINQ)
        public SinhVien TimTheoMa(string maSinhVien)
        {
            return danhSach.FirstOrDefault(sv =>
                sv.MaSinhVien.Equals(maSinhVien, StringComparison.OrdinalIgnoreCase));
        }

        // Tim theo ten (dung LINQ), tim gan dung - chua tu khoa
        public List<SinhVien> TimTheoTen(string tuKhoa)
        {
            return danhSach
                .Where(sv => sv.HoTen.IndexOf(tuKhoa, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        // Sap xep theo diem giam dan (dung LINQ)
        public List<SinhVien> SapXepTheoDiem()
        {
            return danhSach
                .OrderByDescending(sv => sv.DiemTrungBinh)
                .ToList();
        }

        // Loc sinh vien dat (diem >= 5), dung LINQ
        public List<SinhVien> LocSinhVienDat()
        {
            return danhSach
                .Where(sv => sv.DiemTrungBinh >= 5.0)
                .ToList();
        }

        // Lay toan bo danh sach
        public List<SinhVien> LayDanhSach()
        {
            return danhSach;
        }
    }
}
