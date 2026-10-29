using System.Data;
using Microsoft.Data.SqlClient;

namespace QLSinhVienLab12.DAL;

/// <summary>
/// Lớp truy cập dữ liệu (Data Access Layer) cho kết quả học tập.
/// Mọi thao tác với database đều đi qua stored procedure và SqlParameter,
/// Form không viết SQL và không nối chuỗi SQL từ dữ liệu người dùng.
/// </summary>
public class KetQuaDAL
{
    /// <summary>Danh sách lớp (MaLop, TenLop) - gọi sp_Lop_GetAll.</summary>
    public DataTable GetLop() => ExecuteTable("sp_Lop_GetAll");

    /// <summary>Danh sách môn học (MaMon, TenMon, SoTinChi) - gọi sp_MonHoc_GetAll.</summary>
    public DataTable GetMonHoc() => ExecuteTable("sp_MonHoc_GetAll");

    /// <summary>
    /// Danh sách sinh viên của lớp kèm điểm môn học (nếu đã có) - gọi sp_KetQua_GetByLopMon.
    /// Cột trả về: MaSV, HoTen, Diem (NULL nếu chưa có điểm), GhiChu.
    /// </summary>
    public DataTable GetKetQuaByLopMon(string maLop, string maMon)
    {
        using SqlConnection conn = Database.CreateConnection();
        using SqlCommand cmd = new SqlCommand("sp_KetQua_GetByLopMon", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@MaLop", SqlDbType.NVarChar, 20).Value = maLop;
        cmd.Parameters.Add("@MaMon", SqlDbType.NVarChar, 20).Value = maMon;
        return Fill(cmd);
    }

    /// <summary>
    /// Lưu điểm của TẤT CẢ dòng trong <paramref name="table"/> bằng MỘT transaction:
    /// mọi dòng thành công mới Commit; chỉ cần một dòng lỗi thì Rollback toàn bộ.
    /// Trả về số dòng đã lưu. Khi lỗi sẽ ném InvalidOperationException có nêu rõ sinh viên bị lỗi.
    /// </summary>
    public int SaveAll(string maMon, DataTable table)
    {
        using SqlConnection conn = Database.CreateConnection();
        conn.Open();
        using SqlTransaction trans = conn.BeginTransaction();

        string currentMaSV = string.Empty;   // sinh viên đang lưu, để báo đúng dòng bị lỗi
        try
        {
            int count = 0;
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted) continue;

                currentMaSV = Convert.ToString(row["MaSV"]) ?? string.Empty;

                // Command PHẢI gắn transaction (tham số thứ 3), nếu không transaction không có tác dụng.
                using SqlCommand cmd = new SqlCommand("sp_KetQua_Save", conn, trans);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MaSV", SqlDbType.NVarChar, 20).Value = currentMaSV;
                cmd.Parameters.Add("@MaMon", SqlDbType.NVarChar, 20).Value = maMon;

                // Điểm rỗng thì truyền DBNull để stored procedure tự báo lỗi (không đoán giá trị thay người dùng).
                if (row.IsNull("Diem"))
                    cmd.Parameters.Add("@Diem", SqlDbType.Float).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@Diem", SqlDbType.Float).Value = Convert.ToDouble(row["Diem"]);

                string ghiChu = (Convert.ToString(row["GhiChu"]) ?? string.Empty).Trim();
                if (ghiChu.Length == 0)
                    cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar, 255).Value = DBNull.Value;
                else
                    cmd.Parameters.Add("@GhiChu", SqlDbType.NVarChar, 255).Value = ghiChu;

                cmd.ExecuteNonQuery();
                count++;
            }

            trans.Commit();   // chỉ commit MỘT LẦN, sau khi tất cả dòng đã lưu thành công
            return count;
        }
        catch (Exception ex)
        {
            try
            {
                trans.Rollback();   // hủy toàn bộ các dòng đã lưu trước đó trong lần này
            }
            catch (InvalidOperationException)
            {
                // Giao dịch đã bị SQL Server hủy sẵn, không cần Rollback nữa.
            }

            string where = currentMaSV.Length == 0 ? string.Empty : $" tại sinh viên {currentMaSV}";
            throw new InvalidOperationException($"Lưu thất bại{where}: {ex.Message}", ex);
        }
    }

    private static DataTable ExecuteTable(string procedureName)
    {
        using SqlConnection conn = Database.CreateConnection();
        using SqlCommand cmd = new SqlCommand(procedureName, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        return Fill(cmd);
    }

    private static DataTable Fill(SqlCommand cmd)
    {
        using SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable table = new DataTable();
        adapter.Fill(table);   // Fill tự mở và đóng kết nối
        return table;
    }
}
