using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Interfaces
{
    public interface ISimulationApplication
    {
        Simulation SimulateTwoBodies(SimulationConfigModel planetsToSimulate);
    }
}