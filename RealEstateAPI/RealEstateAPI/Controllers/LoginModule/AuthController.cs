using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public AuthController(IAuthRepo _authRepo)
        {
            authRepo = _authRepo;
        }
        [HttpPost]
     public async Task<IActionResult>AddUser(DomainRegister req)
        {

            if (ModelState.IsValid)
            {
                var user = await authRepo.RegisterUser(req);

                return Ok(user);
            }

            return BadRequest();
            

              
        }

      

        
    }
}

