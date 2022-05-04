using System.Collections.Generic;

namespace MecOrb.Application.Models
{
    public class ManouverResultModel
    {
        public Dictionary<string, double> DeltaV { get; set; }
        public List<PlanetModel> Planets { get; set; }

    }
}
