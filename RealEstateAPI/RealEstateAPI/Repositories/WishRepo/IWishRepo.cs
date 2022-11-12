using RealEstateAPI.DomainModels.WishDto;
using RealEstateAPI.Models;
using RealEstateAPI.Models.WishModule;

namespace RealEstateAPI.Repositories.WishRepo
{
    public interface IWishRepo
    {
        Task<Response> AddedWishAsync(WishedDto wishedData);
        Task<Response> GetwishedList(int id);

    }
}
