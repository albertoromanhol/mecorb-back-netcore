using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using System;
using System.Collections.Generic;
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

        public async Task<List<Planet>> GetAllWithEphemerits()
        {
            List<Planet> planets = _planetRepository.GetAll();

            await GetPlanetsEphemerities(planets);

            return planets;
        }

        public Planet GetPlanetWithEphemerits(int bodyId)
        {
            Planet planet = _planetRepository.GetByNasaBodyId(bodyId);

            planet.Ephemerities = GetEphemeritiesByBodyId(planet.NasaHorizonBodyId).Result;

            return planet;
        }

        private async Task GetPlanetsEphemerities(List<Planet> planets)
        {
            foreach (var planet in planets)
            {
                planet.Ephemerities = await GetEphemeritiesByBodyId(planet.NasaHorizonBodyId);
            }
        }

        private async Task<Dictionary<string, VectorXYZ>> GetEphemeritiesByBodyId(int bodyId)
        {
            Dictionary<string, VectorXYZ> ephemerities = new Dictionary<string, VectorXYZ>();

            try
            {
                ephemerities = await _nasaHorizonRepository.GetEphemerities(bodyId);
            }
            catch (Exception e)
            {
                Console.Write(e.Message + Environment.NewLine + e.StackTrace);
            }

            return ephemerities;
        }
    }
}
