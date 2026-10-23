// MathTransformation.cpp - 对应 csharp/.../Internals/Functions/MathTransformation/
#include "MathTransformation.h"

#include <cstdint>
#include <cstdlib>
#include <regex>
#include <string>
#include <vector>

#include "../../../AlgorithmEngine.h"
#include "../../../Decimal.h"
#include "../../../Operands/Operand.h"
#include "../../FunctionUtil.h"

namespace toolgood {

namespace {

// ---------------- 对应 NumberBaseConverter.cs ----------------

/// places 参数允许的最大位数(与 10 位补码表示一致)
const int kMaxPlaces = 10;

/// 二进制 10 位补码的符号位权值(2^9)
const int kBinHalf = 512;
/// 二进制 10 位补码的模(2^10)
const int kBinModulus = 1024;

/// 八进制 10 位补码的符号位权值(2^29)
const int kOctHalf = 536870912;
/// 八进制 10 位补码的模(2^30)
const int kOctModulus = 1073741824;
/// 八进制 10 位补码的掩码(2^30-1)
const int kOctMask = 0x3FFFFFFF;

/// 十六进制 10 位补码的符号位权值(2^39)
const int64_t kHexHalf = 0x8000000000LL;
/// 十六进制 10 位补码的模(2^40)
const int64_t kHexModulus = 0x10000000000LL;
/// 十六进制 10 位补码的掩码(2^40-1)
const int64_t kHexMask = 0xFFFFFFFFFFLL;

std::string TrimSpace(const std::string& s) {
    size_t b = 0, e = s.size();
    auto isSpace = [](char c) {
        return c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\f' || c == '\v';
    };
    while (b < e && isSpace(s[b])) b++;
    while (e > b && isSpace(s[e - 1])) e--;
    return s.substr(b, e - b);
}

std::string ToUpperAscii(const std::string& s) {
    std::string r = s;
    for (char& c : r) {
        if (c >= 'a' && c <= 'z') c = static_cast<char>(c - 'a' + 'A');
    }
    return r;
}

std::string PadLeft(const std::string& text, int length, char pad) {
    if (static_cast<int>(text.size()) >= length) return text;
    return std::string(static_cast<size_t>(length) - text.size(), pad) + text;
}

/// 非负整数转大写十六进制文本（对应 ToString("X")）
std::string ToHexString(int64_t v) {
    static const char* kDigits = "0123456789ABCDEF";
    if (v == 0) return "0";
    uint64_t u = static_cast<uint64_t>(v);
    char buf[32];
    int i = 31;
    buf[i] = '\0';
    while (u != 0) {
        buf[--i] = kDigits[u & 0xF];
        u >>= 4;
    }
    return std::string(buf + i);
}

/// 非负整数转八进制文本（对应 Convert.ToString(x, 8)）
std::string ToOctString(int64_t v) {
    if (v == 0) return "0";
    uint64_t u = static_cast<uint64_t>(v);
    char buf[32];
    int i = 31;
    buf[i] = '\0';
    while (u != 0) {
        buf[--i] = static_cast<char>('0' + static_cast<int>(u & 7));
        u >>= 3;
    }
    return std::string(buf + i);
}

/// 非负整数转二进制文本（对应 Convert.ToString(x, 2)）
std::string ToBinString(int64_t v) {
    if (v == 0) return "0";
    uint64_t u = static_cast<uint64_t>(v);
    char buf[64];
    int i = 63;
    buf[i] = '\0';
    while (u != 0) {
        buf[--i] = static_cast<char>('0' + static_cast<int>(u & 1));
        u >>= 1;
    }
    return std::string(buf + i);
}

/// 读取并校验 places 参数:必须为 0~10 的整数。
bool TryGetPlaces(const Operand& arg, int& places) {
    const Decimal t = arg.NumberValue().Trunc();
    if (t.Less(Decimal::Zero) || t.Greater(Decimal::FromInt(kMaxPlaces))) return false;
    places = t.Int();
    return true;
}

bool IsValidText(const std::string& text, int fromBase) {
    if (fromBase == 2) return RegexIsBin(text);
    if (fromBase == 8) return RegexIsOct(text);
    return RegexIsHex(text);
}

int64_t GetHalf(int fromBase) {
    if (fromBase == 2) return kBinHalf;
    if (fromBase == 8) return kOctHalf;
    return kHexHalf;
}

/// 解析进制文本为有符号整数:先裁剪首尾空白，再校验字符与长度(不超过 10 位)，
/// 最后按 10 位补码规则还原符号。
bool ParseComplement(const std::string& value, int fromBase, int64_t& out) {
    const std::string text = TrimSpace(value);
    if (text.empty()) return false;
    if (static_cast<int>(text.size()) > kMaxPlaces) return false;
    if (!IsValidText(text, fromBase)) return false;
    const int64_t num = static_cast<int64_t>(std::strtoll(text.c_str(), nullptr, fromBase));
    const int64_t half = GetHalf(fromBase);
    out = num >= half ? num - half * 2 : num;
    return true;
}

/// 按 places 补零格式化进制文本。
/// 负数结果固定为 10 位补码，按 Excel 语义忽略 places 的位数限制。
bool TryFormat(const std::string& value, bool negative, bool hasPlaces, int places,
               std::string& result) {
    result = value;
    if (!hasPlaces) return true;
    if (!negative && static_cast<int>(value.size()) > places) return false;
    result = PadLeft(value, places, '0');
    return true;
}

/// 按 places 对齐十进制文本(负号占一位)。
bool TryFormatDecimal(int64_t value, int places, std::string& result) {
    const std::string text = std::to_string(value);
    if (static_cast<int>(text.size()) > places) return false;
    if (value < 0) {
        result = "-" + PadLeft(text.substr(1), places - 1, '0');
    } else {
        result = PadLeft(text, places, '0');
    }
    return true;
}

/// 对应 C# 的 TryGetInt：向零截断后判断是否落在 int 范围内。
bool TryGetInt(const Operand& op, int& value) {
    const Decimal t = op.NumberValue().Trunc();
    if (!t.FitsInt32()) return false;
    value = t.Int();
    return true;
}

// ---------------- ROMAN / ARABIC ----------------

const std::vector<int>& RomanValues(int form) {
    static const std::vector<std::vector<int>> kValues = {
        {1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1},
        {1000, 950, 900, 500, 450, 400, 100, 95, 90, 50, 45, 40, 10, 9, 5, 4, 1},
        {1000, 990, 950, 900, 500, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9, 5, 4, 1},
        {1000, 995, 990, 950, 900, 500, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45, 40, 10, 9,
         5, 4, 1},
        {1000, 999, 995, 990, 950, 900, 500, 499, 495, 490, 450, 400, 100, 99, 95, 90, 50, 49, 45,
         40, 10, 9, 5, 4, 1}};
    return kValues[static_cast<size_t>(form)];
}

const std::vector<std::string>& RomanSymbols(int form) {
    static const std::vector<std::vector<std::string>> kSymbols = {
        {"M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I"},
        {"M", "LM", "CM", "D", "LD", "CD", "C", "VC", "XC", "L", "VL", "XL", "X", "IX", "V", "IV",
         "I"},
        {"M", "XM", "LM", "CM", "D", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL", "VL", "XL",
         "X", "IX", "V", "IV", "I"},
        {"M", "VM", "XM", "LM", "CM", "D", "VD", "XD", "LD", "CD", "C", "IC", "VC", "XC", "L", "IL",
         "VL", "XL", "X", "IX", "V", "IV", "I"},
        {"M", "IM", "VM", "XM", "LM", "CM", "D", "ID", "VD", "XD", "LD", "CD", "C", "IC", "VC",
         "XC", "L", "IL", "VL", "XL", "X", "IX", "V", "IV", "I"}};
    return kSymbols[static_cast<size_t>(form)];
}

// form 0: 经典形式(标准减法表示)
// form 1: 增加 V-L=45, V-C=95, L-D=450, L-M=950
// form 2: 再增加 I-L=49, I-C=99, X-D=490, X-M=990
// form 3: 再增加 V-D=495, V-M=995
// form 4: 再增加 I-D=499, I-M=999(最简化)
std::string ArabicToRoman(int num, int form) {
    if (num == 0) return std::string();
    const std::vector<int>& values = RomanValues(form);
    const std::vector<std::string>& symbols = RomanSymbols(form);
    std::string sb;
    for (size_t i = 0; i < values.size(); i++) {
        while (num >= values[i]) {
            sb += symbols[i];
            num -= values[i];
        }
    }
    return sb;
}

int GetRomanValue(char c) {
    switch (c) {
        case 'I': return 1;
        case 'V': return 5;
        case 'X': return 10;
        case 'L': return 50;
        case 'C': return 100;
        case 'D': return 500;
        case 'M': return 1000;
        default: return -1;
    }
}

int RomanToArabic(const std::string& roman) {
    int result = 0;
    int prevValue = 0;
    for (int i = static_cast<int>(roman.size()) - 1; i >= 0; i--) {
        const int value = GetRomanValue(roman[static_cast<size_t>(i)]);
        if (value < 0) return -1;
        if (value < prevValue) {
            result -= value;
        } else {
            result += value;
        }
        prevValue = value;
    }
    return result;
}

// ------------------------------------------------------------------ ARABIC

class Function_ARABIC : public FunctionN {
public:
    explicit Function_ARABIC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "ARABIC", 1, 1);
    }

    std::string Name() const override { return "ARABIC"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto arg = GetText(1, engine, tp);
        if (arg.IsErrorOrNone()) return arg;
        // 裁剪首尾空白后统一转大写,与 Excel 忽略空白/大小写的语义一致
        const std::string text = ToUpperAscii(TrimSpace(arg.TextValue()));
        // Excel: 空文本或不符合罗马数字语法(非法字符/非法减法/非法重复)时返回 #VALUE!
        if (text.empty()) return ParameterError(1);
        static const std::string kRomanPattern =
            "^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$";
        const std::regex& kRomanRegex = GetRegex(kRomanPattern);
        if (!std::regex_match(text, kRomanRegex)) return ParameterError(1);
        const int result = RomanToArabic(text);
        if (result < 0) return ParameterError(1);
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ------------------------------------------------------------------ ROMAN

class Function_ROMAN : public FunctionN {
public:
    explicit Function_ROMAN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "ROMAN", 1, 2);
    }

    std::string Name() const override { return "ROMAN"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto numArg = GetNumber(1, engine, tp);
        if (numArg.IsErrorOrNone()) return numArg;
        // 数值超出 int 范围时按 #VALUE! 处理，避免 IntValue 抛出 OverflowException
        int num = 0;
        if (!TryGetInt(numArg, num)) return ParameterError(1);

        // Excel: number 为负或大于 3999 时返回 #VALUE!,number 为 0 返回空文本
        if (num < 0 || num > 3999) return ParameterError(1);

        int form = 0;
        if (ArgCount() >= 2) {
            auto formArg = EvalArg(2, engine, tp);
            if (formArg.IsErrorOrNone()) return formArg;
            if (formArg.IsBoolean()) {
                // Excel: form 为 TRUE 时使用古典形式(0),为 FALSE 时使用最简化形式(4)
                form = formArg.BooleanValue() ? 0 : 4;
            } else {
                auto formNum = ConvertToNumber(formArg, 2);
                if (formNum.IsErrorOrNone()) return formNum;
                if (!TryGetInt(formNum, form)) return ParameterError(2);
                // Excel: form 超出 0~4 范围返回 #VALUE!
                if (form < 0 || form > 4) return ParameterError(2);
            }
        }

        return Operand::Create(ArabicToRoman(num, form));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ BIN2DEC

class Function_BIN2DEC : public FunctionN {
public:
    explicit Function_BIN2DEC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Bin2Dec", 1, 2);
    }

    std::string Name() const override { return "Bin2Dec"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位二进制补码
        int64_t num = 0;
        if (!ParseComplement(args1.TextValue(), 2, num)) return NumError();
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            int places = 0;
            if (!TryGetPlaces(args2, places)) return NumError();
            std::string n;
            if (!TryFormatDecimal(num, places, n)) return NumError();
            return Operand::Create(n);
        }
        return Operand::Create(static_cast<int>(num));
    }

    OperandType GetResultType() const override {
        if (ArgCount() >= 2) return OperandType::TEXT;
        return OperandType::NUMBER;
    }
};

// ------------------------------------------------------------------ BIN2HEX

class Function_BIN2HEX : public FunctionN {
public:
    explicit Function_BIN2HEX(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Bin2Hex", 1, 2);
    }

    std::string Name() const override { return "Bin2Hex"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        int64_t bin = 0;
        if (!ParseComplement(args1.TextValue(), 2, bin)) return NumError();

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        std::string hex;
        if (bin < 0) {
            // 负数:10 位十六进制补码,按 Excel 语义忽略 places
            hex = PadLeft(ToHexString(bin & kHexMask), kMaxPlaces, '0');
        } else {
            hex = ToHexString(bin);
        }
        std::string result;
        if (!TryFormat(hex, bin < 0, hasPlaces, places, result)) return NumError();
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ BIN2OCT

class Function_BIN2OCT : public FunctionN {
public:
    explicit Function_BIN2OCT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Bin2Oct", 1, 2);
    }

    std::string Name() const override { return "Bin2Oct"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        int64_t parsed = 0;
        if (!ParseComplement(args1.TextValue(), 2, parsed)) return NumError();
        const int bin = static_cast<int>(parsed);

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于结果分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }

        if (bin < 0) {
            // 负数:10 位八进制补码,按 Excel 语义忽略 places
            return Operand::Create(
                PadLeft(ToOctString(bin & kOctMask), kMaxPlaces, '0'));
        }
        std::string oct;
        if (!TryFormat(ToOctString(bin), false, hasPlaces, places, oct)) return NumError();
        return Operand::Create(oct);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ DEC2BIN

class Function_DEC2BIN : public FunctionN {
public:
    explicit Function_DEC2BIN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Dec2Bin", 1, 2);
    }

    std::string Name() const override { return "Dec2Bin"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 数值超出 int 范围时按 #NUM! 处理，避免 IntValue 抛出 OverflowException
        int num = 0;
        if (!TryGetInt(args1, num)) return NumError();
        // Excel 范围:-512 ~ 511
        if (num < -kBinHalf || num > kBinHalf - 1) return NumError();

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于负数分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        if (num < 0) {
            // 负数:返回 10 位二进制补码,按 Excel 语义忽略 places
            return Operand::Create(
                PadLeft(ToBinString(num & (kBinModulus - 1)), kMaxPlaces, '0'));
        }
        std::string binaryStr;
        if (!TryFormat(ToBinString(num), false, hasPlaces, places, binaryStr)) return NumError();
        return Operand::Create(binaryStr);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ DEC2HEX

class Function_DEC2HEX : public FunctionN {
public:
    explicit Function_DEC2HEX(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Dec2Hex", 1, 2);
    }

    std::string Name() const override { return "Dec2Hex"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // Excel 会先截去小数部分再校验范围:-549755813888 ~ 549755813887
        const Decimal numValue = args1.NumberValue().Trunc();
        if (numValue.Less(Decimal::FromInt(-549755813888LL)) ||
            numValue.Greater(Decimal::FromInt(549755813887LL))) {
            return NumError();
        }
        const int64_t num = numValue.Int64();

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于负数分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        if (num < 0) {
            // 负数:返回 10 位十六进制补码,按 Excel 语义忽略 places
            return Operand::Create(PadLeft(ToHexString(num & kHexMask), kMaxPlaces, '0'));
        }
        std::string hex;
        if (!TryFormat(ToHexString(num), false, hasPlaces, places, hex)) return NumError();
        return Operand::Create(hex);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ DEC2OCT

class Function_DEC2OCT : public FunctionN {
public:
    explicit Function_DEC2OCT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Dec2Oct", 1, 2);
    }

    std::string Name() const override { return "Dec2Oct"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // Excel 会先截去小数部分再校验范围:-536870912 ~ 536870911
        const Decimal numValue = args1.NumberValue().Trunc();
        if (numValue.Less(Decimal::FromInt(-kOctHalf)) ||
            numValue.Greater(Decimal::FromInt(kOctHalf - 1))) {
            return NumError();
        }
        const int num = numValue.Int();

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于负数分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        if (num < 0) {
            // 负数:返回 10 位八进制补码,按 Excel 语义忽略 places
            return Operand::Create(PadLeft(ToOctString(num & kOctMask), kMaxPlaces, '0'));
        }
        std::string oct;
        if (!TryFormat(ToOctString(num), false, hasPlaces, places, oct)) return NumError();
        return Operand::Create(oct);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ HEX2BIN

class Function_HEX2BIN : public FunctionN {
public:
    explicit Function_HEX2BIN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Hex2Bin", 1, 2);
    }

    std::string Name() const override { return "Hex2Bin"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位十六进制补码
        int64_t num = 0;
        if (!ParseComplement(args1.TextValue(), 16, num)) return NumError();
        // Excel HEX2BIN 结果范围为 -512~511
        if (num < -kBinHalf || num > kBinHalf - 1) return NumError();

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于结果分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        if (num < 0) {
            // 负数:10 位二进制补码,按 Excel 语义忽略 places
            return Operand::Create(PadLeft(ToBinString(num & (kBinModulus - 1)), kMaxPlaces, '0'));
        }
        std::string bin;
        if (!TryFormat(ToBinString(num), false, hasPlaces, places, bin)) return NumError();
        return Operand::Create(bin);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ HEX2DEC

class Function_HEX2DEC : public FunctionN {
public:
    explicit Function_HEX2DEC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Hex2Dec", 1, 2);
    }

    std::string Name() const override { return "Hex2Dec"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位十六进制补码
        int64_t num = 0;
        if (!ParseComplement(args1.TextValue(), 16, num)) return NumError();
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            int places = 0;
            if (!TryGetPlaces(args2, places)) return NumError();
            std::string n;
            if (!TryFormatDecimal(num, places, n)) return NumError();
            return Operand::Create(n);
        }
        return Operand::Create(num);
    }

    OperandType GetResultType() const override {
        if (ArgCount() >= 2) return OperandType::TEXT;
        return OperandType::NUMBER;
    }
};

// ------------------------------------------------------------------ HEX2OCT

class Function_HEX2OCT : public FunctionN {
public:
    explicit Function_HEX2OCT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Hex2Oct", 1, 2);
    }

    std::string Name() const override { return "Hex2Oct"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位十六进制补码
        int64_t num = 0;
        if (!ParseComplement(args1.TextValue(), 16, num)) return NumError();
        // Excel HEX2OCT 结果范围为 -536870912~536870911
        if (num < -kOctHalf || num > kOctHalf - 1) return NumError();

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于结果分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        if (num < 0) {
            // 负数:10 位八进制补码,按 Excel 语义忽略 places
            return Operand::Create(PadLeft(ToOctString(num & kOctMask), kMaxPlaces, '0'));
        }
        std::string oct;
        if (!TryFormat(ToOctString(num), false, hasPlaces, places, oct)) return NumError();
        return Operand::Create(oct);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ OCT2BIN

class Function_OCT2BIN : public FunctionN {
public:
    explicit Function_OCT2BIN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Oct2Bin", 1, 2);
    }

    std::string Name() const override { return "Oct2Bin"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位八进制补码
        int64_t parsed = 0;
        if (!ParseComplement(args1.TextValue(), 8, parsed)) return NumError();
        const int num = static_cast<int>(parsed);
        // Excel OCT2BIN 结果范围为 -512~511
        if (num < -kBinHalf || num > kBinHalf - 1) return NumError();

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于结果分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        if (num < 0) {
            // 负数:10 位二进制补码,按 Excel 语义忽略 places
            return Operand::Create(
                PadLeft(ToBinString(num & (kBinModulus - 1)), kMaxPlaces, '0'));
        }
        std::string bin;
        if (!TryFormat(ToBinString(num), false, hasPlaces, places, bin)) return NumError();
        return Operand::Create(bin);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ------------------------------------------------------------------ OCT2DEC

class Function_OCT2DEC : public FunctionN {
public:
    explicit Function_OCT2DEC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Oct2Dec", 1, 2);
    }

    std::string Name() const override { return "Oct2Dec"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位八进制补码
        int64_t num = 0;
        if (!ParseComplement(args1.TextValue(), 8, num)) return NumError();
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            int places = 0;
            if (!TryGetPlaces(args2, places)) return NumError();
            std::string n;
            if (!TryFormatDecimal(num, places, n)) return NumError();
            return Operand::Create(n);
        }
        return Operand::Create(static_cast<int>(num));
    }

    OperandType GetResultType() const override {
        if (ArgCount() >= 2) return OperandType::TEXT;
        return OperandType::NUMBER;
    }
};

// ------------------------------------------------------------------ OCT2HEX

class Function_OCT2HEX : public FunctionN {
public:
    explicit Function_OCT2HEX(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Oct2Hex", 1, 2);
    }

    std::string Name() const override { return "Oct2Hex"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 文本裁剪空白、校验字符与长度(<=10),并解析 10 位八进制补码
        int64_t parsed = 0;
        if (!ParseComplement(args1.TextValue(), 8, parsed)) return NumError();
        const int num = static_cast<int>(parsed);

        int places = 0;
        bool hasPlaces = false;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            // places 校验先于结果分支:places 非法时即使 number 为负也应返回 #NUM!
            if (!TryGetPlaces(args2, places)) return NumError();
            hasPlaces = true;
        }
        if (num < 0) {
            // 负数:10 位十六进制补码,按 Excel 语义忽略 places
            return Operand::Create(PadLeft(ToHexString(num & kHexMask), kMaxPlaces, '0'));
        }
        std::string hex;
        if (!TryFormat(ToHexString(num), false, hasPlaces, places, hex)) return NumError();
        return Operand::Create(hex);
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

}  // namespace

Function NewFunction_ARABIC(std::vector<Function> args) {
    return std::make_shared<Function_ARABIC>(std::move(args));
}
Function NewFunction_BIN2DEC(std::vector<Function> args) {
    return std::make_shared<Function_BIN2DEC>(std::move(args));
}
Function NewFunction_BIN2HEX(std::vector<Function> args) {
    return std::make_shared<Function_BIN2HEX>(std::move(args));
}
Function NewFunction_BIN2OCT(std::vector<Function> args) {
    return std::make_shared<Function_BIN2OCT>(std::move(args));
}
Function NewFunction_DEC2BIN(std::vector<Function> args) {
    return std::make_shared<Function_DEC2BIN>(std::move(args));
}
Function NewFunction_DEC2HEX(std::vector<Function> args) {
    return std::make_shared<Function_DEC2HEX>(std::move(args));
}
Function NewFunction_DEC2OCT(std::vector<Function> args) {
    return std::make_shared<Function_DEC2OCT>(std::move(args));
}
Function NewFunction_HEX2BIN(std::vector<Function> args) {
    return std::make_shared<Function_HEX2BIN>(std::move(args));
}
Function NewFunction_HEX2DEC(std::vector<Function> args) {
    return std::make_shared<Function_HEX2DEC>(std::move(args));
}
Function NewFunction_HEX2OCT(std::vector<Function> args) {
    return std::make_shared<Function_HEX2OCT>(std::move(args));
}
Function NewFunction_OCT2BIN(std::vector<Function> args) {
    return std::make_shared<Function_OCT2BIN>(std::move(args));
}
Function NewFunction_OCT2DEC(std::vector<Function> args) {
    return std::make_shared<Function_OCT2DEC>(std::move(args));
}
Function NewFunction_OCT2HEX(std::vector<Function> args) {
    return std::make_shared<Function_OCT2HEX>(std::move(args));
}
Function NewFunction_ROMAN(std::vector<Function> args) {
    return std::make_shared<Function_ROMAN>(std::move(args));
}

}  // namespace toolgood
