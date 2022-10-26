using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;
using System.Security.Cryptography;

namespace RealEstateAPI.Repositories
{
    public class Auth:IAuthRepo
    {

        private readonly ApplicationDbContext db;
        //   private readonly IMapper mapper;

        public Auth(ApplicationDbContext _db)
        {
            db = _db;
            // mapper = _mapper;
        }

        public async Task<Db_Register> RegisterUser(DomainRegister request)
        {

            using (var hmac = new HMACSHA512())
            {
                Db_Register registers = new Db_Register();

                registers.IsRegister = true;
                registers.UserName = request.UserName;
                registers.PaswordSalt = hmac.Key;
                registers.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(request.Password));
                registers.Role = request.Role;
                registers.Email = request.Email;
                db.Db_Registers.Add(registers);
                db.SaveChanges();
                return  registers;

            }
        }
    }
}
