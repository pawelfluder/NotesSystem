﻿using System;
using System.Collections.Generic;
using SharpFileServiceProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceProg.Workers.AAPublic;
using SharpRepoServiceProg.Workers.APublic;
using SharpRepoServiceProg.Workers.APublic.ItemWorkers;
using ItemWorker = SharpRepoServiceProg.Workers.APublic.ItemWorkers.ItemWorker;

namespace SharpRepoServiceProg.Service;

internal class RepoService : IRepoService
{
    private readonly IFileService _fileService;
    private Lazy<ItemWorker> _item;
    private Lazy<ManyItemsWorker> _manyItems;
    private Lazy<MethodWorker> _methods;
    public IItemWorker Item => _item.Value;
    public ManyItemsWorker ManyItems => _manyItems.Value;
    public MethodWorker Methods => _methods.Value;

    internal RepoService(
        IFileService fileService)
    {
        _fileService = fileService;
        _item = new Lazy<ItemWorker>(
            () => new ItemWorker());
        _manyItems = new Lazy<ManyItemsWorker>(
            () => new ManyItemsWorker());
        _methods = new Lazy<MethodWorker>(
            () => new MethodWorker(_fileService));
    }

    public void InitGroupsFromSearchPaths(
        List<string> searchPaths)
    {
        Methods.InitGroupsFromSearchPaths(searchPaths);

        if (!(Methods.GetReposCount() > 0))
        {
            throw new Exception();
        }
    }
    
    public (string Repo, string Loca) GetFirstRepo()
    {
        (string Repos, string Loca) adrTuple = Methods.GetFirstRepo();
        return adrTuple;
    }
}
