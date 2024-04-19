using System.Collections.Generic;
using PdfService.GridWorker;
using PdfService.Offer;
using PdfService2CoreProj.Info;
using PdfSharpCore.Drawing;

namespace PdfService.Worker
{
    public class HeaderGridWorker
    {
        private PdfContainer pdfContainer;

        private double lastPosition;
        private readonly MarginInfo info;

        public HeaderGridWorker()
        {
            info = new MarginInfo();
        }

        public void Generate(List<IRow> rows, PdfContainer pdfContainer)
        {
            this.pdfContainer = pdfContainer;
            lastPosition = 0;
            AddRows(rows);
        }

        private void AddRows(IEnumerable<IRow> rows)
        {
            foreach (var row in rows)
            {
                AddRow(row);
                CheckIfNewPageNeeded();
            }

            var rect = GetNextMainRectangle();
            PrintTopLine(rect);
        }

        private void CheckIfNewPageNeeded()
        {
            if (lastPosition > info.MaxHeight)
            {
                pdfContainer.NewPage();
                lastPosition = 0;
            }
        }

        private void AddRow(IRow row)
        {
            var outerBorderRect = GetNextMainRectangle();

            //PrintRect(outerBorderRect, XColor.FromKnownColor(XKnownColor.Gray));

            if (row.IsHeader)
            {
                var borderRect = ShrinkForLevel(outerBorderRect, row.Level);
                PrintRectAndOuter(borderRect, outerBorderRect, row.Level);

                var textRect = ShrinkForLevel(outerBorderRect, row.Level);
                PrintText(textRect, row.Data);
            }
            else
            {
                var borderRect = ShrinkForLevel(outerBorderRect, row.Level);
                PrintLinesAndOuter(borderRect, outerBorderRect, row.Level);

                var textRect = ShrinkForLevel(outerBorderRect, row.Level + 1);
                PrintText(textRect, row.Data);
            }

            lastPosition += outerBorderRect.Height;
        }

        private XRect ShrinkForLevel(XRect input, int level)
        {
            var space = info.LevelSize;
            var diff = (level - 1) * space;
            var topLeft = new XPoint(input.TopLeft.X + diff, input.TopLeft.Y);
            var rect = new XRect(topLeft, input.BottomRight);
            return rect;
        }

        private XRect GetNextMainRectangle()
        {
            var marginLeft = 0.5 + info.MarginLeft;
            var marginTop = 0.5 + info.MarginTop;
            var mariginRight = info.MarginRight;

            var width = 841.0;
            var height = 12;

            var aX = marginLeft;
            var aY = lastPosition + marginTop;

            var bX = aX + width - marginLeft - mariginRight;
            var bY = aY + height;

            var pointA = new XPoint(aX, aY);
            var pointB = new XPoint(bX, bY);
            var rect = new XRect(pointA, pointB);

            return rect;
        }

        private void PrintText(XRect rect, string data)
        {
            var font = new XFont(info.FontName, info.TextSize);
            pdfContainer.Txt.DrawString(data, font, info.TextBrush, rect, XStringFormats.TopLeft);
        }

        private void PrintRectAndOuter(XRect rect, XRect outerBorderRect, int level)
        {
            PrintRect(rect);

            for (int i = 1; i < level; i++)
            {
                var lineRect = ShrinkForLevel(outerBorderRect, i);
                PrintLeftLine(lineRect);
            }
        }

        private void PrintRect(XRect rect)
        {
            var xpen = new XPen(info.LineColor, 1.0);
            pdfContainer.Gfx.DrawRectangle(xpen, rect);
        }

        private void PrintRect(XRect rect, XColor color)
        {
            var xpen = new XPen(color, 1.0);
            pdfContainer.Gfx.DrawRectangle(xpen, rect);
        }

        private void PrintLinesAndOuter(XRect rect, XRect outerBorderRect, int level)
        {
            PrintLines(rect);

            for (int i = 1; i < level; i++)
            {
                var lineRect = ShrinkForLevel(outerBorderRect, i);
                PrintLeftLine(lineRect);
            }
        }

        private void PrintLines(XRect rect)
        {
            var xpen = new XPen(info.LineColor, info.LineSize);

            pdfContainer.Gfx.DrawLine(xpen, rect.TopLeft, rect.BottomLeft);
            pdfContainer.Gfx.DrawLine(xpen, rect.TopRight, rect.BottomRight);
        }

        private void PrintTopLine(XRect rect)
        {
            var xpen = new XPen(info.LineColor, info.LineSize);
            pdfContainer.Gfx.DrawLine(xpen, rect.TopLeft, rect.TopRight);
        }

        private void PrintLeftLine(XRect rect)
        {
            var xpen = new XPen(info.LineColor, info.LineSize);
            pdfContainer.Gfx.DrawLine(xpen, rect.TopLeft, rect.BottomLeft);
        }
    }
}
