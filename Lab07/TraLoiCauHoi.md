# COMP1019 – Lab 07: Trả lời câu hỏi

**1. SDI và MDI khác nhau như thế nào?**
- **SDI (Single Document Interface):** mỗi cửa sổ là một Form độc lập, không có cửa sổ cha; mỗi tài liệu/chức năng nằm trong cửa sổ riêng (ví dụ Notepad, Paint).
- **MDI (Multiple Document Interface):** có một Form cha (`IsMdiContainer = true`) chứa nhiều Form con bên trong vùng làm việc của nó. Form con thu nhỏ/phóng to/di chuyển trong Form cha, không ra ngoài; thường dùng chung menu và status bar của Form cha (ví dụ ShopManagerMini).

**2. Vì sao Form đăng nhập nên dùng `ShowDialog()`?**
`ShowDialog()` mở Form ở dạng modal: chương trình dừng tại dòng gọi cho tới khi Form đóng, và người dùng không thể thao tác Form khác. Nhờ vậy chỉ khi đăng nhập thành công (`DialogResult.OK`) mới mở được Form chính; nếu bấm Thoát (`Cancel`) thì kết thúc chương trình. Ngoài ra kết quả trả về (`DialogResult`) giúp code bên ngoài biết đăng nhập thành công hay không.

**3. Dữ liệu user được truyền từ FrmLogin sang FrmMain bằng cách nào?**
Sau khi kiểm tra đúng tài khoản, `FrmLogin` lưu đối tượng `User` vào property `LoggedInUser` rồi đặt `DialogResult = OK`. Trong `Program.cs`, sau `ShowDialog()` lấy `login.LoggedInUser` và truyền vào **constructor** của `FrmMain(user)`. `FrmMain` giữ trong biến `_currentUser` và hiển thị tên, vai trò lên StatusStrip.

**4. Nếu bấm menu sản phẩm nhiều lần, chương trình xử lý thế nào để không mở trùng Form?**
Hàm `OpenChild<T>()` duyệt `MdiChildren`; nếu đã có Form kiểu `T` (`child is T`) thì đưa về trạng thái bình thường (nếu đang thu nhỏ) và gọi `Activate()` rồi `return`. Chỉ khi chưa có mới tạo Form mới, gán `MdiParent = this` và `Show()`.

**5. Khi nào dùng constructor, khi nào dùng property để truyền dữ liệu giữa các Form?**
- **Constructor:** dữ liệu bắt buộc, Form không thể hoạt động thiếu nó và chỉ cần truyền một lần khi tạo (ví dụ `FrmMain(User user)`).
- **Property:** dữ liệu tùy chọn hoặc cần đọc/ghi sau khi Form đã tạo/đóng, hoặc truyền ngược từ Form con về Form cha (ví dụ `FrmLogin.LoggedInUser`). Cũng phù hợp khi Form được tạo bằng `new T()` (generic, như `OpenChild<T>`) nên không có constructor có tham số.
