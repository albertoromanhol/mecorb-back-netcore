using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class OrbitMap : Profile
    {
        public OrbitMap()
        {
            CreateMap<Orbit, OrbitModel>()
                .ReverseMap();
        }
    }
}
