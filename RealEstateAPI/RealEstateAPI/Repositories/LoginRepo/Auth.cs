using MailKit.Security;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MimeKit.Text;
using MimeKit;
using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;
using System.Security.Cryptography;
using MailKit.Net.Smtp;
using RealEstateAPI.Controllers.LoginModule;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstateAPI.Repositories.LoginRepo
{
    public class AuthRepo : IAuthRepo
    {
        private static Response response = new Response();

        private readonly ApplicationDbContext db;
        public AuthRepo(ApplicationDbContext _db)
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
                registers.Mobile = request.Mobile;

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
                var id = 0;
                foreach(var i in obj)
                {
                    id = i.ID;
                }

                bool isvalid = DecriptPassword(obj,req.Password);

                if (isvalid) {

                    List<Claim> claims = new List<Claim>
            {
                //new Claim (ClaimTypes.Name,req.UserName),
                new Claim("Name",req.UserName),
                new Claim("Id",id.ToString())
               // new Claim (ClaimTypes.Email,req.Email)
               
            };
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("mytoken idkaldkhodsildbjafso"));
                    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: cred);
                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                    return CreateResponse("User Found", StatusCodes.Status302Found, jwt, ""); 
                
                }

                else { return CreateResponse("", StatusCodes.Status404NotFound, "", "User not found"); }
            }
            else { return new Response(); }
           
        }

        public async Task<Response> ForgotPasswordAsync(string email,int myotp, string password, string confirmpassword)
        {
            var mail = db.Db_Registers.AnyAsync(x => x.Email == email).Result;
            if(mail == false)
            {
                return CreateResponse("", StatusCodes.Status404NotFound, "", "Invalid Email");
            }
            else
            {
                
                if(AuthController.SaveOtp == myotp)
                {
                    AuthController.SaveOtp = new Random().Next(100000, 2302030);
                    if (password == confirmpassword)
                    {
                        var currentObject = db.Db_Registers.FirstOrDefaultAsync(x => x.Email == email).Result;
                        using (var hmac = new HMACSHA512())
                        {
                            var salt = hmac.Key;
                            var hashedPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                            currentObject.PaswordSalt = salt;
                            currentObject.PasswordHash = hashedPassword;
                            db.Db_Registers.Update(currentObject);
                            db.SaveChangesAsync();

                            return CreateResponse("Password changed sucessfully", StatusCodes.Status201Created, currentObject, "");

                        }
                    }
                    else
                    {
                        return CreateResponse("", StatusCodes.Status406NotAcceptable, "", "Password And ConfirmPassword Not Matched");
                    }
                }
                else
                {
                    return CreateResponse("", StatusCodes.Status403Forbidden, "", "Invalid Otp");
                }


               
              
            }
           
        }


        public async Task<Response> GenerateOtpAsync(string myemail)
        {
            if(db.Db_Registers.AnyAsync(x=>x.Email == myemail).Result)
            {
                Random randomNumber = new Random();
                int value = randomNumber.Next(100000, 999999);

                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("kpurushothamsingh@gmail.com"));
                email.To.Add(MailboxAddress.Parse(myemail));
                email.Subject = "Test Email Subject";
                email.Body = new TextPart(TextFormat.Html) { Text = "<h3>Your otp is <h3>" + value };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                smtp.Authenticate("kpurushothamsingh@gmail.com", "evqypbiaclpnrrlb");
                smtp.Send(email);
                smtp.Disconnect(true);

                return CreateResponse("Otp Sent Sucessfully", StatusCodes.Status200OK, value, "");
            }

            return CreateResponse("Invalid Email", StatusCodes.Status404NotFound, "", "Email Not found");
        }
    }
}
