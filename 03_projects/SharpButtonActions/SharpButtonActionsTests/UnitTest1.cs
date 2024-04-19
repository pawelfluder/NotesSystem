using SharpButtonActionsProj.Service;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;
using OutBorder1 = SharpFileServiceProg.Repetition.OutBorder;
using OutBorder2 = SharpConfigProg.Repetition.OutBorder;

namespace SharpButtonActionsTests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IFileService fileService;

        public UnitTest1()
        {
            fileService = OutBorder1.FileService();
        }

        [TestMethod]
        public void TestMethod2()
        {
            //https://docs.google.com/document/d/18H_5aGqmrch7M_WCJ49PcA0doRxbLCC_bmULwraspe4/edit
            var id = "18H_5aGqmrch7M_WCJ49PcA0doRxbLCC_bmULwraspe4";
            var url1 = string.Format("https://docs.google.com/document/d/" + "{0}" + "/edit", id);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var buttonActionService = new ButtonActionsService();
            var configService = OutBorder2.ConfigService(fileService);
            configService.Prepare(typeof(IConfigService.ILocalProgramDataPreparer));
            //var repoService = new RepoService(fileService, configService.GetRepoSearchPaths());
            //var repo = "Notki";
            //var loca = "01/02";
            //var path = repoService.Methods.GetElemPath((repo, loca));
            //buttonActionService.OpenFolder(path);
        }
    }
}