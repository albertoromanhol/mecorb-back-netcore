using MecOrb.Domain.Core.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class Planet : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string NamePTBR { get; set; }
        public int NasaHorizonBodyId { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }
        public Dictionary<string, VectorXYZ> Ephemerities { get; set; }
    }
}
