using Newtonsoft.Json;
using SharpFileServiceProg.Service;
using SharpRepoServiceCoreProj;
using SharpRepoServiceProg.Names;
using SharpTinderComplexTests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SharpRepoServiceProg.RepoOperations
{
    public class ItemWorker
    {
        // string names
        private string contentFileName;
        private string configFileName;
        private string repoConfigName;
        private string errorValue;
        private string refGuidStr;
        private string refLocaStr;

        // char names
        private static char slash = '/';

        private readonly ServerInfo serverInfo;
        private readonly LocalInfo localInfo;
        private readonly IFileService fileService;


        private List<string> reposPathsList;
        private RepoWorker repo;

        public ItemWorker(
            RepoWorker repoWorker,
            IFileService fileService)
        {
            this.repo = repoWorker;
            this.fileService = fileService;
            reposPathsList = new List<string>();
        }

        public List<string> GetManyItemByName(
            (string Repo, string Loca) address,
            List<string> names)
        {
            // ReadElemListByNames
            address = repo.GetAdrTupleByNameList(address, names);
            var localPath = repo.GetElemPath(address);
            var folders = repo.GetDirectories(localPath);
            var tmp = folders.Select(x => Path.GetFileName(x));

            var contentsList = new List<string>();

            foreach (var tmp2 in tmp)
            {
                var index = fileService.Index.StringToIndex(tmp2);
                var newAddress = fileService.Index.SelectAddress(address, index);
                var content = repo.GetText2(newAddress);
                contentsList.Add(content);
            }

            return contentsList;
        }

        public string GetItemList(
            (string repo, string loca) adrTuple)
        {
            var adrTupleList = repo.GetFolderAdrTupleList(adrTuple);
            var itemJsonObjList = adrTupleList.Select(x => GetItemDict(x)).ToList();
            var itemList = JsonConvert.SerializeObject(itemJsonObjList);
            return itemList;
        }

        public string GetItem(
            (string repo, string loca) adrTuple)
        {
            var dict = GetItemDict(adrTuple);
            var item = JsonConvert.SerializeObject(dict);
            return item;
        }

        public string CreateItem(
            (string repo, string loca) adrTuple,
            string name,
            string type)
        {
            string item = "";

            if (type == ItemTypes.Text)
            {
                var newAdrTuple = repo.CreateChildText(adrTuple, name, "");
                item = GetItem(newAdrTuple);
            }
            if (type == ItemTypes.Folder)
            {
                var newAdrTuple = repo.CreateChildFolder(adrTuple, name);
                item = GetItem(newAdrTuple);
            }

            return item;
        }

        public string PutItem(
            (string repo, string loca) adrTuple,
            string type,
            string name,
            string body)
        {
            string item = "";

            if (type == ItemTypes.Text)
            {
                // todo add version with name & change name to PutText
                var newAdrTuple = repo.CreateText(adrTuple, body);
                item = GetItem(newAdrTuple);
            }

            return item;
        }

        public Dictionary<string, object> GetItemDict(
            (string repo, string loca) adrTuple)
        {
            var dict = new Dictionary<string, object>();

            try
            {
                var type = repo.GetItemType(adrTuple);
                object body = null;

                if (type == ItemTypeNames.Text)
                {
                    body = repo.GetText2(adrTuple);
                }

                if (type == ItemTypeNames.RefText)
                {
                    var refAdrTuple = repo.GetRefAdrTuple(adrTuple);
                    body = repo.GetText2(refAdrTuple);
                }

                if (type == ItemTypeNames.Folder)
                {
                    body = repo.GetAllIndexesQNames(adrTuple);
                }
                var name = repo.GetName(adrTuple);

                var config = repo.GetConfigDictionary(adrTuple);
                var address = fileService.Index.GetAddressString(adrTuple);

                
                dict.Add("Type", type);
                dict.Add("Name", name);
                dict.Add("Body", body);
                dict.Add("Config", config);
                dict.Add("Address", address);

                return dict;
            }
            catch (Exception ex)
            {
                return dict;
            }
        }
    }
}
