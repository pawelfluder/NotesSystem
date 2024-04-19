using System.Collections.Generic;

namespace PdfService.GridWorker
{
   public class Row : IRow
   {
      public string Data { get; set; }
      public bool IsHeader { get; set; }
      public int Level { get; set; }

        public Row()
        {

        }

        public Row(string data,
            bool isHeader,
            int level)
        {
            Data = data;
            IsHeader = isHeader;
            level = level;
        }
    }
}
