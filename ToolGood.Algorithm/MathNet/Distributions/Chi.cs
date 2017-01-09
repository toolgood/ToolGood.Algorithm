//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
//{
//    /// <summary>
//    /// Continuous Univariate Chi distribution.
//    /// This distribution is a continuous probability distribution. The distribution usually arises when a k-dimensional vector's orthogonal
//    /// components are independent and each follow a standard normal distribution. The length of the vector will
//    /// then have a chi distribution.
//    /// <a href="http://en.wikipedia.org/wiki/Chi_distribution">Wikipedia - Chi distribution</a>.
//    /// </summary>
//    public class Chi
//    {
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

//            return SpecialFunctions.GammaLowerRegularized(freedom / 2.0, x * x / 2.0);
//        }

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

//            return (Math.Pow(2.0, 1.0 - (freedom / 2.0)) * Math.Pow(x, freedom - 1.0) * Math.Exp(-x * x / 2.0)) / SpecialFunctions.Gamma(freedom / 2.0);
//        }

//        public static double PDFLn(double freedom, double x)
//        {
//            if (freedom <= 0.0) {
//                throw new ArgumentException("InvalidDistributionParameters");
//            }

//            if (double.IsPositiveInfinity(freedom) || double.IsPositiveInfinity(x) || x == 0.0) {
//                return double.NegativeInfinity;
//            }

//            return ((1.0 - (freedom / 2.0)) * Math.Log(2.0)) + ((freedom - 1.0) * Math.Log(x)) - (x * x / 2.0) - SpecialFunctions.GammaLn(freedom / 2.0);
//        }

//    }
//}
