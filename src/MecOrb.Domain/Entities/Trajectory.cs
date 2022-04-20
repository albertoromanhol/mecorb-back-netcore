using System.Collections.Generic;

namespace MecOrb.Domain.Entities
{
    public class Trajectory
    {
        public Trajectory()
        {
            this.X = new List<double>();
            this.Y = new List<double>();
            this.Z = new List<double>();
        }
        public List<double> X { get; set; }
        public List<double> Y { get; set; }
        public List<double> Z { get; set; }

        public void AddVector(Vector3 vector)
        {
            this.X.Add(vector.X);
            this.Y.Add(vector.Y);
            this.Z.Add(vector.Z);
        }
    }
}
