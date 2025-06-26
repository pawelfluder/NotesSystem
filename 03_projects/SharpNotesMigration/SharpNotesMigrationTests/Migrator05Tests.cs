using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpNotesMigrationTests.Registration;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;

namespace SharpNotesMigrationTests;

[TestClass]
public class Migrator05Tests
{
    public Migrator05Tests()
    {
        OutBorder01.DefaultPreparer("PrivateNotesPreparer").Prepare();
    }

    [TestMethod]
    public void MigrateOneAddress()
    {
        // arrange
        var migrationService = MyBorder.OutContainer.Resolve<IMigrationService>();
        var adrTuple = ("Winder2", "01/03/01/110");
        var agree = true;

        // act
        migrationService.MigrateOneAddress(typeof(IMigrator05), adrTuple, agree);
    }

    [TestMethod]
    public void TestMethod3()
    {
        // arrange
        var migrationService = MyBorder.OutContainer.Resolve<IMigrationService>();
        var adrTuple = ("Winder2", "01/03/01");
        var agree = true;

        // act
        migrationService.MigrateOneFolder(typeof(IMigrator05), adrTuple, agree);
    }
}