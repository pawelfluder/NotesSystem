using SharpGoogleDocsProg.Worker;

namespace SharpGoogleDocsProg.AAPublic
{
    public interface IGoogleDocsService
    {
        StackCoposite Stack { get; }
        ExecuteComposite Execute { get; }
        void Initialize();
        void OverrideSettings(Dictionary<string, object> settingDict);
    }
}