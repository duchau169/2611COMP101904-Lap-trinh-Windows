# Lab04 - Quản lý sản phẩm (Exception, Delegate/Event, Func/Action, Generic)

Chương trình Console C# quản lý sản phẩm, học phần **COMP1019 - Lập trình trên Windows**.

## 1. Cấu trúc dự án

```
Lab04_QuanLySanPham
│
├── IEntity.cs                     # Interface { string Id { get; } } - rang buoc generic
├── Product.cs                     # Id = MaSP, TenSP, Price, Quantity (khong am), ToString()
├── DuplicateProductException.cs   # Exception tu tao: ma san pham bi trung
├── ProductNotFoundException.cs    # Exception tu tao: khong tim thay san pham
├── Repository.cs                  # Generic class Repository<T> where T : IEntity
├── ProductService.cs              # Nghiep vu: AddProduct, RemoveProduct, Search, Filter + event
├── Program.cs                     # Main, menu, nhap xuat, bat exception
└── Lab04_QuanLySanPham.csproj
```

## 2. Yêu cầu kỹ thuật đã đáp ứng

- [x] Chương trình không dừng đột ngột khi nhập sai dữ liệu (validate vòng lặp + try-catch bao toàn bộ switch trong `Main`)
- [x] Có ít nhất 2 exception tự tạo: `DuplicateProductException`, `ProductNotFoundException`
- [x] Có generic class `Repository<T> where T : IEntity` với `Add`, `Remove`, `FindById`, `Find(Func<T,bool>)`, `GetAll`
- [x] Có event khi thêm và xóa sản phẩm thành công: `ProductService.ProductAdded`, `ProductService.ProductRemoved` (kiểu `Action<Product>`), `Program.cs` đăng ký lắng nghe và in thông báo `[EVENT]`
- [x] Dùng `Func<Product,bool>` trong `Search` (tìm theo tên) và `Filter` (lọc theo khoảng giá)
- [x] Không viết toàn bộ xử lý trong `Main` — tách riêng `Repository<T>` và `ProductService`
- [x] `Price`, `Quantity` là property tự kiểm tra, không cho phép giá trị âm (ném `ArgumentException`)

## 3. Luồng xử lý exception

- `Product.Price` / `Product.Quantity` set giá trị âm → `ArgumentException`
- `ProductService.AddProduct` với mã đã tồn tại → `DuplicateProductException`
- `ProductService.RemoveProduct` với mã không tồn tại → `ProductNotFoundException`
- Tất cả được bắt tập trung trong vòng lặp menu ở `Program.Main` bằng nhiều khối `catch` (cụ thể trước, `Exception` chung ở cuối để đảm bảo chương trình không bao giờ crash).

## 4. Cách chạy chương trình

1. Cài .NET SDK 8.0 trở lên.
2. Mở thư mục dự án bằng Visual Studio (**File → Open → Folder**) hoặc chạy trong terminal:
   ```
   cd Lab04_QuanLySanPham
   dotnet run
   ```
3. Nếu tiếng Việt hiển thị lỗi dấu trên console (do code page Windows chưa phải UTF-8), chạy `chcp 65001` trước khi `dotnet run`, và chọn font hỗ trợ Unicode (Consolas...) cho cửa sổ console/terminal.

## 5. Gợi ý kiểm thử

| STT | Tình huống | Kết quả cần kiểm tra |
|---|---|---|
| 1 | Thêm SP001, giá 10000, SL 5 | Thêm thành công, in thông báo `[EVENT]` |
| 2 | Thêm lại mã SP001 | Báo lỗi `DuplicateProductException`, không crash |
| 3 | Nhập giá hoặc số lượng âm | Báo lỗi, yêu cầu nhập lại |
| 4 | Xóa mã không tồn tại | Báo lỗi `ProductNotFoundException`, không crash |
| 5 | Xóa mã tồn tại | Xóa thành công, in thông báo `[EVENT]` |
| 6 | Lọc theo khoảng giá | Chỉ in sản phẩm có giá trong khoảng, dùng `Func<Product,bool>` |
| 7 | Tính tổng giá trị kho | Tổng = Σ (đơn giá × số lượng) của tất cả sản phẩm |

## 6. Kết quả kiểm thử (Test Results)

Các ảnh chụp màn hình dưới đây minh chứng chương trình chạy đúng theo các tình huống kiểm thử ở mục 5, bao gồm cả luồng hợp lệ lẫn các luồng lỗi/exception.

### 6.1. Thêm sản phẩm - thành công
Thêm SP002 - Sữa bò, giá 20.000, SL 10 → thêm thành công, phát event `[EVENT] Da them san pham '002 - Sua bo' vao kho.`

![Them san pham thanh cong](screenshots/01_them_thanh_cong.png)

### 6.2. Xuất danh sách
In toàn bộ danh sách sản phẩm hiện có, đầy đủ mã, tên, giá, số lượng và thành tiền.

![Xuat danh sach](screenshots/02_xuat_danh_sach.png)

### 6.3. Tìm sản phẩm theo mã
Nhập mã `001` → tìm thấy và in đúng thông tin sản phẩm tương ứng.

![Tim theo ma](screenshots/03_tim_theo_ma.png)

### 6.4. Tìm sản phẩm theo tên
Nhập từ khóa `bo` → in ra sản phẩm có tên chứa từ khóa (dùng `Func<Product,bool>` trong `Search`).

![Tim theo ten](screenshots/04_tim_theo_ten.png)

### 6.5. Lọc theo khoảng giá
Nhập khoảng giá 3.000 - 20.000 → chỉ in các sản phẩm có giá trong khoảng, dùng `Func<Product,bool>` trong `Filter`.

![Loc theo khoang gia](screenshots/05_loc_theo_khoang_gia.png)

### 6.6. Xóa sản phẩm - thành công
Xóa mã `002` đang tồn tại → xóa thành công, phát event `[EVENT] Da xoa san pham '002 - Sua bo' khoi kho.`

![Xoa san pham thanh cong](screenshots/06_xoa_thanh_cong.png)

### 6.7. Tính tổng giá trị kho
Tổng giá trị kho = Σ (đơn giá × số lượng) của tất cả sản phẩm hiện có, ra đúng kết quả `88.000`.

![Tinh tong gia tri kho](screenshots/07_tinh_tong_gia_tri_kho.png)

### 6.8. Chọn menu không hợp lệ
Nhập `8` (ngoài khoảng 0-7) → chương trình báo "Lua chon khong hop le. Vui long chon lai.", không crash.

![Menu lua chon khong hop le](screenshots/08_menu_lua_chon_khong_hop_le.png)

### 6.9. Thêm sản phẩm - đơn giá không phải số
Nhập đơn giá `abc` → chương trình báo lỗi "Don gia phai la mot so." và yêu cầu nhập lại, không crash.

![Them don gia khong phai so](screenshots/09_them_don_gia_khong_phai_so.png)

### 6.10. Tìm theo mã - mã không tồn tại
Nhập mã `002` (đã bị xóa trước đó) → chương trình báo "Khong tim thay san pham co ma nay.", không crash.

![Tim theo ma khong ton tai](screenshots/10_tim_theo_ma_khong_ton_tai.png)

### 6.11. Thêm sản phẩm - đơn giá âm
Nhập đơn giá `-1000` → chương trình báo "Don gia khong duoc am." và yêu cầu nhập lại.

![Them don gia am](screenshots/11_them_don_gia_am.png)

### 6.12. Xóa sản phẩm - mã không tồn tại (exception)
Nhập mã `007` không có trong kho → `ProductNotFoundException` được ném và bắt đúng, in "Loi: Khong tim thay san pham co ma '007'.", chương trình không dừng đột ngột.

![Xoa ma khong ton tai exception](screenshots/12_xoa_ma_khong_ton_tai_exception.png)

### 6.13. Thoát chương trình
Chọn `0` → chương trình kết thúc bình thường, không lỗi.

![Thoat chuong trinh](screenshots/13_thoat_chuong_trinh.png)
