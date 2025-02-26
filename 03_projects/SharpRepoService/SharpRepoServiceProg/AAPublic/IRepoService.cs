﻿using System.Collections.Generic;
using SharpRepoServiceProg.Workers.AAPublic;
using SharpRepoServiceProg.Workers.APublic;
using ItemWorker = SharpRepoServiceProg.Workers.APublic.ItemWorkers.ItemWorker;

namespace SharpRepoServiceProg.AAPublic;

public interface IRepoService
{
    ItemWorker Item { get; }
    
    ManyItemsWorker ManyItems { get; }
    
    MethodWorker Methods { get; }

    void InitGroupsFromSearchPaths(List<string> searchPaths);

    (string Repo, string Loca) GetFirstRepo();
}
