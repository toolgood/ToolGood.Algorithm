// AlgorithmEngine.h - 对应 csharp/ToolGood.Algorithm/AlgorithmEngine.cs
#pragma once

#include <map>
#include <memory>
#include <string>
#include <vector>

#include "Decimal.h"
#include "Enums.h"
#include "Internals/FunctionBase.h"
#include "MyDate.h"
#include "Operands/Operand.h"

namespace toolgood {

/// 算法引擎。自定义参数/函数请继承并重写 GetParameter / ExecuteDiyFunction。
class AlgorithmEngine {
public:
    AlgorithmEngine() = default;
    virtual ~AlgorithmEngine() = default;

    int ExcelIndex = 1;

    /// 使用本地时间，影响时间戳转化
    bool UseLocalTime = true;
    DistanceUnitType DistanceUnit = DistanceUnitType::M;
    AreaUnitType AreaUnit = AreaUnitType::M2;
    VolumeUnitType VolumeUnit = VolumeUnitType::M3;
    MassUnitType MassUnit = MassUnitType::KG;
    /// 最后一个错误
    std::string LastError;
    /// 是否使用严格模式（影响逻辑运算符短路行为）
    bool UseStrictMode = true;
    /// 使用解析缓存（不要在多线程环境共享同一个引擎）
    bool UseParseCache = false;

    void SetUseExcelIndex(bool value) { ExcelIndex = value ? 1 : 0; }

    /// 自定义参数（请重写）
    virtual Operand GetParameter(const std::string& parameter);
    /// 自定义函数（请重写）
    virtual Operand ExecuteDiyFunction(const std::string& parameter, const std::vector<Operand>& args);

    /// 编译公式；失败时设置 LastError 并抛 std::runtime_error。
    virtual Function Parse(const std::string& exp);
    /// 编译公式；失败返回 nullptr，不抛错。
    virtual Function ParseWithoutError(const std::string& exp);

    /// 执行函数。
    virtual Operand Evaluate(const Function& function);

    // ---- TryEvaluate ----
    int TryEvaluate(const std::string& exp, int def);
    int64_t TryEvaluate(const std::string& exp, int64_t def);
    double TryEvaluate(const std::string& exp, double def);
    Decimal TryEvaluate(const std::string& exp, const Decimal& def);
    std::string TryEvaluate(const std::string& exp, const std::string& def);
    bool TryEvaluate(const std::string& exp, bool def);
    MyDate TryEvaluate(const std::string& exp, const MyDate& def);
    /// 解析并求值的便捷入口（等价于 Evaluate(Parse(exp))）。
    Operand Calculate(const std::string& exp);

    /// 取得参数（带 tempParameter 支持）
    static Operand GetParameterValue(AlgorithmEngine& engine, const TempParameter* tp,
                                     const std::string& name);

protected:
    std::map<std::string, Function> parseCache_;

private:
    Function ParseInternal(const std::string& exp, bool throwOnError);
};

/// 只用于类型推导的引擎：所有参数返回 NONE（对应 Internals/Functions/NoneEngine.cs）。
class NoneEngine : public AlgorithmEngine {
public:
    explicit NoneEngine(AlgorithmEngine& engine) : AlgorithmEngine(engine) {}
    Operand GetParameter(const std::string&) override { return Operand::None(); }
};

}  // namespace toolgood
