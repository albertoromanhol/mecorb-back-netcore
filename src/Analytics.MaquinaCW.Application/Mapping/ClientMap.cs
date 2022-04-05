using AutoMapper;
using Analytics.MaquinaCW.Application.Models;
using Analytics.MaquinaCW.Domain.Entities;

namespace Analytics.MaquinaCW.Application.Mapping
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
