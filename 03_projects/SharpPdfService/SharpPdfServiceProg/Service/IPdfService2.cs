using PdfService.GridWorker;
using System.Collections.Generic;

namespace PdfService.PdfService
{
   public interface IPdfService2
    {
       bool Export(
           List<(string type, int level, string text)> rows,
           string outputPath);
        bool Open(string path);
        void RunPrinter(string pdfFilePath);
    }
}
