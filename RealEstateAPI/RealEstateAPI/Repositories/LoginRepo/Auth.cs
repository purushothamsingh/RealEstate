using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;
using System.Security.Cryptography;

namespace RealEstateAPI.Repositories.LoginRepo
{
    public class Auth : IAuthRepo
    {
        private static Response response = new Response();

        private readonly ApplicationDbContext db;
        public Auth(ApplicationDbContext _db)
        {
            db = _db;
           
        }

        public Response CreateResponse(string message, int code, dynamic data, string error)
        {
            response.Message = message;
            response.Code = code;
            response.Data = data;
            response.Error = error;

            return response;
        }

        private bool DecriptPassword(IQueryable<Db_Register> obj, string password)
        {
            byte[] salt = new byte[32];
            byte[] hassedPassword = new byte[32];
            foreach (var i in obj)
            {
                salt = i.PaswordSalt;
                hassedPassword = i.PasswordHash;
            }

            using (var hmac = new HMACSHA512(salt))
            {
                var generatedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return generatedHash.SequenceEqual(hassedPassword);
            }



        }

        public async Task<Response> RegisterUserAsync(DomainRegister request)
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

                if ( db.Db_Registers.FirstOrDefaultAsync(x => x.UserName.ToLower() == registers.UserName.ToLower()).Result != null){

                    return CreateResponse("User Already exits", StatusCodes.Status406NotAcceptable, "", "Duplicate user found");
                }

                 db.Db_Registers.Add(registers);
                 db.SaveChanges();
                return CreateResponse("User found", StatusCodes.Status201Created, registers, "");

            }
        }

        public async Task<Response> ValidateUserAsync(Login req)
        {
            var user = db.Db_Registers.AnyAsync(x => x.UserName.ToLower() == req.UserName.ToLower());
     
            if (user.Result == false)
            {
               return CreateResponse("", StatusCodes.Status404NotFound, "", "Invalid User");
            }
            else if(user.Result)
            { 
                var obj = db.Db_Registers.Where(x => x.UserName.ToLower() == req.UserName.ToLower()).Select(x => x);

                bool isvalid = DecriptPassword(obj,req.Password);

                if (isvalid) { return CreateResponse("User Found", StatusCodes.Status302Found, req, ""); }

                else { return CreateResponse("", StatusCodes.Status404NotFound, "", "User not found"); }
            }
            else { return new Response(); }
           
        }

      
    }
}
