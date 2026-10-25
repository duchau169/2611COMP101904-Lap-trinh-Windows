/* ============================================================
   DEMO ROLLBACK - TẮT (xóa ràng buộc giả)
   ============================================================ */
USE QLSinhVienLab12;
GO

IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Demo_Rollback')
    ALTER TABLE KETQUA DROP CONSTRAINT CK_Demo_Rollback;
GO
