using MecOrb.Domain.Core.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class ManouverResult : Entity, IAggregateRoot
    {
        public double FirstDeltaV { get; set; }
        public double SecondDeltaV { get; set; }
        public double TotalDeltaV { get; set; }
        public List<Planet> Planets { get; set; }
    }
}
