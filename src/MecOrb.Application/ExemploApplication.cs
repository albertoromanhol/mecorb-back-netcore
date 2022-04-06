using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using MecOrb.Domain.Repositories;

namespace MecOrb.Application
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
