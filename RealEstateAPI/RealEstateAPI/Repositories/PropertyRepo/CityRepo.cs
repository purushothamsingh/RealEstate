using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property ;
using RealEstateAPI.Repositories.LoginRepo;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public class CityRepo : ICityRepo
    {
        private static Response response = new Response();

        private readonly ApplicationDbContext db;

        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CityRepo));

        public CityRepo(ApplicationDbContext _db)
        {
            db = _db;
            
        }

        
        public async Task<Response> GetCitiesAsync()
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Get Cities Repository method invoked");
            var cites = await db.Cities.ToListAsync();
            if(cites.Count >0 )
            {
                _log4net.Info("Cities details found");
                return new Response("cities found", StatusCodes.Status302Found, cites, "");
            }
            else {
                _log4net.Error("404 Error: No Cities Found");
                return new Response("", StatusCodes.Status404NotFound, null, "No cities found"); 
            }
        }
        public async Task<Response> AddCityAsync(City city)
        {
            _log4net.Info("Add City Repository method invoked");
            var cities = await db.Cities.AddAsync(city);
            db.SaveChanges();
            _log4net.Info("City added Successfully");
            return new Response("Added Successfully", StatusCodes.Status201Created, city, "");
        }
        public async Task<Response> DeleteCityAsync(int CityId)
        {
            _log4net.Info("Delete City Repository method invoked");
            var city = await db.Cities.FindAsync(CityId);
            if(city != null)
            {
                db.Remove(city);
                db.SaveChanges();
                _log4net.Info("City deleted successfully");
                return new Response("Deleted Successfully", StatusCodes.Status200OK, "", "");
            }
            else {
                _log4net.Error("400 BadRequest: City not Found");
                return new Response("", StatusCodes.Status400BadRequest, "", "City Not Found"); 
            }
           
        }
        public async Task<Response> UpdateCityAsync(int id, CityDto city)
            
        {
            _log4net.Info("Update City Repository method invoked");
            var fetchCity = db.Cities.Find(id);
            
            if (fetchCity != null)
            {
                fetchCity.LastUpdatedBy = 1;
                fetchCity.LastUpdatedOn = DateTime.Now;
                fetchCity.Name =  city.Name;
                fetchCity.Country = city.Country;
                db.Cities.Update(fetchCity);
                db.SaveChanges();
                _log4net.Info("City Updated Successfully");
                return new Response("updated Successfully", StatusCodes.Status200OK, "", "");
            }
            _log4net.Error("204 - No Content: City Not Found");
          return new Response("", StatusCodes.Status204NoContent, "", "City Not Found");
        }


    }
}
