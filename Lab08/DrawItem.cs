using System.Drawing.Drawing2D;

namespace COMP1019_Lab08_SimplePaint
{
    /// <summary>
    /// Một đối tượng đã vẽ trên canvas. Lưu dữ liệu gốc để có thể vẽ lại bất cứ lúc nào
    /// (resize, bị che, undo...).
    /// </summary>
    public class DrawItem
    {
        public ShapeMode Mode { get; set; }
        public Color Color { get; set; }
        public int Width { get; set; }
        public bool Fill { get; set; }

        /// <summary>Điểm bắt đầu và kết thúc (dùng cho Line, Rectangle, Ellipse).</summary>
        public Point Start { get; set; }
        public Point End { get; set; }

        /// <summary>Danh sách điểm của nét vẽ tự do (Pen).</summary>
        public List<Point> Points { get; } = new List<Point>();

        /// <summary>
        /// Hình chữ nhật bao quanh Start-End, chuẩn hóa để kéo chuột theo mọi hướng đều đúng
        /// (Left/Top luôn là giá trị nhỏ hơn, Width/Height luôn không âm).
        /// </summary>
        public Rectangle GetBounds()
        {
            int left = Math.Min(Start.X, End.X);
            int top = Math.Min(Start.Y, End.Y);
            int right = Math.Max(Start.X, End.X);
            int bottom = Math.Max(Start.Y, End.Y);
            return Rectangle.FromLTRB(left, top, right, bottom);
        }

        /// <summary>Vẽ đối tượng lên Graphics. Pen/Brush tạo ra được giải phóng bằng using.</summary>
        public void Draw(Graphics g)
        {
            using (var pen = new Pen(Color, Width))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                pen.LineJoin = LineJoin.Round;

                switch (Mode)
                {
                    case ShapeMode.Pen:
                        DrawFreehand(g, pen);
                        break;

                    case ShapeMode.Line:
                        g.DrawLine(pen, Start, End);
                        break;

                    case ShapeMode.Rectangle:
                        {
                            Rectangle r = GetBounds();
                            if (Fill)
                            {
                                using (var brush = new SolidBrush(Color))
                                    g.FillRectangle(brush, r);
                            }
                            g.DrawRectangle(pen, r);
                            break;
                        }

                    case ShapeMode.Ellipse:
                        {
                            Rectangle r = GetBounds();
                            if (Fill)
                            {
                                using (var brush = new SolidBrush(Color))
                                    g.FillEllipse(brush, r);
                            }
                            g.DrawEllipse(pen, r);
                            break;
                        }
                }
            }
        }

        private void DrawFreehand(Graphics g, Pen pen)
        {
            if (Points.Count == 0) return;

            if (Points.Count == 1)
            {
                // Chỉ bấm chuột, không kéo: vẽ một chấm tròn
                Point p = Points[0];
                using (var brush = new SolidBrush(Color))
                    g.FillEllipse(brush, p.X - Width / 2f, p.Y - Width / 2f, Width, Width);
                return;
            }

            g.DrawLines(pen, Points.ToArray());
        }
    }
}
