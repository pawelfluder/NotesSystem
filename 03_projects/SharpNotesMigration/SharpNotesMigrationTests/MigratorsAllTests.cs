using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpNotesMigrationTests.Registration;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using OutBorder01 = SharpSetup01Prog.AAPublic.OutBorder;
using OutBorder02 = SharpNotesMigrationProg.AAPublic.OutBorder;

namespace SharpNotesMigrationTests;

[TestClass]
public class MigratorAllTests
{
    public MigratorAllTests()
    {
        OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
        var operationService = MyBorder.OutContainer.Resolve<IOperationsService>();
        var repoService = MyBorder.OutContainer.Resolve<IRepoService>();
        var migrationService = OutBorder02.MigrationService(operationService, repoService);
        MyBorder.OutContainer.RegisterByFunc(() => migrationService);
    }

    [TestMethod]
    public void MigrateOneFolder()
    {
        // arrange
        var migrationService = MyBorder.OutContainer.Resolve<IMigrationService>();
        var adrTuple = ("Persistency", "");
        var agree = true;

        // act
        migrationService.MigrateOneFolder(typeof(IMigrator01), adrTuple, agree);
        migrationService.MigrateOneFolder(typeof(IMigrator02), adrTuple, agree);
        migrationService.MigrateOneFolder(typeof(IMigrator03), adrTuple, agree);
        migrationService.MigrateOneFolder(typeof(IMigrator04), adrTuple, agree);
        migrationService.MigrateOneFolder(typeof(IMigrator05), adrTuple, agree);
    }
}