using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Moq;
using RealEstateAPI.Controllers.LoginModule;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.LoginRepo;
using RealEstateAPI.Repositories.PropertyRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateTests
{
    public class AuthTest
    {
        private static Response response = new Response();
        protected readonly ApplicationDbContext _context;
        private DomainRegister User;

        public AuthTest() {
            User = new DomainRegister()
            {
                UserName="santo",
                Password="12345678",
                ConfirmPassword="12345678",
                Email="santo123@gmail.com",
                Role="user",
                IsRegister=true,
                Mobile=9361438996
            };
        }

        [Fact]
        public async Task AddUser_ShouldReturn201Status()
        {
            var AuthService = new Mock<IAuthRepo>();
            AuthService.Setup(x =>x.RegisterUserAsync(User)).ReturnsAsync(new Response("User added", StatusCodes.Status201Created, User, ""));
            var authcon = new AuthController(AuthService.Object);
            var result = await authcon.AddUser(User);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status201Created, response.Code);

        }


        [Fact]
        public async Task AddUser_ShouldReturn406Status()
        {
            var AuthService = new Mock<IAuthRepo>();
            AuthService.Setup(x => x.RegisterUserAsync(User)).ReturnsAsync(new Response("User Already exits", StatusCodes.Status406NotAcceptable, "", "Duplicate user found"));
            var authcon = new AuthController(AuthService.Object);
            var result = await authcon.AddUser(User);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status406NotAcceptable, response.Code);

        }


    }
}
