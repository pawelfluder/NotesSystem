using System.Collections.Generic;
using SharpGoogleDriveProg.Composite;

namespace SharpGoogleDriveProg.AAPublic;

public interface IGoogleDriveService
{
    void OverrideSettings(Dictionary<string, object> settingDict);
    public DriveComposite Composite { get; }
}