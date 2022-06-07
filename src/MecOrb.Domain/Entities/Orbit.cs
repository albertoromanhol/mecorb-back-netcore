using MecOrb.Domain.Core.Entities;

namespace MecOrb.Domain.Entities
{
    public class Orbit : Entity, IAggregateRoot
    {
        public double Excentricity { get; set; }
        public double MajorSemiAxis { get; set; }
        public double PerigeeRadius { get; set; }
        public double ApogeeRadius { get; set; }
        public double AngularMoment { get; set; }
        public double PerigeeVelocity { get; set; }
        public double ApogeeVelocity { get; set; }
        public double PeriodInSeconds { get; set; }
        public string Name { get; set; }
        public Planet EquivalentPlanet { get; set; }
    }
}
