using ToolGood.Algorithm.MathNet.Numerics.RootFinding;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    internal sealed class StudentT
    {
        public static decimal CDF(decimal location, decimal scale, int freedom, decimal x)
        {
            var k = (x - location) / scale;
            var h = freedom / (freedom + (k * k));
            var ib = 0.5m * SpecialFunctions.BetaRegularized(freedom / 2.0m, 0.5m, h);
            return x <= location ? ib : 1.0m - ib;
        }

        public static decimal InvCDF(decimal location, decimal scale, int freedom, decimal p)
        {
            if (p == 0.5m) {
                return location;
            }

            return Brent.FindRoot(x => {
                var k = (x - location) / scale;
                var h = freedom / (freedom + (k * k));
                var ib = 0.5m * SpecialFunctions.BetaRegularized(freedom / 2.0m, 0.5m, h);
                return x <= location ? ib - p : 1.0m - ib - p;
            }, -800, 800, accuracy: 1e-12m);
        }
    }
}
