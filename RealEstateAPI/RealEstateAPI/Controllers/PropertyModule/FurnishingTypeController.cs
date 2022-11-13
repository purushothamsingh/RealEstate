using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Repositories.PropertyRepo;

namespace RealEstateAPI.Controllers.PropertyModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class FurnishingTypeController : ControllerBase
    {
        private readonly IFurnishingTypeRepo _repo;
        private readonly IMapper mapper;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(FurnishingTypeController));
        public FurnishingTypeController(IFurnishingTypeRepo repo, IMapper _mapper)
        {
            _repo = repo;
            mapper = _mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetFurnishingTypes()
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("GetFurnishingTypes method invoked");

            var FurnishingTypes = await _repo.GetFurnishingTypesAsync();
            dynamic furnishingTypesDTO = "";
            if (!FurnishingTypes.Data.Equals(""))
            {

                furnishingTypesDTO = mapper.Map<IEnumerable<KeyValuePairDto>>(FurnishingTypes.Data);
            }
                FurnishingTypes.Data= furnishingTypesDTO;
            return Ok(FurnishingTypes);
        }
    }
}
