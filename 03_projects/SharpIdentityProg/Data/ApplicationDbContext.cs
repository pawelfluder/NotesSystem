using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpCountriesProg.Models;
using SharpIdentityProg.Repet;

namespace SharpIdentityProg.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private string connectionString = "Data Source=TemporaryName.db";

        public string ConnectionString => connectionString;

        public ApplicationDbContext(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public ApplicationDbContext()
        {
            this.connectionString = new ConnectionStringHelper().Get(null);
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (connectionString.StartsWith("Server="))
            {
                optionsBuilder.UseSqlServer(connectionString);
                return;
            }

            if (connectionString.StartsWith("Data Source="))
            {
                optionsBuilder.UseSqlite(connectionString);
                return;
            }
        }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();
        }
    }
}
