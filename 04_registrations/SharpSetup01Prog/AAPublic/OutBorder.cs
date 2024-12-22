using SharpConfigProg.AAPublic;
using SharpSetup01Prog.Preparer;

namespace SharpSetup01Prog.AAPublic
{
    public static class OutBorder
    {
        public static IPreparer GetPreparer(string name)
        {
            if (name == typeof(DefaultPreparer).Name)
            {
                DefaultPreparer preparer = new();
                return preparer;
            }

            return default;
        }
    }
}
