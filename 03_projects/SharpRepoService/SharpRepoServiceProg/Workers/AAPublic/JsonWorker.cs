using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.CrudReads;
using SharpRepoServiceProg.Workers.CrudWrites;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.AAPublic;

public class JsonWorker
{
    private readonly CustomOperationsService _customOperationsService;
    private readonly ReadFolderWorker _readFolder;
    private readonly ReadMultiWorker _readMulti;
    private readonly ReadManyWorker _readMany;
    private readonly BodyWorker _body;
    private readonly PathWorker _path;
    private readonly ConfigWorker _config;
    private readonly SystemWorker _system;
    private readonly WriteTextWorker _writeText;
    private readonly WriteFolderWorker _writeFolder;
    private readonly IFileService _fileService;
    private ReadAddressWorker _address;
    private WriteMultiWorker _writeMulti;

    public JsonWorker()
    {
        _fileService = MyBorder.OutContainer.Resolve<IFileService>();
        _customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        
        _readFolder = MyBorder.MyContainer.Resolve<ReadFolderWorker>();
        _readMulti = MyBorder.MyContainer.Resolve<ReadMultiWorker>();
        _writeText = MyBorder.MyContainer.Resolve<WriteTextWorker>();
        _readMany = MyBorder.MyContainer.Resolve<ReadManyWorker>();
        _writeFolder = MyBorder.MyContainer.Resolve<WriteFolderWorker>();
        _writeMulti = MyBorder.MyContainer.Resolve<WriteMultiWorker>();
        _address = MyBorder.MyContainer.Resolve<ReadAddressWorker>();
        _body = MyBorder.MyContainer.Resolve<BodyWorker>();
        _path = MyBorder.MyContainer.Resolve<PathWorker>();
        _config = MyBorder.MyContainer.Resolve<ConfigWorker>();
        _system = MyBorder.MyContainer.Resolve<SystemWorker>();
    }

    public List<string> GetManyItemByName(
        (string Repo, string Loca) address,
        List<string> names)
    {
        // ReadElemListByNames
        address = _address.GetAdrTupleBySequenceOfNames(address, names.ToArray());
        var localPath = _path.GetItemPath(address);
        var folders = _system.GetDirectories(localPath);
        var tmp = folders.Select(x => Path.GetFileName(x));

        var contentsList = new List<string>();

        foreach (var tmp2 in tmp)
        {
            int index = _customOperationsService.Index.StringToIndex(tmp2);
            (string, string) newAddress = _customOperationsService.Index.SelectAddress(address, index);
            var content = _body.GetBody(newAddress);
            // todo - use read worker instead of body worker
            // ItemModel content = rw.GetItem(newAddress);
            // var body = content.Body.ToString();
            contentsList.Add(content);
        }

        return contentsList;
    }

    public string GetItemList(
        (string repo, string loca) adrTuple)
    {
        List<ItemModel> items = _readMany.GetListOfItems(adrTuple);
        string itemList = JsonConvert.SerializeObject(items);
        return itemList;
    }

    public string GetItem(
        (string repo, string loca) adrTuple)
    {
        ItemModel item = _readMulti.GetItem(adrTuple);
        string jsonString = JsonConvert.SerializeObject(item, Formatting.Indented);
        return jsonString;
    }

    public string PostItem(
        (string repo, string loca) address,
        string type,
        string name)
    {
        ItemModel item = _writeMulti.PostItem(address, type, name);
        string result = JsonConvert.SerializeObject(item, Formatting.Indented);
        return result;
    }
    
    public string PutItem(
        (string repo, string loca) address,
        string type,
        string name,
        string body = "")
    {
        ItemModel item = null;
        if (type == UniItemTypes.Text)
        {
            item = _writeText.Put(name, address, body);
        }
        if (type == "Folder")
        {
            item = _writeFolder.Put(name, address);
        }

        var result = JsonConvert.SerializeObject(item, Formatting.Indented);
        return result;
    }
    
    //public string CreateItem(
    //    (string repo, string loca) adrTuple,
    //    string name,
    //    string type)
    //{
    //    string item = "";

    //    if (type == ItemTypes.Text)
    //    {
    //        var newAdrTuple = repo.CreateChildText(adrTuple, name, "");
    //        item = GetItem(newAdrTuple);
    //    }
    //    if (type == ItemTypes.Folder)
    //    {
    //        var newAdrTuple = repo.CreateChildFolder(adrTuple, name);
    //        item = GetItem(newAdrTuple);
    //    }

    //    return item;
    //}

    //public string PutItem(
    //    (string repo, string loca) adrTuple,
    //    string type,
    //    string name,
    //    string body)
    //{
    //    string item = "";

    //    if (type == ItemTypes.Text)
    //    {
    //        // todo add version with name & change name to PutText
    //        var newAdrTuple = repo.CreateText(adrTuple, body);
    //        item = GetItem(newAdrTuple);
    //    }

    //    return item;
    //}
}