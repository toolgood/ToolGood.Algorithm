// Decimal.h - 定点十进制数
// 语义对齐 C# System.Decimal（96 位尾数 / 28~29 位有效数字），
// 并采用 go/python 移植版约定的 DECIMAL128（34 位）除法精度。
// 表示：value = unscaled * 10^-scale
//   - 加减乘为精确运算（不舍入）；
//   - 除法按 34 位有效数字舍入，HALF_EVEN。
#pragma once

#include <cstdint>
#include <string>

#include "BigInt.h"

namespace toolgood {

/// 定点十进制数（不可变值类型）。
class Decimal {
public:
    Decimal() = default;
    Decimal(const BigInt& unscaled, int32_t scale);

    // ---- 构造 ----
    static Decimal FromInt(int64_t v);
    static Decimal FromUInt(uint64_t v);
    static Decimal FromBigInt(const BigInt& v);
    static Decimal FromDouble(double v);
    /// 解析十进制字符串（支持符号、小数、指数），失败返回 false。
    static bool TryParse(const std::string& s, Decimal& out);
    static Decimal FromString(const std::string& s);

    // ---- 常用常量 ----
    static const Decimal Zero;
    static const Decimal One;
    /// C# decimal.MaxValue
    static const Decimal Max;
    /// 解析范围上限（与 Max 一致，用于数值越界判断）
    static const Decimal MinusMax;

    // ---- 判定 ----
    bool IsZero() const { return unscaled_.IsZero(); }
    int Sign() const { return unscaled_.Sign(); }
    bool IsNegative() const { return unscaled_.IsNegative(); }
    bool IsPositive() const { return unscaled_.IsPositive(); }
    bool IsInteger() const;
    int32_t Scale() const { return scale_; }
    const BigInt& Unscaled() const { return unscaled_; }

    // ---- 算术 ----
    Decimal Add(const Decimal& other) const;
    Decimal Sub(const Decimal& other) const;
    Decimal Mul(const Decimal& other) const;
    /// 除法：按 DECIMAL128（34 位有效数字）HALF_EVEN 舍入；除零返回 0。
    Decimal Div(const Decimal& other) const;
    /// 除法：按 DECIMAL32（7 位有效数字）舍入。
    Decimal Div32(const Decimal& other) const;
    /// 取余：符号随被除数（BigDecimal.remainder）。
    Decimal Mod(const Decimal& other) const;
    /// 向下取整除法。
    Decimal FloorDiv(const Decimal& other) const;
    /// 取余：符号随除数（Excel MOD / Python %）。
    Decimal FloorMod(const Decimal& other) const;
    Decimal Neg() const;
    Decimal Abs() const;
    /// 整数次幂；负指数按 34 位有效数字舍入。
    Decimal Pow(int n) const;
    /// 平方根；负数返回 0。按 34 位有效数字舍入。
    Decimal Sqrt() const;

    // ---- 比较 ----
    int Cmp(const Decimal& other) const;
    bool Equal(const Decimal& other) const { return Cmp(other) == 0; }
    bool Less(const Decimal& other) const { return Cmp(other) < 0; }
    bool LessOrEqual(const Decimal& other) const { return Cmp(other) <= 0; }
    bool Greater(const Decimal& other) const { return Cmp(other) > 0; }
    bool GreaterOrEqual(const Decimal& other) const { return Cmp(other) >= 0; }

    // ---- 取整 / 舍入 ----
    Decimal Trunc() const;
    Decimal Floor() const;
    Decimal Ceil() const;
    /// 四舍五入（远离零），digits 为保留小数位数。
    Decimal RoundHalfUp(int32_t digits) const;
    /// 银行家舍入，digits 为保留小数位数。
    Decimal RoundHalfEven(int32_t digits) const;

    // ---- 转换 ----
    double Float64() const;
    /// 向零截断为 int64，越界饱和。
    int64_t Int64() const;
    int Int() const { return static_cast<int>(Int64()); }
    /// 是否可用 int 表示（不饱和）。
    bool FitsInt32() const;
    /// BigDecimal.toPlainString()：保留末尾 0。
    std::string ToString() const;
    /// 去掉末尾 0 的普通字符串。
    std::string PlainStrip() const;
    /// 保留 digits 位小数（四舍五入）。
    std::string StringFixed(int32_t digits) const;

private:
    BigInt unscaled_;
    int32_t scale_ = 0;

    static Decimal Normalize(const BigInt& unscaled, int32_t scale);
    /// 把非标度整数舍入到 prec 位有效数字。
    static Decimal RoundUnscaledToPrecision(const BigInt& unscaled, int32_t scale, int prec);
    static Decimal DivideWithPrecision(const Decimal& a, const Decimal& b, int prec);
    static Decimal RatToDecimal(const BigInt& num, const BigInt& den, int prec);
};

/// 便捷运算：Decimal 与 int 混用
inline Decimal operator+(const Decimal& a, const Decimal& b) { return a.Add(b); }
inline Decimal operator-(const Decimal& a, const Decimal& b) { return a.Sub(b); }
inline Decimal operator*(const Decimal& a, const Decimal& b) { return a.Mul(b); }
inline Decimal operator/(const Decimal& a, const Decimal& b) { return a.Div(b); }

}  // namespace toolgood
