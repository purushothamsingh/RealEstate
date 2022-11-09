using AutoMapper;
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
        private readonly IMapper mapper;
        public City(ApplicationDbContext _db,IMapper _mapper)
        {
            db = _db;
            mapper = _mapper;
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
        public async Task<Response> AddCityAsync(Cities city)
        {
            var cities = await db.Cities.AddAsync(city);
            db.SaveChanges();
            return CreateResponse("Added Successfully", StatusCodes.Status201Created, city, "");
        }
        public async Task<Response> DeleteCityAsync(int CityId)
        {
            var city = await db.Cities.FindAsync(CityId);
            if(city != null)
            {
                db.Remove(city);
                db.SaveChanges();
                return CreateResponse("Deleted Successfully", StatusCodes.Status200OK, "", "");
            }
            else { return CreateResponse("", StatusCodes.Status400BadRequest, "", "City Not Found"); }
           
        }
        public async Task<Response> UpdateCityAsync(int id, CityDto city)
            
        {
            var fetchCity = db.Cities.Find(id);
            
            if (fetchCity != null)
            {
                fetchCity.LastUpdatedBy = 1;
                fetchCity.LastUpdatedOn = DateTime.Now;
                fetchCity.Name =  city.Name;
                fetchCity.Country = city.Country;
                db.Cities.Update(fetchCity);
                db.SaveChanges();
                return CreateResponse("updated Successfully", StatusCodes.Status200OK, "", "");
            }
          return CreateResponse("", StatusCodes.Status204NoContent, "", "City Not Found");
        }


    }
}
