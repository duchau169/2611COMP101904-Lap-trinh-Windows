namespace StudentInfoApp
{
    /// <summary>
    /// Lớp chứa hàm Main - điểm khởi đầu của chương trình.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Điểm khởi đầu chính của ứng dụng.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Cấu hình cấu hình mặc định cho ứng dụng (font, DPI...)
            ApplicationConfiguration.Initialize();

            // Chạy Form chính
            Application.Run(new Form1());
        }
    }
}
