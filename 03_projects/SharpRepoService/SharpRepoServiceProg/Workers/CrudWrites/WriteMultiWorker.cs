using System;
using SharpRepoServiceProg.AAPublic.Names;
using SharpRepoServiceProg.Models;
using SharpRepoServiceProg.Operations;
using SharpRepoServiceProg.Registration;
using SharpRepoServiceProg.Workers.CrudReads;
using SharpRepoServiceProg.Workers.System;

namespace SharpRepoServiceProg.Workers.CrudWrites;

public class WriteMultiWorker
{
    private readonly PathWorker pw;
    private readonly SystemWorker _sw;
    private readonly ConfigWorker _cw;
    private readonly BodyWorker _bw;
    private readonly ReadFolderWorker _readFolder;
    private readonly ReadTextWorker _readText;
    private readonly WriteTextWorker _writeText;
    private readonly WriteFolderWorker _writeFolder;
    
    private UniType _myType = UniType.Text;

    public WriteMultiWorker()
    {
        _readFolder = MyBorder.MyContainer.Resolve<ReadFolderWorker>();
        _readText = MyBorder.MyContainer.Resolve<ReadTextWorker>();
        _writeFolder = MyBorder.MyContainer.Resolve<WriteFolderWorker>();
        _writeText = MyBorder.MyContainer.Resolve<WriteTextWorker>();
        _bw = MyBorder.MyContainer.Resolve<BodyWorker>();
        _cw = MyBorder.MyContainer.Resolve<ConfigWorker>();
        _sw = MyBorder.MyContainer.Resolve<SystemWorker>();
    }

    public ItemModel PostItem(
        (string repo, string loca) adrTuple,
        string type,
        string name)
    {
        bool isKnownType = Enum.TryParse<UniType>(type, out var uniType);
        ItemModel item = new();
        if (!isKnownType) { return item; }
        
        item = _writeText.TryInternalPost(item, name, adrTuple, uniType);
        item = _writeFolder.TryInternalPost(item, name, adrTuple, uniType);
        return item;
    }
}
