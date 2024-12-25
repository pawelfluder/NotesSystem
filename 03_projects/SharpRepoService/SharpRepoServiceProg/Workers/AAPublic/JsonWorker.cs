using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Names;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.Crud;
using SharpRepoServiceProg.Workers.System;
using NotImplementedException = System.NotImplementedException;

namespace SharpRepoServiceProg.Workers.Public;

public class JsonWorker
{
    private readonly CustomOperationsService _customOperationsService;
    private readonly ReadWorker rw;
    private readonly BodyWorker bw;
    private readonly PathWorker pw;
    private readonly ConfigWorker cw;
    private readonly SystemWorker sw;
    private readonly WriteTextWorker writeText;
    private readonly WriteFolderWorker writeFolder;
    private readonly IFileService _fileService;

    public JsonWorker()
    {
        _fileService = MyBorder.OutContainer.Resolve<IFileService>();
        _customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        rw = MyBorder.MyContainer.Resolve<ReadWorker>();
        bw = MyBorder.MyContainer.Resolve<BodyWorker>();
        pw = MyBorder.MyContainer.Resolve<PathWorker>();
        cw = MyBorder.MyContainer.Resolve<ConfigWorker>();
        sw = MyBorder.MyContainer.Resolve<SystemWorker>();

        writeText = MyBorder.MyContainer.Resolve<WriteTextWorker>();
        writeFolder = MyBorder.MyContainer.Resolve<WriteFolderWorker>();
    }

    public List<string> GetManyItemByName(
        (string Repo, string Loca) address,
        List<string> names)
    {
        // ReadElemListByNames
        address = rw.GetAdrTupleByNames(address, names.ToArray());
        var localPath = pw.GetItemPath(address);
        var folders = sw.GetDirectories(localPath);
        var tmp = folders.Select(x => Path.GetFileName(x));

        var contentsList = new List<string>();

        foreach (var tmp2 in tmp)
        {
            var index = _customOperationsService.Index.StringToIndex(tmp2);
            var newAddress = _customOperationsService.Index.SelectAddress(address, index);
            var content = bw.GetText2(newAddress);
            contentsList.Add(content);
        }

        return contentsList;
    }

    public string GetItemList(
        (string repo, string loca) adrTuple)
    {
        var items = rw.GetListOfItems(adrTuple);
        var itemList = JsonConvert.SerializeObject(items);
        return itemList;
    }

    public string GetItem(
        (string repo, string loca) adrTuple)
    {
        //var dict = GetItemDict(adrTuple);
        var item = rw.GetItem(adrTuple, true);
        var jsonString = JsonConvert.SerializeObject(item, Formatting.Indented);
        return jsonString;
    }

    public string PostItem(
        (string repo, string loca) address,
        string type,
        string name)
    {
        bool isKnownType = Enum.TryParse<UniItemTypesEnum>(type, out var enumType);
        if (!isKnownType) { return string.Empty; }
        
        ItemModel item = null;
        item = writeText.TryInternalPost(item, name, address, enumType);
        item = writeFolder.TryInternalPost(item, name, address, enumType);

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
            item = writeText.Put(name, address, body);
        }
        if (type == "Folder")
        {
            item = writeFolder.Put(name, address);
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