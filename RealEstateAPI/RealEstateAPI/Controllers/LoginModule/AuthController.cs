using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public static int SaveOtp;
        public AuthController(IAuthRepo _authRepo, IMapper mapper)
        {
            authRepo = _authRepo;
            this.mapper = mapper;
        }
        [HttpPost("Register")]
     public async Task<IActionResult>AddUser(DomainRegister req)
        {
                var user = await authRepo.RegisterUserAsync(req);
                return Ok(user);    
        }

        [HttpPost("Login")]
     public async Task<IActionResult>ValidateCredentials(Login req)
        {
            var request =  await authRepo.ValidateUserAsync(req);

            return Ok(request);
        }

        [HttpPut]
        [Route("ForgotPassword/{otp:int}")]
        public async Task<IActionResult> ForgotPassword(string email,int otp, string password, string confirmpass )
        {
            var request = await authRepo.ForgotPasswordAsync(email, otp, password, confirmpass);

            return Ok(request);
        }
        [HttpGet("GenerateOtp")]

        public async Task<IActionResult> GenerateOtp(string email)
        {
            var request = await authRepo.GenerateOtpAsync(email);
            if(request.Code == 200)
            {
                SaveOtp = request.Data;
                return Ok(request.Message);
            }
            return Ok(request);

        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id) { 

            var user = await authRepo.GetUserByIdAsync(id);
            var userDTO = mapper.Map<UserDto>(user.Data);
            user.Data = userDTO;
            return Ok(user);

        }



    }
}

