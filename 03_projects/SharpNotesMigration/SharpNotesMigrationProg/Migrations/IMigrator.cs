namespace SharpNotesMigrationProg.Migrations
{
    internal interface IMigrator
    {
        void SetAgree(bool agree);

        void MigrateOneAddress((string Repo, string Loca) address);
        void MigrateOneFolder((string Repo, string Loca) adrTuple);
        void MigrateOneRepo(string repoName);
        void MigrateAllRepos();
    }
}
