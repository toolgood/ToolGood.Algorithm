using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    public static partial class SpecialFunctions
    {

        public static double GammaLn(double z)
        {
            if (z < 0.5) {
                double s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (i - z);
                }

                return Constants.LnPi
                       - Math.Log(Math.Sin(Math.PI * z))
                       - Math.Log(s)
                       - Constants.LogTwoSqrtEOverPi
                       - ((0.5 - z) * Math.Log((0.5 - z + GammaR) / Math.E));
            } else {
                double s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (z + i - 1.0);
                }

                return Math.Log(s)
                       + Constants.LogTwoSqrtEOverPi
                       + ((z - 0.5) * Math.Log((z - 0.5 + GammaR) / Math.E));
            }
        }



        public static double BetaRegularized(double a, double b, double x)
        {
            //if (a < 0.0) {
            //    throw new ArgumentOutOfRangeException("a", Resources.ArgumentNotNegative);
            //}

            //if (b < 0.0) {
            //    throw new ArgumentOutOfRangeException("b", Resources.ArgumentNotNegative);
            //}

            //if (x < 0.0 || x > 1.0) {
            //    throw new ArgumentOutOfRangeException("x", Resources.ArgumentInIntervalXYInclusive);
            //}

            var bt = (x == 0.0 || x == 1.0)
                ? 0.0
                : Math.Exp(GammaLn(a + b) - GammaLn(a) - GammaLn(b) + (a * Math.Log(x)) + (b * Math.Log(1.0 - x)));

            var symmetryTransformation = x >= (a + 1.0) / (a + b + 2.0);

            /* Continued fraction representation */
            var eps = Precision.DoublePrecision;
            var fpmin = 0.0.Increment() / eps;

            if (symmetryTransformation) {
                x = 1.0 - x;
                var swap = a;
                a = b;
                b = swap;
            }

            var qab = a + b;
            var qap = a + 1.0;
            var qam = a - 1.0;
            var c = 1.0;
            var d = 1.0 - (qab * x / qap);

            if (Math.Abs(d) < fpmin) {
                d = fpmin;
            }

            d = 1.0 / d;
            var h = d;

            for (int m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
                var aa = m * (b - m) * x / ((qam + m2) * (a + m2));
                d = 1.0 + (aa * d);

                if (Math.Abs(d) < fpmin) {
                    d = fpmin;
                }

                c = 1.0 + (aa / c);
                if (Math.Abs(c) < fpmin) {
                    c = fpmin;
                }

                d = 1.0 / d;
                h *= d * c;
                aa = -(a + m) * (qab + m) * x / ((a + m2) * (qap + m2));
                d = 1.0 + (aa * d);

                if (Math.Abs(d) < fpmin) {
                    d = fpmin;
                }

                c = 1.0 + (aa / c);

                if (Math.Abs(c) < fpmin) {
                    c = fpmin;
                }

                d = 1.0 / d;
                var del = d * c;
                h *= del;

                if (Math.Abs(del - 1.0) <= eps) {
                    return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
                }
            }

            return symmetryTransformation ? 1.0 - (bt * h / a) : bt * h / a;
        }

    }
}
