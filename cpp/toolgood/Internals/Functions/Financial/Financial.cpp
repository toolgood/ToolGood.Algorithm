// Financial.cpp - 对应 csharp/.../Internals/Functions/Financial/（16 个财务函数）
// 说明：
//   - FinancialMath.TryPowPositiveBase 的守卫（底数 > 0、|指数| <= 1000000）与失败返回原样照抄；
//   - MathEx.Pow/Log/Exp 一律走 decimal 版（与 C# 相同的算法与舍入），
//     指数超出 int 范围、结果溢出等 C# 抛异常的情形转成计算失败；
//   - IRR/XIRR/RATE 的牛顿迭代次数(100)、收敛阈值(1e-12 / 1e-10)与失败返回值完全照抄 C#。
#include "Financial.h"

#include <cmath>
#include <limits>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

#include "../../../AlgorithmEngine.h"
#include "../../../MathEx.h"
#include "../../../MyDate.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

// ---------------------------------------------------------------------------
// 通用辅助
// ---------------------------------------------------------------------------

/// 对应 FinancialMath.MaxSafeExponent
const Decimal& MaxSafeExponent() {
    static const Decimal v = Decimal::FromInt(1000000);
    return v;
}

/// 牛顿迭代的斜率阈值 1e-12（对应 C# 的 1e-12m）
const Decimal& EpsSlope() {
    static const Decimal v = Decimal::FromString("0.000000000001");
    return v;
}

/// 牛顿迭代的步长阈值 1e-10（对应 C# 的 1e-10m）
const Decimal& EpsStep() {
    static const Decimal v = Decimal::FromString("0.0000000001");
    return v;
}

/// 对应 C# MathEx.Pow(decimal, decimal)。
/// true 表示计算成功；false 表示 C# 端会抛 OverflowException / InvalidOperationException。
bool PowDecimal(const Decimal& value, const Decimal& power, Decimal& result) {
    try {
        result = MathEx::Pow(value, power);
        return true;
    } catch (const std::exception&) {
        result = Decimal::Zero;
        return false;
    }
}

/// 对应 FinancialMath.TryPowPositiveBase：以正数为底求幂，失败返回 false
bool TryPowPositiveBase(const Decimal& baseValue, const Decimal& exponent, Decimal& result) {
    result = Decimal::Zero;
    if (!baseValue.IsPositive()) return false;
    if (exponent.Greater(MaxSafeExponent()) || exponent.Less(MaxSafeExponent().Neg())) return false;
    return PowDecimal(baseValue, exponent, result);
}

/// 对应 C# MathEx.Log(decimal)：参数必须大于 0
bool TryLogDecimal(const Decimal& value, Decimal& result) {
    if (!value.IsPositive()) return false;
    try {
        result = MathEx::Log(value);
        return true;
    } catch (const std::exception&) {
        result = Decimal::Zero;
        return false;
    }
}

/// 数组元素转数值；错误文案与 C# 一致。失败返回 false 并写出错误操作数。
bool CollectNumbers(const std::string& name, int index, const Operand& array,
                    std::vector<Decimal>& values, Operand& error) {
    for (const auto& v : array.ArrayValue()) {
        if (v.IsNumber()) {
            values.push_back(v.NumberValue());
            continue;
        }
        const Operand v2 =
            v.ToNumber("Function '" + name + "' parameter " + std::to_string(index) + " is error!");
        if (v2.IsErrorOrNone()) {
            error = v2;
            return false;
        }
        values.push_back(v2.NumberValue());
    }
    return true;
}

/// 数组元素转日期（日期或可解析的文本）；年月日为空的纯时间串视为失败
bool CollectDates(const Operand& array, std::vector<MyDate>& dates) {
    for (const auto& d : array.ArrayValue()) {
        if (d.IsDate()) {
            dates.push_back(d.DateValue());
        } else if (d.IsText()) {
            MyDate md;
            if (!MyDate::TryParse(d.TextValue(), md) || !md.HasDate()) return false;
            dates.push_back(md);
        } else {
            return false;
        }
    }
    return true;
}

/// 对应 C# (a - b).TotalDays
Decimal TotalDays(const MyDate& start, const MyDate& end) {
    const int64_t dayDiff = end.ToDays() - start.ToDays();
    const int64_t secondDiff = static_cast<int64_t>(end.ToSecondsOfDay()) - start.ToSecondsOfDay();
    Decimal days = Decimal::FromInt(dayDiff);
    if (secondDiff != 0) {
        days = days.Add(Decimal::FromInt(secondDiff).Div(Decimal::FromInt(86400)));
    }
    return days;
}

/// 对应 C# 的 365.0m
const Decimal& DaysInYear() {
    static const Decimal v = Decimal::FromString("365.0");
    return v;
}

/// 对应 C# IRR.NewtonRaphsonIRR；不收敛返回 false（C# 抛 InvalidOperationException）
bool NewtonRaphsonIRR(const std::vector<Decimal>& values, Decimal rate, Decimal& result) {
    for (int iter = 0; iter < 100; iter++) {
        Decimal npv = Decimal::Zero;
        Decimal dnpv = Decimal::Zero;
        const Decimal onePlusRate = Decimal::One.Add(rate);
        if (onePlusRate.IsZero()) return false;  // C# DivideByZeroException
        Decimal factor = Decimal::One;

        for (size_t i = 0; i < values.size(); i++) {
            npv = npv.Add(values[i].Div(factor));
            dnpv = dnpv.Sub(Decimal::FromInt(static_cast<int64_t>(i))
                                .Mul(values[i])
                                .Div(factor.Mul(onePlusRate)));
            factor = factor.Mul(onePlusRate);
        }

        if (dnpv.Abs().Less(EpsSlope())) break;
        const Decimal newRate = rate.Sub(npv.Div(dnpv));

        if (newRate.Sub(rate).Abs().Less(EpsStep())) {
            result = newRate;
            return true;
        }
        rate = newRate;
    }
    return false;
}

/// 对应 C# RATE.NewtonRaphson
bool NewtonRaphsonRate(const Decimal& n, const Decimal& p, const Decimal& v, const Decimal& f,
                       Decimal type, Decimal rate, Decimal& result) {
    for (int i = 0; i < 100; i++) {
        if (rate.IsZero()) return false;  // C# DivideByZeroException
        const Decimal onePlusRate = Decimal::One.Add(rate);
        Decimal factor;
        Decimal powNMinus1;
        if (!PowDecimal(onePlusRate, n, factor)) return false;
        if (!PowDecimal(onePlusRate, n.Sub(Decimal::One), powNMinus1)) return false;

        Decimal fn;
        Decimal dfn;
        if (type.Equal(Decimal::One)) {
            fn = v.Mul(factor)
                     .Add(p.Mul(onePlusRate).Mul(factor.Sub(Decimal::One)).Div(rate))
                     .Add(f);
            dfn = v.Mul(n).Mul(powNMinus1)
                      .Add(p.Mul(onePlusRate)
                               .Mul(n.Mul(rate).Mul(powNMinus1).Sub(factor.Sub(Decimal::One)))
                               .Div(rate.Mul(rate)))
                      .Add(p.Mul(factor.Sub(Decimal::One)).Div(rate));
        } else {
            fn = v.Mul(factor).Add(p.Mul(factor.Sub(Decimal::One)).Div(rate)).Add(f);
            dfn = v.Mul(n).Mul(powNMinus1).Add(
                p.Mul(n.Mul(rate).Mul(powNMinus1).Sub(factor.Sub(Decimal::One))).Div(rate.Mul(rate)));
        }

        if (dfn.Abs().Less(EpsSlope())) break;
        const Decimal newRate = rate.Sub(fn.Div(dfn));

        if (newRate.Sub(rate).Abs().Less(EpsStep())) {
            result = newRate;
            return true;
        }
        rate = newRate;
    }
    return false;
}

/// 对应 C# XIRR.NewtonRaphsonXIRR
bool NewtonRaphsonXIRR(const std::vector<Decimal>& values, const std::vector<MyDate>& dates,
                       Decimal rate, Decimal& result) {
    const MyDate& baseDate = dates[0];
    std::vector<Decimal> exps;
    exps.reserve(values.size());
    for (size_t i = 0; i < values.size(); i++) {
        exps.push_back(TotalDays(baseDate, dates[i]).Div(DaysInYear()));
    }

    for (int iter = 0; iter < 100; iter++) {
        Decimal npv = Decimal::Zero;
        Decimal dnpv = Decimal::Zero;
        const Decimal onePlusRate = Decimal::One.Add(rate);
        Decimal logBase;
        if (!TryLogDecimal(onePlusRate, logBase)) return false;  // C# ArgumentException

        for (size_t i = 0; i < values.size(); i++) {
            // 对应 C# MathEx.Exp(exps[i] * logBase)
            const Decimal factor = MathEx::Exp(exps[i].Mul(logBase));
            if (factor.IsZero()) return false;  // C# DivideByZeroException
            npv = npv.Add(values[i].Div(factor));
            dnpv = dnpv.Sub(values[i].Mul(exps[i]).Div(factor.Mul(onePlusRate)));
        }

        if (dnpv.Abs().Less(EpsSlope())) break;
        const Decimal newRate = rate.Sub(npv.Div(dnpv));

        if (newRate.Sub(rate).Abs().Less(EpsStep())) {
            result = newRate;
            return true;
        }
        rate = newRate;
    }
    return false;
}

/// PPMT / IPMT 共用的 PMT 计算
bool TryCalculatePMT(const Decimal& rate, const Decimal& nper, const Decimal& pv, const Decimal& fv,
                     int type, Decimal& pmt) {
    pmt = Decimal::Zero;
    if (rate.IsZero()) {
        pmt = pv.Add(fv).Neg().Div(nper);
        return true;
    }
    Decimal factor;
    if (!TryPowPositiveBase(Decimal::One.Add(rate), nper, factor)) return false;
    pmt = pv.Mul(factor).Add(fv).Neg().Mul(rate).Div(factor.Sub(Decimal::One));
    if (type == 1) pmt = pmt.Div(Decimal::One.Add(rate));
    return true;
}

/// PPMT / IPMT 共用的 IPMT 计算
bool TryCalculateIPMT(const Decimal& rate, const Decimal& per, const Decimal& nper, const Decimal& pv,
                      const Decimal& fv, int type, Decimal& ipmt) {
    ipmt = Decimal::Zero;
    if (rate.IsZero()) return true;
    Decimal pmt;
    if (!TryCalculatePMT(rate, nper, pv, fv, type, pmt)) return false;
    Decimal factor;
    if (!TryPowPositiveBase(Decimal::One.Add(rate), per.Sub(Decimal::One), factor)) return false;
    ipmt = pv.Mul(factor).Add(pmt.Mul(factor.Sub(Decimal::One)).Div(rate)).Neg().Mul(rate);
    if (type == 1 && per.Equal(Decimal::One)) ipmt = Decimal::Zero;
    return true;
}

// ---------------------------------------------------------------------------
// 折旧函数
// ---------------------------------------------------------------------------

class Function_DB : public FunctionN {
public:
    explicit Function_DB(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "DB", 4, 5);
    }

    std::string Name() const override { return "DB"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto costArg = GetNumber(1, engine, tp);
        if (costArg.IsErrorOrNone()) return costArg;
        const Decimal cost = costArg.NumberValue();

        auto salvageArg = GetNumber(2, engine, tp);
        if (salvageArg.IsErrorOrNone()) return salvageArg;
        const Decimal salvage = salvageArg.NumberValue();

        auto lifeArg = GetNumber(3, engine, tp);
        if (lifeArg.IsErrorOrNone()) return lifeArg;
        const Decimal life = lifeArg.NumberValue();

        auto periodArg = GetNumber(4, engine, tp);
        if (periodArg.IsErrorOrNone()) return periodArg;
        const Decimal period = periodArg.NumberValue();

        int month = 12;
        if (ArgCount() > 4) {
            auto monthArg = GetNumber(5, engine, tp);
            if (monthArg.IsErrorOrNone()) return monthArg;
            // 先校验再转换, 避免大数值上溢出
            const Decimal monthValue = monthArg.NumberValue();
            if (monthValue.Less(Decimal::One) || monthValue.Greater(Decimal::FromInt(12))) {
                return ParameterError(5);
            }
            month = monthValue.Int();
        }

        if (life.IsZero() || cost.IsZero()) return Div0Error();

        if (life.Less(Decimal::One) ||
            life.Greater(Decimal::FromInt(std::numeric_limits<int>::max()))) {
            return ParameterError(3);
        }

        // Excel: month<12 时折旧跨越 life+1 个期间(第1年部分月 + life-1 个整年 + 最后部分月),
        // 最后一期乘 (12-month)/12 修正系数, 其余期间(含第 life 期)为完整年折旧
        const int lifeInt = life.Int();
        const int totalPeriods = (month == 12) ? lifeInt : lifeInt + 1;
        if (period.Less(Decimal::One) || period.Greater(Decimal::FromInt(totalPeriods))) {
            return ParameterError(4);
        }

        Decimal rate;
        if (salvage.IsZero()) {
            // Pow(0, 正指数) = 0, 即残值为 0 时折旧率为 1
            rate = Decimal::One;
        } else {
            Decimal factor;
            if (!TryPowPositiveBase(salvage.Div(cost), Decimal::One.Div(life), factor)) {
                return NumError();
            }
            rate = Decimal::One.Sub(factor);
        }
        rate = rate.RoundHalfEven(3);

        Decimal remainingCost = cost;
        Decimal depreciation = Decimal::Zero;
        for (int i = 1; Decimal::FromInt(i).LessOrEqual(period); i++) {
            if (i == 1) {
                depreciation =
                    cost.Mul(rate).Mul(Decimal::FromInt(month)).Div(Decimal::FromInt(12));
            } else if (i == totalPeriods && month != 12) {
                depreciation = remainingCost.Mul(rate).Mul(Decimal::FromInt(12 - month)).Div(
                    Decimal::FromInt(12));
            } else {
                depreciation = remainingCost.Mul(rate);
            }
            remainingCost = remainingCost.Sub(depreciation);
        }

        return Operand::Create(depreciation);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_DDB : public FunctionN {
public:
    explicit Function_DDB(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "DDB", 4, 5);
    }

    std::string Name() const override { return "DDB"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto costArg = GetNumber(1, engine, tp);
        if (costArg.IsErrorOrNone()) return costArg;
        const Decimal cost = costArg.NumberValue();

        auto salvageArg = GetNumber(2, engine, tp);
        if (salvageArg.IsErrorOrNone()) return salvageArg;
        const Decimal salvage = salvageArg.NumberValue();

        auto lifeArg = GetNumber(3, engine, tp);
        if (lifeArg.IsErrorOrNone()) return lifeArg;
        const Decimal life = lifeArg.NumberValue();

        auto periodArg = GetNumber(4, engine, tp);
        if (periodArg.IsErrorOrNone()) return periodArg;
        const Decimal period = periodArg.NumberValue();

        Decimal factor = Decimal::FromInt(2);
        if (ArgCount() > 4) {
            auto factorArg = GetNumber(5, engine, tp);
            if (factorArg.IsErrorOrNone()) return factorArg;
            factor = factorArg.NumberValue();
        }

        if (factor.LessOrEqual(Decimal::Zero)) return ParameterError(5);
        if (life.IsZero()) return Div0Error();
        if (life.IsNegative()) return ParameterError(3);
        if (period.Less(Decimal::One) || period.Greater(life)) return ParameterError(4);

        Decimal remainingCost = cost;

        // 累计 period 之前各期的折旧
        for (Decimal i = Decimal::One; i.Less(period); i = i.Add(Decimal::One)) {
            Decimal ddb = remainingCost.Mul(factor).Div(life);
            const Decimal maxDepreciation = remainingCost.Sub(salvage);
            if (ddb.Greater(maxDepreciation)) ddb = maxDepreciation;
            remainingCost = remainingCost.Sub(ddb);
            if (remainingCost.LessOrEqual(salvage)) break;
        }

        // 计算当前期间(period)的折旧, period 可为小数
        Decimal depreciation = remainingCost.Mul(factor).Div(life);
        const Decimal maxDep = remainingCost.Sub(salvage);
        if (depreciation.Greater(maxDep)) depreciation = maxDep;

        return Operand::Create(depreciation);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_SLN : public FunctionN {
public:
    explicit Function_SLN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SLN", 3, 3);
    }

    std::string Name() const override { return "SLN"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto costArg = GetNumber(1, engine, tp);
        if (costArg.IsErrorOrNone()) return costArg;
        const Decimal cost = costArg.NumberValue();

        auto salvageArg = GetNumber(2, engine, tp);
        if (salvageArg.IsErrorOrNone()) return salvageArg;
        const Decimal salvage = salvageArg.NumberValue();

        auto lifeArg = GetNumber(3, engine, tp);
        if (lifeArg.IsErrorOrNone()) return lifeArg;
        const Decimal life = lifeArg.NumberValue();

        if (life.IsZero()) return Div0Error();
        // 负的使用年限无实际意义
        if (life.IsNegative()) return ParameterError(3);

        return Operand::Create(cost.Sub(salvage).Div(life));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_SYD : public FunctionN {
public:
    explicit Function_SYD(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SYD", 4, 4);
    }

    std::string Name() const override { return "SYD"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto costArg = GetNumber(1, engine, tp);
        if (costArg.IsErrorOrNone()) return costArg;
        const Decimal cost = costArg.NumberValue();

        auto salvageArg = GetNumber(2, engine, tp);
        if (salvageArg.IsErrorOrNone()) return salvageArg;
        const Decimal salvage = salvageArg.NumberValue();

        auto lifeArg = GetNumber(3, engine, tp);
        if (lifeArg.IsErrorOrNone()) return lifeArg;
        const Decimal life = lifeArg.NumberValue();

        auto periodArg = GetNumber(4, engine, tp);
        if (periodArg.IsErrorOrNone()) return periodArg;
        const Decimal period = periodArg.NumberValue();

        if (life.IsZero()) return Div0Error();
        if (life.LessOrEqual(Decimal::Zero)) return ParameterError(3);
        if (period.Less(Decimal::One) || period.Greater(life)) return ParameterError(4);

        const Decimal syd = cost.Sub(salvage)
                                .Mul(life.Sub(period).Add(Decimal::One))
                                .Mul(Decimal::FromInt(2))
                                .Div(life.Mul(life.Add(Decimal::One)));
        return Operand::Create(syd);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------------------------------------------------------------------
// 时间价值函数
// ---------------------------------------------------------------------------

class Function_FV : public FunctionN {
public:
    explicit Function_FV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "FV", 3, 5);
    }

    std::string Name() const override { return "FV"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();

        auto nperArg = GetNumber(2, engine, tp);
        if (nperArg.IsErrorOrNone()) return nperArg;
        const Decimal nper = nperArg.NumberValue();

        auto pmtArg = GetNumber(3, engine, tp);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        const Decimal pmt = pmtArg.NumberValue();

        Decimal pv = Decimal::Zero;
        if (ArgCount() > 3) {
            auto pvArg = GetNumber(4, engine, tp);
            if (pvArg.IsErrorOrNone()) return pvArg;
            pv = pvArg.NumberValue();
        }

        int type = 0;
        if (ArgCount() > 4) {
            auto typeArg = GetNumber(5, engine, tp);
            if (typeArg.IsErrorOrNone()) return typeArg;
            // 先校验再转换, 避免截断小数或在大数值上溢出
            const Decimal typeValue = typeArg.NumberValue();
            if (!typeValue.IsZero() && !typeValue.Equal(Decimal::One)) return ParameterError(5);
            type = typeValue.Int();
        }

        if (rate.IsZero()) {
            return Operand::Create(pmt.Neg().Mul(nper).Sub(pv));
        }

        Decimal factor;
        if (!TryPowPositiveBase(Decimal::One.Add(rate), nper, factor)) return NumError();

        Decimal fv = pv.Neg().Mul(factor).Sub(pmt.Mul(factor.Sub(Decimal::One)).Div(rate));
        if (type == 1) {
            fv = pv.Neg().Mul(factor).Sub(
                pmt.Mul(Decimal::One.Add(rate)).Mul(factor.Sub(Decimal::One)).Div(rate));
        }

        return Operand::Create(fv);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_PV : public FunctionN {
public:
    explicit Function_PV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "PV", 3, 5);
    }

    std::string Name() const override { return "PV"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();

        auto nperArg = GetNumber(2, engine, tp);
        if (nperArg.IsErrorOrNone()) return nperArg;
        const Decimal nper = nperArg.NumberValue();

        auto pmtArg = GetNumber(3, engine, tp);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        const Decimal pmt = pmtArg.NumberValue();

        Decimal fv = Decimal::Zero;
        if (ArgCount() > 3) {
            auto fvArg = GetNumber(4, engine, tp);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (ArgCount() > 4) {
            auto typeArg = GetNumber(5, engine, tp);
            if (typeArg.IsErrorOrNone()) return typeArg;
            // 先校验再转换, 避免截断小数或在大数值上溢出
            const Decimal typeValue = typeArg.NumberValue();
            if (!typeValue.IsZero() && !typeValue.Equal(Decimal::One)) return ParameterError(5);
            type = typeValue.Int();
        }

        if (rate.IsZero()) {
            return Operand::Create(pmt.Neg().Mul(nper).Sub(fv));
        }

        Decimal factor;
        if (!TryPowPositiveBase(Decimal::One.Add(rate), nper, factor)) return NumError();

        Decimal pv = fv.Add(pmt.Mul(factor.Sub(Decimal::One)).Div(rate)).Neg().Div(factor);
        if (type == 1) {
            pv = fv.Add(pmt.Mul(Decimal::One.Add(rate)).Mul(factor.Sub(Decimal::One)).Div(rate))
                     .Neg()
                     .Div(factor);
        }

        return Operand::Create(pv);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_PMT : public FunctionN {
public:
    explicit Function_PMT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "PMT", 3, 5);
    }

    std::string Name() const override { return "PMT"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();

        auto nperArg = GetNumber(2, engine, tp);
        if (nperArg.IsErrorOrNone()) return nperArg;
        const Decimal nper = nperArg.NumberValue();

        auto pvArg = GetNumber(3, engine, tp);
        if (pvArg.IsErrorOrNone()) return pvArg;
        const Decimal pv = pvArg.NumberValue();

        if (nper.IsZero()) return Div0Error();

        Decimal fv = Decimal::Zero;
        if (ArgCount() > 3) {
            auto fvArg = GetNumber(4, engine, tp);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (ArgCount() > 4) {
            auto typeArg = GetNumber(5, engine, tp);
            if (typeArg.IsErrorOrNone()) return typeArg;
            // 先校验再转换, 避免截断小数或在大数值上溢出
            const Decimal typeValue = typeArg.NumberValue();
            if (!typeValue.IsZero() && !typeValue.Equal(Decimal::One)) return ParameterError(5);
            type = typeValue.Int();
        }

        if (rate.IsZero()) {
            return Operand::Create(pv.Add(fv).Neg().Div(nper));
        }

        Decimal factor;
        if (!TryPowPositiveBase(Decimal::One.Add(rate), nper, factor)) return NumError();

        Decimal pmt = pv.Mul(factor).Add(fv).Neg().Mul(rate).Div(factor.Sub(Decimal::One));
        if (type == 1) {
            pmt = pmt.Div(Decimal::One.Add(rate));
        }

        return Operand::Create(pmt);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_NPER : public FunctionN {
public:
    explicit Function_NPER(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NPER", 3, 5);
    }

    std::string Name() const override { return "NPER"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();

        auto pmtArg = GetNumber(2, engine, tp);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        const Decimal pmt = pmtArg.NumberValue();

        auto pvArg = GetNumber(3, engine, tp);
        if (pvArg.IsErrorOrNone()) return pvArg;
        const Decimal pv = pvArg.NumberValue();

        Decimal fv = Decimal::Zero;
        if (ArgCount() > 3) {
            auto fvArg = GetNumber(4, engine, tp);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (ArgCount() > 4) {
            auto typeArg = GetNumber(5, engine, tp);
            if (typeArg.IsErrorOrNone()) return typeArg;
            // 先校验再转换, 避免截断小数或在大数值上溢出
            const Decimal typeValue = typeArg.NumberValue();
            if (!typeValue.IsZero() && !typeValue.Equal(Decimal::One)) return ParameterError(5);
            type = typeValue.Int();
        }

        if (rate.IsZero()) {
            if (pmt.IsZero()) return Div0Error();
            return Operand::Create(pv.Add(fv).Neg().Div(pmt));
        }
        if (rate.Equal(Decimal::FromInt(-1))) return Div0Error();

        Decimal factor = pmt;
        if (type == 1) {
            factor = pmt.Mul(Decimal::One.Add(rate));
        }

        // Log 要求参数大于 0, 底数与真数非法时返回错误而非抛异常
        const Decimal logBase = Decimal::One.Add(rate);
        if (!logBase.IsPositive()) return NumError();
        const Decimal denominator = pv.Mul(rate).Add(factor);
        if (denominator.IsZero()) return Div0Error();
        const Decimal ratio = MathEx::Div(fv.Neg().Mul(rate).Add(factor), denominator);
        if (!ratio.IsPositive()) return NumError();

        Decimal logRatio;
        Decimal logBaseValue;
        if (!TryLogDecimal(ratio, logRatio) || !TryLogDecimal(logBase, logBaseValue)) return NumError();

        // C#: MathEx.Log(ratio) / MathEx.Log(logBase)
        return Operand::Create(MathEx::Div(logRatio, logBaseValue));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_IPMT : public FunctionN {
public:
    explicit Function_IPMT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IPMT", 4, 6);
    }

    std::string Name() const override { return "IPMT"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();

        auto perArg = GetNumber(2, engine, tp);
        if (perArg.IsErrorOrNone()) return perArg;
        const Decimal per = perArg.NumberValue();

        auto nperArg = GetNumber(3, engine, tp);
        if (nperArg.IsErrorOrNone()) return nperArg;
        const Decimal nper = nperArg.NumberValue();

        auto pvArg = GetNumber(4, engine, tp);
        if (pvArg.IsErrorOrNone()) return pvArg;
        const Decimal pv = pvArg.NumberValue();

        Decimal fv = Decimal::Zero;
        if (ArgCount() > 4) {
            auto fvArg = GetNumber(5, engine, tp);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (ArgCount() > 5) {
            auto typeArg = GetNumber(6, engine, tp);
            if (typeArg.IsErrorOrNone()) return typeArg;
            // 先校验再转换, 避免截断小数或在大数值上溢出
            const Decimal typeValue = typeArg.NumberValue();
            if (!typeValue.IsZero() && !typeValue.Equal(Decimal::One)) return ParameterError(6);
            type = typeValue.Int();
        }

        if (per.Less(Decimal::One) || per.Greater(nper)) return ParameterError(2);

        if (rate.IsZero()) {
            return Operand::Create(Decimal::Zero);
        }

        Decimal pmt;
        if (!TryCalculatePMT(rate, nper, pv, fv, type, pmt)) return NumError();
        Decimal factor;
        if (!TryPowPositiveBase(Decimal::One.Add(rate), per.Sub(Decimal::One), factor)) return NumError();

        Decimal ipmt = pv.Mul(factor).Add(pmt.Mul(factor.Sub(Decimal::One)).Div(rate)).Neg().Mul(rate);

        if (type == 1 && per.Equal(Decimal::One)) {
            ipmt = Decimal::Zero;
        }

        return Operand::Create(ipmt);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_PPMT : public FunctionN {
public:
    explicit Function_PPMT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "PPMT", 4, 6);
    }

    std::string Name() const override { return "PPMT"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();

        auto perArg = GetNumber(2, engine, tp);
        if (perArg.IsErrorOrNone()) return perArg;
        const Decimal per = perArg.NumberValue();

        auto nperArg = GetNumber(3, engine, tp);
        if (nperArg.IsErrorOrNone()) return nperArg;
        const Decimal nper = nperArg.NumberValue();

        auto pvArg = GetNumber(4, engine, tp);
        if (pvArg.IsErrorOrNone()) return pvArg;
        const Decimal pv = pvArg.NumberValue();

        Decimal fv = Decimal::Zero;
        if (ArgCount() > 4) {
            auto fvArg = GetNumber(5, engine, tp);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (ArgCount() > 5) {
            auto typeArg = GetNumber(6, engine, tp);
            if (typeArg.IsErrorOrNone()) return typeArg;
            // 先校验再转换, 避免截断小数或在大数值上溢出
            const Decimal typeValue = typeArg.NumberValue();
            if (!typeValue.IsZero() && !typeValue.Equal(Decimal::One)) return ParameterError(6);
            type = typeValue.Int();
        }

        if (per.Less(Decimal::One) || per.Greater(nper)) return ParameterError(2);

        Decimal pmtResult;
        if (!TryCalculatePMT(rate, nper, pv, fv, type, pmtResult)) return NumError();
        Decimal ipmtResult;
        if (!TryCalculateIPMT(rate, per, nper, pv, fv, type, ipmtResult)) return NumError();
        return Operand::Create(pmtResult.Sub(ipmtResult));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_RATE : public FunctionN {
public:
    explicit Function_RATE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RATE", 3, 6);
    }

    std::string Name() const override { return "RATE"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto nperArg = GetNumber(1, engine, tp);
        if (nperArg.IsErrorOrNone()) return nperArg;
        const Decimal nper = nperArg.NumberValue();

        auto pmtArg = GetNumber(2, engine, tp);
        if (pmtArg.IsErrorOrNone()) return pmtArg;
        const Decimal pmt = pmtArg.NumberValue();

        auto pvArg = GetNumber(3, engine, tp);
        if (pvArg.IsErrorOrNone()) return pvArg;
        const Decimal pv = pvArg.NumberValue();

        if (nper.LessOrEqual(Decimal::Zero)) return ParameterError(1);

        Decimal fv = Decimal::Zero;
        if (ArgCount() > 3) {
            auto fvArg = GetNumber(4, engine, tp);
            if (fvArg.IsErrorOrNone()) return fvArg;
            fv = fvArg.NumberValue();
        }

        int type = 0;
        if (ArgCount() > 4) {
            auto typeArg = GetNumber(5, engine, tp);
            if (typeArg.IsErrorOrNone()) return typeArg;
            // 先校验再转换, 避免截断小数或在大数值上溢出
            const Decimal typeValue = typeArg.NumberValue();
            if (!typeValue.IsZero() && !typeValue.Equal(Decimal::One)) return ParameterError(5);
            type = typeValue.Int();
        }

        Decimal guess = Decimal::FromString("0.1");
        if (ArgCount() > 5) {
            auto guessArg = GetNumber(6, engine, tp);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.NumberValue();
        }

        Decimal rate;
        // 牛顿迭代中 rate=0 除零、Pow 负底/溢出视为失败, 与 C# 捕获异常后返回 FunctionError 一致
        if (!NewtonRaphsonRate(nper, pmt, pv, fv, Decimal::FromInt(type), guess, rate)) {
            return FunctionError();
        }
        return Operand::Create(rate);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------------------------------------------------------------------
// 现金流函数
// ---------------------------------------------------------------------------

class Function_NPV : public FunctionN {
public:
    explicit Function_NPV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "NPV", 2, -1);
    }

    std::string Name() const override { return "NPV"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();
        if (rate.Equal(Decimal::FromInt(-1))) return Div0Error();

        std::vector<Decimal> values;
        values.reserve(ArgCount());
        for (size_t i = 2; i <= ArgCount(); i++) {
            auto arg = GetNumber(static_cast<int>(i), engine, tp);
            if (arg.IsErrorOrNone()) return arg;
            values.push_back(arg.NumberValue());
        }

        const Decimal base = Decimal::One.Add(rate);
        Decimal npv = Decimal::Zero;
        for (size_t i = 0; i < values.size(); i++) {
            Decimal factor;
            if (!PowDecimal(base, Decimal::FromInt(static_cast<int64_t>(i) + 1), factor)) {
                return NumError();
            }
            npv = npv.Add(values[i].Div(factor));
        }

        return Operand::Create(npv);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_IRR : public FunctionN {
public:
    explicit Function_IRR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IRR", 1, 2);
    }

    std::string Name() const override { return "IRR"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto valuesArg = GetArray(1, engine, tp);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        std::vector<Decimal> values;
        Operand error;
        if (!CollectNumbers("IRR", 1, valuesArg, values, error)) return error;

        if (values.empty()) return ParameterError(1);

        bool hasPositive = false;
        bool hasNegative = false;
        for (const auto& v : values) {
            if (v.IsPositive()) hasPositive = true;
            if (v.IsNegative()) hasNegative = true;
        }
        if (!hasPositive || !hasNegative) return ParameterError(1);

        Decimal guess = Decimal::FromString("0.1");
        if (ArgCount() > 1) {
            auto guessArg = GetNumber(2, engine, tp);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.NumberValue();
        }

        Decimal irr;
        if (!NewtonRaphsonIRR(values, guess, irr)) return FunctionError();
        return Operand::Create(irr);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_MIRR : public FunctionN {
public:
    explicit Function_MIRR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "MIRR", 3, 3);
    }

    std::string Name() const override { return "MIRR"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto valuesArg = GetArray(1, engine, tp);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        std::vector<Decimal> values;
        Operand error;
        if (!CollectNumbers("MIRR", 1, valuesArg, values, error)) return error;

        auto financeRateArg = GetNumber(2, engine, tp);
        if (financeRateArg.IsErrorOrNone()) return financeRateArg;
        const Decimal financeRate = financeRateArg.NumberValue();

        auto reinvestRateArg = GetNumber(3, engine, tp);
        if (reinvestRateArg.IsErrorOrNone()) return reinvestRateArg;
        const Decimal reinvestRate = reinvestRateArg.NumberValue();

        Decimal npvNegative = Decimal::Zero;
        Decimal npvPositive = Decimal::Zero;
        const int n = static_cast<int>(values.size());

        if (n == 0) return ParameterError(1);
        if (n == 1) return Div0Error();

        for (int i = 0; i < n; i++) {
            if (values[static_cast<size_t>(i)].IsNegative()) {
                // financeRate <= -1 时底数非正
                Decimal discountFactor;
                if (!TryPowPositiveBase(Decimal::One.Add(financeRate), Decimal::FromInt(i),
                                        discountFactor)) {
                    return NumError();
                }
                npvNegative = npvNegative.Add(values[static_cast<size_t>(i)].Div(discountFactor));
            } else {
                // reinvestRate <= -1 时底数非正
                Decimal compoundFactor;
                if (!TryPowPositiveBase(Decimal::One.Add(reinvestRate), Decimal::FromInt(n - 1 - i),
                                        compoundFactor)) {
                    return NumError();
                }
                npvPositive = npvPositive.Add(values[static_cast<size_t>(i)].Mul(compoundFactor));
            }
        }

        if (npvNegative.IsZero() || npvPositive.IsZero()) return Div0Error();

        Decimal ratio;
        if (!TryPowPositiveBase(npvPositive.Neg().Div(npvNegative),
                                Decimal::One.Div(Decimal::FromInt(n - 1)), ratio)) {
            return NumError();
        }
        return Operand::Create(ratio.Sub(Decimal::One));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_XIRR : public FunctionN {
public:
    explicit Function_XIRR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "XIRR", 2, 3);
    }

    std::string Name() const override { return "XIRR"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto valuesArg = GetArray(1, engine, tp);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        std::vector<Decimal> values;
        Operand error;
        if (!CollectNumbers("XIRR", 1, valuesArg, values, error)) return error;

        auto datesArg = GetArray(2, engine, tp);
        if (datesArg.IsErrorOrNone()) return datesArg;
        std::vector<MyDate> dates;
        if (!CollectDates(datesArg, dates)) return ParameterError(2);

        if (values.size() != dates.size() || values.size() < 2) return FunctionError();

        Decimal guess = Decimal::FromString("0.1");
        if (ArgCount() > 2) {
            auto guessArg = GetNumber(3, engine, tp);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.NumberValue();
        }

        Decimal xirr;
        if (!NewtonRaphsonXIRR(values, dates, guess, xirr)) return FunctionError();
        return Operand::Create(xirr);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_XNPV : public FunctionN {
public:
    explicit Function_XNPV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "XNPV", 3, 3);
    }

    std::string Name() const override { return "XNPV"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto rateArg = GetNumber(1, engine, tp);
        if (rateArg.IsErrorOrNone()) return rateArg;
        const Decimal rate = rateArg.NumberValue();
        if (rate.Equal(Decimal::FromInt(-1))) return Div0Error();

        auto valuesArg = GetArray(2, engine, tp);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        std::vector<Decimal> values;
        Operand error;
        if (!CollectNumbers("XNPV", 2, valuesArg, values, error)) return error;

        auto datesArg = GetArray(3, engine, tp);
        if (datesArg.IsErrorOrNone()) return datesArg;

        if (values.size() != datesArg.ArrayValue().size()) return FunctionError();
        if (values.empty()) return ParameterError(2);

        std::vector<MyDate> dateList;
        if (!CollectDates(datesArg, dateList)) return ParameterError(3);

        const MyDate& baseDate = dateList[0];
        Decimal xnpv = Decimal::Zero;

        for (size_t i = 0; i < values.size(); i++) {
            const Decimal days = TotalDays(baseDate, dateList[i]);
            // rate < -1 时底数为负
            Decimal factor;
            if (!TryPowPositiveBase(Decimal::One.Add(rate), days.Div(DaysInYear()), factor)) {
                return NumError();
            }
            xnpv = xnpv.Add(values[i].Div(factor));
        }

        return Operand::Create(xnpv);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

}  // namespace

Function NewFunction_DB(std::vector<Function> args) {
    return std::make_shared<Function_DB>(std::move(args));
}

Function NewFunction_DDB(std::vector<Function> args) {
    return std::make_shared<Function_DDB>(std::move(args));
}

Function NewFunction_FV(std::vector<Function> args) {
    return std::make_shared<Function_FV>(std::move(args));
}

Function NewFunction_IPMT(std::vector<Function> args) {
    return std::make_shared<Function_IPMT>(std::move(args));
}

Function NewFunction_IRR(std::vector<Function> args) {
    return std::make_shared<Function_IRR>(std::move(args));
}

Function NewFunction_MIRR(std::vector<Function> args) {
    return std::make_shared<Function_MIRR>(std::move(args));
}

Function NewFunction_NPER(std::vector<Function> args) {
    return std::make_shared<Function_NPER>(std::move(args));
}

Function NewFunction_NPV(std::vector<Function> args) {
    return std::make_shared<Function_NPV>(std::move(args));
}

Function NewFunction_PMT(std::vector<Function> args) {
    return std::make_shared<Function_PMT>(std::move(args));
}

Function NewFunction_PPMT(std::vector<Function> args) {
    return std::make_shared<Function_PPMT>(std::move(args));
}

Function NewFunction_PV(std::vector<Function> args) {
    return std::make_shared<Function_PV>(std::move(args));
}

Function NewFunction_RATE(std::vector<Function> args) {
    return std::make_shared<Function_RATE>(std::move(args));
}

Function NewFunction_SLN(std::vector<Function> args) {
    return std::make_shared<Function_SLN>(std::move(args));
}

Function NewFunction_SYD(std::vector<Function> args) {
    return std::make_shared<Function_SYD>(std::move(args));
}

Function NewFunction_XIRR(std::vector<Function> args) {
    return std::make_shared<Function_XIRR>(std::move(args));
}

Function NewFunction_XNPV(std::vector<Function> args) {
    return std::make_shared<Function_XNPV>(std::move(args));
}

}  // namespace toolgood
