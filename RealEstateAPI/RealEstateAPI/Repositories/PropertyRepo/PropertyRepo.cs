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
            var properties = await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.City)
                .Include(p => p.FurnishingType)                
                .Where(p => p.SellRent == id).ToListAsync();
            if(properties != null)
            {
                return CreateResponse("Property Found", StatusCodes.Status302Found, properties, "");
            }
            return CreateResponse("", StatusCodes.Status404NotFound, "", "Property not Found");
        }

        public async Task<Response> AddProperty(Property property)
        {
           await _context.Properties.AddAsync(property);
            _context.SaveChanges();
            return CreateResponse("Added Property successfully",StatusCodes.Status201Created,property,"");
        }

        public Task<Response> DeleteProperty(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> GetPropertyDetailAsync(int id)
        {
            var property = await _context.Properties
                .Include(p => p.PropertyType)
                .Include(p => p.City)
                .Include(p => p.FurnishingType)
                .Where(p => p.Id == id)
                .FirstAsync();
            if (property != null)
            {
                return CreateResponse("Property Found", StatusCodes.Status302Found, property, "");
            }
            return CreateResponse("", StatusCodes.Status404NotFound, "", "Property not Found");
        }
    }
}
