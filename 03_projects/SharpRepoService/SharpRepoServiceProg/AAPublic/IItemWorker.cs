using System;
using System.Collections.Generic;

namespace SharpRepoServiceProg.AAPublic;

public interface IItemWorker
{
    List<string> GetManyItemByName(
        (string Repo, string Loca) address,
        List<string> names);

    string GetItem(string repo, string loca);

    string AppendLine(
        string repo,
        string loca,
        string content,
        string position);

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
        string repo,
        string loca,
        string type,
        string name);
}