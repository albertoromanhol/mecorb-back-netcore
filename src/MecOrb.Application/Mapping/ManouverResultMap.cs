using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class ManouverResultMap : Profile
    {
        public ManouverResultMap()
        {
            CreateMap<ManouverResult, ManouverResultModel>()
                .ReverseMap();
        }
    }
}
