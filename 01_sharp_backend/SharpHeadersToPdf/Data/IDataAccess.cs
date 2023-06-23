using System.Collections.Generic;
using PdfService.GridWorker;
using TextHeaderAnalyzerFrameProj;

namespace PdfService.Data
{
    public interface IDataAccess
    {
        List<IRow> GetRows(Header header);

        List<IRow> GetDummyRows(int num);
    }
}