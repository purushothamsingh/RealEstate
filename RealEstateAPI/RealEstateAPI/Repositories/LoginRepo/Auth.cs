﻿using MailKit.Security;
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
    public class Auth : IAuthRepo<Auth>
    {
        public static int userId = 0;
        private static Response response = new Response();

        private readonly ApplicationDbContext db;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(Auth));
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
            _log4net.Info("Decrypt Password method invoked successfully");
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
                _log4net.Info("Password hashed successfully");

                return generatedHash.SequenceEqual(hassedPassword);
            }



        }

        public async Task<Response> RegisterUserAsync(DomainRegister request)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Register User Repository method invoked");

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

                    _log4net.Error("406 - Not Acceptable: User already exist");
                    return CreateResponse("User Already exits", StatusCodes.Status406NotAcceptable, "", "Duplicate user found");
                }

                 db.Db_Registers.Add(registers);
                 db.SaveChanges();
                _log4net.Info("User Added Successfully");
                return CreateResponse("User found", StatusCodes.Status201Created, registers, "");

            }
        }

        public async Task<Response> ValidateUserAsync(Login req)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Validate User Repository method invoked");

            var user =db.Db_Registers.AnyAsync(x => x.UserName.ToLower() == req.UserName.ToLower());
     
            if (user.Result == false)
            {
                _log4net.Error("404 - Not Found: Invalid User");
               return CreateResponse("", StatusCodes.Status404NotFound, "", "Invalid User");
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
                    return CreateResponse("User Found", StatusCodes.Status302Found, jwt, ""); 
                
                }
                
                else {
                    _log4net.Error("404 - Not Found: User Not Found"); 
                    return CreateResponse("", StatusCodes.Status404NotFound, "", "User not found"); }
            }
            else { return new Response(); }
           
        }

        public async Task<Response> ForgotPasswordAsync(string email,int myotp, string password, string confirmpassword)
        {
            _log4net.Info("Forgot Password Repository method invoked");
            
            var mail = db.Db_Registers.AnyAsync(x => x.Email == email).Result;
            if(mail == false)
            {
                _log4net.Error("404 - Not Found: Invalid Email entered");
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

                            _log4net.Info("Password Changed");
                            return CreateResponse("Password changed sucessfully", StatusCodes.Status201Created, currentObject, "");

                        }
                    }
                    else
                    {
                        _log4net.Error("406 - Not Acceptable: Password and Confirm Password not matched");
                        return CreateResponse("", StatusCodes.Status406NotAcceptable, "", "Password And ConfirmPassword Not Matched");
                    }
                }
                else
                {
                    _log4net.Error("403 - Forbidden: Invalid OTP");
                    return CreateResponse("", StatusCodes.Status403Forbidden, "", "Invalid Otp");
                }


               
              
            }
           
        }


        public async Task<Response> GenerateOtpAsync(string myemail)
        {
            _log4net.Info("Generate OTP Repository method invoked");
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
                return CreateResponse("Otp Sent Sucessfully", StatusCodes.Status200OK, value, "");
            }

            _log4net.Error("404 - Not Found; Invalid Email");
            return CreateResponse("Invalid Email", StatusCodes.Status404NotFound, "", "Email Not found");
        }
    }
}
