using Microsoft.EntityFrameworkCore;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class City : ICityRepo
    {
        private static Response response = new Response();

        private readonly ApplicationDbContext db;
        public City(ApplicationDbContext _db)
        {
            db = _db;

        }

        public Response CreateResponse(string message, int code, dynamic data, string error)
        {
            response.Message = message;
            response.Code = code;
            response.Data = data;
            response.Error = error;

            return response;
        }

        public async Task<Response> GetCitiesAsync()
        {
            var cites = await db.Cities.ToListAsync();
            if(cites.Count >0 )
            {
                return CreateResponse("cities found", StatusCodes.Status302Found, cites, "");
            }
            else { return CreateResponse("", StatusCodes.Status404NotFound, null, "No cities found"); }
        }
        public async Task<Response> AddCity(Cities city)
        {
            var cities = await db.Cities.AddAsync(city);
            db.SaveChanges();

            return CreateResponse("Added Successfully", StatusCodes.Status201Created, city, "");
        }

    }
}
