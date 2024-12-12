using SharpConfigProg.AAPublic;
using SharpFileServiceProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SharpSetup01Prog.Preparer;
using OutBorder01 = SharpFileServiceProg.AAPublic.OutBorder;
using OutBorder02 = SharpOperationsProg.AAPublic.OutBorder;

namespace SharpSetup01Prog.AAPublic
{
    public static class OutBorder
    {
        public static IPreparer GetPreparer(string name)
        {
            if (name == typeof(DefaultPreparer).Name)
            {
                IFileService fileService = OutBorder01.FileService();
                IOperationsService operationsService = OutBorder02.OperationsService(fileService);
                DefaultPreparer preparer = new(operationsService);
                return preparer;
            }

            return default;
        }
    }
}
