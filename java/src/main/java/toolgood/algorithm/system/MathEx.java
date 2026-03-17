package toolgood.algorithm.system;

import java.math.BigDecimal;
import java.math.MathContext;

public class MathEx {
    public static final BigDecimal E = new BigDecimal("2.7182818284590452353602874713526624977572470936999595749");

    public static final BigDecimal Epsilon = new BigDecimal("0.0000000000000000001");

    public static final BigDecimal One = new BigDecimal("1.0");

    public static final BigDecimal PI = new BigDecimal("3.14159265358979323846264338327950288419716939937510");

    public static final BigDecimal Zero = new BigDecimal("0.0");

    private static final BigDecimal EInverted = new BigDecimal("0.3678794411714423215955237701614608674458111310317678");

    private static final BigDecimal Half = new BigDecimal("0.5");

    private static final BigDecimal Log10Inv = new BigDecimal("0.434294481903251827651128918916605082294397005803666566114");

    private static final int MaximumIterations = 100;

    private static final BigDecimal HalfPi = new BigDecimal("1.570796326794896619231321691639751442098584699687552910487");

    private static final BigDecimal QuarterPi = new BigDecimal("0.785398163397448309615660845819875721049292349843776455243");

    private static final BigDecimal TwoPi = new BigDecimal("6.28318530717958647692528676655900576839433879875021");

    public static BigDecimal Acos(BigDecimal x) {
        if (x.compareTo(Zero) == 0) {
            return HalfPi;
        }
        if (x.compareTo(One) == 0) {
            return Zero;
        }

        if (x.compareTo(Zero) < 0) {
            return PI.subtract(Acos(x.negate()));
        }

        return HalfPi.subtract(Asin(x));
    }

    public static BigDecimal Acosh(BigDecimal x) {
        if (x.compareTo(One) < 0) {
            throw new IllegalArgumentException("x must be >= 1");
        }
        return Log(x.add(Sqrt(x.multiply(x).subtract(One))));
    }

    public static BigDecimal Asin(BigDecimal x) {
        if (x.compareTo(One) > 0 || x.compareTo(One.negate()) < 0) {
            throw new IllegalArgumentException("x must be in [-1,1]");
        }

        if (x.compareTo(Zero) == 0) {
            return Zero;
        }
        if (x.compareTo(One) == 0) {
            return HalfPi;
        }

        if (x.compareTo(Zero) < 0) {
            return Asin(x.negate()).negate();
        }

        BigDecimal newX = One.subtract(x.multiply(x).multiply(new BigDecimal("2")));

        if (x.abs().compareTo(newX.abs()) > 0) {
            BigDecimal t = Asin(newX);
            return Half.multiply(HalfPi.subtract(t));
        }

        BigDecimal y = Zero;
        BigDecimal result = x;
        BigDecimal cachedResult;
        int i = 1;
        y = y.add(result);
        BigDecimal xx = x.multiply(x);
        do {
            cachedResult = result;
            result = result.multiply(xx).multiply(One.subtract(Half.divide(new BigDecimal(i), MathContext.DECIMAL128)));
            y = y.add(result.divide(new BigDecimal(2 * i + 1), MathContext.DECIMAL128));
            i++;
        } while (cachedResult.compareTo(result) != 0);

        return y;
    }

    public static BigDecimal Asinh(BigDecimal x) {
        return Log(x.add(Sqrt(x.multiply(x).add(One))));
    }

    public static BigDecimal Atan(BigDecimal x) {
        if (x.compareTo(Zero) == 0) {
            return Zero;
        }
        if (x.compareTo(One) == 0) {
            return QuarterPi;
        }
        return Asin(x.divide(Sqrt(One.add(x.multiply(x))), MathContext.DECIMAL128));
    }

    public static BigDecimal Atan2(BigDecimal y, BigDecimal x) {
        if (x.compareTo(Zero) > 0) {
            return Atan(y.divide(x, MathContext.DECIMAL128));
        }

        if (x.compareTo(Zero) < 0 && y.compareTo(Zero) >= 0) {
            return Atan(y.divide(x, MathContext.DECIMAL128)).add(PI);
        }

        if (x.compareTo(Zero) < 0 && y.compareTo(Zero) < 0) {
            return Atan(y.divide(x, MathContext.DECIMAL128)).subtract(PI);
        }

        if (x.compareTo(Zero) == 0 && y.compareTo(Zero) > 0) {
            return HalfPi;
        }
        if (x.compareTo(Zero) == 0 && y.compareTo(Zero) < 0) {
            return HalfPi.negate();
        }

        throw new IllegalArgumentException("invalid atan2 arguments");
    }

    public static BigDecimal Atanh(BigDecimal x) {
        if (x.abs().compareTo(One) >= 0) {
            throw new IllegalArgumentException("x must be |x|<1");
        }
        return Log(One.add(x).divide(One.subtract(x), MathContext.DECIMAL128)).multiply(Half);
    }

    public static BigDecimal Cos(BigDecimal x) {
        BigDecimal xCopy = x;
        while (xCopy.compareTo(TwoPi) > 0) {
            xCopy = xCopy.subtract(TwoPi);
        }

        while (xCopy.compareTo(TwoPi.negate()) < 0) {
            xCopy = xCopy.add(TwoPi);
        }

        if (xCopy.compareTo(PI) >= 0 && xCopy.compareTo(TwoPi) <= 0) {
            return Cos(xCopy.subtract(PI)).negate();
        }

        if (xCopy.compareTo(TwoPi.negate()) >= 0 && xCopy.compareTo(PI.negate()) <= 0) {
            return Cos(xCopy.add(PI)).negate();
        }

        BigDecimal xx = xCopy.multiply(xCopy);

        BigDecimal yy = xx.multiply(Half.negate());
        BigDecimal y = One.add(yy);
        BigDecimal cachedY = y.subtract(One);
        for (int i = 1; cachedY.compareTo(y) != 0 && i < MaximumIterations; i++) {
            cachedY = y;

            BigDecimal factor = new BigDecimal(i * (i + i + 3) + 1);
            factor = Half.divide(factor, MathContext.DECIMAL128).negate();
            xx = xx.multiply(xCopy).multiply(xCopy).multiply(factor);
            y = y.add(xx);
        }

        return y;
    }

    public static BigDecimal Cosh(BigDecimal x) {
        BigDecimal y = Exp(x);
        BigDecimal yy = One.divide(y, MathContext.DECIMAL128);
        return y.add(yy).multiply(Half);
    }

    public static BigDecimal Exp(BigDecimal x) {
        int count = 0;
        BigDecimal xCopy = x;
        while (xCopy.compareTo(One) > 0) {
            xCopy = xCopy.subtract(One);
            count++;
        }

        while (xCopy.compareTo(Zero) < 0) {
            xCopy = xCopy.add(One);
            count--;
        }

        int iteration = 1;
        BigDecimal result = One;
        BigDecimal factor = One;
        BigDecimal cachedResult;
        do {
            cachedResult = result;
            factor = factor.multiply(xCopy.divide(new BigDecimal(iteration++), MathContext.DECIMAL128));
            result = result.add(factor);
        } while (cachedResult.compareTo(result) != 0);

        if (count != 0) {
            result = result.multiply(PowerN(E, count));
        }

        return result;
    }

    public static BigDecimal Log(BigDecimal x) {
        if (x.compareTo(Zero) <= 0) {
            throw new IllegalArgumentException("x must be greater than zero");
        }

        int count = 0;
        BigDecimal xCopy = x;
        while (xCopy.compareTo(One) >= 0) {
            xCopy = xCopy.multiply(EInverted);
            count++;
        }

        while (xCopy.compareTo(EInverted) <= 0) {
            xCopy = xCopy.multiply(E);
            count--;
        }

        xCopy = xCopy.subtract(One);

        if (xCopy.compareTo(Zero) == 0) {
            return new BigDecimal(count);
        }

        BigDecimal result = Zero;
        int iteration = 0;
        BigDecimal y = One;
        BigDecimal cacheResult = result.subtract(One);
        while (cacheResult.compareTo(result) != 0 && iteration < MaximumIterations) {
            iteration++;
            cacheResult = result;
            y = y.multiply(xCopy.negate());
            result = result.add(y.divide(new BigDecimal(iteration), MathContext.DECIMAL128));
        }

        return new BigDecimal(count).subtract(result);
    }

    public static BigDecimal Log(BigDecimal d, BigDecimal newBase) {
        if (d.compareTo(One) == 0) {
            return Zero;
        }

        if (newBase.compareTo(One) == 0) {
            throw new IllegalArgumentException("Logarithm for base 1 is undefined.");
        }
        if (d.compareTo(Zero) < 0) {
            throw new IllegalArgumentException("Logarithm is a complex number for values less than zero!");
        }
        if (d.compareTo(Zero) == 0) {
            throw new ArithmeticException("Logarithm is defined as negative infinity at zero which the Decimal data type can't represent!");
        }
        if (newBase.compareTo(Zero) < 0) {
            throw new IllegalArgumentException("Logarithm base would be a complex number for values less than zero!");
        }
        if (newBase.compareTo(Zero) == 0) {
            throw new ArithmeticException("Logarithm base would be negative infinity at zero which the Decimal data type can't represent!");
        }

        return Log(d).divide(Log(newBase), MathContext.DECIMAL128);
    }

    public static BigDecimal Log10(BigDecimal x) {
        return Log(x).multiply(Log10Inv);
    }

    public static BigDecimal Pow(BigDecimal value, BigDecimal pow) {
        if (pow.compareTo(Zero) == 0) {
            return One;
        }
        if (pow.compareTo(One) == 0) {
            return value;
        }

        if (value.compareTo(One) == 0) {
            return One;
        }
        if (value.compareTo(Zero) == 0 && pow.compareTo(Zero) == 0) {
            return One;
        }
        if (value.compareTo(Zero) == 0 && pow.compareTo(Zero) > 0) {
            return Zero;
        }
        if (value.compareTo(Zero) == 0) {
            throw new ArithmeticException("Invalid Operation: zero base and negative power");
        }

        if (pow.compareTo(One.negate()) == 0) {
            return One.divide(value, MathContext.DECIMAL128);
        }

        boolean isPowerInteger = IsInteger(pow);
        if (value.compareTo(Zero) < 0 && !isPowerInteger) {
            throw new ArithmeticException("Invalid Operation: negative base and non-integer power");
        }

        if (isPowerInteger && value.compareTo(Zero) > 0) {
            int powerInt = pow.intValue();
            return PowerN(value, powerInt);
        }

        if (!isPowerInteger || value.compareTo(Zero) >= 0) {
            return Exp(pow.multiply(Log(value)));
        }

        int powerInt2 = pow.intValue();
        if (powerInt2 % 2 == 0) {
            return Exp(pow.multiply(Log(value.negate())));
        }

        return Exp(pow.multiply(Log(value.negate()))).negate();
    }

    public static BigDecimal PowerN(BigDecimal value, int power) {
        while (true) {
            if (power == 0) {
                return One;
            }

            if (power < 0) {
                value = One.divide(value, MathContext.DECIMAL128);
                power = -power;
                continue;
            }

            int q = power;
            BigDecimal prod = One;
            BigDecimal current = value;

            while (q > 0) {
                if (q % 2 == 1) {
                    prod = current.multiply(prod);
                    q--;
                }

                current = current.multiply(current);
                q /= 2;
            }

            return prod;
        }
    }

    public static BigDecimal Sin(BigDecimal x) {
        BigDecimal cos = Cos(x);
        BigDecimal moduleOfSin = Sqrt(One.subtract(cos.multiply(cos)));
        boolean sineIsPositive = IsSignOfSinusPositive(x);
        if (sineIsPositive) {
            return moduleOfSin;
        }

        return moduleOfSin.negate();
    }

    public static BigDecimal Sinh(BigDecimal x) {
        BigDecimal y = Exp(x);
        BigDecimal yy = One.divide(y, MathContext.DECIMAL128);
        return y.subtract(yy).multiply(Half);
    }

    public static BigDecimal Sqrt(BigDecimal x) {
        return Sqrt(x, Zero);
    }

    public static BigDecimal Sqrt(BigDecimal x, BigDecimal epsilon) {
        if (x.compareTo(Zero) < 0) {
            throw new ArithmeticException("Cannot calculate square root from a negative number");
        }

        BigDecimal current = BigDecimal.valueOf(Math.sqrt(x.doubleValue()));
        BigDecimal previous;
        do {
            previous = current;
            if (previous.compareTo(Zero) == 0) {
                return Zero;
            }

            current = previous.add(x.divide(previous, MathContext.DECIMAL128)).multiply(Half);
        } while (previous.subtract(current).abs().compareTo(epsilon) > 0);

        return current;
    }

    public static BigDecimal Tan(BigDecimal x) {
        BigDecimal cos = Cos(x);
        if (cos.compareTo(Zero) == 0) {
            throw new IllegalArgumentException("x");
        }

        return Sin(x).divide(cos, MathContext.DECIMAL128);
    }

    public static BigDecimal Tanh(BigDecimal x) {
        BigDecimal y = Exp(x);
        BigDecimal yy = One.divide(y, MathContext.DECIMAL128);
        return y.subtract(yy).divide(y.add(yy), MathContext.DECIMAL128);
    }

    private static boolean IsInteger(BigDecimal x) {
        long longValue = x.longValue();
        return x.subtract(BigDecimal.valueOf(longValue)).abs().compareTo(Epsilon) <= 0;
    }

    private static boolean IsSignOfSinusPositive(BigDecimal x) {
        BigDecimal xCopy = x;
        while (xCopy.compareTo(TwoPi) >= 0) {
            xCopy = xCopy.subtract(TwoPi);
        }

        while (xCopy.compareTo(TwoPi.negate()) <= 0) {
            xCopy = xCopy.add(TwoPi);
        }

        if (xCopy.compareTo(TwoPi.negate()) >= 0 && xCopy.compareTo(PI.negate()) <= 0) {
            return true;
        }

        if (xCopy.compareTo(PI.negate()) >= 0 && xCopy.compareTo(Zero) <= 0) {
            return false;
        }

        if (xCopy.compareTo(Zero) >= 0 && xCopy.compareTo(PI) <= 0) {
            return true;
        }

        if (xCopy.compareTo(PI) >= 0 && xCopy.compareTo(TwoPi) <= 0) {
            return false;
        }

        throw new IllegalArgumentException("x");
    }
}
