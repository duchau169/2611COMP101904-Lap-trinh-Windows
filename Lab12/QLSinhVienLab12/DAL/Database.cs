using Microsoft.Data.SqlClient;

namespace QLSinhVienLab12.DAL;

/// <summary>
/// Nơi DUY NHẤT chứa chuỗi kết nối. Cần đổi Data Source cho đúng máy của bạn:
///   .                       : SQL Server mặc định trên máy này
///   .\SQLEXPRESS            : SQL Server Express (cũng viết được là localhost\SQLEXPRESS)
///   (localdb)\MSSQLLocalDB  : LocalDB đi kèm Visual Studio
/// Không muốn sửa code: đặt biến môi trường QLSV_LAB12_CONNSTR (xem README).
/// </summary>
public static class Database
{
    private const string DefaultConnectionString =
        @"Data Source=localhost\SQLEXPRESS;Initial Catalog=QLSinhVienLab12;Integrated Security=True;TrustServerCertificate=True";

    public static string ConnectionString
    {
        get
        {
            string? fromEnvironment = Environment.GetEnvironmentVariable("QLSV_LAB12_CONNSTR");
            return string.IsNullOrWhiteSpace(fromEnvironment) ? DefaultConnectionString : fromEnvironment;
        }
    }

    /// <summary>Tạo kết nối mới (chưa mở). Người gọi dùng "using" để đóng kết nối.</summary>
    public static SqlConnection CreateConnection() => new SqlConnection(ConnectionString);
}
