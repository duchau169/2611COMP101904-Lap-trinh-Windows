# Test case – Lab 09: Thẻ sinh viên

Cách làm: chạy chương trình, thực hiện từng test case, ghi kết quả thực tế vào cột cuối (Đạt / Không đạt, kèm ghi chú nếu có).

| STT | Chức năng | Bước thực hiện / Dữ liệu | Kết quả mong đợi | Kết quả thực tế |
|-----|-----------|--------------------------|------------------|-----------------|
| TC01 | Chọn ảnh JPG | Bấm *Chọn ảnh...*, chọn file `.jpg` | Ảnh hiện trong PictureBox theo chế độ Zoom, đúng tỉ lệ; preview thẻ có ảnh; hiện tên file và kích thước ảnh | |
| TC02 | Chọn ảnh PNG và BMP | Lần lượt chọn file `.png`, `.bmp` | Cả hai hiển thị bình thường, không lỗi | |
| TC03 | Ảnh không bị méo | Chọn ảnh có tỉ lệ rất khác 3x4 (ảnh ngang rộng, ảnh dọc hẹp) | Ảnh trong PictureBox và trên thẻ giữ đúng tỉ lệ, có viền trắng hai bên, không bị kéo giãn | |
| TC04 | Lọc loại file | Mở hộp thoại chọn ảnh | Chỉ hiện các file JPG, JPEG, PNG, BMP; có thể chuyển từng loại trong danh sách Filter | |
| TC05 | File không phải ảnh | Đổi tên file `.txt` thành `.jpg` rồi chọn | Hiện thông báo lỗi "Không đọc được ảnh", chương trình không bị crash, ảnh cũ (nếu có) giữ nguyên | |
| TC06 | Đổi ảnh, không khóa file | Chọn ảnh A, rồi chọn ảnh B; sau đó xóa hoặc đổi tên ảnh A trong Explorer khi chương trình còn chạy | Đổi ảnh không lỗi; xóa/đổi tên ảnh A thành công (file không bị khóa) | |
| TC07 | Preview cập nhật khi nhập | Gõ mã SV, họ tên; đổi lớp, khoa, khóa, ngày sinh, giới tính | Thẻ trên Panel preview cập nhật ngay sau mỗi thay đổi | |
| TC08 | Thiếu mã sinh viên | Để trống mã SV, điền các trường khác và chọn ảnh, bấm *Xem trước in* | Hiện cảnh báo "Vui lòng nhập mã sinh viên!", dấu lỗi cạnh ô mã SV, không mở Print Preview | |
| TC09 | Thiếu họ tên | Chỉ điền mã SV và chọn ảnh, bấm *In thẻ* | Hiện cảnh báo thiếu họ tên, không mở PrintDialog | |
| TC10 | Thiếu ảnh | Điền mã SV, họ tên, không chọn ảnh, bấm *Xem trước in* | Hiện cảnh báo "Vui lòng chọn ảnh đại diện!", không mở Print Preview | |
| TC11 | Họ tên toàn khoảng trắng | Nhập họ tên là vài dấu cách | Bị coi là rỗng, hiện cảnh báo thiếu họ tên | |
| TC12 | Ngày sinh tương lai | Thử chọn ngày sinh sau hôm nay | Không chọn được ngày sau hôm nay (giới hạn MaxDate) | |
| TC13 | Xem trước in | Điền đủ dữ liệu, bấm *Xem trước in* | Mở PrintPreviewDialog; thẻ có khung, tiêu đề, ảnh bên trái, thông tin bên phải; ảnh không méo | |
| TC14 | In thẻ | Điền đủ dữ liệu, bấm *In thẻ*, chọn máy in (ví dụ Microsoft Print to PDF) | Mở PrintDialog; in thành công, bản in giống bản xem trước | |
| TC15 | Làm mới | Nhập dữ liệu, chọn ảnh, bấm *Làm mới* | Xóa ô nhập, xóa ảnh, đưa combo/ngày sinh/màu thẻ về mặc định, preview về trạng thái ban đầu | |
| TC16 | Họ tên / khoa dài | Nhập họ tên 40 ký tự, chọn khoa dài nhất | Chữ được cắt bằng dấu "…", không tràn ra ngoài khung thẻ | |
| TC17 | Màu nền thẻ (mở rộng) | Bấm *Màu nền thẻ*, chọn màu đậm | Preview đổi màu nền, chữ tự đổi sang màu sáng để dễ đọc | |
| TC18 | Lưu PNG (mở rộng) | Điền đủ dữ liệu, bấm *Lưu PNG*, lưu file | File PNG được tạo, mở ra thấy đúng thẻ đã xem trước | |
| TC19 | Thu phóng cửa sổ | Kéo thay đổi kích thước Form | Thẻ trong preview tự co giãn, luôn giữ đúng tỉ lệ và căn giữa | |
