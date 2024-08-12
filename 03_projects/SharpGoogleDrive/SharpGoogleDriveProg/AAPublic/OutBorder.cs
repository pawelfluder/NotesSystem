using SharpFileServiceProg.Service;
using SharpGoogleDriveProg.Service;

namespace SharpGoogleDriveProg.AAPublic
{
    public class OutBorder
    {
        public static IGoogleDriveService GoogleDriveService(
            Dictionary<string, object> settingsDict,
            IFileService fileService)
        {
            var googleDocsService = new GoogleDriveService(settingsDict, fileService);
            return googleDocsService;
        }
    }
}
