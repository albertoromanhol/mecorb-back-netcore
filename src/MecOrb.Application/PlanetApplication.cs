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

        public async Task<List<Planet>> GetEphemerits(List<Planet> planets, DateTime initialDate)
        {
            await GetPlanetsEphemerities(planets);

            return planets;
        }
        public async Task<List<Planet>> GetAllWithEphemerits()
        {
            List<Planet> planets = _planetRepository.GetAll();

            await GetPlanetsEphemerities(planets);

            return planets;
        }
        public async Task<List<Planet>> GetSunAndEarthWithEphemerits()
        {
            List<Planet> planets = new List<Planet>();

            planets.Add(_planetRepository.GetByNasaBodyId(399));
            planets.Add(_planetRepository.GetByNasaBodyId(0));

            await GetPlanetsEphemerities(planets);

            return planets;
        }

        public Planet GetPlanetWithEphemerits(int bodyId)
        {
            Planet planet = _planetRepository.GetByNasaBodyId(bodyId);

            planet.Ephemerities = GetEphemeritiesByBodyId(planet.NasaHorizonBodyId).Result;

            return planet;
        }

        private async Task GetPlanetsEphemerities(List<Planet> planets, DateTime? initialDate = null)
        {
            foreach (var planet in planets)
            {
                planet.Ephemerities = await GetEphemeritiesByBodyId(planet.NasaHorizonBodyId, initialDate);
            }
        }

        private async Task<Dictionary<string, Vector3>> GetEphemeritiesByBodyId(int bodyId, DateTime? initialDate = null)
        {
            Dictionary<string, Vector3> ephemerities = new Dictionary<string, Vector3>();

            try
            {
                ephemerities = await _nasaHorizonRepository.GetEphemerities(bodyId, initialDate);
            }
            catch (Exception e)
            {
                Console.Write(e.Message + Environment.NewLine + e.StackTrace);
            }

            return ephemerities;
        }

    }
}
