using AutoMapper;
using Analytics.MaquinaCW.Application.Interfaces;
using Analytics.MaquinaCW.Application.Models;
using Analytics.MaquinaCW.Domain.Entities;
using Analytics.MaquinaCW.Domain.Repositories;

namespace Analytics.MaquinaCW.Application
{
    public class ExemploApplication : IExemploApplication
    {
        private readonly IMapper _mapper;
        private readonly IExemploRepository _exemploRepository;

        public ExemploApplication(IMapper mapper, IExemploRepository exemploRepository)
        {
            _mapper = mapper;
            _exemploRepository = exemploRepository;
        }

        public Result<Exemplo> Salvar(ExemploModel exemploModel)
        {
            var exemplo = _mapper.Map<ExemploModel, Exemplo>(exemploModel);

            if (exemplo.Valid)
            {
                _exemploRepository.Incluir(exemplo);
                return Result<Exemplo>.Ok(exemplo);
            }

            return Result<Exemplo>.Error(exemplo.Notifications);
        }
    }
}
