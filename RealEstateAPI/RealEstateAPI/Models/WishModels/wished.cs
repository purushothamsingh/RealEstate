using RealEstateAPI.Models.AuthModels;
using RealEstateAPI.Models.Property;
using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models.WishModule
{
    public class wished
    {
        [Key]
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int SellRent { get; set; }
        public string Name { get; set; }
        public string PropertyType { get; set; }
        public string FurnishingType { get; set; }
        public int Price { get; set; }
        public int BHK { get; set; }
        public int BuiltArea { get; set; }
        public string City { get; set; }
        public bool ReadyToMove { get; set; }
        public string photo { get; set; } = null;
        public int db_RegisterId { get; set; }
        public Db_Register db_Register { get; set; }
    }
}