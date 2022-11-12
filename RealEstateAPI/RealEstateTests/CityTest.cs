using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Org.BouncyCastle.Asn1.Pkcs;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.PropertyRepo;
using Xunit;

namespace RealEstateTests
{
    public class CityTest:IDisposable
    {
        private static Response response = new Response();
        protected readonly ApplicationDbContext _context;
        private List<City> CityList;
        public Response CreateResponse(string message, int code, dynamic data, string error)
        {
            response.Message = message;
            response.Code = code;
            response.Data = data;
            response.Error = error;

            return response;
        }

        public CityTest()
        {
            CityList = new List<City>()
            {
                new()
                {Id=1
                ,Name="New York"

                ,Country="USA"

                ,LastUpdatedOn=DateTime.Now

                 ,LastUpdatedBy=1
    },
                new()
                {Id=2
                ,Name="Los Angels"

                ,Country="USA"

                ,LastUpdatedOn=DateTime.Now

                 ,LastUpdatedBy=1
    }
            };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureCreated();
        }

        


        [Fact]
        public async Task GetAllCities_ShouldReturn200Status()
        {
            var CityService = new Mock<ICityRepo>();
                        CityService.Setup(x => x.GetCitiesAsync())
                .ReturnsAsync(CreateResponse("cities found", 
                StatusCodes.Status302Found,CityList, ""));
            var citycon = new CityController(CityService.Object);
            var res =await citycon.GetCities();
            res.GetType().Should().Be(typeof(OkObjectResult));
        }


        [Fact]
        public async Task GetAllAsync_ReturnExpectedValues()
        {
            /// Arrange
            _context.Cities.AddRange(CityList);
            _context.SaveChanges();

            var sut = new CityRepo(_context);

            /// Act
            var result = await sut.GetCitiesAsync();
            var res = result.Data;
            /// Assert 

            Assert.Equal(CityList[0].Id, res[0].Id);
            Assert.Equal(CityList[0].Name, res[0].Name);
            Assert.Equal(CityList[0].Country, res[0].Country);
            Assert.Equal(CityList[0].LastUpdatedOn, res[0].LastUpdatedOn);
            Assert.Equal(CityList[0].LastUpdatedBy, res[0].LastUpdatedBy);
            Assert.Equal(CityList[1].Id, res[1].Id);
            Assert.Equal(CityList[1].Name, res[1].Name);
            Assert.Equal(CityList[1].Country, res[1].Country);
            Assert.Equal(CityList[1].LastUpdatedOn, res[1].LastUpdatedOn);
            Assert.Equal(CityList[1].LastUpdatedBy, res[1].LastUpdatedBy);

        }



        [Fact]
        public async Task AddCity_ReturnExpectedValues()
        {
            var CityService = new Mock<ICityRepo>();
            CityService.Setup(x => x.AddCityAsync(CityList[0])).ReturnsAsync(CreateResponse("Added Successfully", StatusCodes.Status201Created, CityList[0], ""));
            var citycon = new CityController(CityService.Object);
            var result = await citycon.AddCity(CityList[0]);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(response.Code, StatusCodes.Status201Created);

        }





        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}