﻿using System.Collections.Generic;
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

namespace SharpRepoServiceProg.Workers.Public;

public class JsonWorker
{
    private readonly CustomOperationsService _customOperationsService;
    private readonly ReadWorker rw;
    private readonly BodyWorker bw;
    private readonly PathWorker pw;
    private readonly ConfigWorker cw;
    private readonly SystemWorker sw;
    private readonly WriteTextWorker tww;
    private readonly WriteFolderWorker fww;
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

        tww = MyBorder.MyContainer.Resolve<WriteTextWorker>();
        fww = MyBorder.MyContainer.Resolve<WriteFolderWorker>();
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
        var items = rw.GetItemList(adrTuple);
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

    //public Dictionary<string, object> GetItemDict(
    //    (string repo, string loca) adrTuple)
    //{
    //    var dict = new Dictionary<string, object>();

    //    try
    //    {
    //        var item = rw.GetItem(adrTuple, true);

    //        dict.Add("Type", item.Type);
    //        dict.Add("Name", item.Name);
    //        dict.Add("Config", item.Settings);
    //        dict.Add("Address", item.Address);

    //        object body = null;
    //        if (item.Type == ItemTypeNames.Text)
    //        {
    //            body = item.Body;
    //        }
    //        if (item.Type == ItemTypeNames.Folder)
    //        {
    //            body = rw.GetIndexesQNames2(adrTuple);
    //        }
    //        if (item.Type == ItemTypeNames.RefText)
    //        {
    //            // todo
    //        }
    //        dict.Add("Body", body);

    //        return dict;
    //    }
    //    catch (Exception ex)
    //    {
    //        return dict;
    //    }
    //}

    public string PostItem(
        (string repo, string loca) address,
        string type,
        string name)
    {
        ItemModel item = null;
        if (type == UniItemTypes.Text)
        {
            item = tww.InternalPost(name, address);
        }
        if (type == UniItemTypes.Folder)
        {
            item = fww.InternalPost(name, address);
        }

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
            item = tww.Put(name, address, body);
        }
        if (type == "Folder")
        {
            item = fww.Put(name, address);
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