using System;

namespace ToolGood.Algorithm.MathNet.Numerics.RootFinding
{
    internal static class Brent
    {
        public static decimal FindRoot(Func<decimal, decimal> f, decimal lowerBound, decimal upperBound, decimal accuracy = 1e-8m, int maxIterations = 100)
        {
            if (TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations, out decimal root)) {
                return root;
            }

            throw new InvalidOperationException("RootFindingFailed");
        }

        public static bool TryFindRoot(Func<decimal, decimal> f, decimal lowerBound, decimal upperBound, decimal accuracy, int maxIterations, out decimal root)
        {
            decimal fmin = f(lowerBound);
            decimal fmax = f(upperBound);
            decimal froot = fmax;
            decimal d = 0.0m, e = 0.0m;

            root = upperBound;
            decimal xMid = decimal.MinValue;

            if (Math.Sign(fmin) == Math.Sign(fmax)) {
                return false;
            }

            for (int i = 0; i <= maxIterations; i++) {
                if (Math.Sign(froot) == Math.Sign(fmax)) {
                    upperBound = lowerBound;
                    fmax = fmin;
                    e = d = root - lowerBound;
                }

                if (Math.Abs(fmax) < Math.Abs(froot)) {
                    lowerBound = root;
                    root = upperBound;
                    upperBound = lowerBound;
                    fmin = froot;
                    froot = fmax;
                    fmax = fmin;
                }

                decimal xAcc1 = Precision.PositiveDecimalPrecision * Math.Abs(root) + 0.5m * accuracy;
                decimal xMidOld = xMid;
                xMid = (upperBound - root) / 2.0m;

                if (Math.Abs(xMid) <= xAcc1 || froot.AlmostEqualNormRelative(0, froot, accuracy)) {
                    return true;
                }

                if (xMid == xMidOld) {
                    return false;
                }

                if (Math.Abs(e) >= xAcc1 && Math.Abs(fmin) > Math.Abs(froot)) {
                    decimal s = froot / fmin;
                    decimal p;
                    decimal q;
                    if (lowerBound.AlmostEqualRelative(upperBound)) {
                        p = 2.0m * xMid * s;
                        q = 1.0m - s;
                    } else {
                        q = fmin / fmax;
                        decimal r = froot / fmax;
                        p = s * (2.0m * xMid * q * (q - r) - (root - lowerBound) * (r - 1.0m));
                        q = (q - 1.0m) * (r - 1.0m) * (s - 1.0m);
                    }

                    if (p > 0.0m) {
                        q = -q;
                    }

                    p = Math.Abs(p);
                    if (2.0m * p < Math.Min(3.0m * xMid * q - Math.Abs(xAcc1 * q), Math.Abs(e * q))) {
                        e = d;
                        d = p / q;
                    } else {
                        d = xMid;
                        e = d;
                    }
                } else {
                    d = xMid;
                    e = d;
                }

                lowerBound = root;
                fmin = froot;
                if (Math.Abs(d) > xAcc1) {
                    root += d;
                } else {
                    root += Sign(xAcc1, xMid);
                }

                froot = f(root);
            }

            return false;
        }

        private static decimal Sign(decimal a, decimal b)
        {
            return b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a);
        }
    }
}
