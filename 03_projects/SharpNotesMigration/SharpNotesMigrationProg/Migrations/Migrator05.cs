using SharpNotesMigrationProg.AAPublic;
using SharpRepoServiceProg.AAPublic;

namespace SharpNotesMigrationProg.Migrations
{
    internal class Migrator05 : IMigrator, IMigrator05
    {
        private readonly IFileService fileService;
        private readonly IRepoService repoService;
        private readonly IFileService.IYamlOperations yamlOperations;
        private bool agree;

        public string Description
        {
            get
            {
                return @"Previously - there was 4 empty lines at the top of body file.
                         After update - there is no empty lines at the top.";
            }
        }

        // 
        public List<(int, string, string, string)> Changes { get; private set; }

        public Migrator05(
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

            foreach (var foundAddress in foundAddressList)
            {
                MigrateOneAddress(foundAddress);
            }
        }

        private void MigrateOneAddress(
            (string Repo, string Loca) adrTuple)
        {
            var type = repoService.Methods.GetType(adrTuple);

            if (type == "Text")
            {
                var newText = RemoveTopEmptyLines(adrTuple);
                repoService.Methods.PatchText(newText, adrTuple);
            }
        }

        private string RemoveTopEmptyLines((string Repo, string Loca) adrTuple)
        {
            var lines = repoService.Methods.GetTextLines(adrTuple);
            for (var i = 0; i < 4; i++)
            {
                var line = lines[0];
                if (line == string.Empty)
                {
                    lines.RemoveAt(0);
                }

                if (line != string.Empty)
                {
                    break;
                }
            }

            var text = string.Join("\n", lines);

            return text;
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
