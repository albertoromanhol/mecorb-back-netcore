using MecOrb.Domain.Core.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class SimulationResult : Entity, IAggregateRoot
    {
        public List<Planet> Planets { get; set; }
        public List<double> Time { get; set; }
        public List<string> Collision { get; set; }
        public int TrajectoryPoints { get; set; }
    }
}
