using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.Helper;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.PropertyRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateTests
{
    public class PropertyTypeTest:IDisposable
    {
        private static Response response = new Response();

        private List<PropertyType> PropertyTypeList;
        protected readonly ApplicationDbContext _context;

        public PropertyTypeTest()
        {
            PropertyTypeList = new List<PropertyType>()
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
        public async Task GetPropertyTypes_ShouldReturn201Status()
        {
            var PropertyService = new Mock<IPropertyTypeRepo>();
            PropertyService.Setup(x => x.GetPropertyTypesAsync()).ReturnsAsync(new Response("Property Types Found", StatusCodes.Status302Found, PropertyTypeList, ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var propcon = new PropertyTypeController(PropertyService.Object, mapper);
            var result = await propcon.GetPropertyTypes();
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status302Found, response.Code);
        }

        [Fact]
        public async Task GetPropertyTypes_ShouldReturn404Status()
        {
            var PropertyService = new Mock<IPropertyTypeRepo>();
            PropertyService.Setup(x => x.GetPropertyTypesAsync()).ReturnsAsync(new Response("", StatusCodes.Status404NotFound, "", "Property Types not Found") );
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var propcon = new PropertyTypeController(PropertyService.Object, mapper);
            var result = await propcon.GetPropertyTypes();
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status404NotFound, response.Code);
        }



        [Fact]
        public async Task GetAllAsync_ReturnExpectedValues()
        {
            /// Arrange
            _context.PropertyTypes.AddRange(PropertyTypeList);
            _context.SaveChanges();

            var sut = new PropertyTypeRepo(_context);

            /// Act
            var result = await sut.GetPropertyTypesAsync();
            var res = result.Data;
            /// Assert 

            Assert.Equal(PropertyTypeList[0].Id, res[0].Id);
            Assert.Equal(PropertyTypeList[0].Name, res[0].Name);
            Assert.Equal(PropertyTypeList[0].LastUpdatedOn, res[0].LastUpdatedOn);
            Assert.Equal(PropertyTypeList[0].LastUpdatedBy, res[0].LastUpdatedBy);
            Assert.Equal(PropertyTypeList[1].Id, res[1].Id);
            Assert.Equal(PropertyTypeList[1].Name, res[1].Name);
            Assert.Equal(PropertyTypeList[1].LastUpdatedOn, res[1].LastUpdatedOn);
            Assert.Equal(PropertyTypeList[1].LastUpdatedBy, res[1].LastUpdatedBy);

        }




        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


    }
}