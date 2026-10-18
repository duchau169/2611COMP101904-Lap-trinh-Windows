# Test case – Lab 11: CRUD lớp học với SQL Server

Cách làm: chạy `Database/EduManagerDB.sql` trong SSMS, chạy chương trình, thực hiện từng test case rồi ghi kết quả thực tế (Đạt / Không đạt) vào cột cuối.

> Lưu ý: các test case dùng mã lớp mẫu `CNTT01`, `CNTT02`, `SPTIN01`. Nếu database của bạn là bản buổi 10 (ví dụ `22CTT1`, `22KT1`), hãy thay bằng mã lớp tương ứng: lớp đang có sinh viên dùng cho TC12, lớp chưa có sinh viên dùng cho TC13-TC14.

| STT | Chức năng | Bước thực hiện / Dữ liệu | Kết quả mong đợi | Kết quả thực tế |
|-----|-----------|--------------------------|------------------|-----------------|
| TC01 | F1 Load dữ liệu | Mở Form | Lưới hiện 3 lớp mẫu (CNTT01, CNTT02, SPTIN01) kèm số sinh viên; thanh trạng thái hiện tên server và database | |
| TC02 | F2 Chọn dòng | Bấm vào một dòng trong lưới | Mã lớp và tên lớp được đưa lên ô nhập; ô mã lớp bị khóa | |
| TC03 | F3 Thêm hợp lệ | Mã `CNTT03`, tên `Công nghệ thông tin 03`, bấm Thêm | Thông báo thành công, lưới có thêm lớp mới (kiểm tra lại trong SSMS) | |
| TC04 | F3 Mã rỗng | Để trống mã lớp, nhập tên, bấm Thêm | Cảnh báo "Mã lớp không được để trống!", không thêm | |
| TC05 | F3 Tên rỗng | Nhập mã, để trống tên, bấm Thêm | Cảnh báo "Tên lớp không được để trống!", không thêm | |
| TC06 | F3 Trùng mã | Nhập mã `CNTT01`, bấm Thêm | Cảnh báo mã lớp đã tồn tại, không thêm | |
| TC07 | F3 Trùng mã khác hoa/thường | Nhập mã `cntt01`, bấm Thêm | Cảnh báo mã lớp đã tồn tại (SQL Server không phân biệt hoa/thường) | |
| TC08 | F3 Mã có dấu hoặc khoảng trắng | Nhập mã `Lớp 01`, bấm Thêm | Cảnh báo mã lớp chỉ gồm chữ không dấu, số, `_`, `-` | |
| TC09 | F4 Sửa hợp lệ | Chọn `CNTT03`, đổi tên, bấm Sửa | Thông báo thành công, lưới hiện tên mới | |
| TC10 | F4 Chưa chọn lớp | Bấm Làm mới rồi bấm Sửa | Cảnh báo "Vui lòng chọn lớp cần sửa..." | |
| TC11 | F4 Tên rỗng khi sửa | Chọn lớp, xóa hết tên, bấm Sửa | Cảnh báo tên lớp không được để trống, dữ liệu không đổi | |
| TC12 | F5 Xóa lớp có sinh viên | Chọn `CNTT01`, bấm Xóa | Cảnh báo không thể xóa vì đang có sinh viên, lớp vẫn còn | |
| TC13 | F5 Xóa lớp trống, chọn Không | Chọn `CNTT03`, bấm Xóa, chọn Không | Không xóa, lớp vẫn còn | |
| TC14 | F5 Xóa lớp trống, chọn Có | Chọn `CNTT03`, bấm Xóa, chọn Có | Thông báo thành công, lớp biến mất khỏi lưới và khỏi database | |
| TC15 | F5 Chưa chọn lớp | Bấm Làm mới rồi bấm Xóa | Cảnh báo "Vui lòng chọn lớp cần xóa..." | |
| TC16 | F6 Tìm theo mã | Nhập `SPTIN`, bấm Tìm kiếm | Chỉ hiện lớp `SPTIN01` | |
| TC17 | F6 Tìm theo tên | Nhập `thông tin`, bấm Tìm kiếm | Hiện các lớp có tên chứa "thông tin" | |
| TC18 | F6 Không có kết quả | Nhập `zzzz`, bấm Tìm kiếm | Lưới rỗng, hiện thông báo không tìm thấy | |
| TC19 | F6 Ký tự đặc biệt | Nhập `%` rồi `'; DROP TABLE LOP;--`, bấm Tìm kiếm | Không lỗi, không tìm thấy kết quả, bảng LOP vẫn còn nguyên (chứng minh dùng parameter) | |
| TC20 | F7 Làm mới | Chọn một dòng, nhập chữ ở ô tìm kiếm, bấm Làm mới | Xóa ô nhập, mở khóa mã lớp, xóa ô tìm kiếm, lưới hiện đủ danh sách | |
| TC21 | Mất kết nối | Dừng dịch vụ SQL Server rồi bấm Làm mới | Hiện thông báo lỗi kết nối, chương trình không bị crash | |
| TC22 | Bảng LOP buổi 10 có cột bắt buộc (MaKhoa NOT NULL) | Mở Form khi bảng LOP còn cột `MaKhoa NOT NULL`, chọn Yes ở hộp thoại hỏi, rồi thêm lớp `22KT11` / `OOP` | Hộp thoại hỏi điều chỉnh cột xuất hiện; sau khi chọn Yes thêm lớp thành công, 3 lớp cũ vẫn còn nguyên | |
