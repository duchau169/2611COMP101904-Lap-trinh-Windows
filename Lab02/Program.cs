using System;

namespace QuanLyMang
{
    internal class Program
    {
        // Mảng dùng chung cho toàn chương trình, null khi chưa nhập
        static int[] mang = null;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool tiepTuc = true;
            while (tiepTuc)
            {
                HienThiMenu();
                int luaChon = NhapLuaChonMenu();

                switch (luaChon)
                {
                    case 1:
                        mang = NhapMang();
                        Console.WriteLine("Nhập mảng thành công!");
                        break;

                    case 2:
                        if (KiemTraDaNhapMang())
                        {
                            Console.Write("Mảng vừa nhập: ");
                            XuatMang(mang);
                        }
                        break;

                    case 3:
                        if (KiemTraDaNhapMang())
                        {
                            int tong = TinhTong(mang);
                            Console.WriteLine($"Tổng các phần tử trong mảng = {tong}");
                        }
                        break;

                    case 4:
                        if (KiemTraDaNhapMang())
                        {
                            int max = TimMax(mang);
                            int min = TimMin(mang);
                            Console.WriteLine($"Giá trị lớn nhất = {max}");
                            Console.WriteLine($"Giá trị nhỏ nhất = {min}");
                        }
                        break;

                    case 5:
                        if (KiemTraDaNhapMang())
                        {
                            int soChan = DemChan(mang);
                            int soLe = DemLe(mang);
                            Console.WriteLine($"Số lượng phần tử chẵn = {soChan}");
                            Console.WriteLine($"Số lượng phần tử lẻ = {soLe}");
                        }
                        break;

                    case 6:
                        if (KiemTraDaNhapMang())
                        {
                            SapXepTangDan(mang);
                            Console.Write("Mảng sau khi sắp xếp tăng dần: ");
                            XuatMang(mang);
                        }
                        break;

                    case 7:
                        if (KiemTraDaNhapMang())
                        {
                            int x = NhapSoNguyen("Nhập giá trị x cần tìm: ");
                            int viTri = TimKiem(mang, x);
                            if (viTri != -1)
                            {
                                Console.WriteLine($"Tìm thấy x = {x} tại vị trí {viTri} (tính từ 0).");
                            }
                            else
                            {
                                Console.WriteLine($"Không tìm thấy x = {x} trong mảng.");
                            }
                        }
                        break;

                    case 0:
                        Console.WriteLine("Cảm ơn đã sử dụng chương trình. Tạm biệt!");
                        tiepTuc = false;
                        break;

                    default:
                        // Trường hợp nhập sai lựa chọn menu: đã được xử lý ở NhapLuaChonMenu,
                        // nhưng để an toàn vẫn có nhánh default tránh crash chương trình.
                        Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng chọn lại.");
                        break;
                }

                if (tiepTuc)
                {
                    Console.WriteLine();
                    Console.WriteLine("Nhấn phím bất kỳ để quay lại menu...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        /// <summary>
        /// In menu chức năng ra màn hình.
        /// </summary>
        static void HienThiMenu()
        {
            Console.WriteLine("===== MENU =====");
            Console.WriteLine("1. Nhap mang");
            Console.WriteLine("2. Xuat mang");
            Console.WriteLine("3. Tinh tong");
            Console.WriteLine("4. Tim max/min");
            Console.WriteLine("5. Dem chan/le");
            Console.WriteLine("6. Sap xep tang dan");
            Console.WriteLine("7. Tim kiem");
            Console.WriteLine("0. Thoat");
        }

        /// <summary>
        /// Nhập lựa chọn menu từ người dùng, đảm bảo hợp lệ (số nguyên từ 0 đến 7).
        /// Không làm chương trình dừng bất thường nếu nhập sai định dạng.
        /// </summary>
        static int NhapLuaChonMenu()
        {
            while (true)
            {
                Console.Write("Chon chuc nang: ");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int luaChon) && luaChon >= 0 && luaChon <= 7)
                {
                    return luaChon;
                }

                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập số từ 0 đến 7.");
            }
        }

        /// <summary>
        /// Kiểm tra người dùng đã nhập mảng hay chưa trước khi cho phép xử lý.
        /// </summary>
        static bool KiemTraDaNhapMang()
        {
            if (mang == null)
            {
                Console.WriteLine("Bạn chưa nhập mảng! Vui lòng chọn chức năng 1 để nhập mảng trước.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Nhập một số nguyên bất kỳ (không giới hạn dấu), yêu cầu nhập lại nếu sai định dạng.
        /// </summary>
        static int NhapSoNguyen(string message)
        {
            int soNguyen;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (int.TryParse(input, out soNguyen))
                {
                    return soNguyen;
                }

                Console.WriteLine("Dữ liệu không hợp lệ. Vui lòng nhập một số nguyên.");
            }
        }

        /// <summary>
        /// Nhập một số nguyên dương (lớn hơn 0), yêu cầu nhập lại nếu sai định dạng hoặc không dương.
        /// </summary>
        static int NhapSoNguyenDuong(string message)
        {
            int soNguyen;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine();

                if (int.TryParse(input, out soNguyen) && soNguyen > 0)
                {
                    return soNguyen;
                }

                Console.WriteLine("Dữ liệu không hợp lệ. Vui lòng nhập một số nguyên dương (> 0).");
            }
        }

        /// <summary>
        /// Nhập số lượng phần tử n (số nguyên dương) và n phần tử của mảng.
        /// </summary>
        static int[] NhapMang()
        {
            int n = NhapSoNguyenDuong("Nhập số lượng phần tử n (n > 0): ");
            int[] a = new int[n];

            for (int i = 0; i < n; i++)
            {
                a[i] = NhapSoNguyen($"Nhập phần tử thứ {i + 1}: ");
            }

            return a;
        }

        /// <summary>
        /// In toàn bộ phần tử của mảng ra màn hình, cách nhau bởi khoảng trắng.
        /// </summary>
        static void XuatMang(int[] a)
        {
            Console.Write("[ ");
            foreach (int giaTri in a)
            {
                Console.Write(giaTri + " ");
            }
            Console.WriteLine("]");
        }

        /// <summary>
        /// Tính tổng các phần tử trong mảng.
        /// </summary>
        static int TinhTong(int[] a)
        {
            int tong = 0;
            foreach (int giaTri in a)
            {
                tong += giaTri;
            }
            return tong;
        }

        /// <summary>
        /// Tìm giá trị lớn nhất trong mảng.
        /// </summary>
        static int TimMax(int[] a)
        {
            int max = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] > max)
                {
                    max = a[i];
                }
            }
            return max;
        }

        /// <summary>
        /// Tìm giá trị nhỏ nhất trong mảng.
        /// </summary>
        static int TimMin(int[] a)
        {
            int min = a[0];
            for (int i = 1; i < a.Length; i++)
            {
                if (a[i] < min)
                {
                    min = a[i];
                }
            }
            return min;
        }

        /// <summary>
        /// Đếm số lượng phần tử chẵn trong mảng.
        /// </summary>
        static int DemChan(int[] a)
        {
            int demChan = 0;
            foreach (int giaTri in a)
            {
                if (giaTri % 2 == 0)
                {
                    demChan++;
                }
            }
            return demChan;
        }

        /// <summary>
        /// Đếm số lượng phần tử lẻ trong mảng.
        /// </summary>
        static int DemLe(int[] a)
        {
            int demLe = 0;
            foreach (int giaTri in a)
            {
                if (giaTri % 2 != 0)
                {
                    demLe++;
                }
            }
            return demLe;
        }

        /// <summary>
        /// Sắp xếp mảng theo thứ tự tăng dần (thuật toán Selection Sort), thay đổi trực tiếp trên mảng gốc.
        /// </summary>
        static void SapXepTangDan(int[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int viTriNhoNhat = i;
                for (int j = i + 1; j < a.Length; j++)
                {
                    if (a[j] < a[viTriNhoNhat])
                    {
                        viTriNhoNhat = j;
                    }
                }

                if (viTriNhoNhat != i)
                {
                    (a[i], a[viTriNhoNhat]) = (a[viTriNhoNhat], a[i]);
                }
            }
        }

        /// <summary>
        /// Tìm kiếm tuần tự giá trị x trong mảng.
        /// Trả về vị trí xuất hiện đầu tiên (tính từ 0), hoặc -1 nếu không tìm thấy.
        /// </summary>
        static int TimKiem(int[] a, int x)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == x)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
