namespace SharpNotesMigrationProg.AAPublic
{
    public interface IMigrator03
    {
        List<(int, string, string, string)> Changes { get; }
        void MigrateOneAddress((string Repo, string Loca) address);
        void MigrateOneRepo((string Repo, string Loca) address);
        void SetAgree(bool agree);
    };
}