// Csharp.cpp - 对应 csharp/ToolGood.Algorithm/Internals/Functions/Csharp/（19 个 C# 风格函数）
// 说明：字符级位置（IndexOf/LastIndexOf/Substring）统一按 UTF-8 码点计数，
//       大小写不敏感的序数比较仅折叠 ASCII a-z/A-Z。
#include "Csharp.h"

#include <algorithm>
#include <cstdint>
#include <random>
#include <regex>
#include <string>
#include <utility>
#include <vector>

#include "../../FunctionUtil.h"
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

/// 对应 char.IsWhiteSpace（TrimStart/TrimEnd 无参版本）
bool IsWhiteSpaceCp(uint32_t c) {
    if (c == ' ' || (c >= 0x09 && c <= 0x0D)) return true;
    if (c == 0x85 || c == 0xA0 || c == 0x1680) return true;
    if (c >= 0x2000 && c <= 0x200A) return true;
    if (c == 0x2028 || c == 0x2029 || c == 0x202F || c == 0x205F || c == 0x3000) return true;
    return false;
}

/// 序数比较时的 ASCII 折叠（对应 OrdinalIgnoreCase 对 ASCII 的处理）
char FoldAscii(char c) {
    if (c >= 'A' && c <= 'Z') return static_cast<char>(c - 'A' + 'a');
    return c;
}

bool StartsWith(const std::string& text, const std::string& value, bool ignoreCase) {
    if (value.size() > text.size()) return false;
    for (size_t i = 0; i < value.size(); i++) {
        const char a = ignoreCase ? FoldAscii(text[i]) : text[i];
        const char b = ignoreCase ? FoldAscii(value[i]) : value[i];
        if (a != b) return false;
    }
    return true;
}

bool EndsWith(const std::string& text, const std::string& value, bool ignoreCase) {
    if (value.size() > text.size()) return false;
    const size_t offset = text.size() - value.size();
    for (size_t i = 0; i < value.size(); i++) {
        const char a = ignoreCase ? FoldAscii(text[offset + i]) : text[offset + i];
        const char b = ignoreCase ? FoldAscii(value[i]) : value[i];
        if (a != b) return false;
    }
    return true;
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

/// 在 [start, start+count) 范围内查找（匹配必须整体落在范围内）
int IndexOfCountCp(const CodePoints& text, const CodePoints& pat, size_t start, size_t count) {
    if (pat.empty()) return start <= text.size() ? static_cast<int>(start) : -1;
    if (pat.size() > count) return -1;
    const size_t last = start + count - pat.size();
    for (size_t p = start; p <= last; p++) {
        if (p + pat.size() > text.size()) break;
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

/// 从 startIndex 起向前查找，匹配需整体落在 [startIndex-count+1, startIndex] 内
int LastIndexOfRangeCp(const CodePoints& text, const CodePoints& pat, int startIndex, int count) {
    if (pat.empty()) {
        // 空串在 .NET 中视为匹配在 startIndex+1 处
        const int r = startIndex + 1;
        return r <= static_cast<int>(text.size()) ? r : static_cast<int>(text.size());
    }
    if (count <= 0) return -1;
    int lower = startIndex - count + 1;
    if (lower < 0) lower = 0;
    const int m = static_cast<int>(pat.size());
    for (int p = startIndex; p >= lower; p--) {
        if (p + m > static_cast<int>(text.size())) continue;
        if (p + m - 1 > startIndex) continue;
        bool ok = true;
        for (int k = 0; k < m; k++) {
            if (text[static_cast<size_t>(p + k)] != pat[static_cast<size_t>(k)]) {
                ok = false;
                break;
            }
        }
        if (ok) return p;
    }
    return -1;
}

int LastIndexOfCp(const CodePoints& text, const CodePoints& pat) {
    if (pat.empty()) return static_cast<int>(text.size());
    if (pat.size() > text.size()) return -1;
    for (int p = static_cast<int>(text.size() - pat.size()); p >= 0; p--) {
        bool ok = true;
        for (size_t k = 0; k < pat.size(); k++) {
            if (text[static_cast<size_t>(p) + k] != pat[k]) {
                ok = false;
                break;
            }
        }
        if (ok) return p;
    }
    return -1;
}

/// 生成与 Guid.NewGuid().ToString() 相同格式的字符串（小写、带连字符）
std::string NewGuid() {
    std::random_device rd;
    std::mt19937_64 gen((static_cast<uint64_t>(rd()) << 32) ^ rd());
    unsigned char b[16];
    for (int i = 0; i < 16; i++) b[i] = static_cast<unsigned char>(gen() & 0xFF);
    b[6] = static_cast<unsigned char>((b[6] & 0x0F) | 0x40);  // version 4
    b[8] = static_cast<unsigned char>((b[8] & 0x3F) | 0x80);  // variant 10
    static const char* kHex = "0123456789abcdef";
    std::string s;
    s.reserve(36);
    for (int i = 0; i < 16; i++) {
        if (i == 4 || i == 6 || i == 8 || i == 10) s.push_back('-');
        s.push_back(kHex[b[i] >> 4]);
        s.push_back(kHex[b[i] & 0x0F]);
    }
    return s;
}

// ---------------------------------------------------------------------------
// 函数实现
// ---------------------------------------------------------------------------

class Function_ENDSWITH : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 2) args_[2]->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
    }
    explicit Function_ENDSWITH(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "EndsWith", 2, 3);
    }

    std::string Name() const override { return "EndsWith"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const std::string text = args1.TextValue();
        if (ArgCount() == 2) return Operand::Create(EndsWith(text, args2.TextValue(), false));

        auto args3 = GetBoolean(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        return Operand::Create(EndsWith(text, args2.TextValue(), args3.BooleanValue()));
    }

    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_GUID : public FunctionN {
public:
    explicit Function_GUID(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Guid", 0, 0);
    }

    std::string Name() const override { return "Guid"; }

    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override {
        return Operand::Create(NewGuid());
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_HAS : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::JSON);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HAS(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Has", 2, 2);
    }

    std::string Name() const override { return "Has"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = EvalArg(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const std::string text = args2.TextValue();
        if (auto* keyValueList = args1.AsKeyValueList()) {
            return Operand::Create(keyValueList->ContainsKey(text));
        }
        if (const KeyValue* keyValue = args1.AsKeyValue()) {
            return Operand::Create(keyValue->Key == text);
        }
        if (args1.IsJson()) {
            const JsonData* json = args1.JsonValue();
            if (json != nullptr) {
                if (json->IsArray()) {
                    for (int i = 0; i < json->Count(); i++) {
                        if (JsonValueEquals(&(*json)[i], text)) return Operand::True();
                    }
                } else if (json->ContainsKey(text)) {
                    return Operand::True();
                }
            }
            return Operand::False();
        }
        if (args1.IsArray()) {
            for (const auto& item : args1.ArrayValue()) {
                auto t = item.ToText();
                if (t.IsErrorOrNone()) continue;
                if (t.TextValue() == text) return Operand::True();
            }
            return Operand::False();
        }
        return ParameterError(1);
    }

    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_HASVALUE : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::JSON);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_HASVALUE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HasValue", 2, 2);
    }

    std::string Name() const override { return "HasValue"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = EvalArg(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const std::string text = args2.TextValue();
        if (auto* keyValueList = args1.AsKeyValueList()) {
            return Operand::Create(keyValueList->ContainsValue(text));
        }
        if (const KeyValue* keyValue = args1.AsKeyValue()) {
            return Operand::Create(OperandValueEquals(keyValue->Value, text));
        }
        if (args1.IsJson()) {
            const JsonData* json = args1.JsonValue();
            if (json != nullptr) {
                if (json->IsArray()) {
                    for (int i = 0; i < json->Count(); i++) {
                        if (JsonValueEquals(&(*json)[i], text)) return Operand::True();
                    }
                } else if (json->IsObject()) {
                    for (const std::string& key : json->Keys()) {
                        if (JsonValueEquals(json->GetChild(key), text)) return Operand::True();
                    }
                }
            }
            return Operand::False();
        }
        if (args1.IsArray()) {
            for (const auto& item : args1.ArrayValue()) {
                auto t = item.ToText();
                if (t.IsErrorOrNone()) continue;
                if (t.TextValue() == text) return Operand::True();
            }
            return Operand::False();
        }
        return ParameterError(1);
    }

    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_INDEXOF : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 2) args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        if (args_.size() > 3) args_[3]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }
    explicit Function_INDEXOF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IndexOf", 2, 4);
    }

    std::string Name() const override { return "IndexOf"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const CodePoints text = ToCps(args1.TextValue());
        const CodePoints pat = ToCps(args2.TextValue());
        const int excelIndex = engine.ExcelIndex;

        if (ArgCount() == 2) return Operand::Create(IndexOfCp(text, pat, 0) + excelIndex);

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        const long long startIndex = args3.NumberValue().Int64() - excelIndex;
        if (startIndex < 0 || startIndex > static_cast<long long>(text.size())) {
            return ParameterError(3);
        }

        if (ArgCount() == 3) {
            return Operand::Create(IndexOfCp(text, pat, static_cast<size_t>(startIndex)) + excelIndex);
        }

        auto args4 = GetNumber(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;
        const long long count = args4.NumberValue().Int64();
        if (count < 0 || startIndex + count > static_cast<long long>(text.size())) {
            return ParameterError(4);
        }
        return Operand::Create(IndexOfCountCp(text, pat, static_cast<size_t>(startIndex),
                                             static_cast<size_t>(count)) +
                               excelIndex);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_ISREGEX : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_ISREGEX(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IsRegex", 2, 2);
    }

    std::string Name() const override { return "IsRegex"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        try {
            const std::string pattern = args2.TextValue();
            const std::regex& re = GetRegex(pattern);
            return Operand::Create(std::regex_search(args1.TextValue(), re));
        } catch (const std::regex_error&) {
            return ParameterError(2);
        }
    }

    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_JOIN : public FunctionN {
public:
    explicit Function_JOIN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Join", 2, -1);
    }

    std::string Name() const override { return "Join"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        auto error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        Operand args1 = args[0];
        if (args1.IsJson()) {
            auto converted = args1.ToArray();
            if (!converted.IsErrorOrNone()) args1 = converted;
        }

        if (args1.IsArray()) {
            std::vector<std::string> list;
            if (!FlattenToStringList(args1, list)) return ParameterError(1);

            auto args2 = ConvertToText(args[1], 2);
            if (args2.IsErrorOrNone()) return args2;
            return Operand::Create(JoinStrings(args2.TextValue(), list));
        }

        auto text = ConvertToText(args1, 1);
        if (text.IsErrorOrNone()) return text;

        std::vector<std::string> list;
        for (size_t i = 1; i < args.size(); i++) {
            if (!FlattenToStringList(args[i], list)) return ParameterError(static_cast<int>(i) + 1);
        }
        return Operand::Create(JoinStrings(text.TextValue(), list));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }

private:
    static std::string JoinStrings(const std::string& separator, const std::vector<std::string>& list) {
        std::string sb;
        for (size_t i = 0; i < list.size(); i++) {
            if (i > 0) sb += separator;
            sb += list[i];
        }
        return sb;
    }
};

class Function_LASTINDEXOF : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 2) args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        if (args_.size() > 3) args_[3]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }
    explicit Function_LASTINDEXOF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "LastIndexOf", 2, 4);
    }

    std::string Name() const override { return "LastIndexOf"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const CodePoints text = ToCps(args1.TextValue());
        const CodePoints pat = ToCps(args2.TextValue());
        const int excelIndex = engine.ExcelIndex;

        if (ArgCount() == 2) return Operand::Create(LastIndexOfCp(text, pat) + excelIndex);

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        const long long startIndex = args3.NumberValue().Int64() - excelIndex;
        if (startIndex < 0 || startIndex > static_cast<long long>(text.size())) {
            return ParameterError(3);
        }

        if (ArgCount() == 3) {
            const int start = static_cast<int>(startIndex);
            return Operand::Create(LastIndexOfRangeCp(text, pat, start, start + 1) + excelIndex);
        }

        auto args4 = GetNumber(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;
        const long long count = args4.NumberValue().Int64();
        if (count < 0 || count > startIndex + 1) return ParameterError(4);
        return Operand::Create(LastIndexOfRangeCp(text, pat, static_cast<int>(startIndex),
                                                 static_cast<int>(count)) +
                               excelIndex);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_LOOKCEILING : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
    }
    explicit Function_LOOKCEILING(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "LookCeiling", 2, 2);
    }

    std::string Name() const override { return "LookCeiling"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetArray(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args2, list)) return ParameterError(2);
        if (list.empty()) return ParameterError(2);
        std::sort(list.begin(), list.end(), [](const Decimal& a, const Decimal& b) {
            return a.Cmp(b) < 0;
        });

        const Decimal value = args1.NumberValue();
        auto it = std::lower_bound(list.begin(), list.end(), value,
                                   [](const Decimal& a, const Decimal& b) { return a.Cmp(b) < 0; });
        const size_t index = static_cast<size_t>(it - list.begin());
        if (index < list.size() && list[index].Equal(value)) return args1;
        if (index == list.size()) {
            // 所有元素都小于 value，返回最大值
            return Operand::Create(list[list.size() - 1]);
        }
        return Operand::Create(list[index]);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_LOOKFLOOR : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
    }
    explicit Function_LOOKFLOOR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "LookFloor", 2, 2);
    }

    std::string Name() const override { return "LookFloor"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetArray(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args2, list)) return ParameterError(2);
        if (list.empty()) return ParameterError(2);
        std::sort(list.begin(), list.end(), [](const Decimal& a, const Decimal& b) {
            return a.Cmp(b) < 0;
        });

        const Decimal value = args1.NumberValue();
        auto it = std::lower_bound(list.begin(), list.end(), value,
                                   [](const Decimal& a, const Decimal& b) { return a.Cmp(b) < 0; });
        const size_t index = static_cast<size_t>(it - list.begin());
        if (index < list.size() && list[index].Equal(value)) return args1;
        if (index == 0) {
            // 所有元素都大于 value，返回最小值
            return Operand::Create(list[0]);
        }
        return Operand::Create(list[index - 1]);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_REGEX : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_REGEX(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Regex", 2, 2);
    }

    std::string Name() const override { return "Regex"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        try {
            const std::string pattern = args2.TextValue();
            const std::regex& re = GetRegex(pattern);
            const std::string text = args1.TextValue();
            std::smatch m;
            if (!std::regex_search(text, m, re)) return FunctionError();
            return Operand::Create(m.str());
        } catch (const std::regex_error&) {
            return ParameterError(2);
        }
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_REGEXREPLACE : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[2]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_REGEXREPLACE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RegexReplace", 3, 3);
    }

    std::string Name() const override { return "RegexReplace"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        auto args3 = GetText(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        try {
            const std::string pattern = args2.TextValue();
            const std::regex& re = GetRegex(pattern);
            return Operand::Create(std::regex_replace(args1.TextValue(), re, args3.TextValue()));
        } catch (const std::regex_error&) {
            return ParameterError(2);
        }
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_REMOVEEND : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 2) args_[2]->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
    }
    explicit Function_REMOVEEND(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RemoveEnd", 2, 3);
    }

    std::string Name() const override { return "RemoveEnd"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        bool ignoreCase = false;
        if (ArgCount() == 3) {
            auto args3 = GetBoolean(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            ignoreCase = args3.BooleanValue();
        }

        const std::string text = args1.TextValue();
        const std::string suffix = args2.TextValue();
        if (EndsWith(text, suffix, ignoreCase)) {
            return Operand::Create(text.substr(0, text.size() - suffix.size()));
        }
        return args1;
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_REMOVESTART : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 2) args_[2]->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
    }
    explicit Function_REMOVESTART(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RemoveStart", 2, 3);
    }

    std::string Name() const override { return "RemoveStart"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        bool ignoreCase = false;
        if (ArgCount() == 3) {
            auto args3 = GetBoolean(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            ignoreCase = args3.BooleanValue();
        }

        const std::string text = args1.TextValue();
        const std::string prefix = args2.TextValue();
        if (StartsWith(text, prefix, ignoreCase)) {
            return Operand::Create(text.substr(prefix.size()));
        }
        return args1;
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_SPLIT : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_SPLIT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Split", 2, 2);
    }

    std::string Name() const override { return "Split"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const std::string text = args1.TextValue();
        const std::string sep = args2.TextValue();
        if (sep.empty()) return ParameterError(2);

        std::vector<Operand> parts;
        size_t pos = 0;
        while (true) {
            const size_t p = text.find(sep, pos);
            if (p == std::string::npos) {
                parts.push_back(Operand::Create(text.substr(pos)));
                break;
            }
            parts.push_back(Operand::Create(text.substr(pos, p - pos)));
            pos = p + sep.size();
        }
        return Operand::Create(std::move(parts));
    }

    OperandType GetResultType() const override { return OperandType::ARRAY; }
};

class Function_STARTSWITH : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 2) args_[2]->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
    }
    explicit Function_STARTSWITH(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "StartsWith", 2, 3);
    }

    std::string Name() const override { return "StartsWith"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const std::string text = args1.TextValue();
        if (ArgCount() == 2) return Operand::Create(StartsWith(text, args2.TextValue(), false));

        auto args3 = GetBoolean(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        return Operand::Create(StartsWith(text, args2.TextValue(), args3.BooleanValue()));
    }

    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_SUBSTRING : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        args_[1]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        if (args_.size() > 2) args_[2]->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }
    explicit Function_SUBSTRING(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Substring", 2, 3);
    }

    std::string Name() const override { return "Substring"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        const CodePoints cps = ToCps(args1.TextValue());
        const long long startIndex = args2.NumberValue().Int64() - engine.ExcelIndex;
        if (startIndex < 0) return ParameterError(2);
        if (startIndex >= static_cast<long long>(cps.size())) {
            return Operand::Create(std::string());
        }

        if (ArgCount() == 2) {
            return Operand::Create(FromCps(cps, static_cast<size_t>(startIndex), cps.size()));
        }

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        long long length = args3.NumberValue().Int64();
        if (length < 0) return ParameterError(3);
        if (startIndex + length > static_cast<long long>(cps.size())) {
            length = static_cast<long long>(cps.size()) - startIndex;
        }
        return Operand::Create(FromCps(cps, static_cast<size_t>(startIndex),
                                       static_cast<size_t>(startIndex + length)));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_TRIMEND : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 1) args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_TRIMEND(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "TrimEnd", 1, 2);
    }

    std::string Name() const override { return "TrimEnd"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        CodePoints cps = ToCps(args1.TextValue());
        if (ArgCount() == 1) {
            size_t e = cps.size();
            while (e > 0 && IsWhiteSpaceCp(cps[e - 1])) e--;
            return Operand::Create(FromCps(cps, 0, e));
        }

        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        const CodePoints trimChars = ToCps(args2.TextValue());
        size_t e = cps.size();
        while (e > 0 &&
               std::find(trimChars.begin(), trimChars.end(), cps[e - 1]) != trimChars.end()) {
            e--;
        }
        return Operand::Create(FromCps(cps, 0, e));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_TRIMSTART : public FunctionN {
public:
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() > 1) args_[1]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
    }
    explicit Function_TRIMSTART(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "TrimStart", 1, 2);
    }

    std::string Name() const override { return "TrimStart"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetText(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        const CodePoints cps = ToCps(args1.TextValue());
        if (ArgCount() == 1) {
            size_t b = 0;
            while (b < cps.size() && IsWhiteSpaceCp(cps[b])) b++;
            return Operand::Create(FromCps(cps, b, cps.size()));
        }

        auto args2 = GetText(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        const CodePoints trimChars = ToCps(args2.TextValue());
        size_t b = 0;
        while (b < cps.size() &&
               std::find(trimChars.begin(), trimChars.end(), cps[b]) != trimChars.end()) {
            b++;
        }
        return Operand::Create(FromCps(cps, b, cps.size()));
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

}  // namespace

// ---------------------------------------------------------------------------
// 工厂
// ---------------------------------------------------------------------------

Function NewFunction_ENDSWITH(std::vector<Function> args) {
    return std::make_shared<Function_ENDSWITH>(std::move(args));
}

Function NewFunction_GUID(std::vector<Function> args) {
    return std::make_shared<Function_GUID>(std::move(args));
}

Function NewFunction_HAS(std::vector<Function> args) {
    return std::make_shared<Function_HAS>(std::move(args));
}

Function NewFunction_HASVALUE(std::vector<Function> args) {
    return std::make_shared<Function_HASVALUE>(std::move(args));
}

Function NewFunction_INDEXOF(std::vector<Function> args) {
    return std::make_shared<Function_INDEXOF>(std::move(args));
}

Function NewFunction_ISREGEX(std::vector<Function> args) {
    return std::make_shared<Function_ISREGEX>(std::move(args));
}

Function NewFunction_JOIN(std::vector<Function> args) {
    return std::make_shared<Function_JOIN>(std::move(args));
}

Function NewFunction_LASTINDEXOF(std::vector<Function> args) {
    return std::make_shared<Function_LASTINDEXOF>(std::move(args));
}

Function NewFunction_LOOKCEILING(std::vector<Function> args) {
    return std::make_shared<Function_LOOKCEILING>(std::move(args));
}

Function NewFunction_LOOKFLOOR(std::vector<Function> args) {
    return std::make_shared<Function_LOOKFLOOR>(std::move(args));
}

Function NewFunction_REGEX(std::vector<Function> args) {
    return std::make_shared<Function_REGEX>(std::move(args));
}

Function NewFunction_REGEXREPLACE(std::vector<Function> args) {
    return std::make_shared<Function_REGEXREPLACE>(std::move(args));
}

Function NewFunction_REMOVEEND(std::vector<Function> args) {
    return std::make_shared<Function_REMOVEEND>(std::move(args));
}

Function NewFunction_REMOVESTART(std::vector<Function> args) {
    return std::make_shared<Function_REMOVESTART>(std::move(args));
}

Function NewFunction_SPLIT(std::vector<Function> args) {
    return std::make_shared<Function_SPLIT>(std::move(args));
}

Function NewFunction_STARTSWITH(std::vector<Function> args) {
    return std::make_shared<Function_STARTSWITH>(std::move(args));
}

Function NewFunction_SUBSTRING(std::vector<Function> args) {
    return std::make_shared<Function_SUBSTRING>(std::move(args));
}

Function NewFunction_TRIMEND(std::vector<Function> args) {
    return std::make_shared<Function_TRIMEND>(std::move(args));
}

Function NewFunction_TRIMSTART(std::vector<Function> args) {
    return std::make_shared<Function_TRIMSTART>(std::move(args));
}

}  // namespace toolgood
