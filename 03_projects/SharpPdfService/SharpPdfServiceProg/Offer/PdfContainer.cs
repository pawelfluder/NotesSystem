using System;
using PdfService.Worker;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;

namespace PdfService.Offer
{
   public class PdfContainer
    {
        private readonly PdfDocument document;

        public PdfContainer(PdfDocument document)
        {            
            document.Info.CreationDate = DateTime.Now;
            GenerationTime = document.Info.CreationDate;
            document.Info.Author = PdfOfferParameters.Author;

            this.document = document;
        }

        public DateTime GenerationTime
        {
            get;
        }

        public PdfOfferParameters Parameters
        {
            get;
            set;
        }

        public XTextFormatter Txt
        {
            get;
            private set;
        }

        public XGraphics Gfx
        {
            get;
            private set;
        }

        public PdfPage Page
        {
            get;
            private set;
        }

        public int CurrentPage { get; private set; }

        public int CurrentRow { get; set; }

        private void CreatePage()
        {
            Page = document.AddPage();
            Page.Size = PageSize.A4;
            Page.Orientation = PageOrientation.Landscape;
            Gfx = XGraphics.FromPdfPage(Page);
            Txt = new XTextFormatter(Gfx);
        }

        public void NewPage()
         {
               CreatePage();
               CurrentPage++;

               var footerWorker = new FooterWorker();
               footerWorker.Generate(this);
         }
   }
}
