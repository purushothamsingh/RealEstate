using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.DomainModels
{
    public class DomainRegister
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public string Email { get; set; } = "dummy@gmail.com";
        //[Compare("Password")]
        //public byte[] ConfirmPassword {get; set; }
        // public string Email { get; set; }

        public string Role { get; set; } = "user";
        public bool IsRegister = false;
    }
}
