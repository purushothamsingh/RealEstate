using RealEstateAPI.Models;

namespace RealEstateAPI.Repositories.PropertyRepo
{
    public interface IFurnishingTypeRepo
    {
        Task<Response> GetFurnishingTypesAsync();
    }
}
