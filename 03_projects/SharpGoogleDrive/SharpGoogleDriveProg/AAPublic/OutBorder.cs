using System.Collections.Generic;
using SharpGoogleDriveProg.Service;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;

namespace SharpGoogleDriveProg.AAPublic;

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