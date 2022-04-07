using AutoMapper;
using MecOrb.Application.Interfaces;
using MecOrb.Application.Models;
using MecOrb.Domain.Entities;

namespace MecOrb.Application
{
    public class SimulationApplication : ISimulationApplication
    {
        private readonly IMapper _mapper;

        public SimulationApplication(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Simulation SimulateTwoBodies(SimulationConfigModel simulationConfigModel)
        {
            Simulation simulation = new Simulation();

            SimulationConfig simulationConfig = _mapper.Map<SimulationConfigModel, SimulationConfig>(simulationConfigModel);

            return simulation;
        }
    }
}
