namespace COMP1019_Lab08_SimplePaint
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmPaint());
        }
    }
}
