using SharpFileServiceProg.AAPublic;
using SharpFileServiceProg.Yaml;

namespace SharpFileServiceProg.Service;

public class FileService
{
    public IYamlWrk Yaml {get; set;}
    
    public FileService()
    {
        Yaml = new YamlWorker();
    }
}