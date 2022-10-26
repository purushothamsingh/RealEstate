using System.ComponentModel.DataAnnotations;

namespace RealEstateAPI.Models.AuthModels
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public String Password { get; set; }
    }
}
