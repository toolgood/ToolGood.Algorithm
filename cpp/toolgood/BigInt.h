// BigInt.h - 任意精度有符号整数
// 移植自 go/algorithm/decimal 依赖的 math/big.Int 所需子集。
// 表示：|value| = sum(limb[i] * (10^9)^i)，limb 为 32 位无符号。
#pragma once

#include <cstdint>
#include <string>
#include <vector>

namespace toolgood {

/// 任意精度有符号整数（十进制 10^9 进制存储）。
/// 只实现 Decimal 所需的运算子集：加减乘、带余除法、比较、10 的幂缩放、字符串转换。
class BigInt {
public:
    BigInt() = default;
    explicit BigInt(int64_t v);
    /// 由无符号 64 位整数构造（避免 int 字面量产生重载歧义）。
    static BigInt FromUInt64(uint64_t v);

    // ---- 构造 ----
    /// 解析十进制字符串（可带正负号）；失败返回 false。
    static bool TryParse(const std::string& s, BigInt& out);
    static BigInt FromString(const std::string& s);
    /// 10^n（n>=0）。
    static BigInt Pow10(int n);

    // ---- 判定 ----
    bool IsZero() const { return mag_.empty(); }
    bool IsNegative() const { return neg_; }
    bool IsPositive() const { return !neg_ && !mag_.empty(); }
    bool IsOdd() const { return !mag_.empty() && (mag_[0] & 1u) != 0; }
    /// 符号：-1 / 0 / 1。
    int Sign() const { return mag_.empty() ? 0 : (neg_ ? -1 : 1); }
    /// 十进制位数（不含符号），0 返回 1。
    int DigitCount() const;

    // ---- 算术 ----
    BigInt Add(const BigInt& other) const;
    BigInt Sub(const BigInt& other) const;
    BigInt Mul(const BigInt& other) const;
    /// 带余除法（向零截断），返回商，余数写入 rem。除数为 0 时返回 0、rem 为 0。
    BigInt QuoRem(const BigInt& divisor, BigInt& rem) const;
    BigInt Quo(const BigInt& divisor) const;
    BigInt Rem(const BigInt& divisor) const;
    BigInt Neg() const;
    BigInt Abs() const;
    /// 乘以 10^n（n>=0）。
    BigInt MulPow10(int n) const;

    // ---- 比较 ----
    int Cmp(const BigInt& other) const;
    bool operator==(const BigInt& other) const { return Cmp(other) == 0; }
    bool operator!=(const BigInt& other) const { return Cmp(other) != 0; }
    bool operator<(const BigInt& other) const { return Cmp(other) < 0; }
    bool operator<=(const BigInt& other) const { return Cmp(other) <= 0; }
    bool operator>(const BigInt& other) const { return Cmp(other) > 0; }
    bool operator>=(const BigInt& other) const { return Cmp(other) >= 0; }

    // ---- 转换 ----
    std::string ToString() const;
    /// 转 int64，越界饱和。
    int64_t ToInt64() const;
    /// 是否可用 int64 表示。
    bool FitsInt64() const;

private:
    static const uint32_t kBase = 1000000000u;

    std::vector<uint32_t> mag_;  // 小端，base 1e9
    bool neg_ = false;

    void Trim();
    void SetUInt64(uint64_t v);
    static int CmpMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b);
    static std::vector<uint32_t> AddMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b);
    /// 要求 |a| >= |b|
    static std::vector<uint32_t> SubMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b);
    static std::vector<uint32_t> MulMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b);
    /// 无符号长除法，返回商，余数写入 rem。
    static std::vector<uint32_t> DivModMag(const std::vector<uint32_t>& a,
                                          const std::vector<uint32_t>& b,
                                          std::vector<uint32_t>& rem);
};

}  // namespace toolgood
