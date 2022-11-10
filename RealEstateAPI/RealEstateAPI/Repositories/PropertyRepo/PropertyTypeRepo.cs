using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class PropertyTypeRepo : IPropertyTypeRepo
    {
        private readonly ApplicationDbContext _context;

        private static Response response = new Response();

        public PropertyTypeRepo(ApplicationDbContext context)
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

        public async Task<Response> GetPropertyTypesAsync()
        {
            var propertyTypes = await _context.PropertyTypes.ToListAsync();

            if (propertyTypes != null)
            {
                return CreateResponse("Property Type Found", StatusCodes.Status302Found, propertyTypes, "");
            }
            return CreateResponse("", StatusCodes.Status404NotFound, "", "Property Type not Found");
        }
    }
}
