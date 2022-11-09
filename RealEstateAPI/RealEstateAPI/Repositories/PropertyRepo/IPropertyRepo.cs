

using RealEstateAPI.Models;
using RealEstateAPI.Models.Property;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public interface IPropertyRepo
    {
        Task<Response> GetPropertiesByIdAsync(int cityId);
        void AddProperty(Property property);
        void DeleteProperty(int id);
        Task<Response> GetPropertyDetailAsync(int id);
    }
}
