/* =====================================================================
   COMP1019 - Lab 10: EduManagerDB
   File 01: Tạo database, bảng, khóa chính, khóa ngoại, ràng buộc
   Chạy file này đầu tiên (bằng SSMS, kết nối vào SQL Server).
   ===================================================================== */

USE master;
GO

-- Xóa database cũ (nếu có) để chạy lại từ đầu; đóng mọi kết nối đang mở
IF DB_ID(N'EduManagerDB') IS NOT NULL
BEGIN
    ALTER DATABASE EduManagerDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EduManagerDB;
END
GO

CREATE DATABASE EduManagerDB;
GO

USE EduManagerDB;
GO

/* ---------------------------------------------------------------------
   Bảng KHOA: thông tin khoa
   --------------------------------------------------------------------- */
CREATE TABLE KHOA
(
    MaKhoa   VARCHAR(10)   NOT NULL,
    TenKhoa  NVARCHAR(100) NOT NULL,
    CONSTRAINT PK_KHOA PRIMARY KEY (MaKhoa),
    CONSTRAINT UQ_KHOA_TenKhoa UNIQUE (TenKhoa)
);
GO

/* ---------------------------------------------------------------------
   Bảng LOP: mỗi lớp thuộc một khoa
   --------------------------------------------------------------------- */
CREATE TABLE LOP
(
    MaLop    VARCHAR(10)   NOT NULL,
    TenLop   NVARCHAR(100) NOT NULL,
    MaKhoa   VARCHAR(10)   NOT NULL,
    CONSTRAINT PK_LOP PRIMARY KEY (MaLop),
    CONSTRAINT FK_LOP_KHOA FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);
GO

/* ---------------------------------------------------------------------
   Bảng SINHVIEN: mỗi sinh viên thuộc một lớp
   Giới tính chỉ nhận 'Nam' hoặc 'Nữ'
   --------------------------------------------------------------------- */
CREATE TABLE SINHVIEN
(
    MaSV      VARCHAR(10)   NOT NULL,
    HoTen     NVARCHAR(100) NOT NULL,
    GioiTinh  NVARCHAR(3)   NOT NULL,
    NgaySinh  DATE          NULL,
    Email     VARCHAR(100)  NULL,
    MaLop     VARCHAR(10)   NOT NULL,
    CONSTRAINT PK_SINHVIEN PRIMARY KEY (MaSV),
    CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) REFERENCES LOP(MaLop),
    CONSTRAINT CK_SINHVIEN_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ'))
);
GO

/* ---------------------------------------------------------------------
   Bảng MONHOC: số tín chỉ phải > 0
   --------------------------------------------------------------------- */
CREATE TABLE MONHOC
(
    MaMH      VARCHAR(10)   NOT NULL,
    TenMH     NVARCHAR(100) NOT NULL,
    SoTinChi  INT           NOT NULL,
    CONSTRAINT PK_MONHOC PRIMARY KEY (MaMH),
    CONSTRAINT CK_MONHOC_SoTinChi CHECK (SoTinChi > 0)
);
GO

/* ---------------------------------------------------------------------
   Bảng KETQUA: điểm của sinh viên theo môn
   Khóa chính kép (MaSV, MaMH); điểm trong khoảng 0..10
   --------------------------------------------------------------------- */
CREATE TABLE KETQUA
(
    MaSV  VARCHAR(10)  NOT NULL,
    MaMH  VARCHAR(10)  NOT NULL,
    Diem  DECIMAL(4,2) NOT NULL,
    CONSTRAINT PK_KETQUA PRIMARY KEY (MaSV, MaMH),
    CONSTRAINT FK_KETQUA_SINHVIEN FOREIGN KEY (MaSV) REFERENCES SINHVIEN(MaSV),
    CONSTRAINT FK_KETQUA_MONHOC   FOREIGN KEY (MaMH) REFERENCES MONHOC(MaMH),
    CONSTRAINT CK_KETQUA_Diem CHECK (Diem >= 0 AND Diem <= 10)
);
GO

/* ---------------------------------------------------------------------
   Bảng TAIKHOAN: tài khoản đăng nhập demo
   Mật khẩu lưu dạng băm SHA-256 (không lưu mật khẩu thô)
   --------------------------------------------------------------------- */
CREATE TABLE TAIKHOAN
(
    TenDangNhap  VARCHAR(50)   NOT NULL,
    MatKhauHash  VARBINARY(32) NOT NULL,
    HoTen        NVARCHAR(100) NOT NULL,
    VaiTro       NVARCHAR(20)  NOT NULL CONSTRAINT DF_TAIKHOAN_VaiTro DEFAULT N'User',
    CONSTRAINT PK_TAIKHOAN PRIMARY KEY (TenDangNhap),
    CONSTRAINT CK_TAIKHOAN_VaiTro CHECK (VaiTro IN (N'Admin', N'User'))
);
GO

-- Kiểm tra nhanh: liệt kê các bảng vừa tạo
SELECT name AS TenBang FROM sys.tables ORDER BY name;
GO
