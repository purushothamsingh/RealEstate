using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class FurnishingTypeRepo : IFurnishingTypeRepo
    {
        private readonly ApplicationDbContext _context;

        private static Response response = new Response();
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(FurnishingTypeRepo));


        public FurnishingTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Response> GetFurnishingTypesAsync()
        {
            _log4net.Info("Get Furnishing Type Repository method invoked");
            var furnishingTypes = await _context.FurnishingTypes.ToListAsync();

            if (furnishingTypes != null)
            {
                _log4net.Info("Property Found");
                return new Response("Property Found", StatusCodes.Status302Found, furnishingTypes, "");
            }
            _log4net.Error("404 Error: Property not found");
            return new Response("", StatusCodes.Status404NotFound, "", "Property not Found");
        }
    }
}
