using SharpButtonActionsProj.Service;
using SharpConfigProg.Service;
using SharpFileServiceProg.Service;
using SharpRepoServiceProg.Service;

namespace SharpButtonActionsTests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly FileService fileService;

        public UnitTest1()
        {
            fileService = new FileService();
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
            var configService = new ConfigService(fileService);
            configService.PrepareForWidner();
            var repoService = new RepoService(fileService, configService.GetRepoRootPaths());
            var repo = "Notki";
            var loca = "01/02";
            var path = repoService.Methods.GetElemPath((repo, loca));
            buttonActionService.OpenFolder(path);
        }
    }
}