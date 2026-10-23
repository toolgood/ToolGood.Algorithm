// CalculationLogic.cpp - 对应 csharp/ToolGood.Algorithm/CalculationLogic/*.cs
#include "CalculationLogic.h"

#include <cstdint>
#include <stdexcept>

#include "../Lexer.h"

namespace toolgood {

namespace {

/// MyDate -> "yyyy-MM-dd HH:mm:ss"（对应 C# 的 DateTime.ToString("yyyy-MM-dd HH:mm:ss")）
std::string FormatDateTimeText(const MyDate& value) {
    return value.ToString("yyyy-MM-dd HH:mm:ss");
}

/// 文本转义（对应 C# 的 value.Replace("\"", "\\\"")）
std::string EscapeQuote(const std::string& value) {
    std::string r;
    r.reserve(value.size());
    for (char c : value) {
        if (c == '"') r += '\\';
        r += c;
    }
    return r;
}

/// 反斜杠与引号转义（对应 GetOperand 里的 Replace("\\","\\\\").Replace("\"","\\\"")）
std::string EscapeJsonText(const std::string& value) {
    std::string r;
    r.reserve(value.size());
    for (char c : value) {
        if (c == '\\' || c == '"') r += '\\';
        r += c;
    }
    return r;
}

}  // namespace

std::string CalculationLogicInfo::ToInfoString() const {
    const std::string remark = Remark.empty() ? std::string() : (" // " + Remark);
    const int layer = Layer < 0 ? 0 : (Layer > 1000 ? 1000 : Layer);
    const std::string layerStr(static_cast<size_t>(layer) * 3, ' ');

    switch (LogicType) {
        case CalculationLogicType::BlankLine: return std::string();
        case CalculationLogicType::Scene: return "===== " + Name + " =====";
        case CalculationLogicType::InitValue: return Name + "=" + Exp + ";";
        case CalculationLogicType::Condition:
            if (Value.IsError()) {
                return "[条件][错误] " + layerStr + "if " + Exp + ": // " + Exp2 + " = " +
                       Value.ErrorMsg() + remark;
            }
            if (Value.BooleanValue()) {
                return "[成功] " + layerStr + "if " + Exp + ": // " + Exp2 + remark;
            }
            return "[失败] " + layerStr + "if " + Exp + ": // " + Exp2 + remark;
        case CalculationLogicType::SetFormula:
            if (Value.IsError()) {
                return "[赋值][错误] " + layerStr + Name + " = " + Exp + " = " + Exp2 + " // " +
                       Value.ErrorMsg() + remark;
            }
            return "[赋值] " + layerStr + Name + " = " + Exp + " = " + Exp2 + " = " +
                   Value.ToText().TextValue() + remark;
        case CalculationLogicType::SetValue:
            return "[赋值] " + layerStr + Name + " = " + Exp + remark;
        case CalculationLogicType::Error: return "[错误] " + layerStr + Exp;
        default: break;
    }
    return "[错误]: " + layerStr + Name + Exp + remark;
}

CalculationLogicEngine::CalculationLogicEngine(IFunctionCache& functionCache,
                                               bool useCalculationLogicInfo)
    : functionCache_(functionCache), useCalculationLogicInfo_(useCalculationLogicInfo) {}

void CalculationLogicEngine::SetSceneName(const std::string& scene) {
    if (useCalculationLogicInfo_) {
        CalculationLogicInfo info;
        info.LogicType = CalculationLogicType::Scene;
        info.Name = scene;
        calculationLogicInfos_.push_back(std::move(info));
    }
}

// ------------------------------------------------------------------ InitValue
void CalculationLogicEngine::InitValue(const std::string& key, const std::string& value,
                                       int layer, const std::string& remark) {
    InitValueCore(key, Operand::Create(value), value, layer, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, const std::string& value,
                                       const std::string& remark) {
    InitValue(key, value, 0, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, const Decimal& value, int layer,
                                       const std::string& remark) {
    InitValueCore(key, Operand::Create(value), value.ToString(), layer, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, const Decimal& value,
                                       const std::string& remark) {
    InitValue(key, value, 0, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, double value, int layer,
                                       const std::string& remark) {
    InitValueCore(key, Operand::Create(value), Decimal::FromDouble(value).ToString(), layer, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, double value,
                                       const std::string& remark) {
    InitValue(key, value, 0, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, bool value, int layer,
                                       const std::string& remark) {
    InitValueCore(key, Operand::Create(value), value ? "True" : "False", layer, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, bool value,
                                       const std::string& remark) {
    InitValue(key, value, 0, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, int value, int layer,
                                       const std::string& remark) {
    InitValueCore(key, Operand::Create(value), std::to_string(value), layer, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, int value,
                                       const std::string& remark) {
    InitValue(key, value, 0, remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, const MyDate& value, int layer,
                                       const std::string& remark) {
    InitValueCore(key, Operand::Create(value), "\"" + FormatDateTimeText(value) + "\"", layer,
                  remark);
}

void CalculationLogicEngine::InitValue(const std::string& key, const MyDate& value,
                                       const std::string& remark) {
    InitValue(key, value, 0, remark);
}

void CalculationLogicEngine::InitValueCore(const std::string& key, const Operand& value,
                                           const std::string& exp, int layer,
                                           const std::string& remark) {
    engine_.AddParameter(key, value);
    if (useCalculationLogicInfo_) {
        CalculationLogicInfo info;
        info.LogicType = CalculationLogicType::InitValue;
        info.Name = key;
        info.Exp = exp;
        info.Layer = layer;
        info.Remark = remark;
        initValueInfos_.push_back(std::move(info));
    }
}

// ------------------------------------------------------------- CheckCondition
bool CalculationLogicEngine::CheckCondition(const std::string& condition, int layer,
                                            const std::string& remark) {
    Function func = functionCache_.ParseWithCache(condition);
    Operand operand = func->Evaluate(engine_);
    operand = operand.ToBoolean("The condition must be a boolean value!");
    if (operand.IsError()) {
        if (useCalculationLogicInfo_) {
            const std::string expStr = ExpAnalysis(condition);
            CalculationLogicInfo info;
            info.LogicType = CalculationLogicType::Condition;
            info.Exp = condition;
            info.Exp2 = expStr;
            info.Value = operand;
            info.Layer = layer;
            info.Remark = remark;
            calculationLogicInfos_.push_back(std::move(info));
        }
        throw std::runtime_error(operand.ErrorMsg());
    }
    if (useCalculationLogicInfo_) {
        const std::string expStr = ExpAnalysis(condition);
        CalculationLogicInfo info;
        info.LogicType = CalculationLogicType::Condition;
        info.Exp = condition;
        info.Exp2 = expStr;
        info.Value = operand;
        info.Layer = layer;
        info.Remark = remark;
        calculationLogicInfos_.push_back(std::move(info));
    }
    return operand.BooleanValue();
}

bool CalculationLogicEngine::CheckCondition(const std::string& condition,
                                            const std::string& remark) {
    return CheckCondition(condition, 0, remark);
}

// ----------------------------------------------------------------- SetFormula
void CalculationLogicEngine::SetFormula(const std::string& key, const std::string& exp, int layer,
                                        const std::string& remark) {
    Function func = functionCache_.ParseWithCache(exp);
    Operand operand = func->Evaluate(engine_);
    if (operand.IsError()) {
        if (useCalculationLogicInfo_) {
            const std::string expStr = ExpAnalysis(exp);
            CalculationLogicInfo info;
            info.LogicType = CalculationLogicType::SetFormula;
            info.Name = key;
            info.Exp = exp;
            info.Exp2 = expStr;
            info.Value = operand;
            info.Layer = layer;
            info.Remark = remark;
            calculationLogicInfos_.push_back(std::move(info));
        }
        throw std::runtime_error(operand.ErrorMsg());
    }
    engine_.AddParameter(key, operand);

    if (useCalculationLogicInfo_) {
        const std::string expStr = ExpAnalysis(exp);
        CalculationLogicInfo info;
        info.LogicType = CalculationLogicType::SetFormula;
        info.Name = key;
        info.Exp = exp;
        info.Exp2 = expStr;
        info.Value = operand;
        info.Layer = layer;
        info.Remark = remark;
        calculationLogicInfos_.push_back(std::move(info));
    }
}

void CalculationLogicEngine::SetFormula(const std::string& key, const std::string& exp,
                                        const std::string& remark) {
    SetFormula(key, exp, 0, remark);
}

// ------------------------------------------------------------------- SetValue
void CalculationLogicEngine::SetValue(const std::string& key, const std::string& value, int layer,
                                      const std::string& remark) {
    SetValueCore(key, Operand::Create(value), "\"" + EscapeQuote(value) + "\"", layer, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, const std::string& value,
                                      const std::string& remark) {
    SetValue(key, value, 0, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, const Decimal& value, int layer,
                                      const std::string& remark) {
    SetValueCore(key, Operand::Create(value), value.ToString(), layer, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, const Decimal& value,
                                      const std::string& remark) {
    SetValue(key, value, 0, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, double value, int layer,
                                      const std::string& remark) {
    SetValueCore(key, Operand::Create(value), Decimal::FromDouble(value).ToString(), layer, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, double value,
                                      const std::string& remark) {
    SetValue(key, value, 0, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, bool value, int layer,
                                      const std::string& remark) {
    SetValueCore(key, Operand::Create(value), value ? "True" : "False", layer, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, bool value,
                                      const std::string& remark) {
    SetValue(key, value, 0, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, int value, int layer,
                                      const std::string& remark) {
    SetValueCore(key, Operand::Create(value), std::to_string(value), layer, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, int value,
                                      const std::string& remark) {
    SetValue(key, value, 0, remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, const MyDate& value, int layer,
                                      const std::string& remark) {
    SetValueCore(key, Operand::Create(value), "\"" + FormatDateTimeText(value) + "\"", layer,
                 remark);
}

void CalculationLogicEngine::SetValue(const std::string& key, const MyDate& value,
                                      const std::string& remark) {
    SetValue(key, value, 0, remark);
}

void CalculationLogicEngine::SetValueCore(const std::string& key, const Operand& value,
                                          const std::string& exp, int layer,
                                          const std::string& remark) {
    engine_.AddParameter(key, value);
    if (useCalculationLogicInfo_) {
        CalculationLogicInfo info;
        info.LogicType = CalculationLogicType::SetValue;
        info.Name = key;
        info.Exp = exp;
        info.Layer = layer;
        info.Remark = remark;
        calculationLogicInfos_.push_back(std::move(info));
    }
}

// ------------------------------------------------------------------ BlankLine
void CalculationLogicEngine::BlankLine() {
    if (useCalculationLogicInfo_) {
        CalculationLogicInfo info;
        info.LogicType = CalculationLogicType::BlankLine;
        calculationLogicInfos_.push_back(std::move(info));
    }
}

// ------------------------------------------------------------- ExpAnalysis
std::string CalculationLogicEngine::ExpAnalysis(const std::string& exp) {
    const DiyNameInfo diyNameInfo = functionCache_.GetDiyNamesWithCache(exp);
    const std::vector<uint32_t> cps = Lexer::ToCodePoints(exp);
    std::string sb;
    size_t index = 0;
    for (const auto& parameter : diyNameInfo.Parameters) {
        const size_t start = static_cast<size_t>(parameter.Start < 0 ? 0 : parameter.Start);
        const size_t end = static_cast<size_t>(parameter.End < 0 ? 0 : parameter.End);
        if (index < start) {
            sb += Lexer::FromCodePoints(cps, index, start);
        }
        sb += GetOperandText(engine_, parameter.Name);
        index = end + 1;
    }
    if (index < cps.size()) {
        sb += Lexer::FromCodePoints(cps, index, cps.size());
    }
    return sb;
}

std::string CalculationLogicEngine::GetOperandText(AlgorithmEngineEx& engine,
                                                   const std::string& name) {
    const Operand operand = engine.GetParameter(name);
    if (operand.IsError()) {
        return "error(\"" + EscapeJsonText(operand.ErrorMsg()) + "\")";
    }
    if (operand.Type() == OperandType::TEXT) {
        return operand.ToString();
    }
    return operand.ToText().TextValue();
}

// ------------------------------------------------------------------ GetValue
Operand CalculationLogicEngine::GetValue(const std::string& key) {
    return engine_.GetParameter(key);
}

// --------------------------------------------------------------- ToInfoString
std::string CalculationLogicEngine::ToInfoString() const {
    if (!useCalculationLogicInfo_) return std::string();
    std::string sb;
    if (!initValueInfos_.empty()) {
        sb += "[初始] ";
        for (const auto& item : initValueInfos_) {
            sb += item.ToInfoString();
        }
        sb += "\n";
    }
    for (const auto& item : calculationLogicInfos_) {
        sb += item.ToInfoString();
        sb += "\n";
    }
    return sb;
}

}  // namespace toolgood
