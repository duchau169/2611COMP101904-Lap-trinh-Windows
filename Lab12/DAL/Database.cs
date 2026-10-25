using Microsoft.Data.SqlClient;

namespace QLSinhVienLab12
{
    /// <summary>
    /// Cấu hình kết nối SQL Server.
    /// Thứ tự ưu tiên:
    ///   1. Hằng FixedConnectionString (nếu điền).
    ///   2. Biến môi trường QLSINHVIEN_CONNECTION (nếu có).
    ///   3. Tự dò lần lượt các server trong CandidateServers (Windows Authentication).
    /// </summary>
    public static class Database
    {
        public const string DatabaseName = "QLSinhVienLab12";

        /// <summary>Điền connection string vào đây nếu muốn bỏ qua việc tự dò. Để rỗng thì tự dò.</summary>
        private const string FixedConnectionString = "";

        private static readonly string[] CandidateServers =
        {
            "localhost",
            @".\SQLEXPRESS",
            @"(localdb)\MSSQLLocalDB"
        };

        private static string _connectionString;

        public static string ConnectionString => _connectionString ??= Resolve();

        public static SqlConnection CreateConnection() => new SqlConnection(ConnectionString);

        private static string Resolve()
        {
            if (!string.IsNullOrWhiteSpace(FixedConnectionString))
                return FixedConnectionString;

            string env = Environment.GetEnvironmentVariable("QLSINHVIEN_CONNECTION");
            if (!string.IsNullOrWhiteSpace(env))
                return env;

            foreach (string server in CandidateServers)
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = server,
                    InitialCatalog = "master",   // thử vào server trước, chưa cần có database
                    IntegratedSecurity = true,
                    TrustServerCertificate = true,
                    ConnectTimeout = 3
                };

                try
                {
                    using (var conn = new SqlConnection(builder.ConnectionString))
                    {
                        conn.Open();
                    }
                    builder.InitialCatalog = DatabaseName;
                    builder.ConnectTimeout = 15;
                    return builder.ConnectionString;
                }
                catch (SqlException)
                {
                    // thử server tiếp theo
                }
            }

            throw new InvalidOperationException(
                "Không kết nối được SQL Server. Đã thử: " + string.Join(", ", CandidateServers) +
                ".\nHãy kiểm tra dịch vụ SQL Server đang chạy, hoặc điền connection string vào Database.cs.");
        }
    }
}
