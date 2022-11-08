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
        private readonly IMapper mapper;
        public CityController(ICityRepo _cityRepo,IMapper _mapper)
        {
            cityRepo = _cityRepo;
            mapper = _mapper;
        }

        [HttpGet("GetCities")]

        public async Task<IActionResult> GetCities()
        {
            var cities = await cityRepo.GetCitiesAsync();
            return Ok(cities);
        }

        [HttpPost("AddCity")]

        public async Task<IActionResult> AddCity(Cities city)
        {
            var fetchedCity = await cityRepo.AddCityAsync(city);
            return Ok(fetchedCity); 
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
           var data = await cityRepo.DeleteCityAsync(id);
            return Ok(data);
        }
        [HttpPut("{id}/updateCity")]

        public async Task<IActionResult> UpdateCity(int id, CityDto citydto)
        {
            var Updateddata = await cityRepo.UpdateCityAsync(id, citydto);
            return Ok(Updateddata);


        }
    }
}
