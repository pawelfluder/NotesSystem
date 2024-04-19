using PdfService.PdfService;
using SharpPdfServiceProg.Service;

namespace SharpPdfServiceProg.Repetition
{
    public class OutBorder
    {
        public static IPdfService2 PdfService()
        {
            return new PdfExecutor2();
        }
    }
}
