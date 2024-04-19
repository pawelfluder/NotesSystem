using SharpGoogleDriveProg.Service;

namespace SharpGoogleDriveProg.AAPublic
{
    public interface IGoogleDriveService
    {
        void OverrideSettings(Dictionary<string, object> settingDict);
        public DriveWorker Worker { get; }
    }
}
