using ToolGood.Algorithm.MathNet.Numerics.Distributions;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal static partial class ExcelFunctions
    {
        public static double GAMMALN(double z)
        {
            return SpecialFunctions.GammaLn(z);
        }


        public static double HypgeomDist(int k, int draws, int success, int population)
        {
            return Hypergeometric.PMF(population, success, draws, k);
        }

        public static double NegbinomDist(int k, double r, double p)
        {
            return NegativeBinomial.PMF(r, p, k);
        }

        public static double LognormDist(double x, double mu, double sigma)
        {
            return LogNormal.CDF(mu, sigma, x);
        }

        public static double LogInv(double p, double mu, double sigma)
        {
            return LogNormal.InvCDF(mu, sigma, p);
        }

        public static double BinomDist(int k, int n, double p, bool state)
        {
            if (state == false) {
                return Binomial.PMF(p, n, k);
            }
            return Binomial.CDF(p, n, k);
        }

        public static double Poisson(int k, double lambda, bool state)
        {
            if (state == false) {
                return Distributions.Poisson.PMF(lambda, k);
            }
            return Distributions.Poisson.CDF(lambda, k);
        }
    }
}