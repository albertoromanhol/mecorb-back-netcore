using MecOrb.Domain.Core.Entities;
using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class Planet : Entity, IAggregateRoot
    {
        public string Name { get; set; }
        public string NamePTBR { get; set; }
        public int NasaHorizonBodyId { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }
        public string ReferenceBodyId { get; set; }
        public VectorXYZ CurrentPosition { get; set; }
        public VectorXYZ CurrentVelocity { get; set; }
        public VectorXYZ CurrentAcceleration { get; set; }
        public VectorXYZ BasePosition { get; set; }
        public VectorXYZ BaseVelocity { get; set; }
        public Trajectory BodyTrajectory { get; set; }
        public Dictionary<string, VectorXYZ> Ephemerities { get; set; }


        public void StartTrajectory()
        {
            BodyTrajectory = new Trajectory();
            BodyTrajectory.AddVector(CurrentPosition);
        }
    }
}
