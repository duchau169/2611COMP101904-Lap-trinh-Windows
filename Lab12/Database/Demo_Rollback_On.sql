/* ============================================================
   DEMO ROLLBACK - BẬT
   Thêm một ràng buộc "giả" khiến database từ chối ghi chú đúng bằng chữ LOI.
   Cách demo: tải danh sách, nhập điểm hợp lệ cho mọi dòng, gõ ghi chú LOI
   vào một dòng ở giữa, bấm Lưu tất cả => chương trình báo lỗi và rollback:
   các dòng đứng trước dòng lỗi cũng KHÔNG được lưu.
   Demo xong nhớ chạy Demo_Rollback_Off.sql.
   ============================================================ */
USE QLSinhVienLab12;
GO

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Demo_Rollback')
    ALTER TABLE KETQUA ADD CONSTRAINT CK_Demo_Rollback CHECK (GhiChu IS NULL OR GhiChu <> N'LOI');
GO
