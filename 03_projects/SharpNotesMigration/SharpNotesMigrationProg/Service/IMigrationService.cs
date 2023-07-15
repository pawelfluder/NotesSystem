namespace SharpNotesMigrationProg.Service
{
    public partial interface IMigrationService
    {
        interface IMigrator01 { };
        interface IMigrator02 { };
        interface IMigrator03
        {
            List<(string, string, string)> BeforeAfter { get; }
            void MigrateOneAddress((string Repo, string Loca) address);
            void MigrateOneRepo(string repoPath);
        };
    }
}