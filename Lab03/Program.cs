using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lab03_QuanLySinhVienOOP
{
    class Program
    {
        static QuanLySinhVien quanLy = new QuanLySinhVien();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool tiepTuc = true;

            while (tiepTuc)
            {
                HienThiMenu();
                string luaChon = Console.ReadLine();

                switch (luaChon)
                {
                    case "1":
                        ThemSinhVien();
                        break;
                    case "2":
                        XuatDanhSach(quanLy.LayDanhSach());
                        break;
                    case "3":
                        TimTheoMa();
                        break;
                    case "4":
                        TimTheoTen();
                        break;
                    case "5":
                        SuaDiem();
                        break;
                    case "6":
                        XoaSinhVien();
                        break;
                    case "7":
                        XuatDanhSach(quanLy.SapXepTheoDiem());
                        break;
                    case "8":
                        XuatDanhSach(quanLy.LocSinhVienDat());
                        break;
                    case "0":
                        tiepTuc = false;
                        Console.WriteLine("Ket thuc chuong trinh. Tam biet!");
                        break;
                    default:
                        Console.WriteLine("Lua chon khong hop le. Vui long chon lai.");
                        break;
                }

                if (tiepTuc)
                {
                    Console.WriteLine("\nNhan phim bat ky de tiep tuc...");
                    Console.ReadKey();
                }
            }
        }

        static void HienThiMenu()
        {
            Console.Clear();
            Console.WriteLine("===== QUAN LY SINH VIEN =====");
            Console.WriteLine("1. Them sinh vien");
            Console.WriteLine("2. Xuat danh sach");
            Console.WriteLine("3. Tim sinh vien theo ma");
            Console.WriteLine("4. Tim sinh vien theo ten");
            Console.WriteLine("5. Sua diem trung binh");
            Console.WriteLine("6. Xoa sinh vien");
            Console.WriteLine("7. Sap xep theo diem giam dan");
            Console.WriteLine("8. Loc sinh vien dat");
            Console.WriteLine("0. Thoat");
            Console.Write("Chon chuc nang: ");
        }

        // ================== CAC HAM NHAP LIEU (co kiem tra loi) ==================

        static string NhapChuoi(string thongBao, bool baoBuocRong = true)
        {
            string ketQua;
            do
            {
                Console.Write(thongBao);
                ketQua = Console.ReadLine();
                if (baoBuocRong && string.IsNullOrWhiteSpace(ketQua))
                {
                    Console.WriteLine("Gia tri khong duoc de trong. Vui long nhap lai.");
                }
            } while (baoBuocRong && string.IsNullOrWhiteSpace(ketQua));

            return ketQua;
        }

        static DateTime NhapNgay(string thongBao)
        {
            DateTime ketQua;
            while (true)
            {
                Console.Write(thongBao);
                string input = Console.ReadLine();
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out ketQua))
                {
                    return ketQua;
                }
                Console.WriteLine("Ngay sinh khong hop le. Dinh dang yeu cau: dd/MM/yyyy. Vui long nhap lai.");
            }
        }

        static double NhapDiem(string thongBao)
        {
            double diem;
            while (true)
            {
                Console.Write(thongBao);
                string input = Console.ReadLine();
                if (!double.TryParse(input, out diem))
                {
                    Console.WriteLine("Diem phai la mot so. Vui long nhap lai.");
                    continue;
                }
                if (diem < 0 || diem > 10)
                {
                    Console.WriteLine("Diem khong hop le (phai tu 0 den 10). Vui long nhap lai.");
                    continue;
                }
                return diem;
            }
        }

        // ================== CAC CHUC NANG ==================

        static void ThemSinhVien()
        {
            Console.WriteLine("\n--- THEM SINH VIEN ---");
            string ma = NhapChuoi("Nhap ma sinh vien: ");

            if (quanLy.TimTheoMa(ma) != null)
            {
                Console.WriteLine($"Ma sinh vien '{ma}' da ton tai. Khong the them.");
                return;
            }

            string hoTen = NhapChuoi("Nhap ho ten: ");
            DateTime ngaySinh = NhapNgay("Nhap ngay sinh (dd/MM/yyyy): ");
            string maLop = NhapChuoi("Nhap ma lop: ");
            double diem = NhapDiem("Nhap diem trung binh (0-10): ");

            try
            {
                SinhVien sv = new SinhVien(ma, hoTen, ngaySinh, maLop, diem);
                bool ok = quanLy.Them(sv);
                Console.WriteLine(ok
                    ? $"Them sinh vien thanh cong. Xep loai: {sv.XepLoai()}"
                    : "Them sinh vien that bai: ma da ton tai.");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine("Loi: " + ex.Message);
            }
        }

        static void XuatDanhSach(List<SinhVien> danhSach)
        {
            Console.WriteLine("\n--- DANH SACH SINH VIEN ---");
            if (danhSach == null || danhSach.Count == 0)
            {
                Console.WriteLine("Danh sach rong.");
                return;
            }
            foreach (var sv in danhSach)
            {
                Console.WriteLine(sv.LayThongTin());
            }
        }

        static void TimTheoMa()
        {
            Console.WriteLine("\n--- TIM SINH VIEN THEO MA ---");
            string ma = NhapChuoi("Nhap ma sinh vien can tim: ");
            SinhVien sv = quanLy.TimTheoMa(ma);
            if (sv == null)
            {
                Console.WriteLine("Khong tim thay sinh vien co ma nay.");
            }
            else
            {
                Console.WriteLine(sv.LayThongTin());
            }
        }

        static void TimTheoTen()
        {
            Console.WriteLine("\n--- TIM SINH VIEN THEO TEN ---");
            string tuKhoa = NhapChuoi("Nhap tu khoa ho ten: ");
            var ketQua = quanLy.TimTheoTen(tuKhoa);
            XuatDanhSach(ketQua);
        }

        static void SuaDiem()
        {
            Console.WriteLine("\n--- SUA DIEM TRUNG BINH ---");
            string ma = NhapChuoi("Nhap ma sinh vien can sua: ");
            if (quanLy.TimTheoMa(ma) == null)
            {
                Console.WriteLine("Khong tim thay sinh vien co ma nay.");
                return;
            }
            double diemMoi = NhapDiem("Nhap diem trung binh moi (0-10): ");
            bool ok = quanLy.Sua(ma, diemMoi);
            Console.WriteLine(ok ? "Cap nhat diem thanh cong." : "Cap nhat that bai.");
        }

        static void XoaSinhVien()
        {
            Console.WriteLine("\n--- XOA SINH VIEN ---");
            string ma = NhapChuoi("Nhap ma sinh vien can xoa: ");
            bool ok = quanLy.Xoa(ma);
            Console.WriteLine(ok ? "Xoa sinh vien thanh cong." : "Khong tim thay sinh vien co ma nay.");
        }
    }
}
