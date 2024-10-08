using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpNotesMigrationTests.Registration;
using SharpOperationsProg.AAPublic.Operations;
using SharpRepoServiceProg.AAPublic;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;
using OutBorder02 = SharpNotesMigrationProg.AAPublic.OutBorder;

namespace SharpNotesMigrationTests;

[TestClass]
public class Migrator04Tests
{
    public Migrator04Tests()
    {
        OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
        var operationService = MyBorder.Container.Resolve<IOperationsService>();
        var repoService = MyBorder.Container.Resolve<IRepoService>();
        var migrationService = OutBorder02.MigrationService(operationService, repoService);
        MyBorder.Registration.RegisterByFunc(() => migrationService);
    }

    [TestMethod]
    public void MigrateOneAddress()
    {
        // arrange
        var migrationService = MyBorder.Container.Resolve<IMigrationService>();
        var adrTuple = ("Winder2", "01/03/01/110");
        var agree = true;

        // act
        migrationService.MigrateOneAddress(typeof(IMigrator04), adrTuple, agree);
    }

    [TestMethod]
    public void MigrateOneFolder()
    {
        // arrange
        var migrationService = MyBorder.Container.Resolve<IMigrationService>();
        var adrTuple = ("Persistency", "");
        var agree = true;

        // act
        migrationService.MigrateOneFolder(typeof(IMigrator04), adrTuple, agree);
    }
}