using System.Drawing.Drawing2D;

namespace COMP1019_Lab09_StudentCard
{
    /// <summary>
    /// Vẽ thẻ sinh viên bằng GDI+. Dùng chung cho Panel preview, PrintDocument và lưu PNG.
    /// Thẻ được thiết kế trong "không gian thiết kế" 400 x 252 rồi phóng to/thu nhỏ bằng
    /// ScaleTransform nên cùng một đoạn code vẽ đúng ở mọi kích thước (màn hình, máy in).
    /// </summary>
    public static class CardRenderer
    {
        public const float DesignWidth = 400f;
        public const float DesignHeight = 252f;

        private static readonly Color Accent = Color.FromArgb(31, 78, 121);

        /// <summary>Vẽ thẻ vào vùng bounds của Graphics g.</summary>
        public static void Draw(Graphics g, RectangleF bounds, StudentInfo student, Color cardColor)
        {
            GraphicsState state = g.Save();
            try
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                float scale = bounds.Width / DesignWidth;
                g.TranslateTransform(bounds.X, bounds.Y);
                g.ScaleTransform(scale, scale);

                DrawCard(g, student, cardColor);
            }
            finally
            {
                g.Restore(state);
            }
        }

        /// <summary>Màu sáng hay tối theo độ sáng cảm nhận (dùng để chọn màu chữ dễ đọc).</summary>
        public static bool IsLight(Color c)
        {
            double luminance = (0.299 * c.R + 0.587 * c.G + 0.114 * c.B) / 255.0;
            return luminance > 0.55;
        }

        private static void DrawCard(Graphics g, StudentInfo s, Color cardColor)
        {
            var cardRect = new RectangleF(1.5f, 1.5f, DesignWidth - 3f, DesignHeight - 3f);
            bool lightCard = IsLight(cardColor);
            Color textColor = lightCard ? Color.FromArgb(30, 30, 30) : Color.White;
            Color labelColor = lightCard ? Color.FromArgb(90, 90, 90) : Color.FromArgb(210, 210, 210);

            using (GraphicsPath path = RoundedRect(cardRect, 14f))
            {
                // Nền thẻ: gradient nhẹ từ màu sáng hơn xuống màu thẻ
                using (var bg = new LinearGradientBrush(cardRect, Blend(cardColor, Color.White, 0.55f), cardColor, LinearGradientMode.Vertical))
                    g.FillPath(bg, path);

                // Dải tiêu đề (cắt theo bo góc của thẻ)
                GraphicsState st = g.Save();
                g.SetClip(path);
                using (var headBrush = new SolidBrush(Accent))
                    g.FillRectangle(headBrush, 0, 0, DesignWidth, 52);
                g.Restore(st);

                // Khung ngoài
                using (var border = new Pen(Accent, 3f))
                    g.DrawPath(border, path);
            }

            DrawHeader(g);
            DrawPhoto(g, s.Avatar);
            DrawInfo(g, s, textColor, labelColor);
            DrawFooter(g, labelColor);
            DrawFakeQr(g, new RectangleF(352, 205, 36, 36), s.StudentId);
        }

        private static void DrawHeader(Graphics g)
        {
            using (var small = new Font("Segoe UI", 10f, FontStyle.Regular, GraphicsUnit.Pixel))
            using (var title = new Font("Segoe UI", 18f, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var white = new SolidBrush(Color.White))
            using (var gold = new SolidBrush(Color.FromArgb(255, 224, 130)))
            using (var sf = new StringFormat { Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap })
            {
                g.DrawString("TRƯỜNG ĐẠI HỌC SƯ PHẠM TP. HỒ CHÍ MINH", small, white, new RectangleF(0, 7, DesignWidth, 16), sf);
                g.DrawString("THẺ SINH VIÊN", title, gold, new RectangleF(0, 22, DesignWidth, 26), sf);
            }
        }

        /// <summary>Vẽ ảnh trong khung 100x130, giữ đúng tỉ lệ (không méo), căn giữa.</summary>
        private static void DrawPhoto(Graphics g, Image avatar)
        {
            var photoRect = new RectangleF(18, 66, 100, 130);

            g.FillRectangle(Brushes.White, photoRect);

            if (avatar != null && avatar.Width > 0 && avatar.Height > 0)
            {
                float ratio = Math.Min(photoRect.Width / avatar.Width, photoRect.Height / avatar.Height);
                float w = avatar.Width * ratio;
                float h = avatar.Height * ratio;
                g.DrawImage(avatar,
                    photoRect.X + (photoRect.Width - w) / 2f,
                    photoRect.Y + (photoRect.Height - h) / 2f,
                    w, h);
            }
            else
            {
                using (var f = new Font("Segoe UI", 12f, FontStyle.Italic, GraphicsUnit.Pixel))
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString("Ảnh 3x4", f, Brushes.Gray, photoRect, sf);
            }

            using (var pen = new Pen(Color.FromArgb(120, 120, 120), 1f))
                g.DrawRectangle(pen, photoRect.X, photoRect.Y, photoRect.Width, photoRect.Height);
        }

        private static void DrawInfo(Graphics g, StudentInfo s, Color textColor, Color labelColor)
        {
            var rows = new (string Label, string Value, bool Bold)[]
            {
                ("Mã SV:", s.StudentId, true),
                ("Họ tên:", s.FullName.ToUpper(), true),
                ("Ngày sinh:", s.BirthDate.ToString("dd/MM/yyyy"), false),
                ("Giới tính:", s.Gender, false),
                ("Lớp:", s.ClassName, false),
                ("Khoa:", s.Faculty, false),
                ("Khóa:", s.Course, false),
            };

            using (var labelFont = new Font("Segoe UI", 11f, FontStyle.Regular, GraphicsUnit.Pixel))
            using (var valueFont = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel))
            using (var boldFont = new Font("Segoe UI", 12f, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var labelBrush = new SolidBrush(labelColor))
            using (var textBrush = new SolidBrush(textColor))
            using (var sf = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap })
            {
                float y = 64f;
                foreach (var row in rows)
                {
                    g.DrawString(row.Label, labelFont, labelBrush, new RectangleF(132, y + 1, 64, 18), sf);
                    g.DrawString(row.Value, row.Bold ? boldFont : valueFont, textBrush, new RectangleF(198, y, 190, 18), sf);
                    y += 19f;
                }
            }
        }

        private static void DrawFooter(Graphics g, Color labelColor)
        {
            using (var f = new Font("Segoe UI", 10f, FontStyle.Italic, GraphicsUnit.Pixel))
            using (var b = new SolidBrush(labelColor))
                g.DrawString("Sinh viên xuất trình thẻ khi được yêu cầu.", f, b, new RectangleF(18, 214, 320, 16));
        }

        /// <summary>Mã QR giả lập: lưới 9x9 ô vuông, sinh từ mã sinh viên (cùng mã cho cùng hình).</summary>
        private static void DrawFakeQr(Graphics g, RectangleF r, string seedText)
        {
            const int n = 9;
            float cell = r.Width / n;

            using (var back = new SolidBrush(Color.White))
                g.FillRectangle(back, r.X - 2, r.Y - 2, r.Width + 4, r.Height + 4);

            int seed = 17;
            foreach (char c in seedText ?? "")
                seed = unchecked(seed * 31 + c);
            var rnd = new Random(seed);

            using (var black = new SolidBrush(Color.Black))
            {
                for (int row = 0; row < n; row++)
                {
                    for (int col = 0; col < n; col++)
                    {
                        bool inFinder = (row < 3 && col < 3) || (row < 3 && col >= n - 3) || (row >= n - 3 && col < 3);
                        bool on;
                        if (inFinder)
                        {
                            int lr = row < 3 ? row : row - (n - 3);
                            int lc = col < 3 ? col : col - (n - 3);
                            on = !(lr == 1 && lc == 1); // vòng vuông có lỗ giữa
                        }
                        else
                        {
                            on = rnd.Next(2) == 0;
                        }
                        if (on)
                            g.FillRectangle(black, r.X + col * cell, r.Y + row * cell, cell, cell);
                    }
                }
            }
        }

        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = radius * 2f;
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        /// <summary>Trộn màu a với màu b theo tỉ lệ amount (0 = a, 1 = b).</summary>
        private static Color Blend(Color a, Color b, float amount)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * amount),
                (int)(a.G + (b.G - a.G) * amount),
                (int)(a.B + (b.B - a.B) * amount));
        }
    }
}
