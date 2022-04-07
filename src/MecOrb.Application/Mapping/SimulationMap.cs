using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class SimulationMap : Profile
    {
        public SimulationMap()
        {
            CreateMap<Simulation, SimulationModel>()
                .ReverseMap();
        }
    }
}
