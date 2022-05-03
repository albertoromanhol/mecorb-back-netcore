using MecOrb.Domain.Core.Entities;
using System;
using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class SimulationConfig : Entity, IAggregateRoot
    {
        public List<Planet> Planets { get; set; }
        public DateTime InitialDate { get; set; }
        public int SimulationDays { get; set; }
        public double? SimulationInSeconds { get; set; }
        public int? SimulationSteps { get; set; }
    }
}
