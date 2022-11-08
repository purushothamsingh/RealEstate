using RealEstateAPI.DomainModels;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public interface ICityRepo
    {
        Task<Response> GetCitiesAsync();
        Task<Response> AddCity(Cities city);
    }
}
