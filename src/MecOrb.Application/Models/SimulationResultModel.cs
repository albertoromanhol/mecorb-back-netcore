using System.Collections.Generic;

namespace MecOrb.Application.Models
{
    public class SimulationResultModel
    {
        public List<PlanetModel> Planets { get; set; }
        public List<double> Time { get; set; }
        public List<string> Collision { get; set; }
        public int TrajectoryPoints { get; set; }

    }
}
