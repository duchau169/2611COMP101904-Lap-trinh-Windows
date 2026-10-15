namespace ShopManagerMini
{
    /// <summary>Form đăng nhập. Mở bằng ShowDialog(), trả DialogResult.OK khi thành công.</summary>
    public class FrmLogin : Form
    {
        private TextBox txtUsername, txtPassword;
        private Button btnLogin, btnExit;

        /// <summary>Người dùng đăng nhập thành công (Program đọc property này).</summary>
        public User LoggedInUser { get; private set; }

        public FrmLogin()
        {
            Text = "Đăng nhập - ShopManagerMini";
            ClientSize = new Size(340, 190);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false; MinimizeBox = false;

            var lblTitle = new Label
            {
                Text = "ĐĂNG NHẬP", Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = true, Left = 100, Top = 15
            };
            txtUsername = UiHelper.TextBox("txtUsername", 120, 60, 190);
            txtPassword = UiHelper.TextBox("txtPassword", 120, 95, 190);
            txtPassword.UseSystemPasswordChar = true;
            btnLogin = UiHelper.Button("btnLogin", "Đăng nhập", 120, 135, 90);
            btnExit = UiHelper.Button("btnExit", "Thoát", 220, 135, 90);
            btnExit.DialogResult = DialogResult.Cancel;

            btnLogin.Click += BtnLogin_Click;
            AcceptButton = btnLogin;
            CancelButton = btnExit;

            Controls.AddRange(new Control[]
            {
                lblTitle,
                UiHelper.Label("Username:", 30, 60), txtUsername,
                UiHelper.Label("Password:", 30, 95), txtPassword,
                btnLogin, btnExit
            });
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ username và password!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = DataStore.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            LoggedInUser = user;
            DialogResult = DialogResult.OK; // tự đóng form
        }
    }
}
