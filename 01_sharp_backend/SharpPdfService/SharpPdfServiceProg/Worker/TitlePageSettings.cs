using PdfService.Offer;
using PdfSharpCore.Drawing;

namespace PdfService.Worker
{
   public class TitlePageSettings
    {
        private XUnit bottom = XUnit.Zero;
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

        public string[] CompanyName
        {
            get;
            set;
        }

        public string[] CompanyAddress
        {
            get;
            set;
        }

        public string ImagePath
        {
            get;
            set;
        }

        public string CustomTitle
        {
            get;
            set;
        }

        public string[] CompanyContact
        {
            get;
            set;
        }

        public XUnit Left => left == XUnit.Zero ? (left = XUnit.FromCentimeter(LeftMargin.Centimeter)) : left;

        public XUnit Right => right == XUnit.Zero ? (right = XUnit.FromCentimeter(PdfOfferParameters.PageWidth - RightMargin.Centimeter)) : right;

        public XUnit Top => top == XUnit.Zero ? (top = XUnit.FromCentimeter(TopMargin.Centimeter)) : top;

        public XUnit Bottom => bottom == XUnit.Zero ? (bottom = XUnit.FromCentimeter(PdfOfferParameters.PageHeight - BottomMargin.Centimeter)) : bottom;

        public XUnit Width => width == XUnit.Zero ? (width = XUnit.FromCentimeter(PdfOfferParameters.PageWidth - LeftMargin.Centimeter - RightMargin.Centimeter)) : width;
    }
}
