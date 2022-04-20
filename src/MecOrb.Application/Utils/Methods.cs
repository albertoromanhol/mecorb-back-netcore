using MecOrb.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MecOrb.Application.Utils
{
    public static class Methods
    {
        public static Vector3 VectorialProduct(Vector3 v1, Vector3 v2)
            => new Vector3
            {
                X = v1.Y * v2.Z - v1.Z * v2.Y,
                Y = v1.Z * v2.X - v1.X * v2.Z,
                Z = v1.X * v2.Y - v1.Y * v2.X,
            };
        public static double DotProduct(Vector3 v1, Vector3 v2)
            => (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);

        public static List<double> ReduceArray(List<double> inputArray, int reductionSize)
        {
            List<double> finalArray = new List<double>();

            foreach (var (item, index) in inputArray.Select((item, index) => (item, index)))
            {
                if (index % reductionSize == 0)
                {
                    finalArray.Add(item);
                }
            }

            return finalArray;
        }
    }
}
