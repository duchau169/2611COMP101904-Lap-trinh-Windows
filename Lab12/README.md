# COMP1019 – Lab 12: Data Binding, Stored Procedure và Transaction

Module **Quản lý kết quả học tập** viết bằng **C# WinForms (.NET 8)**, SQL Server và ADO.NET (`Microsoft.Data.SqlClient`).
Chọn lớp, chọn môn học, nhập điểm cho cả lớp rồi bấm **Lưu tất cả**: dữ liệu được lưu bằng **transaction**, có dòng nào lỗi thì **rollback toàn bộ**.

- Sinh viên: Nguyễn Trọng Đức Hậu
- MSSV: 51.01.104.025
- Lớp: 51.CNTT.A

## Cách chạy

1. Cài SQL Server (Express hoặc Developer) và SSMS; đảm bảo dịch vụ SQL Server đang chạy.
2. Mở `Database/QLSinhVienLab12.sql` trong SSMS rồi bấm **Execute (F5)**. Script tạo database `QLSinhVienLab12`, 4 bảng, ràng buộc, dữ liệu mẫu và 5 stored procedure. Chạy lại nhiều lần vẫn an toàn.
3. Mở `QLSinhVienLab12.csproj` bằng Visual Studio 2022 (workload **.NET desktop development**, .NET 8 SDK). Package `Microsoft.Data.SqlClient` tự tải khi build.
4. Nhấn **F5** để chạy.

Chạy bằng dòng lệnh: `dotnet run` trong thư mục project.

## Connection string

Ứng dụng dùng **Windows Authentication**. Class `DAL/Database.cs` tự dò lần lượt các server sau và dùng server đầu tiên kết nối được: `localhost`, `.\SQLEXPRESS`, `(localdb)\MSSQLLocalDB`.

```
Data Source=<server>;Initial Catalog=QLSinhVienLab12;Integrated Security=True;TrustServerCertificate=True
```

**Connection string trên máy cá nhân:** `Data Source=.\SQLEXPRESS;Initial Catalog=QLSinhVienLab12;Integrated Security=True;TrustServerCertificate=True` (SQL Server Express, Windows Authentication; xem lại tên server đang dùng trong SSMS nếu khác)

Muốn cố định connection string: điền vào hằng `FixedConnectionString` trong `DAL/Database.cs`, hoặc đặt biến môi trường `QLSINHVIEN_CONNECTION`.

## Cơ sở dữ liệu

| Bảng | Cột | Ràng buộc |
|------|-----|-----------|
| `LOP` | MaLop, TenLop | PK: MaLop |
| `SINHVIEN` | MaSV, HoTen, NgaySinh, MaLop | PK: MaSV; FK: MaLop → LOP |
| `MONHOC` | MaMon, TenMon, SoTinChi | PK: MaMon |
| `KETQUA` | MaSV, MaMon, Diem, GhiChu | PK: (MaSV, MaMon); FK: MaSV → SINHVIEN, MaMon → MONHOC; CHECK: Diem từ 0 đến 10 |

Dữ liệu mẫu: 3 lớp, 4 môn học, 12 sinh viên và vài điểm có sẵn (để thấy "kèm điểm nếu đã có").

**Stored procedure**

| Tên | Chức năng |
|-----|-----------|
| `sp_Lop_GetAll` | Lấy danh sách lớp |
| `sp_MonHoc_GetAll` | Lấy danh sách môn học |
| `sp_KetQua_GetByLopMon` | Lấy sinh viên của lớp, kèm điểm môn học nếu đã có (`LEFT JOIN`) |
| `sp_KetQua_Save` | Đã có điểm thì cập nhật, chưa có thì thêm mới; chặn điểm ngoài 0 đến 10 |
| `sp_KetQua_ThongKe` | (mở rộng) Thống kê điểm của lớp theo môn |

## Chức năng

| Mã | Chức năng | Mô tả |
|----|-----------|-------|
| 1 | Load ComboBox | Mở Form tự nạp lớp và môn học vào `cboLop`, `cboMonHoc` qua `BindingSource` |
| 2 | Tải danh sách | Chọn lớp, môn học, bấm `btnTaiDanhSach` để hiện sinh viên và điểm (nếu có) lên `dgvKetQua` |
| 3 | Nhập điểm | Chỉ sửa được cột **Điểm** và **Ghi chú** (tô nền vàng nhạt); Mã SV, Họ tên chỉ đọc |
| 4 | Validation | Điểm không rỗng, là số, trong khoảng 0 đến 10, tối đa 2 chữ số thập phân; ghi chú tối đa 200 ký tự |
| 5 | Lưu tất cả | Lưu mọi dòng trong **một transaction** |
| 6 | Rollback | Một dòng lỗi thì hủy toàn bộ, báo sinh viên gây lỗi và lý do |
| 7 | Commit | Thành công thì commit, báo số dòng đã lưu, nạp lại dữ liệu từ database |

**Mở rộng:** lọc sinh viên theo mã hoặc họ tên, thống kê điểm (sĩ số, đã có điểm, TB, cao nhất, thấp nhất, số đạt), tô nền cột cho phép nhập, cảnh báo khi đóng Form hoặc tải lại mà còn điểm chưa lưu.

**Quy tắc nhập điểm:** có thể gõ `7.5` hoặc `7,5`. Khi bấm **Lưu tất cả**, **mọi sinh viên trong danh sách phải có điểm** (đề yêu cầu điểm không được rỗng); dòng nào để trống sẽ bị đánh dấu lỗi và chưa lưu gì.

## Cách demo rollback

1. Chạy `Database/Demo_Rollback_On.sql` trong SSMS (thêm ràng buộc giả: database từ chối ghi chú đúng chữ `LOI`).
2. Trong chương trình, tải lớp `CNTT01` với môn `MH004` (chưa có điểm). Nhập điểm hợp lệ cho cả 5 sinh viên, gõ ghi chú `LOI` ở sinh viên thứ 3 rồi bấm **Lưu tất cả**.
3. Chương trình báo lỗi và rollback: kiểm tra bằng `SELECT * FROM KETQUA WHERE MaMon = 'MH004'` sẽ thấy **không có dòng nào**, kể cả hai sinh viên đứng trước dòng lỗi.
4. Xóa chữ `LOI` rồi lưu lại: lưu thành công cả 5 dòng.
5. Demo xong chạy `Database/Demo_Rollback_Off.sql` để bỏ ràng buộc giả.

## Kỹ thuật chính

- **Tách lớp rõ ràng:** `FrmNhapDiemTheoLop` (giao diện), `KetQuaDAL` (truy cập database), `Database` (kết nối), `KetQuaValidator` (kiểm tra dữ liệu), `KetQuaItem`, `ThongKe` (model). Các nút chỉ gọi hàm xử lý, không chứa logic dài.
- **Data binding:** `SqlDataAdapter` đổ dữ liệu vào `DataTable`, gắn vào `BindingSource`, rồi gán cho `ComboBox` và `DataGridView`. ComboBox dùng `DisplayMember`/`ValueMember` (hiển thị "Mã - Tên", giá trị là mã).
- **Transaction:** `SqlConnection.BeginTransaction()`, gọi `sp_KetQua_Save` cho từng dòng cùng một transaction; xong thì `Commit()`, có lỗi thì `Rollback()` và ném `KetQuaSaveException` (cho biết sinh viên gây lỗi).
- **An toàn:** chỉ gọi stored procedure bằng `SqlParameter` có khai báo kiểu và độ dài (`Decimal(4,2)` cho điểm, `NVarChar` cho ghi chú để lưu đúng tiếng Việt), không nối chuỗi SQL từ dữ liệu người dùng.
- **Kiểm tra hai lớp:** Form kiểm tra trước khi gửi, stored procedure và `CHECK` constraint kiểm tra lại ở database.
- `SqlConnection`, `SqlCommand`, `SqlTransaction`, `SqlDataAdapter` đều dùng `using`; có `try/catch` hợp lý và thông báo lỗi bằng tiếng Việt.

## Cấu trúc project

```
QLSinhVienLab12
|-- Program.cs
|-- Forms
|   |-- FrmNhapDiemTheoLop.cs
|-- DAL
|   |-- Database.cs               # connection string
|   |-- KetQuaDAL.cs              # truy cập database, transaction
|-- Models
|   |-- KetQuaItem.cs
|   |-- ThongKe.cs
|-- Validation
|   |-- KetQuaValidator.cs        # kiểm tra điểm, ghi chú
|-- Database
|   |-- QLSinhVienLab12.sql       # tạo database, bảng, dữ liệu mẫu, stored procedure
|   |-- Demo_Rollback_On.sql      # bật demo rollback
|   |-- Demo_Rollback_Off.sql     # tắt demo rollback
|-- TestCases.md                  # các test case kiểm thử
```
