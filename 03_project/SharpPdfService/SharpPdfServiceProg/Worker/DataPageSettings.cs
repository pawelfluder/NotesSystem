using System.Collections.Generic;
using PdfService.GridWorker;
using PdfService.Offer;
using PdfSharpCore.Drawing;

namespace PdfService.Worker
{
   public class DataPageSettings
    {
        private XUnit bottom = XUnit.Zero;

        private XFont dataFont;
        private XFont headerFont;
        private XUnit left = XUnit.Zero;
        private XUnit right = XUnit.Zero;
        private XUnit top = XUnit.Zero;
        private XUnit width = XUnit.Zero;

        public XUnit LeftMargin
        {
            get;
            set;
        }

        public XUnit RightMargin
        {
            get;
            set;
        }

        public XUnit TopMargin
        {
            get;
            set;
        }

        public XUnit BottomMargin
        {
            get;
            set;
        }

        public int DataRowHeight
        {
            get;
            set;
        }

        public int HeaderRowHeight
        {
            get;
            set;
        }

        public int DataFontSize
        {
            get;
            set;
        }

        public int HeaderFontSize
        {
            get;
            set;
        }

        public int RowsPerPage
        {
            get;
            set;
        }

        public XFontStyle HeaderFontStyle
        {
            get;
            set;
        }

        public List<DataColumn> DataColumns
        {
            get;
            set;
        }

        public XFont DataFont => dataFont ?? (dataFont = new XFont(PdfOfferParameters.FamilyName, DataFontSize, XFontStyle.Regular));

        public XFont HeaderFont => headerFont ?? (headerFont = new XFont(PdfOfferParameters.FamilyName, HeaderFontSize, HeaderFontStyle));

        public XUnit Left => left == XUnit.Zero ? (left = XUnit.FromCentimeter(LeftMargin.Centimeter)) : left;

        public XUnit Right => right == XUnit.Zero ? (right = XUnit.FromCentimeter(PdfOfferParameters.PageWidth - RightMargin.Centimeter)) : right;

        public XUnit Top => top == XUnit.Zero ? (top = XUnit.FromCentimeter(TopMargin.Centimeter)) : top;

        public XUnit Bottom => bottom == XUnit.Zero ? (bottom = XUnit.FromCentimeter(PdfOfferParameters.PageHeight - BottomMargin.Centimeter)) : bottom;

        public XUnit Width => width == XUnit.Zero ? (width = XUnit.FromCentimeter(PdfOfferParameters.PageWidth - LeftMargin.Centimeter - RightMargin.Centimeter)) : width;
    }
}
