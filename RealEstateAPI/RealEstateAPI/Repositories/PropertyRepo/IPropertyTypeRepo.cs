using RealEstateAPI.Models;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public interface IPropertyTypeRepo
    {
        Task<Response> GetPropertyTypesAsync();
    }
}
