using Google.Apis.Services;
using SharpGoogleDocsProg.Worker;

namespace SharpGoogleDocsProg.AAPublic
{
    public interface IGoogleDocsService
    {
        StackWorker StackWkr { get; }
        void OverrideSettings(Dictionary<string, object> settingDict);

        // todo - can this be removed?
        BaseClientService.Initializer GetInitilizer(string clientId, string clientSecret);
        void Initialize(string clientId, string clientSecret);
    }
}