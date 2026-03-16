using System;

namespace ToolGood.Algorithm.MathNet.Numerics.Distributions
{
    internal sealed class Gamma
    {
        public static decimal CDF(decimal shape, decimal rate, decimal x)
        {
            if (shape == 0.0m && rate == 0.0m) {
                return 0.0m;
            }

            return SpecialFunctions.GammaLowerRegularized(shape, x * rate);
        }

        public static decimal PDF(decimal shape, decimal rate, decimal x)
        {
            if (shape == 0.0m && rate == 0.0m) {
                return 0.0m;
            }

            if (shape == 1.0m) {
                return rate * MathEx.Exp(-rate * x);
            }

            if (shape > 160.0m) {
                return MathEx.Exp(PDFLn(shape, rate, x));
            }

            return MathEx.Pow(rate, shape) * MathEx.Pow(x, shape - 1.0m) * MathEx.Exp(-rate * x) / SpecialFunctions.Gamma(shape);
        }

        public static decimal PDFLn(decimal shape, decimal rate, decimal x)
        {
            if (shape == 0.0m && rate == 0.0m) {
                return decimal.MinValue;
            }

            if (shape == 1.0m) {
                return MathEx.Log(rate) - (rate * x);
            }

            return (shape * MathEx.Log(rate)) + ((shape - 1.0m) * MathEx.Log(x)) - (rate * x) - SpecialFunctions.GammaLn(shape);
        }

        public static decimal InvCDF(decimal shape, decimal rate, decimal p)
        {
            return SpecialFunctions.GammaLowerRegularizedInv(shape, p) / rate;
        }
    }
}
