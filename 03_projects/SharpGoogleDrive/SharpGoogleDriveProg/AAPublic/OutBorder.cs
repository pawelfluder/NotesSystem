using SharpFileServiceProg.Service;
using SharpGoogleDriveProg.Service;
using SharpOperationsProg.AAPublic;

namespace SharpGoogleDriveProg.AAPublic
{
    public class OutBorder
    {
        public static IGoogleDriveService GoogleDriveService(
            Dictionary<string, object> settingsDict,
            IOperationsService fileService)
        {
            var googleDocsService = new GoogleDriveService(settingsDict, fileService);
            return googleDocsService;
        }
    }
}
