using System;
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
            switch (tails) {
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

        public static double Quartile(double[] array, int quant)
        {
            switch (quant) {
                case 0:
                    return ArrayStatistics.Minimum(array);

                case 1:
                    return array.QuantileCustom(0.25, QuantileDefinition.Excel);

                case 2:
                    return array.QuantileCustom(0.5, QuantileDefinition.Excel);

                case 3:
                    return array.QuantileCustom(0.75, QuantileDefinition.Excel);

                case 4:
                    return ArrayStatistics.Maximum(array);

                default:
                    throw new ArgumentOutOfRangeException("quant");
            }
        }

        public static double Percentile(double[] array, double k)
        {
            return array.QuantileCustom(k, QuantileDefinition.Excel);
        }

        public static double PercentRank(double[] array, double x)
        {
            return array.QuantileRank(x);
        }
    }
}