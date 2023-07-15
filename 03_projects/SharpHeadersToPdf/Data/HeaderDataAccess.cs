using System;
using System.Collections.Generic;
using System.Linq;
using PdfService.Data;
using PdfService.GridWorker;
using TextHeaderAnalyzerCoreProj;
using TextHeaderAnalyzerFrameProj;

namespace PdfServiceCoreProj.Data
{
    public class HeaderDataAccess : IDataAccess
    {
        private readonly HeaderPrinter printer;

        public HeaderDataAccess()
        {
            printer = new HeaderPrinter();
        }


        public List<IRow> GetRows(Header header)
        {
            var rows = GetRowsFromHeaders(header, 1).ToList();
            return rows;
        }



        public List<IRow> GetDummyRows(int num)
        {
            Header header = null;


            if (num == 1)
            {
                header = GetDummy1();
            }
            else if (num == 2)
            {
                header = GetDummy2();
            }
            else if (num == 3)
            {
                header = GetDummy4();
            }
            else if (num == 4)
            {
                header = GetDummy4();
            }

            var rows = GetRowsFromHeaders(header, 1).ToList();
            return rows;
        }

        private Header GetDummy3()
        {
            var header1 = GetDummy2();
            var header1111 = new Header("Header.1.1.1.1", "Line.H.1.1.1.1.L.1");

            var text = printer.PrintToString(header1.SubHeaders.Select(x => x as Header));

            header1.AddSubHeaderByPosition(header1111, 1, 1, 1, 1);

            return header1;
        }

        private Header GetDummy4()
        {
            var header1 = GetDummy3();

            var text = printer.PrintToString(header1.SubHeaders.Select(x => x as Header));

            var header1111 = new Header("Header.1.1.1.1.1", "Line.H.1.1.1.1.1.L.1");
            header1.AddSubHeaderByPosition(header1111, 1, 1, 1 ,1, 1);

            return header1;
        }

        private Header GetDummy2()
        {
            // Header1
            var header1 = new Header("Header.1", "Line.H.1.L.1");

            // Header11
            var header11 = new Header("Header.1.1", "Line.H.1.1.L.1", "Line.H.1.1.L.2");
            header1.AddSubHeader(header11);

            // Header111
            var header111 = new Header("Header.1.1.1", "Line.H.1.1.1.L.1");
            header11.AddSubHeader(header111);
            
            // Header12
            var header12 = new Header("Header.1.2", "Line.H.1.2.L.1", "Line.H.1.2.L.2");
            header1.AddSubHeader(header12);

            // Header112
            var header112 = new Header("Header.1.1.2", "Line.H.1.1.2.L.1", "Line.H.1.1.2.L.2");

            return header1;
        }

        private Header GetDummy1()
        {
            // Header1
            var header1 = new Header("Header.1", "Line.H.1.L.1");
            var lineText = "Line.H.1.L.";
            var max = 43;
            var lines = new List<string>();

            for (int i = 1; i <= max; i++)
            {
                lines.Add(lineText + i);
            }

            header1.AddContent(lines);
            return header1;
        }

        private IEnumerable<IRow> GetRowsFromHeaders(INotesContainer container, int level)
        {
            var rows = new List<IRow>();
            var row1 = GetRowFromHeader(container, level);
            rows.AddRange(row1);

            if (container is Header header)
            {
                for (int i = 0; i < header.Content.Count; i++)
                {
                    var content = header.Content[i];
                    var contRow = GetRowFronContent(content, level);
                    rows.Add(contRow);
                }

                foreach (var subHeader in header.SubHeaders)
                {
                    var subRows = GetRowsFromHeaders(subHeader, level + 1);
                    rows.AddRange(subRows);
                }
            }

            return rows;
        }

        private List<IRow> GetRowFromHeader(INotesContainer container, int level)
        {
            var rows = new List<IRow>();

            if (container is Header header)
            {
                var headerRow = new Row();

                headerRow.Data = (header.Name);
                headerRow.IsHeader = true;
                headerRow.Level = level;
                rows.Add(headerRow);
            }
            else if(container is NotesLines notesLines)
            {
                rows.AddRange(notesLines.Lines.Select(x => new Row(x, false, level+1)));
            }

            return rows;
        }

        private IRow GetRowFronContent(string content, int level)
        {
            var headerRow = new Row();
            headerRow.Data = (content);
            headerRow.IsHeader = false;
            headerRow.Level = level;

            return headerRow;
        }
    }
}