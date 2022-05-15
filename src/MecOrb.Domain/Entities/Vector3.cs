using System;

namespace MecOrb.Domain.Entities
{
    public class Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
            => new Vector3
            {
                X = v1.X + v2.X,
                Y = v1.Y + v2.Y,
                Z = v1.Z + v2.Z,
            };

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
            => new Vector3
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y,
                Z = v1.Z - v2.Z,
            };

        public static Vector3 operator *(Vector3 v1, double value)
            => new Vector3
            {
                X = v1.X * value,
                Y = v1.Y * value,
                Z = v1.Z * value,
            };

        public static Vector3 operator /(Vector3 v1, double value)
            => new Vector3
            {
                X = v1.X / value,
                Y = v1.Y / value,
                Z = v1.Z / value,
            };

        public double Norm() => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
    }
}
