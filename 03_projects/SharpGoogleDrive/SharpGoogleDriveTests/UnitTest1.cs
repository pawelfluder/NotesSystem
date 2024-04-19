using SharpGoogleDriveProg.AAPublic;
using SharpGoogleDriveTests.Registrations;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

namespace SharpGoogleDriveTests
{
    [TestClass]
    public class UnitTest1
    {
        IGoogleDriveService driveService;

        public UnitTest1()
        {
            OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
            driveService = MyBorder.Container.Resolve<IGoogleDriveService>();
        }

        [TestMethod]
        public void TestMethod1()
        {
            var id = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
            var name = "crypto";
            var q1 = $"ids='{id}'";
            var q2 = $"name='{name}'";
            var g3 = "mimeType = 'application/vnd.google-apps.spreadsheet'";
            var g4 = $"mimeType = 'application/vnd.google-apps.spreadsheet' and '{id}' in id";

            //var result01 = driveService.Worker.GetFileById(id);
            var result02 = driveService.Worker.GetFilesRequest(g4);
            var result03 = driveService.Worker.GetFileByName(name);

        }

        [TestMethod]
        public void Method_GetSpreadSheetList()
        {
            // arrange
            var id1 = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
            var id2 = "1lKvCqllFGcALi9PpuA8H9XMRN2QeOLH7VQni-k2pMIM";
            var idsList = new List<string>() { id1, id2 };

            // act
            var result02 = driveService.Worker.GetSpreadSheetsList(idsList);

            // assert
            Assert.AreEqual(2, result02.Count());
        }
    }
}