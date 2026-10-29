using QLSinhVienLab12.Forms;

namespace QLSinhVienLab12;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FrmNhapDiemTheoLop());
    }
}
