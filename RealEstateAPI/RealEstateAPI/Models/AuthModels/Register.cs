using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models.AuthModels
{
    public class Register
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Email { get; set; } = "dummy@gmail.com";

        public string Role { get; set; } = "user";
        public bool IsRegister = false;
    }
}
