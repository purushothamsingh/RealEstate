
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models.AuthModels;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        public DbSet<Register> register { get; set; }
        public DbSet<Login> login { get; set; }

        public DbSet<Db_Register> Db_Registers { get; set; }
        public DbSet<Cities> Cities { get; set; }
    }
}
