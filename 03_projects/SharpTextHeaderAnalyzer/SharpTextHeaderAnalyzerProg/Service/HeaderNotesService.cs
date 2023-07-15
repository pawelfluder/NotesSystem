using System.Collections.Generic;
using System.IO;

namespace TextHeaderAnalyzerCoreProj.Service
{
    public partial class HeaderNotesService
    {
        private readonly FirstWorker firstWorker;
        private readonly TupleElementWorker elementWorker;

        public HeaderNotesService()
        {
            firstWorker = new FirstWorker();
            elementWorker = new TupleElementWorker();
        }

        public List<INotesContainer> AnalyzeFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            var subHeaders = AnalyzeTextLines(lines);
            return subHeaders;
        }

        public List<INotesContainer> AnalyzeTextLines(string[] textLines)
        {
            var subHeaders = firstWorker.FindHeaders(textLines);
            return subHeaders;
        }

        public List<(ElementType type, int level, string text)> GetElements(string[] lines)
        {
            var elements = elementWorker.GetElements(lines);
            return elements;
        }

        public List<(string type, int level, string text)> GetElements2(string[] lines)
        {
            var elements = elementWorker.GetElements2(lines);
            return elements;
        }
    }
}