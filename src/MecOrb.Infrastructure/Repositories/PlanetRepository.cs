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
            PlanetDbModel sun = new PlanetDbModel(0, "Sun", "Sol", 0, 1988500e24, 695700);
            PlanetDbModel mercury = new PlanetDbModel(1, "Mercury", "Mercúrio", 199, 302e23, 2440);
            PlanetDbModel venus = new PlanetDbModel(2, "Venus", "Vênus", 299, 48.685e23, 6051.84);
            PlanetDbModel moon = new PlanetDbModel(9, "Moon", "Lua", 301, 7.349e22, 1737.53);
            PlanetDbModel earth = new PlanetDbModel(3, "Earth", "Terra", 399, 5.97219e24, 6371.01);
            PlanetDbModel mars = new PlanetDbModel(4, "Mars", "Marte", 499, 6.4171e23, 3389.92);
            PlanetDbModel jupiter = new PlanetDbModel(5, "Jupiter", "Júpiter", 599, 1898.13e24, 69911);
            PlanetDbModel saturn = new PlanetDbModel(6, "Saturn", "Saturno", 699, 5.6835e26, 58232);
            PlanetDbModel uranus = new PlanetDbModel(7, "Uranus", "Urano", 799, 86.813e24, 25362);
            PlanetDbModel neptune = new PlanetDbModel(8, "Neptune", "Netuno", 899, 1.0243e26, 24624);

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