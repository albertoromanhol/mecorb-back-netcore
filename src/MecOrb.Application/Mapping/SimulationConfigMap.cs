using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class SimulationConfigMap : Profile
    {
        public SimulationConfigMap()
        {
            CreateMap<SimulationConfig, SimulationConfigModel>()
                .ReverseMap();
        }
    }
}
