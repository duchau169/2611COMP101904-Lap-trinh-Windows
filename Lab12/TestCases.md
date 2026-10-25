# Test case – Lab 12: Nhập điểm theo lớp

Cách làm: chạy `Database/QLSinhVienLab12.sql` trong SSMS, chạy chương trình, thực hiện từng test case rồi ghi kết quả thực tế (Đạt / Không đạt) vào cột cuối. Kiểm tra dữ liệu trong database bằng SSMS: `SELECT * FROM KETQUA;`.

| STT | Chức năng | Bước thực hiện / Dữ liệu | Kết quả mong đợi | Kết quả thực tế |
|-----|-----------|--------------------------|------------------|-----------------|
| TC01 | Load ComboBox | Mở Form | `cboLop` có 3 lớp, `cboMonHoc` có 4 môn (hiển thị "Mã - Tên"); thanh trạng thái báo đã tải | |
| TC02 | Tải danh sách, chưa có điểm | Chọn `CNTT01`, môn `MH004`, bấm Tải danh sách | Hiện 5 sinh viên, cột Điểm và Ghi chú trống; nút Lưu tất cả được bật | |
| TC03 | Tải danh sách, kèm điểm có sẵn | Chọn `CNTT01`, môn `MH001`, bấm Tải danh sách | SV001 = 8,5 (ghi chú "Làm bài tốt"), SV002 = 7, các SV còn lại để trống | |
| TC04 | Chưa chọn lớp/môn | (Nếu danh sách rỗng) bấm Tải danh sách khi chưa chọn | Cảnh báo "Vui lòng chọn lớp và môn học!" | |
| TC05 | Cột chỉ đọc | Thử sửa cột Mã SV, Họ tên | Không sửa được; chỉ sửa được Điểm và Ghi chú | |
| TC06 | Điểm không phải số | Gõ `abc` vào ô Điểm rồi chuyển ô | Hiện dấu lỗi "Điểm phải là số", không cho rời ô (Esc để hủy) | |
| TC07 | Điểm âm | Gõ `-1` | Báo điểm phải nằm trong khoảng 0 đến 10 | |
| TC08 | Điểm lớn hơn 10 | Gõ `10.5` rồi `11` | Báo điểm phải nằm trong khoảng 0 đến 10 | |
| TC09 | Điểm biên | Gõ `0` rồi `10` | Chấp nhận cả hai giá trị | |
| TC10 | Dấu thập phân | Gõ `7.5` rồi `7,5` | Chấp nhận cả hai, hiển thị đúng 7,5 | |
| TC11 | Quá 2 chữ số thập phân | Gõ `7.555` | Báo điểm tối đa 2 chữ số thập phân | |
| TC12 | Để trống điểm khi lưu | Tải CNTT01/MH004, chỉ nhập điểm cho 3 sinh viên, bấm Lưu tất cả | Báo danh sách dòng chưa có điểm, đánh dấu lỗi từng dòng, **chưa lưu gì** (kiểm tra SSMS) | |
| TC13 | Ghi chú quá dài | Nhập ghi chú dài hơn 200 ký tự | Ô ghi chú không cho nhập quá 200 ký tự | |
| TC14 | Lưu thành công (thêm mới) | CNTT01/MH004, nhập đủ 5 điểm, bấm Lưu tất cả | Thông báo "Lưu thành công 5 dòng"; SSMS có 5 dòng mới; thống kê cập nhật | |
| TC15 | Lưu thành công (cập nhật) | CNTT01/MH001, sửa điểm SV001 từ 8,5 thành 9, nhập điểm các SV còn lại, Lưu tất cả | Điểm SV001 được **cập nhật** (không bị trùng dòng), các SV khác được thêm mới | |
| TC16 | Rollback khi một dòng lỗi | Chạy `Demo_Rollback_On.sql`; CNTT01/MH004 nhập đủ điểm, ghi chú `LOI` ở SV003, Lưu tất cả | Báo lỗi nêu sinh viên SV003; thông báo đã rollback; SSMS **không có dòng nào** của MH004 (kể cả SV001, SV002) | |
| TC17 | Lưu lại sau khi sửa lỗi | Sau TC16 xóa chữ `LOI` rồi Lưu tất cả | Lưu thành công 5 dòng | |
| TC18 | Lọc sinh viên | Gõ `Bình` rồi `SV003` vào ô Lọc SV | Lưới chỉ còn các dòng phù hợp; xóa ô lọc thì hiện đủ | |
| TC19 | Lưu khi đang lọc | Đang lọc 1 sinh viên, bấm Lưu tất cả | Chương trình bỏ lọc và kiểm tra đủ cả lớp (không bỏ sót dòng ẩn) | |
| TC20 | Thống kê | Tải CNTT01/MH001 sau khi lưu đủ điểm | Dòng thống kê hiện sĩ số, số đã có điểm, TB, cao nhất, thấp nhất, số đạt đúng với dữ liệu | |
| TC21 | Đổi lớp khi còn điểm chưa lưu | Nhập vài điểm, đổi lớp, bấm Tải danh sách | Hỏi xác nhận bỏ thay đổi; chọn Không thì giữ nguyên dữ liệu đang nhập | |
| TC22 | Đóng Form khi còn điểm chưa lưu | Nhập vài điểm rồi đóng Form | Hỏi xác nhận trước khi đóng | |
| TC23 | Đổi ComboBox sau khi tải | Tải lớp CNTT01/MH004, đổi `cboMonHoc` sang MH002 (không bấm Tải), bấm Lưu tất cả | Điểm vẫn được lưu đúng cho **MH004** (môn đã tải), không bị lưu nhầm sang MH002 | |
| TC24 | Mất kết nối | Dừng dịch vụ SQL Server rồi bấm Tải danh sách | Hiện thông báo lỗi kết nối, chương trình không bị crash | |
