using RealEstateAPI.Models.Property;
using RealEstateAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstateAPI.Models.WishModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.Repositories.PropertyRepo;
using RealEstateAPI.Repositories.WishRepo;
using RealEstateAPI.DomainModels.WishDto;
using RealEstateAPI.Controllers.WishModule;
using AutoMapper;
using RealEstateAPI.Helper;

namespace RealEstateTests
{
    public class WishTest:IDisposable
    {
        private static Response response = new Response();
        protected readonly ApplicationDbContext _context;
        private List<WishedDto> WishList;
        private List<wished> WishListDb;

        public WishTest()
        {
            WishList = new List<WishedDto>()
            {
                new()
                {
                    Id=1,
                    PropertyId=1,
                    SellRent=1,
                    Name ="Santo Villa",
                    PropertyType="Apartment",
                    FurnishingType ="Semi Furnished",
                    Price =1000,
                    BHK =3,
                    BuiltArea=1000,
                    City ="New York",
                    ReadyToMove =true,
                    photo ="xxxxx",
                    db_RegisterId=1

    },new()
                {
                    Id=2,
                    PropertyId=2,
                    SellRent=1,
                    Name ="Daksha Villa",
                    PropertyType="Apartment",
                    FurnishingType ="Semi Furnished",
                    Price =1000,
                    BHK =3,
                    BuiltArea=1000,
                    City ="New York",
                    ReadyToMove =true,
                    photo ="xxxxx",
                    db_RegisterId=1

    }
            };
                WishListDb = new List<wished>()
            {
                new()
                {
                    Id=1,
                    PropertyId=1,
                    SellRent=1,
                    Name ="Santo Villa",
                    PropertyType="Apartment",
                    FurnishingType ="Semi Furnished",
                    Price =1000,
                    BHK =3,
                    BuiltArea=1000,
                    City ="New York",
                    ReadyToMove =true,
                    photo ="xxxxx",
                    db_RegisterId=1

    },new()
                {
                    Id=2,
                    PropertyId=2,
                    SellRent=1,
                    Name ="Daksha Villa",
                    PropertyType="Apartment",
                    FurnishingType ="Semi Furnished",
                    Price =1000,
                    BHK =3,
                    BuiltArea=1000,
                    City ="New York",
                    ReadyToMove =true,
                    photo ="xxxxx",
                    db_RegisterId=1

    },


                };

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

            _context = new ApplicationDbContext(options);

            _context.Database.EnsureCreated();
        }


        [Fact]
        public async Task AddWish_ShouldReturn201Status()
        {
            var WishService = new Mock<IWishRepo>();
            WishService.Setup(x => x.AddedWishAsync(WishList[0])).ReturnsAsync(new Response("added", StatusCodes.Status201Created, WishList[0], ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var wishcon = new WishController(WishService.Object,mapper);
            var result = await wishcon.WishedData(WishList[0]);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status201Created, response.Code);

        }


        [Fact]
        public async Task GetAllWishById_ReturnExpectedValues()
        {
            /// Arrange
            _context.Wishes.AddRange(WishListDb);
            _context.SaveChanges();

            var sut = new Wish(_context);

            /// Act
            var result = await sut.GetwishedList(1);
            var res = result.Data;
            /// Assert 

            Assert.Equal(WishListDb[0].Id, res[0].Id);
            Assert.Equal(WishListDb[0].Name, res[0].Name);
            Assert.Equal(WishListDb[0].PropertyId, res[0].PropertyId);
            Assert.Equal(WishListDb[0].SellRent, res[0].SellRent);
            Assert.Equal(WishListDb[0].PropertyType, res[0].PropertyType);
            Assert.Equal(WishListDb[0].FurnishingType, res[0].FurnishingType);
            Assert.Equal(WishListDb[0].Price, res[0].Price);
            Assert.Equal(WishListDb[0].BHK, res[0].BHK);
            Assert.Equal(WishListDb[0].BuiltArea, res[0].BuiltArea);
            Assert.Equal(WishListDb[0].City, res[0].City);
            Assert.Equal(WishListDb[0].db_Register, res[0].db_Register);
            Assert.Equal(WishListDb[1].Id, res[1].Id);
            Assert.Equal(WishListDb[1].Name, res[1].Name);
            Assert.Equal(WishListDb[1].PropertyId, res[1].PropertyId);
            Assert.Equal(WishListDb[1].SellRent, res[1].SellRent);
            Assert.Equal(WishListDb[1].PropertyType, res[1].PropertyType);
            Assert.Equal(WishListDb[1].FurnishingType, res[1].FurnishingType);
            Assert.Equal(WishListDb[1].Price, res[1].Price);
            Assert.Equal(WishListDb[1].BHK, res[1].BHK);
            Assert.Equal(WishListDb[1].BuiltArea, res[1].BuiltArea);
            Assert.Equal(WishListDb[1].City, res[1].City);
            Assert.Equal(WishListDb[1].db_Register, res[1].db_Register);
        }


        [Fact]
        public async Task GetAllWish_ShouldReturn302Status()
        {
            var WishService = new Mock<IWishRepo>();
            WishService.Setup(x => x.GetwishedList(1)).ReturnsAsync(new Response("found", StatusCodes.Status302Found, WishList, ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var wishcon = new WishController(WishService.Object, mapper);
            var result = await wishcon.WishedData(1);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status302Found, response.Code);
        }


        [Fact]
        public async Task GetAllWish_ShouldReturnCorrectNumberOfValues()
        {
            var WishService = new Mock<IWishRepo>();
            WishService.Setup(x => x.GetwishedList(1)).ReturnsAsync(new Response("found", StatusCodes.Status302Found, WishList, ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);

            var wishcon = new WishController(WishService.Object, mapper);
            var result = await wishcon.WishedData(1);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(2, response.Data.Count);
        }







        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
