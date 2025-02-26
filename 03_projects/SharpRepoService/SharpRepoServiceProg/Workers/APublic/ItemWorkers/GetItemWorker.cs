﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.CrudReads;
using SharpRepoServiceProg.Workers.CrudWrites;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.APublic.ItemWorkers;

public partial class ItemWorker
{
    private readonly CustomOperationsService _customOperationsService;
    private readonly ReadMultiWorker _readMulti;
    private readonly BodyWorker _body;
    private readonly PathWorker _path;
    private readonly ConfigWorker _config;
    private readonly SystemWorker _system;
    private ReadAddressWorker _address;
    private WriteMultiWorker _writeMulti;

    public ItemWorker()
    {
        _customOperationsService = MyBorder.MyContainer.Resolve<CustomOperationsService>();
        _readMulti = MyBorder.MyContainer.Resolve<ReadMultiWorker>();
        _writeMulti = MyBorder.MyContainer.Resolve<WriteMultiWorker>();
        _address = MyBorder.MyContainer.Resolve<ReadAddressWorker>();
        _body = MyBorder.MyContainer.Resolve<BodyWorker>();
        _path = MyBorder.MyContainer.Resolve<PathWorker>();
        _config = MyBorder.MyContainer.Resolve<ConfigWorker>();
        _system = MyBorder.MyContainer.Resolve<SystemWorker>();
        _guid = MyBorder.MyContainer.Resolve<GuidWorker>();
    }

    public List<string> GetManyItemByName(
        (string Repo, string Loca) address,
        List<string> names)
    {
        // ReadElemListByNames
        address = _address.GetAdrTupleBySequenceOfNames(address, names.ToArray());
        string localPath = _path.GetItemPath(address);
        string[] folders = _system.GetDirectories(localPath);
        List<string> tmp = folders
            .Select(x => Path.GetFileName(x))
            .ToList();

        List<string> contentsList = new();

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

    public string GetItem(
        (string Repo, string Loca) adrTuple)
    {
        ItemModel item = new();
        bool s01 = _readMulti.GetItem(ref item, adrTuple);
        if (s01)
        {
            string jsonString = JsonConvert.SerializeObject(item, Formatting.Indented);
            return jsonString;
        }
        return string.Empty;
    }

    public string GetItemBySeqOfNames(
        (string Repo, string Loca) adrTuple,
        params string[] names)
    {
        ItemModel item = new();
        bool s01 = _readMulti.GetItemBySeqOfNames(ref item, adrTuple, names);
        
        if (s01)
        {
            string json = JsonConvert.SerializeObject(item, Formatting.Indented);
            return json;
        }
        return string.Empty;
    }
    
    public string GetBody(
        (string repo, string loca) adrTuple)
    {
        ItemModel item = new();
        bool s01 = _readMulti.GetItem(ref item, adrTuple);
        string jsonString = JsonConvert.SerializeObject(item.Body, Formatting.Indented);
        return jsonString;
    }
}
