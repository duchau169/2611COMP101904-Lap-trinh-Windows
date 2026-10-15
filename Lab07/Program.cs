namespace ShopManagerMini
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Vòng lặp: Đăng nhập -> Form chính -> (Đăng xuất thì quay lại Đăng nhập)
            while (true)
            {
                User user;
                using (var login = new FrmLogin())
                {
                    // ShowDialog: chặn luồng cho đến khi đăng nhập xong
                    if (login.ShowDialog() != DialogResult.OK) break;
                    user = login.LoggedInUser;
                }

                var main = new FrmMain(user);
                Application.Run(main);
                if (!main.IsLogout) break; // thoát hẳn chương trình
            }
        }
    }
}
