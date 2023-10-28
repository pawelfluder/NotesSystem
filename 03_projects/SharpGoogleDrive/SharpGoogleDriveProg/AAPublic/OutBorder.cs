using SharpGoogleDriveProg.Service;

namespace SharpGoogleDriveProg.AAPublic
{
    public class OutBorder
    {
        public static IGoogleDriveService GoogleDriveService(
            Dictionary<string, object> settingsDict)
        {
            var googleDocsService = new GoogleDriveService(settingsDict);
            return googleDocsService;
        }
    }
}
