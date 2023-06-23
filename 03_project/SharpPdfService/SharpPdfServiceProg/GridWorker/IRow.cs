using System.Collections.Generic;

namespace PdfService.GridWorker
{
   public interface IRow
    {
        string Data
        {
            get;
            set;
        }

        bool IsHeader
        {
           get;
           set;
        }

        int Level
        {
           get;
           set;
        }
   }
}
