using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.DomainModels.PropertyDtos
{
    public class CityDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "City Name required")]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression(".*[a-zA-Z]+.*", ErrorMessage = "Only Numerics not allowed")]
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
