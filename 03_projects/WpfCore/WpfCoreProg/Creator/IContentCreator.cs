using System.Collections.Generic;

namespace WpfNotesSystem
{
    public interface IContentCreator
    {
        void CreateRowsAndColls(int jmax, int imax);
        void CreateHeader((int, int) pos, string text, int collSpan);
        void CreateLines((int, int) pos, string line, int collSpan);
        void CreateEmpty((int, int) pos);
    }
}