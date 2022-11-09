using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Cryptography;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models;
using RealEstateAPI.Repositories.PropertyRepo;
using System.Runtime.CompilerServices;

namespace RealEstateAPI.Controllers.PropertyModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepo _repo;
        private readonly IMapper mapper;
        public PropertyController(IPropertyRepo repo,IMapper _mapper)
        {
            _repo = repo;
            mapper = _mapper;
        }

        [HttpGet("{SellRent}")]
        public async Task<IActionResult> GetPropertyList(int SellRent)
        {
            var properties = await _repo.GetPropertiesByIdAsync(SellRent);
            var propertyListDto = mapper.Map<IEnumerable<PropertyListDto>>(properties.Data);
            return Ok(propertyListDto);
        }
    }
}
