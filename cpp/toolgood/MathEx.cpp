// MathEx.cpp - 逐段移植 csharp/ToolGood.Algorithm/System/MathEx.cs
// 说明：
//   - 常量按 C# decimal 字面量的实际取值（已舍入到 28 位小数）书写，
//     不可直接写源码里的长字面量，否则会多出 C# 无法表示的位数；
//   - C# 的 decimal 运算溢出演变成 std::overflow_error，定义域非法演变成 std::domain_error；
//   - 除法按 C# System.Decimal 的语义（最大标度 28、96 位尾数、HALF_EVEN）逐位计算，
//     避免用本仓库 34 位除法的结果二次舍入导致末位偏差。
#include "MathEx.h"

#include <cmath>
#include <limits>
#include <stdexcept>

namespace toolgood {
namespace MathEx {

namespace {

/// C# MathEx.Epsilon
const Decimal& Epsilon() {
    static const Decimal v = Decimal::FromString("0.0000000000000000001");
    return v;
}

/// C# MathEx.Half
const Decimal& Half() {
    static const Decimal v = Decimal::FromString("0.5");
    return v;
}

/// C# MathEx.EInverted = 1/E
const Decimal& EInverted() {
    static const Decimal v = Decimal::FromString("0.3678794411714423215955237702");
    return v;
}

/// C# MathEx.Log10Inv = log(10,E)
const Decimal& Log10Inv() {
    static const Decimal v = Decimal::FromString("0.4342944819032518276511289189");
    return v;
}

/// C# MathEx.HalfPi
const Decimal& HalfPi() {
    static const Decimal v = Decimal::FromString("1.5707963267948966192313216916");
    return v;
}

/// C# MathEx.QuarterPi
const Decimal& QuarterPi() {
    static const Decimal v = Decimal::FromString("0.7853981633974483096156608458");
    return v;
}

/// C# MathEx.TwoPi
const Decimal& TwoPi() {
    static const Decimal v = Decimal::FromString("6.2831853071795864769252867666");
    return v;
}

/// C# MathEx.Ln2
const Decimal& Ln2() {
    static const Decimal v = Decimal::FromString("0.6931471805599453094172321215");
    return v;
}

/// C# MathEx.SquareSafeMax：x*x 仍不溢出的上限
const Decimal& SquareSafeMax() {
    static const Decimal v = Decimal::FromString("100000000000000");
    return v;
}

/// C# MathEx.HyperbolicSafeMax
const Decimal& HyperbolicSafeMax() {
    static const Decimal v = Decimal::FromInt(60);
    return v;
}

/// C# MathEx.MaximumIterations
const int kMaximumIterations = 100;

Decimal Int(int64_t v) { return Decimal::FromInt(v); }

/// decimal 的 96 位尾数上限
const BigInt& MaxUnscaled96() {
    static const BigInt v = BigInt::FromString("79228162514264337593543950335");
    return v;
}

/// round(num/den)，HALF_EVEN（对齐 C# decimal 的舍入）
BigInt RoundRatHalfEven(const BigInt& num, const BigInt& den) {
    BigInt rem;
    BigInt q = num.QuoRem(den, rem);
    if (!rem.IsZero()) {
        const BigInt twice = rem.Abs().Mul(BigInt(2));
        const int cmp = twice.Cmp(den.Abs());
        if (cmp > 0 || (cmp == 0 && q.IsOdd())) {
            q = num.IsNegative() ? q.Sub(BigInt(1)) : q.Add(BigInt(1));
        }
    }
    return q;
}

/// 一次性把 unscaled/10^scale 归一到 C# decimal 的表示范围
/// （标度 <= 28、尾数 <= 96 位，HALF_EVEN；溢出抛 overflow_error）。
/// 与 Decimal 内部逐位舍入不同，这里一次舍入到位，避免级数运算累积末位偏差。
Decimal NormalizeOnce(BigInt unscaled, int32_t scale) {
    if (unscaled.IsZero()) return Decimal();
    if (scale < 0) {
        unscaled = unscaled.MulPow10(-scale);
        scale = 0;
    }

    // 溢出判定：|unscaled| > Max * 10^scale 即超出 decimal 范围
    if (unscaled.Abs().Cmp(MaxUnscaled96().MulPow10(scale)) > 0) {
        throw std::overflow_error("decimal overflow");
    }

    int drop = scale - 28;
    if (drop < 0) drop = 0;
    const int digits = unscaled.DigitCount();
    if (digits - drop > 29) drop = digits - 29;
    if (drop > 0) {
        unscaled = RoundRatHalfEven(unscaled, BigInt::Pow10(drop));
        scale -= drop;
    }
    // 29 位仍可能超过 96 位上限（如 8e28），再舍入一位
    while (scale > 0 && unscaled.Abs().Cmp(MaxUnscaled96()) > 0) {
        unscaled = RoundRatHalfEven(unscaled, BigInt(10));
        scale--;
    }
    return Decimal(unscaled, scale);
}

/// 加法（C# decimal 语义）
Decimal Add(const Decimal& a, const Decimal& b) {
    const int32_t scale = a.Scale() > b.Scale() ? a.Scale() : b.Scale();
    const BigInt x = a.Unscaled().MulPow10(scale - a.Scale());
    const BigInt y = b.Unscaled().MulPow10(scale - b.Scale());
    return NormalizeOnce(x.Add(y), scale);
}

/// 减法（C# decimal 语义）
Decimal Sub(const Decimal& a, const Decimal& b) {
    const int32_t scale = a.Scale() > b.Scale() ? a.Scale() : b.Scale();
    const BigInt x = a.Unscaled().MulPow10(scale - a.Scale());
    const BigInt y = b.Unscaled().MulPow10(scale - b.Scale());
    return NormalizeOnce(x.Sub(y), scale);
}

/// 乘法（C# decimal 语义）
Decimal Mul(const Decimal& a, const Decimal& b) {
    return NormalizeOnce(a.Unscaled().Mul(b.Unscaled()), a.Scale() + b.Scale());
}

/// 对应 C# decimal 的除法：结果取"尾数不超过 96 位、标度不超过 28"的最大标度，
/// 舍入为 HALF_EVEN，并去掉末尾的 0（与 C# 的最小标度表示一致）。
Decimal DivImpl(const Decimal& a, const Decimal& b) {
    if (b.IsZero() || a.IsZero()) return Decimal::Zero;

    const BigInt num = a.Unscaled().Abs();
    const BigInt den = b.Unscaled().Abs();
    const int exp10 = b.Scale() - a.Scale();

    for (int s = 28; s >= 0; s--) {
        const int shift = s + exp10;
        const BigInt q = shift >= 0 ? RoundRatHalfEven(num.MulPow10(shift), den)
                                    : RoundRatHalfEven(num, den.MulPow10(-shift));
        if (q.Abs().Cmp(MaxUnscaled96()) > 0) continue;

        BigInt unscaled = q;
        int32_t scale = s;
        while (scale > 0) {
            BigInt rem;
            const BigInt q10 = unscaled.QuoRem(BigInt(10), rem);
            if (!rem.IsZero()) break;
            unscaled = q10;
            scale--;
        }
        if ((a.Sign() < 0) != (b.Sign() < 0)) unscaled = unscaled.Neg();
        return Decimal(unscaled, scale);
    }

    // 商本身超出 decimal 范围（C# 抛 OverflowException）
    throw std::overflow_error("decimal overflow");
}

/// 对应 C# MathEx.IsInteger：先把 x 截断成 long（越界抛 OverflowException），
/// 再判断 |x - longValue| <= Epsilon。
bool IsInteger(const Decimal& x) {
    static const Decimal kLongMax = Decimal::FromString("9223372036854775807");
    static const Decimal kLongMin = Decimal::FromString("-9223372036854775808");
    if (x.Greater(kLongMax) || x.Less(kLongMin)) throw std::overflow_error("decimal overflow");
    const int64_t longValue = x.Trunc().Int64();
    return x.Sub(Decimal::FromInt(longValue)).Abs().LessOrEqual(Epsilon());
}

}  // namespace

const Decimal& E() {
    static const Decimal v = Decimal::FromString("2.7182818284590452353602874714");
    return v;
}

const Decimal& PI() {
    static const Decimal v = Decimal::FromString("3.1415926535897932384626433833");
    return v;
}

Decimal Div(const Decimal& a, const Decimal& b) { return DivImpl(a, b); }

Decimal Exp(const Decimal& value) {
    // O(1) 归约：一次取出整数部分
    const Decimal integral = value.Floor();
    const Decimal x = Sub(value, integral);

    // decimal 的取值范围远大于 int，整数部分超出 int 时 E^count 必然溢出或下溢
    if (integral.Greater(Int(std::numeric_limits<int>::max()))) {
        throw std::overflow_error("decimal overflow");
    }
    if (integral.Less(Int(std::numeric_limits<int>::min()))) {
        return Decimal::Zero;
    }
    const int count = integral.Int();

    int iteration = 1;
    Decimal result = Decimal::One;
    Decimal factor = Decimal::One;
    Decimal cachedResult;
    do {
        cachedResult = result;
        factor = Mul(factor, Div(x, Int(iteration++)));
        result = Add(result, factor);
    } while (!cachedResult.Equal(result) && iteration < kMaximumIterations);

    if (count != 0) {
        result = Mul(result, PowerN(E(), count));
    }

    return result;
}

Decimal Log(const Decimal& value) {
    if (!value.IsPositive()) {
        throw std::domain_error("x must be greater than zero");
    }
    // ln(1)=0。必须短路：归约中 EInverted*E 的舍入误差会把 x 变成 1-1e-28
    if (value.Equal(Decimal::One)) {
        return Decimal::Zero;
    }

    Decimal x = value;
    // O(1) 归约：先用 double 对数估算数量级，再用下面的循环做微调
    int count = static_cast<int>(std::floor(std::log(x.Float64())));
    if (count > 0) {
        x = Div(x, PowerN(E(), count));
    } else if (count < 0) {
        x = Mul(x, PowerN(E(), -count));
    }

    while (x.GreaterOrEqual(Decimal::One)) {
        x = Mul(x, EInverted());
        count++;
    }

    while (x.LessOrEqual(EInverted())) {
        x = Mul(x, E());
        count--;
    }

    x = Sub(x, Decimal::One);

    if (x.IsZero()) {
        return Int(count);
    }

    Decimal result = Decimal::Zero;
    int iteration = 0;
    Decimal y = Decimal::One;
    Decimal cacheResult = Sub(result, Decimal::One);
    while (!cacheResult.Equal(result) && iteration < kMaximumIterations) {
        iteration++;
        cacheResult = result;
        y = Mul(y, x.Neg());
        result = Add(result, Div(y, Int(iteration)));
    }

    return Sub(Int(count), result);
}

Decimal Log(const Decimal& d, const Decimal& newBase) {
    // d 为 1 时直接返回 0，底数非法时语义与 C# 一致
    if (d.Equal(Decimal::One)) return Decimal::Zero;

    if (newBase.Equal(Decimal::One)) throw std::domain_error("Logarithm for base 1 is undefined.");
    if (d.IsNegative()) throw std::domain_error("Logarithm is a complex number for values less than zero!");
    if (d.IsZero()) throw std::overflow_error("Logarithm is defined as negative infinity at zero!");
    if (newBase.IsNegative()) throw std::domain_error("Logarithm base would be a complex number for values less than zero!");
    if (newBase.IsZero()) throw std::overflow_error("Logarithm base would be negative infinity at zero!");

    return Div(Log(d), Log(newBase));
}

Decimal Log10(const Decimal& x) { return Mul(Log(x), Log10Inv()); }

Decimal PowerN(const Decimal& value, int power) {
    Decimal base = value;
    int e = power;
    for (;;) {
        if (e == 0) return Decimal::One;
        if (e < 0) {
            base = Div(Decimal::One, base);
            e = -e;
            continue;
        }

        int q = e;
        Decimal prod = Decimal::One;
        Decimal current = base;

        while (q > 0) {
            if (q % 2 == 1) {
                // 取出指数二进制中的 1
                prod = Mul(current, prod);
                q--;
                if (q == 0) {
                    // 防止最后一次无谓平方导致溢出
                    break;
                }
            }
            // value^i -> value^(2*i)
            current = Mul(current, current);
            q /= 2;
        }

        return prod;
    }
}

Decimal Pow(const Decimal& value, const Decimal& pow) {
    if (pow.IsZero()) return Decimal::One;
    if (pow.Equal(Decimal::One)) return value;

    if (value.Equal(Decimal::One)) return Decimal::One;
    if (value.IsZero()) {
        if (pow.Greater(Decimal::Zero)) return Decimal::Zero;
        throw std::domain_error("Invalid Operation: zero base and negative power");
    }

    if (pow.Equal(Decimal::One.Neg())) return Div(Decimal::One, value);

    const bool isPowerInteger = IsInteger(pow);
    if (value.IsNegative() && !isPowerInteger) {
        throw std::domain_error("Invalid Operation: negative base and non-integer power");
    }

    if (isPowerInteger && value.IsPositive()) {
        // C# 的 (int)pow 转换，越界抛 OverflowException
        if (!pow.FitsInt32()) throw std::overflow_error("decimal overflow");
        return PowerN(value, pow.Int());
    }

    if (!isPowerInteger || !value.IsNegative()) {
        return Exp(Mul(pow, Log(value)));
    }

    // C# 的 (int)pow 转换，越界抛 OverflowException
    if (!pow.FitsInt32()) throw std::overflow_error("decimal overflow");
    const int powerInt2 = pow.Int();
    const Decimal positive = Exp(Mul(pow, Log(value.Neg())));
    return powerInt2 % 2 == 0 ? positive : positive.Neg();
}

Decimal Sqrt(const Decimal& x) {
    if (x.IsNegative()) {
        throw std::overflow_error("Cannot calculate square root from a negative number");
    }
    return x.Sqrt();
}

Decimal Sin(const Decimal& value) {
    Decimal x = value;
    // O(1) 归约到 [-π,π]
    if (x.Greater(PI()) || x.Less(PI().Neg())) {
        x = Sub(x, Mul(Div(Add(x, PI()), TwoPi()).Floor(), TwoPi()));
    }

    // 利用 sin(π-x)=sin(x) 把参数压到 [-π/2,π/2]
    if (x.Greater(HalfPi())) {
        x = Sub(PI(), x);
    } else if (x.Less(HalfPi().Neg())) {
        x = Sub(PI().Neg(), x);
    }

    // sin(x)=x-x³/3!+x⁵/5!-...
    const Decimal xx = Mul(x, x);
    Decimal result = x;
    Decimal term = x;
    int i = 1;
    Decimal cachedResult;
    do {
        cachedResult = result;
        term = Mul(term, Div(xx.Neg(), Int(2 * i * (2 * i + 1))));
        result = Add(result, term);
        i++;
    } while (!cachedResult.Equal(result) && i < kMaximumIterations);

    return result;
}

Decimal Cos(const Decimal& value) {
    Decimal x = value;
    // O(1) 归约：一次性取模
    if (x.Greater(TwoPi()) || x.Less(TwoPi().Neg())) {
        x = Sub(x, Mul(Div(x, TwoPi()).Floor(), TwoPi()));
    }

    // Now x is in (-2pi,2pi)
    if (x.GreaterOrEqual(PI()) && x.LessOrEqual(TwoPi())) {
        return Cos(Sub(x, PI())).Neg();
    }

    if (x.GreaterOrEqual(TwoPi().Neg()) && x.LessOrEqual(PI().Neg())) {
        return Cos(Add(x, PI())).Neg();
    }

    x = Mul(x, x);

    // y=1-x/2!+x^2/4!-x^3/6!...
    Decimal xx = Mul(x, Half()).Neg();
    Decimal y = Add(Decimal::One, xx);
    Decimal cachedY = Sub(y, Decimal::One);  // init cache with different value
    for (int i = 1; !cachedY.Equal(y) && i < kMaximumIterations; i++) {
        cachedY = y;

        // 2i^2+2i+i+1=2i^2+3i+1
        Decimal factor = Int(static_cast<int64_t>(i) * (i + i + 3) + 1);
        factor = Div(Half().Neg(), factor);
        xx = Mul(xx, Mul(x, factor));
        y = Add(y, xx);
    }

    return y;
}

Decimal Tan(const Decimal& x) {
    const Decimal cos = Cos(x);
    if (cos.IsZero()) {
        throw std::domain_error("invalid tan arguments");
    }
    return Div(Sin(x), cos);
}

Decimal Asin(const Decimal& value) {
    if (value.Greater(Decimal::One) || value.Less(Decimal::One.Neg())) {
        throw std::domain_error("x must be in [-1,1]");
    }

    // Known values
    if (value.IsZero()) return Decimal::Zero;
    if (value.Equal(Decimal::One)) return HalfPi();

    // Asin 是奇函数
    if (value.IsNegative()) return Asin(value.Neg()).Neg();

    // asin(x)=0.5*(pi/2-asin(1-2*x*x)) if x>=0
    const Decimal newX = Sub(Decimal::One, Mul(Mul(Int(2), value), value));

    // 取更靠近 0 的一侧，级数收敛更快
    if (value.Abs().Greater(newX.Abs())) {
        const Decimal t = Asin(newX);
        return Mul(Half(), Sub(HalfPi(), t));
    }

    Decimal y = Decimal::Zero;
    Decimal result = value;
    int i = 1;
    y = Add(y, result);
    const Decimal xx = Mul(value, value);
    Decimal cachedResult;
    do {
        cachedResult = result;
        result = Mul(result, Mul(xx, Sub(Decimal::One, Div(Half(), Int(i)))));
        y = Add(y, Div(result, Int(2 * i + 1)));
        i++;
    } while (!cachedResult.Equal(result) && i < kMaximumIterations);

    return y;
}

Decimal Acos(const Decimal& x) {
    if (x.IsZero()) return HalfPi();
    if (x.Equal(Decimal::One)) return Decimal::Zero;

    if (x.IsNegative()) return Sub(PI(), Acos(x.Neg()));

    return Sub(HalfPi(), Asin(x));
}

Decimal Atan(const Decimal& x) {
    if (x.IsZero()) return Decimal::Zero;
    if (x.Equal(Decimal::One)) return QuarterPi();

    // |x|>1 时用 atan(x)=±π/2-atan(1/x) 归约到 [-1,1]
    if (x.Greater(Decimal::One)) {
        return Sub(HalfPi(), Atan(Div(Decimal::One, x)));
    }
    if (x.Less(Decimal::One.Neg())) {
        return Atan(x.Neg()).Neg();
    }

    return Asin(Div(x, Sqrt(Add(Decimal::One, Mul(x, x)))));
}

Decimal Atan2(const Decimal& y, const Decimal& x) {
    if (x.IsPositive()) {
        return Atan(Div(y, x));
    }

    if (x.IsNegative() && !y.IsNegative()) {
        return Add(Atan(Div(y, x)), PI());
    }

    if (x.IsNegative() && y.IsNegative()) {
        return Sub(Atan(Div(y, x)), PI());
    }

    if (x.IsZero() && y.IsPositive()) return HalfPi();
    if (x.IsZero() && y.IsNegative()) return HalfPi().Neg();

    throw std::domain_error("invalid atan2 arguments");
}

Decimal Asinh(const Decimal& value) {
    // 奇函数，先归一到非负，便于用等价变形规避 x*x 溢出
    if (value.IsNegative()) return Asinh(value.Neg()).Neg();
    if (value.Greater(SquareSafeMax())) {
        const Decimal inverted = Div(Decimal::One, value);
        return Add(Log(value), Log(Add(Decimal::One, Sqrt(
            Add(Decimal::One, Mul(inverted, inverted))))));
    }
    return Log(Add(value, Sqrt(Add(Decimal::One, Mul(value, value)))));
}

Decimal Acosh(const Decimal& x) {
    if (x.Less(Decimal::One)) {
        throw std::domain_error("x must be >= 1");
    }
    // |x| 很大时 x*x 会溢出 decimal，改用等价变形
    if (x.Greater(SquareSafeMax())) {
        const Decimal inverted = Div(Decimal::One, x);
        return Add(Log(x), Log(Add(Decimal::One, Sqrt(
            Sub(Decimal::One, Mul(inverted, inverted))))));
    }
    return Log(Add(x, Sqrt(Sub(Mul(x, x), Decimal::One))));
}

Decimal Atanh(const Decimal& x) {
    // 不用 Abs：它会因 decimal.MinValue 取绝对值溢出而抛异常
    if (x.GreaterOrEqual(Decimal::One) || x.LessOrEqual(Decimal::One.Neg())) {
        throw std::domain_error("x must be |x|<1");
    }

    // |x| 较小时改用级数 atanh(x)=x+x³/3+x⁵/5+...
    if (x.LessOrEqual(Half()) && x.GreaterOrEqual(Half().Neg())) {
        const Decimal xx = Mul(x, x);
        Decimal result = x;
        Decimal term = x;
        int k = 3;
        Decimal cachedResult;
        do {
            cachedResult = result;
            term = Mul(term, xx);
            result = Add(result, Div(term, Int(k)));
            k += 2;
        } while (!cachedResult.Equal(result) && k < 2 * kMaximumIterations);

        return result;
    }

    return Mul(Half(), Log(Div(Add(Decimal::One, x), Sub(Decimal::One, x))));
}

Decimal Sinh(const Decimal& value) {
    // 奇函数，先归一到非负，避免 Exp(x) 下溢为 0 后 1/y 除零
    if (value.IsNegative()) return Sinh(value.Neg()).Neg();
    if (value.Greater(HyperbolicSafeMax())) {
        return Exp(Sub(value, Ln2()));
    }
    const Decimal y = Exp(value);
    const Decimal yy = Div(Decimal::One, y);
    return Mul(Sub(y, yy), Half());
}

Decimal Cosh(const Decimal& value) {
    const Decimal absolute = value.IsNegative() ? value.Neg() : value;
    if (absolute.Greater(HyperbolicSafeMax())) {
        return Exp(Sub(absolute, Ln2()));
    }
    const Decimal y = Exp(value);
    const Decimal yy = Div(Decimal::One, y);
    return Mul(Add(y, yy), Half());
}

Decimal Tanh(const Decimal& x) {
    const Decimal y = Exp(x);
    const Decimal yy = Div(Decimal::One, y);
    return Div(Sub(y, yy), Add(y, yy));
}

}  // namespace MathEx
}  // namespace toolgood
