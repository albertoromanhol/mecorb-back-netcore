using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class Vector3Map : Profile
    {
        public Vector3Map()
        {
            CreateMap<Vector3, Vector3Model>()
                .ReverseMap();
        }
    }
}
