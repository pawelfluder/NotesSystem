using GoogleDocsServiceProj.Service;

namespace SharpGoogleDocsProg.AAPublic
{
    public class OutBorder
    {
        public static IGoogleDocsService GoogleDocsService(
            Dictionary<string, object> settingsDict)
        {
            var googleDocsService = new GoogleDocsService(settingsDict);
            return googleDocsService;
        }
    }
}
