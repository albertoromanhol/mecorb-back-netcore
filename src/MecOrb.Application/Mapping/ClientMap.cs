using AutoMapper;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Mapping
{
    public class ClientMap : Profile
    {
        public ClientMap()
        {
            CreateMap<Client, ClientModel>()
                .ReverseMap();                           
        }
    }
}
