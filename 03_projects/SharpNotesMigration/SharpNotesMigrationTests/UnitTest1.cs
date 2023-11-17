using SharpConfigProg.Service;
using SharpNotesMigrationProg.Service;
using SharpNotesMigrationTests.Repetition;
using SharpRepoServiceProg.Service;
using Unity;

namespace SharpNotesMigrationTests
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            // WpfNotesSystemPrivate01.Repetition
            var registration = new SharpSetupProg21Private.AAPublic.Registration();
            registration.Start();
            var configService = MyBorder.Container.Resolve<IConfigService>();
            configService.Prepare();
            var repoService = MyBorder.Container.Resolve<IRepoService>();
            repoService.Initialize(configService.GetRepoSearchPaths());
        }

        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            var migrationService = MyBorder.Container.Resolve<IMigrationService>();

            // act
            migrationService.Migrate(typeof(IMigrationService.IMigrator03));
        }

        [TestMethod]
        public void TestMethod2()
        {
            // arrange
            var migrator03 = MyBorder.Container.Resolve<IMigrationService.IMigrator03>();
            var repo = "todo";
            var loca = "02";

            // act
            migrator03.MigrateOneAddress((repo, loca));

            // assert
            var beforeAfter = migrator03.Changes;
        }

        [TestMethod]
        public void MigrateOneFolderRecoursively()
        {
            // arrange
            var migrator03 = MyBorder.Container.Resolve<IMigrationService.IMigrator03>();
            migrator03.SetAgree(true);
            var repoServer = MyBorder.Container.Resolve<IRepoService>();
            var repoName = "Rama";
            var loca = "";
            //var repoName = "02_appData";
            var address = (repoName, loca);
            var folderPath = repoServer.Methods.GetElemPath((repoName, loca));

            // act
            migrator03.MigrateOneFolderRecourively(address);

            // assert
            var beforeAfter = migrator03.Changes;
            beforeAfter.ForEach((x) =>
            {
                Console.WriteLine(x.Item1);
                Console.WriteLine(x.Item2);
                Console.WriteLine(x.Item3);
                Console.WriteLine(x.Item4);
                Console.WriteLine();
            });
        }

        [TestMethod]
        public void MigrateOneRepo()
        {
            // arrange
            var migrator03 = MyBorder.Container.Resolve<IMigrationService.IMigrator03>();
            migrator03.SetAgree(true);
            var repoServer = MyBorder.Container.Resolve<IRepoService>();
            var repoName = "Notki";
            var address = (repoName, "");
            //var repoName = "02_appData";
            //var repoPath = repoServer.Methods.GetRepoPath(repoName);

            // act
            migrator03.MigrateOneRepo(address);

            // assert
            
            var beforeAfter = migrator03.Changes;
            beforeAfter.ForEach((x) =>
            {
                Console.WriteLine(x.Item1);
                Console.WriteLine(x.Item2);
                Console.WriteLine(x.Item3);
                Console.WriteLine(x.Item4);
                Console.WriteLine();
            });
        }
    }
}