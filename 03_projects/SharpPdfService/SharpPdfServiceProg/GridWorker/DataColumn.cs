using PdfSharpCore.Drawing;

namespace PdfService.GridWorker
{
   public class DataColumn
    {
        public int Id
        {
            get;
            set;
        }

        public XUnit Left
        {
            get;
            set;
        }

        public string Header
        {
            get;
            set;
        }
    }
}
