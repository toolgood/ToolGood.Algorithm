#include "BigInt.h"

#include <algorithm>
#include <limits>

namespace toolgood {

namespace {
const uint32_t kBase32 = 1000000000u;
// 10^0..10^9，用于 MulPow10 的余数部分
const uint32_t kPow10Small[10] = {1u,        10u,        100u,        1000u,       10000u,
                                  100000u,   1000000u,   10000000u,   100000000u,  1000000000u};
}  // namespace

BigInt::BigInt(int64_t v) {
    if (v == 0) return;
    if (v < 0) {
        neg_ = true;
        // 处理 INT64_MIN
        uint64_t u = static_cast<uint64_t>(-(v + 1)) + 1u;
        SetUInt64(u);
    } else {
        SetUInt64(static_cast<uint64_t>(v));
    }
}

BigInt BigInt::FromUInt64(uint64_t v) {
    BigInt r;
    r.SetUInt64(v);
    return r;
}

void BigInt::SetUInt64(uint64_t v) {
    mag_.clear();
    while (v > 0) {
        mag_.push_back(static_cast<uint32_t>(v % kBase));
        v /= kBase;
    }
    if (mag_.empty()) neg_ = false;
}

void BigInt::Trim() {
    while (!mag_.empty() && mag_.back() == 0) mag_.pop_back();
    if (mag_.empty()) neg_ = false;
}

bool BigInt::TryParse(const std::string& s, BigInt& out) {
    size_t i = 0;
    const size_t n = s.size();
    while (i < n && (s[i] == ' ' || s[i] == '\t' || s[i] == '\r' || s[i] == '\n')) i++;
    bool neg = false;
    if (i < n && (s[i] == '+' || s[i] == '-')) {
        neg = (s[i] == '-');
        i++;
    }
    size_t start = i;
    while (i < n && s[i] >= '0' && s[i] <= '9') i++;
    if (i == start) return false;
    while (i < n && (s[i] == ' ' || s[i] == '\t' || s[i] == '\r' || s[i] == '\n')) i++;
    if (i != n) return false;

    // 按 9 位一组从低位切分
    std::vector<uint32_t> mag;
    size_t end = start;
    // 跳过前导 0
    while (end < i - 1 && s[end] == '0') end++;
    size_t len = i - end;
    size_t full = len / 9;
    size_t rest = len % 9;
    size_t p = end;
    if (rest > 0) {
        uint32_t v = 0;
        for (size_t k = 0; k < rest; k++) v = v * 10 + static_cast<uint32_t>(s[p + k] - '0');
        mag.push_back(v);
        p += rest;
    }
    for (size_t g = 0; g < full; g++) {
        uint32_t v = 0;
        for (size_t k = 0; k < 9; k++) v = v * 10 + static_cast<uint32_t>(s[p + k] - '0');
        mag.push_back(v);
        p += 9;
    }
    // p 递增方向是从高位到低位，上面 push 顺序错误，需反转
    std::reverse(mag.begin(), mag.end());

    out.mag_ = std::move(mag);
    out.neg_ = neg;
    out.Trim();
    return true;
}

BigInt BigInt::FromString(const std::string& s) {
    BigInt r;
    TryParse(s, r);
    return r;
}

BigInt BigInt::Pow10(int n) {
    if (n <= 0) return BigInt(1);
    BigInt r(1);
    return r.MulPow10(n);
}

int BigInt::DigitCount() const {
    if (mag_.empty()) return 1;
    int digits = static_cast<int>(mag_.size() - 1) * 9;
    uint32_t top = mag_.back();
    while (top > 0) {
        digits++;
        top /= 10;
    }
    return digits;
}

int BigInt::CmpMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b) {
    if (a.size() != b.size()) return a.size() < b.size() ? -1 : 1;
    for (size_t i = a.size(); i-- > 0;) {
        if (a[i] != b[i]) return a[i] < b[i] ? -1 : 1;
    }
    return 0;
}

std::vector<uint32_t> BigInt::AddMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b) {
    std::vector<uint32_t> r;
    const size_t n = std::max(a.size(), b.size());
    r.reserve(n + 1);
    uint64_t carry = 0;
    for (size_t i = 0; i < n; i++) {
        uint64_t sum = carry;
        if (i < a.size()) sum += a[i];
        if (i < b.size()) sum += b[i];
        r.push_back(static_cast<uint32_t>(sum % kBase));
        carry = sum / kBase;
    }
    if (carry > 0) r.push_back(static_cast<uint32_t>(carry));
    return r;
}

std::vector<uint32_t> BigInt::SubMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b) {
    std::vector<uint32_t> r;
    r.reserve(a.size());
    int64_t borrow = 0;
    for (size_t i = 0; i < a.size(); i++) {
        int64_t cur = static_cast<int64_t>(a[i]) - borrow;
        if (i < b.size()) cur -= static_cast<int64_t>(b[i]);
        if (cur < 0) {
            cur += kBase;
            borrow = 1;
        } else {
            borrow = 0;
        }
        r.push_back(static_cast<uint32_t>(cur));
    }
    while (!r.empty() && r.back() == 0) r.pop_back();
    return r;
}

std::vector<uint32_t> BigInt::MulMag(const std::vector<uint32_t>& a, const std::vector<uint32_t>& b) {
    if (a.empty() || b.empty()) return {};
    std::vector<uint32_t> r(a.size() + b.size(), 0);
    for (size_t i = 0; i < a.size(); i++) {
        uint64_t carry = 0;
        const uint64_t ai = a[i];
        if (ai == 0) continue;
        for (size_t j = 0; j < b.size(); j++) {
            uint64_t cur = static_cast<uint64_t>(r[i + j]) + ai * static_cast<uint64_t>(b[j]) + carry;
            r[i + j] = static_cast<uint32_t>(cur % kBase);
            carry = cur / kBase;
        }
        size_t k = i + b.size();
        while (carry > 0) {
            uint64_t cur = static_cast<uint64_t>(r[k]) + carry;
            r[k] = static_cast<uint32_t>(cur % kBase);
            carry = cur / kBase;
            k++;
        }
    }
    while (!r.empty() && r.back() == 0) r.pop_back();
    return r;
}

std::vector<uint32_t> BigInt::DivModMag(const std::vector<uint32_t>& a,
                                        const std::vector<uint32_t>& b,
                                        std::vector<uint32_t>& rem) {
    rem.clear();
    if (b.empty()) return {};                      // 除零：调用方保证不发生
    if (CmpMag(a, b) < 0) {
        rem = a;
        return {};
    }
    std::vector<uint32_t> q(a.size(), 0);
    std::vector<uint32_t> r;
    r.reserve(a.size() + 1);
    for (size_t i = a.size(); i-- > 0;) {
        // r = r * base + a[i]
        r.insert(r.begin(), a[i]);
        while (!r.empty() && r.back() == 0) r.pop_back();
        // 二分查找最大 qd 使 qd*b <= r
        uint32_t lo = 0, hi = kBase32 - 1, best = 0;
        while (lo <= hi) {
            uint32_t mid = lo + (hi - lo) / 2;
            std::vector<uint32_t> prod = MulMag(b, std::vector<uint32_t>{mid});
            if (CmpMag(prod, r) <= 0) {
                best = mid;
                if (mid == kBase32 - 1) break;
                lo = mid + 1;
            } else {
                if (mid == 0) break;
                hi = mid - 1;
            }
        }
        q[i] = best;
        if (best != 0) {
            std::vector<uint32_t> prod = MulMag(b, std::vector<uint32_t>{best});
            r = SubMag(r, prod);
        }
    }
    while (!q.empty() && q.back() == 0) q.pop_back();
    rem = r;
    return q;
}

BigInt BigInt::Add(const BigInt& other) const {
    BigInt r;
    if (neg_ == other.neg_) {
        r.mag_ = AddMag(mag_, other.mag_);
        r.neg_ = neg_;
    } else {
        int c = CmpMag(mag_, other.mag_);
        if (c == 0) return BigInt();
        if (c > 0) {
            r.mag_ = SubMag(mag_, other.mag_);
            r.neg_ = neg_;
        } else {
            r.mag_ = SubMag(other.mag_, mag_);
            r.neg_ = other.neg_;
        }
    }
    r.Trim();
    return r;
}

BigInt BigInt::Sub(const BigInt& other) const { return Add(other.Neg()); }

BigInt BigInt::Mul(const BigInt& other) const {
    BigInt r;
    r.mag_ = MulMag(mag_, other.mag_);
    r.neg_ = (neg_ != other.neg_);
    r.Trim();
    return r;
}

BigInt BigInt::QuoRem(const BigInt& divisor, BigInt& rem) const {
    BigInt q;
    BigInt r;
    if (divisor.mag_.empty()) {
        rem = BigInt();
        return BigInt();
    }
    std::vector<uint32_t> rmag;
    q.mag_ = DivModMag(mag_, divisor.mag_, rmag);
    r.mag_ = rmag;
    q.neg_ = !q.mag_.empty() && (neg_ != divisor.neg_);
    r.neg_ = !r.mag_.empty() && neg_;  // 余数符号随被除数
    q.Trim();
    r.Trim();
    rem = r;
    return q;
}

BigInt BigInt::Quo(const BigInt& divisor) const {
    BigInt rem;
    return QuoRem(divisor, rem);
}

BigInt BigInt::Rem(const BigInt& divisor) const {
    BigInt rem;
    QuoRem(divisor, rem);
    return rem;
}

BigInt BigInt::Neg() const {
    BigInt r = *this;
    if (!r.mag_.empty()) r.neg_ = !r.neg_;
    return r;
}

BigInt BigInt::Abs() const {
    BigInt r = *this;
    r.neg_ = false;
    return r;
}

BigInt BigInt::MulPow10(int n) const {
    if (mag_.empty() || n <= 0) return *this;
    BigInt r;
    const int limbShift = n / 9;
    const int rest = n % 9;
    r.mag_.assign(static_cast<size_t>(limbShift), 0);
    std::vector<uint32_t> scaled;
    uint64_t carry = 0;
    const uint64_t mul = kPow10Small[rest];
    scaled.reserve(mag_.size() + 1);
    for (size_t i = 0; i < mag_.size(); i++) {
        uint64_t cur = static_cast<uint64_t>(mag_[i]) * mul + carry;
        scaled.push_back(static_cast<uint32_t>(cur % kBase));
        carry = cur / kBase;
    }
    if (carry > 0) scaled.push_back(static_cast<uint32_t>(carry));
    r.mag_.insert(r.mag_.end(), scaled.begin(), scaled.end());
    r.neg_ = neg_;
    r.Trim();
    return r;
}

int BigInt::Cmp(const BigInt& other) const {
    const int sa = Sign();
    const int sb = other.Sign();
    if (sa != sb) return sa < sb ? -1 : 1;
    if (sa == 0) return 0;
    int c = CmpMag(mag_, other.mag_);
    return sa > 0 ? c : -c;
}

std::string BigInt::ToString() const {
    if (mag_.empty()) return "0";
    std::string s;
    if (neg_) s.push_back('-');
    s += std::to_string(mag_.back());
    for (size_t i = mag_.size() - 1; i-- > 0;) {
        std::string part = std::to_string(mag_[i]);
        s.append(9 - part.size(), '0');
        s += part;
    }
    return s;
}

bool BigInt::FitsInt64() const {
    static const BigInt kMax(std::numeric_limits<int64_t>::max());
    static const BigInt kMinMinus1 = BigInt(std::numeric_limits<int64_t>::min()).Add(BigInt(1));
    if (neg_) return Cmp(kMinMinus1) >= 0;
    return Cmp(kMax) <= 0;
}

int64_t BigInt::ToInt64() const {
    if (!FitsInt64()) {
        return neg_ ? std::numeric_limits<int64_t>::min() : std::numeric_limits<int64_t>::max();
    }
    uint64_t u = 0;
    for (size_t i = mag_.size(); i-- > 0;) {
        u = u * kBase + mag_[i];
    }
    int64_t r;
    if (neg_) {
        r = -static_cast<int64_t>(u);
    } else {
        r = static_cast<int64_t>(u);
    }
    return r;
}

}  // namespace toolgood
