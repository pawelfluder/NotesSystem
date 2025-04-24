using System;
using System.Collections.Generic;

namespace SharpRepoServiceProg.Workers.APublic.ItemWorkers;

public interface IItemWorker
{
    List<string> GetManyItemByName(
        (string Repo, string Loca) address,
        List<string> names);

    string GetItem(
        (string Repo, string Loca) adrTuple);

    string GetItemBySeqOfNames(
        (string Repo, string Loca) adrTuple,
        params string[] names);

    string GetBody(
        (string repo, string loca) adrTuple);

    string PutItem(
        (string repo, string loca) adrTuple,
        string type,
        string name,
        string body = "");

    void PutConfig(
        (string repo, string loca) adrTuple,
        Dictionary<string, object> config);

    string GetByGuid(
        string repoName,
        Guid guid);

    string PostParentItem(
        (string Repo, string Loca) adrTuple,
        string type,
        string name);
}