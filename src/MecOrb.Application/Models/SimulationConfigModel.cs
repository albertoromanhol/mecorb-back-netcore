using System;
using System.Collections.Generic;

namespace MecOrb.Application.Models
{
    public class SimulationConfigModel
    {
        public List<PlanetModel> Planets { get; set; }
        public DateTime InitialDate { get; set; }
        public int SimulationDays { get; set; }
    }
}
