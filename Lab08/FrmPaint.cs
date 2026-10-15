using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace COMP1019_Lab08_SimplePaint
{
    /// <summary>Form chính của ứng dụng Paint đơn giản.</summary>
    public class FrmPaint : Form
    {
        // ----- Control -----
        private ComboBox cboMode;
        private NumericUpDown nudWidth;
        private Button btnColor, btnUndo, btnClear, btnBackColor, btnOpenImage, btnSave;
        private CheckBox chkFill;
        private DoubleBufferedPanel canvas;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus, lblCount;

        // ----- Dữ liệu -----
        private readonly List<DrawItem> _items = new List<DrawItem>(); // các nét đã vẽ xong
        private DrawItem _current;           // nét đang vẽ dở (xem trước khi kéo chuột)
        private bool _isDrawing;
        private Color _currentColor = Color.Black;
        private Bitmap _backgroundImage;     // ảnh nền (nâng cao)

        public FrmPaint()
        {
            Text = "COMP1019 - Lab 08 - Simple Paint";
            ClientSize = new Size(1000, 650);
            MinimumSize = new Size(700, 400);
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true; // để bắt Ctrl+Z, Ctrl+S

            BuildUi();
            WireEvents();
            UpdateStatusCount();
        }

        // ================= GIAO DIỆN =================
        private void BuildUi()
        {
            var toolbar = new FlowLayoutPanel
            {
                Name = "pnlTools",
                Dock = DockStyle.Top,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(6, 6, 6, 2),
                BackColor = SystemColors.Control
            };

            cboMode = new ComboBox { Name = "cboMode", DropDownStyle = ComboBoxStyle.DropDownList, Width = 110 };
            cboMode.Items.AddRange(new object[] { "Pen", "Line", "Rectangle", "Ellipse" });
            cboMode.SelectedIndex = 0;

            nudWidth = new NumericUpDown { Name = "nudWidth", Minimum = 1, Maximum = 20, Value = 3, Width = 55 };

            btnColor = MakeButton("btnColor", "Màu vẽ", 85);
            btnColor.BackColor = _currentColor;
            btnColor.ForeColor = Color.White;

            chkFill = new CheckBox { Name = "chkFill", Text = "Tô màu", AutoSize = true, Enabled = false };
            chkFill.Margin = new Padding(6, 7, 6, 0);

            btnBackColor = MakeButton("btnBackColor", "Màu nền", 85);
            btnOpenImage = MakeButton("btnOpenImage", "Mở ảnh nền", 95);
            btnSave = MakeButton("btnSave", "Lưu PNG", 80);
            btnUndo = MakeButton("btnUndo", "Undo", 70);
            btnClear = MakeButton("btnClear", "Clear", 70);

            toolbar.Controls.AddRange(new Control[]
            {
                MakeLabel("Chế độ:"), cboMode,
                MakeLabel("Độ dày:"), nudWidth,
                btnColor, chkFill, btnBackColor, btnOpenImage, btnSave, btnUndo, btnClear
            });

            canvas = new DoubleBufferedPanel
            {
                Name = "canvas",
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Cursor = Cursors.Cross
            };

            statusStrip = new StatusStrip { Name = "statusStrip1" };
            lblStatus = new ToolStripStatusLabel { Name = "lblStatus", Text = "Tọa độ: -" };
            lblCount = new ToolStripStatusLabel { Name = "lblCount", Spring = true, TextAlign = ContentAlignment.MiddleRight };
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus, lblCount });

            // Thứ tự thêm: Fill trước, rồi Top/Bottom để canvas không bị che
            Controls.Add(canvas);
            Controls.Add(toolbar);
            Controls.Add(statusStrip);
        }

        private static Button MakeButton(string name, string text, int width) =>
            new Button { Name = name, Text = text, Width = width, Height = 28 };

        private static Label MakeLabel(string text) =>
            new Label { Text = text, AutoSize = true, Margin = new Padding(8, 8, 2, 0) };

        private void WireEvents()
        {
            canvas.Paint += Canvas_Paint;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.MouseLeave += (s, e) => { if (!_isDrawing) lblStatus.Text = "Tọa độ: -"; };

            cboMode.SelectedIndexChanged += (s, e) =>
                chkFill.Enabled = cboMode.SelectedIndex >= 2; // chỉ Rectangle, Ellipse mới tô màu được

            btnColor.Click += BtnColor_Click;
            btnBackColor.Click += BtnBackColor_Click;
            btnOpenImage.Click += BtnOpenImage_Click;
            btnSave.Click += BtnSave_Click;
            btnUndo.Click += (s, e) => Undo();
            btnClear.Click += BtnClear_Click;

            KeyDown += FrmPaint_KeyDown;
            FormClosed += (s, e) => _backgroundImage?.Dispose();
        }

        // ================= VẼ (sự kiện Paint) =================
        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            RenderScene(e.Graphics);
        }

        /// <summary>
        /// Vẽ toàn bộ cảnh từ dữ liệu: ảnh nền, các nét đã lưu, nét đang vẽ.
        /// Dùng chung cho Paint event và khi lưu PNG.
        /// </summary>
        private void RenderScene(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_backgroundImage != null)
                g.DrawImage(_backgroundImage, 0, 0, _backgroundImage.Width, _backgroundImage.Height);

            foreach (DrawItem item in _items)
                item.Draw(g);

            _current?.Draw(g);
        }

        // ================= CHUỘT =================
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            ShapeMode mode = (ShapeMode)cboMode.SelectedIndex;
            _current = new DrawItem
            {
                Mode = mode,
                Color = _currentColor,
                Width = (int)nudWidth.Value,
                Fill = chkFill.Checked && chkFill.Enabled,
                Start = e.Location,
                End = e.Location
            };
            if (mode == ShapeMode.Pen) _current.Points.Add(e.Location);

            _isDrawing = true;
            canvas.Invalidate();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            lblStatus.Text = $"Tọa độ: {e.X}, {e.Y}";

            if (!_isDrawing || _current == null) return;

            if (_current.Mode == ShapeMode.Pen)
                _current.Points.Add(e.Location);
            else
                _current.End = e.Location;

            canvas.Invalidate();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_isDrawing || _current == null) return;
            _isDrawing = false;

            if (_current.Mode != ShapeMode.Pen)
                _current.End = e.Location;

            // Bỏ qua Line/Rectangle/Ellipse nếu chỉ click mà không kéo
            bool isEmptyShape = _current.Mode != ShapeMode.Pen && _current.Start == _current.End;
            if (!isEmptyShape)
                _items.Add(_current);

            _current = null;
            UpdateStatusCount();
            canvas.Invalidate();
        }

        // ================= NÚT CHỨC NĂNG =================
        private void BtnColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog { Color = _currentColor, FullOpen = true })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    _currentColor = dlg.Color;
                    btnColor.BackColor = _currentColor;
                    // Chữ nút đổi theo độ sáng để dễ đọc
                    btnColor.ForeColor = _currentColor.GetBrightness() > 0.6f ? Color.Black : Color.White;
                }
            }
        }

        private void BtnBackColor_Click(object sender, EventArgs e)
        {
            using (var dlg = new ColorDialog { Color = canvas.BackColor, FullOpen = true })
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    canvas.BackColor = dlg.Color;
                    canvas.Invalidate();
                }
            }
        }

        private void Undo()
        {
            if (_items.Count == 0)
            {
                lblStatus.Text = "Không còn nét vẽ nào để Undo";
                return;
            }
            _items.RemoveAt(_items.Count - 1);
            UpdateStatusCount();
            canvas.Invalidate();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            if (_items.Count == 0) return;

            if (MessageBox.Show("Bạn có chắc muốn xóa toàn bộ nét vẽ?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _items.Clear();
                _current = null;
                UpdateStatusCount();
                canvas.Invalidate();
            }
        }

        private void BtnOpenImage_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog { Filter = "Ảnh (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp", Title = "Chọn ảnh nền" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // Copy sang Bitmap mới để không bị khóa file gốc
                    using (var temp = Image.FromFile(dlg.FileName))
                    {
                        _backgroundImage?.Dispose();
                        _backgroundImage = new Bitmap(temp);
                    }
                    canvas.Invalidate();
                }
                catch (Exception ex) when (ex is OutOfMemoryException || ex is IOException || ex is ArgumentException)
                {
                    MessageBox.Show("Không mở được ảnh. Hãy chọn file ảnh hợp lệ.\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            using (var dlg = new SaveFileDialog { Filter = "PNG (*.png)|*.png", FileName = "paint.png", Title = "Lưu hình vẽ" })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    int w = Math.Max(1, canvas.ClientSize.Width);
                    int h = Math.Max(1, canvas.ClientSize.Height);
                    using (var bmp = new Bitmap(w, h))
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.Clear(canvas.BackColor);
                        RenderScene(g);
                        bmp.Save(dlg.FileName, ImageFormat.Png);
                    }
                    MessageBox.Show("Đã lưu ảnh thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is System.Runtime.InteropServices.ExternalException)
                {
                    MessageBox.Show("Không lưu được ảnh.\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FrmPaint_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                Undo();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                BtnSave_Click(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void UpdateStatusCount()
        {
            lblCount.Text = $"Số nét vẽ: {_items.Count}";
        }
    }
}
