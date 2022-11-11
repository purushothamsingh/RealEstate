using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models.Property;
using RealEstateAPI.Repositories.LoginRepo;
using RealEstateAPI.Repositories.PropertyRepo;

namespace RealEstateAPI.Controllers.PropertyModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {

        private readonly ICityRepo cityRepo;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CityController));

        public CityController(ICityRepo _cityRepo)
        {
            cityRepo = _cityRepo;
           
        }

        [HttpGet("GetCities")]

        public async Task<IActionResult> GetCities()
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Get Cities method invoked");
            

            var cities = await cityRepo.GetCitiesAsync();
            return Ok(cities);
        }

        [HttpPost("AddCity")]

        public async Task<IActionResult> AddCity(City city)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Add City method invoked");
            

            var fetchedCity = await cityRepo.AddCityAsync(city);
            return Ok(fetchedCity); 
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Delete City based on Id invoked");
            _log4net.Info("Deleted City Id: " + id);
            
            var data = await cityRepo.DeleteCityAsync(id);
            return Ok(data);
        }
        [HttpPut("{id}/updateCity")]

        public async Task<IActionResult> UpdateCity(int id, CityDto citydto)
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Update City method invoked");
            _log4net.Info("Updated City Id: " + id);

            var Updateddata = await cityRepo.UpdateCityAsync(id, citydto);
            return Ok(Updateddata);


        }
    }
}
