using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.Controllers.LoginModule;
using RealEstateAPI.DomainModels;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.DomainModels.WishDto;
using RealEstateAPI.Models.WishModule;
using RealEstateAPI.Repositories.LoginRepo;
using RealEstateAPI.Repositories.WishRepo;
using wished = RealEstateAPI.Models.WishModule.wished;

namespace RealEstateAPI.Controllers.WishModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        private readonly IWishRepo wishRepo;
        private readonly IMapper mapper;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(AuthController));


        public static int SaveOtp;
        public WishController(IWishRepo _authRepo, IMapper _mapper)
        {
            wishRepo = _authRepo;
            mapper = _mapper;
        }



        [HttpPost()]
        public async Task<IActionResult> WishedData(WishedDto wishedData)
        {

            var data = await wishRepo.AddedWishAsync(wishedData);

            return Ok(data);
        }
        [HttpGet("userWished/{id:int}")]

        public async Task<IActionResult> WishedData(int id)
        {

            var data = await wishRepo.GetwishedList(id);

            return Ok(data);
        }

    }
}
