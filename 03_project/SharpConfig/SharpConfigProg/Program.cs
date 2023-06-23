using SharpConfigProg.Preparer;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;

namespace SharpPrepareConfigProg
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileService = new FileService();
            var service = new ConfigService(fileService);
            service.Prepare(typeof(IPreparer.IWinder));
        }
    }
}