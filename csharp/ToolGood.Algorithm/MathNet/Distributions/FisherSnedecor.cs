using ToolGood.Algorithm.MathNet.Numerics.RootFinding;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    internal sealed class FisherSnedecor
    {
        public static decimal CDF(int d1, int d2, decimal x)
        {
            return SpecialFunctions.BetaRegularized(d1 / 2.0m, d2 / 2.0m, d1 * x / (d1 * x + d2));
        }

        public static decimal InvCDF(int d1, int d2, decimal p)
        {
            return Brent.FindRoot(
                x => SpecialFunctions.BetaRegularized(d1 / 2.0m, d2 / 2.0m, d1 * x / (d1 * x + d2)) - p,
                0, 1000, accuracy: 1e-12m);
        }
    }
}
