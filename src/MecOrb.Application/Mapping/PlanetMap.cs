using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class PlanetMap : Profile
    {
        public PlanetMap()
        {
            CreateMap<Planet, PlanetModel>()
                .ReverseMap();
        }
    }
}
