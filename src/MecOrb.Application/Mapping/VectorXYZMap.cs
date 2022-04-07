using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class VectorXYZMap : Profile
    {
        public VectorXYZMap()
        {
            CreateMap<VectorXYZ, VectorXYZModel>()
                .ReverseMap();
        }
    }
}
