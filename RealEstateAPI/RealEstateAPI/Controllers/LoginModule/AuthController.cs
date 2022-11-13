using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.DomainModels;
using RealEstateAPI.DomainModels.PropertyDtos;
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
        private readonly IMapper mapper;

        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthController));


        public static int SaveOtp;
        public AuthController(IAuthRepo _authRepo, IMapper mapper)
        {
            authRepo = _authRepo;
            this.mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> AddUser(DomainRegister req)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("AddUser method invoked");

            var user = await authRepo.RegisterUserAsync(req);
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> ValidateCredentials(Login req)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Login method invoked");

            var request = await authRepo.ValidateUserAsync(req);
            return Ok(request);
        }

        [HttpPut]
        [Route("ForgotPassword/{otp:int}")]
        public async Task<IActionResult> ForgotPassword(string email, int otp, string password, string confirmpass)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("ForgotPassword method invoked");

            var request = await authRepo.ForgotPasswordAsync(email, otp, password, confirmpass);

            return Ok(request);
        }
        [HttpGet("GenerateOtp/{email}")]

        public async Task<IActionResult> GenerateOtp(string email)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("GenerateOTP method invoked");

            var request = await authRepo.GenerateOtpAsync(email);
            if (request.Code == 200)
            {
                SaveOtp = request.Data;
                return Ok(request);
            }
            return Ok(request);

        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id) 
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("GetUserById method invoked");
            var user = await authRepo.GetUserByIdAsync(id);
            var userDTO = mapper.Map<UserDto>(user.Data);
            user.Data = userDTO;
            return Ok(user);

        }

        [HttpPost]
        [Route("{requestEmail}/{subject}")]
        public async Task<IActionResult> SendComplaintMail(string requestEmail, string subject)
        {
            var complaint = await authRepo.EmailVerification(requestEmail, subject);
            if(complaint.Code == 200)
            {
                return Ok(complaint);
            }
            else
            {
                complaint.Message = "";
                complaint.Error = "Internal Erro";
                return Ok(complaint);
            }
        }
    }
}

