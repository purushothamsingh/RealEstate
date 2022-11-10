using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateAPI.DomainModels.PropertyDtos;
using RealEstateAPI.Repositories.PropertyRepo;

namespace RealEstateAPI.Controllers.PropertyModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypeController : ControllerBase
    {
        private readonly IPropertyTypeRepo _repo;
        private readonly IMapper mapper;
        public PropertyTypeController(IPropertyTypeRepo repo, IMapper _mapper)
        {
            _repo = repo;
            mapper = _mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            var PropertyTypes = await _repo.GetPropertyTypesAsync();
            var PropertyTypesDto = mapper.Map<IEnumerable<KeyValuePairDto>>(PropertyTypes.Data);
            PropertyTypes.Data= PropertyTypesDto;
            return Ok(PropertyTypes);
        }
    }
}
