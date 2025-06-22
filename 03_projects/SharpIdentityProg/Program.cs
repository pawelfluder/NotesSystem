using SharpIdentityProg.Data;

namespace SharpIdentityProg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ApplicationDbContext db = new ();
            string cs = db.ConnectionString;
            List<ApplicationUser> users = db.Users.ToList();

            //var pendingMigrations = db.Database.GetPendingMigrations().ToList();
            //db.Database.EnsureCreated();
            //if (pendingMigrations.Any())
            //{
            //    db.Database.Migrate();
            //}
        }
    }
}
