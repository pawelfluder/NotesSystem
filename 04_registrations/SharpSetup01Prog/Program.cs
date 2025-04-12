using SharpConfigProg.AAPublic;
using SharpSetup01Prog.Preparer;
using OutBorder = SharpSetup01Prog.AAPublic.OutBorder;

namespace SharpSetup01Prog
{
    class Program
    {
        static void Main(string[] args)
        {
            IPreparer preparer = OutBorder.GetPreparer(typeof(DefaultPreparer).Name);
            preparer.Prepare();
        }
    }
}
