using AutoMapper;
using Analytics.MaquinaCW.Domain.Entities;
using Analytics.MaquinaCW.Domain.ValueObjects;
using Analytics.MaquinaCW.Infrastructure.DbModels;

namespace Analytics.MaquinaCW.Infrastructure.Mapping
{
    public class ExemploMap : Profile
    {
        public ExemploMap()
        {
            CreateMap<ExemploDbModel, Exemplo>()
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
