/* =====================================================================
   COMP1019 - Lab 10: EduManagerDB
   File 02: Nhập dữ liệu mẫu (chạy sau file 01)
   Thứ tự nhập: KHOA -> LOP -> SINHVIEN -> MONHOC -> KETQUA -> TAIKHOAN
   (bảng cha trước, bảng con sau để không vi phạm khóa ngoại)
   ===================================================================== */

USE EduManagerDB;
GO

-- 2 khoa
INSERT INTO KHOA (MaKhoa, TenKhoa) VALUES
('CNTT', N'Công nghệ thông tin'),
('KT',   N'Kinh tế');

-- 3 lớp
INSERT INTO LOP (MaLop, TenLop, MaKhoa) VALUES
('22CTT1', N'Công nghệ thông tin 22A', 'CNTT'),
('22CTT2', N'Công nghệ thông tin 22B', 'CNTT'),
('22KT1',  N'Kinh tế 22A',             'KT');

-- 7 sinh viên (SV007 cố ý chưa có điểm để dùng cho truy vấn xóa)
INSERT INTO SINHVIEN (MaSV, HoTen, GioiTinh, NgaySinh, Email, MaLop) VALUES
('SV001', N'Nguyễn Văn An',   N'Nam', '20040115', 'an.nv@example.com',   '22CTT1'),
('SV002', N'Trần Thị Bình',   N'Nữ',  '20040302', 'binh.tt@example.com', '22CTT1'),
('SV003', N'Lê Hoàng Cường',  N'Nam', '20030927', 'cuong.lh@example.com','22CTT1'),
('SV004', N'Phạm Thị Dung',   N'Nữ',  '20040721', 'dung.pt@example.com', '22CTT2'),
('SV005', N'Võ Minh Em',      N'Nam', '20041105', 'em.vm@example.com',   '22CTT2'),
('SV006', N'Đặng Thu Hà',     N'Nữ',  '20040210', 'ha.dt@example.com',   '22KT1'),
('SV007', N'Bùi Quốc Khánh',  N'Nam', '20040630', 'khanh.bq@example.com','22KT1');

-- 4 môn học
INSERT INTO MONHOC (MaMH, TenMH, SoTinChi) VALUES
('MH001', N'Lập trình căn bản',        3),
('MH002', N'Cơ sở dữ liệu',            3),
('MH003', N'Lập trình trên Windows',   3),
('MH004', N'Kinh tế vi mô',            2);

-- 10 kết quả học tập
INSERT INTO KETQUA (MaSV, MaMH, Diem) VALUES
('SV001', 'MH001', 8.5),
('SV001', 'MH002', 9.0),
('SV001', 'MH003', 7.5),
('SV002', 'MH001', 6.5),
('SV002', 'MH002', 7.0),
('SV003', 'MH001', 9.5),
('SV003', 'MH003', 8.0),
('SV004', 'MH002', 5.5),
('SV005', 'MH004', 7.0),
('SV006', 'MH004', 8.0);

-- 2 tài khoản demo (mật khẩu được băm SHA-256)
-- admin / Admin@123    user1 / User@123
INSERT INTO TAIKHOAN (TenDangNhap, MatKhauHash, HoTen, VaiTro) VALUES
('admin', HASHBYTES('SHA2_256', 'Admin@123'), N'Quản trị viên',  N'Admin'),
('user1', HASHBYTES('SHA2_256', 'User@123'),  N'Người dùng demo', N'User');
GO

-- Kiểm tra số dòng của từng bảng
SELECT 'KHOA' AS Bang, COUNT(*) AS SoDong FROM KHOA
UNION ALL SELECT 'LOP',       COUNT(*) FROM LOP
UNION ALL SELECT 'SINHVIEN',  COUNT(*) FROM SINHVIEN
UNION ALL SELECT 'MONHOC',    COUNT(*) FROM MONHOC
UNION ALL SELECT 'KETQUA',    COUNT(*) FROM KETQUA
UNION ALL SELECT 'TAIKHOAN',  COUNT(*) FROM TAIKHOAN;
GO
