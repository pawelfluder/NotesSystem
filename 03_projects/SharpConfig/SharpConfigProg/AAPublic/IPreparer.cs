using SharpConfigProg.Service;

namespace SharpConfigProg.AAPublic
{
    public interface IPreparer
    {
        Dictionary<string, object> Prepare();
        void SetConfigService(IConfigService configService);
    }
}