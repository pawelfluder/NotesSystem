using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.WorkersSystem;
using SharpTinderComplexTests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpRepoServiceProg.Workers
{
    public class MethodWorker
    {
        // services
        private readonly IFileService fileService;
        private readonly IFileService.IYamlOperations yamlOperations;

        private readonly ReadWorker rw;
        private readonly MemoWorker mw;
        private readonly JsonWorker jw;
        private readonly FolderWriteWorker fww;
        private readonly TextWriteWorker tww;
        private readonly PathWorker pw;
        private readonly ConfigWorker cw;

        // char names
        private char slash = '/';
        private char newLine = '\n';

        private readonly ServerInfo serverInfo;
        private readonly LocalInfo localInfo;

        private string errorValue;
        private string refGuidStr;
        private string refLocaStr;

        public MethodWorker(IFileService fileService)
        {
            this.fileService = fileService;
        }

        public MethodWorker(
           IFileService fileService,
           ServerInfo serverInfo,
           LocalInfo localInfo)
        {
            this.fileService = fileService;
            this.serverInfo = serverInfo;
            this.localInfo = localInfo;
            yamlOperations = fileService.Yaml.Custom03;

            this.rw = MyBorder.Container.Resolve<ReadWorker>();
            this.mw = MyBorder.Container.Resolve<MemoWorker>();
            this.jw = MyBorder.Container.Resolve<JsonWorker>();
            this.fww = new FolderWriteWorker(fileService);
            this.tww = new TextWriteWorker();

            this.pw = MyBorder.Container.Resolve<PathWorker>();
            this.cw = MyBorder.Container.Resolve<ConfigWorker>();
        }

        public void CreateTextGenerate(
            (string Repo, string Loca) address,
            string name,
            string content)
            => tww.Put(name, address, content);

        public List<(string Repo, string Loca)> GetAllRepoAddresses(
            (string Repo, string Loca) adrTuple)
            => rw.GetAllRepoAddresses(adrTuple.Repo);

        public void PutPaths(List<string> searchPaths)
            => mw.PutPaths(searchPaths);

        public int GetReposCount()
            => pw.GetRepoCount();

        public string GetSectionFromPath(
            string repo,
            string path)
            => mw.GetSectionFromPath(repo, path);

        public string GetText2(
            (string Repo, string Loca) adrTuple)
            => rw.GetText2(adrTuple);

        public void CreateConfig(
            (string Repo, string Loca) adrTuple,
            Dictionary<string, object> dict)
            => cw.CreateConfig(adrTuple, dict);

        public (string, string) GetAdrTupleByName(
            (string Repo, string Loca) adrTuple,
            string name)
            => rw.GetAdrTupleByName(adrTuple, name);

        public (string, string) GetAdrTupleByNameList(
            (string Repo, string Loca) adrTuple,
            params string[] names)
            => rw.GetAdrTupleByNames(adrTuple, names);

        public (string, string) GetAdrTupleByNameList(
            (string Repo, string Loca) adrTuple,
            List<string> names)
            => rw.GetAdrTupleByNames(adrTuple, names.ToArray());

        public List<string> GetManyItemByName(
            (string Repo, string Loca) adrTuple,
            List<string> names)
            => rw.GetManyTextByNames(adrTuple, names.ToArray());

        public List<(int, string)> GetManyItemByName2(
            (string Repo, string Loca) adrTuple,
            List<string> names)
            => rw.GetManyIdxQTextByNames(adrTuple, names.ToArray());

        public object GetConfigKey(
            (string Repo, string Loca) address,
            string key)
        {
            var result = rw.GetConfigKey(address, key);
            return result;
        }

        public Dictionary<string, object> GetConfigKeyDict(
            (string Repo, string Loca) address,
            params string[] keyArray)
            => rw.GetConfigDict(address, keyArray);

        public List<string> GetManyText((string Repo, string Loca) adrTuple)
            => rw.GetManyText(adrTuple);

        //public string GetElemPath(
        //    (string Repo, string Loca) adrTuple)
        //    => rw.GetItemPath(adrTuple);

        public string GetItem(
            (string repo, string loca) adrTuple)
            => jw.GetItem(adrTuple);

        public string GetItemList(
            (string repo, string loca) adrTuple)
            => jw.GetItemList(adrTuple);

        public string GetLocalName(
            (string repo, string loca) adrTuple)
        {
            var item = rw.GetItemConfig(adrTuple);
            var name = item.Name;
            return name;
        }

        public string GetItemType(
            (string repo, string loca) adrTuple)
        {
            var item = rw.GetItemConfig(adrTuple);
            return item.Type;
        }

        public string GetItemType(
            string repo,
            string loca)
        {
            var adrTuple = (repo, loca);
            var item = rw.GetItemConfig(adrTuple);
            return item.Type;
        }

        public string GetType(
            (string repo, string loca) adrTuple)
        {
            var type = rw.GetConfigKey(adrTuple, Fields_Item.Type);
            if(type == null)
            {
                var type2 = GuesTypeByFiles(adrTuple);
                return type2;
            }
            return type.ToString();
        }

        private string GuesTypeByFiles((string repo, string loca) adrTuple)
        {
            var settingsFile = pw.GetBodyPath(adrTuple);
            if (File.Exists(settingsFile))
            {
                return ItemTypes.Text;
            }
            return ItemTypes.Folder;
        }

        public string GetName(
            (string repo, string loca) adrTuple)
        {
            var item = rw.GetItemConfig(adrTuple);
            return item.Name;
        }







        public List<string> GetTextLines(
            (string repo, string loca) adrTuple)
            => rw.GetTextLines(adrTuple);




        public object TryGetConfigValue(
            (string repo, string loca) adrTuple,
            string keyName)
            => rw.TryGetConfigValue(adrTuple, keyName);

        







        // Todo
        //public string CreateItem(
        //    (string repo, string loca) adrTuple, string type, string name)
        //{
        //    string item = "";

        //    if (type == ItemTypeNames.Text)
        //    {
        //        var newAdrTuple = CreateChildText(adrTuple, name, "");
        //        item = GetItem(newAdrTuple);
        //    }

        //    if (type == ItemTypeNames.Folder)
        //    {
        //        var newAdrTuple = CreateChildFolder(adrTuple, name);
        //        item = GetItem(newAdrTuple);
        //    }

        //    if (type == ItemTypeNames.RefText)
        //    {
        //        var newAdrTuple = CreateChildRefText(adrTuple, name, "");
        //        item = GetItem(newAdrTuple);
        //    }

        //    return item;
        //}

        //public Dictionary<string, object> GetItemDict(
        //    (string repo, string loca) adrTuple)
        //    => jw.GetItemDict(adrTuple);

        public (string repo, string newLoca) GetRefAdrTuple(
            (string repo, string loca) adrTuple)
            => rw.GetRefAdrTuple(adrTuple);

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
            var result = rw.GetFolderByName(repo, section, name);
            return result;
        }

        public List<(string, string)> GetSubAddresses(
            (string repo, string loca) address)
        {
            var result = rw.GetSubAddresses(address);
            return result;
        }

        public List<string> GetAllReposNames()
            => rw.GetAllReposNames();

        public List<string> GetAllReposPaths()
            => rw.GetAllRepoAddresses();

        //public int GetReposCount()
        //{
        //    return .Count;
        //}

        public List<(string Repo, string Loca)> GetFolderAdrTupleList(
            (string Repo, string Loca) adrTuple)
            => rw.GetSubAddresses(adrTuple);

        

        //public List<string> GetDirectories(string path)
        //{
        //    var dirs = Directory.GetDirectories(path).ToList();
        //    dirs = dirs.Select(x => x.Replace('\\', '/')).ToList();
        //    dirs.RemoveAll(x => Path.GetFileName(x) == ".git");
        //    dirs.RemoveAll(x => StringToIndex(Path.GetFileName(x)) == -1);

        //    return dirs;
        //}

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

        public int GetFolderLastNumber(
            (string Repo, string Loca) address)
            => rw.GetFolderLastNumber(address);


        //public int GetFolderLastNumber(string elemPath)
        //{
        //    var directories = GetDirectories(elemPath);
        //    var numbers = directories
        //        .Select(x => StringToIndex(Path.GetFileName(x)))
        //        .ToList();
        //    if (numbers.Count != 0)
        //    {
        //        return numbers.Max();
        //    }

        //    return 0;
        //}

        // GET
        //-------------------------

        //-------------------------
        // TRY GET
        public bool TryGetConfigLines(
            (string Repo, string Loca) address,
            out List<string> lines)
            => rw.TryGetConfigLines(address, out lines);

        // TRY GET
        //-------------------------


        //-------------------------
        // CREATE

        // todo remove
        public string GetElemPath(
            (string Repo, string Loca) adrTuple)
            => pw.GetItemPath(adrTuple);

        public void CreateConfigKey(
            (string Repo, string Loca) address,
            string key,
            object value)
            => cw.CreateConfigKey(address, key, value);

        public void CreateManyText(
            (string Name, string Location) address,
            List<(string Name, string Content)> nQcList)
        {
            foreach (var nQc in nQcList)
            {
                PostText(address, nQc.Name, nQc.Content);
            }
        }

        //public void CreateRepoConfig(string repoName, string content)
        //{
        //    var address = (repoName, "");
        //    var itemPath = GetElemPath(address);
        //    var nameFilePath = itemPath + slash + repoConfigName;
        //    File.WriteAllText(nameFilePath, content);
        //}

        //public (string, string) CreateChildText(
        //    (string Repo, string Loca) address,
        //    string name,
        //    string content = "")
        //{
        //    var lastNumber = GetFolderLastNumber(address);
        //    var newAddress = fileService.Index.SelectAddress(address, lastNumber + 1);
        //    bw.CreateTextGenerate(newAddress, name, string.Empty);
        //    return newAddress;
        //}

        public List<string> GetConfigLines(
            (string Repo, string Loca) adrtuple)
            => rw.GetConfigLines(adrtuple);

        public void PatchText(
            string content,
            (string Repo, string Loca) adrTuple)
            => tww.Patch(content, adrTuple);

        public (string, string) PutText(
            (string Repo, string Loca) address,
            string name,
            string content = "")
        {
            tww.Put(name, address, content);
            return address;
        }

        public (string, string) PostText(
            (string Repo, string Loca) adrTuple,
            string name,
            string content)
            => tww.Post(name, adrTuple, content);


        //public (string Repo, string Loca) CreateFolder(
        //    (string Repo, string Loca) address,
        //    string name)
        //{
        //    var existingItem = GetExistingItem(address, name);
        //    //Console.Write($"existingItem: {existingItem}");
        //    if (existingItem != default)
        //    {
        //        return existingItem;
        //    }

        //    var localPath = pw.GetElemPath(address);
        //    var lastNumber = GetFolderLastNumber(localPath);
        //    var indexString = IndexToString(lastNumber + 1);
        //    var newLoca = indexString;

        //    if (address.Loca != string.Empty)
        //    {
        //        newLoca = address.Loca + slash + newLoca;
        //    }

        //    var newAddress = (address.Repo, newLoca);
        //    CreateFolderGenerate(newAddress, name);
        //    return newAddress;
        //}

        public (string Repo, string Loca) CreateChildFolder(
            (string Repo, string Loca) adrTuple,
            string name)
            => fww.Post(name, adrTuple);


        //public (string Repo, string Loca) CreateChildFolder(
        //    (string Repo, string Loca) address,
        //    string name)

        //{
        //    var localPath = bw.GetElemPath(address);
        //    var lastNumber = GetFolderLastNumber(localPath);
        //    var indexString = IndexToString(lastNumber + 1);
        //    var newLoca = indexString;

        //    if (address.Loca != string.Empty)
        //    {
        //        newLoca = address.Loca + slash + newLoca;
        //    }

        //    var newAddress = (address.Repo, newLoca);
        //    CreateFolderGenerate(newAddress, name);
        //    return newAddress;
        //}


        //private void CreateFolderGenerate(
        //    (string Repo, string Loca) address,
        //    string name)
        //{
        //    var elemPath = bw.GetElemPath(address);
        //    Directory.CreateDirectory(elemPath);
        //    var dict = new Dictionary<string, object>()
        //    {
        //        { "id", Guid.NewGuid() },
        //        { "type", ItemTypeNames.Folder },
        //        { "name", name }
        //    };
        //    bw.CreateConfig(address, dict);
        //}

        //public (string, string) CreateChildText(
        //    (string Repo, string Loca) address,
        //    string name,
        //    string content)
        //{
        //    var lastNumber = GetFolderLastNumber(address);
        //    var newAddress = fileService.Index.SelectAddress(address, lastNumber + 1);
        //    CreateTextGenerate(newAddress, name, content);
        //    return newAddress;
        //}

        //public (string, string) CreateChildRefText(
        //    (string Repo, string Loca) address,
        //    string name,
        //    string content)
        //{
        //    var lastNumber = GetFolderLastNumber(address);
        //    var newAddress = fileService.Index.SelectAddress(address, lastNumber + 1);
        //    CreateRefTextGenerate(newAddress, name, content);
        //    return newAddress;
        //}

        //public (string, string) CreateText(
        //    (string Repo, string Loca) address,
        //    string content)
        //{
        //    OverrideTextGenerate(address, content);
        //    return address;
        //}

        //private void CreateRefTextGenerate(
        //    (string Repo, string Loca) address,
        //    string name,
        //    string content)
        //{
        //    // directory
        //    var itemPath = pw.GetItemPath(address);
        //    Directory.CreateDirectory(itemPath);

        //    // config
        //    var dict = new Dictionary<string, object>()
        //    {
        //        { "id", Guid.NewGuid().ToString() },
        //        { "type", ItemTypeNames.RefText },
        //        { "name", name }
        //    };
        //    bw.InternalCreateConfig(itemPath, dict);

        //    // body
        //    bw.InternalCreateBody(itemPath, content);
        //}


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
                tww.Put(name, existingItem, content);
                return existingItem;
            }

            var lastNumber = GetFolderLastNumber(address);
            var newAddress = fileService.Index.SelectAddress(address, lastNumber + 1);
            tww.Put(name, newAddress, content);
            return newAddress;
        }

        public void AppendText(
            (string Repo, string Loca) address,
            string content)
        {
            tww.Append(address, content);
        }



        // APPEND
        //-------------------------


        //-------------------------
        // OTHER
        //private bool IsRepoConfig(string path)
        //{
        //    var filePath = path + slash + repoConfigName;
        //    if (File.Exists(filePath))
        //    {
        //        return true;
        //    }

        //    return false;
        //}





        //private int GetLocationLastNumber(string location)
        //{
        //    var lastString = location.Split("/").Last();
        //    int.TryParse(lastString, out var lastNumber);
        //    return lastNumber;
        //}

        //private string IndexToString(int index)
        //{
        //    if (index < 10)
        //    {
        //        return "0" + index;
        //    }
        //    if (index < 100)
        //    {
        //        return index.ToString();
        //    }
        //    if (index < 1000)
        //    {
        //        return index.ToString();
        //    }

        //    throw new Exception();
        //}

        //private int StringToIndex(string numberString)
        //{
        //    var success = int.TryParse(numberString, out int result);

        //    if (!success)
        //    {
        //        return -1;
        //    }

        //    return result;
        //}


        // OTHER
        //-------------------------

        //-------------------------
        // SELECT

        //public int SelectNumberFromLoca(string loca)
        //{
        //    // GetNumberFromLoca
        //    var tmp1 = loca.Split('/').Last();
        //    var number = StringToIndex(tmp1);
        //    return number;
        //}
        // SELECT
        //-------------------------

        //public List<string> GetAllMsgFolders()
        //{
        //    var guid = "ebf8d4ba-06c2-43eb-a201-4d32d13656e4";
        //    var path = localInfo.LocalRootPaths + "/" + guid;
        //    var allDirectories = Directory.GetDirectories(path);
        //    var msg = "Msg";
        //    var msgDirectories = allDirectories.Where(x => Path.GetFileName(x).StartsWith(msg)).ToList();
        //    return msgDirectories;
        //}

        //public (Guid, string) GetRepoFromAgruments(string[] args)
        //{
        //    if (args.Length == 1)
        //    {
        //        var curPath = Environment.CurrentDirectory;
        //        var repo = pw.GetRepoPath(curPath);
        //        return default;
        //    }

        //    if (args.Length == 2 &&
        //    Directory.Exists(args[1]))
        //    {
        //        var repo = pw.GetRepoPath(args[1]);
        //        return default;
        //    }

        //    if (args.Length == 3)
        //    {
        //        //var repo = GetRepo(args[1], args[2]);
        //        return default;
        //    }

        //    return default;
        //}

        //public List<string> GetAllItems(string repoName)
        //{
        //    var repoPath = bw.ReposPathsList.SingleOrDefault(x => Path.GetFileName(x) == repoName);
        //    if (repoPath == null) { throw new Exception(); }

        //    fileService.File.GetNewRecursivelyVisitDirectory();

        //    return null;
        //}

        //public Dictionary<string, string> GetAllIndexesQNames(
        //    (string repo, string loca) address)
        //    => rw.GetIndexesQNames(address);
        //{
        //    var repoPath = pw.GetItemPath(address);
        //    Console.WriteLine($"repoPath: {repoPath}");

        //    if (!Directory.Exists(repoPath))
        //    {
        //        return new Dictionary<string, string>();
        //    }

        //    var subLocasList = GetDirectories(repoPath)
        //        .Select(x => mw.SelectDirToSection(address.loca, x))
        //        .OrderBy(x => x)
        //        .ToList();

        //    var names = new Dictionary<string, string>();
        //    foreach (var subLoca in subLocasList)
        //    {
        //        var subAddress = (address.repo, subLoca);
        //        var name = GetName(subAddress);
        //        var lastInt = fileService.Index.GetLocaLast(subLoca);
        //        var lastString = fileService.Index.IndexToString(lastInt);
        //        names.Add(lastString, name);
        //    }

        //    return names;
        //}

        //public List<(string Repo, string Loca)> GetFolderAdrTupleList(
        //    (string Repo, string Loca) adrTuple)
        //{
        //    var path = pw.GetItemPath(adrTuple);
        //    var directories = GetDirectories(path);
        //    var locaList = directories.Select(x => x.Replace(path, "")).ToList();

        //    var adrTupleList = locaList.Select(x => fileService.RepoAddress.AdrTupleJoinLoca(adrTuple, x))
        //        .ToList();
        //    return adrTupleList;
        //}
    }
}