/* ============================================================================
   COMP1019 - Lập trình trên Windows | Lab 12: Data Binding, Stored Procedure, Transaction
   Script tạo database QLSinhVienLab12: bảng, ràng buộc, 4 stored procedure, dữ liệu mẫu

   Cách chạy: mở file trong SSMS, bấm Execute (F5), KHÔNG bôi đen gì.
   Chạy lại nhiều lần được: mỗi lần chạy sẽ xóa database QLSinhVienLab12 cũ rồi tạo lại.
   (Database này độc lập, KHÔNG đụng đến EduManagerDB của các buổi 10, 11.)
   ============================================================================ */


/* ----------------------------------------------------------------------------
   PHẦN 1. TẠO DATABASE
   ---------------------------------------------------------------------------- */
USE master;
GO

IF DB_ID(N'QLSinhVienLab12') IS NOT NULL
BEGIN
    -- Đóng mọi kết nối đang mở (kể cả chương trình WinForms) rồi mới xóa được.
    ALTER DATABASE QLSinhVienLab12 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLSinhVienLab12;
END
GO

CREATE DATABASE QLSinhVienLab12 COLLATE Vietnamese_CI_AS;
GO

USE QLSinhVienLab12;
GO


/* ----------------------------------------------------------------------------
   PHẦN 2. TẠO 4 BẢNG (bảng cha trước, bảng con sau)
   ---------------------------------------------------------------------------- */
CREATE TABLE LOP (
    MaLop   NVARCHAR(20)  NOT NULL,
    TenLop  NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_LOP PRIMARY KEY (MaLop)
);
GO

CREATE TABLE SINHVIEN (
    MaSV     NVARCHAR(20)  NOT NULL,
    HoTen    NVARCHAR(100) NOT NULL,
    NgaySinh DATE          NULL,
    MaLop    NVARCHAR(20)  NOT NULL,
    CONSTRAINT PK_SINHVIEN PRIMARY KEY (MaSV),
    CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) REFERENCES LOP (MaLop)
);
GO

CREATE TABLE MONHOC (
    MaMon    NVARCHAR(20)  NOT NULL,
    TenMon   NVARCHAR(100) NOT NULL,
    SoTinChi INT           NOT NULL,
    CONSTRAINT PK_MONHOC PRIMARY KEY (MaMon),
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi > 0)
);
GO

-- KETQUA: khóa chính ghép (MaSV, MaMon) -> mỗi sinh viên chỉ có MỘT điểm cho mỗi môn.
CREATE TABLE KETQUA (
    MaSV   NVARCHAR(20)  NOT NULL,
    MaMon  NVARCHAR(20)  NOT NULL,
    Diem   FLOAT         NOT NULL,
    GhiChu NVARCHAR(255) NULL,
    CONSTRAINT PK_KETQUA PRIMARY KEY (MaSV, MaMon),
    CONSTRAINT CK_KETQUA_Diem CHECK (Diem >= 0 AND Diem <= 10),   -- yêu cầu đề: điểm từ 0 đến 10
    CONSTRAINT FK_KETQUA_SINHVIEN FOREIGN KEY (MaSV)  REFERENCES SINHVIEN (MaSV),
    CONSTRAINT FK_KETQUA_MONHOC   FOREIGN KEY (MaMon) REFERENCES MONHOC (MaMon)
);
GO

-- Chỉ mục cho cột khóa ngoại thường dùng để lọc (SQL Server không tự tạo chỉ mục cho khóa ngoại)
CREATE INDEX IX_SINHVIEN_MaLop ON SINHVIEN (MaLop);
CREATE INDEX IX_KETQUA_MaMon   ON KETQUA (MaMon);
GO


/* ----------------------------------------------------------------------------
   PHẦN 3. DỮ LIỆU MẪU
   2 lớp, 8 sinh viên (mỗi lớp 4), 3 môn học, 4 kết quả có sẵn.
   Có chủ ý để lớp CNTTA môn COMP1019 đã có điểm của SV001, SV002 nhưng CHƯA có
   của SV003, SV007 -> thấy rõ LEFT JOIN và ô điểm trống khi tải danh sách.
   ---------------------------------------------------------------------------- */
INSERT INTO LOP (MaLop, TenLop) VALUES
(N'CNTTA', N'Công nghệ thông tin A'),
(N'CNTTB', N'Công nghệ thông tin B');

INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, MaLop) VALUES
(N'SV001', N'Nguyễn Văn An',   '2007-01-15', N'CNTTA'),
(N'SV002', N'Trần Thị Bình',   '2007-05-20', N'CNTTA'),
(N'SV003', N'Lê Minh Cường',   '2007-03-11', N'CNTTA'),
(N'SV007', N'Đặng Thu Trang',  '2007-06-18', N'CNTTA'),
(N'SV004', N'Phạm Thanh Dung', '2007-07-09', N'CNTTB'),
(N'SV005', N'Hoàng Gia Hân',   '2007-10-30', N'CNTTB'),
(N'SV006', N'Võ Thành Khang',  '2007-09-09', N'CNTTB'),
(N'SV008', N'Bùi Ngọc Lan',    '2007-03-25', N'CNTTB');

INSERT INTO MONHOC (MaMon, TenMon, SoTinChi) VALUES
(N'COMP1019', N'Lập trình trên Windows', 3),
(N'COMP1017', N'Cấu trúc dữ liệu',        3),
(N'COMP1020', N'Cơ sở dữ liệu',          3);

INSERT INTO KETQUA (MaSV, MaMon, Diem, GhiChu) VALUES
(N'SV001', N'COMP1019', 8.5, N'Làm bài tốt'),
(N'SV002', N'COMP1019', 7.0, NULL),
(N'SV001', N'COMP1017', 9.0, NULL),
(N'SV005', N'COMP1019', 6.5, N'Cần cố gắng thêm');
GO


/* ----------------------------------------------------------------------------
   PHẦN 4. 4 STORED PROCEDURE
   CREATE OR ALTER PROCEDURE phải là câu lệnh đầu tiên của batch nên cần GO giữa các procedure.
   ---------------------------------------------------------------------------- */

-- 4.1 Danh sách lớp (đổ vào cboLop)
CREATE OR ALTER PROCEDURE sp_Lop_GetAll
AS
BEGIN
    SET NOCOUNT ON;   -- không trả về thông báo "n rows affected" thừa cho ADO.NET

    SELECT MaLop, TenLop
    FROM LOP
    ORDER BY TenLop;
END;
GO

-- 4.2 Danh sách môn học (đổ vào cboMonHoc)
CREATE OR ALTER PROCEDURE sp_MonHoc_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaMon, TenMon, SoTinChi
    FROM MONHOC
    ORDER BY TenMon;
END;
GO

-- 4.3 Sinh viên của một lớp kèm điểm của một môn (nếu đã có)
--     LEFT JOIN: sinh viên chưa có điểm vẫn xuất hiện, Diem = NULL (hiển thị ô trống để nhập).
--     Điều kiện kq.MaMon = @MaMon đặt ở ON (không phải WHERE) để không làm mất sinh viên chưa có điểm.
CREATE OR ALTER PROCEDURE sp_KetQua_GetByLopMon
    @MaLop NVARCHAR(20),
    @MaMon NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT sv.MaSV,
           sv.HoTen,
           CAST(kq.Diem AS FLOAT)   AS Diem,
           ISNULL(kq.GhiChu, N'')   AS GhiChu
    FROM SINHVIEN sv
    LEFT JOIN KETQUA kq
           ON sv.MaSV = kq.MaSV AND kq.MaMon = @MaMon
    WHERE sv.MaLop = @MaLop
    ORDER BY sv.MaSV;
END;
GO

-- 4.4 Lưu điểm (upsert): đã có thì UPDATE, chưa có thì INSERT.
--     Tự kiểm tra điểm để báo lỗi rõ ràng bằng THROW (ràng buộc CHECK của bảng vẫn là chốt chặn cuối).
--     Khi chạy trong transaction của chương trình, lỗi ở đây làm cả transaction bị Rollback.
CREATE OR ALTER PROCEDURE sp_KetQua_Save
    @MaSV   NVARCHAR(20),
    @MaMon  NVARCHAR(20),
    @Diem   FLOAT,
    @GhiChu NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Diem IS NULL
        THROW 50001, N'Điểm không được để trống.', 1;

    IF @Diem < 0 OR @Diem > 10
        THROW 50002, N'Điểm phải nằm trong khoảng từ 0 đến 10.', 1;

    UPDATE KETQUA
    SET Diem = @Diem,
        GhiChu = @GhiChu
    WHERE MaSV = @MaSV AND MaMon = @MaMon;

    IF @@ROWCOUNT = 0          -- chưa có dòng nào để cập nhật -> thêm mới
        INSERT INTO KETQUA (MaSV, MaMon, Diem, GhiChu)
        VALUES (@MaSV, @MaMon, @Diem, @GhiChu);
END;
GO


/* ----------------------------------------------------------------------------
   PHẦN 5. KIỂM TRA NHANH (chụp ảnh kết quả phần này cho báo cáo)
   ---------------------------------------------------------------------------- */

-- 5.1 Danh sách lớp và môn học (2 lớp, 3 môn)
EXEC sp_Lop_GetAll;
EXEC sp_MonHoc_GetAll;

-- 5.2 Lớp CNTTA, môn COMP1019: 4 sinh viên; SV001 = 8.5, SV002 = 7 đã có điểm,
--     SV003 và SV007 chưa có điểm (Diem = NULL)
EXEC sp_KetQua_GetByLopMon @MaLop = N'CNTTA', @MaMon = N'COMP1019';
GO


/* ----------------------------------------------------------------------------
   PHẦN 6. MINH HỌA TRANSACTION BẰNG T-SQL
   Lưu 2 dòng trong cùng một transaction: dòng 1 hợp lệ, dòng 2 điểm 15 (sai).
   Dòng 2 lỗi -> CATCH -> ROLLBACK -> dòng 1 CŨNG KHÔNG được lưu (kết quả cuối: không có dòng nào).
   Tương ứng đúng với hàm KetQuaDAL.SaveAll trong chương trình C#.
   ---------------------------------------------------------------------------- */
BEGIN TRY
    BEGIN TRANSACTION;
        EXEC sp_KetQua_Save @MaSV = N'SV003', @MaMon = N'COMP1019', @Diem = 9,  @GhiChu = N'Dòng hợp lệ';
        EXEC sp_KetQua_Save @MaSV = N'SV007', @MaMon = N'COMP1019', @Diem = 15, @GhiChu = N'Dòng sai điểm';
    COMMIT TRANSACTION;
    PRINT N'Đã commit (không đúng kỳ vọng).';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    PRINT CONCAT(N'Đã ROLLBACK toàn bộ. Lỗi ', ERROR_NUMBER(), N': ', ERROR_MESSAGE());
END CATCH;

-- Kết quả đúng: KHÔNG có dòng nào (cả SV003 lẫn SV007 đều chưa có điểm COMP1019)
SELECT MaSV, MaMon, Diem, GhiChu
FROM KETQUA
WHERE MaMon = N'COMP1019' AND MaSV IN (N'SV003', N'SV007');
GO
