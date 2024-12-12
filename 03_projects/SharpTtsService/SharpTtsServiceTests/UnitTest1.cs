using SharpConfigProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.AAPublic;
using SharpTtsServiceTests.Registration;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace SharpTtsServiceTests;

public class Tests
{
    private readonly ITtsService _ttsService;
    private readonly IRepoService _repoService;

    public Tests()
    {
        OutBorder01.GetPreparer("DefaultPreparer").Prepare();
        IConfigService configService = MyBorder.OutContainer.Resolve<IConfigService>();
        _repoService = MyBorder.OutContainer.Resolve<IRepoService>();
        _ttsService = MyBorder.OutContainer.Resolve<ITtsService>();
    }
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        //_ttsService.Tts.SaveAudioFile()
        (string Repo, string Loca) adrTuple = ("Notki", "");
        string? gg2 = _repoService.Item.GetItem(adrTuple);
        _ttsService.RepoTts.PlStartNew(adrTuple.Repo, adrTuple.Loca);
    }
}
