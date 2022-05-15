using System.Collections.Generic;

namespace MecOrb.Application.Models
{
    public class ManouverResultModel : SimulationResultModel
    {
        public Dictionary<string, double> DeltaV { get; set; }

    }
}
