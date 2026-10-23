// AlgorithmEngineHelper.h - 对应 csharp/ToolGood.Algorithm/AlgorithmEngineHelper.cs
//                       与 FunctionCache.cs / IFunctionCache.cs
//
// C# 的 AlgorithmEngineHelper 依赖 ANTLR 的 Visitor（MathSplitVisitor /
// MathSplitVisitor2 / DiyNameVisitor）。本移植用 Lexer 的 token 流直接构造
// CalculateTree / ConditionTree（等价语义），不再保留 ANTLR 层。
//
// 异常约定（对应 C# 的异常类型）：
//   ArgumentException            -> std::invalid_argument
//   FormatException / Exception  -> std::runtime_error
#pragma once

#include <map>
#include <string>
#include <vector>

#include "Decimal.h"
#include "Enums.h"
#include "Internals/CalculateTree.h"
#include "Internals/ConditionTree.h"
#include "Internals/DiyNameInfo.h"
#include "Internals/FunctionBase.h"

namespace toolgood {

/// 算法引擎助手（对应静态类 AlgorithmEngineHelper）。
class AlgorithmEngineHelper {
public:
    /// 是不是参数（对应 IsParameter）
    static bool IsParameter(const std::string& parameter);

    /// 获取 DIY 名称（对应 GetDiyNames）
    static DiyNameInfo GetDiyNames(const std::string& exp);

    /// 单位转换（对应 UnitConversion），失败抛 std::runtime_error
    static Decimal UnitConversion(const Decimal& src, const std::string& oldSrcUnit,
                                  const std::string& oldTarUnit,
                                  const std::string& name = std::string());

    /// 编译公式（对应 ParseFormula），失败抛 std::invalid_argument
    static Function ParseFormula(const std::string& exp);

    /// 检查公式是否正确（对应 CheckFormula）
    static bool CheckFormula(const std::string& exp);

    /// 解析条件（对应 ParseCondition）
    static ConditionTree ParseCondition(const std::string& condition);

    /// 逻辑与（对应 Condition_And）
    static Function Condition_And(const Function& left, const Function& right);

    /// 逻辑或（对应 Condition_Or）
    static Function Condition_Or(const Function& left, const Function& right);

    /// 错误函数包装（对应 Error）
    static Function Error(const Function& txt);

    /// 解析计算表达式（对应 ParseCalculate）
    static CalculateTree ParseCalculate(const std::string& exp);

    /// 拼接计算表达式（对应 CombineCalculate）
    static Function CombineCalculate(const Function& left, CombineCalculateType type,
                                     const Function& right);
};

/// 函数缓存接口（对应 IFunctionCache）。
class IFunctionCache {
public:
    virtual ~IFunctionCache() = default;

    /// 获取自定义名称信息（带缓存）。
    virtual DiyNameInfo GetDiyNamesWithCache(const std::string& exp) = 0;
    /// 解析计算表达式（带缓存）。
    virtual Function ParseWithCache(const std::string& funExp) = 0;
    /// 解析条件表达式（带缓存）。
    virtual Function ParseConditionWithCache(const std::string& funExp) = 0;
};

/// 函数缓存（对应 FunctionCache）。
/// 说明：C# 用 ConcurrentDictionary 保证线程安全；C++ 侧未引入锁，
/// 与 AlgorithmEngine 的 UseParseCache 一样，要求单线程使用同一实例。
class FunctionCache : public IFunctionCache {
public:
    DiyNameInfo GetDiyNamesWithCache(const std::string& exp) override;
    Function ParseWithCache(const std::string& funExp) override;
    Function ParseConditionWithCache(const std::string& funExp) override;

private:
    Function CreateCalculate(const CalculateTree& tree);
    Function CreateCondition(const ConditionTree& tree);

    std::map<std::string, Function> calculateCache_;
    std::map<std::string, Function> conditionCache_;
    std::map<std::string, DiyNameInfo> diyNameCache_;
};

}  // namespace toolgood
