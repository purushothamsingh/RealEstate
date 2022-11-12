using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Controllers.PropertyModule;
using RealEstateAPI.Models;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class PropertyTypeRepo : IPropertyTypeRepo
    {
        private readonly ApplicationDbContext _context;

        private static Response response = new Response();
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PropertyTypeRepo));

        public PropertyTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

       

        public async Task<Response> GetPropertyTypesAsync()
        {
            _log4net.Info("Get Property Types Repository method invoked");
            var propertyTypes = await _context.PropertyTypes.ToListAsync();

            if (propertyTypes != null)
            {
                _log4net.Info("Property Types Found");
                return new Response("Property Types Found", StatusCodes.Status302Found, propertyTypes, "");
            }
            _log4net.Error("404 Error: Property Types not found");
            return new Response("", StatusCodes.Status404NotFound, "", "Property Types not Found");
        }
    }
}
