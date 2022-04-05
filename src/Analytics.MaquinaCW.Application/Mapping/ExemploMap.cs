using AutoMapper;
using Analytics.MaquinaCW.Application.Models;
using Analytics.MaquinaCW.Domain.Entities;
using Analytics.MaquinaCW.Domain.ValueObjects;

namespace Analytics.MaquinaCW.Application.Mapping
{
    public class ExemploMap : Profile
    {
        public ExemploMap()
        {
            CreateMap<Exemplo, ExemploModel>()
                .ForMember(dest => dest.Nome, m => m.MapFrom(src => src.Nome.PrimeiroNome))
                .ForMember(dest => dest.Sobrenome, m => m.MapFrom(src => src.Nome.Sobrenome))
                .ForMember(dest => dest.Cpf, m => m.MapFrom(src => src.Cpf.ToString()))
                .ForMember(dest => dest.Ddd, m => m.MapFrom(src => src.Telefone.Ddd))
                .ForMember(dest => dest.Telefone, m => m.MapFrom(src => src.Telefone.Numero))
                .ForMember(dest => dest.Email, m => m.MapFrom(src => src.Email.ToString()));

            CreateMap<ExemploModel, Exemplo>()
                .ForMember(dest => dest.Nome, m => m.Ignore())
                .ForMember(dest => dest.Cpf, m => m.Ignore())
                .ForMember(dest => dest.Email, m => m.Ignore())
                .ForMember(dest => dest.Telefone, m => m.MapFrom(src => src.Ddd.HasValue ? new Telefone(src.Ddd.Value, src.Telefone) : null))
                .ConstructUsing(src =>
                    new Exemplo(
                        new Nome(src.Nome, src.Sobrenome),
                        new CPF(src.Cpf),
                        new Email(src.Email),
                        src.Segmento)
                    );
        }
    }
}
