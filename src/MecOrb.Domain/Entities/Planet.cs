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
        public Vector3 CurrentPosition { get; set; }
        public Vector3 CurrentVelocity { get; set; }
        public Vector3 CurrentAcceleration { get; set; }
        public Vector3 BasePosition { get; set; }
        public Vector3 BaseVelocity { get; set; }
        public Trajectory Trajectory { get; set; }
        public Dictionary<string, Vector3> Ephemerities { get; set; }


        public void StartTrajectory()
        {
            Trajectory = new Trajectory();
            Trajectory.AddVector(CurrentPosition);
        }
    }
}
