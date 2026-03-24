using ToolGood.Algorithm.MathNet.Numerics.RootFinding;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    internal sealed class Beta
    {
        public static decimal CDF(decimal a, decimal b, decimal x)
        {
            if (x < 0.0m) {
                return 0.0m;
            }

            if (x >= 1.0m) {
                return 1.0m;
            }

            if (a == 0.0m && b == 0.0m) {
                if (x >= 0.0m && x < 1.0m) {
                    return 0.5m;
                }

                return 1.0m;
            }

            if (a == 0.0m) {
                return 1.0m;
            }

            if (b == 0.0m) {
                return x >= 1.0m ? 1.0m : 0.0m;
            }

            if (a == 1.0m && b == 1.0m) {
                return x;
            }

            return SpecialFunctions.BetaRegularized(a, b, x);
        }

        public static decimal InvCDF(decimal a, decimal b, decimal p)
        {
            return Brent.FindRoot(x => SpecialFunctions.BetaRegularized(a, b, x) - p, 0.0m, 1.0m, accuracy: 1e-12m);
        }
    }
}
