using System.Collections.Generic;

namespace MecOrb.Application.Models
{
    public class ManouverResultModel
    {
        public double FirstDeltaV { get; set; }
        public double SecondDeltaV { get; set; }
        public double TotalDeltaV { get; set; }
        public List<PlanetModel> Planets { get; set; }

    }
}
