using RealEstateAPI.DomainModels;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public interface ICityRepo
    {
        Task<Response> GetCitiesAsync();
        Task<Response> AddCityAsync(Cities city);
        Task<Response> DeleteCityAsync( int cityId );
        Task<Response> UpdateCityAsync(int id, CityDto city);
    }
}
