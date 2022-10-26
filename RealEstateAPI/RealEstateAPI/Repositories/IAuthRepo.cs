using RealEstateAPI.DomainModels;
using RealEstateAPI.Models.AuthModels;

namespace RealEstateAPI.Repositories
{
    public interface IAuthRepo
    {

         Task<Db_Register> RegisterUser(DomainRegister req);
    }
}
