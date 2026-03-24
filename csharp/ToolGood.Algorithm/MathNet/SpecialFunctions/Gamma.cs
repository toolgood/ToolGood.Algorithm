using System;

namespace ToolGood.Algorithm.MathNet.Numerics
{
    internal partial class SpecialFunctions
    {
        private const int GammaN = 10;

        private const decimal GammaR = 10.900511M;

        private static readonly decimal[] GammaDk ={
            2.48574089138753565546e-5M,
            1.05142378581721974210M,
            -3.45687097222016235469M,
            4.51227709466894823700M,
            -2.98285225323576655721M,
            1.05639711577126713077M,
            -1.95428773191645869583e-1M,
            1.70970543404441224307e-2M,
            -5.71926117404305781283e-4M,
            4.63399473359905636708e-6M,
            -2.71994908488607703910e-9M
        };

        public static decimal GammaLowerRegularized(decimal a, decimal x)
        {
            const decimal epsilon = 0.000000000000001M;
            const decimal big = 4503599627370496.0M;
            const decimal bigInv = 2.22044604925031308085e-16M;

            if (a.AlmostEqual(0.0m)) {
                if (x.AlmostEqual(0.0m)) {
                    return 1m;
                }

                return 1m;
            }

            if (x.AlmostEqual(0.0m)) {
                return 0m;
            }

            decimal ax = (a * MathEx.Log(x)) - x - GammaLn(a);
            if (ax < -709.78271289338399m) {
                return a < x ? 1m : 0m;
            }

            if (x <= 1 || x <= a) {
                decimal r2 = a;
                decimal c2 = 1;
                decimal ans2 = 1;

                do {
                    r2 = r2 + 1;
                    c2 = c2 * x / r2;
                    ans2 += c2;
                }
                while ((c2 / ans2) > epsilon);

                return MathEx.Exp(ax) * ans2 / a;
            }

            int c = 0;
            decimal y = 1 - a;
            decimal z = x + y + 1;

            decimal p3 = 1;
            decimal q3 = x;
            decimal p2 = x + 1;
            decimal q2 = z * x;
            decimal ans = p2 / q2;

            decimal error;

            do {
                c++;
                y += 1;
                z += 2;
                decimal yc = y * c;

                decimal p = (p2 * z) - (p3 * yc);
                decimal q = (q2 * z) - (q3 * yc);

                if (q != 0) {
                    decimal nextans = p / q;
                    error = Math.Abs((ans - nextans) / nextans);
                    ans = nextans;
                } else {
                    error = 1;
                }

                p3 = p2;
                p2 = p;
                q3 = q2;
                q2 = q;

                if (Math.Abs(p) > big) {
                    p3 *= bigInv;
                    p2 *= bigInv;
                    q3 *= bigInv;
                    q2 *= bigInv;
                }
            }
            while (error > epsilon);

            return 1m - (MathEx.Exp(ax) * ans);
        }

        public static decimal GammaLowerRegularizedInv(decimal a, decimal y0)
        {
            const decimal epsilon = 0.000000000000001M;
            const decimal big = 4503599627370496.0M;
            const decimal threshold = 5 * epsilon;

            if (y0.AlmostEqual(0.0m)) {
                return 0m;
            }

            if (y0.AlmostEqual(1.0m)) {
                return 79228162514264337593543950335M;
            }

            y0 = 1 - y0;

            decimal xUpper = big;
            decimal xLower = 0;
            decimal yUpper = 1;
            decimal yLower = 0;

            decimal d = 1 / (9 * a);
            decimal y = 1 - d - (0.98m * Constants.Sqrt2 * ErfInv((2.0m * y0) - 1.0m) * MathEx.Sqrt(d));
            decimal x = a * y * y * y;
            decimal lgm = GammaLn(a);

            for (int i = 0; i < 10; i++) {
                if (x < xLower || x > xUpper) {
                    d = 0.0625m;
                    break;
                }

                y = 1 - GammaLowerRegularized(a, x);
                if (y < yLower || y > yUpper) {
                    d = 0.0625m;
                    break;
                }

                if (y < y0) {
                    xUpper = x;
                    yLower = y;
                } else {
                    xLower = x;
                    yUpper = y;
                }

                d = ((a - 1) * MathEx.Log(x)) - x - lgm;
                if (d < -709.78271289338399m) {
                    d = 0.0625m;
                    break;
                }

                d = -MathEx.Exp(d);
                d = (y - y0) / d;
                if (Math.Abs(d / x) < epsilon) {
                    return x;
                }

                if ((d > (x / 4)) && (y0 < 0.05m)) {
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
            d = 0.5m;
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

                if (x <= 0m) {
                    return 0m;
                }

                if (y >= y0) {
                    xLower = x;
                    yUpper = y;
                    if (dir < 0) {
                        dir = 0;
                        d = 0.5m;
                    } else {
                        if (dir > 1) {
                            d = (0.5m * d) + 0.5m;
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
                        d = 0.5m;
                    } else {
                        if (dir < -1) {
                            d = 0.5m * d;
                        } else {
                            d = (y0 - yLower) / (yUpper - yLower);
                        }
                    }

                    dir = dir - 1;
                }
            }

            return x;
        }

        public static decimal Gamma(decimal z)
        {
            if (z < 0.5m) {
                decimal s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (i - z);
                }

                return MathEx.PI / (MathEx.Sin(MathEx.PI * z)
                                * s
                                * Constants.TwoSqrtEOverPi
                                * MathEx.Pow((0.5m - z + GammaR) / MathEx.E, 0.5m - z));
            } else {
                decimal s = GammaDk[0];
                for (int i = 1; i <= GammaN; i++) {
                    s += GammaDk[i] / (z + i - 1.0m);
                }

                return s * Constants.TwoSqrtEOverPi * MathEx.Pow((z - 0.5m + GammaR) / MathEx.E, z - 0.5m);
            }
        }
    }
}
