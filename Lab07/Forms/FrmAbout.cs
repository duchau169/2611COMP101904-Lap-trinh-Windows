namespace ShopManagerMini
{
    /// <summary>Form giới thiệu ứng dụng.</summary>
    public class FrmAbout : Form
    {
        public FrmAbout()
        {
            Text = "Giới thiệu";
            ClientSize = new Size(380, 200);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11),
                // TODO: thay bằng thông tin của bạn
                Text = "ShopManagerMini v1.0\r\n" +
                       "Ứng dụng quản lý cửa hàng mini\r\n\r\n" +
                       "Sinh viên: [Họ và tên]\r\n" +
                       "MSSV: [Mã số sinh viên]\r\n" +
                       "Lớp: [Tên lớp]\r\n" +
                       "Môn: COMP1019 - Lập trình trên Windows"
            };
            Controls.Add(lbl);
        }
    }
}
