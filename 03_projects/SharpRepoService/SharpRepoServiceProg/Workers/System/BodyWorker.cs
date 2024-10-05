using SharpRepoServiceProg.Registration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace SharpRepoServiceProg.WorkersSystem;

internal class BodyWorker
{
    private char newLine = '\n';
    private PathWorker pw;

    public BodyWorker()
    {
        //this.fileService = MyBorder.Container.Resolve<IFileService>();
        pw = MyBorder.Container.Resolve<PathWorker>();
    }

    public void CreateBody(
        (string Repo, string Loca) adrTuple,
        string content)
    {
        var filePath = pw.GetBodyPath(adrTuple);
        File.WriteAllText(filePath, content);

        //var topSpace = GetTopSpace();
        //File.WriteAllText(contentFilePath, topSpace + content);
    }

    private void OverrideTextGenerate(
        (string Repo, string Loca) adrTuple,
        string newContent)
    {
        var contentFilePath = pw.GetBodyPath(adrTuple);
        File.WriteAllText(contentFilePath, newContent);
    }

    private void AppendTextTopGenerate(
        (string Repo, string Loca) adrTuple,
        string content)
    {
        var contentFilePath = pw.GetBodyPath(adrTuple);
        var oldContent = GetText2(adrTuple);
        var newContent = oldContent + newLine + content;
        File.WriteAllText(contentFilePath, newContent);
    }

    public List<string> GetTextLines(
        (string Repo, string Loca) adrTuple)
    {
        var path = pw.GetBodyPath(adrTuple);
        var lines = File.ReadAllLines(path).ToList();
        return lines;
    }

    public string GetText3(
        (string Repo, string Loca) adrTuple)
    {
        var path = pw.GetBodyPath(adrTuple);
        //var lines = File.ReadAllLines(path).Skip(4);
        var lines = File.ReadAllLines(path);
        var content = string.Join(newLine, lines);
        return content;
    }

    public string GetText2(
        (string Repo, string Loca) adrTuple)
    {
        var path = pw.GetBodyPath(adrTuple);
        var lines = File.ReadAllLines(path);
        var content = string.Join(newLine, lines);
        return content;
    }
}