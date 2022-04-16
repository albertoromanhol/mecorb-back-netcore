using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class SimulationResultMap : Profile
    {
        public SimulationResultMap()
        {
            CreateMap<SimulationResult, SimulationResultModel>()
                .ReverseMap();
        }
    }
}
