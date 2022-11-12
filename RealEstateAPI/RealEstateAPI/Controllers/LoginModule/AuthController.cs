using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;
using RealEstateAPI.Repositories.LoginRepo;
using System.Security.Cryptography;

namespace RealEstateAPI.Controllers.LoginModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IAuthRepo authRepo;      
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthController));


        public static int SaveOtp;
        public AuthController(IAuthRepo _authRepo)
        {
            authRepo = _authRepo;
        }
        [HttpPost("Register")]
     public async Task<IActionResult>AddUser(DomainRegister req)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Add User method invoked");

            var user = await authRepo.RegisterUserAsync(req);                  
            return Ok(user);    
        }

        [HttpPost("Login")]
     public async Task<IActionResult>ValidateCredentials(Login req)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Login method invoked");

            var request =  await authRepo.ValidateUserAsync(req);            
            return Ok(request);
        }

        [HttpPut]
        [Route("ForgotPassword/{otp:int}")]
        public async Task<IActionResult> ForgotPassword(string email,int otp, string password, string confirmpass )
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Forgot Password method invoked");

            var request = await authRepo.ForgotPasswordAsync(email, otp, password, confirmpass);

            return Ok(request);
        }
        [HttpGet("GenerateOtp/{email}")]

        public async Task<IActionResult> GenerateOtp(string email)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Generate OTP method invoked");

            var request = await authRepo.GenerateOtpAsync(email);
            if(request.Code == 200)
            {
                SaveOtp = request.Data;
                return Ok(request);
            }
            return Ok(request);

        }



    }
}

