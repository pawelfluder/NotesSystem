using System;
using System.IO;
using SharpImageSplitterProg.AAPublic.Strategies;
using SharpOperationsProg.AAPublic;
using SharpOperationsProg.AAPublic.Operations;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SharpImageSplitterProg.Backup2.Workers;

public class ResizeJob3
{
    private readonly StrategyBase<IResizeStrategy> _resizeStrategyBase;

    public ResizeJob3()
    {
        _resizeStrategyBase = new StrategyBase<IResizeStrategy>();
    }

    // public IResizeStrategy GetNewStrategy(string name) => _resizeStrategyBase.GetNewStrategy(name);

    public (int, int) Resize(
        (string folderPath, string fileName) folderQfile,
        string resizeStrategyName)
    {
        var strategy = _resizeStrategyBase.GetNewStrategy(resizeStrategyName);
        var size = Resize(folderQfile, strategy);
        return size;
    }

    private (int, int) Resize(
        (string folderPath, string fileName) folderQfile,
        IResizeStrategy strategy)
    {
        Action<Image>? action = GetAction(strategy);
        string imageOryginalPath = folderQfile.folderPath + "/" + folderQfile.fileName;
        string imageNewPath = imageOryginalPath;
        CopyOryginal(folderQfile);
        string copyOfImageOriginalPath = AddCopyToName(folderQfile);

        using (Image image = Image.Load(imageOryginalPath))
        {
            image.Save(copyOfImageOriginalPath);
        }

        (int, int) imageSize = default;
        using (Image image = Image.Load(copyOfImageOriginalPath))
        {
            action.Invoke(image);
            image.Save(imageNewPath);
            imageSize = (image.Width, image.Height);
        }
            
        return imageSize;
    }

    private void CopyOryginal((string folderPath, string fileName) folderQfile)
    {
        string oryginalFilePath = folderQfile.folderPath + "/" + folderQfile.fileName;
        string oryginalCopyFolderPath = folderQfile.folderPath + "/" + "oryginal";
        string oryginalCopyFilePath = oryginalCopyFolderPath + "/" + folderQfile.fileName;
            
        if (!Directory.Exists(oryginalCopyFolderPath))
        {
            Directory.CreateDirectory(oryginalCopyFolderPath);
        }
        
        if (!File.Exists(oryginalCopyFilePath))
        {
            File.Copy(oryginalFilePath, oryginalCopyFilePath);
        }
            
        File.Delete(oryginalFilePath);
        File.Copy(oryginalCopyFilePath, oryginalFilePath);
    }

    private string AddCopyToName((string folderPath, string fileName) folderQfile)
    {
        string copyStr = "_Copy";
        int i = 1;
        string path = folderQfile.folderPath + "/" + folderQfile.fileName;
        string extension = Path.GetExtension(path);
        path = path.Replace(extension, string.Empty);
        path += "_Copy" + extension;

        string copyStr2 = copyStr + i;
        while (File.Exists(path) && i <= 4)
        {
            path = path.Replace(copyStr2 + extension, string.Empty);
            i++;
            copyStr2 = copyStr + i;
            path += copyStr2 + extension;
        }
            
        return path;
    }

    private static Action<Image>? GetAction(IResizeStrategy strategy)
    {
        if (strategy.ValidOption == nameof(strategy.ResizeWidthQHeight))
        {
            return ((image) =>
            {
                int newWidth = strategy.ResizeWidthQHeight.Width;
                int newHeight = strategy.ResizeWidthQHeight.Height;
                image.Mutate(x => x.Resize(newWidth, newHeight));
            });
        }
            
        if (strategy.ValidOption == nameof(strategy.DesireHeight))
        {
            return ((image) =>
            {
                var rate = strategy.DesireHeight / image.Height;
                var desiredWidth = image.Width * rate;
                image.Mutate(x => x.Resize(desiredWidth, strategy.DesireHeight));
            });
        }
            
        if (strategy.ValidOption == nameof(strategy.DesireWidth))
        {
            return ((image) =>
            {
                var rate = strategy.DesireWidth / image.Width;
                var desiredHeight = image.Height * rate;
                image.Mutate(x => x.Resize(strategy.DesireWidth, desiredHeight));
            });
        }

        return default;
    }
}