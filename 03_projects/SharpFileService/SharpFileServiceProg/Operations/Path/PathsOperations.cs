using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SharpFileServiceProg.Operations.Files
{
    internal class PathsOperations : IPathsOperations
    {
        public string MoveDirectoriesUp(string path, int level)
        {
            for (int i = 0; i < level; i++)
            {
                path = Directory.GetParent(path).FullName;
            }
            return path;
        }

        public string GetBinPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            var tmp = Path.GetDirectoryName(path);

            var parent = new DirectoryInfo(tmp);
            while (parent?.Name != "bin")
            {
                parent = Directory.GetParent(parent.FullName);
            }

            var binPath = parent.FullName;
            return binPath;
        }

        public void CreateMissingDirectories(string path)
        {
            var parentsPaths = new List<string>();

            for (int i = 0; i < 3; i++)
            {
                path = Directory.GetParent(path).FullName;
                parentsPaths.Insert(0, path);
            }

            foreach (var parentPath in parentsPaths)
            {
                if (!Directory.Exists(parentPath))
                {
                    Directory.CreateDirectory(parentPath);
                }
            }
        }
    }
}
