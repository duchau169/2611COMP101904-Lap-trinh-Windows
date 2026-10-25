/* ============================================================
   COMP1019 - Lab 12: Script tạo database QLSinhVienLab12
   Gồm: bảng, ràng buộc, dữ liệu mẫu, stored procedure.
   Chạy nhiều lần vẫn an toàn (không tạo trùng, không lỗi).
   Mở bằng SSMS, bấm Execute (F5).
   ============================================================ */
IF DB_ID('QLSinhVienLab12') IS NULL
    CREATE DATABASE QLSinhVienLab12;
GO

USE QLSinhVienLab12;
GO

/* ---------------- BẢNG ---------------- */
IF OBJECT_ID('dbo.LOP', 'U') IS NULL
CREATE TABLE LOP (
    MaLop  VARCHAR(20)   NOT NULL,
    TenLop NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_LOP PRIMARY KEY (MaLop)
);
GO

IF OBJECT_ID('dbo.MONHOC', 'U') IS NULL
CREATE TABLE MONHOC (
    MaMon    VARCHAR(20)   NOT NULL,
    TenMon   NVARCHAR(100) NOT NULL,
    SoTinChi INT           NOT NULL,
    CONSTRAINT PK_MONHOC PRIMARY KEY (MaMon),
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi > 0)
);
GO

IF OBJECT_ID('dbo.SINHVIEN', 'U') IS NULL
CREATE TABLE SINHVIEN (
    MaSV     VARCHAR(20)   NOT NULL,
    HoTen    NVARCHAR(100) NOT NULL,
    NgaySinh DATE          NULL,
    MaLop    VARCHAR(20)   NOT NULL,
    CONSTRAINT PK_SINHVIEN PRIMARY KEY (MaSV),
    CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) REFERENCES LOP(MaLop)
);
GO

IF OBJECT_ID('dbo.KETQUA', 'U') IS NULL
CREATE TABLE KETQUA (
    MaSV   VARCHAR(20)   NOT NULL,
    MaMon  VARCHAR(20)   NOT NULL,
    Diem   DECIMAL(4,2)  NOT NULL,
    GhiChu NVARCHAR(200) NULL,
    CONSTRAINT PK_KETQUA PRIMARY KEY (MaSV, MaMon),
    CONSTRAINT FK_KETQUA_SINHVIEN FOREIGN KEY (MaSV)  REFERENCES SINHVIEN(MaSV),
    CONSTRAINT FK_KETQUA_MONHOC   FOREIGN KEY (MaMon) REFERENCES MONHOC(MaMon),
    CONSTRAINT CK_KETQUA_Diem CHECK (Diem >= 0 AND Diem <= 10)
);
GO

/* ---------------- DỮ LIỆU MẪU (chỉ thêm nếu chưa có) ---------------- */
INSERT INTO LOP (MaLop, TenLop)
SELECT v.MaLop, v.TenLop
FROM (VALUES
    ('CNTT01',  N'Công nghệ thông tin 01'),
    ('CNTT02',  N'Công nghệ thông tin 02'),
    ('SPTIN01', N'Sư phạm Tin học 01')
) AS v(MaLop, TenLop)
WHERE NOT EXISTS (SELECT 1 FROM LOP l WHERE l.MaLop = v.MaLop);
GO

INSERT INTO MONHOC (MaMon, TenMon, SoTinChi)
SELECT v.MaMon, v.TenMon, v.SoTinChi
FROM (VALUES
    ('MH001', N'Lập trình trên Windows',    3),
    ('MH002', N'Cơ sở dữ liệu',             3),
    ('MH003', N'Lập trình hướng đối tượng', 3),
    ('MH004', N'Cấu trúc dữ liệu',          4)
) AS v(MaMon, TenMon, SoTinChi)
WHERE NOT EXISTS (SELECT 1 FROM MONHOC m WHERE m.MaMon = v.MaMon);
GO

INSERT INTO SINHVIEN (MaSV, HoTen, NgaySinh, MaLop)
SELECT v.MaSV, v.HoTen, v.NgaySinh, v.MaLop
FROM (VALUES
    ('SV001', N'Nguyễn Văn An',     CAST('2004-01-15' AS DATE), 'CNTT01'),
    ('SV002', N'Trần Thị Bình',     CAST('2004-03-02' AS DATE), 'CNTT01'),
    ('SV003', N'Lê Hoàng Cường',    CAST('2003-09-27' AS DATE), 'CNTT01'),
    ('SV004', N'Phạm Thị Dung',     CAST('2004-07-21' AS DATE), 'CNTT01'),
    ('SV005', N'Võ Minh Em',        CAST('2004-11-05' AS DATE), 'CNTT01'),
    ('SV006', N'Đặng Thu Hà',       CAST('2004-02-10' AS DATE), 'CNTT02'),
    ('SV007', N'Huỳnh Gia Huy',     CAST('2003-12-30' AS DATE), 'CNTT02'),
    ('SV008', N'Ngô Thanh Khoa',    CAST('2004-05-18' AS DATE), 'CNTT02'),
    ('SV009', N'Bùi Mai Lan',       CAST('2004-08-09' AS DATE), 'CNTT02'),
    ('SV010', N'Phan Quốc Minh',    CAST('2004-04-12' AS DATE), 'SPTIN01'),
    ('SV011', N'Đỗ Ngọc Nhi',       CAST('2004-06-25' AS DATE), 'SPTIN01'),
    ('SV012', N'Lý Thanh Phong',    CAST('2003-10-03' AS DATE), 'SPTIN01')
) AS v(MaSV, HoTen, NgaySinh, MaLop)
WHERE NOT EXISTS (SELECT 1 FROM SINHVIEN s WHERE s.MaSV = v.MaSV);
GO

/* Một vài điểm đã có sẵn để thấy "kèm điểm nếu đã có" */
INSERT INTO KETQUA (MaSV, MaMon, Diem, GhiChu)
SELECT v.MaSV, v.MaMon, v.Diem, v.GhiChu
FROM (VALUES
    ('SV001', 'MH001', CAST(8.50 AS DECIMAL(4,2)), N'Làm bài tốt'),
    ('SV002', 'MH001', CAST(7.00 AS DECIMAL(4,2)), NULL),
    ('SV001', 'MH002', CAST(9.00 AS DECIMAL(4,2)), NULL),
    ('SV006', 'MH001', CAST(6.50 AS DECIMAL(4,2)), N'Cần cố gắng thêm')
) AS v(MaSV, MaMon, Diem, GhiChu)
WHERE NOT EXISTS (SELECT 1 FROM KETQUA k WHERE k.MaSV = v.MaSV AND k.MaMon = v.MaMon);
GO

/* ---------------- STORED PROCEDURE ---------------- */

/* Lấy danh sách lớp */
CREATE OR ALTER PROCEDURE sp_Lop_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaLop, TenLop FROM LOP ORDER BY MaLop;
END
GO

/* Lấy danh sách môn học */
CREATE OR ALTER PROCEDURE sp_MonHoc_GetAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MaMon, TenMon, SoTinChi FROM MONHOC ORDER BY MaMon;
END
GO

/* Lấy danh sách sinh viên của một lớp, kèm điểm môn học nếu đã có (chưa có thì Diem = NULL) */
CREATE OR ALTER PROCEDURE sp_KetQua_GetByLopMon
    @MaLop VARCHAR(20),
    @MaMon VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT sv.MaSV, sv.HoTen, kq.Diem, kq.GhiChu
    FROM SINHVIEN sv
    LEFT JOIN KETQUA kq ON kq.MaSV = sv.MaSV AND kq.MaMon = @MaMon
    WHERE sv.MaLop = @MaLop
    ORDER BY sv.MaSV;
END
GO

/* Lưu điểm: đã có thì cập nhật, chưa có thì thêm mới */
CREATE OR ALTER PROCEDURE sp_KetQua_Save
    @MaSV   VARCHAR(20),
    @MaMon  VARCHAR(20),
    @Diem   DECIMAL(4,2),
    @GhiChu NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Diem IS NULL OR @Diem < 0 OR @Diem > 10
        THROW 50001, N'Điểm phải nằm trong khoảng từ 0 đến 10.', 1;

    UPDATE KETQUA
    SET Diem = @Diem, GhiChu = @GhiChu
    WHERE MaSV = @MaSV AND MaMon = @MaMon;

    IF @@ROWCOUNT = 0
        INSERT INTO KETQUA (MaSV, MaMon, Diem, GhiChu)
        VALUES (@MaSV, @MaMon, @Diem, @GhiChu);
END
GO

/* Thống kê điểm một môn của một lớp (mở rộng) */
CREATE OR ALTER PROCEDURE sp_KetQua_ThongKe
    @MaLop VARCHAR(20),
    @MaMon VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*)                                      AS SoSinhVien,
           COUNT(kq.Diem)                                AS SoCoDiem,
           CAST(AVG(CAST(kq.Diem AS DECIMAL(5,2))) AS DECIMAL(4,2)) AS DiemTB,
           MAX(kq.Diem)                                  AS DiemMax,
           MIN(kq.Diem)                                  AS DiemMin,
           SUM(CASE WHEN kq.Diem >= 5 THEN 1 ELSE 0 END) AS SoDat
    FROM SINHVIEN sv
    LEFT JOIN KETQUA kq ON kq.MaSV = sv.MaSV AND kq.MaMon = @MaMon
    WHERE sv.MaLop = @MaLop;
END
GO

/* Kiểm tra nhanh */
SELECT * FROM LOP;
SELECT * FROM MONHOC;
SELECT * FROM SINHVIEN;
SELECT * FROM KETQUA;
GO
