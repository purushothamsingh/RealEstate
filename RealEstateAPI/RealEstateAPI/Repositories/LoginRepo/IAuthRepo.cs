﻿using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;

namespace RealEstateAPI.Repositories.LoginRepo
{
    public interface IAuthRepo<Auth>

    {

        Task<Response> RegisterUserAsync(DomainRegister req);
        Task<Response> ValidateUserAsync(Login req);
        
        Task<Response> ForgotPasswordAsync(string email,int otp,string password,string confirmpassword);

        Task<Response> GenerateOtpAsync(string email);
    }
} 
