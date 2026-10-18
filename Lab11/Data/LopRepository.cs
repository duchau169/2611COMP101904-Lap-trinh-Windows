using System.Data;
using Microsoft.Data.SqlClient;

namespace COMP1019_Lab11_AdoNet
{
    /// <summary>
    /// Truy cập bảng LOP bằng ADO.NET. Mọi câu SQL nhận dữ liệu từ người dùng đều dùng
    /// SqlParameter, tuyệt đối không nối chuỗi.
    /// </summary>
    public class LopRepository
    {
        private const string SelectWithCount =
            @"SELECT l.MaLop, l.TenLop,
                     (SELECT COUNT(*) FROM SINHVIEN sv WHERE sv.MaLop = l.MaLop) AS SoSinhVien
              FROM LOP l";

        private static SqlConnection CreateConnection() => new SqlConnection(DbConfig.ConnectionString);

        // ---------- Tham số dùng chung ----------
        private static void AddMaLop(SqlCommand cmd, string value) =>
            cmd.Parameters.Add("@MaLop", SqlDbType.VarChar, 20).Value = value;

        private static void AddTenLop(SqlCommand cmd, string value) =>
            cmd.Parameters.Add("@TenLop", SqlDbType.NVarChar, 100).Value = value;

        // ---------- F1: lấy toàn bộ danh sách ----------
        public DataTable GetAll()
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(SelectWithCount + " ORDER BY l.MaLop", conn))
            using (var adapter = new SqlDataAdapter(cmd))
            {
                var table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        // ---------- F6: tìm theo mã hoặc tên bằng LIKE + parameter ----------
        public DataTable Search(string keyword)
        {
            string sql = SelectWithCount +
                         @" WHERE l.MaLop LIKE @Keyword ESCAPE '\' OR l.TenLop LIKE @Keyword ESCAPE '\'
                            ORDER BY l.MaLop";

            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@Keyword", SqlDbType.NVarChar, 200).Value = "%" + EscapeLike(keyword) + "%";
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        /// <summary>Escape các ký tự đặc biệt của LIKE (% _ [ \) để tìm đúng nghĩa đen.</summary>
        public static string EscapeLike(string text)
        {
            return text.Replace(@"\", @"\\")
                       .Replace("%", @"\%")
                       .Replace("_", @"\_")
                       .Replace("[", @"\[");
        }

        // ---------- Tương thích bảng LOP của buổi 10 ----------
        /// <summary>
        /// Tìm các cột bắt buộc nhập (NOT NULL, không có giá trị mặc định, không phải khóa chính)
        /// ngoài MaLop và TenLop. Những cột này sẽ làm câu INSERT của Lab 11 bị lỗi.
        /// Câu SQL chỉ đọc danh mục hệ thống, không chứa dữ liệu người dùng.
        /// </summary>
        public List<SchemaFix> FindExtraRequiredColumns()
        {
            const string sql = @"
SELECT c.name AS ColumnName,
       N'ALTER TABLE dbo.LOP ALTER COLUMN ' + QUOTENAME(c.name) + N' ' +
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
       END + N' NULL' AS AlterStatement
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
                  WHERE i.is_primary_key = 1 AND ic.object_id = c.object_id AND ic.column_id = c.column_id)";

            var result = new List<SchemaFix>();
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new SchemaFix
                        {
                            ColumnName = reader.GetString(0),
                            AlterStatement = reader.GetString(1)
                        });
                    }
                }
            }
            return result;
        }

        /// <summary>Cho phép các cột tìm được ở trên nhận NULL. Dữ liệu hiện có không bị thay đổi.</summary>
        public void AllowNulls(IEnumerable<SchemaFix> fixes)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                foreach (SchemaFix fix in fixes)
                {
                    // AlterStatement do chính SQL Server tạo ra từ danh mục hệ thống (đã QUOTENAME), không phải dữ liệu người dùng
                    using (var cmd = new SqlCommand(fix.AlterStatement, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // ---------- Kiểm tra trùng mã / đếm sinh viên ----------
        public bool Exists(string maLop)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM LOP WHERE MaLop = @MaLop", conn))
            {
                AddMaLop(cmd, maLop);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        public int CountStudents(string maLop)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand("SELECT COUNT(1) FROM SINHVIEN WHERE MaLop = @MaLop", conn))
            {
                AddMaLop(cmd, maLop);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        // ---------- F3: thêm ----------
        public int Insert(Lop lop)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand("INSERT INTO LOP(MaLop, TenLop) VALUES (@MaLop, @TenLop)", conn))
            {
                AddMaLop(cmd, lop.MaLop);
                AddTenLop(cmd, lop.TenLop);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // ---------- F4: sửa tên lớp theo mã ----------
        public int Update(Lop lop)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand("UPDATE LOP SET TenLop = @TenLop WHERE MaLop = @MaLop", conn))
            {
                AddTenLop(cmd, lop.TenLop);
                AddMaLop(cmd, lop.MaLop);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        // ---------- F5: xóa ----------
        public int Delete(string maLop)
        {
            using (var conn = CreateConnection())
            using (var cmd = new SqlCommand("DELETE FROM LOP WHERE MaLop = @MaLop", conn))
            {
                AddMaLop(cmd, maLop);
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
