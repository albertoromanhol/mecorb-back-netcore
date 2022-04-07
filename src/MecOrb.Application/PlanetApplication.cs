using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MecOrb.Application
{
    public class PlanetApplication : IPlanetApplication
    {
        private readonly IMapper _mapper;
        private readonly IPlanetRepository _planetRepository;
        private readonly INasaHorizonRepository _nasaHorizonRepository;

        public PlanetApplication(IMapper mapper, IPlanetRepository planetRepository, INasaHorizonRepository nasaHorizonRepository)
        {
            _mapper = mapper;
            _planetRepository = planetRepository;
            _nasaHorizonRepository = nasaHorizonRepository;
        }

        public List<Planet> GetAll()
        {
            List<Planet> planets = _planetRepository.GetAll();

            return planets;
        }

        public List<Planet> GetAllWithEphemerits()
        {
            List<Planet> planets = _planetRepository.GetAll();

            // HOW CAN DO THAT ASYNCHRONOUS, ALL PLANETS 
            var tasks = planets.Select(async planet =>
            {
                planet.Ephemerities = await _nasaHorizonRepository.GetEphemerities(planet.NasaHorizonBodyId);
            });

            Task.WhenAll(tasks);

            return planets;
        }

    }
}
