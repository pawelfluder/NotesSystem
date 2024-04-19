using System.Globalization;
using PdfService.Offer;
using PdfSharpCore.Drawing;

namespace PdfService.Worker
{
    public class FooterWorker
    {
        private readonly XFont footerFont = new XFont(PdfOfferParameters.FamilyName, 8, XFontStyle.Regular);
        private readonly XFont footerFontBold = new XFont(PdfOfferParameters.FamilyName, 8, XFontStyle.Bold);

        public void Generate(PdfContainer pdfContainer)
        {
            //AddFooter(pdfContainer);
        }

        private void AddFooter(PdfContainer pdfContainer)
        {
            PdfOfferParameters parameters = pdfContainer.Parameters;
            var top = parameters.TitlePage.TopMargin;
            var generationTime = pdfContainer.GenerationTime;
            var currentPage = pdfContainer.CurrentPage;

            pdfContainer.Gfx.DrawRectangle(XBrushes.LightGray, parameters.DataPage.Left, parameters.DataPage.Bottom - 12, parameters.DataPage.Width, 12);
            pdfContainer.Gfx.DrawString($"Generation time: {generationTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)}", footerFont,
                XBrushes.Black, parameters.DataPage.Left + 2, parameters.DataPage.Bottom - 10, XStringFormats.TopLeft);

            pdfContainer.Gfx.DrawString(string.Join(" ", parameters.TitlePage.CompanyName), footerFontBold, XBrushes.Black,
                parameters.DataPage.Left + (parameters.DataPage.Width / 2) - 100, parameters.DataPage.Bottom - 10, XStringFormats.TopLeft);

            pdfContainer.Gfx.DrawString(
                $"{currentPage} of {(pdfContainer.CurrentPage * 2 / parameters.DataPage.RowsPerPage) + 1}",
                footerFont, XBrushes.Black, parameters.DataPage.Right - 50, parameters.DataPage.Bottom - 10, XStringFormats.TopLeft);
        }
    }
}
