using AutoMapper;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;
using MecOrb.Infrastructure.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace MecOrb.Infrastructure.Repositories
{
    public class PlanetRepository : IPlanetRepository
    {
        private readonly IMapper _mapper;

        public PlanetRepository(IMapper mapper)
        {
            _mapper = mapper;
        }

        public List<Planet> GetAll()
        {
            List<PlanetDbModel> planets = GetPlanets();

            return _mapper.Map<List<PlanetDbModel>, List<Planet>>(planets);
        }

        public Planet GetByNasaBodyId(int bodyId)
        {
            PlanetDbModel planet = GetPlanets().Where(p => p.NasaHorizonBodyId == bodyId).FirstOrDefault();

            return _mapper.Map<PlanetDbModel, Planet>(planet);
        }

        // TODO: create a new file, thats return the planets
        private List<PlanetDbModel> GetPlanets()
        {
            PlanetDbModel sun = new PlanetDbModel(0, "Sun", "Sol", 0, 1.989e30, 696_500);
            PlanetDbModel mercury = new PlanetDbModel(1, "Mercury", "Mercúrio", 199, 330.2e21, 2_440);
            PlanetDbModel venus = new PlanetDbModel(2, "Venus", "Vênus", 299, 4.869e24, 6_052);
            PlanetDbModel earth = new PlanetDbModel(3, "Earth", "Terra", 399, 5.97219e24, 6_378);
            PlanetDbModel moon = new PlanetDbModel(9, "Moon", "Lua", 301, 73.48e21, 1_737);
            PlanetDbModel mars = new PlanetDbModel(4, "Mars", "Marte", 499, 641.9e21, 3_396);
            PlanetDbModel jupiter = new PlanetDbModel(5, "Jupiter", "Júpiter", 599, 1.899e27, 71_490);
            PlanetDbModel saturn = new PlanetDbModel(6, "Saturn", "Saturno", 699, 568.5e24, 60_270);
            PlanetDbModel uranus = new PlanetDbModel(7, "Uranus", "Urano", 799, 86.83e24, 25_560);
            PlanetDbModel neptune = new PlanetDbModel(8, "Neptune", "Netuno", 899, 102.4e24, 24_760);

            List<PlanetDbModel> planets = new List<PlanetDbModel>()
            {
                sun,
                mercury,
                venus,
                earth,
                mars,
                jupiter,
                saturn,
                uranus,
                neptune,
                //moon
            };

            return planets;

        }
    }
}