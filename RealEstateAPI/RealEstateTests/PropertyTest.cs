using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Helper;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.LoginRepo;
using RealEstateAPI.Repositories.PropertyRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RealEstateTests
{
    public class PropertyTest
    {
        private static Response response = new Response();
      
        private List<Property> PropertyList;
        private List<PropertyDto> PropertyDtoList;
        public PropertyTest()
        {
           PropertyList = new List<Property>() {
        new(){
        Id = 1,
        SellRent=2,
        Name="Atul Villa",
        PropertyTypeId=1,
        FurnishingTypeId=1,
        Price=20000,
        BHK=3,
        BuiltArea=1000,
        CityId=2,
        ReadyToMove=true,
        CarpetArea=900,
        Address="NewYork",
        Address2="USA",
        FloorNo=3,
        TotalFloors=5,
        MainEntrance="East",
        Security=0,
        Gated=true,
        Maintenance=300,
        EstPossessionOn=DateTime.Today,
        Age=10,
        Description="Good",
        PostedBy=1,
        LastUpdatedBy=1 },
        new(){
        Id=2,
        SellRent=1,
        Name="Santo Villa",
        PropertyTypeId=1,
        FurnishingTypeId=1,
        Price=10000,
        BHK=3,
        BuiltArea=1000,
        CityId=2,
        ReadyToMove=true,
        CarpetArea=900,
        Address="NewYork",
        Address2="USA",
        FloorNo=3,
        TotalFloors=5,
        MainEntrance="East",
        Security=0,
        Gated=true,
        Maintenance=300,
        EstPossessionOn=DateTime.Today,
        Age=10,
        Description="Good",
        PostedBy=1,
        LastUpdatedBy=1 }
        };


            PropertyDtoList = new List<PropertyDto>() {
        new(){
        SellRent=1,
        Name="Atul Villa",
        PropertyTypeId=1,
        FurnishingTypeId=1,
        Price=20000,
        BHK=3,
        BuiltArea=1000,
        CityId=2,
        ReadyToMove=true,
        CarpetArea=900,
        Address="NewYork",
        Address2="USA",
        FloorNo=3,
        TotalFloors=5,
        MainEntrance="East",
        Security=0,
        Gated=true,
        Maintenance=300,
        EstPossessionOn=DateTime.Today,
        Age=10,
        Description="Good",
        PostedBy=1,
        LastUpdatedBy=1 } 
            };
       

      
        }


        [Fact]
        public async Task GetAllSellOrRentAsync_ReturnExpectedValues()
        {
            var PropertyService = new Mock<IPropertyRepo>();
            PropertyService.Setup(x => x.GetPropertiesByIdAsync(1)).ReturnsAsync(new Response("Property Found", StatusCodes.Status302Found, PropertyList, ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var propcon = new PropertyController(PropertyService.Object, mapper);
            var result = await propcon.GetPropertyList(1);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            List<PropertyListDto> propertyListDtos = new List<PropertyListDto>();
            propertyListDtos = response.Data;
            Assert.Equal(PropertyList[0].Id, propertyListDtos[0].Id);
            Assert.Equal(PropertyList[0].Name, propertyListDtos[0].Name);
            Assert.Equal(PropertyList[0].BHK, propertyListDtos[0].BHK);
            Assert.Equal(PropertyList[0].Price, propertyListDtos[0].Price);
            Assert.Equal(PropertyList[1].Id, propertyListDtos[1].Id);
            Assert.Equal(PropertyList[1].Name, propertyListDtos[1].Name);
            Assert.Equal(PropertyList[1].BHK, propertyListDtos[1].BHK);
            Assert.Equal(PropertyList[1].Price, propertyListDtos[1].Price);
        }
        [Fact]
        public async Task GetPropertyDetailsById_ShouldReturn302Status()
        {
            var PropertyService = new Mock<IPropertyRepo>();
            PropertyService.Setup(x => x.GetPropertyDetailAsync(1)).ReturnsAsync(new Response("Property Found", StatusCodes.Status302Found, PropertyList[0], ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var propcon = new PropertyController(PropertyService.Object, mapper);
            var result = await propcon.GetPropertyDetail(1);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            
            Assert.Equal(StatusCodes.Status302Found, response.Code);
            
        }


        [Fact]
        public async Task GetPropertyDetailsById_ShouldReturn404Status()
        {
            var PropertyService = new Mock<IPropertyRepo>();
            PropertyService.Setup(x => x.GetPropertyDetailAsync(3)).ReturnsAsync(new Response("", StatusCodes.Status404NotFound, "", "Property not Found"));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var propcon = new PropertyController(PropertyService.Object, mapper);
            var result = await propcon.GetPropertyDetail(3);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;

            Assert.Equal(StatusCodes.Status404NotFound, response.Code);

        }





        [Fact]
    public async Task AddProperty_ShouldReturn201Status()
        {
            var PropertyService = new Mock<IPropertyRepo>();
            PropertyService.Setup(x => x.AddProperty(It.IsAny<Property>())).ReturnsAsync(new Response("Added Successfully", StatusCodes.Status201Created, PropertyList[0], ""));
            var myProfile = new AutoMapperProfiles();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            var propcon = new PropertyController(PropertyService.Object,mapper);
            var result = await propcon.AddProperty(PropertyDtoList[0]);
            var okObjectResult = result as OkObjectResult;
            var response = okObjectResult.Value as Response;
            Assert.Equal(StatusCodes.Status201Created,response.Code );

        }


      
    }
}
