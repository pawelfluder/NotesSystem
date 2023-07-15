using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;
using SharpRepoServiceProg.FileOperations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SharpRepoServiceProg.Service.IRepoService;

namespace SharpRepoServiceProg.RepoOperations
{
    public class RepoServiceMethods
    {
        private readonly IFileService fileService;

        private readonly string contentFileName;
        private readonly string configFileName;
        private readonly string repoConfigName;

        private static char slash = '/';
        private static string git = ".git";
        private readonly ServerInfo serverInfo;
        private readonly LocalInfo localInfo;
        private readonly IFileService.IYamlOperations yamlOperations;
        private List<string> searchFoldersPaths;
        private List<string> allRepoPathsList;

        public RepoServiceMethods(
           IFileService fileService,
           ServerInfo serverInfo,
           LocalInfo localInfo)
        {
            this.fileService = fileService;
            this.serverInfo = serverInfo;
            this.localInfo = localInfo;
            yamlOperations = fileService.Yaml.Custom03;

            searchFoldersPaths = localInfo.LocalRootPaths;
            contentFileName = "lista.txt";
            configFileName = "nazwa.txt";
            repoConfigName = "repoConfig.txt";
            allRepoPathsList = new List<string>();
            //InitializeSearchFoldersPaths(searchFoldersPaths);
        }

        public RepoServiceMethods(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public List<(string Repo, string Loca)> GetAllRepoAddresses(string repoPath)
        {
            var result = new GetRepoAllAddresses(fileService).Visit(repoPath);
            return result;
        }

        public string GetConfigText((string Repo, string Loca) address)
        {
            var itemPath = GetElemPath(address);
            var nameFilePath = itemPath + slash + configFileName;
            var configLines = File.ReadAllLines(nameFilePath);
            var configText = string.Join('\n', configLines);
            return configText;
        }

        public List<string> GetConfigLines((string Repo, string Loca) address)
        {
            var itemPath = GetElemPath(address);
            var nameFilePath = itemPath + slash + configFileName;
            var configLines = File.ReadAllLines(nameFilePath).ToList();
            return configLines;
        }

        public bool TryGetConfigLines(
            (string Repo, string Loca) address,
            out List<string> lines)
        {
            try
            {
                lines = GetConfigLines(address);
                return true;
            }
            catch
            {
                lines = null;
                return false;
            }
        }

        public void CreateConfig(
            (string Repo, string Loca) address,
            List<string> contentLines)
        {
            var itemPath = GetElemPath(address);
            var nameFilePath = itemPath + slash + configFileName;
            var content = string.Join('\n', contentLines);
            File.WriteAllText(nameFilePath, content);
        }

        public object GetConfigKey(
            (string Repo, string Loca) address,
            string key)
        {
            var itemPath = GetElemPath(address);
            var configItemPath = itemPath + slash + configFileName;

            var obj = yamlOperations.DeserializeFile<Dictionary<string, object>>(configItemPath);

            var result = obj[key];
            return result;
        }

        public Dictionary<string, object> GetConfigDictionary(
            (string Repo, string Loca) address)
        {
            var itemPath = GetElemPath(address);
            var configItemPath = itemPath + slash + configFileName;

            var dict = yamlOperations.DeserializeFile<Dictionary<string, object>>(configItemPath);
            return dict;
        }

        public object GetConfigKeyValue(
            (string Repo, string Loca) address,
            string key)
        {
            var dict = GetConfigDictionary(address);
            var exists = dict.TryGetValue(key, out var value);
            if (exists)
            {
                return value;
            }

            return null;
        }

        public void CreateConfigKey(
            (string Repo, string Loca) address,
            string key,
            object value)
        {
            var dict = GetConfigDictionary(address);
            var exists = dict.TryGetValue(key, out var tmp);
            if (exists)
            {
                dict[key] = value;
            }

            if (!exists)
            {
                dict.Add(key, value);
                try
                {
                    CreateConfig(address, dict);
                }
                catch { };
                
            }
        }

        private void CreateConfig(
            (string Name, string Location) address,
            Dictionary<string, object> dict)
        {
            var elemPath = GetConfigFilePath(address);
            yamlOperations.SerializeToFile(elemPath, dict);
        }

        private string GetConfigFilePath((string Name, string Location) address)
        {
            var tmp = GetLocalPath(address);
            var path = tmp + slash + configFileName;
            return path;
        }

        private bool IsRepoConfig(string path)
        {
            var filePath = path + slash + repoConfigName;
            if (File.Exists(filePath))
            {
                return true;
            }

            return false;
        }

        public void CreateRepoConfig(string repoName, string content)
        {
            var address = (repoName, "");
            var itemPath = GetElemPath(address);
            var nameFilePath = itemPath + slash + repoConfigName;
            File.WriteAllText(nameFilePath, content);
        }

        

        public List<string> GetAllReposNames()
        {
            var repos = allRepoPathsList.Select(x => Path.GetFileName(x)).ToList();
            return repos;
        }

        public void InitializeSearchFoldersPaths(List<string> searchFoldersPaths)
        {
            foreach (var searchFolder in searchFoldersPaths)
            {
                var folders = Directory.GetDirectories(searchFolder).Select(x => CorrectPath(x));
                foreach (var folder in folders)
                {
                    if (true || IsRepoConfig(folder))
                    {
                        allRepoPathsList.Add(folder);
                    }
                }
            }
        }

        public List<string> GetAllItems(string repoName)
        {
            var repoPath = allRepoPathsList.SingleOrDefault(x => Path.GetFileName(x) == repoName);
            if (repoPath == null) { throw new Exception(); }

            fileService.File.GetNewRecursivelyVisitDirectory();

            return null;
        }

        private string CorrectPath(string path)
        {
            return path.Replace("\\", "/");
        }

        public List<string> GetAllReposPaths()
        {
            return allRepoPathsList;
        }


        public void SaveElementsList(
            (string Name, string Location) address,
            List<(string Name, string Content)> nQcList)
        {
            var localPath = GetElemPath(address);
            var lastNumber = GetFolderLastNumber(localPath);

            foreach (var nQc in nQcList)
            {
                lastNumber++;
                CreateText(address, nQc.Name, nQc.Content);
            }
        }

        public List<string> GetDirectories(string path)
        {
            var dirs = Directory.GetDirectories(path).ToList();
            dirs = dirs.Select(x => x.Replace('\\', '/')).ToList();
            dirs.RemoveAll(x => Path.GetFileName(x) == ".git");
            dirs.RemoveAll(x => StringToIndex(Path.GetFileName(x)) == -1);

            return dirs;
        }

        [MethodLogger]
        public (string Repo, string Loca) GetExistingItem(
            (string Repo, string Loca) address,
            string name)
        {
            var localNames = GetAllFoldersNames(address);

            var match = localNames.Where(x => x == name);
            if (match.Count() > 0)
            {
                var tmp = match.First();
                var tmp2 = GetFolderByName(address.Repo, address.Loca, tmp);
                return tmp2;
            }

            return default;
        }

        [MethodLogger]
        public (string Repo, string Loca) CreateFolder(
            (string Repo, string Loca) address,
            string name)
        {
            var existingItem = GetExistingItem(address, name);
            Console.Write($"existingItem: {existingItem}");
            if (existingItem != default)
            {
                return existingItem;
            }

            var localPath = GetElemPath(address);
            var lastNumber = GetFolderLastNumber(localPath);
            var indexString = IndexToString(lastNumber + 1);
            var newLoca = indexString;

            if (address.Loca != string.Empty)
            {
                newLoca = address.Loca + slash + newLoca;
            }

            var newAddress = (address.Repo, newLoca);
            CreateFolderGenerate(newAddress, name);
            return newAddress;
        }

        [MethodLogger]
        private void CreateFolderGenerate(
            (string Repo, string Loca) address,
            string name)
        {
            var elemPath = GetElemPath(address);
            Directory.CreateDirectory(elemPath);
            var nameFilePath = elemPath + slash + configFileName;
            File.WriteAllText(nameFilePath, name);
        }

        private int GetLocationLastNumber(string location)
        {
            var lastString = location.Split("/").Last();
            int.TryParse(lastString, out var lastNumber);
            return lastNumber;
        }

        public (string, string) CreateText(
            (string Repo, string Loca) address,
            string name,
            string content)
        {
            var existingItem = GetExistingItem(address, name);
            if (existingItem != default)
            {
                CreateTextGenerate(existingItem, name, content);
                return existingItem;
            }

            var lastNumber = GetFolderLastNumber(address);
            var newAddress = AddIndexToAddress(address, lastNumber + 1);
            CreateTextGenerate(newAddress, name, content);
            return newAddress;
        }

        public (string, string) AppendText(
            (string Repo, string Loca) address,
            string name,
            string content)
        {
            var existingItem = GetExistingItem(address, name);
            if (existingItem != default)
            {
                AppendTextGenerate(existingItem, content);
                return existingItem;
            }

            var lastNumber = GetFolderLastNumber(address);
            var newAddress = AddIndexToAddress(address, lastNumber + 1);
            CreateTextGenerate(newAddress, name, content);
            return newAddress;
        }

        private (string, string) AddIndexToAddress(
            (string Repo, string Loca) address,
            int index)
        {
            var newLoca = address.Loca + slash + IndexToString(index);
            return (address.Repo, newLoca);
        }

        private void CreateTextGenerate(
            (string Repo, string Loca) address,
            string name,
            string content)
        {
            var elemPath = GetElemPath(address);

            Directory.CreateDirectory(elemPath);

            var nameFilePath = elemPath + slash + configFileName;
            File.WriteAllText(nameFilePath, name);

            var contentFilePath = elemPath + slash + contentFileName;
            File.WriteAllText(contentFilePath, content);
        }

        private void AppendTextGenerate(
            (string Repo, string Loca) address,
            string content)
        {
            var elemPath = GetElemPath(address);

            var contentFilePath = elemPath + slash + contentFileName;
            var oldContent = ReadText(address);
            var newContent = oldContent + "\n" + content;
            File.WriteAllText(contentFilePath, newContent);
        }

        public int GetNumberFromLoca(string loca)
        {
            var tmp1 = loca.Split('/').Last();
            var number = StringToIndex(tmp1);
            return number;
        }

        [MethodLogger]
        public int GetFolderLastNumber(
            (string Repo, string Loca) address)
        {
            var elemPath = GetElemPath(address);
            var directories = GetDirectories(elemPath);
            var numbers = directories
                .Select(x => StringToIndex(Path.GetFileName(x)))
                .ToList();
            if (numbers.Count != 0)
            {
                return numbers.Max();
            }

            return 0;
        }

        [MethodLogger]
        public int GetFolderLastNumber(string elemPath)
        {
            var directories = GetDirectories(elemPath);
            var numbers = directories
                .Select(x => StringToIndex(Path.GetFileName(x)))
                .ToList();
            if (numbers.Count != 0)
            {
                return numbers.Max();
            }

            return 0;
        }

        [MethodLogger]
        public string GetElemPath((string Repo, string Loca) address)
        {
            var elemPath = GetRepoPath(address.Repo);
            if (address.Loca != string.Empty)
            {
                elemPath += slash + address.Loca;
            }

            return elemPath;
        }

        [MethodLogger]
        public string GetRepoPath(string repo)
        {
            var foundList = allRepoPathsList.Where(x => Path.GetFileName(x) == repo).ToList();
            if (foundList?.Count() != 1 ||
                !Directory.Exists(foundList.First()))
            {
                HandleError();
            }

            return foundList.First();
        }

        [MethodLogger]
        public string HandleError()
        {
            throw new Exception();
        }

        [MethodLogger]
        public string GetLocalPath((string repo, string loca) address)
        {
            var elemPath = GetRepoPath(address.repo);
            if (address.loca != string.Empty)
            {
                elemPath += slash + address.loca;
            }

            return elemPath;
        }

        [MethodLogger]
        public string GetLocalName((string repo, string loca) address)
        {
            var repoPath = GetRepoPath(address.repo);
            var path = repoPath + "/" + address.loca + "/" + configFileName;
            var name = GetConfigKey(address, ConfigKeys.name.ToString());
            //var name = File.ReadAllLines(path).First();
            return name.ToString();
        }

        [MethodLogger]
        private string GetLocalName(string elemPath)
        {
            var path = elemPath + "/" + configFileName;
            var name = File.ReadAllLines(path).First();
            return name;
        }

        private string GetAddress(string elemPath)
        {
            var path = elemPath + "/" + configFileName;
            var name = File.ReadAllLines(path).First();
            return name;
        }

        [MethodLogger]
        public List<string> GetAllFoldersNames((string repo, string loca) address)
        {
            var repoPath = GetLocalPath(address);
            Console.WriteLine($"repoPath: {repoPath}");
            var dirs = GetDirectories(repoPath);
            var names = new List<string>();
            foreach (var dir in dirs)
            {
                var name = GetLocalName(dir);
                names.Add(name);
            }

            return names;
        }

        public (string, string) GetFolderByName(
            string repo,
            string section,
            string name)
        {
            var repoPath = GetElemPath((repo, section));
            var dirs = GetDirectories(repoPath);

            foreach (var dir in dirs)
            {
                var tmp = GetLocalName(dir);
                if (tmp == name)
                {
                    var newSection = Path.GetFileName(dir);
                    if (section != string.Empty)
                    {
                        newSection = section + slash + newSection;
                    }

                    return (repo, newSection);
                }
            }

            return default;
        }

        public string GetSectionFromPath(
            string repo,
            string path)
        {
            var repoPath = GetRepoPath(repo);
            if (path.StartsWith(repoPath))
            {
                var tmp = path.Replace(repoPath, "");
                var tmp2 = tmp.Trim('/');
                return tmp2;
            }

            return default;
        }

        public (Guid, string) GetRepoFromAgruments(string[] args)
        {
            if (args.Length == 1)
            {
                var curPath = Environment.CurrentDirectory;
                var repo = GetRepoPath(curPath);
                return default;
            }

            if (args.Length == 2 &&
            Directory.Exists(args[1]))
            {
                var repo = GetRepoPath(args[1]);
                return default;
            }

            if (args.Length == 3)
            {
                //var repo = GetRepo(args[1], args[2]);
                return default;
            }

            return default;
        }

        public List<string> GetAllMsgFolders()
        {
            var guid = "ebf8d4ba-06c2-43eb-a201-4d32d13656e4";
            var path = localInfo.LocalRootPaths + "/" + guid;
            var allDirectories = Directory.GetDirectories(path);
            var msg = "Msg";
            var msgDirectories = allDirectories.Where(x => Path.GetFileName(x).StartsWith(msg)).ToList();
            return msgDirectories;
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
            if (index < 1000)
            {
                return index.ToString();
            }

            throw new Exception();
        }

        private int StringToIndex(string numberString)
        {
            var success = int.TryParse(numberString, out int result);

            if (!success)
            {
                return -1;
            }

            return result;
        }

        public (string, string) ReadElemPathByNames((string Repo, string Loca) address, List<string> names)
        {
            foreach (var name in names)
            {
                var tmp = GetAllFoldersNames(address);
                var find = tmp.SingleOrDefault(x => x == name);
                if (name != null)
                {
                    address = GetExistingItem(address, name);
                }
            }

            return address;
        }

        public List<string> ReadElemListByNames((string Repo, string Loca) address, List<string> names)
        {
            address = ReadElemPathByNames(address, names);
            var localPath = GetElemPath(address);
            var folders = GetDirectories(localPath);
            var tmp = folders.Select(x => Path.GetFileName(x));

            var contentsList = new List<string>();

            foreach (var tmp2 in tmp)
            {
                var index = StringToIndex(tmp2);
                var newAddress = AddIndexToAddress(address, index);
                var content = ReadText(newAddress);
                contentsList.Add(content);
            }

            return contentsList;
        }

        public string ReadText((string Repo, string Loca) address)
        {
            var path = GetElemPath(address) + "/" + contentFileName;
            var lines = File.ReadAllLines(path);
            var content = string.Join('\n', lines);
            return content;
        }

        public List<string> ReadTextLines((string Repo, string Loca) address)
        {
            var path = GetElemPath(address) + "/" + contentFileName;
            var lines = File.ReadAllLines(path).ToList();
            return lines;
        }

        public List<string> ReadTextElemList((string Repo, string Loca) address)
        {
            var localPath = GetElemPath(address);
            var folders = GetDirectories(localPath);
            var tmp = folders.Select(x => Path.GetFileName(x));

            var contentsList = new List<string>();

            foreach (var tmp2 in tmp)
            {
                var index = StringToIndex(tmp2);
                var newAddress = AddIndexToAddress(address, index);
                var path = GetElemPath(newAddress) + slash + contentFileName;
                var content = File.ReadAllText(path);
                contentsList.Add(content);
            }

            return contentsList;
        }
    }
}
