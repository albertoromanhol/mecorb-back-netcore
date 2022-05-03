using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class ManouverConfigMap : Profile
    {
        public ManouverConfigMap()
        {
            CreateMap<ManouverConfig, ManouverConfigModel>()
                .ReverseMap();
        }
    }
}
