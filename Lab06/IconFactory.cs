using System.Drawing.Drawing2D;

namespace ProductManager;

/// <summary>Vẽ icon cho ToolStrip bằng GDI+ (không cần file ảnh/resource).</summary>
public static class IconFactory
{
    private static readonly Color Green = Color.FromArgb(22, 163, 74);
    private static readonly Color Blue = Color.FromArgb(37, 99, 235);
    private static readonly Color Red = Color.FromArgb(220, 38, 38);

    private static Bitmap Draw(Action<Graphics> paint)
    {
        var bmp = new Bitmap(32, 32);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        paint(g);
        return bmp;
    }

    public static Bitmap Add() => Draw(g =>
    {
        using var br = new SolidBrush(Green);
        g.FillRectangle(br, 12, 4, 8, 24);
        g.FillRectangle(br, 4, 12, 24, 8);
    });

    public static Bitmap Edit() => Draw(g =>
    {
        using var body = new Pen(Blue, 7) { StartCap = LineCap.Flat, EndCap = LineCap.Flat };
        g.DrawLine(body, 12, 20, 24, 8);
        using var eraser = new Pen(Color.FromArgb(96, 165, 250), 7) { StartCap = LineCap.Flat, EndCap = LineCap.Flat };
        g.DrawLine(eraser, 24, 8, 27.5f, 4.5f);
        using var br = new SolidBrush(Blue);
        g.FillPolygon(br, new[] { new PointF(4, 28), new PointF(9.5f, 17.5f), new PointF(14.5f, 22.5f) });
    });

    public static Bitmap Delete() => Draw(g =>
    {
        using var br = new SolidBrush(Red);
        g.FillRectangle(br, 12, 3, 8, 4);          // quai thùng rác
        g.FillRectangle(br, 5, 7, 22, 4);          // nắp
        g.FillPolygon(br, new[] { new PointF(7, 13), new PointF(25, 13), new PointF(23, 29), new PointF(9, 29) });
        using var white = new Pen(Color.White, 2);
        foreach (int x in new[] { 12, 16, 20 }) g.DrawLine(white, x, 16, x, 26);
    });

    public static Bitmap Refresh() => Draw(g =>
    {
        using var pen = new Pen(Blue, 4);
        g.DrawArc(pen, 6, 6, 20, 20, 40, 270);

        // đầu mũi tên ở cuối cung (góc 310°, chiều kim đồng hồ)
        double a = 310 * Math.PI / 180;
        var p = new PointF(16 + 10 * (float)Math.Cos(a), 16 + 10 * (float)Math.Sin(a));
        var t = new PointF(-(float)Math.Sin(a), (float)Math.Cos(a));   // tiếp tuyến
        var n = new PointF(-t.Y, t.X);                                  // pháp tuyến
        using var br = new SolidBrush(Blue);
        g.FillPolygon(br, new[]
        {
            new PointF(p.X + t.X * 7, p.Y + t.Y * 7),
            new PointF(p.X + n.X * 6, p.Y + n.Y * 6),
            new PointF(p.X - n.X * 6, p.Y - n.Y * 6)
        });
    });

    public static Bitmap Search() => Draw(g =>
    {
        using var ring = new Pen(Blue, 3.5f);
        g.DrawEllipse(ring, 4, 4, 16, 16);
        using var handle = new Pen(Blue, 5) { StartCap = LineCap.Round, EndCap = LineCap.Round };
        g.DrawLine(handle, 18, 18, 27, 27);
    });
}
