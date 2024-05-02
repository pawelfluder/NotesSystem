using SharpConfigProg.AAPublic;
using SharpSetup21ProgPrivate.Preparer;
using OutBorder01 = SharpFileServiceProg.AAPublic.OutBorder;

namespace SharpSetup01Prog.AAPublic
{
    public static class OutBorder
    {
        public static IPreparer GetPreparer(string name)
        {
            if (name == typeof(DefaultPreparer).Name)
            {
                var fileService = OutBorder01.FileService();
                var preparer = new DefaultPreparer(fileService);
                return preparer;
            }

            return default;
        }
    }
}