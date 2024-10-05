using SharpRepoServiceProg.Registration;
using System.IO;

namespace SharpRepoServiceProg.WorkersSystem;

internal class SystemWorker
{
    private PathWorker pw;

    public char NewLine => '\n';

    public SystemWorker()
    {
        pw = MyBorder.Container.Resolve<PathWorker>();
    }

    public void CreateDirectoryIfNotExists(
        (string Repo, string Loca) adrTuple)
    {
        var path = pw.GetItemPath(adrTuple);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void CreateDirectoryIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public string[] GetDirectories(string path)
    {
        var result = Directory.GetDirectories(path);
        return result;
    }

    public string[] GetDirectories(
        (string Repo, string Loca) adrTuple)
    {
        var path = pw.GetItemPath(adrTuple);
        var result = Directory.GetDirectories(path);
        return result;
    }
}