using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal partial class SpecialFunctions
    {
        public static decimal GammaLn(decimal z)
        {
            if (z < 0.5m) {
                decimal s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (i - z);
                }

                return Constants.LnPi
                       - MathEx.Log(MathEx.Sin(MathEx.PI * z))
                       - MathEx.Log(s)
                       - Constants.LogTwoSqrtEOverPi
                       - ((0.5m - z) * MathEx.Log((0.5m - z + GammaR) / MathEx.E));
            } else {
                decimal s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (z + i - 1.0m);
                }

                return MathEx.Log(s)
                       + Constants.LogTwoSqrtEOverPi
                       + ((z - 0.5m) * MathEx.Log((z - 0.5m + GammaR) / MathEx.E));
            }
        }

        public static decimal BetaRegularized(decimal a, decimal b, decimal x)
        {
            var bt = (x == 0.0m || x == 1.0m)
                ? 0.0m
                : MathEx.Exp(GammaLn(a + b) - GammaLn(a) - GammaLn(b) + (a * MathEx.Log(x)) + (b * MathEx.Log(1.0m - x)));

            var symmetryTransformation = x >= (a + 1.0m) / (a + b + 2.0m);

            var eps = Precision.DecimalPrecision;

            if (symmetryTransformation) {
                x = 1.0m - x;
                var swap = a;
                a = b;
                b = swap;
            }

            var qab = a + b;
            var qap = a + 1.0m;
            var qam = a - 1.0m;
            var c = 1.0m;
            var d = 1.0m - (qab * x / qap);

            if (Math.Abs(d) < eps) {
                d = eps;
            }

            d = 1.0m / d;
            var h = d;

            for (int m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
                var aa = m * (b - m) * x / ((qam + m2) * (a + m2));
                d = 1.0m + (aa * d);

                if (Math.Abs(d) < eps) {
                    d = eps;
                }

                c = 1.0m + (aa / c);
                if (Math.Abs(c) < eps) {
                    c = eps;
                }

                d = 1.0m / d;
                h *= d * c;
                aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2));
                d = 1.0m + (aa * d);

                if (Math.Abs(d) < eps) {
                    d = eps;
                }

                c = 1.0m + (aa / c);

                if (Math.Abs(c) < eps) {
                    c = eps;
                }

                d = 1.0m / d;
                var del = d * c;
                h *= del;

                if (Math.Abs(del - 1.0m) <= eps) {
                    return symmetryTransformation ? 1.0m - (bt * h / a) : bt * h / a;
                }
            }

            return symmetryTransformation ? 1.0m - (bt * h / a) : bt * h / a;
        }
    }
}
