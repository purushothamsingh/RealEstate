
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models.AuthModels;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Models.WishModule;

namespace RealEstateAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Register> register { get; set; }
        public DbSet<Login> login { get; set; }
        public DbSet<Db_Register> Db_Registers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Property.Property> Properties { get; set; }
        public DbSet<FurnishingType> FurnishingTypes { get; set; }
        public DbSet<PropertyType> PropertyTypes { get; set; }

        public DbSet<wished> Wishes { get; set; }

    }
}
