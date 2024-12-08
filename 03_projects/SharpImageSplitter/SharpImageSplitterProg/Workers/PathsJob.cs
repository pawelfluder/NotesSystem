using System;
using System.IO;
using SharpImageSplitterProg.Models;

namespace SharpImageSplitterProg.Workers;

internal class PathsJob
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

    internal string ReCreateNewTempFolder(
        string tempFolderPath)
    {
        if (Directory.Exists(tempFolderPath))
        {
            Directory.Delete(tempFolderPath, true);
        }
        
        Directory.CreateDirectory(tempFolderPath);
        return tempFolderPath;
    }

    internal string GetTempFolderFilePath(
        (string folderPath, string fileName) folderQfile)
    {
        string tempFolderPath = Path.Combine(folderQfile.folderPath, "temp");
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

    public void AddPaths(
        (string folderPath, string fileName) folderQfile,
        SplitInfo info)
    {
        string tempFolderPath = GetTempFolderFilePath(folderQfile);
        info.ImageFilePath = tempFolderPath;
        info.ImageFilePath = GetInputImageFilePath(folderQfile);
    }

    public void AddTempFolderPath(
        string tempFolderPath,
        SplitInfo info)
    {
        info.TempFolderPath = tempFolderPath;
    }
}
