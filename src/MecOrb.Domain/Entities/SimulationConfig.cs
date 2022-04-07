using MecOrb.Domain.Core.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class SimulationConfig : Entity, IAggregateRoot
    {
        public List<Planet> Planets { get; set; }
    }
}
