using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models.AuthModels
{
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public String Password { get; set; }
    }
}
