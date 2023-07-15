using SharpConfigProg.Preparer;
using SharpConfigProg.Service;
using SharpNotesMigrationProg.Service;
using SharpNotesMigrationTests.Repetition;
using SharpRepoServiceProg.Service;
using System.ComponentModel;
using Unity;

namespace SharpNotesMigrationTests
{
    [TestClass]
    public class UnitTest1
    {
        public void Prepare(Type type)
        {
            var configService = MyBorder.Container.Resolve<IConfigService>();
            configService.Prepare(type);
            var repoService = MyBorder.Container.Resolve<RepoService>();
            repoService.Methods.InitializeSearchFoldersPaths(configService.GetRepoSearchPaths());
        }

        [TestMethod]
        public void TestMethod1()
        {
            // arrange
            Prepare(typeof(IPreparer.ILocalProgramData));
            var migrationService = MyBorder.Container.Resolve<IMigrationService>();

            // act
            migrationService.Migrate(typeof(IMigrationService.IMigrator03));
        }

        [TestMethod]
        public void TestMethod2()
        {
            // arrange
            Prepare(typeof(IPreparer.ILocalProgramData));
            var migrator03 = MyBorder.Container.Resolve<IMigrationService.IMigrator03>();
            var repo = "todo";
            var loca = "02";

            // act
            migrator03.MigrateOneAddress((repo, loca));

            // assert
            var beforeAfter = migrator03.BeforeAfter;
        }

        [TestMethod]
        public void TestMethod3()
        {
            // arrange
            Prepare(typeof(IPreparer.INotesSystem));
            var migrator03 = MyBorder.Container.Resolve<IMigrationService.IMigrator03>();
            var repoServer = MyBorder.Container.Resolve<RepoService>();
            var repoPath = repoServer.Methods.GetRepoPath("Notki");

            // act
            migrator03.MigrateOneRepo(repoPath);

            // assert
            var beforeAfter = migrator03.BeforeAfter;
            beforeAfter.ForEach((x) =>
            {
                Console.WriteLine(x.Item1);
                Console.WriteLine(x.Item2);
                Console.WriteLine(x.Item3);
                Console.WriteLine();
            });
        }
    }
}