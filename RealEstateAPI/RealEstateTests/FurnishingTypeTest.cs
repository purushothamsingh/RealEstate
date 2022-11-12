using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.Helper;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories.PropertyRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateTests
{
    public class FurnishingTypeTest
    {
        private static Response response = new Response();

        private List<FurnishingType> FurnishingTypeList;
        protected readonly ApplicationDbContext _context;

        public FurnishingTypeTest()
        {
            FurnishingTypeList = new List<FurnishingType>()
            {
                new()
                {
                    Id=1,
                    Name="House",
                    LastUpdatedBy=1,
                    LastUpdatedOn=DateTime.Now
                },
                new()
                {
                    Id=2,
                    Name="Apartment",
                    LastUpdatedBy=1,
                    LastUpdatedOn=DateTime.Now
                }

            };


            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureCreated();

        }



        [Fact]
        public async Task GetFurnishingTypes_ShouldReturn201Status()
        {
            var FurnishingService = new Mock<IFurnishingTypeRepo>();
            FurnishingService.Setup(x => x.GetFurnishingTypesAsync()).ReturnsAsync(new Response("Property Found", StatusCodes.Status302Found, FurnishingTypeList, ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var furcon = new FurnishingTypeController(FurnishingService.Object, mapper);
            var result = await furcon.GetFurnishingTypes();
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status302Found, response.Code);
        }

        [Fact]
        public async Task GetFurnishingTypes_ShouldReturn404Status()
        {
            var FurnishingService = new Mock<IFurnishingTypeRepo>();
            FurnishingService.Setup(x => x.GetFurnishingTypesAsync()).ReturnsAsync(new Response("", StatusCodes.Status404NotFound, "", "Property not Found"));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var furcon = new FurnishingTypeController(FurnishingService.Object, mapper);
            var result = await furcon.GetFurnishingTypes();
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status404NotFound, response.Code);
        }



        [Fact]
        public async Task GetAllAsync_ReturnExpectedValues()
        {
            /// Arrange
            _context.FurnishingTypes.AddRange(FurnishingTypeList);
            _context.SaveChanges();

            var sut = new FurnishingTypeRepo(_context);

            /// Act
            var result = await sut.GetFurnishingTypesAsync();
            var res = result.Data;
            /// Assert 

            Assert.Equal(FurnishingTypeList[0].Id, res[0].Id);
            Assert.Equal(FurnishingTypeList[0].Name, res[0].Name);
            Assert.Equal(FurnishingTypeList[0].LastUpdatedOn, res[0].LastUpdatedOn);
            Assert.Equal(FurnishingTypeList[0].LastUpdatedBy, res[0].LastUpdatedBy);
            Assert.Equal(FurnishingTypeList[1].Id, res[1].Id);
            Assert.Equal(FurnishingTypeList[1].Name, res[1].Name);
            Assert.Equal(FurnishingTypeList[1].LastUpdatedOn, res[1].LastUpdatedOn);
            Assert.Equal(FurnishingTypeList[1].LastUpdatedBy, res[1].LastUpdatedBy);

        }
    }
}
