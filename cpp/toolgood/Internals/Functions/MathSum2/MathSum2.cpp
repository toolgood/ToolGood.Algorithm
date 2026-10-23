// MathSum2.cpp - 对应 csharp/ToolGood.Algorithm/Internals/Functions/MathSum2/（28 个统计分布函数）
#include "MathSum2.h"

#include <cmath>
#include <functional>
#include <initializer_list>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

#include "../../../MathEx.h"
#include "../../../MathNet/MathNet.h"
#include "../../../Operands/Operand.h"
#include "../../Registry.h"

namespace toolgood {

namespace {

using mn::DecimalMax;

/// 对应 MathEx.Log 之外的 decimal 溢出语义：C# 超出 ±decimal.MaxValue 会抛异常，
/// 这里统一判定为「已溢出/无效」，由调用方转成 FunctionError()。
bool IsOverflow(double v) {
    return !std::isfinite(v) || std::fabs(v) > DecimalMax;
}

/// 对应 FunctionBase.TryGetInt（decimal 超出 int 范围时返回 false）
bool TryGetInt(const Operand& arg, int& value) {
    const Decimal& d = arg.NumberValue();
    if (!d.FitsInt32()) return false;
    value = d.Int();
    return true;
}

/// double 结果转操作数；溢出（C# 会抛 OverflowException 的地方）转成函数错误。
Operand CheckedOperand(const FunctionBase& fn, double v) {
    if (IsOverflow(v)) return fn.FunctionError();
    return Operand::Create(v);
}

/// 对应 C# 各函数对 GetParameterTypes 的覆写（按参数顺序给出期望类型）
void ParamTypes(const FunctionN& fn, NoneEngine& noneEngine, std::vector<ParameterType>& result,
                std::initializer_list<OperandType> types) {
    size_t i = 0;
    for (OperandType t : types) {
        fn.Arg(i)->GetParameterTypes(noneEngine, result, t);
        i++;
    }
}

// ---------------------------------------------------------------------------
// BESSELI / BESSELJ / BESSELK / BESSELY
// ---------------------------------------------------------------------------
class Function_BESSELI : public FunctionN {
public:
    explicit Function_BESSELI(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "BesselI", 2, 2);
    }

    std::string Name() const override { return "BesselI"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        double x = args1.DoubleValue();
        // n 取整数部分，且必须为非负整数(与 Excel BESSELI 一致)
        int n = 0;
        if (!TryGetInt(args2, n) || n < 0) {
            return ParameterError(2);
        }

        // 复用 SpecialFunctions 中基于 Chebyshev 展开的实现(移植自 MathNet.Numerics)
        return CheckedOperand(*this, mn::SpecialFunctions::BesselI(n, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_BESSELJ : public FunctionN {
public:
    explicit Function_BESSELJ(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "BesselJ", 2, 2);
    }

    std::string Name() const override { return "BesselJ"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        double x = args1.DoubleValue();
        int n = 0;
        if (!TryGetInt(args2, n) || n < 0) {
            return ParameterError(2);
        }

        return CheckedOperand(*this, mn::SpecialFunctions::BesselJ(n, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_BESSELK : public FunctionN {
public:
    explicit Function_BESSELK(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "BesselK", 2, 2);
    }

    std::string Name() const override { return "BesselK"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        double x = args1.DoubleValue();
        // 对应 C# 的 (int)Math.Truncate(args2.NumberValue)
        if (!args2.NumberValue().FitsInt32()) {
            return ParameterError(2);
        }
        int n = static_cast<int>(std::trunc(args2.DoubleValue()));

        if (x <= 0) {
            return ParameterError(1);
        }
        if (n < 0) {
            return ParameterError(2);
        }

        // 复用 SpecialFunctions 中基于 Chebyshev 展开的实现(移植自 MathNet.Numerics)
        return CheckedOperand(*this, mn::SpecialFunctions::BesselK(n, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_BESSELY : public FunctionN {
public:
    explicit Function_BESSELY(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "BesselY", 2, 2);
    }

    std::string Name() const override { return "BesselY"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        double x = args1.DoubleValue();
        // 对应 C# 的 (int)Math.Truncate(args2.NumberValue)
        if (!args2.NumberValue().FitsInt32()) {
            return ParameterError(2);
        }
        int n = static_cast<int>(std::trunc(args2.DoubleValue()));

        if (x <= 0) {
            return ParameterError(1);
        }
        if (n < 0) {
            return ParameterError(2);
        }

        return CheckedOperand(*this, mn::SpecialFunctions::BesselY(n, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// BETADIST / BETAINV
// ---------------------------------------------------------------------------
class Function_BETADIST : public FunctionN {
public:
    explicit Function_BETADIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "BetaDist", 3, 3);
    }

    std::string Name() const override { return "BetaDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        double x = args1.DoubleValue();
        if (x < 0 || x > 1) {
            return ParameterError(1);
        }
        double alpha = args2.DoubleValue();
        if (alpha <= 0) {
            return ParameterError(2);
        }
        double beta = args3.DoubleValue();
        if (beta <= 0) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::Beta::CDF(alpha, beta, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_BETAINV : public FunctionN {
public:
    explicit Function_BETAINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "BetaInv", 3, 3);
    }

    std::string Name() const override { return "BetaInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        double p = args1.DoubleValue();
        // 概率必须为开区间 (0,1)，与 Excel BETA.INV 的 #NUM! 判定保持一致
        if (p <= 0 || p >= 1) {
            return ParameterError(1);
        }
        double alpha = args2.DoubleValue();
        if (alpha <= 0) {
            return ParameterError(2);
        }
        double beta = args3.DoubleValue();
        if (beta <= 0) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::Beta::InvCDF(alpha, beta, p));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// BINOMDIST
// ---------------------------------------------------------------------------
class Function_BINOMDIST : public FunctionN {
public:
    explicit Function_BINOMDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "BinomDist", 4, 4);
    }

    std::string Name() const override { return "BinomDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        auto args4 = GetBoolean(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;

        int n2 = 0;
        if (!TryGetInt(args2, n2) || n2 < 0) {
            return ParameterError(2);
        }
        int k = 0;
        if (!TryGetInt(args1, k) || k < 0 || k > n2) {
            return ParameterError(1);
        }
        double n3 = args3.DoubleValue();
        if (n3 < 0 || n3 > 1) {
            return ParameterError(3);
        }
        bool state = args4.BooleanValue();
        // 极端输入导致底层数值计算失败时，与 Excel 返回 #NUM! 保持一致
        return CheckedOperand(*this, state ? mn::dist::Binomial::CDF(n3, n2, k)
                                           : mn::dist::Binomial::PMF(n3, n2, k));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER, OperandType::BOOLEAN});
    }
};

// ---------------------------------------------------------------------------
// ERF / ERFC
// ---------------------------------------------------------------------------
class Function_ERF : public FunctionN {
public:
    explicit Function_ERF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Erf", 1, 1);
    }

    std::string Name() const override { return "Erf"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        double x = args1.DoubleValue();
        return Operand::Create(mn::SpecialFunctions::Erf(x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER});
    }
};

class Function_ERFC : public FunctionN {
public:
    explicit Function_ERFC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Erfc", 1, 1);
    }

    std::string Name() const override { return "Erfc"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        double x = args1.DoubleValue();
        return Operand::Create(mn::SpecialFunctions::Erfc(x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// EXPONDIST
// ---------------------------------------------------------------------------
class Function_EXPONDIST : public FunctionN {
public:
    explicit Function_EXPONDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "ExpOnDist", 3, 3);
    }

    std::string Name() const override { return "ExpOnDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetBoolean(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        Decimal n1 = args1.NumberValue();
        if (n1.IsNegative()) {
            return ParameterError(1);
        }
        Decimal rate = args2.NumberValue();
        if (!rate.IsPositive()) {
            return ParameterError(2);
        }
        bool state = args3.BooleanValue();
        // 对应 C# ExponDist：state ? 1-Exp(-rate*x) : rate*Exp(-rate*x)
        const Decimal power = rate.Mul(n1).Neg();
        Decimal v;
        try {
            v = state ? Decimal::One.Sub(MathEx::Exp(power)) : rate.Mul(MathEx::Exp(power));
        } catch (const std::overflow_error&) {
            return FunctionError();
        }
        return Operand::Create(v);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::BOOLEAN});
    }
};

// ---------------------------------------------------------------------------
// FDIST / FINV
// ---------------------------------------------------------------------------
class Function_FDIST : public FunctionN {
public:
    explicit Function_FDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "FDist", 3, 3);
    }

    std::string Name() const override { return "FDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        double x = args1.DoubleValue();
        // Excel 允许 x=0(FDIST(0,d1,d2)=1)，仅当 x<0 时报错
        if (x < 0) {
            return ParameterError(1);
        }
        int degreesFreedom = 0;
        if (!TryGetInt(args2, degreesFreedom) || degreesFreedom <= 0) {
            return ParameterError(2);
        }
        int degreesFreedom2 = 0;
        if (!TryGetInt(args3, degreesFreedom2) || degreesFreedom2 <= 0) {
            return ParameterError(3);
        }
        // 极端输入导致底层数值计算失败时，与 Excel 返回 #NUM! 保持一致
        return CheckedOperand(*this, 1.0 - mn::dist::FisherSnedecor::CDF(degreesFreedom, degreesFreedom2, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_FINV : public FunctionN {
public:
    explicit Function_FINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "FInv", 3, 3);
    }

    std::string Name() const override { return "FInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        double p = args1.DoubleValue();
        if (p <= 0 || p >= 1) {
            return ParameterError(1);
        }
        int degreesFreedom = 0;
        if (!TryGetInt(args2, degreesFreedom) || degreesFreedom <= 0) {
            return ParameterError(2);
        }
        int degreesFreedom2 = 0;
        if (!TryGetInt(args3, degreesFreedom2) || degreesFreedom2 <= 0) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::FisherSnedecor::InvCDF(degreesFreedom, degreesFreedom2, 1.0 - p));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// FISHER / FISHERINV
// ---------------------------------------------------------------------------
class Function_FISHER : public FunctionN {
public:
    explicit Function_FISHER(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Fisher", 1, 1);
    }

    std::string Name() const override { return "Fisher"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        if (x.GreaterOrEqual(Decimal::One) || x.LessOrEqual(Decimal::One.Neg())) {
            return ParameterError(1);
        }
        // 对应 C#：0.5m * MathEx.Log((1+x)/(1-x))
        const Decimal ratio = MathEx::Div(Decimal::One.Add(x), Decimal::One.Sub(x));
        try {
            return Operand::Create(MathEx::Log(ratio).Mul(Decimal::FromString("0.5")));
        } catch (const std::exception&) {
            return FunctionError();
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER});
    }
};

class Function_FISHERINV : public FunctionN {
public:
    explicit Function_FISHERINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "FisherInv", 1, 1);
    }

    std::string Name() const override { return "FisherInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        try {
            const Decimal e = MathEx::Exp(x.Mul(Decimal::FromInt(2)));
            return Operand::Create(MathEx::Div(e.Sub(Decimal::One), e.Add(Decimal::One)));
        } catch (const std::overflow_error&) {
            // 2x 超出 decimal 可表示范围(约 ±66)时,tanh 趋近于 ±1,与 Java 版 double 计算结果一致
            return Operand::Create(x.IsNegative() ? Decimal::One.Neg() : Decimal::One);
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// GAMMADIST / GAMMAINV / GAMMALN
// ---------------------------------------------------------------------------
class Function_GAMMADIST : public FunctionN {
public:
    explicit Function_GAMMADIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "GammaDist", 4, 4);
    }

    std::string Name() const override { return "GammaDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        auto args4 = GetBoolean(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;

        double x = args1.DoubleValue();
        if (x < 0) {
            return ParameterError(1);
        }
        double alpha = args2.DoubleValue();
        if (alpha <= 0) {
            return ParameterError(2);
        }
        double beta = args3.DoubleValue();
        if (beta <= 0) {
            return ParameterError(3);
        }
        bool cumulative = args4.BooleanValue();
        // Excel 的 beta 是尺度参数，MathNet 的 rate = 1/beta
        double rate = 1 / beta;
        // 如 x=0 且 alpha<1 时零底负数次幂会失败，与 Excel 返回 #NUM! 保持一致
        return CheckedOperand(*this, cumulative ? mn::dist::Gamma::CDF(alpha, rate, x)
                                               : mn::dist::Gamma::PDF(alpha, rate, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER, OperandType::BOOLEAN});
    }
};

class Function_GAMMAINV : public FunctionN {
public:
    explicit Function_GAMMAINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "GammaInv", 3, 3);
    }

    std::string Name() const override { return "GammaInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        double probability = args1.DoubleValue();
        // 概率必须为开区间 (0,1)：p=0 结果恒为 0，p=1 时分布无上界
        if (probability <= 0 || probability >= 1) {
            return ParameterError(1);
        }
        double alpha = args2.DoubleValue();
        if (alpha <= 0) {
            return ParameterError(2);
        }
        double beta = args3.DoubleValue();
        if (beta <= 0) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::Gamma::InvCDF(alpha, 1 / beta, probability));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_GAMMALN : public FunctionN {
public:
    explicit Function_GAMMALN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "GammaLn", 1, 1);
    }

    std::string Name() const override { return "GammaLn"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        if (args1.DoubleValue() <= 0) {
            return ParameterError(1);
        }
        return Operand::Create(mn::SpecialFunctions::GammaLn(args1.DoubleValue()));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// HYPGEOMDIST
// ---------------------------------------------------------------------------
class Function_HYPGEOMDIST : public FunctionN {
public:
    explicit Function_HYPGEOMDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HypgeomDist", 4, 4);
    }

    std::string Name() const override { return "HypgeomDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        auto args4 = GetNumber(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;

        int k = 0;
        if (!TryGetInt(args1, k) || k < 0) {
            return ParameterError(1);
        }
        int draws = 0;
        if (!TryGetInt(args2, draws) || draws < 0) {
            return ParameterError(2);
        }
        int success = 0;
        if (!TryGetInt(args3, success) || success < 0) {
            return ParameterError(3);
        }
        int population = 0;
        if (!TryGetInt(args4, population) || population < 0) {
            return ParameterError(4);
        }
        // 交叉约束：k 必须落在 [max(0, draws+success-population), min(draws, success)] 区间内
        if (k > draws) {
            return ParameterError(1);
        }
        if (k > success) {
            return ParameterError(1);
        }
        if (k < static_cast<long long>(draws) + success - population) {
            return ParameterError(1);
        }
        if (success > population) {
            return ParameterError(3);
        }
        if (draws > population) {
            return ParameterError(2);
        }
        return CheckedOperand(*this, mn::dist::Hypergeometric::PMF(population, success, draws, k));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// LOGINV / LOGNORMDIST
// ---------------------------------------------------------------------------
class Function_LOGINV : public FunctionN {
public:
    explicit Function_LOGINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "LogInv", 3, 3);
    }

    std::string Name() const override { return "LogInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        double probability = args1.DoubleValue();
        if (probability <= 0 || probability >= 1) {
            return ParameterError(1);
        }

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        double n3 = args3.DoubleValue();
        if (n3 <= 0) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::LogNormal::InvCDF(args2.DoubleValue(), n3, probability));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_LOGNORMDIST : public FunctionN {
public:
    explicit Function_LOGNORMDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "LogNormDist", 3, 3);
    }

    std::string Name() const override { return "LogNormDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        double x = args1.DoubleValue();
        if (x <= 0) {
            return ParameterError(1);
        }

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        double n3 = args3.DoubleValue();
        if (n3 <= 0) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::LogNormal::CDF(args2.DoubleValue(), n3, x));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// NEGBINOMDIST
// ---------------------------------------------------------------------------
class Function_NEGBINOMDIST : public FunctionN {
public:
    explicit Function_NEGBINOMDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NegBinomDist", 3, 3);
    }

    std::string Name() const override { return "NegBinomDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        int k = 0;
        if (!TryGetInt(args1, k) || k < 0) {
            return ParameterError(1);
        }
        double r = args2.DoubleValue();
        if (r <= 0) {
            return ParameterError(2);
        }
        double p = args3.DoubleValue();
        // p 必须为开区间 (0,1)：p=0 或 p=1 时底层 Log 会抛异常
        if (p <= 0 || p >= 1) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::NegativeBinomial::PMF(r, p, k));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// NORMDIST / NORMINV / NORMSDIST / NORMSINV
// ---------------------------------------------------------------------------
class Function_NORMDIST : public FunctionN {
public:
    explicit Function_NORMDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NormDist", 4, 4);
    }

    std::string Name() const override { return "NormDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        auto args4 = GetBoolean(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;

        double num = args1.DoubleValue();
        double avg = args2.DoubleValue();
        double STDEV = args3.DoubleValue();
        if (STDEV <= 0) {
            return ParameterError(3);
        }
        bool b = args4.BooleanValue();

        // |num-avg|/STDEV 过大时指数运算结果溢出，与 Excel 返回 #NUM! 保持一致
        return CheckedOperand(*this, b ? mn::dist::Normal::CDF(avg, STDEV, num)
                                       : mn::dist::Normal::PDF(avg, STDEV, num));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER, OperandType::BOOLEAN});
    }
};

class Function_NORMINV : public FunctionN {
public:
    explicit Function_NORMINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NormInv", 3, 3);
    }

    std::string Name() const override { return "NormInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        double p = args1.DoubleValue();
        if (p <= 0 || p >= 1) {
            return ParameterError(1);
        }
        double avg = args2.DoubleValue();
        double STDEV = args3.DoubleValue();
        if (STDEV <= 0) {
            return ParameterError(3);
        }
        return CheckedOperand(*this, mn::dist::Normal::InvCDF(avg, STDEV, p));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_NORMSDIST : public FunctionN {
public:
    explicit Function_NORMSDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NormSDist", 1, 1);
    }

    std::string Name() const override { return "NormSDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        double num = args1.DoubleValue();
        return Operand::Create(mn::dist::Normal::CDF(0, 1, num));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER});
    }
};

class Function_NORMSINV : public FunctionN {
public:
    explicit Function_NORMSINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NormSInv", 1, 1);
    }

    std::string Name() const override { return "NormSInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        double p = args1.DoubleValue();
        if (p <= 0 || p >= 1) {
            return ParameterError(1);
        }
        return Operand::Create(mn::dist::Normal::InvCDF(0, 1, p));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// TDIST / TINV
// ---------------------------------------------------------------------------
class Function_TDIST : public FunctionN {
public:
    explicit Function_TDIST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "TDist", 3, 3);
    }

    std::string Name() const override { return "TDist"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;
        double x = args1.DoubleValue();
        // Excel 允许 x=0(TDIST(0,df,1)=0.5, TDIST(0,df,2)=1)，仅当 x<0 时报错
        if (x < 0) {
            return ParameterError(1);
        }
        int degreesFreedom = 0;
        if (!TryGetInt(args2, degreesFreedom) || degreesFreedom <= 0) {
            return ParameterError(2);
        }
        int tails = 0;
        if (!TryGetInt(args3, tails) || tails < 1 || tails > 2) {
            return ParameterError(3);
        }
        switch (tails) {
            case 1:
                return CheckedOperand(
                    *this, 1.0 - mn::dist::StudentT::CDF(0, 1, degreesFreedom, x));
            case 2:
                return CheckedOperand(*this, 1.0 - mn::dist::StudentT::CDF(0, 1, degreesFreedom, x) +
                                                mn::dist::StudentT::CDF(0, 1, degreesFreedom, -x));
            default:
                return ParameterError(3);
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER});
    }
};

class Function_TINV : public FunctionN {
public:
    explicit Function_TINV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "TInv", 2, 2);
    }

    std::string Name() const override { return "TInv"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        double p = args1.DoubleValue();
        // Excel 允许 p=1(TInv(1,df)=0)，仅当 p<=0 或 p>1 时报错
        if (p <= 0 || p > 1) {
            return ParameterError(1);
        }
        int degreesFreedom = 0;
        if (!TryGetInt(args2, degreesFreedom) || degreesFreedom <= 0) {
            return ParameterError(2);
        }
        return CheckedOperand(
            *this, -mn::dist::StudentT::InvCDF(0, 1, degreesFreedom, p / 2));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::NUMBER, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// WEIBULL
// ---------------------------------------------------------------------------
/// C# decimal 能表示的最小正数（1e-28）
const Decimal& MinEpsilon() {
    static const Decimal v = Decimal::FromString("0.0000000000000000000000000001");
    return v;
}

/// 对应 C# Weibull.EpsilonOf
Decimal EpsilonOf(const Decimal& value) {
    if (value.IsZero()) return MinEpsilon();
    Decimal epsilon = MinEpsilon();
    while (value.Sub(epsilon).Equal(value)) {
        epsilon = epsilon.Mul(Decimal::FromString("0.1"));
    }
    return epsilon;
}

/// 对应 C# Weibull.PositiveEpsilonOf
Decimal PositiveEpsilonOf(const Decimal& value) {
    return EpsilonOf(value).Mul(Decimal::FromInt(2));
}

/// 对应 C# Weibull.Series（Kahan 求和）
Decimal Series(const std::function<Decimal()>& nextSummand) {
    Decimal compensation = Decimal::Zero;
    const Decimal factor = Decimal::FromInt(1 << 16);
    Decimal sum = nextSummand();
    Decimal current;
    do {
        current = nextSummand();
        const Decimal y = current.Sub(compensation);
        const Decimal t = sum.Add(y);
        compensation = t.Sub(sum);
        compensation = compensation.Sub(y);
        sum = t;
    } while (sum.Abs().Less(factor.Mul(current).Abs()));
    return sum;
}

/// 对应 C# Weibull.ExponentialMinusOne
Decimal ExponentialMinusOne(const Decimal& power) {
    const Decimal x = power.Abs();
    if (x.Greater(Decimal::FromString("0.1"))) {
        return MathEx::Exp(power).Sub(Decimal::One);
    }
    if (x.Less(PositiveEpsilonOf(x))) {
        return power;
    }
    // Series Expansion to x^k / k!
    int k = 0;
    Decimal term = Decimal::One;
    return Series([&k, &term, &power]() {
        k++;
        term = MathEx::Div(term.Mul(power), Decimal::FromInt(k));
        return term;
    });
}

/// 对应 C# Function_WEIBULL.PDF
Decimal WeibullPDF(const Decimal& shape, const Decimal& scale, const Decimal& x) {
    if (!x.IsNegative()) {
        if (x.IsZero() && shape.Equal(Decimal::One)) {
            return MathEx::Div(shape, scale);
        }
        const Decimal ratio = MathEx::Div(x, scale);
        return MathEx::Div(shape.Mul(MathEx::Pow(ratio, shape.Sub(Decimal::One)))
                               .Mul(MathEx::Exp(MathEx::Pow(ratio, shape).Neg())),
                           scale);
    }
    return Decimal::Zero;
}

/// 对应 C# Function_WEIBULL.CDF
Decimal WeibullCDF(const Decimal& shape, const Decimal& scale, const Decimal& x) {
    if (x.IsNegative()) return Decimal::Zero;
    const Decimal ratio = MathEx::Div(x, scale);
    return ExponentialMinusOne(MathEx::Pow(ratio, shape).Neg()).Neg();
}

class Function_WEIBULL : public FunctionN {
public:
    explicit Function_WEIBULL(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Weibull", 4, 4);
    }

    std::string Name() const override { return "Weibull"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        auto args3 = GetNumber(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        auto args4 = GetBoolean(4, engine, tp);
        if (args4.IsErrorOrNone()) return args4;
        Decimal x = args1.NumberValue();
        Decimal shape = args2.NumberValue();
        if (!shape.IsPositive()) {
            return ParameterError(2);
        }
        Decimal scale = args3.NumberValue();
        if (!scale.IsPositive()) {
            return ParameterError(3);
        }
        bool state = args4.BooleanValue();

        // x 过大时 Pow/Exp 结果溢出、x=0 且 shape<1 时零的负次幂，均与 Excel 返回 #NUM! 保持一致
        try {
            return Operand::Create(state ? WeibullCDF(shape, scale, x)
                                         : WeibullPDF(shape, scale, x));
        } catch (const std::exception&) {
            return FunctionError();
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER, OperandType::BOOLEAN});
    }
};

}  // namespace

// ---------------------------------------------------------------------------
// 工厂函数
// ---------------------------------------------------------------------------
Function NewFunction_BESSELI(std::vector<Function> args) {
    return std::make_shared<Function_BESSELI>(std::move(args));
}
Function NewFunction_BESSELJ(std::vector<Function> args) {
    return std::make_shared<Function_BESSELJ>(std::move(args));
}
Function NewFunction_BESSELK(std::vector<Function> args) {
    return std::make_shared<Function_BESSELK>(std::move(args));
}
Function NewFunction_BESSELY(std::vector<Function> args) {
    return std::make_shared<Function_BESSELY>(std::move(args));
}
Function NewFunction_BETADIST(std::vector<Function> args) {
    return std::make_shared<Function_BETADIST>(std::move(args));
}
Function NewFunction_BETAINV(std::vector<Function> args) {
    return std::make_shared<Function_BETAINV>(std::move(args));
}
Function NewFunction_BINOMDIST(std::vector<Function> args) {
    return std::make_shared<Function_BINOMDIST>(std::move(args));
}
Function NewFunction_ERF(std::vector<Function> args) {
    return std::make_shared<Function_ERF>(std::move(args));
}
Function NewFunction_ERFC(std::vector<Function> args) {
    return std::make_shared<Function_ERFC>(std::move(args));
}
Function NewFunction_EXPONDIST(std::vector<Function> args) {
    return std::make_shared<Function_EXPONDIST>(std::move(args));
}
Function NewFunction_FDIST(std::vector<Function> args) {
    return std::make_shared<Function_FDIST>(std::move(args));
}
Function NewFunction_FINV(std::vector<Function> args) {
    return std::make_shared<Function_FINV>(std::move(args));
}
Function NewFunction_FISHER(std::vector<Function> args) {
    return std::make_shared<Function_FISHER>(std::move(args));
}
Function NewFunction_FISHERINV(std::vector<Function> args) {
    return std::make_shared<Function_FISHERINV>(std::move(args));
}
Function NewFunction_GAMMADIST(std::vector<Function> args) {
    return std::make_shared<Function_GAMMADIST>(std::move(args));
}
Function NewFunction_GAMMAINV(std::vector<Function> args) {
    return std::make_shared<Function_GAMMAINV>(std::move(args));
}
Function NewFunction_GAMMALN(std::vector<Function> args) {
    return std::make_shared<Function_GAMMALN>(std::move(args));
}
Function NewFunction_HYPGEOMDIST(std::vector<Function> args) {
    return std::make_shared<Function_HYPGEOMDIST>(std::move(args));
}
Function NewFunction_LOGINV(std::vector<Function> args) {
    return std::make_shared<Function_LOGINV>(std::move(args));
}
Function NewFunction_LOGNORMDIST(std::vector<Function> args) {
    return std::make_shared<Function_LOGNORMDIST>(std::move(args));
}
Function NewFunction_NEGBINOMDIST(std::vector<Function> args) {
    return std::make_shared<Function_NEGBINOMDIST>(std::move(args));
}
Function NewFunction_NORMDIST(std::vector<Function> args) {
    return std::make_shared<Function_NORMDIST>(std::move(args));
}
Function NewFunction_NORMINV(std::vector<Function> args) {
    return std::make_shared<Function_NORMINV>(std::move(args));
}
Function NewFunction_NORMSDIST(std::vector<Function> args) {
    return std::make_shared<Function_NORMSDIST>(std::move(args));
}
Function NewFunction_NORMSINV(std::vector<Function> args) {
    return std::make_shared<Function_NORMSINV>(std::move(args));
}
Function NewFunction_TDIST(std::vector<Function> args) {
    return std::make_shared<Function_TDIST>(std::move(args));
}
Function NewFunction_TINV(std::vector<Function> args) {
    return std::make_shared<Function_TINV>(std::move(args));
}
Function NewFunction_WEIBULL(std::vector<Function> args) {
    return std::make_shared<Function_WEIBULL>(std::move(args));
}

void Registermathsum2() {
    RegisterFunction("BESSELI", [](const std::vector<Function>& a) { return NewFunction_BESSELI(a); });
    RegisterFunction("BESSELJ", [](const std::vector<Function>& a) { return NewFunction_BESSELJ(a); });
    RegisterFunction("BESSELK", [](const std::vector<Function>& a) { return NewFunction_BESSELK(a); });
    RegisterFunction("BESSELY", [](const std::vector<Function>& a) { return NewFunction_BESSELY(a); });
    RegisterFunction("BETADIST", [](const std::vector<Function>& a) { return NewFunction_BETADIST(a); });
    RegisterFunction("BETAINV", [](const std::vector<Function>& a) { return NewFunction_BETAINV(a); });
    RegisterFunction("BINOMDIST", [](const std::vector<Function>& a) { return NewFunction_BINOMDIST(a); });
    RegisterFunction("ERF", [](const std::vector<Function>& a) { return NewFunction_ERF(a); });
    RegisterFunction("ERFC", [](const std::vector<Function>& a) { return NewFunction_ERFC(a); });
    RegisterFunction("EXPONDIST", [](const std::vector<Function>& a) { return NewFunction_EXPONDIST(a); });
    RegisterFunction("FDIST", [](const std::vector<Function>& a) { return NewFunction_FDIST(a); });
    RegisterFunction("FINV", [](const std::vector<Function>& a) { return NewFunction_FINV(a); });
    RegisterFunction("FISHER", [](const std::vector<Function>& a) { return NewFunction_FISHER(a); });
    RegisterFunction("FISHERINV", [](const std::vector<Function>& a) { return NewFunction_FISHERINV(a); });
    RegisterFunction("GAMMADIST", [](const std::vector<Function>& a) { return NewFunction_GAMMADIST(a); });
    RegisterFunction("GAMMAINV", [](const std::vector<Function>& a) { return NewFunction_GAMMAINV(a); });
    RegisterFunction("GAMMALN", [](const std::vector<Function>& a) { return NewFunction_GAMMALN(a); });
    RegisterFunction("HYPGEOMDIST", [](const std::vector<Function>& a) { return NewFunction_HYPGEOMDIST(a); });
    RegisterFunction("LOGINV", [](const std::vector<Function>& a) { return NewFunction_LOGINV(a); });
    RegisterFunction("LOGNORMDIST", [](const std::vector<Function>& a) { return NewFunction_LOGNORMDIST(a); });
    RegisterFunction("NEGBINOMDIST", [](const std::vector<Function>& a) { return NewFunction_NEGBINOMDIST(a); });
    RegisterFunction("NORMDIST", [](const std::vector<Function>& a) { return NewFunction_NORMDIST(a); });
    RegisterFunction("NORMINV", [](const std::vector<Function>& a) { return NewFunction_NORMINV(a); });
    RegisterFunction("NORMSDIST", [](const std::vector<Function>& a) { return NewFunction_NORMSDIST(a); });
    RegisterFunction("NORMSINV", [](const std::vector<Function>& a) { return NewFunction_NORMSINV(a); });
    RegisterFunction("TDIST", [](const std::vector<Function>& a) { return NewFunction_TDIST(a); });
    RegisterFunction("TINV", [](const std::vector<Function>& a) { return NewFunction_TINV(a); });
    RegisterFunction("WEIBULL", [](const std::vector<Function>& a) { return NewFunction_WEIBULL(a); });
}

}  // namespace toolgood
