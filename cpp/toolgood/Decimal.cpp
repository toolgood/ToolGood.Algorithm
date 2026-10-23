#include "Decimal.h"

#include <charconv>
#include <cmath>
#include <cstdlib>
#include <limits>
#include <string>

namespace toolgood {

namespace {

const int kPrecision = 34;   // 内部工作精度（最终由 NormalizeToDecimal96 收敛）
const int kPrecision32 = 7;   // DECIMAL32 有效数字

BigInt Pow10(int n) { return BigInt::Pow10(n); }

/// 若分母仅含因子 2/5（即分数为有限小数），返回所需标度。
bool TerminatingScale(const BigInt& den, int& scale) {
    scale = 0;
    if (den.IsZero()) return false;
    BigInt d = den.Abs();
    int two = 0, five = 0;
    BigInt r;
    for (;;) {
        BigInt q = d.QuoRem(BigInt(2), r);
        if (!r.IsZero()) break;
        d = q;
        two++;
    }
    for (;;) {
        BigInt q = d.QuoRem(BigInt(5), r);
        if (!r.IsZero()) break;
        d = q;
        five++;
    }
    if (!(d == BigInt(1))) return false;
    scale = two > five ? two : five;
    return true;
}

/// 比较 num/den 与 10^e（num、den 同号，den 非 0）。
int CmpRatPow10(const BigInt& num, const BigInt& den, int e) {
    if (e >= 0) return num.Cmp(den.MulPow10(e));
    return num.MulPow10(-e).Cmp(den);
}

/// 返回 e，使 10^(e-1) <= num/den < 10^e（num/den > 0）。
int RatExponent(const BigInt& num, const BigInt& den) {
    if (num.IsZero()) return 1;
    int e = num.Abs().DigitCount() - den.Abs().DigitCount() + 1;
    for (int i = 0; i < 8; i++) {
        if (CmpRatPow10(num, den, e) >= 0) {
            e++;
        } else if (CmpRatPow10(num, den, e - 1) < 0) {
            e--;
        } else {
            break;
        }
    }
    return e;
}

/// 计算 round(num/den * 10^shift)，HALF_EVEN。
BigInt RoundRatToInt(const BigInt& num, const BigInt& den, int shift) {
    BigInt n = num;
    BigInt d = den;
    if (shift >= 0) {
        n = n.MulPow10(shift);
    } else {
        d = d.MulPow10(-shift);
    }
    BigInt rem;
    BigInt q = n.QuoRem(d, rem);
    if (!rem.IsZero()) {
        BigInt twice = rem.Abs().Mul(BigInt(2));
        int cmp = twice.Cmp(d.Abs());
        if (cmp > 0 || (cmp == 0 && q.IsOdd())) {
            q = n.IsNegative() ? q.Sub(BigInt(1)) : q.Add(BigInt(1));
        }
    }
    return q;
}

/// 向下取整的整数商。
BigInt FloorDivBig(const BigInt& num, const BigInt& den) {
    BigInt rem;
    BigInt q = num.QuoRem(den, rem);
    if (!rem.IsZero() && (rem.IsNegative() != den.IsNegative())) {
        q = q.Sub(BigInt(1));
    }
    return q;
}

/// 收敛到 C# System.Decimal 的表示范围：96 位尾数（<= 79228162514264337593543950335）、scale <= 28。
/// 超范围时按 HALF_EVEN 逐位舍入，与 C# decimal 运算的自动截断一致。
Decimal NormalizeToDecimal96(const Decimal& d) {
    static const BigInt kMax96 = BigInt::FromString("79228162514264337593543950335");
    BigInt u = d.Unscaled();
    int32_t s = d.Scale();
    while (s > 28 || (s > 0 && u.Abs() > kMax96)) {
        BigInt rem;
        BigInt q = u.QuoRem(BigInt(10), rem);
        BigInt twice = rem.Abs().Mul(BigInt(2));
        const int cmp = twice.Cmp(BigInt(10));
        if (cmp > 0 || (cmp == 0 && q.IsOdd())) {
            q = u.IsNegative() ? q.Sub(BigInt(1)) : q.Add(BigInt(1));
        }
        u = q;
        s--;
    }
    // 标度已为 0 仍超 96 位尾数：保留原值，交由调用方判定溢出（C# 侧会抛 OverflowException）
    return Decimal(u, s);
}

/// 去掉末尾的 0（保持数值不变），对齐 C# decimal 精确除法的标度收敛行为。
Decimal StripTrailingZeros(const BigInt& unscaled, int32_t scale) {
    if (scale <= 0 || unscaled.IsZero()) return Decimal(unscaled, scale);
    BigInt u = unscaled;
    int32_t s = scale;
    BigInt rem;
    while (s > 0) {
        BigInt q = u.QuoRem(BigInt(10), rem);
        if (!rem.IsZero()) break;
        u = q;
        s--;
    }
    return Decimal(u, s);
}

/// 大整数平方根（向下取整）。
BigInt BigIntSqrt(const BigInt& x) {
    if (x.Sign() <= 0) return BigInt();
    int digits = x.DigitCount();
    BigInt guess = Pow10((digits + 1) / 2);
    for (;;) {
        BigInt next = guess.Add(x.Quo(guess)).Quo(BigInt(2));
        if (next.Cmp(guess) >= 0) break;
        guess = next;
    }
    return guess;
}

}  // namespace

const Decimal Decimal::Zero;
const Decimal Decimal::One = Decimal(BigInt(1), 0);
const Decimal Decimal::Max = Decimal::FromString("79228162514264337593543950335");
const Decimal Decimal::MinusMax =
    Decimal::FromString("-79228162514264337593543950335");

Decimal::Decimal(const BigInt& unscaled, int32_t scale) : unscaled_(unscaled), scale_(scale) {
    if (unscaled_.IsZero()) {
        scale_ = 0;
    }
}

Decimal Decimal::FromInt(int64_t v) { return Decimal(BigInt(v), 0); }

Decimal Decimal::FromUInt(uint64_t v) { return Decimal(BigInt::FromUInt64(v), 0); }

Decimal Decimal::FromBigInt(const BigInt& v) { return Decimal(v, 0); }

Decimal Decimal::FromDouble(double v) {
    if (std::isnan(v) || std::isinf(v)) return Decimal();
    char buf[64];
    auto res = std::to_chars(buf, buf + sizeof(buf), v);
    std::string s(buf, res.ptr);
    return FromString(s);
}

bool Decimal::TryParse(const std::string& s, Decimal& out) {
    // 去首尾空白
    size_t b = 0, e = s.size();
    while (b < e && (s[b] == ' ' || s[b] == '\t' || s[b] == '\r' || s[b] == '\n')) b++;
    while (e > b && (s[e - 1] == ' ' || s[e - 1] == '\t' || s[e - 1] == '\r' || s[e - 1] == '\n')) e--;
    if (b == e) return false;
    std::string t = s.substr(b, e - b);

    bool neg = false;
    size_t p = 0;
    if (t[p] == '+' || t[p] == '-') {
        neg = (t[p] == '-');
        p++;
    }
    std::string mantissa = t.substr(p);
    int exp = 0;
    size_t ei = mantissa.find_first_of("eE");
    if (ei != std::string::npos) {
        std::string expPart = mantissa.substr(ei + 1);
        if (expPart.empty()) return false;
        size_t k = 0;
        bool eneg = false;
        if (expPart[k] == '+' || expPart[k] == '-') {
            eneg = (expPart[k] == '-');
            k++;
        }
        if (k >= expPart.size()) return false;
        int val = 0;
        for (; k < expPart.size(); k++) {
            if (expPart[k] < '0' || expPart[k] > '9') return false;
            if (val < 100000000) val = val * 10 + (expPart[k] - '0');
        }
        exp = eneg ? -val : val;
        mantissa = mantissa.substr(0, ei);
    }

    std::string intPart = mantissa;
    std::string fracPart;
    size_t di = mantissa.find('.');
    if (di != std::string::npos) {
        intPart = mantissa.substr(0, di);
        fracPart = mantissa.substr(di + 1);
    }
    if (intPart.empty() && fracPart.empty()) return false;
    std::string digits = intPart + fracPart;
    if (digits.empty()) return false;
    for (char c : digits) {
        if (c < '0' || c > '9') return false;
    }
    BigInt u;
    if (!BigInt::TryParse(digits, u)) return false;
    if (neg) u = u.Neg();

    int32_t scale = static_cast<int32_t>(fracPart.size()) - exp;
    if (scale < 0) {
        u = u.MulPow10(-scale);
        scale = 0;
    }
    if (u.IsZero()) {
        out = Decimal();
        return true;
    }
    out = Decimal(u, scale);
    return true;
}

Decimal Decimal::FromString(const std::string& s) {
    Decimal d;
    TryParse(s, d);
    return d;
}

bool Decimal::IsInteger() const { return Trunc().Equal(*this); }

Decimal Decimal::Normalize(const BigInt& unscaled, int32_t scale) {
    if (unscaled.IsZero()) return Decimal();
    return Decimal(unscaled, scale);
}

Decimal Decimal::Add(const Decimal& other) const {
    if (scale_ == other.scale_) {
        return NormalizeToDecimal96(Normalize(unscaled_.Add(other.unscaled_), scale_));
    }
    if (scale_ < other.scale_) {
        BigInt a = unscaled_.MulPow10(other.scale_ - scale_);
        return NormalizeToDecimal96(Normalize(a.Add(other.unscaled_), other.scale_));
    }
    BigInt b = other.unscaled_.MulPow10(scale_ - other.scale_);
    return NormalizeToDecimal96(Normalize(unscaled_.Add(b), scale_));
}

Decimal Decimal::Sub(const Decimal& other) const {
    if (scale_ == other.scale_) {
        return NormalizeToDecimal96(Normalize(unscaled_.Sub(other.unscaled_), scale_));
    }
    if (scale_ < other.scale_) {
        BigInt a = unscaled_.MulPow10(other.scale_ - scale_);
        return NormalizeToDecimal96(Normalize(a.Sub(other.unscaled_), other.scale_));
    }
    BigInt b = other.unscaled_.MulPow10(scale_ - other.scale_);
    return NormalizeToDecimal96(Normalize(unscaled_.Sub(b), scale_));
}

Decimal Decimal::Mul(const Decimal& other) const {
    return NormalizeToDecimal96(Normalize(unscaled_.Mul(other.unscaled_), scale_ + other.scale_));
}

Decimal Decimal::Neg() const {
    if (IsZero()) return Decimal();
    return Decimal(unscaled_.Neg(), scale_);
}

Decimal Decimal::Abs() const {
    if (Sign() >= 0) return *this;
    return Neg();
}

int Decimal::Cmp(const Decimal& other) const {
    if (scale_ == other.scale_) return unscaled_.Cmp(other.unscaled_);
    if (scale_ < other.scale_) {
        return unscaled_.MulPow10(other.scale_ - scale_).Cmp(other.unscaled_);
    }
    return unscaled_.Cmp(other.unscaled_.MulPow10(scale_ - other.scale_));
}

Decimal Decimal::DivideWithPrecision(const Decimal& a, const Decimal& b, int prec) {
    if (b.IsZero() || a.IsZero()) return Decimal();
    // 把分母统一为正数：RatExponent/CmpRatPow10 按绝对值估算指数，
    // 若分母为负会得到错误的有效数字位数（负/负相除精度掉档）。
    BigInt num = a.unscaled_;
    BigInt den = b.unscaled_;
    if (den.IsNegative()) {
        num = num.Neg();
        den = den.Neg();
    }
    const int exp10 = b.scale_ - a.scale_;

    int termScale = 0;
    if (TerminatingScale(den, termScale)) {
        // 有限小数：精确结果
        BigInt n = num.Mul(Pow10(termScale).Quo(den));
        int32_t scale = termScale - exp10;
        if (scale < 0) {
            n = n.MulPow10(-scale);
            scale = 0;
        }
        Decimal r = RoundUnscaledToPrecision(n, scale, prec);
        return StripTrailingZeros(r.unscaled_, r.scale_);
    }

    // 无限小数：取 prec 位有效数字
    const int e = RatExponent(num, den) + exp10;
    int32_t scale = prec - e;
    if (scale < 0) scale = 0;
    const int shift = scale + exp10;
    BigInt unscaled = RoundRatToInt(num, den, shift);
    return Normalize(unscaled, scale);
}

Decimal Decimal::Div(const Decimal& other) const {
    return NormalizeToDecimal96(DivideWithPrecision(*this, other, kPrecision));
}

Decimal Decimal::Div32(const Decimal& other) const {
    return NormalizeToDecimal96(DivideWithPrecision(*this, other, kPrecision32));
}

Decimal Decimal::RatToDecimal(const BigInt& num, const BigInt& den, int prec) {
    if (num.IsZero()) return Decimal();
    BigInt n0 = num;
    BigInt d0 = den;
    if (d0.IsNegative()) {
        n0 = n0.Neg();
        d0 = d0.Neg();
    }
    int termScale = 0;
    if (TerminatingScale(d0, termScale)) {
        BigInt n = n0.Mul(Pow10(termScale).Quo(d0));
        return RoundUnscaledToPrecision(n, termScale, prec);
    }
    const int e = RatExponent(n0, d0);
    int32_t scale = prec - e;
    if (scale < 0) scale = 0;
    return Normalize(RoundRatToInt(n0, d0, scale), scale);
}

Decimal Decimal::RoundUnscaledToPrecision(const BigInt& unscaled, int32_t scale, int prec) {
    if (unscaled.IsZero()) return Decimal();
    const int digits = unscaled.DigitCount();
    if (digits <= prec) return Normalize(unscaled, scale);
    const int drop = digits - prec;
    BigInt divisor = Pow10(drop);
    BigInt rem;
    BigInt q = unscaled.QuoRem(divisor, rem);
    BigInt twice = rem.Abs().Mul(BigInt(2));
    int cmp = twice.Cmp(divisor);
    if (cmp > 0 || (cmp == 0 && q.IsOdd())) {
        q = unscaled.IsNegative() ? q.Sub(BigInt(1)) : q.Add(BigInt(1));
    }
    return Normalize(q, scale - drop);
}

Decimal Decimal::Mod(const Decimal& other) const {
    if (other.IsZero()) return Decimal();
    BigInt n = unscaled_.MulPow10(other.scale_);
    BigInt d = other.unscaled_.MulPow10(scale_);
    BigInt q = n.Quo(d);  // 向零截断
    Decimal prod = other.Mul(Decimal::FromBigInt(q));
    return Sub(prod);
}

Decimal Decimal::FloorDiv(const Decimal& other) const {
    if (other.IsZero()) return Decimal();
    BigInt n = unscaled_.MulPow10(other.scale_);
    BigInt d = other.unscaled_.MulPow10(scale_);
    return Decimal::FromBigInt(FloorDivBig(n, d));
}

Decimal Decimal::FloorMod(const Decimal& other) const {
    if (other.IsZero()) return Decimal();
    BigInt n = unscaled_.MulPow10(other.scale_);
    BigInt d = other.unscaled_.MulPow10(scale_);
    BigInt q = FloorDivBig(n, d);
    Decimal prod = other.Mul(Decimal::FromBigInt(q));
    return Sub(prod);
}

Decimal Decimal::Pow(int n) const {
    if (n == 0) return Decimal::One;
    if (n < 0) return Decimal::One.Div(Pow(-n));
    Decimal result = Decimal::One;
    Decimal base = *this;
    while (n > 0) {
        if (n & 1) result = result.Mul(base);
        n >>= 1;
        if (n > 0) base = base.Mul(base);
    }
    return result;
}

Decimal Decimal::Sqrt() const {
    if (Sign() < 0 || IsZero()) return Decimal();
    const int extra = 40;
    const int k = (scale_ + 1) / 2 + extra;
    BigInt x = unscaled_.MulPow10(2 * k - scale_);
    BigInt n = BigIntSqrt(x);
    return NormalizeToDecimal96(RoundUnscaledToPrecision(n, static_cast<int32_t>(k), kPrecision));
}

Decimal Decimal::Trunc() const {
    if (scale_ == 0) return *this;
    return Normalize(unscaled_.Quo(Pow10(scale_)), 0);
}

Decimal Decimal::Floor() const {
    if (scale_ == 0) return *this;
    BigInt rem;
    BigInt q = unscaled_.QuoRem(Pow10(scale_), rem);
    if (rem.IsNegative()) q = q.Sub(BigInt(1));
    return Normalize(q, 0);
}

Decimal Decimal::Ceil() const {
    if (scale_ == 0) return *this;
    BigInt rem;
    BigInt q = unscaled_.QuoRem(Pow10(scale_), rem);
    if (rem.IsPositive()) q = q.Add(BigInt(1));
    return Normalize(q, 0);
}

Decimal Decimal::RoundHalfUp(int32_t digits) const {
    if (scale_ <= digits) return *this;
    const int drop = scale_ - digits;
    BigInt divisor = Pow10(drop);
    BigInt rem;
    BigInt q = unscaled_.QuoRem(divisor, rem);
    BigInt twice = rem.Abs().Mul(BigInt(2));
    if (twice.Cmp(divisor) >= 0) {
        q = unscaled_.IsNegative() ? q.Sub(BigInt(1)) : q.Add(BigInt(1));
    }
    return Normalize(q, digits);
}

Decimal Decimal::RoundHalfEven(int32_t digits) const {
    if (scale_ <= digits) return *this;
    const int drop = scale_ - digits;
    BigInt divisor = Pow10(drop);
    BigInt rem;
    BigInt q = unscaled_.QuoRem(divisor, rem);
    BigInt twice = rem.Abs().Mul(BigInt(2));
    int cmp = twice.Cmp(divisor);
    if (cmp > 0 || (cmp == 0 && q.IsOdd())) {
        q = unscaled_.IsNegative() ? q.Sub(BigInt(1)) : q.Add(BigInt(1));
    }
    return Normalize(q, digits);
}

double Decimal::Float64() const {
    if (IsZero()) return 0.0;
    return std::strtod(PlainStrip().c_str(), nullptr);
}

int64_t Decimal::Int64() const { return Trunc().unscaled_.ToInt64(); }

bool Decimal::FitsInt32() const {
    Decimal t = Trunc();
    if (!t.unscaled_.FitsInt64()) return false;
    int64_t v = t.unscaled_.ToInt64();
    return v >= std::numeric_limits<int32_t>::min() && v <= std::numeric_limits<int32_t>::max();
}

std::string Decimal::ToString() const {
    if (unscaled_.IsZero()) return "0";
    const bool neg = unscaled_.IsNegative();
    std::string abs = unscaled_.Abs().ToString();
    const int scale = scale_;
    std::string sb;
    if (neg) sb.push_back('-');
    if (scale <= 0) {
        sb += abs;
        if (scale < 0) sb.append(static_cast<size_t>(-scale), '0');
        return sb;
    }
    const size_t len = abs.size();
    if (len <= static_cast<size_t>(scale)) {
        sb += "0.";
        sb.append(static_cast<size_t>(scale) - len, '0');
        sb += abs;
    } else {
        const size_t cut = len - static_cast<size_t>(scale);
        sb += abs.substr(0, cut);
        sb.push_back('.');
        sb += abs.substr(cut);
    }
    return sb;
}

std::string Decimal::PlainStrip() const {
    std::string s = ToString();
    if (s.find('.') != std::string::npos) {
        size_t p = s.find_last_not_of('0');
        if (p != std::string::npos && s[p] == '.') p--;
        s = s.substr(0, p + 1);
    }
    if (s.empty() || s == "-") return "0";
    return s;
}

std::string Decimal::StringFixed(int32_t digits) const {
    return RoundHalfUp(digits).ToString();
}

}  // namespace toolgood
