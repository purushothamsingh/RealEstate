using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class PropertyRepo : IPropertyRepo
    {
        private readonly ApplicationDbContext _context;
        
        private static Response response = new Response();
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PropertyRepo));

        public PropertyRepo(ApplicationDbContext context)
        {
            _context = context;
           
        }
        public Response CreateResponse(string message, int code, dynamic data, string error)
        {
            response.Message = message;
            response.Code = code;
            response.Data = data;
            response.Error = error;

            return response;
        }

        public async Task<Response> GetPropertiesByIdAsync(int id)
        {
            _log4net.Info("Get Properties By Id Repository method invoked");
            var properties = await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.City)
                .Include(p => p.FurnishingType)                
                .Where(p => p.SellRent == id).ToListAsync();
            if(properties != null)
            {
                _log4net.Info("Properties of Category " +id+ "found");
                return CreateResponse("Property Found", StatusCodes.Status302Found, properties, "");
            }
            _log4net.Error("404 Error: Property not found");
            return CreateResponse("", StatusCodes.Status404NotFound, "", "Property not Found");
        }

        public async Task<Response> AddProperty(Property property)
        {
            _log4net.Info("Add Property Repository method invoked");
           await _context.Properties.AddAsync(property);
            _context.SaveChanges();
            _log4net.Info("Property added Successfully");
            return CreateResponse("Added Property successfully",StatusCodes.Status201Created,property,"");
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
                .Where(p => p.Id == id)
                .FirstAsync();
            if (property != null)
            {
                _log4net.Info("Property found Successfully");
                return CreateResponse("Property Found", StatusCodes.Status302Found, property, "");
            }
            _log4net.Error("404 - Not Found: Property not found");
            return CreateResponse("", StatusCodes.Status404NotFound, "", "Property not Found");
        }
    }
}
