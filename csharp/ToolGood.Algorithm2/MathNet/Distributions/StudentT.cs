using ToolGood.Algorithm.MathNet.Numerics.RootFinding;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    class StudentT
    {
        public static double CDF(double location, double scale, double freedom, double x)
        {
            //if (scale <= 0.0 || freedom <= 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            // TODO JVG we can probably do a better job for Cauchy special case
            if (double.IsPositiveInfinity(freedom)) {
                return Normal.CDF(location, scale, x);
            }

            var k = (x - location) / scale;
            var h = freedom / (freedom + (k * k));
            var ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2.0, 0.5, h);
            return x <= location ? ib : 1.0 - ib;
        }
        public static double InvCDF(double location, double scale, double freedom, double p)
        {
            //if (scale <= 0.0 || freedom <= 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            // TODO JVG we can probably do a better job for Cauchy special case
            if (double.IsPositiveInfinity(freedom)) {
                return Normal.InvCDF(location, scale, p);
            }

            if (p == 0.5d) {
                return location;
            }

            // TODO PERF: We must implement this explicitly instead of solving for CDF^-1
            return Brent.FindRoot(x => {
                var k = (x - location) / scale;
                var h = freedom / (freedom + (k * k));
                var ib = 0.5 * SpecialFunctions.BetaRegularized(freedom / 2.0, 0.5, h);
                return x <= location ? ib - p : 1.0 - ib - p;
            }, -800, 800, accuracy: 1e-12);
        }


    }
}
