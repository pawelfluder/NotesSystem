using SharpGoogleDocsProg.Composits;
using SharpGoogleDocsProg.Worker;

namespace SharpGoogleDocsProg.AAPublic
{
    public interface IGoogleDocsService
    {
        DocumentComposite Document { get; }
        StackCoposite Stack { get; }
        ExecuteComposite Execute { get; }
        void Initialize();
        void OverrideSettings(Dictionary<string, object> settingDict);
    }
}