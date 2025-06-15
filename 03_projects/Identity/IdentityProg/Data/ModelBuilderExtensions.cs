using Microsoft.EntityFrameworkCore;
using SharpIdentityProg.Data;

namespace SharpCountriesProg.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //var seeder = new Seeder();
            // var toSeed = seeder.GetDefault();
            // modelBuilder.Entity<ApplicationUser>()
            //     .HasData(toSeed);
        }
    }
}
