package toolgood.algorithm.mathNet.RootFinding;

import java.math.BigDecimal;
import java.math.MathContext;
import java.util.function.Function;

import toolgood.algorithm.mathNet.Precision;

public class Brent {
    public static BigDecimal FindRoot(Function<BigDecimal, BigDecimal> f, BigDecimal lowerBound, BigDecimal upperBound, BigDecimal accuracy) throws Exception {
        return FindRoot(f, lowerBound, upperBound, accuracy, 100);
    }

    public static BigDecimal FindRoot(Function<BigDecimal, BigDecimal> f, BigDecimal lowerBound, BigDecimal upperBound, BigDecimal accuracy, int maxIterations) throws Exception {
        BigDecimal[] root = new BigDecimal[1];
        if (TryFindRoot(f, lowerBound, upperBound, accuracy, maxIterations, root)) {
            return root[0];
        }
        throw new Exception("RootFindingFailed");
    }

    public static boolean TryFindRoot(Function<BigDecimal, BigDecimal> f, BigDecimal lowerBound, BigDecimal upperBound, BigDecimal accuracy, int maxIterations, BigDecimal[] root) {
        BigDecimal fmin = f.apply(lowerBound);
        BigDecimal fmax = f.apply(upperBound);
        BigDecimal froot = fmax;
        BigDecimal d = BigDecimal.ZERO, e = BigDecimal.ZERO;

        root[0] = upperBound;
        BigDecimal xMid = null;

        if (sign(fmin) == sign(fmax)) {
            return false;
        }

        for (int i = 0; i <= maxIterations; i++) {
            if (sign(froot) == sign(fmax)) {
                upperBound = lowerBound;
                fmax = fmin;
                e = d = root[0].subtract(lowerBound);
            }

            if (fmax.abs().compareTo(froot.abs()) < 0) {
                lowerBound = root[0];
                root[0] = upperBound;
                upperBound = lowerBound;
                fmin = froot;
                froot = fmax;
                fmax = fmin;
            }

            BigDecimal xAcc1 = Precision.PositiveDecimalPrecision.multiply(root[0].abs()).add(new BigDecimal("0.5").multiply(accuracy));
            BigDecimal xMidOld = xMid;
            xMid = upperBound.subtract(root[0]).divide(new BigDecimal("2"), MathContext.DECIMAL128);

            if (xMid.abs().compareTo(xAcc1) <= 0 || Precision.AlmostEqualNormRelative(BigDecimal.ZERO, froot, froot, accuracy)) {
                return true;
            }

            if (xMidOld != null && xMid.compareTo(xMidOld) == 0) {
                return false;
            }

            if (e.abs().compareTo(xAcc1) >= 0 && fmin.abs().compareTo(froot.abs()) > 0) {
                BigDecimal s = froot.divide(fmin, MathContext.DECIMAL128);
                BigDecimal p;
                BigDecimal q;
                if (Precision.AlmostEqualRelative(lowerBound, upperBound)) {
                    p = new BigDecimal("2").multiply(xMid).multiply(s);
                    q = BigDecimal.ONE.subtract(s);
                } else {
                    q = fmin.divide(fmax, MathContext.DECIMAL128);
                    BigDecimal r = froot.divide(fmax, MathContext.DECIMAL128);
                    p = s.multiply(new BigDecimal("2").multiply(xMid).multiply(q).multiply(q.subtract(r))
                            .subtract(root[0].subtract(lowerBound).multiply(r.subtract(BigDecimal.ONE))));
                    q = q.subtract(BigDecimal.ONE).multiply(r.subtract(BigDecimal.ONE)).multiply(s.subtract(BigDecimal.ONE));
                }

                if (p.compareTo(BigDecimal.ZERO) > 0) {
                    q = q.negate();
                }

                p = p.abs();
                if (new BigDecimal("2").multiply(p).compareTo(xAcc1.multiply(new BigDecimal("3").multiply(xMid).multiply(q).abs())
                        .min(e.multiply(q).abs())) < 0) {
                    e = d;
                    d = p.divide(q, MathContext.DECIMAL128);
                } else {
                    d = xMid;
                    e = d;
                }
            } else {
                d = xMid;
                e = d;
            }

            lowerBound = root[0];
            fmin = froot;
            if (d.abs().compareTo(xAcc1) > 0) {
                root[0] = root[0].add(d);
            } else {
                root[0] = root[0].add(Sign(xAcc1, xMid));
            }

            froot = f.apply(root[0]);
        }

        return false;
    }

    static int sign(BigDecimal a) {
        if (a.compareTo(BigDecimal.ZERO) == 0) {
            return 0;
        }
        if (a.compareTo(BigDecimal.ZERO) < 0) {
            return -1;
        }
        return 1;
    }

    static BigDecimal Sign(BigDecimal a, BigDecimal b) {
        if (b.compareTo(BigDecimal.ZERO) >= 0) {
            return a.compareTo(BigDecimal.ZERO) >= 0 ? a : a.negate();
        } else {
            return a.compareTo(BigDecimal.ZERO) >= 0 ? a.negate() : a;
        }
    }
}
