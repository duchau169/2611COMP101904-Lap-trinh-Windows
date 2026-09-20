# Chương trình Quản lý Nhân viên (C# Console)

## Cấu trúc project
```
QuanLyNhanVien/
├── QuanLyNhanVien.csproj
├── NhanVien.cs              # Lớp cơ sở (Class, Property, Constructor, Encapsulation)
├── NhanVienVanPhong.cs      # Kế thừa NhanVien (Đa hình: override TinhLuong/HienThiThongTin)
├── NhanVienKinhDoanh.cs     # Kế thừa NhanVien (Đa hình: override TinhLuong/HienThiThongTin)
├── NhanVienThoiVu.cs        # (Bonus) Kế thừa NhanVien, minh chứng mở rộng không cần sửa code cũ
└── Program.cs               # Chương trình chính + Menu
```

## Cách chạy
Cần cài **.NET SDK 8.0** (hoặc mới hơn). Kiểm tra: `dotnet --version`

```bash
cd QuanLyNhanVien
dotnet run
```

Chương trình sẽ yêu cầu nhập tối thiểu 5 nhân viên (chọn loại: Văn phòng /
Kinh doanh / Thời vụ), sau đó hiển thị menu:

```
========== MENU ==========
1. Xuất danh sách nhân viên
2. Tìm nhân viên theo mã
3. Tìm nhân viên có lương cao nhất
4. Tính tổng lương công ty phải trả
5. Thêm nhân viên mới
0. Thoát
```

## Cách kiến thức OOP được áp dụng

| Kiến thức | Áp dụng ở đâu |
|---|---|
| **Class** | `NhanVien`, `NhanVienVanPhong`, `NhanVienKinhDoanh`, `NhanVienThoiVu` |
| **Property** | Mọi thuộc tính đều qua `get/set` với kiểm tra hợp lệ (VD: `SoNgayLamViec` chỉ nhận 0–31) |
| **Constructor** | Mỗi lớp có constructor riêng, lớp con dùng `base(...)` gọi constructor lớp cha |
| **Encapsulation** | Field private (`maNhanVien`, `luongCoBan`,...), chỉ truy cập qua Property |
| **Kế thừa** | `NhanVienVanPhong`, `NhanVienKinhDoanh`, `NhanVienThoiVu` kế thừa `NhanVien` |
| **Đa hình** | `TinhLuong()` và `HienThiThongTin()` là `virtual`/`override`. Trong `Program.cs`, các hàm `XuatDanhSach`, `TimLuongCaoNhat`, `TinhTongLuong` chỉ gọi `nv.TinhLuong()` / `nv.HienThiThongTin()` — **không** dùng `if`/`switch` để kiểm tra loại nhân viên |

## Điểm quan trọng: mở rộng không phá vỡ code cũ
Lớp `NhanVienThoiVu` (bonus) được thêm vào **sau cùng**, nhưng:
- Không cần sửa `TimLuongCaoNhat()` hay `TinhTongLuong()` trong `Program.cs`.
- Chỉ cần viết class mới kế thừa `NhanVien` và override 2 phương thức `TinhLuong()`, `HienThiThongTin()`.

Đây chính là lợi ích cốt lõi của **Đa hình (Polymorphism)**: code xử lý danh
sách chỉ làm việc với tham chiếu kiểu `NhanVien`, không quan tâm đối tượng
thực sự là lớp con nào.
