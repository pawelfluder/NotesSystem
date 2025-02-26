using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;

namespace SharpRepoServiceProg.Workers.CrudWrites.WriteTexts;

internal partial class WriteTextWorker
{
    internal bool IfMineParentPost(
        ref ItemModel item,
        string name,
        (string Repo, string Loca) parentAdrTuple,
        UniType type = UniType.Text)
    {
        if (type != _myUniType) { return false; }
        IfNotInitialized();
        bool s01 = _readMulti.GetItemBySeqOfNames(ref item, parentAdrTuple, name);
        if (s01)
        {
            return false; // new item created = false
        }

        var nextAdrTuple = _readFolder.GetNextAdrTuple(parentAdrTuple);
        item = PrepareItem(name, nextAdrTuple, string.Empty);
        Put(item);
        return true; // already existed = false
    }
}
