using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
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
        private List<string> reposPathsList;

        public RepoServiceMethods(
           IFileService fileService,
           ServerInfo serverInfo,
           LocalInfo localInfo)
        {
            contentFileName = "lista.txt";
            configFileName = "nazwa.txt";
            repoConfigName = "repoConfig.txt";
            reposPathsList = new List<string>();

            this.fileService = fileService;
            this.serverInfo = serverInfo;
            this.localInfo = localInfo;
            yamlOperations = fileService.Yaml.Custom03;
        }

        public RepoServiceMethods(IFileService fileService)
        {
            this.fileService = fileService;
        }

        //-------------------------
        // GET
        public List<(string Repo, string Loca)> GetAllRepoAddresses(
            (string Repo, string Loca) a)
        {
            var path = GetLocalPath(a);
            var tmp = fileService.File.NewRepoAddressesObtainer().Visit(path);
            var result = tmp.Select(x => (a.Repo, JoinLoca(a.Loca, x))).ToList();
            return result;
        }

        public string JoinLoca(string loca01, string loca02)
        {
            if (loca01 == string.Empty)
            {
                return loca02;
            }

            var newLoca = loca01 + "/" + loca02;
            return newLoca;
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

        public (string, string) GetPathsByName(
            (string Repo, string Loca) address,
            List<string> names)
        {
            // ReadElemPathByNames
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

        public List<string> GetManyItemByName(
            (string Repo, string Loca) address,
            List<string> names)
        {
            // ReadElemListByNames
            address = GetPathsByName(address, names);
            var localPath = GetElemPath(address);
            var folders = GetDirectories(localPath);
            var tmp = folders.Select(x => Path.GetFileName(x));

            var contentsList = new List<string>();

            foreach (var tmp2 in tmp)
            {
                var index = StringToIndex(tmp2);
                var newAddress = SelectAddress(address, index);
                var content = GetText2(newAddress);
                contentsList.Add(content);
            }

            return contentsList;
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

        public object TryGetConfigValue(
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

        public string GetConfigPath((string Name, string Location) address)
        {
            var tmp = GetLocalPath(address);
            var path = tmp + slash + configFileName;
            return path;
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

        public string GetText2((string Repo, string Loca) address)
        {
            // ReadText
            var path = GetElemPath(address) + "/" + contentFileName;
            var lines = File.ReadAllLines(path);
            var content = string.Join('\n', lines);
            return content;
        }

        public List<string> GetTextLines(
            (string Repo, string Loca) address)
        {
            // ReadTextLines
            var path = GetElemPath(address) + "/" + contentFileName;
            var lines = File.ReadAllLines(path).ToList();
            return lines;
        }

        public List<string> GetManyText((string Repo, string Loca) address)
        {
            // ReadTextElemList
            var localPath = GetElemPath(address);
            var folders = GetDirectories(localPath);
            var tmp = folders.Select(x => Path.GetFileName(x));

            var contentsList = new List<string>();

            foreach (var tmp2 in tmp)
            {
                var index = StringToIndex(tmp2);
                var newAddress = SelectAddress(address, index);
                var path = GetElemPath(newAddress) + slash + contentFileName;
                var content = File.ReadAllText(path);
                contentsList.Add(content);
            }

            return contentsList;
        }

        public string GetItem(
            (string repo, string loca) address)
        {
            var type = GetItemType(address);
            JsonValue body = null;
            if (type == "Text")
            {
                body = JsonValue.Create(
                    GetTextLines(address));
            }

            if (type == "Folder")
            {
                body = JsonValue.Create(
                    GetAllIndexesQNames(address));
            }
            var name = GetName(address);

            JsonObject jObj = new JsonObject();
            jObj.Add("type", type);
            jObj.Add("name", name);
            jObj.Add("body", body);

            //var item = new Dictionary<string, object>();
            //var name = GetLocalName(address);
            //item.Add("Name", name);
            //item.Add("Body", body);
            //item.Add("Type", type);
            return jObj.ToJsonString();
        }

        [MethodLogger]
        public string GetLocalName((string repo, string loca) address)
        {
            var name = GetConfigKey(address, ConfigKeys.name.ToString());
            return name.ToString();
        }

        [MethodLogger]
        private string GetItemType((string repo, string loca) address)
        {
            var result = GetType(address);
            return result;
        }

        [MethodLogger]
        public string GetItemType(string repo, string loca)
        {
            var address = (repo, loca);
            var result = GetType(address);
            return result;
        }

        [MethodLogger]
        public string GetType((string repo, string loca) address)
        {
            var repoPath = GetRepoPath(address.repo);
            var contentFilePath = repoPath + "/" + address.loca + "/" + contentFileName;
            if (File.Exists(contentFilePath))
            {
                return "Text";
            }

            return "Folder";
        }

        [MethodLogger]
        private string GetName((string repo, string loca) address)
        {
            var name = GetConfigKey(address, "name").ToString();
            return name;
        }

        [MethodLogger]
        private int GetIndex(string elemPath)
        {
            //var parentName = Directory.GetParent(elemPath).Name;
            var name = Path.GetFileName(elemPath);
            var index = StringToIndex(name.Substring(0, 2));
            return index;
        }

        [MethodLogger]
        private string GetIndexString(string elemPath)
        {
            //var parentName = Directory.GetParent(elemPath).Name;
            var name = Path.GetFileName(elemPath);
            var index = StringToIndex(name.Substring(0, 2));
            var indexString = IndexToString(index);
            return indexString;
        }

        private string GetAddress(string elemPath)
        {
            var path = elemPath + "/" + configFileName;
            var name = File.ReadAllLines(path).First();
            return name;
        }


        [MethodLogger]
        public Dictionary<string, string> GetAllIndexesQNames(
            (string repo, string loca) address)
        {
            var repoPath = GetLocalPath(address);
            Console.WriteLine($"repoPath: {repoPath}");
            var subLocasList = GetDirectories(repoPath)
                .Select(x => SelectDirToSection(address.loca, x)).ToList();
            
            var names = new Dictionary<string, string>();
            foreach (var subLoca in subLocasList)
            {
                var subAddress = (address.repo, subLoca);
                var name = GetName(subAddress);
                var indexString = subLoca.Substring(subLoca.Length - 2, 2);
                names.Add(indexString, name);
            }

            return names;
        }

        [MethodLogger]
        public List<string> GetAllFoldersNames(
            (string repo, string loca) address)
        {
            var subAddresses = GetSubAddresses(address);
            var names = new List<string>();
            foreach (var subAddress in subAddresses)
            {
                var name = GetName(subAddress);
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
            List<string> newSectionsList = GetDirectories(repoPath)
                .Select(x => SelectDirToSection(section, x)).ToList();

            foreach (var newSection in newSectionsList)
            {
                var address = (repo, newSection);
                var tmp = GetName(address);
                if (tmp == name)
                {
                    return address;
                }
            }

            return default;
        }

        public List<(string, string)> GetSubAddresses(
            (string repo, string loca) address)
        {
            var repoPath = GetLocalPath(address);
            var dirs = GetDirectories(repoPath);
            var subAddresses = dirs.Select(x => (address.repo, SelectDirToSection(address.loca, x))).ToList();
            return subAddresses;
        }

        public List<string> GetAllReposNames()
        {
            var repos = reposPathsList.Select(x => Path.GetFileName(x)).ToList();
            return repos;
        }

        public List<string> GetAllItems(string repoName)
        {
            var repoPath = reposPathsList.SingleOrDefault(x => Path.GetFileName(x) == repoName);
            if (repoPath == null) { throw new Exception(); }

            fileService.File.GetNewRecursivelyVisitDirectory();

            return null;
        }

        public List<string> GetAllReposPaths()
        {
            return reposPathsList;
        }

        public int GetReposCount()
        {
            return reposPathsList.Count;
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
            var foundList = reposPathsList.Where(x => Path.GetFileName(x) == repo).ToList();
            if (foundList?.Count() != 1 ||
                !Directory.Exists(foundList.First()))
            {
                HandleError();
            }

            return foundList.First();
        }
        // GET
        //-------------------------

        //-------------------------
        // TRY GET
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
        // TRY GET
        //-------------------------


        //-------------------------
        // CREATE
        public void InternalCreateConfig(
            string itemPath,
            Dictionary<string, object> dict)
        {
            var nameFilePath = itemPath + slash + configFileName;
            var content = yamlOperations.Serialize(dict);
            File.WriteAllText(nameFilePath, content);
        }

        public void InternalCreateBody(
            string itemPath,
            string content)
        {
            var gg = string.Join("", Enumerable.Repeat("\n", 4));
            var contentFilePath = itemPath + slash + contentFileName;
            File.WriteAllText(contentFilePath, gg + content);
        }

        public void CreateConfig(
            (string Repo, string Loca) address,
            Dictionary<string, object> dict)
        {
            var itemPath = GetElemPath(address);
            var nameFilePath = itemPath + slash + configFileName;
            var content = yamlOperations.Serialize(dict);
            File.WriteAllText(nameFilePath, content);
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

        public void CreateManyText(
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

        public void CreateRepoConfig(string repoName, string content)
        {
            var address = (repoName, "");
            var itemPath = GetElemPath(address);
            var nameFilePath = itemPath + slash + repoConfigName;
            File.WriteAllText(nameFilePath, content);
        }

        public (string, string) CreateChildText(
            (string Repo, string Loca) address,
            string name)
        {
            var lastNumber = GetFolderLastNumber(address);
            var newAddress = SelectAddress(address, lastNumber + 1);
            CreateTextGenerate(newAddress, name, string.Empty);
            return newAddress;
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
        public (string Repo, string Loca) CreateChildFolder(
            (string Repo, string Loca) address,
            string name)
        {
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
            var dict = new Dictionary<string, object>() { { "name", name } };
            CreateConfig(address, dict);
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
            var newAddress = SelectAddress(address, lastNumber + 1);
            CreateTextGenerate(newAddress, name, content);
            return newAddress;
        }

        private void CreateTextGenerate(
            (string Repo, string Loca) address,
            string name,
            string content)
        {
            // directory
            var itemPath = GetElemPath(address);
            Directory.CreateDirectory(itemPath);

            // config
            var dict = new Dictionary<string, object>() { { "name", name } };
            InternalCreateConfig(itemPath, dict);

            // body
            InternalCreateBody(itemPath, content);
        }
        // CREATE
        //-------------------------


        //-------------------------
        // APPEND
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
            var newAddress = SelectAddress(address, lastNumber + 1);
            CreateTextGenerate(newAddress, name, content);
            return newAddress;
        }

        public void AppendText(
            (string Repo, string Loca) address,
            string content)
        {
            AppendTextGenerate(address, content);
        }

        private void AppendTextGenerate(
            (string Repo, string Loca) address,
            string content)
        {
            var elemPath = GetElemPath(address);

            var contentFilePath = elemPath + slash + contentFileName;
            var oldContent = GetText2(address);
            var newContent = oldContent + "\n" + content;
            File.WriteAllText(contentFilePath, newContent);
        }

        private void AppendTextTopGenerate(
            (string Repo, string Loca) address,
            string content)
        {
            var elemPath = GetElemPath(address);

            var contentFilePath = elemPath + slash + contentFileName;
            var oldContent = GetText2(address);
            var newContent = oldContent + "\n" + content;
            File.WriteAllText(contentFilePath, newContent);
        }

        public string GetText3((string Repo, string Loca) address)
        {
            // ReadText
            var path = GetElemPath(address) + "/" + contentFileName;
            var lines = File.ReadAllLines(path).Skip(4);
            var content = string.Join('\n', lines);
            return content;
        }

        // APPEND
        //-------------------------


        //-------------------------
        // OTHER
        private bool IsRepoConfig(string path)
        {
            var filePath = path + slash + repoConfigName;
            if (File.Exists(filePath))
            {
                return true;
            }

            return false;
        }

        public void Initialize(List<string> searchPaths)
        {
            foreach (var searchFolder in searchPaths)
            {
                var folders = Directory.GetDirectories(searchFolder).Select(x => CorrectPath(x));
                foreach (var folder in folders)
                {
                    if (true || IsRepoConfig(folder))
                    {
                        reposPathsList.Add(folder);
                    }
                }
            }
        }

        private string CorrectPath(string path)
        {
            return path.Replace("\\", "/");
        }

        private int GetLocationLastNumber(string location)
        {
            var lastString = location.Split("/").Last();
            int.TryParse(lastString, out var lastNumber);
            return lastNumber;
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

        [MethodLogger]
        public string HandleError()
        {
            throw new Exception();
        }
        // OTHER
        //-------------------------

        //-------------------------
        // SELECT
        private (string, string) SelectAddress(
            (string Repo, string Loca) address,
            int index)
        {
            // AddIndexToAddress
            var newLoca = address.Loca + slash + IndexToString(index);
            return (address.Repo, newLoca);
        }

        public int SelectNumberFromLoca(string loca)
        {
            // GetNumberFromLoca
            var tmp1 = loca.Split('/').Last();
            var number = StringToIndex(tmp1);
            return number;
        }

        public string SelectDirToSection(string section, string dir)
        {
            // DirToSection
            var newSection = Path.GetFileName(dir);
            if (section != string.Empty)
            {
                newSection = section + slash + newSection;
            }

            return newSection;
        }
        // SELECT
        //-------------------------
    }
}
