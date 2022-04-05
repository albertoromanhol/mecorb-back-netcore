using AutoMapper;
using Analytics.MaquinaCW.Domain.Entities;
using Analytics.MaquinaCW.Infrastructure.DbModels;

namespace Analytics.MaquinaCW.Infrastructure.Mapping
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
