namespace COMP1019_Lab09_StudentCard
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FrmStudentCard());
        }
    }
}
