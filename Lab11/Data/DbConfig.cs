using Microsoft.Data.SqlClient;

namespace COMP1019_Lab11_AdoNet
{
    /// <summary>
    /// Cấu hình kết nối SQL Server.
    /// Thứ tự ưu tiên:
    ///   1. Biến môi trường EDUMANAGER_CONNECTION (nếu có).
    ///   2. Tự dò lần lượt các server trong CandidateServers, dùng server đầu tiên kết nối được
    ///      (Windows Authentication).
    /// Muốn cố định một connection string, sửa hằng FixedConnectionString bên dưới.
    /// </summary>
    public static class DbConfig
    {
        public const string DatabaseName = "EduManagerDB";

        /// <summary>Điền connection string vào đây nếu muốn bỏ qua việc tự dò. Để rỗng thì tự dò.</summary>
        private const string FixedConnectionString = "";

        private static readonly string[] CandidateServers =
        {
            "localhost",                 // SQL Server bản mặc định (Developer, Standard...)
            @".\SQLEXPRESS",             // SQL Server Express
            @"(localdb)\MSSQLLocalDB"    // LocalDB đi kèm Visual Studio
        };

        private static string _connectionString;

        public static string ConnectionString => _connectionString ??= Resolve();

        private static string Resolve()
        {
            if (!string.IsNullOrWhiteSpace(FixedConnectionString))
                return FixedConnectionString;

            string env = Environment.GetEnvironmentVariable("EDUMANAGER_CONNECTION");
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
                ".\nHãy kiểm tra dịch vụ SQL Server đang chạy, hoặc điền connection string vào DbConfig.cs.");
        }
    }
}
