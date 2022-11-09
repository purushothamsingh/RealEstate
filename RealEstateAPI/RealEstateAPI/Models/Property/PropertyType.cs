using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models.Property
{
    public class PropertyType
    {
        [Key]
        public int Id { get; set; }
     
        [Required]
        public string Name { get; set; }
        public DateTime LastUpdatedOn { get; set; } = DateTime.Now;
        public int LastUpdatedBy { get; set; } = 0;
    }
}