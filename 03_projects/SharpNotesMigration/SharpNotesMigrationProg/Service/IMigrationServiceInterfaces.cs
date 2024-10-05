namespace SharpNotesMigrationProg.Service;

public partial interface IMigrationService
{
    void MigrateOneAddress(Type migratorType, (string Repo, string Loca) address, bool agree);
    void MigrateOneFolder(Type migratorType, (string Repo, string Loca) address, bool agree);
    void MigrateOneRepo(Type migratorType, string repoName, bool agree);
    void MigrateAllRepos(Type migratorType);
}