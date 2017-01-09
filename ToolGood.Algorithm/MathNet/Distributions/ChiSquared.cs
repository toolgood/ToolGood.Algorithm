//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
//{
//    public class ChiSquared
//    {
//        /// <summary>
//        /// Computes the probability density of the distribution (PDF) at x, i.e. ∂P(X ≤ x)/∂x.
//        /// </summary>
//        /// <param name="freedom">The degrees of freedom (k) of the distribution. Range: k > 0.</param>
//        /// <param name="x">The location at which to compute the density.</param>
//        /// <returns>the density at <paramref name="x"/>.</returns>
//        /// <seealso cref="Density"/>
//        public static double PDF(double freedom, double x)
//        {
//            if (freedom <= 0.0) {
//                throw new ArgumentException("InvalidDistributionParameters");
//            }

//            if (double.IsPositiveInfinity(freedom) || double.IsPositiveInfinity(x) || x == 0.0) {
//                return 0.0;
//            }

//            if (freedom > 160.0) {
//                return Math.Exp(PDFLn(freedom, x));
//            }

//            return (Math.Pow(x, (freedom / 2.0) - 1.0) * Math.Exp(-x / 2.0)) / (Math.Pow(2.0, freedom / 2.0) * SpecialFunctions.Gamma(freedom / 2.0));
//        }

//        /// <summary>
//        /// Computes the log probability density of the distribution (lnPDF) at x, i.e. ln(∂P(X ≤ x)/∂x).
//        /// </summary>
//        /// <param name="freedom">The degrees of freedom (k) of the distribution. Range: k > 0.</param>
//        /// <param name="x">The location at which to compute the density.</param>
//        /// <returns>the log density at <paramref name="x"/>.</returns>
//        /// <seealso cref="DensityLn"/>
//        public static double PDFLn(double freedom, double x)
//        {
//            if (freedom <= 0.0) {
//                throw new ArgumentException("InvalidDistributionParameters");
//            }

//            if (double.IsPositiveInfinity(freedom) || double.IsPositiveInfinity(x) || x == 0.0) {
//                return double.NegativeInfinity;
//            }

//            return (-x / 2.0) + (((freedom / 2.0) - 1.0) * Math.Log(x)) - ((freedom / 2.0) * Math.Log(2)) - SpecialFunctions.GammaLn(freedom / 2.0);
//        }

//        /// <summary>
//        /// Computes the cumulative distribution (CDF) of the distribution at x, i.e. P(X ≤ x).
//        /// </summary>
//        /// <param name="x">The location at which to compute the cumulative distribution function.</param>
//        /// <param name="freedom">The degrees of freedom (k) of the distribution. Range: k > 0.</param>
//        /// <returns>the cumulative distribution at location <paramref name="x"/>.</returns>
//        /// <seealso cref="CumulativeDistribution"/>
//        public static double CDF(double freedom, double x)
//        {
//            if (freedom <= 0.0) {
//                throw new ArgumentException("InvalidDistributionParameters");
//            }

//            if (double.IsPositiveInfinity(x)) {
//                return 1.0;
//            }

//            if (double.IsPositiveInfinity(freedom)) {
//                return 1.0;
//            }

//            return SpecialFunctions.GammaLowerRegularized(freedom / 2.0, x / 2.0);
//        }

//        /// <summary>
//        /// Computes the inverse of the cumulative distribution function (InvCDF) for the distribution
//        /// at the given probability. This is also known as the quantile or percent point function.
//        /// </summary>
//        /// <param name="freedom">The degrees of freedom (k) of the distribution. Range: k > 0.</param>
//        /// <param name="p">The location at which to compute the inverse cumulative density.</param>
//        /// <returns>the inverse cumulative density at <paramref name="p"/>.</returns>
//        public static double InvCDF(double freedom, double p)
//        {
//            //if (!IsValidParameterSet(freedom)) {
//            //    throw new ArgumentException("InvalidDistributionParameters");
//            //}

//            return SpecialFunctions.GammaLowerRegularizedInv(freedom / 2.0, p) / 0.5;
//        }
//    }
//}
