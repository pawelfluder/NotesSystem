using System;
using System.Collections.Generic;
using System.Text;

namespace TextHeaderAnalyzerCoreProj
{
    public class NotesLines : INotesContainer
    {
        public List<string> Lines { get; }

        public NotesLines(List<string> lines)
        {
            Lines = lines;
        }
    }
}
