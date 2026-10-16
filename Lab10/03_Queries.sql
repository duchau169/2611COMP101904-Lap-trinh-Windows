/* =====================================================================
   COMP1019 - Lab 10: EduManagerDB
   File 03: Các truy vấn (chạy sau file 01 và 02)
   Nên bôi đen và chạy từng câu một để chụp ảnh kết quả.
   ===================================================================== */

USE EduManagerDB;
GO

/* ---- Câu 1: Liệt kê toàn bộ sinh viên ---- */
SELECT * FROM SINHVIEN;
GO

/* ---- Câu 2: Sinh viên kèm tên lớp và tên khoa (JOIN 3 bảng) ---- */
SELECT sv.MaSV, sv.HoTen, sv.GioiTinh, l.TenLop, k.TenKhoa
FROM SINHVIEN sv
JOIN LOP  l ON sv.MaLop = l.MaLop
JOIN KHOA k ON l.MaKhoa = k.MaKhoa
ORDER BY sv.MaSV;
GO

/* ---- Câu 3: Tìm sinh viên theo một phần họ tên (LIKE) ---- */
SELECT MaSV, HoTen, MaLop
FROM SINHVIEN
WHERE HoTen LIKE N'%Thị%';          -- đổi từ khóa tùy ý
GO

/* ---- Câu 4: Sinh viên theo mã lớp ---- */
SELECT MaSV, HoTen, GioiTinh
FROM SINHVIEN
WHERE MaLop = '22CTT1';
GO

/* ---- Câu 5: Cập nhật họ tên và lớp của một sinh viên ---- */
SELECT MaSV, HoTen, MaLop FROM SINHVIEN WHERE MaSV = 'SV003';   -- trước khi sửa

UPDATE SINHVIEN
SET HoTen = N'Lê Hoàng Quốc Cường',
    MaLop = '22CTT2'
WHERE MaSV = 'SV003';

SELECT MaSV, HoTen, MaLop FROM SINHVIEN WHERE MaSV = 'SV003';   -- sau khi sửa
GO

/* ---- Câu 6: Xóa một sinh viên chưa có dữ liệu điểm ----
   Điều kiện NOT EXISTS đảm bảo không xóa sinh viên đã có điểm
   (nếu xóa sinh viên có điểm sẽ vi phạm khóa ngoại của KETQUA).      */
DELETE FROM SINHVIEN
WHERE MaSV = 'SV007'
  AND NOT EXISTS (SELECT 1 FROM KETQUA kq WHERE kq.MaSV = SINHVIEN.MaSV);

SELECT * FROM SINHVIEN;             -- SV007 đã bị xóa
GO

/* ---- Câu 7: Điểm của từng sinh viên theo từng môn ---- */
SELECT sv.MaSV, sv.HoTen, mh.TenMH, kq.Diem
FROM KETQUA kq
JOIN SINHVIEN sv ON kq.MaSV = sv.MaSV
JOIN MONHOC   mh ON kq.MaMH = mh.MaMH
ORDER BY sv.MaSV, mh.MaMH;
GO

/* ---- Câu 8: Điểm trung bình của từng sinh viên ----
   LEFT JOIN để sinh viên chưa có điểm vẫn hiển thị (DiemTB = NULL).
   Thêm cột điểm TB có trọng số theo số tín chỉ.                        */
SELECT sv.MaSV, sv.HoTen,
       CAST(AVG(kq.Diem) AS DECIMAL(4,2))                          AS DiemTB,
       CAST(SUM(kq.Diem * mh.SoTinChi) * 1.0
            / NULLIF(SUM(mh.SoTinChi), 0) AS DECIMAL(4,2))         AS DiemTB_TinChi
FROM SINHVIEN sv
LEFT JOIN KETQUA kq ON sv.MaSV = kq.MaSV
LEFT JOIN MONHOC mh ON kq.MaMH = mh.MaMH
GROUP BY sv.MaSV, sv.HoTen
ORDER BY sv.MaSV;
GO

/* ---- Câu 9: Số sinh viên theo từng lớp (GROUP BY) ---- */
SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
LEFT JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop
ORDER BY l.MaLop;
GO

/* ---- Câu 10: Các lớp có từ 2 sinh viên trở lên (HAVING) ---- */
SELECT l.MaLop, l.TenLop, COUNT(sv.MaSV) AS SoSinhVien
FROM LOP l
JOIN SINHVIEN sv ON l.MaLop = sv.MaLop
GROUP BY l.MaLop, l.TenLop
HAVING COUNT(sv.MaSV) >= 2;
GO

/* ---- Câu 11: Test lỗi - thêm sinh viên vào lớp không tồn tại ----
   Kỳ vọng: lỗi vi phạm FOREIGN KEY FK_SINHVIEN_LOP.                  */
BEGIN TRY
    INSERT INTO SINHVIEN (MaSV, HoTen, GioiTinh, NgaySinh, MaLop)
    VALUES ('SV999', N'Sinh Viên Lỗi', N'Nam', '20040101', 'LOPXXX');
    PRINT N'KHÔNG MONG ĐỢI: câu lệnh đã chạy thành công!';
END TRY
BEGIN CATCH
    PRINT N'Câu 11 - Lỗi đúng như kỳ vọng: ' + ERROR_MESSAGE();
END CATCH
GO

/* ---- Câu 12: Test lỗi - nhập điểm ngoài khoảng 0..10 ----
   Kỳ vọng: lỗi vi phạm CHECK CK_KETQUA_Diem.                         */
BEGIN TRY
    INSERT INTO KETQUA (MaSV, MaMH, Diem) VALUES ('SV001', 'MH004', 11);
    PRINT N'KHÔNG MONG ĐỢI: câu lệnh đã chạy thành công!';
END TRY
BEGIN CATCH
    PRINT N'Câu 12 - Lỗi đúng như kỳ vọng (điểm = 11): ' + ERROR_MESSAGE();
END CATCH
GO

BEGIN TRY
    INSERT INTO KETQUA (MaSV, MaMH, Diem) VALUES ('SV001', 'MH004', -1);
    PRINT N'KHÔNG MONG ĐỢI: câu lệnh đã chạy thành công!';
END TRY
BEGIN CATCH
    PRINT N'Câu 12 - Lỗi đúng như kỳ vọng (điểm = -1): ' + ERROR_MESSAGE();
END CATCH
GO

/* ---- Test thêm: giới tính sai và số tín chỉ <= 0 ---- */
BEGIN TRY
    INSERT INTO SINHVIEN (MaSV, HoTen, GioiTinh, MaLop)
    VALUES ('SV998', N'Test Giới Tính', N'Khác', '22CTT1');
END TRY
BEGIN CATCH
    PRINT N'Test giới tính - Lỗi đúng như kỳ vọng: ' + ERROR_MESSAGE();
END CATCH
GO

BEGIN TRY
    INSERT INTO MONHOC (MaMH, TenMH, SoTinChi) VALUES ('MH999', N'Môn lỗi', 0);
END TRY
BEGIN CATCH
    PRINT N'Test số tín chỉ - Lỗi đúng như kỳ vọng: ' + ERROR_MESSAGE();
END CATCH
GO
