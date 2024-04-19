using SharpGoogleSheetProg.AAPublic;
using SharpGoogleSheetTests.Registrations;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

namespace SharpGoogleSheetTests
{
    [TestClass]
    public class UnitTest1
    {
        IGoogleSheetService sheetService;

        public UnitTest1()
        {
            OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
            sheetService = MyBorder.GoogleSheetService();
        }

        [TestMethod]
        public void TestMethod1()
        {
            // create get remove spreadsheet
            var id = "1ju5Im_BaQURcmKFhf1rr0hOkDpk_68Lvi8IZ4n_oVSw";
            var result = sheetService.Worker.GetSpreadsheet(id);
        }
    }
}