using SharpGoogleSheetProg.Service;
using System.Collections.Generic;

namespace SharpGoogleSheetProg.AAPublic
{
    public class OutBorder
    {
        public static IGoogleSheetService GoogleSheetService(
            Dictionary<string, object> settingsDict)
        {
            var googleDocsService = new GoogleSheetService(settingsDict);
            return googleDocsService;
        }
    }
}
