namespace COMP1019_Lab08_SimplePaint
{
    /// <summary>Panel bật DoubleBuffered để vẽ không bị nhấp nháy.</summary>
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            DoubleBuffered = true;
            ResizeRedraw = true; // resize thì tự vẽ lại
        }
    }
}
