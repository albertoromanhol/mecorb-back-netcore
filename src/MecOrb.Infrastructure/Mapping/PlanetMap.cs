using AutoMapper;
using MecOrb.Domain.Entities;
using MecOrb.Infrastructure.DbModels;

namespace MecOrb.Infrastructure.Mapping
{
    public class PlanetMap : Profile
    {
        public PlanetMap()
        {
            CreateMap<PlanetDbModel, Planet>()
                .ReverseMap();
        }
    }
}
