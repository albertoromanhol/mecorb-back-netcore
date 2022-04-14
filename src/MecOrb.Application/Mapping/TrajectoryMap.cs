using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class TrajectoryMap : Profile
    {
        public TrajectoryMap()
        {
            CreateMap<Trajectory, TrajectoryModel>()
                .ReverseMap();
        }
    }
}
