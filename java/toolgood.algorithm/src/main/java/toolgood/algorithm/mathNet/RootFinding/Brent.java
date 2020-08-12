package toolgood.algorithm.mathNet.RootFinding;

import java.util.function.Function;

import toolgood.algorithm.mathNet.Precision;

 
public class Brent {
    public static double FindRoot(Function<Double, Double> f, double lowerBound, double upperBound, double accuracy)
            throws Exception {
        return FindRoot(f, lowerBound, upperBound, accuracy, 100);
    }

    public static double FindRoot(Function<Double, Double> f, double lowerBound, double upperBound, double accuracy,
            int maxIterations) throws Exception
    {
        RootNumber root=new RootNumber();
        if (TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations,root)) {
            return root.root;
        }
        throw new Exception("RootFindingFailed");
    }


    public static boolean TryFindRoot(Function<Double, Double> f, double lowerBound, double upperBound, double accuracy,
            int maxIterations, RootNumber root)
    {
        double fmin = f.apply(lowerBound);
        double fmax = f.apply(upperBound);
        double froot = fmax;
        double d = 0.0, e = 0.0;

        root.root = upperBound;
        double xMid = Double.NaN;

        // Root must be bracketed.
        if (sign(fmin) == sign(fmax)) {
            return false;
        }

        for (int i = 0; i <= maxIterations; i++) {
            // adjust bounds
            if (sign(froot) == sign(fmax)) {
                upperBound = lowerBound;
                fmax = fmin;
                e = d = root.root - lowerBound;
            }

            if (Math.abs(fmax) < Math.abs(froot)) {
                lowerBound = root.root;
                root.root = upperBound;
                upperBound = lowerBound;
                fmin = froot;
                froot = fmax;
                fmax = fmin;
            }

            // convergence check
            double xAcc1 = Precision.PositiveDoublePrecision * Math.abs(root.root) + 0.5 * accuracy;
            double xMidOld = xMid;
            xMid = (upperBound - root.root) / 2.0;

            if (Math.abs(xMid) <= xAcc1 || Precision.AlmostEqualNormRelative(froot,0, froot, accuracy)) {
                return true;
            }

            if (xMid == xMidOld) {
                // accuracy not sufficient, but cannot be improved further
                return false;
            }

            if (Math.abs(e) >= xAcc1 && Math.abs(fmin) > Math.abs(froot)) {
                // Attempt inverse quadratic interpolation
                double s = froot / fmin;
                double p;
                double q;
                if (Precision.AlmostEqualRelative(lowerBound,upperBound)) {
                    p = 2.0 * xMid * s;
                    q = 1.0 - s;
                } else {
                    q = fmin / fmax;
                    double r = froot / fmax;
                    p = s * (2.0 * xMid * q * (q - r) - (root.root - lowerBound) * (r - 1.0));
                    q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                }

                if (p > 0.0) {
                    // Check whether in bounds
                    q = -q;
                }

                p = Math.abs(p);
                if (2.0 * p < Math.min(3.0 * xMid * q - Math.abs(xAcc1 * q), Math.abs(e * q))) {
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

            lowerBound = root.root;
            fmin = froot;
            if (Math.abs(d) > xAcc1) {
                root.root += d;
            } else {
                root.root += Sign(xAcc1, xMid);
            }

            froot = f.apply(root.root);
        }

        return false;
    }
    static int sign(double a){
        if(a==0.0){
            return 0;
        }
        if(a<0){
            return -1;
        }
        return 1;
    }

    /// <summary>Helper method useful for preventing rounding errors.</summary>
    /// <returns>a*sign(b)</returns>
    static double Sign(double a, double b)
    {
        return b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a);
    }

}