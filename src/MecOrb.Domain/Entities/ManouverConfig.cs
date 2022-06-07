using MecOrb.Domain.Core.Entities;

namespace MecOrb.Domain.Entities
{
    public class ManouverConfig : Entity, IAggregateRoot
    {
        public Orbit InitialOrbit { get; set; }
        public Orbit FinalOrbit { get; set; }
        public double? FirstBiEllipseApogge { get; set; }
        public Spacecraft Spacecraft { get; set; }
    }
}
