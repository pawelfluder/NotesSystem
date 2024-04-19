using SharpGoogleDocsProg.Worker;

namespace SharpGoogleDocsProg.AAPublic
{
    public interface IGoogleDocsService
    {
        StackWorker StackWkr { get; }
        void Initialize();
        void OverrideSettings(Dictionary<string, object> settingDict);
    }
}