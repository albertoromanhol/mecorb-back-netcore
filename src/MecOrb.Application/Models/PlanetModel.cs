using System.Collections.Generic;

namespace MecOrb.Application.Models
{
    public class PlanetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NamePTBR { get; set; }
        public int NasaHorizonBodyId { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }
        public Dictionary<string, VectorXYZModel> Ephemerities { get; set; }

    }
}
