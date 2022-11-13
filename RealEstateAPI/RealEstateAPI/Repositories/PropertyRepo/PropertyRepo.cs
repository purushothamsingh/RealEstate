using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

using RealEstateAPI.Repositories.PhotoRepo;

using System.Security.Claims;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class PropertyRepo : IPropertyRepo
    {
        private readonly ApplicationDbContext _context;
        private static Response response = new Response();

        private readonly IPhotoService photoService;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PropertyRepo));
        public PropertyRepo(ApplicationDbContext context, IPhotoService photoService)
        {
            _context = context;
            this.photoService = photoService; 
        }

        public async Task<Response> GetPropertiesByIdAsync(int id)
        {
            _log4net.Info("Get Properties By Id Repository method invoked");
            var properties = await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.City)
                .Include(p => p.FurnishingType)  
                .Include(p => p.Photos)
                .Where(p => p.SellRent == id).ToListAsync();
            if(properties != null)
            {
                _log4net.Info("Properties of Category " +id+ "found");
                return new Response("Property Found", StatusCodes.Status302Found, properties, "");
            }
            _log4net.Error("404 Error: Property not found");
            return new Response("", StatusCodes.Status404NotFound, "", "Property not Found");
        }

        public async Task<Response> AddProperty(Property property)
        {
            _log4net.Info("Add Property Repository method invoked");
           await _context.Properties.AddAsync(property);
            _context.SaveChanges();
            _log4net.Info("Property added Successfully");
            return new Response("Added Property successfully",StatusCodes.Status201Created,property,"");
        }

        public Task<Response> DeleteProperty(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetPropertyDetailAsync(int id)
        {
            _log4net.Info("Get Property Details Repository method invoked");
            var property = await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.City)
                .Include(p => p.FurnishingType)
                .Include(p => p.Photos)
                .Where(p => p.Id == id)
                .FirstAsync();
            if (property != null)
            {
                _log4net.Info("Property found Successfully");
                return new Response("Property Found", StatusCodes.Status302Found, property, "");
            }
            _log4net.Error("404 - Not Found: Property not found");
            return new Response("", StatusCodes.Status404NotFound, "", "Property not Found");
        }

        public async Task<Response> GetPropertyByPostedByIdAsync(int userId)
        {
            var reqProperties = await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.City)
                .Include(p => p.FurnishingType)
                .Where(p => p.PostedBy == userId)
                .ToListAsync();
            if (reqProperties != null)
            {
                
                return CreateResponse("Properties Found", StatusCodes.Status302Found, reqProperties, "");
            }
            
            return CreateResponse("", StatusCodes.Status404NotFound, "", "Property not Found");
        }
    }
}
