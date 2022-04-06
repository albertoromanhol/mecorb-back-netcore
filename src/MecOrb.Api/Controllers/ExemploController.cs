using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace MecOrb.Api.Controllers
{
    [ApiController]
    [Route("exemplos")]
    public class ExemploController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IExemploApplication _exemploApplication;
        private readonly IExemploRepository _exemploRepository;

        public ExemploController(IMapper mapper, IExemploApplication exemploApplication, IExemploRepository exemploRepository)
        {
            _mapper = mapper;
            _exemploApplication = exemploApplication;
            _exemploRepository = exemploRepository;
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ExemploModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Get(Guid id)
        {
            var exemplo = _exemploRepository.ObterPorId(id);

            if (exemplo == null)
                return NotFound("Exemplo não encontrado");

            return Ok(_mapper.Map<Exemplo, ExemploModel>(exemplo));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ExemploModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult List()
        {
            var exemplo = _exemploRepository.ListarTodos();

            return Ok(_mapper.Map<IEnumerable<Exemplo>, IEnumerable<ExemploModel>>(exemplo));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ExemploModel), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult Post(ExemploModel exemploModel)
        {
            var result = _exemploApplication.Salvar(exemploModel);

            if (result.Success)
                return Created($"/exemplos/{result.Object.Id}", _mapper.Map<Exemplo, ExemploModel>(result.Object));

            return BadRequest(result.Notifications);
        }
    }
}
