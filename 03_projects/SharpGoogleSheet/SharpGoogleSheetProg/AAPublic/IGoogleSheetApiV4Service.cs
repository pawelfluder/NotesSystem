using SharpGoogleSheetProg.Service;
using System.Collections.Generic;

namespace SharpGoogleSheetProg.AAPublic
{
    public interface IGoogleSheetService
    {
        void OverrideSettings(Dictionary<string, object> settingDict);
        public SheetWorker Worker { get; }
    }
}
