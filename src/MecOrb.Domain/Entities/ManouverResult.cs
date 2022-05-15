using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class ManouverResult : SimulationResult
    {
        public Dictionary<string, double> DeltaV { get; set; }
    }
}
