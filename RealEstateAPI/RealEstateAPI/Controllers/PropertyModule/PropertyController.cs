using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.Cryptography;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.PropertyRepo;
using System.Runtime.CompilerServices;
using Property = RealEstateAPI.Models.Property.Property;

namespace RealEstateAPI.Controllers.PropertyModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepo _repo;
        private readonly IMapper mapper;

        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PropertyController));
        public PropertyController(IPropertyRepo repo, IMapper _mapper)
        {
            _repo = repo;
            mapper = _mapper;
        }

        [HttpGet("list/{SellRent}")]
        public async Task<IActionResult> GetPropertyList(int SellRent)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Get Property List based on Sell(1)/Rent(2) invoked");
            _log4net.Info("SellRent Id: " + SellRent);        


            var properties = await _repo.GetPropertiesByIdAsync(SellRent);
            var propertyListDto = mapper.Map<IEnumerable<PropertyListDto>>(properties.Data);
            properties.Data= propertyListDto;
            return Ok(properties);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Get Property Detail based on Property Id invoked");
            _log4net.Info("Property Id: " + id);

            var property = await _repo.GetPropertyDetailAsync(id);
            var propertyDTO = mapper.Map<PropertyDetailDto>(property.Data);
            property.Data= propertyDTO;
            return Ok(property);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Add Property method invokded");           

            var property = mapper.Map<Property>(propertyDto);
            property.PostedBy = 1;
            property.LastUpdatedBy = 1;
            var addedProperty=_repo.AddProperty(property);
            return Ok(addedProperty);

        }
    }
}
