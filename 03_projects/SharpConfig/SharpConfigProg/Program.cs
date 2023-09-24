using SharpConfigProg.Service;
using Border1 = SharpFileServiceProg.Repetition.OutBorder;

namespace SharpPrepareConfigProg
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileService = Border1.FileService();
            var service = new ConfigService(fileService);
            service.Prepare(typeof(IConfigService.IWinderPreparer));
        }
    }
}