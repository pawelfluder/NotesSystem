using SharpNotesMigrationProg.AAPublic;
using SharpRepoServiceProg.AAPublic;
using System.Net;

namespace SharpNotesMigrationProg.Migrations
{
    internal class Migrator04 : IMigrator, IMigrator04
    {
        private readonly IOperationsService operationsService;
        private readonly IRepoService repoService;
        private readonly IoperationsService.IYamlOperations yamlOperations;
        private bool agree;

        public string Description
        {
            get
            {
                return @"Previously - there was no ""id"" and ""type"" in config.
                         After update - those two values are added to config file.";
            }
        }

        public List<(int, string, string, string)> Changes { get; private set; }

        public Migrator04(
            IOperationsService operationsService,
            IRepoService repoService)
        {
            this.operationsService = operationsService;
            this.repoService = repoService;
            yamlOperations = operationsService.Yaml.Custom03;
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

        public void MigrateOneAddress((string Repo, string Loca) adrTuple)
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

        //void IMigrator.MigrateOneAddress((string Repo, string Loca) adrTuple)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
