using SharpSetup01Prog.AAPublic;

namespace SharpSetup01Prog
{
    class Program
    {
        static void Main(string[] args)
        {
            var preparer = OutBorder.GetPreparer("PrivateNotesPreparer");
            preparer.Prepare();
        }
    }
}