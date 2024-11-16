using SharpConfigProg.Service;
using SharpRepoServiceProg.AAPublic;
using SharpTtsServiceProg.AAPublic;
using SharpTtsServiceTests.Registration;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;
using OutBorder02 = SharpTtsServiceProg.AAPublic.OutBorder;

namespace SharpTtsServiceTests;

public class Tests
{
    private readonly ITtsService _ttsService;

    public Tests()
    {
        IConfigService configService = MyBorder.Container.Resolve<IConfigService>();
        IRepoService repoService = MyBorder.Container.Resolve<IRepoService>();
        
        OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
        configService.Prepare();
        repoService.PutPaths(configService.GetRepoSearchPaths());

        //_ttsService = MyBorder.Container.Resolve<ITtsService>();
    }
    
    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public void Test1()
    {
        //_ttsService.Tts.SaveAudioFile()
        var gg = 0;
    }
}