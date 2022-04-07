using MecOrb.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MecOrb.Application.Interfaces
{
    public interface IPlanetApplication
    {
        List<Planet> GetAll();
        Task<List<Planet>> GetAllWithEphemerits();
        Planet GetPlanetWithEphemerits(int bodyId);
    }
}