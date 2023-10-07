namespace SharpNotesMigrationProg.Service
{
    public partial interface IMigrationService
    {
        interface IMigrator01 { };
        interface IMigrator02 { };
        interface IMigrator03
        {
            List<(int, string, string, string)> Changes { get; }
            void MigrateOneAddress((string Repo, string Loca) address);
            void MigrateOneFolderRecourively((string Repo, string Loca) address);
            void MigrateOneRepo((string Repo, string Loca) address);
            void SetAgree(bool agree);
        };
    }
}