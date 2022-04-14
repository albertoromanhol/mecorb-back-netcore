using MecOrb.Application.Models;
using MecOrb.Domain.Entities;
using System.Collections.Generic;

namespace MecOrb.Application.Interfaces
{
    public interface ISimulationApplication
    {
        Simulation SimulateTwoBodies(SimulationConfigModel planetsToSimulate);
        List<Planet> GetPlanetsAcceleration();
    }
}