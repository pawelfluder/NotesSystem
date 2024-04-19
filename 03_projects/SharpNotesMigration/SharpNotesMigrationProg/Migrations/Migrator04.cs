using SharpFileServiceProg.Service;
using SharpNotesMigrationProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using System.Net;

namespace SharpNotesMigrationProg.Migrations
{
    internal class Migrator04 : IMigrator, IMigrator04
    {
        private readonly IFileService fileService;
        private readonly IRepoService repoService;
        private readonly IFileService.IYamlOperations yamlOperations;
        private bool agree;

        public List<(int, string, string, string)> Changes { get; private set; }

        public Migrator04(
            IFileService fileService,
            IRepoService repoService)
        {
            this.fileService = fileService;
            this.repoService = repoService;
            yamlOperations = fileService.Yaml.Custom03;
            Changes = new List<(int, string, string, string)>();
        }

        public void MigrateEverything()
        {
            throw new NotImplementedException();
        }

        public void MigrateOneFolder((string Repo, string Loca) adrTuple)
        {
            var foundAddressList = repoService.Methods
                .GetAllRepoAddresses(adrTuple).ToList();

            MigrateOneAddress(adrTuple);

            //MigrateOneAddress(address);
            foreach (var foundAddress in foundAddressList)
            {
                MigrateOneAddress(foundAddress);
            }
        }

        private void MigrateOneAddress((string Repo, string Loca) adrTuple)
        {
            var dict = repoService.Methods.GetConfigKeyDict(adrTuple);
            var type = repoService.Methods.GetType(adrTuple);
            var s1 = dict.TryAdd("id", Guid.NewGuid().ToString());
            var s2 = dict.TryAdd("type", type);

            if (agree)
            {
                repoService.Methods.CreateConfig(adrTuple, dict);
            }
        }

        void IMigrator.MigrateOneAddress((string Repo, string Loca) address)
        {
            throw new NotImplementedException();
        }

        public void MigrateOneRepo(string repoName)
        {
            throw new NotImplementedException();
        }

        public void MigrateAllRepos()
        {
            throw new NotImplementedException();
        }

        public void SetAgree(bool agree)
        {
            this.agree = agree;
        }
    }
}
