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
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(PropertyTypeController));
        public PropertyTypeController(IPropertyTypeRepo repo, IMapper _mapper)
        {
            _repo = repo;
            mapper = _mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            _log4net.Info("------------------------------------------------------------------------------------");
            _log4net.Info("Get Property Types method invoked");

            var PropertyTypes = await _repo.GetPropertyTypesAsync();
            dynamic PropertyTypesDto="";
            if (!PropertyTypes.Data.Equals(""))
            {
                PropertyTypesDto = mapper.Map<IEnumerable<KeyValuePairDto>>(PropertyTypes.Data);
            }
            PropertyTypes.Data= PropertyTypesDto;
            return Ok(PropertyTypes);
        }
    }
}
