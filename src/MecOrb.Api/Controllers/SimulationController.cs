using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        [ProducesResponseType(typeof(SimulationResultModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Simulate(SimulationConfigModel simulationConfig)
        {
            SimulationResult simulationResult = _simulationApplication.Simulate(simulationConfig);

            return Ok(_mapper.Map<SimulationResult, SimulationResultModel>(simulationResult));
        }
    }
}
