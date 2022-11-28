using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    sealed class Gamma
    {
        public static double CDF(double shape, double rate, double x)
        {
            //if (shape < 0.0 || rate < 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            if (double.IsPositiveInfinity(rate)) {
                return x >= shape ? 1.0 : 0.0;
            }

            if (shape == 0.0 && rate == 0.0) {
                return 0.0;
            }

            return SpecialFunctions.GammaLowerRegularized(shape, x * rate);
        }

        public static double PDF(double shape, double rate, double x)
        {
            //if (shape < 0.0 || rate < 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            if (double.IsPositiveInfinity(rate)) {
                return x == shape ? double.PositiveInfinity : 0.0;
            }

            if (shape == 0.0 && rate == 0.0) {
                return 0.0;
            }

            if (shape == 1.0) {
                return rate * Math.Exp(-rate * x);
            }

            if (shape > 160.0) {
                return Math.Exp(PDFLn(shape, rate, x));
            }

            return Math.Pow(rate, shape) * Math.Pow(x, shape - 1.0) * Math.Exp(-rate * x) / SpecialFunctions.Gamma(shape);
        }
        public static double PDFLn(double shape, double rate, double x)
        {
            //if (shape < 0.0 || rate < 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}

            if (double.IsPositiveInfinity(rate)) {
                return x == shape ? double.PositiveInfinity : double.NegativeInfinity;
            }

            if (shape == 0.0 && rate == 0.0) {
                return double.NegativeInfinity;
            }

            if (shape == 1.0) {
                return Math.Log(rate) - (rate * x);
            }

            return (shape * Math.Log(rate)) + ((shape - 1.0) * Math.Log(x)) - (rate * x) - SpecialFunctions.GammaLn(shape);
        }

        public static double InvCDF(double shape, double rate, double p)
        {
            //if (shape < 0.0 || rate < 0.0) {
            //    throw new ArgumentException(Resources.InvalidDistributionParameters);
            //}
            //if (a < 0 || a.AlmostEqual(0.0)) {
            //    throw new ArgumentOutOfRangeException("a");
            //}

            //if (y0 < 0 || y0 > 1) {
            //    throw new ArgumentOutOfRangeException("y0");
            //}

            return SpecialFunctions.GammaLowerRegularizedInv(shape, p) / rate;
        }
    }
}
