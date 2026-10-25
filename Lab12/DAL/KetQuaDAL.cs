using System.Data;
using Microsoft.Data.SqlClient;

namespace QLSinhVienLab12
{
    /// <summary>Lỗi khi lưu kết quả: cho biết sinh viên nào gây lỗi. Khi lỗi này xảy ra, transaction đã được rollback.</summary>
    public class KetQuaSaveException : Exception
    {
        public string MaSV { get; }

        public KetQuaSaveException(string maSV, Exception inner)
            : base($"Lỗi khi lưu điểm sinh viên {maSV}: {inner.Message}", inner)
        {
            MaSV = maSV;
        }
    }

    /// <summary>Truy cập database cho module kết quả học tập. Chỉ gọi stored procedure, luôn dùng SqlParameter.</summary>
    public class KetQuaDAL
    {
        // ---------- Lấy dữ liệu (SqlDataAdapter + DataTable để binding) ----------
        public DataTable GetLopList() => ExecuteTable("sp_Lop_GetAll", null);

        public DataTable GetMonHocList() => ExecuteTable("sp_MonHoc_GetAll", null);

        public DataTable GetByLopMon(string maLop, string maMon)
        {
            return ExecuteTable("sp_KetQua_GetByLopMon", p =>
            {
                p.Add("@MaLop", SqlDbType.VarChar, 20).Value = maLop;
                p.Add("@MaMon", SqlDbType.VarChar, 20).Value = maMon;
            });
        }

        private static DataTable ExecuteTable(string procedureName, Action<SqlParameterCollection> addParameters)
        {
            using (SqlConnection conn = Database.CreateConnection())
            using (var cmd = new SqlCommand(procedureName, conn) { CommandType = CommandType.StoredProcedure })
            {
                addParameters?.Invoke(cmd.Parameters);
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        // ---------- Thống kê (mở rộng) ----------
        public ThongKe GetThongKe(string maLop, string maMon)
        {
            using (SqlConnection conn = Database.CreateConnection())
            using (var cmd = new SqlCommand("sp_KetQua_ThongKe", conn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add("@MaLop", SqlDbType.VarChar, 20).Value = maLop;
                cmd.Parameters.Add("@MaMon", SqlDbType.VarChar, 20).Value = maMon;
                conn.Open();

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    var result = new ThongKe();
                    if (r.Read())
                    {
                        result.SoSinhVien = r.GetInt32(r.GetOrdinal("SoSinhVien"));
                        result.SoCoDiem = r.GetInt32(r.GetOrdinal("SoCoDiem"));
                        result.DiemTB = GetNullableDecimal(r, "DiemTB");
                        result.DiemMax = GetNullableDecimal(r, "DiemMax");
                        result.DiemMin = GetNullableDecimal(r, "DiemMin");
                        result.SoDat = r.IsDBNull(r.GetOrdinal("SoDat")) ? 0 : r.GetInt32(r.GetOrdinal("SoDat"));
                    }
                    return result;
                }
            }
        }

        private static decimal? GetNullableDecimal(SqlDataReader r, string column)
        {
            int i = r.GetOrdinal(column);
            return r.IsDBNull(i) ? (decimal?)null : r.GetDecimal(i);
        }

        // ---------- Lưu nhiều dòng bằng transaction ----------
        /// <summary>
        /// Lưu tất cả dòng trong MỘT transaction: tất cả thành công thì Commit, chỉ cần một dòng lỗi
        /// thì Rollback toàn bộ và ném KetQuaSaveException. Trả về số dòng đã lưu.
        /// </summary>
        public int SaveAll(string maMon, IEnumerable<KetQuaItem> items)
        {
            int saved = 0;
            string currentMaSV = null;

            using (SqlConnection conn = Database.CreateConnection())
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        foreach (KetQuaItem item in items)
                        {
                            currentMaSV = item.MaSV;
                            SaveOne(conn, tran, maMon, item);
                            saved++;
                        }

                        tran.Commit();
                        return saved;
                    }
                    catch (SqlException ex)
                    {
                        SafeRollback(tran);
                        throw new KetQuaSaveException(currentMaSV, ex);
                    }
                    catch (Exception)
                    {
                        SafeRollback(tran);
                        throw;
                    }
                }
            }
        }

        private static void SaveOne(SqlConnection conn, SqlTransaction tran, string maMon, KetQuaItem item)
        {
            using (var cmd = new SqlCommand("sp_KetQua_Save", conn, tran) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add("@MaSV", SqlDbType.VarChar, 20).Value = item.MaSV;
                cmd.Parameters.Add("@MaMon", SqlDbType.VarChar, 20).Value = maMon;

                SqlParameter diem = cmd.Parameters.Add("@Diem", SqlDbType.Decimal);
                diem.Precision = 4;
                diem.Scale = 2;
                diem.Value = item.Diem;

                cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar, 200).Value =
                    string.IsNullOrWhiteSpace(item.GhiChu) ? DBNull.Value : item.GhiChu.Trim();

                cmd.ExecuteNonQuery();
            }
        }

        private static void SafeRollback(SqlTransaction tran)
        {
            try
            {
                tran.Rollback();
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is SqlException)
            {
                // transaction đã bị hủy sẵn (ví dụ mất kết nối): không còn gì để rollback
            }
        }
    }
}
