using SharpConfigProg.AAPublic;
using SharpSetup01Prog.Preparations.Default;

namespace SharpSetup01Prog.AAPublic
{
    public static class OutBorder
    {
        public static IPreparer DefaultPreparer()
        {
            DefaultPreparer preparer = new();
            return preparer;
        }
    }
}
