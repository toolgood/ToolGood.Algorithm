using System;
using ToolGood.Algorithm.MathNet.Numerics.Distributions;
using ToolGood.Algorithm.MathNet.Numerics.Statistics;



namespace ToolGood.Algorithm.MathNet.Numerics
{
    public static partial class ExcelFunctions
    {
        public static double GAMMALN(double z)
        {
            return SpecialFunctions.GammaLn(z);
        }

        //public static double ChiDist(double x, double freedom)
        //{
        //    return Chi.PDF(x, freedom);//Is Error
        //}

        public static double ExponDist(double x, double rate, bool state)
        {
            if (state) {
                return Exponential.CDF(rate, x);
            }
            return Exponential.PDF(rate, x);

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

        public static double POISSON(int k, double lambda, bool state)
        {
            if (state == false) {
                return Poisson.PMF(lambda, k);
            }
            return Poisson.CDF(lambda, k);
        }
        public static double WEIBULL(double x, double shape, double scale,  bool state)
        {
            if (state == false) {
                return Weibull.PDF(shape, scale, x);
            }
            return Weibull.CDF(shape, scale, x);
        }

       
    }
}
