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
        public PropertyController(IPropertyRepo repo, IMapper _mapper)
        {
            _repo = repo;
            mapper = _mapper;
        }

        [HttpGet("list/{SellRent}")]
        public async Task<IActionResult> GetPropertyList(int SellRent)
        {
            var properties = await _repo.GetPropertiesByIdAsync(SellRent);
            var propertyListDto = mapper.Map<IEnumerable<PropertyListDto>>(properties.Data);
            return Ok(propertyListDto);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            var property = await _repo.GetPropertyDetailAsync(id);
            var propertyDTO = mapper.Map<PropertyDetailDto>(property.Data);
            return Ok(propertyDTO);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            var property = mapper.Map<Property>(propertyDto);
            property.PostedBy = 1;
            property.LastUpdatedBy = 1;
            _repo.AddProperty(property);
            return StatusCode(201);

        }
    }
}
