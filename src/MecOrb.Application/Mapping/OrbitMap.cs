using AutoMapper;
using MecOrb.Application.Models;

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
