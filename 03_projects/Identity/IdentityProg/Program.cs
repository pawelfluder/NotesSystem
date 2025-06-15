using SharpIdentityProg.Data;

namespace SharpIdentityProg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var db = new ApplicationDbContext();
            var cs = db.ConnectionString;
            var users = db.Users.ToList();

            //var pendingMigrations = db.Database.GetPendingMigrations().ToList();
            //db.Database.EnsureCreated();
            //if (pendingMigrations.Any())
            //{
            //    db.Database.Migrate();
            //}
        }
    }
}
