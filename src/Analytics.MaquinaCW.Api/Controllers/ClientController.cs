﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Analytics.MaquinaCW.Application.Interfaces;
using Analytics.MaquinaCW.Application.Models;
using Analytics.MaquinaCW.Domain.Entities;
using Analytics.MaquinaCW.Domain.Repositories;
using System;
using System.Collections.Generic;

namespace Analytics.MaquinaCW.Api.Controllers
{
    [ApiController]
    [Route("users/{id}/clients")]
    public class ClientController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepository;

        public ClientController(IMapper mapper, IClientRepository clientRepository)
        {
            _mapper = mapper;
            _clientRepository = clientRepository;
        }

     
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ClientModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
        public IActionResult List([FromRoute] int id)
        {
            var client = _clientRepository.ListAllByUser(id);

            return Ok(_mapper.Map<IEnumerable<Client>, IEnumerable<ClientModel>>(client));
        }        
    }
}
