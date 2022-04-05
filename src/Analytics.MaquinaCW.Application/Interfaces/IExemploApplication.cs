using Analytics.MaquinaCW.Application.Models;
using Analytics.MaquinaCW.Domain.Entities;

namespace Analytics.MaquinaCW.Application.Interfaces
{
    public interface IExemploApplication
    {
        Result<Exemplo> Salvar(ExemploModel exemploModel);
    }
}