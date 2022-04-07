using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MecOrb.Api.Controllers
{
    [ApiController]
    [Route("simulations")]
    public class SimulationController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly ISimulationApplication _simulationApplication;

        public SimulationController(IMapper mapper, ISimulationApplication simulationApplication)
        {
            _mapper = mapper;
            _simulationApplication = simulationApplication;
        }

        [HttpPost]
        [Route("two-bodies")]
        [ProducesResponseType(typeof(SimulationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult SimulateTwoBodies(SimulationConfigModel simulationConfig)
        {
            Simulation simulationResult = _simulationApplication.SimulateTwoBodies(simulationConfig);

            return Ok(_mapper.Map<Simulation, SimulationModel>(simulationResult));
        }



    }
}
