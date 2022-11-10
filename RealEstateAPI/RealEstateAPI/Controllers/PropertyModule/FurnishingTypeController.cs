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
        public FurnishingTypeController(IFurnishingTypeRepo repo, IMapper _mapper)
        {
            _repo = repo;
            mapper = _mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetFurnishingTypes()
        {
            var FurnishingTypes = await _repo.GetFurnishingTypesAsync();
            var FurnishingTypesDto = mapper.Map<IEnumerable<KeyValuePairDto>>(FurnishingTypes.Data);
            return Ok(FurnishingTypesDto);
        }
    }
}
