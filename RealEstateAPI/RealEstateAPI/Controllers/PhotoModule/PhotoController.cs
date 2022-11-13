using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.PhotoRepo;
using RealEstateAPI.Repositories.PropertyRepo;

namespace RealEstateAPI.Controllers.PhotoModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {

        private readonly IPropertyRepo _repo;

        private readonly IMapper mapper;
        private readonly IPhotoService photoService;
        private readonly ApplicationDbContext _context;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PhotoController));
        public PhotoController(IPropertyRepo repo, IMapper mapper, IPhotoService photoService, ApplicationDbContext context)
        {
            _repo = repo;
            this.mapper = mapper;
            this.photoService = photoService;
            _context = context;
        }
        [HttpPost("add/photo/{propId}")]
        public async Task<IActionResult> AddPropertyPhoto(IFormFile file, int propId)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("AddPropertyPhoto method invoked");
            var result = await photoService.UploadPhotoAsync(file);
            if (result.Error != null)
            {
                _log4net.Error("400 - BadRequest");
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

            _log4net.Info("Property photo added successfully");
            return StatusCode(201);
        }

        [HttpPost("setprimaryphoto/{propId}/{photoPublicId}")]
        public async Task<IActionResult> SetPrimaryPhoto(int propId, string photoPublicId)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("SetPrimaryPhoto method invoked");
            bool IsPhoto = false;
            //var userId = GetUserId();
            var property = await _context.Properties
            .Include(p => p.Photos)
            .Where(p => p.Id == propId)
            .FirstOrDefaultAsync();

            if (property == null)
            {
                _log4net.Error("400 - BadRequest: No such property or photo exists");
                return BadRequest("No such property or photo exists");
            }

            //if (property.PostedBy != userId)
            //    return BadRequest("You are not authorised to change the photo");

            var photo = property.Photos.FirstOrDefault(p => p.PublicId == photoPublicId);

            if (photo == null)
            {
                _log4net.Error("400 - BadRequest: No such property or photo exists");
                return BadRequest("No such property or photo exists");
            }
                
            if (photo.IsPrimary)
            {
                _log4net.Error("400 - BadRequest: This is already a primary photo");
                return BadRequest("This is already a primary photo");
            }
                

            var currentPrimary = property.Photos.FirstOrDefault(p => p.IsPrimary);
            if (currentPrimary != null) currentPrimary.IsPrimary = false;
            photo.IsPrimary = true;

            if (photo != null)
            {
                await _context.SaveChangesAsync();

                _log4net.Info("Primary photo added successfully");
                return NoContent();
            }
            else
            {
                _log4net.Error("400 - BadRequest: Some error occured, failed to load primary photo");
                return BadRequest("Some error has occured, failed to set primary photo");
            }

        }

        [HttpDelete("deletephoto/{propId}/{photoPublicId}")]
        public async Task<IActionResult> DeletePhoto(int propId, string photoPublicId)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("DeletePhoto method invoked");
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
            {
                _log4net.Error("400 - BadRequest: No such property or photo exists");
                return BadRequest("No such property or photo exists");
            }
                

            if (photo.IsPrimary)
            {
                _log4net.Error("400 - BadRequest: Primary photo cannot be deleted");
                return BadRequest("You can not delete primary photo");
            }
                

            if (photo.PublicId != null)
            {
                var result = await photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null)
                {
                    _log4net.Error("400 - BadRequest");
                    return BadRequest(result.Error.Message);
                }
            }

            if (photo != null)
            {
                property.Photos.Remove(photo);
                await _context.SaveChangesAsync();
                _log4net.Info("Photo deleted Successfully");
                return Ok();
            }
            else
            {
                _log4net.Error("400 - BadRequest: Failed to delete photo");
                return BadRequest("Failed to delete photo");
            }
                
        }
    }
}
