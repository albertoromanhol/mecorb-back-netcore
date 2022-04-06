using AutoMapper;
using MecOrb.Domain.Entities;
using MecOrb.Infrastructure.DbModels;

namespace MecOrb.Infrastructure.Mapping
{
    public class ClientMap : Profile
    {
        public ClientMap()
        {
            CreateMap<ClientDbModel, Client>()
                .ReverseMap();
        }
    }
}
