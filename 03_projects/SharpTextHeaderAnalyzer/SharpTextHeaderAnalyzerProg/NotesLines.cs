using System.Collections.Generic;

namespace TextHeaderAnalyzerCoreProj;

public class NotesLines : INotesContainer
{
    public List<string> Lines { get; }

    public NotesLines(List<string> lines)
    {
        Lines = lines;
    }
}