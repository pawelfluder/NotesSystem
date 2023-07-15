namespace SharpNotesMigrationProg.Service
{
    public partial interface IMigrationService
    {
        void MigrateAll();

        void Migrate(Type migratorType);
    }
}