using System;
using System.Collections.Generic;
using ToolGood.Algorithm.MathNet.Numerics.Distributions;
using ToolGood.Algorithm.MathNet.Numerics.Statistics;

namespace ToolGood.Algorithm.MathNet.Numerics
{
	internal static partial class ExcelFunctions
	{
		public static decimal NormSDist(decimal z)
		{
			return Normal.CDF(0m, 1m, z);
		}

		public static decimal NormSInv(decimal probability)
		{
			return Normal.InvCDF(0m, 1m, probability);
		}

		public static decimal NormDist(decimal x, decimal mean, decimal standardDev, bool cumulative)
		{
			return cumulative ? Normal.CDF(mean, standardDev, x) : Normal.PDF(mean, standardDev, x);
		}

		public static decimal NormInv(decimal probability, decimal mean, decimal standardDev)
		{
			return Normal.InvCDF(mean, standardDev, probability);
		}

		public static decimal TDist(decimal x, int degreesFreedom, int tails)
		{
			switch(tails) {
				case 1:
					return 1m - StudentT.CDF(0m, 1m, degreesFreedom, x);

				case 2:
					return 1m - StudentT.CDF(0m, 1m, degreesFreedom, x) + StudentT.CDF(0m, 1m, degreesFreedom, -x);

				default:
					throw new ArgumentOutOfRangeException("tails");
			}
		}

		public static decimal TInv(decimal probability, int degreesFreedom)
		{
			return -StudentT.InvCDF(0m, 1m, degreesFreedom, probability / 2);
		}

		public static decimal FDist(decimal x, int degreesFreedom1, int degreesFreedom2)
		{
			return 1m - FisherSnedecor.CDF(degreesFreedom1, degreesFreedom2, x);
		}

		public static decimal FInv(decimal probability, int degreesFreedom1, int degreesFreedom2)
		{
			return FisherSnedecor.InvCDF(degreesFreedom1, degreesFreedom2, 1m - probability);
		}

		public static decimal BetaDist(decimal x, decimal alpha, decimal beta)
		{
			return Beta.CDF(alpha, beta, x);
		}

		public static decimal BetaInv(decimal probability, decimal alpha, decimal beta)
		{
			return Beta.InvCDF(alpha, beta, probability);
		}

		public static decimal GammaDist(decimal x, decimal alpha, decimal beta, bool cumulative)
		{
			return cumulative ? Gamma.CDF(alpha, 1 / beta, x) : Gamma.PDF(alpha, 1 / beta, x);
		}

		public static decimal GammaInv(decimal probability, decimal alpha, decimal beta)
		{
			return Gamma.InvCDF(alpha, 1 / beta, probability);
		}

		public static decimal GAMMALN(decimal z)
		{
			return SpecialFunctions.GammaLn(z);
		}

		public static decimal HypgeomDist(int k, int draws, int success, int population)
		{
			return Hypergeometric.PMF(population, success, draws, k);
		}

		public static decimal NegbinomDist(int k, decimal r, decimal p)
		{
			return NegativeBinomial.PMF(r, p, k);
		}

		public static decimal LognormDist(decimal x, decimal mu, decimal sigma)
		{
			return LogNormal.CDF(mu, sigma, x);
		}

		public static decimal LogInv(decimal p, decimal mu, decimal sigma)
		{
			return LogNormal.InvCDF(mu, sigma, p);
		}

		public static decimal BinomDist(int k, int n, decimal p, bool state)
		{
			if(state == false) {
				return Binomial.PMF(p, n, k);
			}
			return Binomial.CDF(p, n, k);
		}

		public static decimal Poisson(int k, decimal lambda, bool state)
		{
			if(state == false) {
				return Distributions.Poisson.PMF(lambda, k);
			}
			return Distributions.Poisson.CDF(lambda, k);
		}

		public static decimal Quartile(decimal[] array, int quant)
		{
			switch(quant) {
				case 0:
					return ArrayStatistics.Minimum(array);

				case 1:
					return QuantileCustom(array, 0.25m);

				case 2:
					return QuantileCustom(array, 0.5m);

				case 3:
					return QuantileCustom(array, 0.75m);

				case 4:
					return ArrayStatistics.Maximum(array);

				default:
					throw new ArgumentOutOfRangeException("quant");
			}
		}

		public static decimal Percentile(decimal[] array, decimal k)
		{
			return QuantileCustom(array, k);
		}

		public static decimal PercentRank(decimal[] array, decimal x)
		{
			Array.Sort(array);
			return SortedArrayStatistics.QuantileRank(array, x);
		}

		private static decimal QuantileCustom(decimal[] data, decimal tau)
		{
			return ArrayStatistics.QuantileCustomInplace(data, tau);
		}

	}
}
