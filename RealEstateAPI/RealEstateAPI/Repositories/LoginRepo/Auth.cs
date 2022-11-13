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
using Org.BouncyCastle.Bcpg;

namespace RealEstateAPI.Repositories.LoginRepo
{
    public class Auth : IAuthRepo
    {
        public static int userId = 0;
        private static Response response = new Response();

        private readonly ApplicationDbContext db;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(Auth));
        public Auth(ApplicationDbContext _db)
        {
            db = _db;
           
        }

        private bool DecriptPassword(IQueryable<Db_Register> obj, string password)
        {
            _log4net.Info("DecryptPassword method invoked successfully");
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
                _log4net.Info("Passwordhashed successfully");

                return generatedHash.SequenceEqual(hassedPassword);
            }



        }

        public async Task<Response> RegisterUserAsync(DomainRegister request)
        {            
            _log4net.Info("RegisterUserAsync Repository method invoked");

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

                if ( db.Db_Registers.FirstOrDefaultAsync(x => x.UserName.ToLower() == registers.UserName.ToLower()).Result != null)
                {
                    _log4net.Error("406 - Not Acceptable: User already exist");
                    return new Response("User Already exits", StatusCodes.Status406NotAcceptable, "", "Duplicate user found");

                }

                 db.Db_Registers.Add(registers);
                 db.SaveChanges();
                _log4net.Info("User Added Successfully");
                return new Response("User added", StatusCodes.Status201Created, registers, "");

            }
        }

        public async Task<Response> ValidateUserAsync(Login req)
        {            
            _log4net.Info("ValidateUser Repository method invoked");

            var user =db.Db_Registers.AnyAsync(x => x.UserName.ToLower() == req.UserName.ToLower());
     
            if (user.Result == false)
            {
                _log4net.Error("404 - Not Found: Invalid User");
               return new Response("", StatusCodes.Status404NotFound, "", "Invalid User");
            }
            else if(user.Result)
            { 
                var obj =db.Db_Registers.Where(x => x.UserName.ToLower() == req.UserName.ToLower()).Select(x => x);
                var id = 0;
                foreach(var i in obj)
                {
                    id = i.ID;
                   
                }
                Auth.userId = id;

                bool isvalid = DecriptPassword(obj,req.Password);

                if (isvalid) {

                    List<Claim> claims = new List<Claim>
            {
                //new Claim (ClaimTypes.Name,req.UserName),
                new Claim("Name",req.UserName),
                new Claim("Id",id.ToString()),
               
               // new Claim (ClaimTypes.Email,req.Email)
               
            };
                    
                    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("mytoken idkaldkhodsildbjafso"));
                    var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(10),
                        signingCredentials: cred);
                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                    _log4net.Info("User Validated and token created successfully");
                    return new Response("User Found", StatusCodes.Status302Found, jwt, ""); 
                
                }
                
                else {
                    _log4net.Error("404 - Not Found: User Not Found"); 
                    return new Response("", StatusCodes.Status404NotFound, "", "User not found"); }
            }
            else
            {
                return new Response(); 
            }
           
        }

        public async Task<Response> ForgotPasswordAsync(string email,int myotp, string password, string confirmpassword)
        {
            _log4net.Info("ForgotPasswordAsync Repository method invoked");
            
            var mail = db.Db_Registers.AnyAsync(x => x.Email == email).Result;
            if(mail == false)
            {
                _log4net.Error("404 - Not Found: Invalid Email entered");
                return new Response("", StatusCodes.Status404NotFound, "", "Invalid Email");
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

                            _log4net.Info("Password Changed");
                            return new Response("Password changed sucessfully", StatusCodes.Status201Created, currentObject, "");

                        }
                    }
                    else
                    {
                        _log4net.Error("406 - Not Acceptable: Password and Confirm Password not matched");
                        return new Response("", StatusCodes.Status406NotAcceptable, "", "Password And ConfirmPassword Not Matched");
                    }
                }
                else
                {
                    _log4net.Error("403 - Forbidden: Invalid OTP");
                    return new Response("", StatusCodes.Status403Forbidden, "", "Invalid Otp");
                }               
              
            }
           
        }


        public async Task<Response> GenerateOtpAsync(string myemail)
        {
            _log4net.Info("GenerateOtpAsync Repository method invoked");
            if (db.Db_Registers.AnyAsync(x=>x.Email == myemail).Result)
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

                _log4net.Info("OTP sent successfully");
                return new Response("Otp Sent Sucessfully", StatusCodes.Status200OK, value, "");
            }

            _log4net.Error("404 - Not Found; Invalid Email");
            return new Response("Invalid Email", StatusCodes.Status404NotFound, "", "Email Not found");
        }

        public async Task<Response> GetUserByIdAsync(int id)
        {
            _log4net.Info("GetUserByIdAsync Repository method invoked");
            var user = await db.Db_Registers.FindAsync(id);
            if (user != null)
            {
                _log4net.Info("User found successfully");
                return  new Response("User Found", StatusCodes.Status302Found, user, "");
            }
            _log4net.Error("404 - Not Found: User not found");
            return new Response("", StatusCodes.Status404NotFound, "", "User not Found");

        }

        public async Task<Response> EmailVerification(string requestEmail, string subject)
        {
            Random randomNumber = new Random();
            int value = randomNumber.Next(100000, 999999);

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("kpurushothamsingh@gmail.com"));
            email.To.Add(MailboxAddress.Parse(requestEmail));
            email.To.Add(MailboxAddress.Parse("kpurushothamsingh@gmail.com"));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Html) { Text = subject };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            smtp.Authenticate("kpurushothamsingh@gmail.com", "evqypbiaclpnrrlb");
            smtp.Send(email);
            smtp.Disconnect(true);

            return new Response("Complaint Recieved", StatusCodes.Status200OK, subject, "");
        }
    }
}
