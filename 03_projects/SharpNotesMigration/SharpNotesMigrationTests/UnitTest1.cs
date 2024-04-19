using SharpConfigProg.Service;
using SharpNotesMigrationProg.AAPublic;
using SharpNotesMigrationProg.Service;
using SharpNotesMigrationTests.Repetition;
using SharpRepoServiceProg.AAPublic;
using Unity;
using OutBorder01 = SharpSetup21ProgPrivate.AAPublic.OutBorder;

namespace SharpNotesMigrationTests
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            OutBorder01.GetPreparer("PrivateNotesPreparer").Prepare();
            var configService = MyBorder.Container.Resolve<IConfigService>();
            configService.Prepare();
            var repoService = MyBorder.Container.Resolve<IRepoService>();
            repoService.Initialize(configService.GetRepoSearchPaths());
        }

        [TestMethod]
        public void TestMethod3()
        {
            // arrange
            var migrationService = MyBorder.Container.Resolve<IMigrationService>();
            var adrTuple = ("Notki", "");
            var agree = true;

            // act
            migrationService.MigrateOneFolder(typeof(IMigrator03), adrTuple, agree);
            migrationService.MigrateOneFolder(typeof(IMigrator04), adrTuple, agree);
        }

        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            var migrationService = MyBorder.Container.Resolve<IMigrationService>();

            // act
            migrationService.MigrateOneRepo(typeof(IMigrator03), "Notes");
        }

        [TestMethod]
        public void TestMethod2()
        {
            // arrange
            var migrator03 = MyBorder.Container.Resolve<IMigrator03>();
            var repo = "Notki";
            var loca = "";

            // act
            migrator03.MigrateOneAddress((repo, loca));

            // assert
            var beforeAfter = migrator03.Changes;
        }

        [TestMethod]
        public void MigrateOneFolderRecoursively()
        {
            // arrange 1
            var repoName = "System";
            var loca = "";
            var agree = true;

            // arrange 2
            var migrator03 = MyBorder.Container.Resolve<IMigrator03>();
            migrator03.SetAgree(agree);
            var repoServer = MyBorder.Container.Resolve<IRepoService>();

            var address = (repoName, loca);
            var folderPath = repoServer.Methods.GetElemPath((repoName, loca));

            // act
            //migrator03.MigrateOneFolder(address);

            //// print
            //var beforeAfter = migrator03.Changes;
            //beforeAfter.ForEach((x) =>
            //{
            //    Console.WriteLine(x.Item1);
            //    Console.WriteLine(x.Item2);
            //    Console.WriteLine(x.Item3);
            //    Console.WriteLine(x.Item4);
            //    Console.WriteLine();
            //});
        }

        [TestMethod]
        public void MigrateOneRepo()
        {
            // arrange
            var migrator03 = MyBorder.Container.Resolve<IMigrator03>();
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