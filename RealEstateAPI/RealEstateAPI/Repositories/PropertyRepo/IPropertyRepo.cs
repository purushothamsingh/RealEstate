

using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public interface IPropertyRepo
    {
        Task<Response> GetPropertiesByIdAsync(int cityId);
        Task<Response> AddProperty(Property property);
        Task<Response> DeleteProperty(int id);
        Task<Response> GetPropertyDetailAsync(int id);

        Task<Response> GetPropertyByPostedByIdAsync(int userId);
        
    }
}
