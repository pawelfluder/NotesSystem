using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpRepoServiceProg.WorkersSystem
{
    internal class PathWorker
    {
        public List<string> reposPathsList;
        private string slash = "/";

        private string contentFileName;
        private string configFileName;

        public PathWorker()
        {
            SetNames();
        }

        public List<string> GetAllReposPaths() => reposPathsList;

        private void SetNames()
        {
            configFileName = "nazwa.txt";
            contentFileName = "lista.txt";
        }

        public string GetItemPath(
            (string Repo, string Loca) adrTuple)
        {
            var elemPath = GetRepoPath(adrTuple.Repo);
            if (adrTuple.Loca != string.Empty)
            {
                elemPath += slash + adrTuple.Loca;
            }

            return elemPath;
        }

        public string GetRepoPath(string repo)
        {
            var foundList = reposPathsList.Where(x => Path.GetFileName(x) == repo).ToList();

            if (foundList != null &&
                foundList.Count() == 1)
            {
                var result = foundList.First();
                return result;
            }

            var result2 = HandleError();
            return result2;
        }

        public string GetConfigPath(
            (string Repo, string Location) address)
        {
            var tmp = GetItemPath(address);
            var path = tmp + slash + configFileName;
            return path;
        }

        public string GetBodyPath((string Name, string Location) address)
        {
            var tmp = GetItemPath(address);
            var path = tmp + slash + contentFileName;
            return path;
        }

        public void PutPaths(List<string> searchPaths)
        {
            reposPathsList = new List<string>();
            foreach (var searchFolder in searchPaths)
            {
                var folders = Directory.GetDirectories(searchFolder).Select(x => CorrectPath(x));
                foreach (var folder in folders)
                {
                    //if (true || IsRepoConfig(folder))
                    //{
                    reposPathsList.Add(folder);
                    //}
                }
            }
        }

        public string CorrectPath(string path)
        {
            return path.Replace("\\", "/");
        }

        private string HandleError()
        {
            throw new NotImplementedException();
        }

        internal int GetRepoCount()
        {
            return reposPathsList.Count();
        }
    }
}