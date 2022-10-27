using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;

namespace RealEstateAPI.Repositories.LoginRepo
{
    public interface IAuthRepo
    {

        Task<Response> RegisterUser(DomainRegister req);
    }
}
