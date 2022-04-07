using MecOrb.Domain.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Repositories
{
    public interface IPlanetRepository
    {
        List<Planet> GetAll();
    }
}
