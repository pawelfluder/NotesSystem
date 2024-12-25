using System;
using System.IO;

namespace SharpImageSplitterProg.Backup2.Workers;

public class PathsJob3
{
    internal string GetOutputFilePath(
        int i,
        string tempFolderPath)
    {
        string name = $"{IndexToString(i+1)}_item.png";
        string outputfilePath = Path.Combine(tempFolderPath, name);
        return outputfilePath;
    }

    internal string GetInputImageFilePath(
        (string folderPath, string fileName) folderQfile)
    {
        string filePath = Path.Combine(folderQfile.folderPath, folderQfile.fileName);
        filePath = filePath.Replace("\\", "/");
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found", filePath);
        }
        return filePath;
    }

    internal string CreateTempFolder(
        (string folderPath, string fileName) folderQfile)
    {
        string tempFolderPath = Path.Combine(folderQfile.folderPath, "temp");

        if (!Directory.Exists(tempFolderPath))
        {
            Directory.CreateDirectory(tempFolderPath);
        }
        return tempFolderPath;
    }
    
    private string IndexToString(int index)
    {
        if (index < 10)
        {
            return "0" + index;
        }
        if (index < 100)
        {
            return index.ToString();
        }

        throw new Exception();
    }
}