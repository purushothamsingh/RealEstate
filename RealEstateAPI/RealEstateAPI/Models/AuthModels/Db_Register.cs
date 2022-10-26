using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models.AuthModels
{
    public class Db_Register
    {

        [Key]
        public int ID { get; set; }
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PaswordSalt { get; set; }
        public string Email { get; set; } = "dummy@gmail.com";
        //[Compare("Password")]
        //public byte[] ConfirmPassword {get; set; }
        // public string Email { get; set; }

        public string Role { get; set; } = "user";
        public bool IsRegister = false;
    }
}
