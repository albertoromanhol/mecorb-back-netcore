using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Interfaces
{
    public interface IExemploApplication
    {
        Result<Exemplo> Salvar(ExemploModel exemploModel);
    }
}