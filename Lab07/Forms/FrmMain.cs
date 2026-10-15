namespace ShopManagerMini
{
    /// <summary>Form chính dạng MDI Parent.</summary>
    public class FrmMain : Form
    {
        private readonly User _currentUser;
        private ToolStripStatusLabel lblUser, lblRole, lblTime;
        private System.Windows.Forms.Timer timer;

        /// <summary>True nếu đóng form do Đăng xuất (Program sẽ hiện lại FrmLogin).</summary>
        public bool IsLogout { get; private set; }

        // Truyền User qua constructor vì đây là dữ liệu bắt buộc của FrmMain
        public FrmMain(User user)
        {
            _currentUser = user;

            Text = "ShopManagerMini - Quản lý cửa hàng mini";
            WindowState = FormWindowState.Maximized;
            IsMdiContainer = true;

            BuildMenu();
            BuildStatusBar();

            FormClosing += FrmMain_FormClosing;
        }

        private void BuildMenu()
        {
            var menu = new MenuStrip { Name = "menuStrip1" };

            var mnuSystem = new ToolStripMenuItem("Hệ thống");
            var mnuLogout = new ToolStripMenuItem("Đăng xuất");
            var mnuExit = new ToolStripMenuItem("Thoát");
            mnuLogout.Click += (s, e) => Logout();
            mnuExit.Click += (s, e) => Close();
            mnuSystem.DropDownItems.AddRange(new ToolStripItem[] { mnuLogout, mnuExit });

            var mnuCategory = new ToolStripMenuItem("Danh mục");
            var mnuProduct = new ToolStripMenuItem("Quản lý sản phẩm");
            var mnuCustomer = new ToolStripMenuItem("Quản lý khách hàng");
            mnuProduct.Click += (s, e) => OpenChild<FrmProduct>();
            mnuCustomer.Click += (s, e) => OpenChild<FrmCustomer>();
            mnuCategory.DropDownItems.AddRange(new ToolStripItem[] { mnuProduct, mnuCustomer });

            var mnuHelp = new ToolStripMenuItem("Trợ giúp");
            var mnuAbout = new ToolStripMenuItem("Giới thiệu");
            mnuAbout.Click += (s, e) => OpenChild<FrmAbout>();
            mnuHelp.DropDownItems.Add(mnuAbout);

            menu.Items.AddRange(new ToolStripItem[] { mnuSystem, mnuCategory, mnuHelp });
            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        private void BuildStatusBar()
        {
            var status = new StatusStrip { Name = "statusStrip1" };
            lblUser = new ToolStripStatusLabel("Người dùng: " + _currentUser.FullName);
            lblRole = new ToolStripStatusLabel("Vai trò: " + _currentUser.Role);
            lblTime = new ToolStripStatusLabel { Spring = true, TextAlign = ContentAlignment.MiddleRight };
            status.Items.AddRange(new ToolStripItem[] { lblUser, new ToolStripSeparator(), lblRole, lblTime });
            Controls.Add(status);

            timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (s, e) => lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            timer.Start();
            lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Mở Form con MDI. Nếu Form cùng loại đã mở thì kích hoạt lại, không tạo mới.
        /// </summary>
        private void OpenChild<T>() where T : Form, new()
        {
            foreach (Form child in MdiChildren)
            {
                if (child is T)
                {
                    if (child.WindowState == FormWindowState.Minimized)
                        child.WindowState = FormWindowState.Normal;
                    child.Activate();
                    return;
                }
            }

            var frm = new T { MdiParent = this };
            frm.Show();
        }

        private void Logout()
        {
            if (MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IsLogout = true;
                Close();
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsLogout) return; // đã xác nhận ở Logout()

            if (MessageBox.Show("Bạn có chắc muốn thoát chương trình?", "Xác nhận thoát",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
    }
}
