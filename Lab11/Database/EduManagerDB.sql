/* ============================================================
   COMP1019 - Lab 11: Script tạo database EduManagerDB
   - Chạy nhiều lần vẫn an toàn (không tạo trùng, không lỗi).
   Mở bằng SSMS, bấm Execute (F5).
   ============================================================ */
IF DB_ID('EduManagerDB') IS NULL
    CREATE DATABASE EduManagerDB;
GO

USE EduManagerDB;
GO

IF OBJECT_ID('dbo.LOP', 'U') IS NULL
BEGIN
    CREATE TABLE LOP (
        MaLop  VARCHAR(20)   PRIMARY KEY,
        TenLop NVARCHAR(100) NOT NULL
    );
END
GO

IF OBJECT_ID('dbo.SINHVIEN', 'U') IS NULL
BEGIN
    CREATE TABLE SINHVIEN (
        MaSV   VARCHAR(20)   PRIMARY KEY,
        HoTen  NVARCHAR(100) NOT NULL,
        MaLop  VARCHAR(20)   NOT NULL,
        CONSTRAINT FK_SINHVIEN_LOP FOREIGN KEY (MaLop) REFERENCES LOP(MaLop)
    );
END
GO

BEGIN TRY
    DECLARE @stmts TABLE (Id INT IDENTITY(1,1), Stmt NVARCHAR(500));

    INSERT INTO @stmts (Stmt)
    SELECT N'ALTER TABLE dbo.LOP ALTER COLUMN ' + QUOTENAME(c.name) + N' ' +
           CASE
               WHEN t.name IN ('varchar','char','varbinary','binary')
                   THEN t.name + '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR(10)) END + ')'
               WHEN t.name IN ('nvarchar','nchar')
                   THEN t.name + '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length / 2 AS VARCHAR(10)) END + ')'
               WHEN t.name IN ('decimal','numeric')
                   THEN t.name + '(' + CAST(c.precision AS VARCHAR(5)) + ',' + CAST(c.scale AS VARCHAR(5)) + ')'
               WHEN t.name IN ('datetime2','time','datetimeoffset')
                   THEN t.name + '(' + CAST(c.scale AS VARCHAR(5)) + ')'
               ELSE t.name
           END + N' NULL'
    FROM sys.columns c
    JOIN sys.types t ON t.user_type_id = c.system_type_id
    WHERE c.object_id = OBJECT_ID('dbo.LOP')
      AND c.is_nullable = 0
      AND c.is_identity = 0
      AND c.is_computed = 0
      AND c.default_object_id = 0
      AND c.name NOT IN ('MaLop', 'TenLop')
      AND t.name <> 'timestamp'
      AND NOT EXISTS (SELECT 1
                      FROM sys.index_columns ic
                      JOIN sys.indexes i ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                      WHERE i.is_primary_key = 1 AND ic.object_id = c.object_id AND ic.column_id = c.column_id);

    DECLARE @i INT = 1, @n INT = (SELECT COUNT(*) FROM @stmts), @sql NVARCHAR(500);
    WHILE @i <= @n
    BEGIN
        SELECT @sql = Stmt FROM @stmts WHERE Id = @i;
        EXEC sp_executesql @sql;
        PRINT N'Đã điều chỉnh bảng LOP: ' + @sql;
        SET @i = @i + 1;
    END
END TRY
BEGIN CATCH
    PRINT N'Không điều chỉnh được bảng LOP: ' + ERROR_MESSAGE();
END CATCH
GO

/* ----- Dữ liệu mẫu (chỉ thêm nếu chưa có) ----- */
BEGIN TRY
    IF NOT EXISTS (SELECT 1 FROM LOP WHERE MaLop = 'CNTT01')
        INSERT INTO LOP(MaLop, TenLop) VALUES ('CNTT01', N'Công nghệ thông tin 01');
    IF NOT EXISTS (SELECT 1 FROM LOP WHERE MaLop = 'CNTT02')
        INSERT INTO LOP(MaLop, TenLop) VALUES ('CNTT02', N'Công nghệ thông tin 02');
    IF NOT EXISTS (SELECT 1 FROM LOP WHERE MaLop = 'SPTIN01')
        INSERT INTO LOP(MaLop, TenLop) VALUES ('SPTIN01', N'Sư phạm Tin học 01');
END TRY
BEGIN CATCH
    PRINT N'Không thêm được dữ liệu mẫu cho LOP: ' + ERROR_MESSAGE();
END CATCH
GO

BEGIN TRY
    IF NOT EXISTS (SELECT 1 FROM SINHVIEN WHERE MaSV = 'SV001')
        INSERT INTO SINHVIEN(MaSV, HoTen, MaLop) VALUES ('SV001', N'Nguyễn Văn An', 'CNTT01');
END TRY
BEGIN CATCH
    PRINT N'Không thêm được dữ liệu mẫu cho SINHVIEN: ' + ERROR_MESSAGE();
END CATCH
GO

/* Kiểm tra nhanh */
SELECT * FROM LOP;
SELECT * FROM SINHVIEN;
GO
