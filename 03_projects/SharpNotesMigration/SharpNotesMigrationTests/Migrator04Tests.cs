using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpNotesMigrationTests.Registration;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

namespace SharpNotesMigrationTests
{
    [TestClass]
    public class Migrator04Tests
    {
        public Migrator04Tests()
        {
            OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
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
            var adrTuple = ("Winder2", "");
            var agree = true;

            // act
            migrationService.MigrateOneFolder(typeof(IMigrator04), adrTuple, agree);
        }
    }
}