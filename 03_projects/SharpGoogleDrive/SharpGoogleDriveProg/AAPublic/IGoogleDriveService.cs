using SharpGoogleDriveProg.Composite;
using SharpGoogleDriveProg.Service;

namespace SharpGoogleDriveProg.AAPublic;

public interface IGoogleDriveService
{
    void OverrideSettings(Dictionary<string, object> settingDict);
    public DriveComposite Composite { get; }
}