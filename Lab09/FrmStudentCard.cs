using System.ComponentModel;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace COMP1019_Lab09_StudentCard
{
    /// <summary>Form chính: nhập thông tin, chọn ảnh, xem trước, in thẻ sinh viên.</summary>
    public class FrmStudentCard : Form
    {
        // ----- Nhóm thông tin -----
        private TextBox txtStudentId, txtFullName;
        private ComboBox cboClass, cboFaculty, cboCourse, cboGender;
        private DateTimePicker dtpBirthDate;

        // ----- Nhóm ảnh -----
        private PictureBox picAvatar;
        private Button btnChooseImage, btnCardColor;
        private Label lblImageInfo;

        // ----- Nút điều khiển -----
        private Button btnPrintPreview, btnPrint, btnSavePng, btnRefresh, btnExit;

        // ----- Preview và in ấn -----
        private DoubleBufferedPanel pnlPreview;
        private PrintDocument printDoc;
        private ErrorProvider errProvider;

        // ----- Dữ liệu -----
        private Bitmap _avatar;                                  // ảnh đại diện (bản sao, không khóa file gốc)
        private Color _cardColor = Color.FromArgb(232, 240, 250); // màu nền thẻ

        public FrmStudentCard()
        {
            Text = "COMP1019 - Lab 09 - Thiết kế và in thẻ sinh viên";
            ClientSize = new Size(1120, 660);
            MinimumSize = new Size(1000, 640);
            StartPosition = FormStartPosition.CenterScreen;

            BuildUi();
            BuildPrintDocument();
            WireEvents();
            ResetForm();
        }

        // ================= GIAO DIỆN =================
        private void BuildUi()
        {
            errProvider = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink };

            // ----- Vùng nhập liệu bên trái -----
            var pnlInput = new Panel { Name = "pnlInput", Dock = DockStyle.Left, Width = 440, AutoScroll = true };

            var grpInfo = new GroupBox { Text = "Thông tin sinh viên", Left = 10, Top = 8, Width = 415, Height = 275 };
            txtStudentId = new TextBox { Name = "txtStudentId", MaxLength = 15 };
            txtFullName = new TextBox { Name = "txtFullName", MaxLength = 40 };
            cboClass = new ComboBox { Name = "cboClass", DropDownStyle = ComboBoxStyle.DropDown };
            cboClass.Items.AddRange(new object[] { "K49-CNTT1", "K49-CNTT2", "K50-CNTT1", "K50-CNTT2", "K51-CNTT1" });
            cboFaculty = new ComboBox { Name = "cboFaculty", DropDownStyle = ComboBoxStyle.DropDownList };
            cboFaculty.Items.AddRange(new object[]
                { "Công nghệ thông tin", "Toán - Tin học", "Vật lý", "Hóa học", "Sinh học", "Ngữ văn", "Ngoại ngữ" });
            cboCourse = new ComboBox { Name = "cboCourse", DropDownStyle = ComboBoxStyle.DropDownList };
            cboCourse.Items.AddRange(new object[] { "K49", "K50", "K51", "K52" });
            dtpBirthDate = new DateTimePicker
            {
                Name = "dtpBirthDate",
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MaxDate = DateTime.Today // không cho chọn ngày sinh ở tương lai
            };
            cboGender = new ComboBox { Name = "cboGender", DropDownStyle = ComboBoxStyle.DropDownList };
            cboGender.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });

            AddRow(grpInfo, "Mã sinh viên (*):", 30, txtStudentId);
            AddRow(grpInfo, "Họ tên (*):", 64, txtFullName);
            AddRow(grpInfo, "Lớp:", 98, cboClass);
            AddRow(grpInfo, "Khoa:", 132, cboFaculty);
            AddRow(grpInfo, "Khóa học:", 166, cboCourse);
            AddRow(grpInfo, "Ngày sinh:", 200, dtpBirthDate);
            AddRow(grpInfo, "Giới tính:", 234, cboGender);

            var grpAvatar = new GroupBox { Text = "Ảnh đại diện (*)", Left = 10, Top = 290, Width = 415, Height = 175 };
            picAvatar = new PictureBox
            {
                Name = "picAvatar",
                Left = 15, Top = 25, Width = 110, Height = 135,
                SizeMode = PictureBoxSizeMode.Zoom, // giữ đúng tỉ lệ, không méo
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };
            btnChooseImage = MakeButton("btnChooseImage", "Chọn ảnh...", 145, 28, 150);
            btnCardColor = MakeButton("btnCardColor", "Màu nền thẻ", 145, 68, 150);
            lblImageInfo = new Label
            {
                Name = "lblImageInfo", Left = 145, Top = 108, Width = 260, Height = 50,
                ForeColor = Color.DimGray, Text = "Chưa chọn ảnh"
            };
            grpAvatar.Controls.AddRange(new Control[] { picAvatar, btnChooseImage, btnCardColor, lblImageInfo });

            btnPrintPreview = MakeButton("btnPrintPreview", "Xem trước in", 10, 478, 130);
            btnPrint = MakeButton("btnPrint", "In thẻ", 150, 478, 130);
            btnSavePng = MakeButton("btnSavePng", "Lưu PNG", 290, 478, 130);
            btnRefresh = MakeButton("btnRefresh", "Làm mới", 10, 518, 130);
            btnExit = MakeButton("btnExit", "Thoát", 150, 518, 130);

            pnlInput.Controls.AddRange(new Control[]
                { grpInfo, grpAvatar, btnPrintPreview, btnPrint, btnSavePng, btnRefresh, btnExit });

            // ----- Vùng preview bên phải -----
            var grpPreview = new GroupBox { Text = "Xem trước thẻ", Dock = DockStyle.Fill, Padding = new Padding(8) };
            pnlPreview = new DoubleBufferedPanel
            {
                Name = "pnlPreview",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(222, 226, 231)
            };
            grpPreview.Controls.Add(pnlPreview);

            // Thêm Fill trước, Left sau để không bị che
            Controls.Add(grpPreview);
            Controls.Add(pnlInput);
        }

        private static void AddRow(GroupBox parent, string label, int y, Control input)
        {
            parent.Controls.Add(new Label { Text = label, Left = 15, Top = y + 3, AutoSize = true });
            input.Left = 125;
            input.Top = y;
            input.Width = 270;
            parent.Controls.Add(input);
        }

        private static Button MakeButton(string name, string text, int x, int y, int w) =>
            new Button { Name = name, Text = text, Left = x, Top = y, Width = w, Height = 32 };

        private void BuildPrintDocument()
        {
            printDoc = new PrintDocument { DocumentName = "The sinh vien" };
            printDoc.DefaultPageSettings.Margins = new Margins(50, 50, 50, 50); // đơn vị 1/100 inch
            printDoc.PrintPage += PrintDoc_PrintPage;
        }

        private void WireEvents()
        {
            // F2: dữ liệu đổi thì vẽ lại preview
            txtStudentId.TextChanged += OnDataChanged;
            txtFullName.TextChanged += OnDataChanged;
            cboClass.TextChanged += OnDataChanged;
            cboFaculty.SelectedIndexChanged += OnDataChanged;
            cboCourse.SelectedIndexChanged += OnDataChanged;
            cboGender.SelectedIndexChanged += OnDataChanged;
            dtpBirthDate.ValueChanged += OnDataChanged;

            pnlPreview.Paint += PnlPreview_Paint;

            btnChooseImage.Click += BtnChooseImage_Click;
            btnCardColor.Click += BtnCardColor_Click;
            btnPrintPreview.Click += BtnPrintPreview_Click;
            btnPrint.Click += BtnPrint_Click;
            btnSavePng.Click += BtnSavePng_Click;
            btnRefresh.Click += (s, e) => ResetForm();
            btnExit.Click += (s, e) => Close();

            FormClosed += (s, e) =>
            {
                picAvatar.Image = null;
                _avatar?.Dispose();
                printDoc.Dispose();
                errProvider.Dispose();
            };
        }

        // ================= DỮ LIỆU =================
        /// <summary>Gom dữ liệu trên Form thành đối tượng StudentInfo.</summary>
        private StudentInfo BuildStudent()
        {
            return new StudentInfo
            {
                StudentId = txtStudentId.Text.Trim(),
                FullName = txtFullName.Text.Trim(),
                ClassName = cboClass.Text.Trim(),
                Faculty = cboFaculty.Text,
                Course = cboCourse.Text,
                BirthDate = dtpBirthDate.Value.Date,
                Gender = cboGender.Text,
                Avatar = _avatar
            };
        }

        private void OnDataChanged(object sender, EventArgs e)
        {
            if (sender is Control c) errProvider.SetError(c, ""); // người dùng đã sửa thì bỏ dấu lỗi
            pnlPreview.Invalidate();
        }

        /// <summary>F6: đưa Form về trạng thái ban đầu.</summary>
        private void ResetForm()
        {
            txtStudentId.Clear();
            txtFullName.Clear();
            cboClass.SelectedIndex = 0;
            cboFaculty.SelectedIndex = 0;
            cboCourse.SelectedIndex = 0;
            cboGender.SelectedIndex = 0;
            dtpBirthDate.Value = DateTime.Today.AddYears(-20);

            picAvatar.Image = null;          // bỏ tham chiếu trước khi Dispose
            _avatar?.Dispose();
            _avatar = null;
            lblImageInfo.Text = "Chưa chọn ảnh";

            _cardColor = Color.FromArgb(232, 240, 250);
            UpdateColorButton();

            errProvider.Clear();
            pnlPreview.Invalidate();
            txtStudentId.Focus();
        }

        // ================= F1: CHỌN ẢNH =================
        private void BtnChooseImage_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog
            {
                Title = "Chọn ảnh đại diện",
                Filter = "Tất cả ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|JPG/JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp",
                CheckFileExists = true
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                LoadAvatar(dlg.FileName);
            }
        }

        /// <summary>
        /// Đọc ảnh qua FileStream rồi sao chép sang Bitmap mới, nhờ đó file gốc không bị khóa
        /// và có thể đổi ảnh nhiều lần.
        /// </summary>
        private void LoadAvatar(string path)
        {
            Bitmap newAvatar;
            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var img = Image.FromStream(fs))
                {
                    newAvatar = new Bitmap(img);
                }
            }
            catch (Exception ex) when (ex is OutOfMemoryException || ex is ArgumentException || ex is IOException
                                       || ex is UnauthorizedAccessException || ex is System.Runtime.InteropServices.ExternalException)
            {
                MessageBox.Show("Không đọc được ảnh. Hãy chọn file ảnh JPG, PNG hoặc BMP hợp lệ.\n" + ex.Message,
                    "Lỗi ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Bitmap old = _avatar;
            _avatar = newAvatar;
            picAvatar.Image = _avatar;
            old?.Dispose();

            errProvider.SetError(btnChooseImage, "");
            lblImageInfo.Text = $"{Path.GetFileName(path)}\n{_avatar.Width} x {_avatar.Height} px";
            pnlPreview.Invalidate();
        }

        // ================= MÀU NỀN THẺ (mở rộng) =================
        private void BtnCardColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog { Color = _cardColor, FullOpen = true })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                _cardColor = dlg.Color;
                UpdateColorButton();
                pnlPreview.Invalidate();
            }
        }

        private void UpdateColorButton()
        {
            btnCardColor.BackColor = _cardColor;
            btnCardColor.ForeColor = CardRenderer.IsLight(_cardColor) ? Color.Black : Color.White;
        }

        // ================= F2: PREVIEW TRÊN PANEL =================
        private void PnlPreview_Paint(object sender, PaintEventArgs e)
        {
            Rectangle area = Rectangle.Inflate(pnlPreview.ClientRectangle, -20, -20);
            if (area.Width < 50 || area.Height < 30) return;

            // Chọn kích thước lớn nhất vừa panel mà vẫn đúng tỉ lệ thẻ, rồi căn giữa
            float ratio = CardRenderer.DesignWidth / CardRenderer.DesignHeight;
            float w = area.Width;
            float h = w / ratio;
            if (h > area.Height)
            {
                h = area.Height;
                w = h * ratio;
            }
            var cardRect = new RectangleF(
                area.X + (area.Width - w) / 2f,
                area.Y + (area.Height - h) / 2f,
                w, h);

            CardRenderer.Draw(e.Graphics, cardRect, BuildStudent(), _cardColor);
        }

        // ================= F3: VALIDATION =================
        /// <summary>Kiểm tra dữ liệu bắt buộc. Trả về false (kèm thông báo) nếu thiếu.</summary>
        private bool ValidateInput()
        {
            errProvider.Clear();

            if (string.IsNullOrWhiteSpace(txtStudentId.Text))
                return Fail(txtStudentId, "Vui lòng nhập mã sinh viên!");

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
                return Fail(txtFullName, "Vui lòng nhập họ tên!");

            if (dtpBirthDate.Value.Date > DateTime.Today)
                return Fail(dtpBirthDate, "Ngày sinh không được ở tương lai!");

            if (_avatar == null)
                return Fail(btnChooseImage, "Vui lòng chọn ảnh đại diện!");

            return true;
        }

        private bool Fail(Control control, string message)
        {
            errProvider.SetError(control, message);
            MessageBox.Show(message, "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }

        // ================= F4, F5: IN ẤN =================
        private bool EnsurePrinterAvailable()
        {
            if (printDoc.PrinterSettings.IsValid) return true;

            MessageBox.Show("Không tìm thấy máy in. Hãy cài đặt một máy in (ví dụ Microsoft Print to PDF) rồi thử lại.",
                "Không có máy in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private void BtnPrintPreview_Click(object sender, EventArgs e)
        {
            if (!ValidateInput() || !EnsurePrinterAvailable()) return;

            try
            {
                using (var dlg = new PrintPreviewDialog
                {
                    Document = printDoc,
                    UseAntiAlias = true,
                    Width = 900,
                    Height = 700,
                    StartPosition = FormStartPosition.CenterParent
                })
                {
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex) when (ex is InvalidPrinterException || ex is Win32Exception)
            {
                MessageBox.Show("Không mở được bản xem trước in.\n" + ex.Message, "Lỗi in ấn",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (!ValidateInput() || !EnsurePrinterAvailable()) return;

            using (var dlg = new PrintDialog { Document = printDoc, UseEXDialog = true })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    printDoc.Print();
                }
                catch (Exception ex) when (ex is InvalidPrinterException || ex is Win32Exception)
                {
                    MessageBox.Show("In không thành công.\n" + ex.Message, "Lỗi in ấn",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>Nội dung trang in: vẽ thẻ ở giữa phía trên trang, giữ đúng tỉ lệ.</summary>
        private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Đơn vị của e.Graphics khi in là 1/100 inch: thẻ rộng 400 = 4 inch (~10,2 cm)
            float cardW = Math.Min(400f, e.MarginBounds.Width);
            float cardH = cardW * CardRenderer.DesignHeight / CardRenderer.DesignWidth;
            float x = e.MarginBounds.Left + (e.MarginBounds.Width - cardW) / 2f;
            float y = e.MarginBounds.Top;

            CardRenderer.Draw(e.Graphics, new RectangleF(x, y, cardW, cardH), BuildStudent(), _cardColor);
            e.HasMorePages = false;
        }

        // ================= LƯU PNG (mở rộng) =================
        private void BtnSavePng_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            using (var dlg = new SaveFileDialog
            {
                Title = "Lưu thẻ sinh viên",
                Filter = "PNG (*.png)|*.png",
                FileName = "the_" + txtStudentId.Text.Trim() + ".png"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    const int width = 1200; // 3 lần không gian thiết kế
                    int height = (int)(width * CardRenderer.DesignHeight / CardRenderer.DesignWidth);
                    using (var bmp = new Bitmap(width, height))
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.White);
                        CardRenderer.Draw(g, new RectangleF(0, 0, width, height), BuildStudent(), _cardColor);
                        bmp.Save(dlg.FileName, ImageFormat.Png);
                    }
                    MessageBox.Show("Đã lưu thẻ thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException
                                           || ex is System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show("Không lưu được ảnh.\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
