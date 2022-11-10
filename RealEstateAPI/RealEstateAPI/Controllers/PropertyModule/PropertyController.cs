using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MimeKit.Cryptography;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.PhotoRepo;
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
        private readonly IMapper _mapper;
        private readonly IPhotoService photoService;
        private readonly ApplicationDbContext _context;
        public PropertyController(IPropertyRepo repo, IMapper mapper, IPhotoService photoService, ApplicationDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            this.photoService = photoService;
            _context = context;
        }

        [HttpGet("list/{SellRent}")]
        public async Task<IActionResult> GetPropertyList(int SellRent)
        {
            var properties = await _repo.GetPropertiesByIdAsync(SellRent);
            var propertyListDto = _mapper.Map<IEnumerable<PropertyListDto>>(properties.Data);
            properties.Data= propertyListDto;
            return Ok(properties);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            var property = await _repo.GetPropertyDetailAsync(id);
            var propertyDTO = _mapper.Map<PropertyDetailDto>(property.Data);
            property.Data = propertyDTO;
            return Ok(property);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProperty(PropertyDto propertyDto)
        {
            var property = _mapper.Map<Property>(propertyDto);
            property.PostedBy = 1;
            property.LastUpdatedBy = 1;
            var addedProperty = _repo.AddProperty(property);
            return Ok(addedProperty);
        }

        [HttpPost("add/photo/{propId}")]
        public async Task<IActionResult> AddPropertyPhoto(IFormFile file, int propId)
        {
            var result = await photoService.UploadPhotoAsync(file);
            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }
            var property = await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.City)
                .Include(p => p.FurnishingType)
                .Include(p => p.Photos)
                .Where(p => p.Id == propId)
                .FirstAsync();

            var photo = new Photo
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (property.Photos.Count == 0)
            {
                photo.IsPrimary = true;
            }
            property.Photos.Add(photo);

            _context.SaveChanges();

            return StatusCode(201);
        }
    }
}
