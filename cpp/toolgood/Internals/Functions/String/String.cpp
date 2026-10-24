// String.cpp - 对应 csharp/ToolGood.Algorithm/Internals/Functions/String/（26 个字符串函数）
// 说明：C# 的 string 以 UTF-16 码元为单位，这里统一按 UTF-8 码点处理字符级操作
//       （Lexer::ToCodePoints / FromCodePoints），字节级操作（大小写、替换、裁剪）保持原样。
#include "String.h"

#include <algorithm>
#include <cstdint>
#include <string>
#include <utility>
#include <vector>

#include "../../../AlgorithmEngine.h"
#include "../../../Lexer.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

// ---------------------------------------------------------------------------
// 通用辅助
// ---------------------------------------------------------------------------

using CodePoints = std::vector<uint32_t>;

CodePoints ToCps(const std::string& text) { return Lexer::ToCodePoints(text); }

std::string FromCps(const CodePoints& cps, size_t from, size_t to) {
    return Lexer::FromCodePoints(cps, from, to);
}

size_t CpLength(const std::string& text) { return Lexer::ToCodePoints(text).size(); }

/// 对应 C# FunctionBase.TryGetInt：decimal 超出 int 范围时返回 false。
bool TryGetInt(const Operand& arg, int& value) {
    value = 0;
    const Decimal& d = arg.NumberValue();
    if (!d.FitsInt32()) return false;
    value = d.Int();
    return true;
}

bool IsAsciiUpper(char c) { return c >= 'A' && c <= 'Z'; }
bool IsAsciiLower(char c) { return c >= 'a' && c <= 'z'; }

/// 只转换 ASCII a-z（对应 ToLowerInvariant 对非 ASCII 保持原样）
std::string ToLowerAscii(const std::string& s) {
    std::string r = s;
    for (auto& c : r) {
        if (c >= 'A' && c <= 'Z') c = static_cast<char>(c - 'A' + 'a');
    }
    return r;
}

/// 只转换 ASCII A-Z（对应 ToUpperInvariant 对非 ASCII 保持原样）
std::string ToUpperAscii(const std::string& s) {
    std::string r = s;
    for (auto& c : r) {
        if (c >= 'a' && c <= 'z') c = static_cast<char>(c - 'a' + 'A');
    }
    return r;
}

/// 对应 char.IsWhiteSpace（用于 TrimStart/TrimEnd 的无参版本）
bool IsWhiteSpaceCp(uint32_t c) {
    if (c == ' ' || (c >= 0x09 && c <= 0x0D)) return true;
    if (c == 0x85 || c == 0xA0 || c == 0x1680) return true;
    if (c >= 0x2000 && c <= 0x200A) return true;
    if (c == 0x2028 || c == 0x2029 || c == 0x202F || c == 0x205F || c == 0x3000) return true;
    return false;
}

/// 对应 char.IsLetter 的近似实现：ASCII 字母或非 ASCII 码点（CJK 等）
bool IsLetterCp(uint32_t c) {
    if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')) return true;
    return c >= 0x80;
}

uint32_t ToUpperInvariantCp(uint32_t c) {
    if (c >= 'a' && c <= 'z') return c - 32;
    return c;
}

uint32_t ToLowerInvariantCp(uint32_t c) {
    if (c >= 'A' && c <= 'Z') return c + 32;
    return c;
}

/// 码点级 IndexOf（ordinal）
int IndexOfCp(const CodePoints& text, const CodePoints& pat, size_t start) {
    if (start > text.size()) return -1;
    if (pat.empty()) return static_cast<int>(start);
    if (pat.size() > text.size()) return -1;
    for (size_t p = start; p + pat.size() <= text.size(); p++) {
        bool ok = true;
        for (size_t k = 0; k < pat.size(); k++) {
            if (text[p + k] != pat[k]) {
                ok = false;
                break;
            }
        }
        if (ok) return static_cast<int>(p);
    }
    return -1;
}

/// 全部替换（ordinal，不重叠）
std::string ReplaceAll(const std::string& text, const std::string& oldText,
                       const std::string& newText) {
    std::string out;
    out.reserve(text.size());
    size_t pos = 0;
    while (true) {
        size_t p = text.find(oldText, pos);
        if (p == std::string::npos) {
            out.append(text, pos, std::string::npos);
            break;
        }
        out.append(text, pos, p - pos);
        out += newText;
        pos = p + oldText.size();
    }
    return out;
}

// ---------------------------------------------------------------------------
// .NET 数字格式串（TEXT 需要；CultureInfo.InvariantCulture）
// 支持标准格式 C/E/F/G/N/P/R 与自定义格式（0/#/.//,%‰/E 与分节）
// ---------------------------------------------------------------------------

std::string RepeatChar(char c, size_t n) { return std::string(n, c); }

std::string TrimLeftZeros(const std::string& s) {
    size_t i = 0;
    while (i < s.size() && s[i] == '0') i++;
    return s.substr(i);
}

std::string TrimRightZeros(const std::string& s) {
    size_t e = s.size();
    while (e > 0 && s[e - 1] == '0') e--;
    return s.substr(0, e);
}

/// 十进制数字串 +1（用于四舍五入进位）
std::string IncrementDigits(const std::string& s) {
    std::string r = s;
    int i = static_cast<int>(r.size()) - 1;
    while (i >= 0) {
        if (r[static_cast<size_t>(i)] == '9') {
            r[static_cast<size_t>(i)] = '0';
            i--;
        } else {
            r[static_cast<size_t>(i)] = static_cast<char>(r[static_cast<size_t>(i)] + 1);
            break;
        }
    }
    if (i < 0) r = "1" + r;
    return r;
}

/// 非负十进制数：value = digits * 10^-scale
struct NumVal {
    std::string digits;
    int scale = 0;

    bool IsZero() const {
        for (char c : digits) {
            if (c != '0') return false;
        }
        return true;
    }

    /// 整数部分数字（无前导 0，可能为空串）
    std::string IntDigits() const {
        if (scale <= 0) return digits + RepeatChar('0', static_cast<size_t>(-scale));
        if (static_cast<int>(digits.size()) > scale) {
            return digits.substr(0, digits.size() - static_cast<size_t>(scale));
        }
        return std::string();
    }

    /// 小数部分数字（长度等于 scale）
    std::string FracDigits() const {
        if (scale <= 0) return std::string();
        if (static_cast<int>(digits.size()) >= scale) {
            return digits.substr(digits.size() - static_cast<size_t>(scale));
        }
        return RepeatChar('0', static_cast<size_t>(scale - static_cast<int>(digits.size()))) + digits;
    }

    NumVal Mul10(int k) const {
        NumVal v;
        v.digits = digits;
        v.scale = scale - k;
        return v;
    }

    NumVal Div10(int k) const {
        NumVal v;
        v.digits = digits;
        v.scale = scale + k;
        return v;
    }

    /// 四舍五入（远离零）到 frac 位小数
    NumVal RoundTo(int frac) const {
        if (scale <= frac) return *this;
        const int drop = scale - frac;
        std::string d = digits;
        if (static_cast<int>(d.size()) < scale + 1) {
            d = RepeatChar('0', static_cast<size_t>(scale + 1 - static_cast<int>(d.size()))) + d;
        }
        std::string keep = d.substr(0, d.size() - static_cast<size_t>(drop));
        const std::string rest = d.substr(d.size() - static_cast<size_t>(drop));
        if (!rest.empty() && rest[0] >= '5') keep = IncrementDigits(keep);
        NumVal v;
        v.digits = keep;
        v.scale = frac;
        return v;
    }

    /// 去尾零的普通表示（至少保留 "0"）
    std::string Plain() const {
        std::string intPart = TrimLeftZeros(IntDigits());
        std::string fracPart = TrimRightZeros(FracDigits());
        if (intPart.empty()) intPart = "0";
        if (fracPart.empty()) return intPart;
        return intPart + "." + fracPart;
    }

    /// 定点表示：保留 frac 位小数
    std::string Fixed(int frac) const {
        NumVal r = RoundTo(frac);
        std::string intPart = TrimLeftZeros(r.IntDigits());
        std::string fracPart = r.FracDigits();
        if (intPart.empty()) intPart = "0";
        if (frac > 0) {
            if (static_cast<int>(fracPart.size()) < frac) {
                fracPart += RepeatChar('0', static_cast<size_t>(frac - static_cast<int>(fracPart.size())));
            }
            return intPart + "." + fracPart;
        }
        return intPart;
    }
};

NumVal NumValFromDecimal(const Decimal& d) {
    const std::string s = d.Abs().ToString();
    std::string intPart = s;
    std::string fracPart;
    const size_t dot = s.find('.');
    if (dot != std::string::npos) {
        intPart = s.substr(0, dot);
        fracPart = s.substr(dot + 1);
    }
    NumVal v;
    v.digits = intPart + fracPart;
    v.scale = static_cast<int>(fracPart.size());
    return v;
}

/// 首个有效数字对应的十进制指数
int SignificantExponent(const NumVal& v) {
    const std::string intPart = TrimLeftZeros(v.IntDigits());
    if (!intPart.empty()) return static_cast<int>(intPart.size()) - 1;
    const std::string frac = v.FracDigits();
    size_t zeros = 0;
    while (zeros < frac.size() && frac[zeros] == '0') zeros++;
    return -static_cast<int>(zeros + 1);
}

std::string FormatExponentDigits(int exp, int digits, char signChar) {
    std::string sign = "+";
    if (exp < 0) {
        sign = "-";
        exp = -exp;
    } else if (signChar == '-') {
        sign.clear();
    }
    std::string text = std::to_string(exp);
    while (static_cast<int>(text.size()) < digits) text = "0" + text;
    return sign + text;
}

/// 在整数部分插入千分位分隔符（不含符号）
std::string GroupDigits(const std::string& intDigits, int groupSize) {
    if (groupSize <= 0 || static_cast<int>(intDigits.size()) <= groupSize) return intDigits;
    std::string sb;
    for (size_t i = 0; i < intDigits.size(); i++) {
        if (i > 0 && (intDigits.size() - i) % static_cast<size_t>(groupSize) == 0) sb.push_back(',');
        sb.push_back(intDigits[i]);
    }
    return sb;
}

/// 对定点表示（"1234.56"）的整数部分插入千分位
std::string GroupFixedText(const std::string& fixed, int groupSize) {
    const size_t dot = fixed.find('.');
    if (dot == std::string::npos) return GroupDigits(fixed, groupSize);
    return GroupDigits(fixed.substr(0, dot), groupSize) + fixed.substr(dot);
}

bool IsStandardPattern(const std::string& pattern) {
    switch (pattern[0]) {
        case 'C': case 'c': case 'D': case 'd': case 'E': case 'e': case 'F': case 'f':
        case 'G': case 'g': case 'N': case 'n': case 'P': case 'p': case 'R': case 'r':
        case 'X': case 'x':
            break;
        default:
            return false;
    }
    for (size_t i = 1; i < pattern.size(); i++) {
        if (pattern[i] < '0' || pattern[i] > '9') return false;
    }
    return true;
}

bool FormatExponent(const Decimal& d, int prec, char spec, int expDigits, std::string& out) {
    const bool neg = d.Sign() < 0;
    const NumVal v = NumValFromDecimal(d);
    const char e = (spec >= 'a') ? 'e' : 'E';
    if (v.IsZero()) {
        std::string mantissa = "0";
        if (prec > 0) mantissa += "." + RepeatChar('0', static_cast<size_t>(prec));
        out = mantissa + e + FormatExponentDigits(0, expDigits, '+');
        return true;
    }
    int exp = SignificantExponent(v);
    NumVal m = v.Mul10(-exp).RoundTo(prec);
    if (TrimLeftZeros(m.IntDigits()).size() > 1) {
        // 进位导致整数部分变成两位（如 9.99 -> 10.0）
        exp++;
        m = v.Mul10(-exp).RoundTo(prec);
    }
    std::string intPart = TrimLeftZeros(m.IntDigits());
    std::string fracPart = m.FracDigits();
    if (intPart.empty()) intPart = "0";
    if (prec > 0) {
        if (static_cast<int>(fracPart.size()) < prec) {
            fracPart += RepeatChar('0', static_cast<size_t>(prec - static_cast<int>(fracPart.size())));
        }
        fracPart = fracPart.substr(0, static_cast<size_t>(prec));
    } else {
        fracPart.clear();
    }
    std::string text = intPart;
    if (prec > 0) text += "." + fracPart;
    text += e + FormatExponentDigits(exp, expDigits, '+');
    out = neg ? "-" + text : text;
    return true;
}

/// 对应 .NET 的 G 格式
bool FormatGeneral(const Decimal& d, int prec, bool limited, std::string& out) {
    if (!limited) {
        out = d.ToString();
        return true;
    }
    const bool neg = d.Sign() < 0;
    const NumVal v = NumValFromDecimal(d);
    if (v.IsZero()) {
        out = "0";
        return true;
    }
    int exp = SignificantExponent(v);
    NumVal m = v.Mul10(-exp).RoundTo(prec - 1);
    if (TrimLeftZeros(m.IntDigits()).size() > 1) {
        exp++;
        m = v.Mul10(-exp).RoundTo(prec - 1);
    }
    std::string text;
    if (exp < -4 || exp >= prec) {
        text = m.Plain() + "E" + FormatExponentDigits(exp, 2, '+');
    } else {
        text = m.Mul10(exp).Plain();
    }
    out = neg ? "-" + text : text;
    return true;
}

bool FormatStandard(const Decimal& d, char spec, int prec, bool hasPrec, std::string& out) {
    const char letter = static_cast<char>(spec | 0x20);
    const NumVal v = NumValFromDecimal(d);
    bool neg = d.Sign() < 0;
    if (letter == 'c' || letter == 'f' || letter == 'n' || letter == 'p') {
        const int p = hasPrec ? prec : 2;
        NumVal rounded = v;
        if (letter == 'p') rounded = rounded.Mul10(2);
        if (rounded.RoundTo(p).IsZero()) neg = false;
    }
    switch (letter) {
        case 'd':
        case 'x':
            // decimal 不支持整数/十六进制格式，C# 抛 FormatException
            return false;
        case 'c': {
            const int p = hasPrec ? prec : 2;
            const std::string body = "\xC2\xA4" + GroupFixedText(v.Fixed(p), 3);
            out = neg ? "(" + body + ")" : body;
            return true;
        }
        case 'f': {
            const int p = hasPrec ? prec : 2;
            const std::string text = v.Fixed(p);
            out = neg ? "-" + text : text;
            return true;
        }
        case 'n': {
            const int p = hasPrec ? prec : 2;
            const std::string text = GroupFixedText(v.Fixed(p), 3);
            out = neg ? "-" + text : text;
            return true;
        }
        case 'p': {
            const int p = hasPrec ? prec : 2;
            const std::string text = GroupFixedText(v.Mul10(2).Fixed(p), 3) + " %";
            out = neg ? "-" + text : text;
            return true;
        }
        case 'e': {
            const int p = hasPrec ? prec : 6;
            return FormatExponent(d, p, spec, 3, out);
        }
        case 'g':
            return FormatGeneral(d, prec, hasPrec && prec > 0, out);
        default:  // 'r'
            out = d.ToString();
            return true;
    }
}

enum class FmtKind { Literal, DigitZero, DigitHash, Point, Comma, Exponent };

struct FmtToken {
    FmtKind kind = FmtKind::Literal;
    std::string text;  // Literal 的 UTF-8 文本
    int expDigits = 0;
    char expSign = '+';
};

struct FmtMeta {
    int intPlaceholders = 0;
    int intZeros = 0;
    int fracPlaceholders = 0;
    int fracZeros = 0;
    bool hasPoint = false;
    bool grouping = false;
    int groupSize = 0;
    int scaling = 0;
    bool percent = false;
    bool permille = false;
    bool hasExponent = false;
};

/// 按顶层 ';' 切分格式串（引号与反斜杠内的分号不切分）
std::vector<std::vector<uint32_t>> SplitFormatSections(const CodePoints& pattern) {
    std::vector<std::vector<uint32_t>> sections;
    std::vector<uint32_t> cur;
    const size_t n = pattern.size();
    for (size_t i = 0; i < n; i++) {
        const uint32_t c = pattern[i];
        if (c == '\'' || c == '"') {
            const uint32_t quote = c;
            cur.push_back(c);
            i++;
            while (i < n) {
                cur.push_back(pattern[i]);
                if (pattern[i] == quote) {
                    if (i + 1 < n && pattern[i + 1] == quote) {
                        cur.push_back(pattern[i + 1]);
                        i += 2;
                        continue;
                    }
                    break;
                }
                i++;
            }
        } else if (c == '\\' && i + 1 < n) {
            cur.push_back(c);
            cur.push_back(pattern[i + 1]);
            i++;
        } else if (c == ';') {
            sections.push_back(cur);
            cur.clear();
        } else {
            cur.push_back(c);
        }
    }
    sections.push_back(cur);
    return sections;
}

/// 尝试解析指数部分（E[+|-]0...），返回位数、符号与结束下标
bool ParseExponentPart(const CodePoints& runes, size_t start, int& digits, char& sign, size_t& next) {
    size_t i = start + 1;
    sign = '+';
    if (i < runes.size() && (runes[i] == '+' || runes[i] == '-')) {
        sign = static_cast<char>(runes[i]);
        i++;
    }
    int count = 0;
    while (i < runes.size() && (runes[i] == '0' || runes[i] == '#')) {
        count++;
        i++;
    }
    if (count == 0) return false;
    digits = count;
    next = i;
    return true;
}

bool TokenizeFormat(const std::vector<uint32_t>& section, std::vector<FmtToken>& tokens,
                    FmtMeta& meta) {
    int intDigits = 0;                 // 整数区数字占位符个数
    int lastIntDigit = -1;             // 整数区最后一个数字占位符的 token 下标
    std::vector<int> commaIdx;         // 整数区逗号位置
    bool inFrac = false;
    const size_t n = section.size();
    size_t i = 0;
    while (i < n) {
        const uint32_t c = section[i];
        if (c == '\'' || c == '"') {
            const uint32_t quote = c;
            std::vector<uint32_t> buf;
            i++;
            bool closed = false;
            while (i < n) {
                if (section[i] == quote) {
                    if (i + 1 < n && section[i + 1] == quote) {
                        buf.push_back(quote);
                        i += 2;
                        continue;
                    }
                    i++;
                    closed = true;
                    break;
                }
                buf.push_back(section[i]);
                i++;
            }
            if (!closed) return false;
            FmtToken tok;
            tok.kind = FmtKind::Literal;
            tok.text = FromCps(buf, 0, buf.size());
            tokens.push_back(tok);
        } else if (c == '\\' && i + 1 < n) {
            const std::vector<uint32_t> buf{section[i + 1]};
            FmtToken tok;
            tok.kind = FmtKind::Literal;
            tok.text = FromCps(buf, 0, 1);
            tokens.push_back(tok);
            i += 2;
        } else if (c == '0' || c == '#') {
            FmtToken tok;
            tok.kind = (c == '0') ? FmtKind::DigitZero : FmtKind::DigitHash;
            tokens.push_back(tok);
            if (inFrac) {
                meta.fracPlaceholders++;
                if (c == '0') meta.fracZeros++;
            } else {
                meta.intPlaceholders++;
                intDigits++;
                if (c == '0') meta.intZeros++;
                lastIntDigit = static_cast<int>(tokens.size()) - 1;
            }
            i++;
        } else if (c == '.') {
            if (!meta.hasPoint && !inFrac) {
                meta.hasPoint = true;
                inFrac = true;
                FmtToken tok;
                tok.kind = FmtKind::Point;
                tokens.push_back(tok);
            } else {
                FmtToken tok;
                tok.kind = FmtKind::Literal;
                tok.text = ".";
                tokens.push_back(tok);
            }
            i++;
        } else if (c == ',') {
            if (!inFrac) commaIdx.push_back(static_cast<int>(tokens.size()));
            FmtToken tok;
            tok.kind = FmtKind::Comma;
            tokens.push_back(tok);
            i++;
        } else if (c == '%') {
            meta.percent = true;
            FmtToken tok;
            tok.kind = FmtKind::Literal;
            tok.text = "%";
            tokens.push_back(tok);
            i++;
        } else if (c == 0x2030) {  // ‰
            meta.permille = true;
            FmtToken tok;
            tok.kind = FmtKind::Literal;
            const std::vector<uint32_t> buf{c};
            tok.text = FromCps(buf, 0, 1);
            tokens.push_back(tok);
            i++;
        } else if (c == 'E' || c == 'e') {
            int digits = 0;
            char sign = '+';
            size_t next = i;
            const bool ok = ParseExponentPart(section, i, digits, sign, next);
            if (ok && intDigits > 0) {
                FmtToken tok;
                tok.kind = FmtKind::Exponent;
                tok.expDigits = digits;
                tok.expSign = sign;
                tokens.push_back(tok);
                meta.hasExponent = true;
                i = next;
                continue;
            }
            FmtToken tok;
            tok.kind = FmtKind::Literal;
            if (ok) {
                // 没有数字占位符时 "E+0" 整体按字面量输出
                tok.text = FromCps(section, i, next);
                i = next;
            } else {
                tok.text = FromCps(section, i, i + 1);
                i++;
            }
            tokens.push_back(tok);
        } else {
            FmtToken tok;
            tok.kind = FmtKind::Literal;
            tok.text = FromCps(section, i, i + 1);
            tokens.push_back(tok);
            i++;
        }
    }

    // 分组与末尾缩放：整数区中最后一个数字占位符之后的逗号为缩放标记
    for (int idx : commaIdx) {
        if (lastIntDigit < 0 || idx > lastIntDigit) {
            if (lastIntDigit >= 0) meta.scaling++;
            continue;
        }
        int count = 0;
        for (int k = idx + 1; k <= lastIntDigit; k++) {
            if (tokens[static_cast<size_t>(k)].kind == FmtKind::DigitZero ||
                tokens[static_cast<size_t>(k)].kind == FmtKind::DigitHash) {
                count++;
            }
        }
        if (count <= 0) return false;
        meta.grouping = true;
        // 分组长度取自 NumberFormatInfo.NumberGroupSizes（InvariantCulture 恒为 3）
        meta.groupSize = 3;
    }
    if (meta.percent || meta.permille) {
        if (meta.intPlaceholders + meta.fracPlaceholders == 0) {
            meta.percent = false;
            meta.permille = false;
        }
    }
    return true;
}

std::string RenderCustomTokens(const std::vector<FmtToken>& tokens, const FmtMeta& meta,
                               const std::string& intDigits, const std::string& fracDigits, int exp,
                               bool showMinus) {
    // 把整数部分数字右对齐分配到各占位符（位数过多时首个占位符吞下多余数字）
    std::vector<std::string> slots(static_cast<size_t>(meta.intPlaceholders));
    if (meta.intPlaceholders > 0) {
        const int k = static_cast<int>(intDigits.size()) - meta.intPlaceholders;
        if (k >= 0) {
            for (int i = 0; i < meta.intPlaceholders; i++) {
                slots[static_cast<size_t>(i)] = std::string(1, intDigits[static_cast<size_t>(i + k)]);
            }
            if (k > 0) slots[0] = intDigits.substr(0, static_cast<size_t>(k) + 1);
        } else {
            const int start = -k;
            for (int i = start; i < meta.intPlaceholders; i++) {
                slots[static_cast<size_t>(i)] =
                    std::string(1, intDigits[static_cast<size_t>(i - start)]);
            }
        }
    }

    std::string sb;
    if (showMinus) sb.push_back('-');
    if (meta.intPlaceholders == 0 && !intDigits.empty()) sb += intDigits;
    const bool groupedMode = meta.grouping && meta.intPlaceholders > 0;
    std::string intAll;
    if (groupedMode) intAll = GroupDigits(intDigits, meta.groupSize);
    int intIdx = 0;
    size_t fracIdx = 0;
    bool pastPoint = false;
    for (const FmtToken& tok : tokens) {
        switch (tok.kind) {
            case FmtKind::Literal:
                sb += tok.text;
                break;
            case FmtKind::DigitZero:
            case FmtKind::DigitHash:
                if (pastPoint) {
                    if (fracIdx < fracDigits.size()) {
                        sb.push_back(fracDigits[fracIdx]);
                    } else if (tok.kind == FmtKind::DigitZero) {
                        sb.push_back('0');
                    }
                    fracIdx++;
                    continue;
                }
                if (groupedMode) {
                    if (intIdx == 0) sb += intAll;
                    intIdx++;
                    continue;
                }
                if (intIdx < static_cast<int>(slots.size())) {
                    sb += slots[static_cast<size_t>(intIdx)];
                    intIdx++;
                }
                break;
            case FmtKind::Point:
                pastPoint = true;
                // .NET 只有存在小数位时才输出小数点
                if (!fracDigits.empty()) sb.push_back('.');
                break;
            case FmtKind::Comma:
                break;
            case FmtKind::Exponent:
                sb.push_back('E');
                if (tok.expSign == '-') {
                    sb += FormatExponentDigits(exp, tok.expDigits, '-');
                } else {
                    sb += FormatExponentDigits(exp, tok.expDigits, '+');
                }
                break;
        }
    }
    return sb;
}

std::string RenderCustomExponent(const NumVal& v, const std::vector<FmtToken>& tokens,
                                 const FmtMeta& meta, bool showMinus) {
    int exp = 0;
    if (!v.IsZero()) exp = SignificantExponent(v);
    NumVal m = v.Mul10(-exp).RoundTo(meta.fracPlaceholders);
    if (TrimLeftZeros(m.IntDigits()).size() > 1) {
        exp++;
        m = v.Mul10(-exp).RoundTo(meta.fracPlaceholders);
    }
    std::string intDigits = TrimLeftZeros(m.IntDigits());
    std::string fracDigits = m.FracDigits();
    if (static_cast<int>(fracDigits.size()) < meta.fracPlaceholders) {
        fracDigits += RepeatChar(
            '0', static_cast<size_t>(meta.fracPlaceholders - static_cast<int>(fracDigits.size())));
    }
    if (static_cast<int>(fracDigits.size()) > meta.fracPlaceholders) {
        fracDigits = fracDigits.substr(0, static_cast<size_t>(meta.fracPlaceholders));
    }
    if (static_cast<int>(intDigits.size()) < meta.intZeros) {
        intDigits = RepeatChar(
                       '0', static_cast<size_t>(meta.intZeros - static_cast<int>(intDigits.size()))) +
                   intDigits;
    }
    while (static_cast<int>(fracDigits.size()) > meta.fracZeros && !fracDigits.empty() &&
           fracDigits.back() == '0') {
        fracDigits.pop_back();
    }
    return RenderCustomTokens(tokens, meta, intDigits, fracDigits, exp, showMinus);
}

bool FormatCustom(const Decimal& d, const std::string& pattern, std::string& out) {
    std::vector<std::vector<uint32_t>> sections = SplitFormatSections(ToCps(pattern));
    if (sections.size() > 3) sections.resize(3);

    const bool neg = d.Sign() < 0;
    struct Section {
        std::vector<FmtToken> tokens;
        FmtMeta meta;
    };
    std::vector<Section> parsed(sections.size());
    int maxFrac = 0;
    for (size_t i = 0; i < sections.size(); i++) {
        if (!TokenizeFormat(sections[i], parsed[i].tokens, parsed[i].meta)) return false;
        if (parsed[i].meta.fracPlaceholders > maxFrac) maxFrac = parsed[i].meta.fracPlaceholders;
    }

    // .NET 先用格式串的小数位数舍入，再按舍入后的正负/是否为零选择分节
    NumVal pre = NumValFromDecimal(d);
    if (parsed[0].meta.percent) pre = pre.Mul10(2);
    if (parsed[0].meta.permille) pre = pre.Mul10(3);
    if (parsed[0].meta.scaling > 0) pre = pre.Div10(3 * parsed[0].meta.scaling);
    const bool roundedToZero = pre.RoundTo(maxFrac).IsZero();

    size_t sectionIndex = 0;
    if (roundedToZero && parsed.size() >= 3) {
        sectionIndex = 2;
    } else if (roundedToZero) {
        sectionIndex = 0;
    } else if (neg && parsed.size() >= 2) {
        sectionIndex = 1;
    }

    // 分节为空时回退到第一段（负分节回退时补负号），与 .NET 行为一致
    bool forceMinus = false;
    if (sectionIndex == 1 && sections[1].empty()) {
        sectionIndex = 0;
        forceMinus = true;
    }
    if (sectionIndex == 2 && sections[2].empty()) sectionIndex = 0;
    const bool withMinus = sections.size() < 2 || forceMinus;

    const std::vector<FmtToken>& tokens = parsed[sectionIndex].tokens;
    const FmtMeta& meta = parsed[sectionIndex].meta;
    const bool hasPlaceholders = meta.intPlaceholders + meta.fracPlaceholders > 0;

    NumVal v = NumValFromDecimal(d);
    if (meta.percent) v = v.Mul10(2);
    if (meta.permille) v = v.Mul10(3);
    if (meta.scaling > 0) v = v.Div10(3 * meta.scaling);

    if (!hasPlaceholders) {
        std::string sb;
        if (neg && withMinus && !v.IsZero()) sb.push_back('-');
        for (const FmtToken& tok : tokens) {
            if (tok.kind == FmtKind::Literal) sb += tok.text;
        }
        out = sb;
        return true;
    }

    if (meta.hasExponent) {
        out = RenderCustomExponent(v, tokens, meta, neg && withMinus && !v.IsZero());
        return true;
    }

    v = v.RoundTo(meta.fracPlaceholders);
    // 舍入为 0 时 .NET 不输出负号
    const bool showMinus = neg && withMinus && !v.IsZero();
    std::string intDigits = TrimLeftZeros(v.IntDigits());
    std::string fracDigits = v.FracDigits();
    if (static_cast<int>(fracDigits.size()) < meta.fracPlaceholders) {
        fracDigits += RepeatChar(
            '0', static_cast<size_t>(meta.fracPlaceholders - static_cast<int>(fracDigits.size())));
    }
    if (static_cast<int>(fracDigits.size()) > meta.fracPlaceholders) {
        fracDigits = fracDigits.substr(0, static_cast<size_t>(meta.fracPlaceholders));
    }
    if (static_cast<int>(intDigits.size()) < meta.intZeros) {
        intDigits = RepeatChar(
                       '0', static_cast<size_t>(meta.intZeros - static_cast<int>(intDigits.size()))) +
                   intDigits;
    }
    while (static_cast<int>(fracDigits.size()) > meta.fracZeros && !fracDigits.empty() &&
           fracDigits.back() == '0') {
        fracDigits.pop_back();
    }
    out = RenderCustomTokens(tokens, meta, intDigits, fracDigits, 0, showMinus);
    return true;
}

/// decimal.ToString(format, CultureInfo.InvariantCulture)
bool FormatNumber(const Decimal& d, const std::string& pattern, std::string& out) {
    if (pattern.empty()) {
        out = d.ToString();
        return true;
    }
    if (pattern.size() == 1) {
        const char spec = pattern[0];
        switch (spec) {
            case 'C': case 'c': case 'E': case 'e': case 'F': case 'f':
            case 'G': case 'g': case 'N': case 'n': case 'P': case 'p':
            case 'R': case 'r':
                return FormatStandard(d, spec, 0, false, out);
            case 'D': case 'd': case 'X': case 'x':
                return false;
            default:
                break;
        }
        if ((spec >= 'A' && spec <= 'Z') || (spec >= 'a' && spec <= 'z')) return false;
        return FormatCustom(d, pattern, out);
    }
    if (IsStandardPattern(pattern)) {
        int prec = 0;
        for (size_t i = 1; i < pattern.size(); i++) prec = prec * 10 + (pattern[i] - '0');
        return FormatStandard(d, pattern[0], prec, true, out);
    }
    return FormatCustom(d, pattern, out);
}

// ---------------------------------------------------------------------------
// 人民币大写（对应 Function_RMB 的格式串 + 正则消零结果）
// ---------------------------------------------------------------------------

const char* const kRmbNumber[] = {"零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"};

/// 每 4 位一组的组单位（组序号 0 为个位组）
std::string RmbGroupUnit(int group) {
    switch (group) {
        case 1: return "万";
        case 2: return "亿";
        case 3: return "兆";
        case 4: return "京";
        case 5: return "垓";
        case 6: return "秭";
        case 7: return "穰";
        default: return std::string();
    }
}

std::string ToChineseRmb(const Decimal& x) {
    if (x.IsZero()) return std::string();
    // 先放大 100 倍并按四舍五入取整（与格式串 "0B0A" 的舍入一致）
    const Decimal cents = x.Abs().Mul(Decimal::FromInt(100)).RoundHalfUp(0);
    std::string digits = cents.Trunc().ToString();
    if (digits.size() < 2) digits = std::string(2 - digits.size(), '0') + digits;

    const int jiao = digits[digits.size() - 2] - '0';
    const int fen = digits[digits.size() - 1] - '0';
    const std::string yuan = TrimLeftZeros(digits.substr(0, digits.size() - 2));

    std::string out;
    if (!yuan.empty()) {
        // 按 4 位一组（从右往左）处理，组内再按 仟佰拾 输出
        std::vector<int> groups;  // 低位组在前
        for (int end = static_cast<int>(yuan.size()); end > 0; end -= 4) {
            const int begin = end - 4 > 0 ? end - 4 : 0;
            groups.push_back(std::stoi(yuan.substr(static_cast<size_t>(begin),
                                                   static_cast<size_t>(end - begin))));
        }
        bool zeroPending = false;
        for (int g = static_cast<int>(groups.size()) - 1; g >= 0; g--) {
            const int value = groups[static_cast<size_t>(g)];
            if (value == 0) {
                if (!out.empty()) zeroPending = true;
                continue;
            }
            if (!out.empty() && (zeroPending || value < 1000)) out += "零";
            zeroPending = false;
            static const int kUnits[4] = {1000, 100, 10, 1};
            bool started = false;
            bool inZero = false;
            for (int k = 0; k < 4; k++) {
                const int d = (value / kUnits[k]) % 10;
                if (d == 0) {
                    if (started) inZero = true;
                    continue;
                }
                if (inZero) {
                    out += "零";
                    inZero = false;
                }
                out += kRmbNumber[d];
                if (kUnits[k] == 1000) out += "仟";
                else if (kUnits[k] == 100) out += "佰";
                else if (kUnits[k] == 10) out += "拾";
                started = true;
            }
            out += RmbGroupUnit(g);
        }
        out += "元";
    }

    if (jiao > 0) {
        out += kRmbNumber[jiao];
        out += "角";
    } else if (fen > 0 && !out.empty()) {
        out += "零";
    }
    if (fen > 0) {
        out += kRmbNumber[fen];
        out += "分";
    }
    return out;
}

// ---------------------------------------------------------------------------
// SEARCH 的通配符匹配（大小写不敏感，支持 ? * ~ 转义）
// ---------------------------------------------------------------------------

enum class WildcardKind { Literal, AnyOne, AnySeq };

struct WildcardToken {
    WildcardKind kind = WildcardKind::Literal;
    uint32_t value = 0;
};

std::vector<WildcardToken> ParseWildcardPattern(const std::string& pattern) {
    const CodePoints cps = ToCps(pattern);
    std::vector<WildcardToken> list;
    list.reserve(cps.size());
    for (size_t i = 0; i < cps.size(); i++) {
        const uint32_t c = cps[i];
        WildcardToken tok;
        if (c == '~') {
            if (i + 1 < cps.size() && (cps[i + 1] == '?' || cps[i + 1] == '*' || cps[i + 1] == '~')) {
                tok.kind = WildcardKind::Literal;
                tok.value = ToUpperInvariantCp(cps[i + 1]);
                i++;
            } else {
                tok.kind = WildcardKind::Literal;
                tok.value = '~';
            }
        } else if (c == '*') {
            tok.kind = WildcardKind::AnySeq;
        } else if (c == '?') {
            tok.kind = WildcardKind::AnyOne;
        } else {
            tok.kind = WildcardKind::Literal;
            tok.value = ToUpperInvariantCp(c);
        }
        list.push_back(tok);
    }
    return list;
}

/// 判断 pattern 是否从 text 的 start 位置开始完整匹配（贪心 + 回溯）
bool MatchWildcard(const CodePoints& text, const std::vector<WildcardToken>& tokens, size_t start) {
    size_t ti = 0;
    size_t si = start;
    int starToken = -1;
    size_t starMatch = 0;
    while (ti < tokens.size()) {
        if (si < text.size()) {
            const WildcardToken& tok = tokens[ti];
            if (tok.kind == WildcardKind::Literal) {
                if (ToUpperInvariantCp(text[si]) == tok.value) {
                    ti++;
                    si++;
                } else if (starToken != -1) {
                    ti = static_cast<size_t>(starToken) + 1;
                    si = ++starMatch;
                } else {
                    return false;
                }
            } else if (tok.kind == WildcardKind::AnyOne) {
                ti++;
                si++;
            } else {
                starToken = static_cast<int>(ti);
                starMatch = si;
                ti++;
            }
        } else {
            break;
        }
    }
    while (ti < tokens.size() && tokens[ti].kind == WildcardKind::AnySeq) ti++;
    return ti == tokens.size();
}

int WildcardIndexOf(const CodePoints& text, const std::string& pattern, size_t startIndex) {
    if (startIndex > text.size()) return -1;
    const std::vector<WildcardToken> tokens = ParseWildcardPattern(pattern);
    if (tokens.empty()) return static_cast<int>(startIndex);
    for (size_t start = startIndex; start <= text.size(); start++) {
        if (MatchWildcard(text, tokens, start)) return static_cast<int>(start);
    }
    return -1;
}

// ---------------------------------------------------------------------------
// ASC / JIS 的全角半角转换
// ---------------------------------------------------------------------------

std::string ToDBC(const std::string& input) {
    CodePoints cps = ToCps(input);
    bool needModify = false;
    for (uint32_t c : cps) {
        if (c == 12288 || (c > 65280 && c < 65375)) {
            needModify = true;
            break;
        }
    }
    if (!needModify) return input;
    for (auto& c : cps) {
        if (c == 12288) {
            c = 32;
        } else if (c > 65280 && c < 65375) {
            c = c - 65248;
        }
    }
    return FromCps(cps, 0, cps.size());
}

std::string ToSBC(const std::string& input) {
    CodePoints cps = ToCps(input);
    bool needModify = false;
    for (uint32_t c : cps) {
        if (c == ' ' || (c > 32 && c < 127)) {
            needModify = true;
            break;
        }
    }
    if (!needModify) return input;
    for (auto& c : cps) {
        if (c == ' ') {
            c = 12288;
        } else if (c > 32 && c < 127) {
            c = c + 65248;
        }
    }
    return FromCps(cps, 0, cps.size());
}

// ---------------------------------------------------------------------------
// VALUE 的宽松解析（NumberStyles.Any，InvariantCulture）
// ---------------------------------------------------------------------------

bool TryParseAnyNumber(const std::string& text, Decimal& out) {
    const CodePoints cps = ToCps(text);
    size_t b = 0;
    size_t e = cps.size();
    while (b < e && IsWhiteSpaceCp(cps[b])) b++;
    while (e > b && IsWhiteSpaceCp(cps[e - 1])) e--;
    if (b == e) return false;

    bool neg = false;
    if (cps[b] == '(') {
        if (cps[e - 1] != ')') return false;
        neg = true;
        b++;
        e--;
        if (b == e) return false;
    }
    // 货币符号（InvariantCulture 为 ¤，可选）
    if (cps[b] == 0xA4) {
        b++;
        if (b == e) return false;
    }
    std::string norm;
    for (size_t i = b; i < e; i++) {
        const uint32_t c = cps[i];
        if (c == ',') continue;  // AllowThousands
        if (c < 128) {
            norm.push_back(static_cast<char>(c));
        } else {
            return false;
        }
    }
    Decimal d;
    if (!Decimal::TryParse(norm, d)) return false;
    if (d.Abs().Cmp(Decimal::Max) > 0) return false;
    out = neg ? d.Neg() : d;
    return true;
}

// ---------------------------------------------------------------------------
// 函数实现
// ---------------------------------------------------------------------------

class Function_ASC : public FunctionN {
public:
    explicit Function_ASC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Asc", 1, 1);
    }

    std::string Name() const override { return "Asc"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToDBC(args1.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_CHAR : public FunctionN {
public:
    explicit Function_CHAR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Char", 1, 1);
    }

    std::string Name() const override { return "Char"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        int code = 0;
        if (!TryGetInt(args1, code)) return ParameterError(1);
        if (code < 0 || code > 65535) return ParameterError(1);
        const CodePoints cps{static_cast<uint32_t>(code)};
        return Operand::Create(FromCps(cps, 0, 1));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_CLEAN : public FunctionN {
public:
    explicit Function_CLEAN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Clean", 1, 1);
    }

    std::string Name() const override { return "Clean"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const std::string t = args1.TextValue();
        bool needClean = false;
        for (char ch : t) {
            if (static_cast<unsigned char>(ch) < 32) {
                needClean = true;
                break;
            }
        }
        if (!needClean) return args1;
        std::string sb;
        sb.reserve(t.size());
        for (char ch : t) {
            if (static_cast<unsigned char>(ch) >= 32) sb.push_back(ch);
        }
        return Operand::Create(sb);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_CODE : public FunctionN {
public:
    explicit Function_CODE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Code", 1, 1);
    }

    std::string Name() const override { return "Code"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const CodePoints cps = ToCps(args1.TextValue());
        if (cps.empty()) return ParameterError(1);
        return Operand::Create(static_cast<int>(cps[0]));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_CONCATENATE : public FunctionN {
public:
    explicit Function_CONCATENATE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Concatenate", 1, -1);
    }

    std::string Name() const override { return "Concatenate"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        for (auto& f : args_) {
            f->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        const int n = static_cast<int>(ArgCount());
        if (n == 0) return Operand::Create(std::string());
        if (n == 1) {
            auto a = GetText(1, engine, tp);
            if (a.IsErrorOrNone()) return a;
            return a;  // 只有一个参数
        }
        std::string sb;
        for (int i = 1; i <= n; i++) {
            auto a = GetText(i, engine, tp);
            if (a.IsErrorOrNone()) return a;
            sb += a.TextValue();
        }
        return Operand::Create(sb);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_EXACT : public FunctionN {
public:
    explicit Function_EXACT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Exact", 2, 2);
    }

    std::string Name() const override { return "Exact"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        const OperandType t1 = args_[0]->GetResultType();
        const OperandType t2 = args_[1]->GetResultType();
        if (t1 == OperandType::NONE) {
            auto p = args_[1]->Evaluate(noneEngine, nullptr).ToText();
            if (t2 != OperandType::ERROR && !p.IsErrorOrNone()) {
                args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT, "==",
                                            p.TextValue());
                args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
                return;
            }
        } else if (t2 == OperandType::NONE) {
            auto p = args_[0]->Evaluate(noneEngine, nullptr).ToText();
            if (t1 != OperandType::ERROR && !p.IsErrorOrNone()) {
                args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT, "==",
                                            p.TextValue());
                args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
                return;
            }
        }
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        return Operand::Create(args1.TextValue() == args2.TextValue());
    }

    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_FIND : public FunctionN {
public:
    explicit Function_FIND(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Find", 2, 3);
    }

    std::string Name() const override { return "Find"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() >= 3) {
            args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const CodePoints pat = ToCps(args1.TextValue());
        const CodePoints text = ToCps(args2.TextValue());
        const int excelIndex = engine.ExcelIndex;

        if (ArgCount() == 2) {
            const int index = IndexOfCp(text, pat, 0);
            if (index < 0) {
                // 未找到: Excel 模式(索引从1开始)返回错误, C# 模式(索引从0开始)返回 -1
                return excelIndex == 1 ? FunctionError() : Operand::Create(-1);
            }
            return Operand::Create(index + excelIndex);
        }

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        int startValue = 0;
        if (!TryGetInt(args3, startValue)) return ParameterError(3);
        const long long startIndex = static_cast<long long>(startValue) - excelIndex;
        if (startIndex < 0 || startIndex >= static_cast<long long>(text.size())) {
            return ParameterError(3);
        }
        // IndexOfCp 返回的是绝对下标（C# 的 AsSpan(startIndex).IndexOf 返回相对下标）
        const int p2 = IndexOfCp(text, pat, static_cast<size_t>(startIndex));
        if (p2 < 0) {
            return excelIndex == 1 ? FunctionError() : Operand::Create(-1);
        }
        return Operand::Create(p2 + excelIndex);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_JIS : public FunctionN {
public:
    explicit Function_JIS(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Jis", 1, 1);
    }

    std::string Name() const override { return "Jis"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToSBC(args1.TextValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_LEFT : public FunctionN {
public:
    explicit Function_LEFT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Left", 1, 2);
    }

    std::string Name() const override { return "Left"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() >= 2) {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const CodePoints cps = ToCps(args1.TextValue());
        if (cps.empty()) return Operand::Create(std::string());
        if (ArgCount() == 1) return Operand::Create(FromCps(cps, 0, 1));

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int count = 0;
        if (!TryGetInt(args2, count)) return ParameterError(2);
        if (count < 0) return ParameterError(2);
        if (static_cast<size_t>(count) >= cps.size()) return args1;
        return Operand::Create(FromCps(cps, 0, static_cast<size_t>(count)));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_LEN : public FunctionN {
public:
    explicit Function_LEN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Len", 1, 1);
    }

    std::string Name() const override { return "Len"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(static_cast<int>(CpLength(args1.TextValue())));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_LOWER : public FunctionN {
public:
    explicit Function_LOWER(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Lower", 1, 1);
    }

    std::string Name() const override { return "Lower"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const std::string text = args1.TextValue();
        for (char c : text) {
            if (IsAsciiUpper(c)) return Operand::Create(ToLowerAscii(text));
        }
        return args1;
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_MID : public FunctionN {
public:
    explicit Function_MID(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Mid", 3, 3);
    }

    std::string Name() const override { return "Mid"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        const CodePoints cps = ToCps(args1.TextValue());
        int startValue = 0;
        int length = 0;
        if (!TryGetInt(args2, startValue)) return ParameterError(2);
        if (!TryGetInt(args3, length)) return ParameterError(3);
        // 用 long long 计算, 避免 startValue 为 int.MinValue 时相减溢出
        const long long startIndex = static_cast<long long>(startValue) - engine.ExcelIndex;
        if (startIndex < 0) return ParameterError(2);
        if (length < 0) return ParameterError(3);
        const size_t len = cps.size();
        if (startIndex == 0 && static_cast<size_t>(length) >= len) return args1;
        if (static_cast<size_t>(startIndex) >= len) return Operand::Create(std::string());
        if (static_cast<size_t>(length) > len - static_cast<size_t>(startIndex)) {
            length = static_cast<int>(len - static_cast<size_t>(startIndex));
        }
        return Operand::Create(FromCps(cps, static_cast<size_t>(startIndex),
                                       static_cast<size_t>(startIndex) + static_cast<size_t>(length)));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_PROPER : public FunctionN {
public:
    explicit Function_PROPER(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Proper", 1, 1);
    }

    std::string Name() const override { return "Proper"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        CodePoints cps = ToCps(args1.TextValue());
        if (cps.empty()) return Operand::Create(std::string());

        bool needModify = false;
        bool isFirst = true;
        for (uint32_t c : cps) {
            if (!IsLetterCp(c)) {
                isFirst = true;
            } else {
                if (isFirst) {
                    if (c >= 'a' && c <= 'z') needModify = true;
                } else {
                    if (c >= 'A' && c <= 'Z') needModify = true;
                }
                if (needModify) break;
                isFirst = false;
            }
        }
        if (!needModify) return args1;

        isFirst = true;
        for (auto& c : cps) {
            if (!IsLetterCp(c)) {
                isFirst = true;
            } else {
                // 只对 ASCII 做大小写转换, 非 ASCII 保持原样
                if (isFirst) {
                    c = ToUpperInvariantCp(c);
                } else {
                    c = ToLowerInvariantCp(c);
                }
                isFirst = false;
            }
        }
        return Operand::Create(FromCps(cps, 0, cps.size()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_REPLACE : public FunctionN {
public:
    explicit Function_REPLACE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Replace", 3, 4);
    }

    std::string Name() const override { return "Replace"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() < 4) {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
            args_[2]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        } else {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
            args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
            args_[3]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const std::string oldText = args1.TextValue();

        if (ArgCount() == 3) {
            auto args2 = GetText(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            auto args3 = GetText(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            const std::string old = args2.TextValue();
            const std::string newStr = args3.TextValue();
            if (old.empty() || oldText.find(old) == std::string::npos) return args1;
            return Operand::Create(ReplaceAll(oldText, old, newStr));
        }

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        auto args4 = GetText(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;

        int startValue = 0;
        int length = 0;
        if (!TryGetInt(args2, startValue)) return ParameterError(2);
        if (!TryGetInt(args3, length)) return ParameterError(3);
        const std::string newText = args4.TextValue();

        const CodePoints cps = ToCps(oldText);
        const long long startIndex = static_cast<long long>(startValue) - engine.ExcelIndex;
        if (startIndex < 0) return ParameterError(2);
        if (length < 0) return ParameterError(3);
        const size_t len = cps.size();
        if (static_cast<size_t>(startIndex) >= len) return Operand::Create(oldText + newText);
        if (static_cast<size_t>(length) > len - static_cast<size_t>(startIndex)) {
            length = static_cast<int>(len - static_cast<size_t>(startIndex));
        }
        const size_t start = static_cast<size_t>(startIndex);
        const size_t endIndex = start + static_cast<size_t>(length);
        std::string sb;
        sb.reserve(oldText.size() + newText.size());
        sb += FromCps(cps, 0, start);
        sb += newText;
        if (endIndex < len) sb += FromCps(cps, endIndex, len);
        return Operand::Create(sb);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_REPT : public FunctionN {
public:
    explicit Function_REPT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Rept", 2, 2);
    }

    std::string Name() const override { return "Rept"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const std::string newText = args1.TextValue();
        int length = 0;
        if (!TryGetInt(args2, length)) return ParameterError(2);
        if (length < 0) return ParameterError(2);
        if (length == 0) return Operand::Create(std::string());
        const size_t cpLen = CpLength(newText);
        if (cpLen == 0) return args1;
        if (length > 32767 / static_cast<int>(cpLen)) return ParameterError(2);
        if (length == 1) return args1;

        std::string sb;
        sb.reserve(newText.size() * static_cast<size_t>(length));
        for (int i = 0; i < length; i++) sb += newText;
        return Operand::Create(sb);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_RIGHT : public FunctionN {
public:
    explicit Function_RIGHT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Right", 1, 2);
    }

    std::string Name() const override { return "Right"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() >= 2) {
            args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const CodePoints cps = ToCps(args1.TextValue());
        if (cps.empty()) return Operand::Create(std::string());
        if (ArgCount() == 1) return Operand::Create(FromCps(cps, cps.size() - 1, cps.size()));

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        int length = 0;
        if (!TryGetInt(args2, length)) return ParameterError(2);
        if (length < 0) return ParameterError(2);
        if (static_cast<size_t>(length) >= cps.size()) return args1;
        const size_t start = cps.size() - static_cast<size_t>(length);
        return Operand::Create(FromCps(cps, start, cps.size()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_RMB : public FunctionN {
public:
    explicit Function_RMB(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Rmb", 1, 1);
    }

    std::string Name() const override { return "Rmb"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(ToChineseRmb(args1.NumberValue()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_SEARCH : public FunctionN {
public:
    explicit Function_SEARCH(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Search", 2, 3);
    }

    std::string Name() const override { return "Search"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() >= 3) {
            args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const std::string pattern = args1.TextValue();
        const CodePoints text = ToCps(args2.TextValue());
        const int excelIndex = engine.ExcelIndex;

        if (ArgCount() == 2) {
            const int index = WildcardIndexOf(text, pattern, 0);
            if (index < 0) {
                // 未找到: Excel 模式(索引从1开始)返回错误, C# 模式(索引从0开始)返回 -1
                return excelIndex == 1 ? FunctionError() : Operand::Create(-1);
            }
            return Operand::Create(index + excelIndex);
        }

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        int startValue = 0;
        if (!TryGetInt(args3, startValue)) return ParameterError(3);
        const long long startIndex = static_cast<long long>(startValue) - excelIndex;
        if (startIndex < 0 || startIndex >= static_cast<long long>(text.size())) {
            return ParameterError(3);
        }
        const int p2 = WildcardIndexOf(text, pattern, static_cast<size_t>(startIndex));
        if (p2 < 0) {
            return excelIndex == 1 ? FunctionError() : Operand::Create(-1);
        }
        return Operand::Create(p2 + excelIndex);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_SUBSTITUTE : public FunctionN {
public:
    explicit Function_SUBSTITUTE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Substitute", 3, 4);
    }

    std::string Name() const override { return "Substitute"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[2]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() >= 4) {
            args_[3]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        auto args3 = GetText(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        const std::string text = args1.TextValue();
        const std::string oldText = args2.TextValue();
        const std::string newText = args3.TextValue();

        if (ArgCount() == 3) {
            if (oldText.empty()) return Operand::Create(text);
            return Operand::Create(ReplaceAll(text, oldText, newText));
        }

        auto args4 = GetNumber(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;
        int replaceIndex = 0;
        if (!TryGetInt(args4, replaceIndex)) return ParameterError(4);
        if (oldText.empty()) return Operand::Create(text);
        if (replaceIndex < 1) return ParameterError(4);

        const int n = static_cast<int>(text.size());
        const int m = static_cast<int>(oldText.size());
        std::string sb;
        int currentIndex = 0;
        int foundCount = 0;
        int searchPos = 0;
        while (searchPos <= n - m) {
            const size_t foundPos = text.find(oldText, static_cast<size_t>(searchPos));
            if (foundPos == std::string::npos) break;
            foundCount++;
            if (foundCount == replaceIndex) {
                sb.append(text, static_cast<size_t>(currentIndex),
                          foundPos - static_cast<size_t>(currentIndex));
                sb += newText;
                currentIndex = static_cast<int>(foundPos) + m;
                break;
            }
            searchPos = static_cast<int>(foundPos) + m;
        }
        if (currentIndex == 0) return Operand::Create(text);
        sb.append(text, static_cast<size_t>(currentIndex), std::string::npos);
        return Operand::Create(sb);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_T : public FunctionN {
public:
    explicit Function_T(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "T", 1, 1);
    }

    std::string Name() const override { return "T"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NONE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = EvalArg(1, engine, tp);
        if (args1.IsText()) return args1;
        return Operand::Create(std::string());
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_TEXT : public FunctionN {
public:
    explicit Function_TEXT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Text", 2, 2);
    }

    std::string Name() const override { return "Text"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NONE);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = EvalArg(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        if (args1.IsText()) return args1;
        if (args1.IsBoolean()) return Operand::Create(args1.BooleanValue() ? "TRUE" : "FALSE");
        if (args1.IsNumber()) {
            std::string s;
            if (!FormatNumber(args1.NumberValue(), args2.TextValue(), s)) return ParameterError(2);
            return Operand::Create(s);
        }
        if (args1.IsDate()) {
            return Operand::Create(args1.DateValue().ToString(args2.TextValue()));
        }
        auto converted = ConvertToText(args1, 1);
        if (converted.IsErrorOrNone()) return converted;
        return Operand::Create(converted.TextValue());
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_TRIM : public FunctionN {
public:
    explicit Function_TRIM(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Trim", 1, 1);
    }

    std::string Name() const override { return "Trim"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const std::string src = args1.TextValue();
        // Excel 的 TRIM 只处理半角空格(0x20), 不处理 Tab/换行/不换行空格等其他空白字符
        size_t b = 0;
        size_t e = src.size();
        while (b < e && src[b] == ' ') b++;
        while (e > b && src[e - 1] == ' ') e--;
        std::string text;
        text.reserve(e - b);
        bool lastSpace = false;
        for (size_t i = b; i < e; i++) {
            if (src[i] == ' ') {
                if (lastSpace) continue;
                lastSpace = true;
            } else {
                lastSpace = false;
            }
            text.push_back(src[i]);
        }
        if (text == src) return args1;
        return Operand::Create(text);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_UNICHAR : public FunctionN {
public:
    explicit Function_UNICHAR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "UniChar", 1, 1);
    }

    std::string Name() const override { return "UniChar"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        int code = 0;
        if (!TryGetInt(args1, code)) return ParameterError(1);
        if (code < 0 || code > 0x10FFFF || (code >= 0xD800 && code <= 0xDFFF)) {
            return ParameterError(1);
        }
        const CodePoints cps{static_cast<uint32_t>(code)};
        return Operand::Create(FromCps(cps, 0, 1));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_UNICODE : public FunctionN {
public:
    explicit Function_UNICODE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Unicode", 1, 1);
    }

    std::string Name() const override { return "Unicode"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const CodePoints cps = ToCps(args1.TextValue());
        if (cps.empty()) return ParameterError(1);
        const uint32_t first = cps[0];
        // 孤立代理项按参数错误处理
        if (first >= 0xD800 && first <= 0xDFFF) return ParameterError(1);
        return Operand::Create(static_cast<int>(first));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_UPPER : public FunctionN {
public:
    explicit Function_UPPER(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Upper", 1, 1);
    }

    std::string Name() const override { return "Upper"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        const std::string text = args1.TextValue();
        for (char c : text) {
            if (IsAsciiLower(c)) return Operand::Create(ToUpperAscii(text));
        }
        return args1;
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_VALUE : public FunctionN {
public:
    explicit Function_VALUE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Value", 1, 1);
    }

    std::string Name() const override { return "Value"; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op,
                           const std::string& val) override {
        (void)operandType;
        (void)op;
        (void)val;
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NONE);
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal d;
        if (TryParseAnyNumber(args1.TextValue(), d)) return Operand::Create(d);
        return ParameterError(1);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

}  // namespace

// ---------------------------------------------------------------------------
// 工厂
// ---------------------------------------------------------------------------

Function NewFunction_ASC(std::vector<Function> args) {
    return std::make_shared<Function_ASC>(std::move(args));
}

Function NewFunction_CHAR(std::vector<Function> args) {
    return std::make_shared<Function_CHAR>(std::move(args));
}

Function NewFunction_CLEAN(std::vector<Function> args) {
    return std::make_shared<Function_CLEAN>(std::move(args));
}

Function NewFunction_CODE(std::vector<Function> args) {
    return std::make_shared<Function_CODE>(std::move(args));
}

Function NewFunction_CONCATENATE(std::vector<Function> args) {
    return std::make_shared<Function_CONCATENATE>(std::move(args));
}

Function NewFunction_EXACT(std::vector<Function> args) {
    return std::make_shared<Function_EXACT>(std::move(args));
}

Function NewFunction_FIND(std::vector<Function> args) {
    return std::make_shared<Function_FIND>(std::move(args));
}

Function NewFunction_JIS(std::vector<Function> args) {
    return std::make_shared<Function_JIS>(std::move(args));
}

Function NewFunction_LEFT(std::vector<Function> args) {
    return std::make_shared<Function_LEFT>(std::move(args));
}

Function NewFunction_LEN(std::vector<Function> args) {
    return std::make_shared<Function_LEN>(std::move(args));
}

Function NewFunction_LOWER(std::vector<Function> args) {
    return std::make_shared<Function_LOWER>(std::move(args));
}

Function NewFunction_MID(std::vector<Function> args) {
    return std::make_shared<Function_MID>(std::move(args));
}

Function NewFunction_PROPER(std::vector<Function> args) {
    return std::make_shared<Function_PROPER>(std::move(args));
}

Function NewFunction_REPLACE(std::vector<Function> args) {
    return std::make_shared<Function_REPLACE>(std::move(args));
}

Function NewFunction_REPT(std::vector<Function> args) {
    return std::make_shared<Function_REPT>(std::move(args));
}

Function NewFunction_RIGHT(std::vector<Function> args) {
    return std::make_shared<Function_RIGHT>(std::move(args));
}

Function NewFunction_RMB(std::vector<Function> args) {
    return std::make_shared<Function_RMB>(std::move(args));
}

Function NewFunction_SEARCH(std::vector<Function> args) {
    return std::make_shared<Function_SEARCH>(std::move(args));
}

Function NewFunction_SUBSTITUTE(std::vector<Function> args) {
    return std::make_shared<Function_SUBSTITUTE>(std::move(args));
}

Function NewFunction_T(std::vector<Function> args) {
    return std::make_shared<Function_T>(std::move(args));
}

Function NewFunction_TEXT(std::vector<Function> args) {
    return std::make_shared<Function_TEXT>(std::move(args));
}

Function NewFunction_TRIM(std::vector<Function> args) {
    return std::make_shared<Function_TRIM>(std::move(args));
}

Function NewFunction_UNICHAR(std::vector<Function> args) {
    return std::make_shared<Function_UNICHAR>(std::move(args));
}

Function NewFunction_UNICODE(std::vector<Function> args) {
    return std::make_shared<Function_UNICODE>(std::move(args));
}

Function NewFunction_UPPER(std::vector<Function> args) {
    return std::make_shared<Function_UPPER>(std::move(args));
}

Function NewFunction_VALUE(std::vector<Function> args) {
    return std::make_shared<Function_VALUE>(std::move(args));
}

}  // namespace toolgood
