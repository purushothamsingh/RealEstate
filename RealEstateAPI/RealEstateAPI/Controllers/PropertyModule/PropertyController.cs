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

        private readonly IMapper _mapper;
        private readonly IPhotoService photoService;
        private readonly ApplicationDbContext _context;
         private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PropertyController));
        public PropertyController(IPropertyRepo repo, IMapper mapper, IPhotoService photoService, ApplicationDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            this.photoService = photoService;
            _context = context;
        }
        //protected int GetUserId()
        //{
        //    return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //}

        [HttpGet("list/{SellRent}")]
        public async Task<IActionResult> GetPropertyList(int SellRent)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Get Property List based on Sell(1)/Rent(2) invoked");
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
            _log4net.Info("Get Property Detail based on Property Id invoked");
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
            _log4net.Info("Add Property method invoked");           

            var property = mapper.Map<Property>(propertyDto);
            property.PostedBy = Auth.userId;
            property.LastUpdatedBy = Auth.userId;
            var addedProperty= await _repo.AddProperty(property);
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
                .FirstOrDefaultAsync();

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

        [HttpPost("set-primary-photo/{propId}/{photoPublicId}")]
        public async Task<IActionResult> SetPrimaryPhoto(int propId, string photoPublicId)
        {
            bool IsPhoto = false;
            //var userId = GetUserId();
            var property = await _context.Properties
            .Include(p => p.Photos)
            .Where(p => p.Id == propId)
            .FirstOrDefaultAsync();

            if (property == null)
                return BadRequest("No such property or photo exists");

            //if (property.PostedBy != userId)
            //    return BadRequest("You are not authorised to change the photo");

            var photo = property.Photos.FirstOrDefault(p => p.PublicId == photoPublicId);

            if (photo == null)
                return BadRequest("No such property or photo exists");
            if (photo.IsPrimary)
                return BadRequest("This is already a primary photo");

            var currentPrimary = property.Photos.FirstOrDefault(p => p.IsPrimary);
            if(currentPrimary != null) currentPrimary.IsPrimary = false;
            photo.IsPrimary = true;

            if (photo != null)
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                return BadRequest("Some error has occured, failed to set primary photo");
            }

        }

        [HttpDelete("delete-photo/{propId}/{photoPublicId}")]
        public async Task<IActionResult> DeletePhoto(int propId, string photoPublicId)
        {
            //var userId = GetUserId();
            bool IsPhoto = false;
            var property = await _context.Properties
            .Include(p => p.Photos)
            .Where(p => p.Id == propId)
            .FirstOrDefaultAsync();

            //if (property.PostedBy != userId)
            //    return BadRequest("You are not authorised to delete the photo");

            //if (property == null || property.PostedBy != userId)
            //    return BadRequest("No such property or photo exists");

            var photo = property.Photos.FirstOrDefault(p => p.PublicId == photoPublicId);

            if (photo == null)
                return BadRequest("No such property or photo exists");

            if (photo.IsPrimary)
                return BadRequest("You can not delete primary photo");

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            if(photo != null)
            {
                property.Photos.Remove(photo);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest("Failed to delete photo");
        }
    }
}
