using MecOrb.Domain.Entities;

namespace MecOrb.Application.Utils
{
    public static class Methods
    {
        public static VectorXYZ VectorialProduct(VectorXYZ v1, VectorXYZ v2)
            => new VectorXYZ
            {
                X = v1.Y * v2.Z - v1.Z * v2.Y,
                Y = v1.Z * v2.X - v1.X * v2.Z,
                Z = v1.X * v2.Y - v1.Y * v2.X,
            };
        public static double DotProduct(VectorXYZ v1, VectorXYZ v2)
            => (v1.X * v2.X) + (v1.Y * v2.Y) + (v1.Z * v2.Z);

        //public static double CalculateSpecificMechanicalEnergy(BodyDTO targetBody, BodyDTO referenceBody)
        //    => 0.5 * Math.Pow((targetBody.CurrentVelocity - referenceBody.CurrentVelocity).Norm(), 2) -
        //            (Constants.G * 1e-9) * referenceBody.Mass / (targetBody.CurrentPosition - referenceBody.CurrentPosition).Norm();

        //public static List<double> ReduceArray(List<double> inputArray, int reductionSize)
        //{
        //    List<double> finalArray = new List<double>();

        //    foreach (var (item, index) in inputArray.Select((item, index) => (item, index)))
        //    {
        //        if (index % reductionSize == 0)
        //        {
        //            finalArray.Add(item);
        //        }
        //    }

        //    return finalArray;
        //}
    }
}
