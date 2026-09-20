namespace QuanLyNhanVien
{
    public class Program
    {
        static List<NhanVien> danhSach = new List<NhanVien>();

        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== NHẬP DANH SÁCH NHÂN VIÊN BAN ĐẦU (tối thiểu 5 người) ===");
            NhapDanhSachBanDau(5);

            bool tiepTuc = true;
            while (tiepTuc)
            {
                HienThiMenu();
                string luaChon = Console.ReadLine() ?? "";

                switch (luaChon.Trim())
                {
                    case "1":
                        XuatDanhSach();
                        break;
                    case "2":
                        TimTheoMa();
                        break;
                    case "3":
                        TimLuongCaoNhat();
                        break;
                    case "4":
                        TinhTongLuong();
                        break;
                    case "5":
                        ThemNhanVienMoi();
                        break;
                    case "0":
                        tiepTuc = false;
                        Console.WriteLine("Đã thoát chương trình. Tạm biệt!");
                        break;
                    default:
                        Console.WriteLine(">> Lựa chọn không hợp lệ, vui lòng chọn lại.\n");
                        break;
                }
            }
        }

        // ================== MENU ==================
        static void HienThiMenu()
        {
            Console.WriteLine();
            Console.WriteLine("========== MENU ==========");
            Console.WriteLine("1. Xuất danh sách nhân viên");
            Console.WriteLine("2. Tìm nhân viên theo mã");
            Console.WriteLine("3. Tìm nhân viên có lương cao nhất");
            Console.WriteLine("4. Tính tổng lương công ty phải trả");
            Console.WriteLine("5. Thêm nhân viên mới");
            Console.WriteLine("0. Thoát");
            Console.Write("Chọn chức năng: ");
        }

        // ================== CÁC CHỨC NĂNG (dùng ĐA HÌNH, KHÔNG if/switch theo loại NV) ==================

        // 1. Xuất danh sách: chỉ gọi HienThiThongTin() -> đa hình tự chọn bản override đúng
        static void XuatDanhSach()
        {
            Console.WriteLine("\n--- DANH SÁCH NHÂN VIÊN ---");
            if (danhSach.Count == 0)
            {
                Console.WriteLine("Danh sách trống.");
                return;
            }
            foreach (NhanVien nv in danhSach)
            {
                nv.HienThiThongTin();
            }
        }

        // 2. Tìm theo mã
        static void TimTheoMa()
        {
            Console.Write("\nNhập mã nhân viên cần tìm: ");
            string ma = Console.ReadLine()?.Trim() ?? "";

            NhanVien? nv = danhSach.Find(x => x.MaNhanVien.Equals(ma, StringComparison.OrdinalIgnoreCase));

            if (nv == null)
            {
                Console.WriteLine($"Không tìm thấy nhân viên có mã '{ma}'.");
            }
            else
            {
                Console.WriteLine("Tìm thấy nhân viên:");
                nv.HienThiThongTin(); // đa hình
            }
        }

        // 3. Tìm nhân viên lương cao nhất - chỉ dựa vào TinhLuong() (đa hình), không biết/không cần biết loại NV
        static void TimLuongCaoNhat()
        {
            if (danhSach.Count == 0)
            {
                Console.WriteLine("\nDanh sách trống.");
                return;
            }

            NhanVien nvLuongCaoNhat = danhSach[0];
            foreach (NhanVien nv in danhSach)
            {
                if (nv.TinhLuong() > nvLuongCaoNhat.TinhLuong())
                {
                    nvLuongCaoNhat = nv;
                }
            }

            Console.WriteLine("\n--- NHÂN VIÊN CÓ LƯƠNG CAO NHẤT ---");
            nvLuongCaoNhat.HienThiThongTin(); // đa hình
        }

        // 4. Tính tổng lương - chỉ dựa vào TinhLuong() (đa hình)
        static void TinhTongLuong()
        {
            double tongLuong = 0;
            foreach (NhanVien nv in danhSach)
            {
                tongLuong += nv.TinhLuong();
            }
            Console.WriteLine($"\nTổng lương công ty phải trả: {tongLuong:N0} VNĐ");
        }

        // 5. Thêm nhân viên mới (mở rộng thêm cho tiện sử dụng, tái dùng logic nhập)
        static void ThemNhanVienMoi()
        {
            Console.WriteLine("\n--- THÊM NHÂN VIÊN MỚI ---");
            NhanVien nv = NhapMotNhanVien();
            danhSach.Add(nv);
            Console.WriteLine("Đã thêm nhân viên thành công!");
        }

        // ================== NHẬP LIỆU BAN ĐẦU ==================

        static void NhapDanhSachBanDau(int soLuongToiThieu)
        {
            for (int i = 1; i <= soLuongToiThieu; i++)
            {
                Console.WriteLine($"\n-- Nhập nhân viên thứ {i}/{soLuongToiThieu} --");
                NhanVien nv = NhapMotNhanVien();
                danhSach.Add(nv);
            }

            // Cho phép nhập thêm nếu muốn (không bắt buộc)
            while (true)
            {
                Console.Write("\nBạn có muốn nhập thêm nhân viên nữa không? (y/n): ");
                string dapAn = Console.ReadLine()?.Trim().ToLower() ?? "n";
                if (dapAn != "y") break;

                NhanVien nv = NhapMotNhanVien();
                danhSach.Add(nv);
            }
        }

        // Nhập 1 nhân viên: hỏi loại rồi gọi constructor tương ứng (đây là lựa chọn LOẠI khi NHẬP,
        // không phải if/switch để XỬ LÝ tính lương/hiển thị - hai việc này vẫn hoàn toàn dùng đa hình)
        static NhanVien NhapMotNhanVien()
        {
            Console.WriteLine("Chọn loại nhân viên:");
            Console.WriteLine("  1. Nhân viên văn phòng");
            Console.WriteLine("  2. Nhân viên kinh doanh");
            Console.WriteLine("  3. Nhân viên thời vụ (bonus)");

            int loai = DocSoNguyen("Nhập lựa chọn (1-3): ", 1, 3);

            Console.Write("Mã nhân viên: ");
            string ma = Console.ReadLine()?.Trim() ?? "";

            Console.Write("Họ tên: ");
            string hoTen = Console.ReadLine()?.Trim() ?? "";

            switch (loai)
            {
                case 1:
                    {
                        double luongCoBan = DocSoThuc("Lương cơ bản (> 0): ", 0.0001, double.MaxValue);
                        int soNgay = DocSoNguyen("Số ngày làm việc (0-31): ", 0, 31);
                        return new NhanVienVanPhong(ma, hoTen, luongCoBan, soNgay);
                    }
                case 2:
                    {
                        double luongCoBan = DocSoThuc("Lương cơ bản (> 0): ", 0.0001, double.MaxValue);
                        double doanhSo = DocSoThuc("Doanh số (>= 0): ", 0, double.MaxValue);
                        return new NhanVienKinhDoanh(ma, hoTen, luongCoBan, doanhSo);
                    }
                default:
                    {
                        double soGio = DocSoThuc("Số giờ làm (>= 0): ", 0, double.MaxValue);
                        double luongGio = DocSoThuc("Lương theo giờ (>= 0): ", 0, double.MaxValue);
                        return new NhanVienThoiVu(ma, hoTen, soGio, luongGio);
                    }
            }
        }

        // ================== HÀM HỖ TRỢ ĐỌC DỮ LIỆU CÓ KIỂM TRA ==================

        static int DocSoNguyen(string thongBao, int min, int max)
        {
            int giaTri;
            while (true)
            {
                Console.Write(thongBao);
                string? nhap = Console.ReadLine();
                if (int.TryParse(nhap, out giaTri) && giaTri >= min && giaTri <= max)
                {
                    return giaTri;
                }
                Console.WriteLine($">> Vui lòng nhập số nguyên trong khoảng [{min}, {max}].");
            }
        }

        static double DocSoThuc(string thongBao, double min, double max)
        {
            double giaTri;
            while (true)
            {
                Console.Write(thongBao);
                string? nhap = Console.ReadLine();
                if (double.TryParse(nhap, out giaTri) && giaTri >= min && giaTri <= max)
                {
                    return giaTri;
                }
                Console.WriteLine($">> Vui lòng nhập số hợp lệ trong khoảng [{min}, {max}].");
            }
        }
    }
}
