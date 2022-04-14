using System;

namespace MecOrb.Domain.Entities
{
    public class VectorXYZ
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public VectorXYZ()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public VectorXYZ(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static VectorXYZ operator +(VectorXYZ v1, VectorXYZ v2)
            => new VectorXYZ
            {
                X = v1.X + v2.X,
                Y = v1.Y + v2.Y,
                Z = v1.Z + v2.Z,
            };

        public static VectorXYZ operator -(VectorXYZ v1, VectorXYZ v2)
            => new VectorXYZ
            {
                X = v1.X - v2.X,
                Y = v1.Y - v2.Y,
                Z = v1.Z - v2.Z,
            };

        public static VectorXYZ operator *(VectorXYZ v1, double value)
            => new VectorXYZ
            {
                X = v1.X * value,
                Y = v1.Y * value,
                Z = v1.Z * value,
            };

        public static VectorXYZ operator /(VectorXYZ v1, double value)
            => new VectorXYZ
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
