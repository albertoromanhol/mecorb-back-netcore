using MecOrb.Domain.Entities;
using System.Collections.Generic;

namespace MecOrb.Application.Interfaces
{
    public interface IPlanetApplication
    {
        List<Planet> GetAll();
        List<Planet> GetAllWithEphemerits();
    }
}