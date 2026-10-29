using StudentReportDemo.Forms;

namespace StudentReportDemo;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmBaoCaoSinhVien());
    }
}
