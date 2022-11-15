using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.Cryptography;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Migrations;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

using RealEstateAPI.Repositories.PhotoRepo;

using RealEstateAPI.Repositories.LoginRepo;

using RealEstateAPI.Repositories.PropertyRepo;
using Property = RealEstateAPI.Models.Property.Property;
using System.Security.Claims;


namespace RealEstateAPI.Controllers.PropertyModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepo _repo;

        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly ApplicationDbContext _context;
         private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PropertyController));
        public PropertyController(IPropertyRepo repo, IMapper mapper)
        {
            _repo = repo;
            this.mapper = mapper;
           
        }
        //protected int GetUserId()
        //{
        //    return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //}

        [HttpGet("list/{SellRent}")]
        public async Task<IActionResult> GetPropertyList(int SellRent)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("GetPropertyList based on Sell(1)/Rent(2) method invoked");
            _log4net.Info("SellRent Id: " + SellRent);        


            var properties = await _repo.GetPropertiesByIdAsync(SellRent);

            dynamic propertyDTO = "";
            if (!properties.Data.Equals(""))
            {

                 propertyDTO = mapper.Map<IEnumerable<PropertyListDto>>(properties.Data);
            }
            properties.Data= propertyDTO;
           return Ok(properties);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {

            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("GetPropertyDetail based on Property Id method invoked");
            _log4net.Info("Property Id: " + id);
                var property = await _repo.GetPropertyDetailAsync(id);
            dynamic propertyDTO = "";
            if (!property.Data.Equals(""))
            {
                 propertyDTO = mapper.Map<PropertyDetailDto>(property.Data);
            }
            property.Data= propertyDTO;
            return Ok(property);
        }

        [Authorize]        
        [HttpPost("add")]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {

            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("AddProperty method invoked");           

            var property = mapper.Map<Property>(propertyDto);
            property.PostedBy = Auth.userId;
            property.LastUpdatedBy = Auth.userId;
            var addedProperty= await _repo.AddProperty(property);
            return Ok(addedProperty);
        }

       
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            var response =await _repo.DeleteProperty(id);
            return Ok(response);
        }

        [HttpGet("listOfProperties/{postedById}")]
        public async Task<IActionResult> getPropertiesByPostedById(int postedById)
        {
            var properties = await _repo.GetPropertyByPostedByIdAsync(postedById);
            var propertyListDto = mapper.Map<IEnumerable<PropertyListDto>>(properties.Data);
            properties.Data = propertyListDto;
            return Ok(properties);

        }

    }
}
