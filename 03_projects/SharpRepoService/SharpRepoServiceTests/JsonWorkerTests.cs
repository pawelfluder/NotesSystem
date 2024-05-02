using SharpFileServiceProg.Service;
using SharpRepoServiceProg.AAPublic;
using SharpRepoServiceTests.Registration;
using OutBorder03 = SharpSetup01Prog.AAPublic.OutBorder;

namespace SharpRepoServiceTests
{
    [TestClass]
    public class JsonWorkerTests
    {
        private readonly IFileService fileService;
        private readonly IRepoService repoService;

        public JsonWorkerTests()
        {
            OutBorder03.GetPreparer("DefaultPreparer").Prepare();
            repoService = MyBorder.Container.Resolve<IRepoService>();
        }

        [TestMethod]
        public void TestMethod4()
        {
            var count = repoService.Methods.GetReposCount();

            var adrTuple = ("Notes", "");
            var json = repoService.Item.CreateItem(adrTuple, "Text", "Test01");

            var json02 = repoService.Item.CreateItem(adrTuple, "Folder", "Folder-04");
        }
    }
}