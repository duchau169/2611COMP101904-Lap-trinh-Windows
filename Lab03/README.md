# Lab03 - Quản lý sinh viên (OOP, Console C#)

Chương trình Console C# quản lý sinh viên theo hướng đối tượng, học phần **COMP1019 - Lập trình trên Windows**.

## 1. Cấu trúc dự án

```
Lab03_QuanLySinhVienOOP
│
├── Nguoi.cs                    # Class cha: HoTen, NgaySinh, constructor, LayThongTin() (virtual)
├── SinhVien.cs                 # Ke thua Nguoi: MaSinhVien, MaLop, DiemTrungBinh (0-10), XepLoai(), override LayThongTin()
├── QuanLySinhVien.cs           # Quan ly List<SinhVien>: Them, Sua, Xoa, TimTheoMa, TimTheoTen, SapXepTheoDiem, LocSinhVienDat, LayDanhSach
├── Program.cs                  # Main, menu, cac ham nhap lieu co validate
├── Lab03_QuanLySinhVienOOP.csproj
└── screenshots/                # Anh chup ket qua chay thu chuong trinh (xem muc 3)
```

## 2. Yêu cầu kỹ thuật đã đáp ứng

- [x] Có quan hệ kế thừa: `SinhVien : Nguoi`
- [x] Có constructor cho `Nguoi` và `SinhVien`
- [x] Property `DiemTrungBinh` tự kiểm tra dữ liệu (chỉ nhận 0-10, ném lỗi nếu sai)
- [x] Dùng `List<SinhVien>` để lưu danh sách trong `QuanLySinhVien`
- [x] Dùng LINQ cho tìm kiếm (`TimTheoMa`, `TimTheoTen`), lọc (`LocSinhVienDat`) và sắp xếp (`SapXepTheoDiem`)
- [x] Không xử lý danh sách trực tiếp trong `Main` — mọi thao tác đi qua `QuanLySinhVien`
- [x] Nhập sai kiểu dữ liệu (chữ, số ngoài khoảng, ngày sai định dạng) không làm crash chương trình, chỉ báo lỗi và yêu cầu nhập lại

## 3. Cách chạy chương trình

1. Cài .NET SDK 8.0 trở lên.
2. Mở thư mục dự án bằng Visual Studio (**File → Open → Folder**) hoặc chạy trong terminal:
   ```
   cd Lab03_QuanLySinhVienOOP
   dotnet run
   ```
3. Nếu tiếng Việt hiển thị lỗi font/dấu `?` trên console (do code page của Windows Console không phải UTF-8), chạy lệnh sau trước khi chạy chương trình, hoặc đặt `chcp 65001` trong terminal:
   ```
   chcp 65001
   dotnet run
   ```
   Ngoài ra nên chọn font hỗ trợ Unicode cho cửa sổ console (ví dụ Consolas / NSimSun) trong Properties của Command Prompt/Terminal.

## 4. Kết quả kiểm thử (Test Results)

Các ảnh chụp màn hình dưới đây minh chứng chương trình chạy đúng theo bảng dữ liệu kiểm thử gợi ý trong đề bài (mục 8).

### 4.1. Thêm sinh viên - thành công
Thêm SV002 - Nguyễn Văn A, điểm 7 → thêm thành công, xếp loại tự động tính (Giỏi).

![Them sinh vien thanh cong](screenshots/01_them_sinh_vien.png)

### 4.2. Thêm sinh viên - điểm không hợp lệ
Nhập điểm 11 (ngoài khoảng 0-10) → chương trình báo lỗi và yêu cầu nhập lại, không bị crash.

![Diem khong hop le](screenshots/02_them_sinh_vien_diem_khong_hop_le.png)

### 4.3. Xuất danh sách
In toàn bộ danh sách sinh viên hiện có, đầy đủ mã, họ tên, lớp, điểm và xếp loại.

![Xuat danh sach](screenshots/03_xuat_danh_sach.png)

### 4.4. Tìm sinh viên theo mã
Nhập mã `001` → tìm thấy và in đúng thông tin sinh viên tương ứng.

![Tim theo ma](screenshots/04_tim_theo_ma.png)

### 4.5. Tìm sinh viên theo tên
Nhập từ khóa họ tên → in ra tất cả sinh viên có tên chứa từ khóa đó (dùng LINQ `Where`).

![Tim theo ten](screenshots/05_tim_theo_ten.png)

### 4.6. Sửa điểm trung bình
Nhập mã `002`, điểm mới = 8 → cập nhật thành công.

![Sua diem](screenshots/06_sua_diem.png)

### 4.7. Xóa sinh viên - mã không tồn tại
Nhập mã không có trong danh sách → chương trình thông báo không tìm thấy, không crash.

![Xoa ma khong ton tai](screenshots/07_xoa_ma_khong_ton_tai.png)

### 4.8. Xóa sinh viên - thành công
Nhập mã hợp lệ đang tồn tại → xóa thành công khỏi danh sách.

![Xoa sinh vien thanh cong](screenshots/08_xoa_sinh_vien_thanh_cong.png)

### 4.9. Sắp xếp theo điểm giảm dần
Dùng LINQ `OrderByDescending` để sắp xếp và in danh sách theo điểm từ cao xuống thấp.

![Sap xep theo diem](screenshots/09_sap_xep_theo_diem.png)

### 4.10. Lọc sinh viên đạt
Dùng LINQ `Where(diem >= 5)` để lọc và chỉ in các sinh viên đạt.

![Loc sinh vien dat](screenshots/10_loc_sinh_vien_dat.png)

### 4.11. Thoát chương trình
Chọn `0` → chương trình kết thúc bình thường, không lỗi.

![Thoat chuong trinh](screenshots/11_thoat_chuong_trinh.png)

## 5. Ghi chú

- Dữ liệu được lưu trong bộ nhớ (`List<SinhVien>`), sẽ mất khi thoát chương trình (đúng theo yêu cầu đề bài, không yêu cầu lưu file/DB).
- Ngưỡng xếp loại trong `XepLoai()`: Xuất sắc ≥ 8.5, Giỏi ≥ 7.0, Khá ≥ 5.5, Trung bình ≥ 5.0, còn lại là Yếu — có thể chỉnh lại nếu giảng viên quy định khác.
