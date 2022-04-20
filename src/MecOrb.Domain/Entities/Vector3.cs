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
        //public static VectorXYZ operator *(Matrix matrix, VectorXYZ v1)
        //    => new VectorXYZ
        //    {
        //        X = Methods.dotProduct(matrix.RowX(), v1),
        //        Y = Methods.dotProduct(matrix.RowY(), v1),
        //        Z = Methods.dotProduct(matrix.RowZ(), v1),
        //    };
        public double Norm() => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
    }
}
