using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;
using RealEstateAPI.Repositories;
using System.Security.Cryptography;

namespace RealEstateAPI.Controllers.LoginModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepo authRepo;
     //   private readonly IMapper mapper;

        public AuthController(IAuthRepo _authRepo)
        {

            authRepo = _authRepo;
            // mapper = _mapper;
        }
        [HttpPost]
     public async Task<IActionResult>AddUser(DomainRegister req)
        {
            var user = await authRepo.RegisterUser(req);

            return Ok(user);

              
        }

      

        
    }
}

