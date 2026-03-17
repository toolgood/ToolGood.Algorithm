package toolgood.algorithm.mathNet;

import java.math.BigDecimal;
import java.math.MathContext;

import toolgood.algorithm.system.MathEx;

public class SpecialFunctions {
    private static final int GammaN = 10;
    private static final BigDecimal GammaR = new BigDecimal("10.900511");

    private static final BigDecimal[] GammaDk = {
        new BigDecimal("2.48574089138753565546e-5"),
        new BigDecimal("1.05142378581721974210"),
        new BigDecimal("-3.45687097222016235469"),
        new BigDecimal("4.51227709466894823700"),
        new BigDecimal("-2.98285225323576655721"),
        new BigDecimal("1.05639711577126713077"),
        new BigDecimal("-1.95428773191645869583e-1"),
        new BigDecimal("1.70970543404441224307e-2"),
        new BigDecimal("-5.71926117404305781283e-4"),
        new BigDecimal("4.63399473359905636708e-6"),
        new BigDecimal("-2.71994908488607703910e-9")
    };

    public static BigDecimal GammaLn(BigDecimal z) {
        BigDecimal half = new BigDecimal("0.5");
        if (z.compareTo(half) < 0) {
            BigDecimal s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s = s.add(GammaDk[i].divide(new BigDecimal(i).subtract(z), MathContext.DECIMAL128));
            }

            return Constants.LnPi
                    .subtract(MathEx.Log(MathEx.Sin(MathEx.PI.multiply(z))))
                    .subtract(MathEx.Log(s))
                    .subtract(Constants.LogTwoSqrtEOverPi)
                    .subtract(half.subtract(z).multiply(MathEx.Log(half.subtract(z).add(GammaR).divide(MathEx.E, MathContext.DECIMAL128))));
        } else {
            BigDecimal s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s = s.add(GammaDk[i].divide(z.add(new BigDecimal(i - 1)), MathContext.DECIMAL128));
            }

            return MathEx.Log(s)
                    .add(Constants.LogTwoSqrtEOverPi)
                    .add(z.subtract(half).multiply(MathEx.Log(z.subtract(half).add(GammaR).divide(MathEx.E, MathContext.DECIMAL128))));
        }
    }

    public static BigDecimal BetaRegularized(BigDecimal a, BigDecimal b, BigDecimal x) {
        BigDecimal zero = BigDecimal.ZERO;
        BigDecimal one = BigDecimal.ONE;

        BigDecimal bt = (x.compareTo(zero) == 0 || x.compareTo(one) == 0)
                ? zero
                : MathEx.Exp(GammaLn(a.add(b)).subtract(GammaLn(a)).subtract(GammaLn(b))
                        .add(a.multiply(MathEx.Log(x))).add(b.multiply(MathEx.Log(one.subtract(x)))));

        boolean symmetryTransformation = x.compareTo(a.add(one).divide(a.add(b).add(new BigDecimal("2")), MathContext.DECIMAL128)) >= 0;

        BigDecimal eps = Precision.DecimalPrecision;

        if (symmetryTransformation) {
            x = one.subtract(x);
            BigDecimal swap = a;
            a = b;
            b = swap;
        }

        BigDecimal qab = a.add(b);
        BigDecimal qap = a.add(one);
        BigDecimal qam = a.subtract(one);
        BigDecimal c = one;
        BigDecimal d = one.subtract(qab.multiply(x).divide(qap, MathContext.DECIMAL128));

        if (d.abs().compareTo(eps) < 0) {
            d = eps;
        }

        d = one.divide(d, MathContext.DECIMAL128);
        BigDecimal h = d;

        for (int m = 1, m2 = 2; m <= 140; m++, m2 += 2) {
            BigDecimal aa = new BigDecimal(m).multiply(b.subtract(new BigDecimal(m))).multiply(x)
                    .divide(qam.add(new BigDecimal(m2)).multiply(a.add(new BigDecimal(m2))), MathContext.DECIMAL128);
            d = one.add(aa.multiply(d));

            if (d.abs().compareTo(eps) < 0) {
                d = eps;
            }

            c = one.add(aa.divide(c, MathContext.DECIMAL128));
            if (c.abs().compareTo(eps) < 0) {
                c = eps;
            }

            d = one.divide(d, MathContext.DECIMAL128);
            h = h.multiply(d).multiply(c);

            aa = a.add(new BigDecimal(m)).negate().multiply(qab.add(new BigDecimal(m))).multiply(x)
                    .divide(a.add(new BigDecimal(m2)).multiply(qap.add(new BigDecimal(m2))), MathContext.DECIMAL128);
            d = one.add(aa.multiply(d));

            if (d.abs().compareTo(eps) < 0) {
                d = eps;
            }

            c = one.add(aa.divide(c, MathContext.DECIMAL128));
            if (c.abs().compareTo(eps) < 0) {
                c = eps;
            }

            d = one.divide(d, MathContext.DECIMAL128);
            BigDecimal del = d.multiply(c);
            h = h.multiply(del);

            if (del.subtract(one).abs().compareTo(eps) <= 0) {
                return symmetryTransformation ? one.subtract(bt.multiply(h).divide(a, MathContext.DECIMAL128))
                        : bt.multiply(h).divide(a, MathContext.DECIMAL128);
            }
        }

        return symmetryTransformation ? one.subtract(bt.multiply(h).divide(a, MathContext.DECIMAL128))
                : bt.multiply(h).divide(a, MathContext.DECIMAL128);
    }

    public static BigDecimal GammaLowerRegularized(BigDecimal a, BigDecimal x) {
        BigDecimal epsilon = new BigDecimal("0.000000000000001");
        BigDecimal big = new BigDecimal("4503599627370496.0");
        BigDecimal bigInv = new BigDecimal("2.22044604925031308085e-16");

        if (a.compareTo(BigDecimal.ZERO) == 0) {
            if (x.compareTo(BigDecimal.ZERO) == 0) {
                return BigDecimal.ONE;
            }
            return BigDecimal.ONE;
        }

        if (x.compareTo(BigDecimal.ZERO) == 0) {
            return BigDecimal.ZERO;
        }

        BigDecimal ax = a.multiply(MathEx.Log(x)).subtract(x).subtract(GammaLn(a));
        if (ax.compareTo(new BigDecimal("-709.78271289338399")) < 0) {
            return a.compareTo(x) > 0 ? BigDecimal.ONE : BigDecimal.ZERO;
        }

        if (x.compareTo(BigDecimal.ONE) <= 0 || x.compareTo(a) <= 0) {
            BigDecimal r2 = a;
            BigDecimal c2 = BigDecimal.ONE;
            BigDecimal ans2 = BigDecimal.ONE;

            do {
                r2 = r2.add(BigDecimal.ONE);
                c2 = c2.multiply(x).divide(r2, MathContext.DECIMAL128);
                ans2 = ans2.add(c2);
            } while (c2.divide(ans2, MathContext.DECIMAL128).compareTo(epsilon) > 0);

            return MathEx.Exp(ax).multiply(ans2).divide(a, MathContext.DECIMAL128);
        }

        int c = 0;
        BigDecimal y = a.subtract(BigDecimal.ONE);
        BigDecimal z = x.add(y).add(BigDecimal.ONE);

        BigDecimal p3 = BigDecimal.ONE;
        BigDecimal q3 = x;
        BigDecimal p2 = x.add(BigDecimal.ONE);
        BigDecimal q2 = z.multiply(x);
        BigDecimal ans = p2.divide(q2, MathContext.DECIMAL128);

        BigDecimal error;

        do {
            c++;
            y = y.add(BigDecimal.ONE);
            z = z.add(new BigDecimal("2"));
            BigDecimal yc = y.multiply(new BigDecimal(c));

            BigDecimal p = p2.multiply(z).subtract(p3.multiply(yc));
            BigDecimal q = q2.multiply(z).subtract(q3.multiply(yc));

            if (q.compareTo(BigDecimal.ZERO) != 0) {
                BigDecimal nextans = p.divide(q, MathContext.DECIMAL128);
                error = ans.subtract(nextans).abs().divide(nextans.abs(), MathContext.DECIMAL128);
                ans = nextans;
            } else {
                error = BigDecimal.ONE;
            }

            p3 = p2;
            p2 = p;
            q3 = q2;
            q2 = q;

            if (p.abs().compareTo(big) > 0) {
                p3 = p3.multiply(bigInv);
                p2 = p2.multiply(bigInv);
                q3 = q3.multiply(bigInv);
                q2 = q2.multiply(bigInv);
            }
        } while (error.compareTo(epsilon) > 0);

        return BigDecimal.ONE.subtract(MathEx.Exp(ax).multiply(ans));
    }

    public static BigDecimal ErfInv(BigDecimal x) {
        BigDecimal zero = BigDecimal.ZERO;
        BigDecimal one = BigDecimal.ONE;
        BigDecimal two = new BigDecimal("2");
        BigDecimal half = new BigDecimal("0.5");

        if (x.compareTo(zero) < 0 || x.compareTo(one) > 0) {
            throw new IllegalArgumentException("x must be in [0,1]");
        }

        if (x.compareTo(zero) == 0) {
            return zero;
        }
        if (x.compareTo(one) == 0) {
            return new BigDecimal("100");
        }

        BigDecimal a1 = new BigDecimal("-3.969683028665376e+01");
        BigDecimal a2 = new BigDecimal("2.209460984245205e+02");
        BigDecimal a3 = new BigDecimal("-2.759285104469687e+02");
        BigDecimal a4 = new BigDecimal("1.383577518672690e+02");
        BigDecimal a5 = new BigDecimal("-3.066479806614716e+01");
        BigDecimal a6 = new BigDecimal("2.506628277459239e+00");

        BigDecimal b1 = new BigDecimal("-5.447609879822406e+01");
        BigDecimal b2 = new BigDecimal("1.615858368580409e+02");
        BigDecimal b3 = new BigDecimal("-1.556989798598866e+02");
        BigDecimal b4 = new BigDecimal("6.680131188771972e+01");
        BigDecimal b5 = new BigDecimal("-1.328068155288572e+01");

        BigDecimal c1 = new BigDecimal("-7.784894002430293e-03");
        BigDecimal c2 = new BigDecimal("-3.223964580411365e-01");
        BigDecimal c3 = new BigDecimal("-2.400758277161838e+00");
        BigDecimal c4 = new BigDecimal("-2.549732539343734e+00");
        BigDecimal c5 = new BigDecimal("4.374664141464968e+00");
        BigDecimal c6 = new BigDecimal("2.938163982698783e+00");

        BigDecimal d1 = new BigDecimal("7.784695709041462e-03");
        BigDecimal d2 = new BigDecimal("3.224671290700398e-01");
        BigDecimal d3 = new BigDecimal("2.445134137142996e+00");
        BigDecimal d4 = new BigDecimal("3.754408661907416e+00");

        BigDecimal p = x;
        BigDecimal q;

        BigDecimal x_low = new BigDecimal("0.02425");
        BigDecimal x_high = one.subtract(x_low);

        if (p.compareTo(x_low) <= 0) {
            q = MathEx.Sqrt(two.multiply(MathEx.Log(p)));
            BigDecimal num = c1.multiply(q).pow(5).add(c2.multiply(q).pow(4)).add(c3.multiply(q).pow(3))
                    .add(c4.multiply(q).pow(2)).add(c5.multiply(q)).add(c6);
            BigDecimal den = d1.multiply(q).pow(5).add(d2.multiply(q).pow(4)).add(d3.multiply(q).pow(3))
                    .add(d4.multiply(q).pow(2)).add(one).multiply(q).add(one);
            return num.divide(den, MathContext.DECIMAL128).negate();
        } else if (p.compareTo(x_high) < 0) {
            q = p.subtract(half).multiply(two);
            BigDecimal num = a1.multiply(q).pow(5).add(a2.multiply(q).pow(4)).add(a3.multiply(q).pow(3))
                    .add(a4.multiply(q).pow(2)).add(a5.multiply(q)).add(a6);
            BigDecimal den = b1.multiply(q).pow(5).add(b2.multiply(q).pow(4)).add(b3.multiply(q).pow(3))
                    .add(b4.multiply(q).pow(2)).add(b5.multiply(q)).add(one);
            return num.divide(den, MathContext.DECIMAL128);
        } else {
            q = MathEx.Sqrt(two.multiply(MathEx.Log(one.subtract(p))));
            BigDecimal num = c1.multiply(q).pow(5).add(c2.multiply(q).pow(4)).add(c3.multiply(q).pow(3))
                    .add(c4.multiply(q).pow(2)).add(c5.multiply(q)).add(c6);
            BigDecimal den = d1.multiply(q).pow(5).add(d2.multiply(q).pow(4)).add(d3.multiply(q).pow(3))
                    .add(d4.multiply(q).pow(2)).add(one).multiply(q).add(one);
            return num.divide(den, MathContext.DECIMAL128);
        }
    }

    public static BigDecimal GammaLowerRegularizedInv(BigDecimal a, BigDecimal y0) {
        BigDecimal epsilon = new BigDecimal("0.000000000000001");
        BigDecimal big = new BigDecimal("4503599627370496.0");
        BigDecimal threshold = new BigDecimal("5").multiply(epsilon);

        if (y0.compareTo(BigDecimal.ZERO) == 0) {
            return BigDecimal.ZERO;
        }

        if (y0.compareTo(BigDecimal.ONE) == 0) {
            return new BigDecimal("79228162514264337593543950335");
        }

        y0 = BigDecimal.ONE.subtract(y0);

        BigDecimal xUpper = big;
        BigDecimal xLower = BigDecimal.ZERO;
        BigDecimal yUpper = BigDecimal.ONE;
        BigDecimal yLower = BigDecimal.ZERO;

        BigDecimal d = BigDecimal.ONE.divide(new BigDecimal("9").multiply(a), MathContext.DECIMAL128);
        BigDecimal y = BigDecimal.ONE.subtract(d).subtract(new BigDecimal("0.98").multiply(Constants.Sqrt2)
                .multiply(ErfInv(new BigDecimal("2").multiply(y0).subtract(BigDecimal.ONE)))
                .multiply(MathEx.Sqrt(d)));
        BigDecimal x = a.multiply(y).multiply(y).multiply(y);
        BigDecimal lgm = GammaLn(a);

        for (int i = 0; i < 10; i++) {
            if (x.compareTo(xLower) < 0 || x.compareTo(xUpper) > 0) {
                d = new BigDecimal("0.0625");
                break;
            }

            y = BigDecimal.ONE.subtract(GammaLowerRegularized(a, x));
            if (y.compareTo(yLower) < 0 || y.compareTo(yUpper) > 0) {
                d = new BigDecimal("0.0625");
                break;
            }

            if (y.compareTo(y0) < 0) {
                xUpper = x;
                yLower = y;
            } else {
                xLower = x;
                yUpper = y;
            }

            d = a.subtract(BigDecimal.ONE).multiply(MathEx.Log(x)).subtract(x).subtract(lgm);
            if (d.compareTo(new BigDecimal("-709.78271289338399")) < 0) {
                d = new BigDecimal("0.0625");
                break;
            }

            d = MathEx.Exp(d.negate());
            d = y.subtract(y0).divide(d, MathContext.DECIMAL128);
            if (d.divide(x, MathContext.DECIMAL128).abs().compareTo(epsilon) < 0) {
                return x;
            }

            if (d.compareTo(x.divide(new BigDecimal("4"), MathContext.DECIMAL128)) > 0 && y0.compareTo(new BigDecimal("0.05")) < 0) {
                d = x.divide(new BigDecimal("10"), MathContext.DECIMAL128);
            }

            x = x.subtract(d);
        }

        if (xUpper.compareTo(big) == 0) {
            if (x.compareTo(BigDecimal.ZERO) <= 0) {
                x = BigDecimal.ONE;
            }

            while (xUpper.compareTo(big) == 0) {
                x = BigDecimal.ONE.add(d).multiply(x);
                y = BigDecimal.ONE.subtract(GammaLowerRegularized(a, x));
                if (y.compareTo(y0) < 0) {
                    xUpper = x;
                    yLower = y;
                    break;
                }

                d = d.add(d);
            }
        }

        int dir = 0;
        d = new BigDecimal("0.5");
        for (int i = 0; i < 400; i++) {
            x = xLower.add(d.multiply(xUpper.subtract(xLower)));
            y = BigDecimal.ONE.subtract(GammaLowerRegularized(a, x));
            lgm = xUpper.subtract(xLower).divide(xLower.add(xUpper), MathContext.DECIMAL128);
            if (lgm.abs().compareTo(threshold) < 0) {
                return x;
            }

            lgm = y.subtract(y0).divide(y0, MathContext.DECIMAL128);
            if (lgm.abs().compareTo(threshold) < 0) {
                return x;
            }

            if (x.compareTo(BigDecimal.ZERO) <= 0) {
                return BigDecimal.ZERO;
            }

            if (y.compareTo(y0) >= 0) {
                xLower = x;
                yUpper = y;
                if (dir < 0) {
                    dir = 0;
                    d = new BigDecimal("0.5");
                } else {
                    if (dir > 1) {
                        d = new BigDecimal("0.5").multiply(d).add(new BigDecimal("0.5"));
                    } else {
                        d = y0.subtract(yLower).divide(yUpper.subtract(yLower), MathContext.DECIMAL128);
                    }
                }

                dir = dir + 1;
            } else {
                xUpper = x;
                yLower = y;
                if (dir > 0) {
                    dir = 0;
                    d = new BigDecimal("0.5");
                } else {
                    if (dir < -1) {
                        d = new BigDecimal("0.5").multiply(d);
                    } else {
                        d = y0.subtract(yLower).divide(yUpper.subtract(yLower), MathContext.DECIMAL128);
                    }
                }

                dir = dir - 1;
            }
        }

        return x;
    }

    public static BigDecimal Gamma(BigDecimal z) {
        BigDecimal half = new BigDecimal("0.5");
        if (z.compareTo(half) < 0) {
            BigDecimal s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s = s.add(GammaDk[i].divide(new BigDecimal(i).subtract(z), MathContext.DECIMAL128));
            }

            return MathEx.PI.divide(MathEx.Sin(MathEx.PI.multiply(z))
                    .multiply(s)
                    .multiply(Constants.TwoSqrtEOverPi)
                    .multiply(MathEx.Pow(half.subtract(z).add(GammaR).divide(MathEx.E, MathContext.DECIMAL128), half.subtract(z))), MathContext.DECIMAL128);
        } else {
            BigDecimal s = GammaDk[0];
            for (int i = 1; i <= GammaN; i++) {
                s = s.add(GammaDk[i].divide(z.add(new BigDecimal(i - 1)), MathContext.DECIMAL128));
            }

            return s.multiply(Constants.TwoSqrtEOverPi)
                    .multiply(MathEx.Pow(z.subtract(half).add(GammaR).divide(MathEx.E, MathContext.DECIMAL128), z.subtract(half)));
        }
    }

    public static BigDecimal Erf(BigDecimal x) {
        BigDecimal one = BigDecimal.ONE;
        BigDecimal two = new BigDecimal("2");
        BigDecimal a1 = new BigDecimal("0.254829592");
        BigDecimal a2 = new BigDecimal("-0.284496736");
        BigDecimal a3 = new BigDecimal("1.421413741");
        BigDecimal a4 = new BigDecimal("-1.453152027");
        BigDecimal a5 = new BigDecimal("1.061405429");
        BigDecimal p = new BigDecimal("0.3275911");

        BigDecimal sign = x.compareTo(BigDecimal.ZERO) < 0 ? one.negate() : one;
        x = x.abs();

        BigDecimal t = one.divide(one.add(p.multiply(x)), MathContext.DECIMAL128);
        BigDecimal y = one.subtract(((((a5.multiply(t).add(a4.multiply(t))).multiply(t)).add(a3.multiply(t))).multiply(t)).add(a2.multiply(t)).multiply(t)).add(a1.multiply(t)).multiply(Expi(x.negate().multiply(x)));

        return sign.multiply(y);
    }

    public static BigDecimal Erfc(BigDecimal x) {
        return BigDecimal.ONE.subtract(Erf(x));
    }

    public static BigDecimal Expi(BigDecimal x) {
        BigDecimal one = BigDecimal.ONE;
        BigDecimal zero = BigDecimal.ZERO;
        BigDecimal epsilon = new BigDecimal("1e-16");
        
        if (x.compareTo(zero) == 0) {
            return zero;
        }
        
        if (x.compareTo(zero) > 0) {
            BigDecimal sum = one;
            BigDecimal term = one;
            BigDecimal n = one;
            
            while (term.abs().compareTo(epsilon) > 0) {
                term = term.multiply(x).divide(n, MathContext.DECIMAL128);
                sum = sum.add(term);
                n = n.add(one);
            }
            
            return sum;
        } else {
            BigDecimal sum = one.negate();
            BigDecimal term = one.negate();
            BigDecimal n = one;
            
            while (term.abs().compareTo(epsilon) > 0) {
                term = term.multiply(x).divide(n, MathContext.DECIMAL128);
                sum = sum.add(term);
                n = n.add(one);
            }
            
            return sum;
        }
    }
}
