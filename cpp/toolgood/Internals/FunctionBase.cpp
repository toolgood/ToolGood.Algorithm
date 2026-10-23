#include "FunctionBase.h"

#include <stdexcept>

#include "../AlgorithmEngine.h"

namespace toolgood {

std::string FunctionBase::ToString() {
    std::string sb;
    ToString(sb, false);
    return sb;
}

std::vector<ParameterType> FunctionBase::GetParameterTypes(AlgorithmEngine& engine) {
    NoneEngine noneEngine(engine);
    std::vector<ParameterType> result;
    GetParameterTypes(noneEngine, result, OperandType::NONE);
    return result;
}

Operand FunctionBase::ConvertToText(const Operand& arg, int paramIndex) const {
    return arg.ToText(Operand::ErrorFormat("Function '{0}' parameter {1} is error!",
                                           {Name(), std::to_string(paramIndex)})
                          .ErrorMsg());
}

Operand FunctionBase::ConvertToBoolean(const Operand& arg, int paramIndex) const {
    return arg.ToBoolean(Operand::ErrorFormat("Function '{0}' parameter {1} is error!",
                                              {Name(), std::to_string(paramIndex)})
                             .ErrorMsg());
}

Operand FunctionBase::ConvertToNumber(const Operand& arg, int paramIndex) const {
    return arg.ToNumber(Operand::ErrorFormat("Function '{0}' parameter {1} is error!",
                                             {Name(), std::to_string(paramIndex)})
                            .ErrorMsg());
}

Operand FunctionBase::ConvertToArray(const Operand& arg, int paramIndex) const {
    return arg.ToArray(Operand::ErrorFormat("Function '{0}' parameter {1} is error!",
                                            {Name(), std::to_string(paramIndex)})
                           .ErrorMsg());
}

Operand FunctionBase::ConvertToDate(const Operand& arg, int paramIndex) const {
    return arg.ToMyDate(Operand::ErrorFormat("Function '{0}' parameter {1} is error!",
                                             {Name(), std::to_string(paramIndex)})
                            .ErrorMsg());
}

Operand FunctionBase::ParameterError(int paramIndex) const {
    return Operand::ErrorFormat("Function '{0}' parameter {1} is error!",
                                {Name(), std::to_string(paramIndex)});
}

Operand FunctionBase::FunctionError() const {
    return Operand::ErrorFormat("Function '{0}' parameter is error!", {Name()});
}

Operand FunctionBase::CompareError() const {
    return Operand::ErrorFormat("Function '{0}' compare is error.", {Name()});
}

Operand FunctionBase::Div0Error() const {
    return Operand::ErrorFormat("Function '{0}' Div 0 error!", {Name()});
}

Operand FunctionBase::NumError() const {
    return Operand::ErrorFormat("Function '{0}' Num error!", {Name()});
}

// ---------------- FunctionN ----------------

void FunctionN::ToString(std::string& sb, bool addBrackets) {
    sb += Name();
    sb += '(';
    for (size_t i = 0; i < args_.size(); i++) {
        if (i > 0) sb += ", ";
        args_[i]->ToString(sb, false);
    }
    sb += ')';
}

void FunctionN::GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                                  OperandType operandType, const std::string& op,
                                  const std::string& val) {
    (void)operandType;
    (void)op;
    (void)val;
    for (auto& f : args_) {
        f->GetParameterTypes(noneEngine, result, OperandType::NONE);
    }
}

Operand FunctionN::EvalArg(int index, AlgorithmEngine& engine, const TempParameter* tp) const {
    return args_[static_cast<size_t>(index - 1)]->Evaluate(engine, tp);
}

Operand FunctionN::GetText(int index, AlgorithmEngine& engine, const TempParameter* tp) const {
    Operand v = EvalArg(index, engine, tp);
    if (v.IsErrorOrNone()) return v;
    if (v.IsText()) return v;
    return ConvertToText(v, index);
}

Operand FunctionN::GetNumber(int index, AlgorithmEngine& engine, const TempParameter* tp) const {
    Operand v = EvalArg(index, engine, tp);
    if (v.IsErrorOrNone()) return v;
    if (v.IsNumber()) return v;
    return ConvertToNumber(v, index);
}

Operand FunctionN::GetBoolean(int index, AlgorithmEngine& engine, const TempParameter* tp) const {
    Operand v = EvalArg(index, engine, tp);
    if (v.IsErrorOrNone()) return v;
    if (v.IsBoolean()) return v;
    return ConvertToBoolean(v, index);
}

Operand FunctionN::GetDate(int index, AlgorithmEngine& engine, const TempParameter* tp) const {
    Operand v = EvalArg(index, engine, tp);
    if (v.IsErrorOrNone()) return v;
    if (v.IsDate()) return v;
    return ConvertToDate(v, index);
}

Operand FunctionN::GetArray(int index, AlgorithmEngine& engine, const TempParameter* tp) const {
    Operand v = EvalArg(index, engine, tp);
    if (v.IsErrorOrNone()) return v;
    if (v.IsArray()) return v;
    return ConvertToArray(v, index);
}

Operand FunctionN::EvaluateAll(AlgorithmEngine& engine, const TempParameter* tp,
                               std::vector<Operand>& out) const {
    for (size_t i = 0; i < args_.size(); i++) {
        Operand v = args_[i]->Evaluate(engine, tp);
        if (v.IsErrorOrNone()) return v;
        out.push_back(v);
    }
    return Operand::Null();
}

// ---------------- 参数个数校验 ----------------

void RequireArgs(const std::vector<Function>& args, const std::string& name, int min, int max) {
    const int n = static_cast<int>(args.size());
    if (n >= min && (max < 0 || n <= max)) return;
    auto plural = [](int c) { return c == 1 ? std::string(" parameter.") : std::string(" parameters."); };
    std::string msg = "Function '" + name + "' ";
    if (min == max) {
        msg += "requires exactly " + std::to_string(min) + plural(min);
    } else if (max < 0) {
        msg += "requires at least " + std::to_string(min) + plural(min);
    } else if (min == 0) {
        msg += "requires at most " + std::to_string(max) + plural(max);
    } else {
        msg += "requires " + std::to_string(min) + " to " + std::to_string(max) + " parameters.";
    }
    throw std::runtime_error(msg);
}

}  // namespace toolgood
