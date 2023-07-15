using PdfSharpCore.Drawing;

namespace PdfService2CoreProj.Info
{
    public class MarginInfo
    {
        public double LineBoxHeight => 12;

        public double TextSize => 10;

        public XColor LineColor => XColor.FromKnownColor(XKnownColor.Gray);

        public XBrush TextBrush => XBrushes.Black;

        public string FontName => "Calibri";

        public double LineSize => 1.0;

        public double LevelSize => 8.0;

        public double MaxHeight => 500;

        public double MarginLeft => 40.0;

        public double MarginRight => 40.0;

        public double MarginTop => 40.0;

        public double MarginBottom => 40.0;
    }
}
