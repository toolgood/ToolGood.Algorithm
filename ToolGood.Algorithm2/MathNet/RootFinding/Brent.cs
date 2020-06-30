using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Algorithm.MathNet.Numerics.RootFinding
{
    /// <summary>
    /// Algorithm by by Brent, Van Wijngaarden, Dekker et al.
    /// Implementation inspired by Press, Teukolsky, Vetterling, and Flannery, "Numerical Recipes in C", 2nd edition, Cambridge University Press
    /// </summary>
    public static class Brent
    {
        public static double FindRoot(Func<double, double> f, double lowerBound, double upperBound, double accuracy = 1e-8, int maxIterations = 100)
        {
            double root;
            if (TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations, out root)) {
                return root;
            }
            
            throw new Exception("RootFindingFailed");
        }


        public static bool TryFindRoot(Func<double, double> f, double lowerBound, double upperBound, double accuracy, int maxIterations, out double root)
        {
            double fmin = f(lowerBound);
            double fmax = f(upperBound);
            double froot = fmax;
            double d = 0.0, e = 0.0;

            root = upperBound;
            double xMid = double.NaN;

            // Root must be bracketed.
            if (Math.Sign(fmin) == Math.Sign(fmax)) {
                return false;
            }

            for (int i = 0; i <= maxIterations; i++) {
                // adjust bounds
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

                // convergence check
                double xAcc1 = Precision.PositiveDoublePrecision * Math.Abs(root) + 0.5 * accuracy;
                double xMidOld = xMid;
                xMid = (upperBound - root) / 2.0;

                if (Math.Abs(xMid) <= xAcc1 || froot.AlmostEqualNormRelative(0, froot, accuracy)) {
                    return true;
                }

                if (xMid == xMidOld) {
                    // accuracy not sufficient, but cannot be improved further
                    return false;
                }

                if (Math.Abs(e) >= xAcc1 && Math.Abs(fmin) > Math.Abs(froot)) {
                    // Attempt inverse quadratic interpolation
                    double s = froot / fmin;
                    double p;
                    double q;
                    if (lowerBound.AlmostEqualRelative(upperBound)) {
                        p = 2.0 * xMid * s;
                        q = 1.0 - s;
                    } else {
                        q = fmin / fmax;
                        double r = froot / fmax;
                        p = s * (2.0 * xMid * q * (q - r) - (root - lowerBound) * (r - 1.0));
                        q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                    }

                    if (p > 0.0) {
                        // Check whether in bounds
                        q = -q;
                    }

                    p = Math.Abs(p);
                    if (2.0 * p < Math.Min(3.0 * xMid * q - Math.Abs(xAcc1 * q), Math.Abs(e * q))) {
                        // Accept interpolation
                        e = d;
                        d = p / q;
                    } else {
                        // Interpolation failed, use bisection
                        d = xMid;
                        e = d;
                    }
                } else {
                    // Bounds decreasing too slowly, use bisection
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

        /// <summary>Helper method useful for preventing rounding errors.</summary>
        /// <returns>a*sign(b)</returns>
        static double Sign(double a, double b)
        {
            return b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a);
        }


    }

}
