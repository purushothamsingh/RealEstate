using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class FurnishingTypeRepo : IFurnishingTypeRepo
    {
        private readonly ApplicationDbContext _context;

        private static Response response = new Response();

        public FurnishingTypeRepo(ApplicationDbContext context)
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
        public async Task<Response> GetFurnishingTypesAsync()
        {
            var furnishingTypes = await _context.FurnishingTypes.ToListAsync();

            if (furnishingTypes != null)
            {
                return CreateResponse("Property Found", StatusCodes.Status302Found, furnishingTypes, "");
            }
            return CreateResponse("", StatusCodes.Status404NotFound, "", "Property not Found");
        }
    }
}
