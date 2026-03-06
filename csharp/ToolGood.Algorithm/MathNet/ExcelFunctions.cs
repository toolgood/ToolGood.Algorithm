using System;
using System.Collections.Generic;
using ToolGood.Algorithm.MathNet.Numerics.Distributions;
using ToolGood.Algorithm.MathNet.Numerics.Statistics;

namespace ToolGood.Algorithm.MathNet.Numerics
{
	/// <summary>
	/// Collection of functions equivalent to those provided by Microsoft Excel
	/// but backed instead by Math.NET Numerics.
	/// We do not recommend to use them except in an intermediate phase when
	/// porting over solutions previously implemented in Excel.
	/// </summary>
	internal static partial class ExcelFunctions
	{
		/// <summary>
		/// NormSDist
		/// </summary>
		/// <param name="z"></param>
		/// <returns></returns>
		public static double NormSDist(double z)
		{
			return Normal.CDF(0d, 1d, z);
		}

		/// <summary>
		/// NormSInv
		/// </summary>
		/// <param name="probability"></param>
		/// <returns></returns>
		public static double NormSInv(double probability)
		{
			return Normal.InvCDF(0d, 1d, probability);
		}

		/// <summary>
		/// NormDist
		/// </summary>
		/// <param name="x"></param>
		/// <param name="mean"></param>
		/// <param name="standardDev"></param>
		/// <param name="cumulative"></param>
		/// <returns></returns>
		public static double NormDist(double x, double mean, double standardDev, bool cumulative)
		{
			return cumulative ? Normal.CDF(mean, standardDev, x) : Normal.PDF(mean, standardDev, x);
		}

		/// <summary>
		/// NormInv
		/// </summary>
		/// <param name="probability"></param>
		/// <param name="mean"></param>
		/// <param name="standardDev"></param>
		/// <returns></returns>
		public static double NormInv(double probability, double mean, double standardDev)
		{
			return Normal.InvCDF(mean, standardDev, probability);
		}

		/// <summary>
		/// TDist
		/// </summary>
		/// <param name="x"></param>
		/// <param name="degreesFreedom"></param>
		/// <param name="tails"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static double TDist(double x, int degreesFreedom, int tails)
		{
			switch(tails) {
				case 1:
					return 1d - StudentT.CDF(0d, 1d, degreesFreedom, x);

				case 2:
					return 1d - StudentT.CDF(0d, 1d, degreesFreedom, x) + StudentT.CDF(0d, 1d, degreesFreedom, -x);

				default:
					throw new ArgumentOutOfRangeException("tails");
			}
		}

		/// <summary>
		/// TInv
		/// </summary>
		/// <param name="probability"></param>
		/// <param name="degreesFreedom"></param>
		/// <returns></returns>
		public static double TInv(double probability, int degreesFreedom)
		{
			return -StudentT.InvCDF(0d, 1d, degreesFreedom, probability / 2);
		}

		/// <summary>
		/// FDist
		/// </summary>
		/// <param name="x"></param>
		/// <param name="degreesFreedom1"></param>
		/// <param name="degreesFreedom2"></param>
		/// <returns></returns>
		public static double FDist(double x, int degreesFreedom1, int degreesFreedom2)
		{
			return 1d - FisherSnedecor.CDF(degreesFreedom1, degreesFreedom2, x);
		}

		/// <summary>
		/// FInv
		/// </summary>
		/// <param name="probability"></param>
		/// <param name="degreesFreedom1"></param>
		/// <param name="degreesFreedom2"></param>
		/// <returns></returns>
		public static double FInv(double probability, int degreesFreedom1, int degreesFreedom2)
		{
			return FisherSnedecor.InvCDF(degreesFreedom1, degreesFreedom2, 1d - probability);
		}

		/// <summary>
		/// BetaDist
		/// </summary>
		/// <param name="x"></param>
		/// <param name="alpha"></param>
		/// <param name="beta"></param>
		/// <returns></returns>
		public static double BetaDist(double x, double alpha, double beta)
		{
			return Beta.CDF(alpha, beta, x);
		}

		/// <summary>
		/// BetaInv
		/// </summary>
		/// <param name="probability"></param>
		/// <param name="alpha"></param>
		/// <param name="beta"></param>
		/// <returns></returns>
		public static double BetaInv(double probability, double alpha, double beta)
		{
			return Beta.InvCDF(alpha, beta, probability);
		}

		public static double GammaDist(double x, double alpha, double beta, bool cumulative)
		{
			return cumulative ? Gamma.CDF(alpha, 1 / beta, x) : Gamma.PDF(alpha, 1 / beta, x);
		}

		public static double GammaInv(double probability, double alpha, double beta)
		{
			return Gamma.InvCDF(alpha, 1 / beta, probability);
		}

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
			if(state == false) {
				return Binomial.PMF(p, n, k);
			}
			return Binomial.CDF(p, n, k);
		}

		public static double Poisson(int k, double lambda, bool state)
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