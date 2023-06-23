using PdfService.GridWorker;
using System.Collections.Generic;

namespace PdfService.PdfService
{
   public interface IPdfService
    {
       bool Export(List<IRow> rows, string outputPath);
    }
}
