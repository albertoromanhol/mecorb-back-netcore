using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application.Interfaces
{
    public interface ISimulationApplication
    {
        SimulationResult Simulate(SimulationConfigModel simulationConfigModel);
        SimulationResult SimulateForManouver(SimulationConfig simulationConfig);
    }
}