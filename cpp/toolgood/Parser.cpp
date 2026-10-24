#include "Parser.h"

#include <algorithm>
#include <stdexcept>
#include <unordered_map>

#include "Internals/Functions/Compare/Compare.h"
#include "Internals/Functions/Csharp/Csharp.h"
#include "Internals/Functions/DateTimes/DateTimes.h"
#include "Internals/Functions/Flow/Flow.h"
#include "Internals/Functions/MathBase/MathBase.h"
#include "Internals/Functions/Operator/Operator.h"
#include "Internals/Functions/Value/Value.h"
#include "Internals/Registry.h"

namespace toolgood {

namespace {

int PrecedenceOf(const std::string& kind) {
    if (kind == "OPOR") return 2;
    if (kind == "OPAND") return 3;
    if (kind == "OPNE" || kind == "OPEQ") return 4;
    if (kind == "OPGT" || kind == "OPGE" || kind == "OPLT" || kind == "OPLE") return 5;
    if (kind == "OPCAT" || kind == "OPADD" || kind == "OPSUB") return 7;
    if (kind == "OPMUL" || kind == "OPDIV" || kind == "OPMOD") return 8;
    return 0;
}

/// 数值单位后缀字典（与 Function_Number2 保持一致）
const std::unordered_map<std::string, std::string>& UnitDict() {
    static const std::unordered_map<std::string, std::string> dict = {
        {"MM", "MM"},   {"CM", "CM"},   {"DM", "DM"},   {"M", "M"},     {"KM", "KM"},
        {"MM2", "MM2"}, {"CM2", "CM2"}, {"DM2", "DM2"}, {"M2", "M2"},   {"KM2", "KM2"},
        {"MM3", "MM3"}, {"CM3", "CM3"}, {"DM3", "DM3"}, {"M3", "M3"},   {"KM3", "KM3"},
        {"G", "G"},     {"KG", "KG"},   {"T", "T"},
        // 与 Function_Number2 的单位表保持一致（升 / 毫升）
        {"L", "L"},     {"ML", "ML"},
    };
    return dict;
}

bool TrySplitUnit(const std::string& text, std::string& body, std::string& unit) {
    // C# 的 unitTypedict 用 StringComparer.OrdinalIgnoreCase，这里同样按大写比较
    for (int n = 3; n >= 1; n--) {
        if (static_cast<int>(text.size()) <= n) continue;
        std::string cand = text.substr(text.size() - static_cast<size_t>(n));
        for (auto& c : cand) {
            if (c >= 'a' && c <= 'z') c = static_cast<char>(c - 32);
        }
        if (UnitDict().find(cand) != UnitDict().end()) {
            body = text.substr(0, text.size() - static_cast<size_t>(n));
            unit = cand;
            return true;
        }
    }
    body = text;
    unit.clear();
    return false;
}

bool ParseDecimalStrict(const std::string& text, Decimal& out) {
    if (!Decimal::TryParse(text, out)) return false;
    if (out.Abs().Greater(Decimal::Max)) return false;
    return true;
}

void AppendUtf8(std::string& out, uint32_t cp) {
    if (cp < 0x80) {
        out += static_cast<char>(cp);
    } else if (cp < 0x800) {
        out += static_cast<char>(0xC0 | (cp >> 6));
        out += static_cast<char>(0x80 | (cp & 0x3F));
    } else if (cp < 0x10000) {
        out += static_cast<char>(0xE0 | (cp >> 12));
        out += static_cast<char>(0x80 | ((cp >> 6) & 0x3F));
        out += static_cast<char>(0x80 | (cp & 0x3F));
    } else {
        out += static_cast<char>(0xF0 | (cp >> 18));
        out += static_cast<char>(0x80 | ((cp >> 12) & 0x3F));
        out += static_cast<char>(0x80 | ((cp >> 6) & 0x3F));
        out += static_cast<char>(0x80 | (cp & 0x3F));
    }
}

/// 去掉字符串字面量首尾引号并解转义（对应 VisitSTRING_fun / UnescapeString）
std::string UnescapeString(const std::string& text) {
    std::vector<uint32_t> cps = Lexer::ToCodePoints(text);
    if (cps.size() < 2) return text;
    std::string out;
    size_t index = 1;
    const size_t end = cps.size() - 1;
    auto hexVal = [](uint32_t c) -> int {
        if (c >= '0' && c <= '9') return static_cast<int>(c - '0');
        if (c >= 'a' && c <= 'f') return static_cast<int>(c - 'a' + 10);
        if (c >= 'A' && c <= 'F') return static_cast<int>(c - 'A' + 10);
        return -1;
    };
    while (index < end) {
        uint32_t c = cps[index++];
        if (c != '\\') {
            AppendUtf8(out, c);
            continue;
        }
        if (index >= end) break;
        uint32_t c2 = cps[index++];
        switch (c2) {
            case 'n': out += '\n'; break;
            case 'r': out += '\r'; break;
            case 't': out += '\t'; break;
            case '0': out += '\0'; break;
            case 'v': out += '\v'; break;
            case 'a': out += '\a'; break;
            case 'b': out += '\b'; break;
            case 'f': out += '\f'; break;
            case 'u': {
                if (index + 4 <= end) {
                    int code = 0;
                    bool ok = true;
                    for (int k = 0; k < 4; k++) {
                        int hv = hexVal(cps[index + static_cast<size_t>(k)]);
                        if (hv < 0) {
                            ok = false;
                            break;
                        }
                        code = code * 16 + hv;
                    }
                    if (ok) {
                        AppendUtf8(out, static_cast<uint32_t>(code));
                        index += 4;
                        break;
                    }
                }
                AppendUtf8(out, c2);
                break;
            }
            default: AppendUtf8(out, c2); break;
        }
    }
    return out;
}

/// 常量名（对应 CONST_fun / CONST2_fun）
enum class ConstKind { None, True, False, Null, Version, E, PI, Rand, Guid, Now, Today };

ConstKind ClassifyConst(const std::string& upper) {
    if (upper == "TRUE" || upper == "YES") return ConstKind::True;
    if (upper == "FALSE" || upper == "NO") return ConstKind::False;
    if (upper == "NULL") return ConstKind::Null;
    if (upper == "ALGORITHMVERSION" || upper == "ENGINEVERSION") return ConstKind::Version;
    if (upper == "E") return ConstKind::E;
    if (upper == "PI") return ConstKind::PI;
    if (upper == "RAND") return ConstKind::Rand;
    if (upper == "GUID") return ConstKind::Guid;
    if (upper == "NOW") return ConstKind::Now;
    if (upper == "TODAY") return ConstKind::Today;
    return ConstKind::None;
}

std::string ToUpperAscii(const std::string& s) {
    std::string r = s;
    for (auto& c : r) {
        if (c >= 'a' && c <= 'z') c = static_cast<char>(c - 32);
    }
    return r;
}

}  // namespace

Parser::Parser(const std::string& text) : text_(text) {
    Lexer lexer(text);
    tokens_ = lexer.Tokenize();
}

const Token& Parser::Peek(size_t offset) const {
    size_t idx = pos_ + offset;
    if (idx >= tokens_.size()) idx = tokens_.size() - 1;
    return tokens_[idx];
}

Token Parser::Advance() {
    Token t = tokens_[pos_];
    if (t.kind != "EOF") pos_++;
    return t;
}

Token Parser::Expect(const std::string& kind) {
    const Token& t = Current();
    if (t.kind != kind) {
        throw std::runtime_error("mismatched input '" + t.text + "' expecting " + kind +
                                 " at position " + std::to_string(t.start));
    }
    return Advance();
}

bool Parser::StartsOperand(const Token& t) const {
    const std::string& k = t.kind;
    if (k == "NUM" || k == "STRING" || k == "IDENT" || k == "LPAREN" || k == "LBRACKET" ||
        k == "LBRACE" || k == "NOT" || k == "OPADD" || k == "OPSUB") {
        return true;
    }
    return false;
}

Function Parser::MakeNumber(const Token& numToken, bool negated) {
    std::string text = numToken.text;
    if (negated) text = "-" + text;
    Decimal d;
    if (ParseDecimalStrict(text, d)) {
        return NewFunction_Number(d);
    }
    std::string body, unit;
    if (TrySplitUnit(text, body, unit)) {
        Decimal d2;
        if (!ParseDecimalStrict(body, d2)) {
            throw std::runtime_error("Number '" + text + "' is invalid or out of range.");
        }
        return NewFunction_Number2(d2, unit);
    }
    throw std::runtime_error("Number '" + text + "' is invalid or out of range.");
}

Function Parser::MakeCall(const std::string& name, const std::string& originalName,
                          std::vector<Function> args) {
    if (HasFunction(name)) {
        return GetFunction(name, args);
    }
    return NewFunction_DiyFunction(originalName, std::move(args));
}

Function Parser::Parse() {
    RegisterAllFunctions();
    Function node = ParseExpr(0);
    if (Current().kind != "EOF") {
        const Token& t = Current();
        throw std::runtime_error("extraneous input '" + t.text + "' at position " +
                                 std::to_string(t.start));
    }
    return node;
}

Function Parser::ParseExpr(int minPrecedence) {
    Function left = ParsePrefix();
    for (;;) {
        const Token& token = Current();
        const std::string kind = token.kind;
        if (kind == "QUESTION" && minPrecedence <= 1) {
            Advance();
            Function whenTrue = ParseExpr(0);
            Expect("COLON");
            Function whenFalse = ParseExpr(1);
            left = NewFunction_IF({left, whenTrue, whenFalse});
            continue;
        }
        if (kind == "OPMOD" && minPrecedence <= 9 && !StartsOperand(Peek(1))) {
            Advance();
            left = NewFunction_Percentage({left});
            continue;
        }
        const int prec = PrecedenceOf(kind);
        if (prec == 0 || prec < minPrecedence) break;
        Advance();
        Function right = ParseExpr(prec + 1);
        if (kind == "OPMUL") left = NewFunction_Mul({left, right});
        else if (kind == "OPDIV") left = NewFunction_Div({left, right});
        else if (kind == "OPMOD") left = NewFunction_Mod({left, right});
        else if (kind == "OPADD") left = NewFunction_Add({left, right});
        else if (kind == "OPSUB") left = NewFunction_Sub({left, right});
        else if (kind == "OPCAT") left = NewFunction_Connect({left, right});
        else if (kind == "OPGT") left = NewFunction_GT({left, right});
        else if (kind == "OPGE") left = NewFunction_GE({left, right});
        else if (kind == "OPLT") left = NewFunction_LT({left, right});
        else if (kind == "OPLE") left = NewFunction_LE({left, right});
        else if (kind == "OPEQ") left = NewFunction_EQ({left, right});
        else if (kind == "OPNE") left = NewFunction_NE({left, right});
        else if (kind == "OPAND") left = NewFunction_AND({left, right});
        else if (kind == "OPOR") left = NewFunction_OR({left, right});
    }
    return left;
}

Function Parser::ParsePrefix() {
    const Token token = Current();
    if (token.kind == "NOT") {
        Advance();
        Function operand = ParseExpr(9);
        return NewFunction_NOT({operand});
    }
    if (token.kind == "OPSUB" && Peek(1).kind == "NUM") {
        Advance();
        Token number = Advance();
        return MakeNumber(number, true);
    }
    Function node = ParsePrimary();
    return ParsePostfix(node);
}

Function Parser::ParsePrimary() {
    const Token token = Current();
    if (token.kind == "NUM") {
        Advance();
        return MakeNumber(token, false);
    }
    if (token.kind == "STRING") {
        Advance();
        return NewFunction_ValueText(UnescapeString(token.text));
    }
    if (token.kind == "LPAREN") {
        Advance();
        Function inner = ParseExpr(0);
        Expect("RPAREN");
        return inner;
    }
    if (token.kind == "LBRACKET") {
        Advance();
        std::vector<Function> items;
        if (Current().kind != "RBRACKET") {
            items.push_back(ParseExpr(0));
            while (Current().kind == "COMMA") {
                Advance();
                if (Current().kind == "RBRACKET") break;
                items.push_back(ParseExpr(0));
            }
        }
        Expect("RBRACKET");
        return NewFunction_ARRAY(std::move(items));
    }
    if (token.kind == "LBRACE") {
        Advance();
        std::vector<Function> items;
        while (Current().kind != "RBRACE") {
            auto kv = ParseArrayJsonItem();
            items.push_back(kv.second);
            if (Current().kind == "COMMA") {
                Advance();
                continue;
            }
            break;
        }
        Expect("RBRACE");
        return NewFunction_ArrayJson(std::move(items));
    }
    if (token.kind == "IDENT") {
        Advance();
        const std::string upper = ToUpperAscii(token.text);
        // 常量名优先于函数调用：C# 文法里 TRUE/FALSE/NULL/ALGORITHMVERSION 的 '()' 可选，
        // RAND/NOW/TODAY/GUID/E/PI 必须带 '()'。两者都要按常量处理，
        // 否则 TRUE() / Guid() 会掉进 DiyFunction 分支。
        ConstKind ck0 = ClassifyConst(upper);
        if (ck0 != ConstKind::None) {
            if (Current().kind == "LPAREN" && Peek(1).kind == "RPAREN") {
                Advance();
                Advance();
            }
            switch (ck0) {
                case ConstKind::True: return NewFunction_ValueBoolean(true);
                case ConstKind::False: return NewFunction_ValueBoolean(false);
                case ConstKind::Null: return NewFunction_NULL();
                case ConstKind::Version: return NewFunction_Version();
                case ConstKind::E: return NewFunction_Number_E();
                case ConstKind::PI: return NewFunction_Number_PI();
                case ConstKind::Rand: return NewFunction_RAND({});
                case ConstKind::Guid: return NewFunction_GUID({});
                case ConstKind::Now: return NewFunction_NOW({});
                case ConstKind::Today: return NewFunction_TODAY({});
                default: break;
            }
        }
        if (Current().kind == "LPAREN") {
            std::vector<Function> args = ParseArguments();
            return MakeCall(upper, token.text, std::move(args));
        }
        ConstKind ck = ClassifyConst(upper);
        switch (ck) {
            case ConstKind::True: return NewFunction_ValueBoolean(true);
            case ConstKind::False: return NewFunction_ValueBoolean(false);
            case ConstKind::Null: return NewFunction_NULL();
            case ConstKind::Version: return NewFunction_Version();
            case ConstKind::E: return NewFunction_Number_E();
            case ConstKind::PI: return NewFunction_Number_PI();
            case ConstKind::Rand: return NewFunction_RAND({});
            case ConstKind::Guid: return NewFunction_GUID({});
            case ConstKind::Now: return NewFunction_NOW({});
            case ConstKind::Today: return NewFunction_TODAY({});
            case ConstKind::None: break;
        }
        return NewFunction_Parameter(token.text);
    }
    std::string display = token.text.empty() ? "<EOF>" : token.text;
    throw std::runtime_error("mismatched input '" + display + "' at position " +
                             std::to_string(token.start));
}

Function Parser::ParsePostfix(Function node) {
    for (;;) {
        const Token token = Current();
        if (token.kind == "DOT") {
            Advance();
            const Token name = Current();
            if (name.kind != "IDENT") {
                throw std::runtime_error("unknown property '" + name.text + "' at position " +
                                         std::to_string(name.start));
            }
            Advance();
            if (Current().kind == "LPAREN") {
                std::vector<Function> args = ParseArguments();
                std::vector<Function> all;
                all.push_back(node);
                for (auto& a : args) all.push_back(a);
                const std::string upper = ToUpperAscii(name.text);
                node = MakeCall(upper, name.text, std::move(all));
            } else {
                node = NewFunction_GetJsonValue(
                    {node, NewFunction_ValueText(name.text)});
            }
            continue;
        }
        if (token.kind == "LBRACKET") {
            Advance();
            bool keyIsParameter = Current().kind == "IDENT" && Peek(1).kind == "RBRACKET";
            Function key;
            if (keyIsParameter) {
                Token id = Advance();
                key = NewFunction_Parameter(id.text);
            } else {
                key = ParseExpr(0);
            }
            Expect("RBRACKET");
            node = NewFunction_GetJsonValue({node, key});
            continue;
        }
        break;
    }
    return node;
}

std::vector<Function> Parser::ParseArguments() {
    Expect("LPAREN");
    std::vector<Function> args;
    if (Current().kind != "RPAREN") {
        args.push_back(ParseExpr(0));
        while (Current().kind == "COMMA") {
            Advance();
            args.push_back(ParseExpr(0));
        }
    }
    Expect("RPAREN");
    return args;
}

std::pair<std::string, Function> Parser::ParseArrayJsonItem() {
    const Token token = Current();
    if (token.kind == "NUM" || token.kind == "STRING" ||
        (token.kind == "IDENT" && Peek(1).kind == "COLON")) {
        Advance();
        std::string key;
        if (token.kind == "STRING") {
            key = UnescapeString(token.text);
        } else {
            key = token.text;
        }
        Expect("COLON");
        Function value = ParseExpr(0);
        return {key, NewFunction_ArrayJsonItem(key, value)};
    }
    Function value = ParseExpr(0);
    return {std::string(), NewFunction_ArrayJsonItem(std::string(), value)};
}

Function Parser::ParseFunction(const std::string& text) {
    Parser p(text);
    return p.Parse();
}

}  // namespace toolgood
