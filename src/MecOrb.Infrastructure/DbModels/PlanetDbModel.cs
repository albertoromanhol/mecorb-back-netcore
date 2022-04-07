
namespace MecOrb.Infrastructure.DbModels
{
    public class PlanetDbModel
    {
        public PlanetDbModel(int id, string name, string namePTBR,
            int nasaHorizonBodyId, double mass, double radius)
        {
            Id = id;
            Name = name;
            NamePTBR = namePTBR;
            NasaHorizonBodyId = nasaHorizonBodyId;
            Mass = mass;
            Radius = radius;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string NamePTBR { get; private set; }
        public int NasaHorizonBodyId { get; private set; }
        public double Mass { get; private set; }
        public double Radius { get; private set; }
    }
}
