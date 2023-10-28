using SharpConfigProg.Service;

namespace SharpConfigProg.ConfigPreparer
{
    public interface IPreparer
    {
        Dictionary<string, object> Prepare();
        void SetConfigService(IConfigService configService);
    }
}