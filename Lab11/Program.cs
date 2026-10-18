namespace COMP1019_Lab11_AdoNet
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmLop());
        }
    }
}
