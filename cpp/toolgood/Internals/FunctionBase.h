// FunctionBase.h - 对应 csharp/.../Internals/Functions/FunctionBase.cs 与 Function_1..N.cs
#pragma once

#include <functional>
#include <memory>
#include <string>
#include <vector>

#include "../Enums.h"
#include "../Operands/Operand.h"

namespace toolgood {

class AlgorithmEngine;
class NoneEngine;

/// 延迟参数求值回调：解析临时参数（对应 C# 的 Func<AlgorithmEngine,string,Operand>）。
using TempParameter = std::function<Operand(AlgorithmEngine&, const std::string&)>;

class FunctionBase;
using Function = std::shared_ptr<FunctionBase>;

/// 函数节点基类。
class FunctionBase {
public:
    virtual ~FunctionBase() = default;

    /// 函数名（用于错误信息与 ToString）。
    virtual std::string Name() const = 0;

    /// 执行计算。
    virtual Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp = nullptr) = 0;

    /// 结果类型。
    virtual OperandType GetResultType() const = 0;

    /// 收集参数类型信息（对应 C# internal abstract GetParameterTypes）。
    virtual void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                                   OperandType operandType, const std::string& op = std::string(),
                                   const std::string& val = std::string()) = 0;

    /// 收集参数类型信息（公开入口）。
    std::vector<ParameterType> GetParameterTypes(AlgorithmEngine& engine);

    /// 表达式文本还原。
    virtual void ToString(std::string& sb, bool addBrackets) = 0;
    std::string ToString();

    // ---- 工具：转换（带参数错误信息）----
    Operand ConvertToText(const Operand& arg, int paramIndex) const;
    Operand ConvertToBoolean(const Operand& arg, int paramIndex) const;
    Operand ConvertToNumber(const Operand& arg, int paramIndex) const;
    Operand ConvertToArray(const Operand& arg, int paramIndex) const;
    Operand ConvertToDate(const Operand& arg, int paramIndex) const;

    // ---- 工具：固定错误 ----
    Operand ParameterError(int paramIndex) const;
    Operand FunctionError() const;
    Operand CompareError() const;
    Operand Div0Error() const;
    Operand NumError() const;
};

/// 带 N 个参数的函数基类（合并 C# 的 Function_1..Function_6 / Function_N）。
/// 参数下标统一从 1 开始（与 C# 一致）。
class FunctionN : public FunctionBase {
public:
    explicit FunctionN(std::vector<Function> args) : args_(std::move(args)) {}

    /// 参数个数。
    size_t ArgCount() const { return args_.size(); }
    /// 第 i 个参数节点（0 基）。
    const Function& Arg(size_t i) const { return args_[i]; }

    void ToString(std::string& sb, bool addBrackets) override;
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType operandType, const std::string& op = std::string(),
                           const std::string& val = std::string()) override;

protected:
    std::vector<Function> args_;

    /// 求值第 index 个参数（1 基）。
    Operand EvalArg(int index, AlgorithmEngine& engine, const TempParameter* tp) const;
    /// 求值并转文本；失败返回错误操作数。
    Operand GetText(int index, AlgorithmEngine& engine, const TempParameter* tp) const;
    Operand GetNumber(int index, AlgorithmEngine& engine, const TempParameter* tp) const;
    Operand GetBoolean(int index, AlgorithmEngine& engine, const TempParameter* tp) const;
    Operand GetDate(int index, AlgorithmEngine& engine, const TempParameter* tp) const;
    Operand GetArray(int index, AlgorithmEngine& engine, const TempParameter* tp) const;
    /// 依次求值全部参数，遇到错误即返回；成功返回 Null() 且填充 out。
    Operand EvaluateAll(AlgorithmEngine& engine, const TempParameter* tp,
                        std::vector<Operand>& out) const;
    /// 文本形式的参数输出（用于 ToString）
    void AppendFunction(std::string& sb, const std::string& name, const std::string& open,
                        const std::string& close);
};

/// 参数个数校验（对应 C# 构造函数的 ArgumentException 检查）。
/// max < 0 表示不限。不满足时抛 std::runtime_error。
void RequireArgs(const std::vector<Function>& args, const std::string& name, int min, int max);

}  // namespace toolgood
