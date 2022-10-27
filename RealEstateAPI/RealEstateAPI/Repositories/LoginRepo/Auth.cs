using Microsoft.AspNetCore.Mvc.ModelBinding;
using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;
using System.Security.Cryptography;

namespace RealEstateAPI.Repositories.LoginRepo
{
    public class Auth : IAuthRepo
    {

        private readonly ApplicationDbContext db;
        //   private readonly IMapper mapper;
        private static Response response = new Response();
        public Auth(ApplicationDbContext _db)
        {
            db = _db;
            // mapper = _mapper;
        }

        public async Task<Response> RegisterUser(DomainRegister request)
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
                registers.IsRegister = true;
                db.Db_Registers.Add(registers);
                db.SaveChanges();

                response.Message = "User Added successfully";
                response.Error = null;
                response.Code = StatusCodes.Status201Created;
                response.data = registers;


                return response;

            }
        }
    }
}
