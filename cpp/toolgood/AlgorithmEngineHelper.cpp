// AlgorithmEngineHelper.cpp - 对应 csharp/ToolGood.Algorithm/AlgorithmEngineHelper.cs
//                           + FunctionCache.cs / IFunctionCache.cs
#include "AlgorithmEngineHelper.h"

#include <cstdint>
#include <stdexcept>

#include "Internals/Functions/Compare/Compare.h"
#include "Internals/Functions/Operator/Operator.h"
#include "Internals/Functions/Value/Value.h"
#include "Lexer.h"
#include "Parser.h"
#include "UnitConversion/UnitConversion.h"

namespace toolgood {

namespace {

using CodePoints = std::vector<uint32_t>;
using CodePointsPtr = std::shared_ptr<const CodePoints>;

bool IsSpaceCp(uint32_t c) {
    return c == ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\f' || c == '\v';
}

std::string Trim(const std::string& s) {
    size_t b = 0, e = s.size();
    while (b < e && (s[b] == ' ' || s[b] == '\t' || s[b] == '\r' || s[b] == '\n' ||
                     s[b] == '\f' || s[b] == '\v'))
        b++;
    while (e > b && (s[e - 1] == ' ' || s[e - 1] == '\t' || s[e - 1] == '\r' ||
                     s[e - 1] == '\n' || s[e - 1] == '\f' || s[e - 1] == '\v'))
        e--;
    return s.substr(b, e - b);
}

std::string ToUpperAscii(const std::string& s) {
    std::string r = s;
    for (auto& c : r) {
        if (c >= 'a' && c <= 'z') c = static_cast<char>(c - 32);
    }
    return r;
}

/// 常量名（与 Parser.cpp 的 ClassifyConst 保持一致：这些标识符不是自定义参数）。
bool IsConstName(const std::string& upper) {
    return upper == "TRUE" || upper == "YES" || upper == "FALSE" || upper == "NO" ||
           upper == "NULL" || upper == "ALGORITHMVERSION" || upper == "ENGINEVERSION" ||
           upper == "E" || upper == "PI" || upper == "RAND" || upper == "GUID" ||
           upper == "NOW" || upper == "TODAY";
}

/// 该 token 能否作为操作数的结尾（用于区分中缀运算符与前缀运算符）。
bool IsOperandEnd(const std::string& kind) {
    return kind == "NUM" || kind == "STRING" || kind == "IDENT" || kind == "RPAREN" ||
           kind == "RBRACKET" || kind == "RBRACE";
}

/// 该 token 能否作为操作数的开头（用于识别百分号后缀）。
bool StartsOperand(const std::string& kind) {
    return kind == "NUM" || kind == "STRING" || kind == "IDENT" || kind == "LPAREN" ||
           kind == "LBRACKET" || kind == "LBRACE" || kind == "NOT" || kind == "OPADD" ||
           kind == "OPSUB";
}

/// 运算符优先级（与 Parser.cpp 的 PrecedenceOf 一致）。
int CalcPrecedence(const std::string& kind) {
    if (kind == "OPOR") return 2;
    if (kind == "OPAND") return 3;
    if (kind == "OPNE" || kind == "OPEQ") return 4;
    if (kind == "OPGT" || kind == "OPGE" || kind == "OPLT" || kind == "OPLE") return 5;
    if (kind == "OPCAT" || kind == "OPADD" || kind == "OPSUB") return 7;
    if (kind == "OPMUL" || kind == "OPDIV" || kind == "OPMOD") return 8;
    return 0;
}

CalculateTreeType CalcType(const std::string& kind) {
    if (kind == "OPADD") return CalculateTreeType::Add;
    if (kind == "OPSUB") return CalculateTreeType::Sub;
    if (kind == "OPMUL") return CalculateTreeType::Mul;
    if (kind == "OPDIV") return CalculateTreeType::Div;
    if (kind == "OPMOD") return CalculateTreeType::Mod;
    if (kind == "OPCAT") return CalculateTreeType::Connect;
    if (kind == "OPAND") return CalculateTreeType::And;
    if (kind == "OPOR") return CalculateTreeType::Or;
    if (kind == "OPGT") return CalculateTreeType::OpGt;
    if (kind == "OPLT") return CalculateTreeType::OpLt;
    if (kind == "OPGE") return CalculateTreeType::OpGe;
    if (kind == "OPLE") return CalculateTreeType::OpLe;
    if (kind == "OPEQ") return CalculateTreeType::OpEq;
    return CalculateTreeType::OpNe;
}

/// 条件树的优先级：只处理 And / Or（其余运算符留给计算树）。
int CondPrecedence(const std::string& kind) {
    if (kind == "OPOR") return 2;
    if (kind == "OPAND") return 3;
    return 0;
}

ConditionTreeType CondType(const std::string& kind) {
    return kind == "OPOR" ? ConditionTreeType::Or : ConditionTreeType::And;
}

/// 返回与 t[i] 的 '(' 配对的 ')' 下标；不配对返回 SIZE_MAX。
size_t MatchParen(const std::vector<Token>& t, size_t i, size_t j) {
    int depth = 0;
    for (size_t k = i; k < j; k++) {
        if (t[k].kind == "LPAREN") {
            depth++;
        } else if (t[k].kind == "RPAREN") {
            depth--;
            if (depth == 0) return k;
        }
    }
    return SIZE_MAX;
}

/// 判断 t[k] 是不是区间内的一级中缀运算符。
bool IsInfixAt(const std::vector<Token>& t, size_t i, size_t k) {
    if (k <= i) return false;
    if (!IsOperandEnd(t[k - 1].kind)) return false;
    // `50%` 里的 % 是后缀百分号，不作为中缀运算符
    if (t[k].kind == "OPMOD") {
        const std::string next = (k + 1 < t.size()) ? t[k + 1].kind : std::string("EOF");
        if (!StartsOperand(next)) return false;
    }
    return true;
}

void BuildCalcTree(const std::vector<Token>& t, const CodePointsPtr& source, size_t i, size_t j,
                   bool hasBracket, CalculateTree& out) {
    bool bracket = hasBracket;
    while (j - i >= 2 && t[i].kind == "LPAREN" && MatchParen(t, i, j) == j - 1) {
        i++;
        j--;
        bracket = true;
    }
    out.HasBracket = bracket;
    out.SetSource(source);
    if (i >= j) {
        out.Type = CalculateTreeType::String;
        return;
    }
    out.Start = t[i].start;
    out.End = t[j - 1].end;

    int depth = 0;
    int bestPrec = 0;
    size_t best = SIZE_MAX;
    for (size_t k = i; k < j; k++) {
        const std::string& kind = t[k].kind;
        if (kind == "LPAREN" || kind == "LBRACKET" || kind == "LBRACE") {
            depth++;
            continue;
        }
        if (kind == "RPAREN" || kind == "RBRACKET" || kind == "RBRACE") {
            depth--;
            continue;
        }
        if (depth != 0) continue;
        const int prec = CalcPrecedence(kind);
        if (prec == 0) continue;
        if (!IsInfixAt(t, i, k)) continue;
        // 左结合：同级运算符取最后一个作为根节点
        if (best == SIZE_MAX || prec <= bestPrec) {
            best = k;
            bestPrec = prec;
        }
    }
    if (best == SIZE_MAX) {
        out.Type = CalculateTreeType::String;
        return;
    }
    out.Type = CalcType(t[best].kind);
    out.Nodes.resize(2);
    BuildCalcTree(t, source, i, best, false, out.Nodes[0]);
    BuildCalcTree(t, source, best + 1, j, false, out.Nodes[1]);
}

void BuildConditionTree(const std::vector<Token>& t, const CodePointsPtr& source, size_t i,
                        size_t j, bool hasBracket, ConditionTree& out) {
    bool bracket = hasBracket;
    while (j - i >= 2 && t[i].kind == "LPAREN" && MatchParen(t, i, j) == j - 1) {
        i++;
        j--;
        bracket = true;
    }
    out.HasBracket = bracket;
    out.SetSource(source);
    if (i >= j) {
        out.Type = ConditionTreeType::String;
        return;
    }
    out.Start = t[i].start;
    out.End = t[j - 1].end;

    int depth = 0;
    int bestPrec = 0;
    size_t best = SIZE_MAX;
    for (size_t k = i; k < j; k++) {
        const std::string& kind = t[k].kind;
        if (kind == "LPAREN" || kind == "LBRACKET" || kind == "LBRACE") {
            depth++;
            continue;
        }
        if (kind == "RPAREN" || kind == "RBRACKET" || kind == "RBRACE") {
            depth--;
            continue;
        }
        if (depth != 0) continue;
        const int prec = CondPrecedence(kind);
        if (prec == 0) continue;
        if (!IsInfixAt(t, i, k)) continue;
        if (best == SIZE_MAX || prec <= bestPrec) {
            best = k;
            bestPrec = prec;
        }
    }
    if (best == SIZE_MAX) {
        out.Type = ConditionTreeType::String;
        return;
    }
    out.Type = CondType(t[best].kind);
    out.Nodes.resize(2);
    BuildConditionTree(t, source, i, best, false, out.Nodes[0]);
    BuildConditionTree(t, source, best + 1, j, false, out.Nodes[1]);
}

/// 去掉单位里的空白与括号（对应 C# 的 `[\s\(\)（）\[\]<>]` 正则）。
std::string StripUnitChars(const std::string& unit) {
    const CodePoints cps = Lexer::ToCodePoints(unit);
    CodePoints kept;
    kept.reserve(cps.size());
    for (uint32_t c : cps) {
        if (IsSpaceCp(c)) continue;
        if (c == '(' || c == ')' || c == '[' || c == ']' || c == '<' || c == '>') continue;
        if (c == 0xFF08 || c == 0xFF09) continue;  // 全角括号（）
        if (c == 0x3000) continue;                 // 全角空格
        kept.push_back(c);
    }
    return Lexer::FromCodePoints(kept, 0, kept.size());
}

bool TryConvertAll(const Decimal& src, const std::string& srcUnit, const std::string& tarUnit,
                   Decimal& result) {
    if (DistanceConverter::TryConvert(srcUnit, tarUnit, src, result)) return true;
    if (MassConverter::TryConvert(srcUnit, tarUnit, src, result)) return true;
    if (AreaConverter::TryConvert(srcUnit, tarUnit, src, result)) return true;
    if (VolumeConverter::TryConvert(srcUnit, tarUnit, src, result)) return true;
    return false;
}

/// 预解析校验（对应 C# 里先用 parser 检查语法，出错则返回 Error 树）。
void ValidateFormula(const std::string& exp) {
    Parser::ParseFunction(exp);
}

}  // namespace

bool AlgorithmEngineHelper::IsParameter(const std::string& parameter) {
    if (Trim(parameter).empty()) return false;
    try {
        DiyNameInfo diy = GetDiyNames(parameter);
        if (!diy.Functions.empty()) return false;
        if (diy.Parameters.size() == 1) {
            return diy.Parameters[0].Name == parameter;
        }
    } catch (...) {
        return false;
    }
    return false;
}

DiyNameInfo AlgorithmEngineHelper::GetDiyNames(const std::string& exp) {
    if (Trim(exp).empty()) {
        throw std::invalid_argument("Parameter exp invalid !");
    }
    std::vector<Token> tokens;
    try {
        ValidateFormula(exp);
        tokens = Tokenize(exp);
    } catch (const std::exception& ex) {
        throw std::invalid_argument(ex.what());
    }

    DiyNameInfo info;
    for (size_t k = 0; k + 1 < tokens.size(); k++) {
        const Token& tk = tokens[k];
        if (tk.kind != "IDENT") continue;
        const std::string& next = tokens[k + 1].kind;
        if (next == "LPAREN") {
            info.Functions.push_back(DiyNameKeyInfo{tk.text, tk.start, tk.end});
            continue;
        }
        // `a.b` 里的 b 是 JSON 键名，不是参数
        if (k > 0 && tokens[k - 1].kind == "DOT") continue;
        // `{key: value}` 里的键名，不是参数
        if (next == "COLON") continue;
        // 常量（TRUE / PI / NOW ...）不是参数
        if (IsConstName(ToUpperAscii(tk.text))) continue;
        info.Parameters.push_back(DiyNameKeyInfo{tk.text, tk.start, tk.end});
    }
    return info;
}

Decimal AlgorithmEngineHelper::UnitConversion(const Decimal& src, const std::string& oldSrcUnit,
                                              const std::string& oldTarUnit,
                                              const std::string& name) {
    if (Trim(oldSrcUnit).empty() || Trim(oldTarUnit).empty()) return src;
    if (oldSrcUnit == oldTarUnit) return src;

    Decimal result;
    if (TryConvertAll(src, oldSrcUnit, oldTarUnit, result)) return result;

    const std::string srcUnit = StripUnitChars(oldSrcUnit);
    const std::string tarUnit = StripUnitChars(oldTarUnit);
    if (TryConvertAll(src, srcUnit, tarUnit, result)) return result;

    if (name.empty()) {
        throw std::runtime_error("The input item has different units and cannot be converted from [" +
                                 srcUnit + "] to [" + tarUnit + "]");
    }
    throw std::runtime_error("The input item [" + name +
                             "] has different units and cannot be converted from [" + srcUnit +
                             "] to [" + tarUnit + "]");
}

Function AlgorithmEngineHelper::ParseFormula(const std::string& exp) {
    if (Trim(exp).empty()) {
        throw std::invalid_argument("Parameter exp invalid !");
    }
    try {
        return Parser::ParseFunction(exp);
    } catch (const std::exception& ex) {
        throw std::invalid_argument(ex.what());
    }
}

bool AlgorithmEngineHelper::CheckFormula(const std::string& exp) {
    if (Trim(exp).empty()) return false;
    try {
        ValidateFormula(exp);
        return true;
    } catch (...) {
        return false;
    }
}

ConditionTree AlgorithmEngineHelper::ParseCondition(const std::string& condition) {
    ConditionTree tree;
    if (Trim(condition).empty()) {
        tree.Type = ConditionTreeType::Error;
        tree.ErrorMessage = "condition is null";
        return tree;
    }
    try {
        ValidateFormula(condition);
        const CodePointsPtr source = std::make_shared<const CodePoints>(Lexer::ToCodePoints(condition));
        const std::vector<Token> tokens = Tokenize(condition);
        BuildConditionTree(tokens, source, 0, tokens.size() - 1, false, tree);
        return tree;
    } catch (const std::exception& ex) {
        ConditionTree err;
        err.Type = ConditionTreeType::Error;
        err.ErrorMessage = ex.what();
        return err;
    }
}

Function AlgorithmEngineHelper::Condition_And(const Function& left, const Function& right) {
    return NewFunction_AND({left, right});
}

Function AlgorithmEngineHelper::Condition_Or(const Function& left, const Function& right) {
    return NewFunction_OR({left, right});
}

Function AlgorithmEngineHelper::Error(const Function& txt) {
    return NewFunction_ERROR({txt});
}

CalculateTree AlgorithmEngineHelper::ParseCalculate(const std::string& exp) {
    CalculateTree tree;
    if (Trim(exp).empty()) {
        tree.Type = CalculateTreeType::Error;
        tree.ErrorMessage = "exp is null";
        return tree;
    }
    try {
        ValidateFormula(exp);
        const CodePointsPtr source = std::make_shared<const CodePoints>(Lexer::ToCodePoints(exp));
        const std::vector<Token> tokens = Tokenize(exp);
        BuildCalcTree(tokens, source, 0, tokens.size() - 1, false, tree);
        return tree;
    } catch (const std::exception& ex) {
        CalculateTree err;
        err.Type = CalculateTreeType::Error;
        err.ErrorMessage = ex.what();
        return err;
    }
}

Function AlgorithmEngineHelper::CombineCalculate(const Function& left, CombineCalculateType type,
                                                 const Function& right) {
    switch (type) {
        case CombineCalculateType::Add: return NewFunction_Add({left, right});
        case CombineCalculateType::Sub: return NewFunction_Sub({left, right});
        case CombineCalculateType::Mul: return NewFunction_Mul({left, right});
        case CombineCalculateType::Div: return NewFunction_Div({left, right});
        case CombineCalculateType::Mod: return NewFunction_Mod({left, right});
        case CombineCalculateType::Connect: return NewFunction_Connect({left, right});
        case CombineCalculateType::And: return NewFunction_AND({left, right});
        case CombineCalculateType::Or: return NewFunction_OR({left, right});
        case CombineCalculateType::OpGt: return NewFunction_GT({left, right});
        case CombineCalculateType::OpLt: return NewFunction_LT({left, right});
        case CombineCalculateType::OpGe: return NewFunction_GE({left, right});
        case CombineCalculateType::OpLe: return NewFunction_LE({left, right});
        case CombineCalculateType::OpEq: return NewFunction_EQ({left, right});
        case CombineCalculateType::OpNe:
        default: return NewFunction_NE({left, right});
    }
}

DiyNameInfo FunctionCache::GetDiyNamesWithCache(const std::string& exp) {
    auto it = diyNameCache_.find(exp);
    if (it != diyNameCache_.end()) return it->second;
    DiyNameInfo info = AlgorithmEngineHelper::GetDiyNames(exp);
    diyNameCache_[exp] = info;
    return info;
}

Function FunctionCache::CreateCalculate(const CalculateTree& tree) {
    if (tree.Type == CalculateTreeType::String) {
        // 叶子节点带原文，可直接交给 ParseFormula；同名子表达式命中缓存
        const std::string text = tree.Text();
        auto it = calculateCache_.find(text);
        if (it != calculateCache_.end()) return it->second;
        Function leaf = AlgorithmEngineHelper::ParseFormula(text);
        calculateCache_[text] = leaf;
        return leaf;
    }
    if (tree.Type == CalculateTreeType::Error) {
        throw std::runtime_error(tree.ErrorMessage);
    }
    if (tree.Nodes.size() < 2) {
        throw std::runtime_error("calculate tree node is invalid");
    }
    Function leftFunc = CreateCalculate(tree.Nodes[0]);
    Function rightFunc = CreateCalculate(tree.Nodes[1]);
    return AlgorithmEngineHelper::CombineCalculate(
        leftFunc, static_cast<CombineCalculateType>(static_cast<uint8_t>(tree.Type)), rightFunc);
}

Function FunctionCache::CreateCondition(const ConditionTree& tree) {
    if (tree.Type == ConditionTreeType::String) {
        // 叶子转交计算树（与 C# 一致：走 ParseCalculate + CreateCalculate）
        CalculateTree calcTree = AlgorithmEngineHelper::ParseCalculate(tree.Text());
        return CreateCalculate(calcTree);
    }
    if (tree.Nodes.size() < 2) {
        // C# 在此处会因 Nodes 为空直接索引越界；这里给出明确的错误信息
        throw std::runtime_error(tree.ErrorMessage);
    }
    Function leftFunc = CreateCondition(tree.Nodes[0]);
    Function rightFunc = CreateCondition(tree.Nodes[1]);
    if (tree.Type == ConditionTreeType::And) {
        return AlgorithmEngineHelper::Condition_And(leftFunc, rightFunc);
    }
    if (tree.Type == ConditionTreeType::Or) {
        return AlgorithmEngineHelper::Condition_Or(leftFunc, rightFunc);
    }
    throw std::runtime_error(tree.ErrorMessage);
}

Function FunctionCache::ParseWithCache(const std::string& funExp) {
    auto it = calculateCache_.find(funExp);
    if (it != calculateCache_.end()) return it->second;
    CalculateTree tree = AlgorithmEngineHelper::ParseCalculate(funExp);
    Function func = CreateCalculate(tree);
    calculateCache_[funExp] = func;
    return func;
}

Function FunctionCache::ParseConditionWithCache(const std::string& funExp) {
    auto it = conditionCache_.find(funExp);
    if (it != conditionCache_.end()) return it->second;
    ConditionTree tree = AlgorithmEngineHelper::ParseCondition(funExp);
    Function func = CreateCondition(tree);
    conditionCache_[funExp] = func;
    return func;
}

}  // namespace toolgood
