using RealEstateAPI.DomainModels;
using RealEstateAPI.Models;
using RealEstateAPI.Models.AuthModels;

namespace RealEstateAPI.Repositories.LoginRepo
{
    public interface IAuthRepo
    {

        Task<Response> RegisterUserAsync(DomainRegister req);
        Task<Response> ValidateUserAsync(Login req);
        Response CreateResponse(string message, int code, dynamic data, string error);

    }
}
