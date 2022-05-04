using MecOrb.Domain.Core.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class ManouverResult : Entity, IAggregateRoot
    {
        public Dictionary<string, double> DeltaV { get; set; }
        public List<Planet> Planets { get; set; }
    }
}
