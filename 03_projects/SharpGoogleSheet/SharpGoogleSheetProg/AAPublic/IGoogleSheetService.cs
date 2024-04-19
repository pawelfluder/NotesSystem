using SharpGoogleSheetProg.Service;

namespace SharpGoogleSheetProg.AAPublic
{
    public interface IGoogleSheetService
    {
        SheetWorker Worker { get; }
        void Initialize();
        void OverrideSettings(Dictionary<string, object> settingDict);
    }
}