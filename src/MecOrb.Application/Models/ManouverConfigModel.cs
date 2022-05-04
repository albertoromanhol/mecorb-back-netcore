namespace MecOrb.Application.Models
{
    public class ManouverConfigModel
    {
        public OrbitModel InitialOrbit { get; set; }
        public OrbitModel FinalOrbit { get; set; }
        public double? FirstBiEllipseApogge { get; set; }
    }
}
