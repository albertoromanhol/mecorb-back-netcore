using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class SpacecraftMap : Profile
    {
        public SpacecraftMap()
        {
            CreateMap<Spacecraft, SpacecraftModel>()
                .ReverseMap();
        }
    }
}
