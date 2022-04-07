using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MecOrb.Api.Controllers
{
    [ApiController]
    [Route("planets")]
    public class PlanetController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IPlanetApplication _planetApplication;

        public PlanetController(IMapper mapper, IPlanetApplication planetApplication)
        {
            _mapper = mapper;
            _planetApplication = planetApplication;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<PlanetModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult ListAll()
        {
            List<Planet> planets = _planetApplication.GetAll();

            return Ok(_mapper.Map<List<Planet>, List<PlanetModel>>(planets));
        }

        [HttpGet]
        [Route("ephemerites")]
        [ProducesResponseType(typeof(List<PlanetModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListAllWithEphemerities()
        {
            List<Planet> planets = await _planetApplication.GetAllWithEphemerits();

            return Ok(_mapper.Map<List<Planet>, List<PlanetModel>>(planets));
        }


        [HttpGet]
        [Route("ephemerites/{bodyId}")]
        [ProducesResponseType(typeof(PlanetModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult ListPlanetWithEphemerities([FromRoute] int bodyId)
        {
            Planet planets = _planetApplication.GetPlanetWithEphemerits(bodyId);

            return Ok(_mapper.Map<Planet, PlanetModel>(planets));
        }
    }
}
