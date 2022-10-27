using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.DomainModels
{
    public class DomainRegister
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
       
        [Required(ErrorMessage ="Email Required")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage ="Invalid Email Format") ]
        public string Email { get; set; } 
        public string Role { get; set; } = "user";
        public bool IsRegister = false;
    }
}
