using SharpArgsManagerProj.AAPublic;
using SharpArgsManagerTests.Registrations;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

namespace SharpArgsManagerTests;

public class Tests
{
    private IArgsManagerService _argsManager;
    
    [SetUp]
    public void Setup()
    {
        OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
        _argsManager = MyBorder.OutContainer.Resolve<IArgsManagerService>();
    }

    [Test]
    public void Test1()
    {
        string[] args =
        [
            nameof(IScriptForGameActive),
            nameof(IScriptForGameActive.GameSynchService),
            "2024"
        ];
        _argsManager.Resolve(args);
    }
}