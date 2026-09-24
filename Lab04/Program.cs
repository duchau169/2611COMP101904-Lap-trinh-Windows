using System;
using System.Collections.Generic;
using System.Globalization;

namespace Lab04_QuanLySanPham
{
    class Program
    {
        static ProductService service;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Repository<Product> repo = new Repository<Product>();
            service = new ProductService(repo);

            // Dang ky lang nghe event: in thong bao khi them / xoa thanh cong
            service.ProductAdded += SanPham => Console.WriteLine($">> [EVENT] Da them san pham '{SanPham.MaSP} - {SanPham.TenSP}' vao kho.");
            service.ProductRemoved += SanPham => Console.WriteLine($">> [EVENT] Da xoa san pham '{SanPham.MaSP} - {SanPham.TenSP}' khoi kho.");

            bool tiepTuc = true;
            while (tiepTuc)
            {
                HienThiMenu();
                string luaChon = Console.ReadLine();

                try
                {
                    switch (luaChon)
                    {
                        case "1":
                            ThemSanPham();
                            break;
                        case "2":
                            XuatDanhSach(service.GetAll());
                            break;
                        case "3":
                            TimTheoMa();
                            break;
                        case "4":
                            TimTheoTen();
                            break;
                        case "5":
                            LocTheoKhoangGia();
                            break;
                        case "6":
                            XoaSanPham();
                            break;
                        case "7":
                            TinhTongGiaTriKho();
                            break;
                        case "0":
                            tiepTuc = false;
                            Console.WriteLine("Ket thuc chuong trinh. Tam biet!");
                            break;
                        default:
                            Console.WriteLine("Lua chon khong hop le. Vui long chon lai.");
                            break;
                    }
                }
                catch (DuplicateProductException ex)
                {
                    Console.WriteLine("Loi: " + ex.Message);
                }
                catch (ProductNotFoundException ex)
                {
                    Console.WriteLine("Loi: " + ex.Message);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Loi du lieu: " + ex.Message);
                }
                catch (Exception ex)
                {
                    // Bat moi loi con lai de chuong trinh khong bao gio dung dot ngot
                    Console.WriteLine("Da xay ra loi khong xac dinh: " + ex.Message);
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
            Console.WriteLine("===== PRODUCT MANAGER =====");
            Console.WriteLine("1. Them san pham");
            Console.WriteLine("2. Xuat danh sach");
            Console.WriteLine("3. Tim theo ma");
            Console.WriteLine("4. Tim theo ten");
            Console.WriteLine("5. Loc theo khoang gia");
            Console.WriteLine("6. Xoa san pham");
            Console.WriteLine("7. Tinh tong gia tri kho");
            Console.WriteLine("0. Thoat");
            Console.Write("Chon: ");
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

        static decimal NhapGia(string thongBao)
        {
            decimal gia;
            while (true)
            {
                Console.Write(thongBao);
                string input = Console.ReadLine();
                if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out gia))
                {
                    Console.WriteLine("Don gia phai la mot so. Vui long nhap lai.");
                    continue;
                }
                if (gia < 0)
                {
                    Console.WriteLine("Don gia khong duoc am. Vui long nhap lai.");
                    continue;
                }
                return gia;
            }
        }

        static int NhapSoLuong(string thongBao)
        {
            int soLuong;
            while (true)
            {
                Console.Write(thongBao);
                string input = Console.ReadLine();
                if (!int.TryParse(input, out soLuong))
                {
                    Console.WriteLine("So luong phai la mot so nguyen. Vui long nhap lai.");
                    continue;
                }
                if (soLuong < 0)
                {
                    Console.WriteLine("So luong khong duoc am. Vui long nhap lai.");
                    continue;
                }
                return soLuong;
            }
        }

        // ================== CAC CHUC NANG ==================

        static void ThemSanPham()
        {
            Console.WriteLine("\n--- THEM SAN PHAM ---");
            string ma = NhapChuoi("Nhap ma san pham: ");
            string ten = NhapChuoi("Nhap ten san pham: ");
            decimal gia = NhapGia("Nhap don gia: ");
            int soLuong = NhapSoLuong("Nhap so luong: ");

            Product sp = new Product(ma, ten, gia, soLuong);
            service.AddProduct(sp); // co the nem DuplicateProductException, duoc bat o Main
        }

        static void XuatDanhSach(List<Product> danhSach)
        {
            Console.WriteLine("\n--- DANH SACH SAN PHAM ---");
            if (danhSach == null || danhSach.Count == 0)
            {
                Console.WriteLine("Danh sach rong.");
                return;
            }
            foreach (var sp in danhSach)
            {
                Console.WriteLine(sp.ToString());
            }
        }

        static void TimTheoMa()
        {
            Console.WriteLine("\n--- TIM SAN PHAM THEO MA ---");
            string ma = NhapChuoi("Nhap ma san pham can tim: ");
            Product sp = service.TimTheoMa(ma);
            if (sp == null)
            {
                Console.WriteLine("Khong tim thay san pham co ma nay.");
            }
            else
            {
                Console.WriteLine(sp.ToString());
            }
        }

        static void TimTheoTen()
        {
            Console.WriteLine("\n--- TIM SAN PHAM THEO TEN ---");
            string tuKhoa = NhapChuoi("Nhap tu khoa ten san pham: ");
            var ketQua = service.Search(tuKhoa);
            XuatDanhSach(ketQua);
        }

        static void LocTheoKhoangGia()
        {
            Console.WriteLine("\n--- LOC THEO KHOANG GIA ---");
            decimal giaMin = NhapGia("Nhap gia nho nhat: ");
            decimal giaMax;
            while (true)
            {
                giaMax = NhapGia("Nhap gia lon nhat: ");
                if (giaMax < giaMin)
                {
                    Console.WriteLine("Gia lon nhat phai >= gia nho nhat. Vui long nhap lai.");
                    continue;
                }
                break;
            }

            var ketQua = service.Filter(giaMin, giaMax);
            XuatDanhSach(ketQua);
        }

        static void XoaSanPham()
        {
            Console.WriteLine("\n--- XOA SAN PHAM ---");
            string ma = NhapChuoi("Nhap ma san pham can xoa: ");
            service.RemoveProduct(ma); // co the nem ProductNotFoundException, duoc bat o Main
        }

        static void TinhTongGiaTriKho()
        {
            Console.WriteLine("\n--- TONG GIA TRI KHO ---");
            decimal tong = service.TinhTongGiaTriKho();
            Console.WriteLine($"Tong gia tri kho hien tai: {tong:N0}");
        }
    }
}
