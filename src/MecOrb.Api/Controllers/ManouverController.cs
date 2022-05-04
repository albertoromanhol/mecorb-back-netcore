using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MecOrb.Api.Controllers
{
    [ApiController]
    [Route("manouvers")]
    public class ManouverController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IManouverApplication _manouverApplication;

        public ManouverController(IMapper mapper, IManouverApplication manouverApplication)
        {
            _mapper = mapper;
            _manouverApplication = manouverApplication;
        }

        [HttpPost]
        [Route("hohmann")]
        [ProducesResponseType(typeof(SimulationResultModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult SimulateHohmann(ManouverConfigModel manouverConfig)
        {
            ManouverResult manouverResult = _manouverApplication.Hohmann(manouverConfig);

            return Ok(_mapper.Map<ManouverResult, ManouverResultModel>(manouverResult));
        }


        [HttpPost]
        [Route("bi-elliptic")]
        [ProducesResponseType(typeof(SimulationResultModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult SimulateBiElliptic(ManouverConfigModel manouverConfig)
        {
            ManouverResult manouverResult = _manouverApplication.BiElliptic(manouverConfig);

            return Ok(_mapper.Map<ManouverResult, ManouverResultModel>(manouverResult));
        }
    }
}
