using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharpCountriesProg.Models;
using SharpIdentityProg.AAPublic;
using SharpIdentityProg.Registrations;
using SharpFileServiceProg.AAPublic;
using SharpIdentityProg.AAPublic;
using SharpIdentityProg.Registrations;

namespace SharpIdentityProg.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        private string _connectionString = "Data Source=TemporaryName.db";

        public string ConnectionString => _connectionString;

        public ApplicationDbContext()
        {
            IIdentityDbConnectionString dbConnectionStr = MyBorder.OutContainer
                .Resolve<IIdentityDbConnectionString>();
            _connectionString = dbConnectionStr.GetConnStr();
        }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString.StartsWith("Server="))
            {
                optionsBuilder.UseSqlServer(_connectionString);
                return;
            }

            if (_connectionString.StartsWith("Data Source="))
            {
                optionsBuilder.UseSqlite(_connectionString);
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
