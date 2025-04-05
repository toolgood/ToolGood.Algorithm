using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal static partial class SpecialFunctions
    {
        private const int GammaN = 10;

        /// <summary>
        /// Auxiliary variable when evaluating the <see cref="GammaLn"/> function.
        /// </summary>
        private const double GammaR = 10.900511;

        private static readonly double[] GammaDk ={
            2.48574089138753565546e-5,
            1.05142378581721974210,
            -3.45687097222016235469,
            4.51227709466894823700,
            -2.98285225323576655721,
            1.05639711577126713077,
            -1.95428773191645869583e-1,
            1.70970543404441224307e-2,
            -5.71926117404305781283e-4,
            4.63399473359905636708e-6,
            -2.71994908488607703910e-9
        };

        public static double GammaLowerRegularized(double a, double x)
        {
            const double epsilon = 0.000000000000001;
            const double big = 4503599627370496.0;
            const double bigInv = 2.22044604925031308085e-16;

            //if (a < 0d) {
            //    throw new ArgumentOutOfRangeException("a", Properties.Resources.ArgumentNotNegative);
            //}

            //if (x < 0d) {
            //    throw new ArgumentOutOfRangeException("x", Properties.Resources.ArgumentNotNegative);
            //}

            if (a.AlmostEqual(0.0)) {
                if (x.AlmostEqual(0.0)) {
                    //use right hand limit value because so that regularized upper/lower gamma definition holds.
                    return 1d;
                }

                return 1d;
            }

            if (x.AlmostEqual(0.0)) {
                return 0d;
            }

            double ax = (a * Math.Log(x)) - x - GammaLn(a);
            if (ax < -709.78271289338399) {
                return a < x ? 1d : 0d;
            }

            if (x <= 1 || x <= a) {
                double r2 = a;
                double c2 = 1;
                double ans2 = 1;

                do {
                    r2 = r2 + 1;
                    c2 = c2 * x / r2;
                    ans2 += c2;
                }
                while ((c2 / ans2) > epsilon);

                return Math.Exp(ax) * ans2 / a;
            }

            int c = 0;
            double y = 1 - a;
            double z = x + y + 1;

            double p3 = 1;
            double q3 = x;
            double p2 = x + 1;
            double q2 = z * x;
            double ans = p2 / q2;

            double error;

            do {
                c++;
                y += 1;
                z += 2;
                double yc = y * c;

                double p = (p2 * z) - (p3 * yc);
                double q = (q2 * z) - (q3 * yc);

                if (q != 0) {
                    double nextans = p / q;
                    error = Math.Abs((ans - nextans) / nextans);
                    ans = nextans;
                } else {
                    // zero div, skip
                    error = 1;
                }

                // shift
                p3 = p2;
                p2 = p;
                q3 = q2;
                q2 = q;

                // normalize fraction when the numerator becomes large
                if (Math.Abs(p) > big) {
                    p3 *= bigInv;
                    p2 *= bigInv;
                    q3 *= bigInv;
                    q2 *= bigInv;
                }
            }
            while (error > epsilon);

            return 1d - (Math.Exp(ax) * ans);
        }

        public static double GammaLowerRegularizedInv(double a, double y0)
        {
            const double epsilon = 0.000000000000001;
            const double big = 4503599627370496.0;
            const double threshold = 5 * epsilon;

            if (double.IsNaN(a) || double.IsNaN(y0)) {
                return double.NaN;
            }

            //if (a < 0 || a.AlmostEqual(0.0)) {
            //    throw new ArgumentOutOfRangeException("a");
            //}

            //if (y0 < 0 || y0 > 1) {
            //    throw new ArgumentOutOfRangeException("y0");
            //}

            if (y0.AlmostEqual(0.0)) {
                return 0d;
            }

            if (y0.AlmostEqual(1.0)) {
                return double.PositiveInfinity;
            }

            y0 = 1 - y0;

            double xUpper = big;
            double xLower = 0;
            double yUpper = 1;
            double yLower = 0;

            // Initial Guess
            double d = 1 / (9 * a);
            double y = 1 - d - (0.98 * Constants.Sqrt2 * ErfInv((2.0 * y0) - 1.0) * Math.Sqrt(d));
            double x = a * y * y * y;
            double lgm = GammaLn(a);

            for (int i = 0; i < 10; i++) {
                if (x < xLower || x > xUpper) {
                    d = 0.0625;
                    break;
                }

                y = 1 - GammaLowerRegularized(a, x);
                if (y < yLower || y > yUpper) {
                    d = 0.0625;
                    break;
                }

                if (y < y0) {
                    xUpper = x;
                    yLower = y;
                } else {
                    xLower = x;
                    yUpper = y;
                }

                d = ((a - 1) * Math.Log(x)) - x - lgm;
                if (d < -709.78271289338399) {
                    d = 0.0625;
                    break;
                }

                d = -Math.Exp(d);
                d = (y - y0) / d;
                if (Math.Abs(d / x) < epsilon) {
                    return x;
                }

                if ((d > (x / 4)) && (y0 < 0.05)) {
                    // Naive heuristics for cases near the singularity
                    d = x / 10;
                }

                x -= d;
            }

            if (xUpper == big) {
                if (x <= 0) {
                    x = 1;
                }

                while (xUpper == big) {
                    x = (1 + d) * x;
                    y = 1 - GammaLowerRegularized(a, x);
                    if (y < y0) {
                        xUpper = x;
                        yLower = y;
                        break;
                    }

                    d = d + d;
                }
            }

            int dir = 0;
            d = 0.5;
            for (int i = 0; i < 400; i++) {
                x = xLower + (d * (xUpper - xLower));
                y = 1 - GammaLowerRegularized(a, x);
                lgm = (xUpper - xLower) / (xLower + xUpper);
                if (Math.Abs(lgm) < threshold) {
                    return x;
                }

                lgm = (y - y0) / y0;
                if (Math.Abs(lgm) < threshold) {
                    return x;
                }

                if (x <= 0d) {
                    return 0d;
                }

                if (y >= y0) {
                    xLower = x;
                    yUpper = y;
                    if (dir < 0) {
                        dir = 0;
                        d = 0.5;
                    } else {
                        if (dir > 1) {
                            d = (0.5 * d) + 0.5;
                        } else {
                            d = (y0 - yLower) / (yUpper - yLower);
                        }
                    }

                    dir = dir + 1;
                } else {
                    xUpper = x;
                    yLower = y;
                    if (dir > 0) {
                        dir = 0;
                        d = 0.5;
                    } else {
                        if (dir < -1) {
                            d = 0.5 * d;
                        } else {
                            d = (y0 - yLower) / (yUpper - yLower);
                        }
                    }

                    dir = dir - 1;
                }
            }

            return x;
        }

        public static double Gamma(double z)
        {
            if (z < 0.5) {
                double s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (i - z);
                }

                return Math.PI / (Math.Sin(Math.PI * z)
                                * s
                                * Constants.TwoSqrtEOverPi
                                * Math.Pow((0.5 - z + GammaR) / Math.E, 0.5 - z));
            } else {
                double s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (z + i - 1.0);
                }

                return s * Constants.TwoSqrtEOverPi * Math.Pow((z - 0.5 + GammaR) / Math.E, z - 0.5);
            }
        }
    }
}