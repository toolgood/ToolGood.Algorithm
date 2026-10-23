// MathSum.cpp - 对应 csharp/ToolGood.Algorithm/Internals/Functions/MathSum/（38 个统计/求和函数）
#include "MathSum.h"

#include <algorithm>
#include <cctype>
#include <cmath>
#include <initializer_list>
#include <map>
#include <stdexcept>
#include <string>
#include <utility>
#include <vector>

#include "../../../AlgorithmEngine.h"
#include "../../../MathEx.h"
#include "../../../MathNet/MathNet.h"
#include "../../../Operands/Operand.h"
#include "../../FunctionUtil.h"
#include "../../Registry.h"

namespace toolgood {

namespace {

using mn::DecimalMax;

// ---------------------------------------------------------------------------
// 通用辅助
// ---------------------------------------------------------------------------

/// C# decimal 超出 ±decimal.MaxValue 会抛 OverflowException；
/// C++ Decimal 为无限精度实现，需显式判定溢出。
bool IsOverflow(const Decimal& v) { return v.Abs().Greater(Decimal::Max); }

/// 去首尾空白（对应 C# string.Trim / ReadOnlySpan.Trim）
std::string TrimText(const std::string& s) {
    size_t begin = 0;
    size_t end = s.size();
    while (begin < end && std::isspace(static_cast<unsigned char>(s[begin])) != 0) begin++;
    while (end > begin && std::isspace(static_cast<unsigned char>(s[end - 1])) != 0) end--;
    return s.substr(begin, end - begin);
}

/// double 结果转操作数；NaN/Inf/超出 decimal 范围（C# 会抛异常的地方）转成函数错误。
Operand CheckedNumber(const FunctionBase& fn, double v) {
    if (!std::isfinite(v) || std::fabs(v) > DecimalMax) return fn.FunctionError();
    return Operand::Create(Decimal::FromDouble(v));
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

/// 对应 C# ArrayStatistics.Minimum/Maximum（仅用于非空列表）
std::vector<Decimal> SortedCopy(const std::vector<Decimal>& data) {
    std::vector<Decimal> ordered = data;
    std::sort(ordered.begin(), ordered.end(),
              [](const Decimal& a, const Decimal& b) { return a.Less(b); });
    return ordered;
}

/// 对应 C# ArrayStatistics.SelectInplace：已排序时等价于取下标，越界取两端
Decimal SelectAt(const std::vector<Decimal>& ordered, int rank) {
    if (rank <= 0) return ordered.front();
    if (rank >= static_cast<int>(ordered.size()) - 1) return ordered.back();
    return ordered[static_cast<size_t>(rank)];
}

/// 对应 C# ArrayStatistics.QuantileCustomInplace（Excel PERCENTILE 的 R-7 定义）
Decimal QuantileCustom(const std::vector<Decimal>& data, const Decimal& tau) {
    if (tau.Less(Decimal::Zero) || tau.Greater(Decimal::One) || data.empty()) {
        return Decimal::MinusMax;
    }
    std::vector<Decimal> ordered = SortedCopy(data);
    const int n = static_cast<int>(ordered.size());
    if (tau.IsZero() || n == 1) return ordered[0];
    if (tau.Equal(Decimal::One)) return ordered[static_cast<size_t>(n - 1)];

    Decimal h = Decimal::FromInt(static_cast<int64_t>(n) - 1).Mul(tau).Add(Decimal::One);
    const int hf = h.Int();
    Decimal lower = SelectAt(ordered, hf - 1);
    Decimal upper = SelectAt(ordered, hf);
    return lower.Add(h.Sub(Decimal::FromInt(hf)).Mul(upper.Sub(lower)));
}

/// 对应 C# SortedArrayStatistics.QuantileRank（升序数组）
Decimal QuantileRank(const std::vector<Decimal>& data, const Decimal& x) {
    std::vector<Decimal> ordered = SortedCopy(data);
    const int n = static_cast<int>(ordered.size());
    if (x.Less(ordered[0])) return Decimal::Zero;
    if (x.GreaterOrEqual(ordered[static_cast<size_t>(n - 1)])) return Decimal::One;

    int right = 0;
    while (right < n && ordered[static_cast<size_t>(right)].Less(x)) right++;
    if (ordered[static_cast<size_t>(right)].Equal(x)) {
        int left = right;
        while (left > 0 && ordered[static_cast<size_t>(left) - 1].Equal(ordered[static_cast<size_t>(left)])) {
            left--;
        }
        while (right < n - 1 &&
               ordered[static_cast<size_t>(right) + 1].Equal(ordered[static_cast<size_t>(right)])) {
            right++;
        }
        return Decimal::FromInt(left).Div(Decimal::FromInt(static_cast<int64_t>(n) - 1));
    }
    int left = right - 1;
    Decimal a = Decimal::FromInt(left).Div(Decimal::FromInt(static_cast<int64_t>(n) - 1));
    Decimal b = Decimal::FromInt(right).Div(Decimal::FromInt(static_cast<int64_t>(n) - 1));
    return ordered[static_cast<size_t>(right)]
        .Sub(x)
        .Mul(a)
        .Add(x.Sub(ordered[static_cast<size_t>(left)]).Mul(b))
        .Div(ordered[static_cast<size_t>(right)].Sub(ordered[static_cast<size_t>(left)]));
}

/// 条件参数解析（对应 COUNTIF / SUMIF / AVERAGEIF 的三分支判断）。
/// op 为空表示"等于 value"的比较。
bool ParseCriteria(const Operand& arg, std::string& op, Decimal& value) {
    if (arg.IsNumber()) {
        op.clear();
        value = arg.NumberValue();
        return true;
    }
    if (!arg.IsText()) return false;
    std::string text = TrimText(arg.TextValue());
    if (Decimal::TryParse(text, value)) {
        op.clear();
        return true;
    }
    return ParseSumIfMatch(text, op, value);
}

/// 对应 C# 各函数 GetParameterTypes 覆写：按参数顺序给出期望类型
void ParamTypes(const FunctionN& fn, NoneEngine& noneEngine, std::vector<ParameterType>& result,
                std::initializer_list<OperandType> types) {
    size_t i = 0;
    for (OperandType t : types) {
        fn.Arg(i)->GetParameterTypes(noneEngine, result, t);
        i++;
    }
}

/// 对应 C# `funcs.Length == 1 ? ARRAY : 全部 NUMBER` 的参数类型传播
void SingleOrMultiTypes(const FunctionN& fn, NoneEngine& noneEngine,
                        std::vector<ParameterType>& result, OperandType single, OperandType multi) {
    if (fn.ArgCount() == 1) {
        fn.Arg(0)->GetParameterTypes(noneEngine, result, single);
        return;
    }
    for (size_t i = 0; i < fn.ArgCount(); i++) {
        fn.Arg(i)->GetParameterTypes(noneEngine, result, multi);
    }
}

/// MAX / MIN 两参数时的特殊类型推导（照抄 C# 同名逻辑）
void ExtremumParameterTypes(const FunctionN& fn, NoneEngine& noneEngine,
                            std::vector<ParameterType>& result) {
    const size_t count = fn.ArgCount();
    if (count == 1) {
        fn.Arg(0)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        return;
    }
    if (count == 2) {
        OperandType t1 = fn.Arg(0)->GetResultType();
        OperandType t2 = fn.Arg(1)->GetResultType();
        if (t1 == OperandType::NONE && t2 == OperandType::NUMBER) {
            Operand p = fn.Arg(1)->Evaluate(noneEngine, nullptr).ToText();
            if (!p.IsErrorOrNone()) {
                fn.Arg(0)->GetParameterTypes(noneEngine, result, t2, fn.Name(), p.TextValue());
                fn.Arg(1)->GetParameterTypes(noneEngine, result, t2);
                return;
            }
        } else if (t1 == OperandType::NUMBER && t2 == OperandType::NONE) {
            Operand p = fn.Arg(0)->Evaluate(noneEngine, nullptr).ToText();
            if (!p.IsErrorOrNone()) {
                fn.Arg(1)->GetParameterTypes(noneEngine, result, t1, fn.Name(), p.TextValue());
                fn.Arg(0)->GetParameterTypes(noneEngine, result, t1);
                return;
            }
        }
        fn.Arg(0)->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        fn.Arg(1)->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        return;
    }
    for (size_t i = 0; i < count; i++) {
        fn.Arg(i)->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
    }
}

// ---------------------------------------------------------------------------
// 统计公式辅助（C# 中多处重复出现的算法）
// ---------------------------------------------------------------------------

/// CORREL / PEARSON 的公共实现（C# 两个类的 Evaluate 完全相同）
Operand Correlation(const FunctionBase& fn, const Operand& array1Arg, const Operand& array2Arg) {
    std::vector<Decimal> xValues;
    for (const auto& item : array1Arg.ArrayValue()) {
        if (!item.IsNumber()) return fn.ParameterError(1);
        xValues.push_back(item.NumberValue());
    }
    std::vector<Decimal> yValues;
    for (const auto& item : array2Arg.ArrayValue()) {
        if (!item.IsNumber()) return fn.ParameterError(2);
        yValues.push_back(item.NumberValue());
    }

    if (xValues.size() != yValues.size() || xValues.size() < 2) return fn.FunctionError();

    const int n = static_cast<int>(xValues.size());
    Decimal sumX;
    Decimal sumY;
    for (int i = 0; i < n; i++) {
        sumX = sumX.Add(xValues[static_cast<size_t>(i)]);
        sumY = sumY.Add(yValues[static_cast<size_t>(i)]);
    }
    Decimal meanX = sumX.Div(Decimal::FromInt(n));
    Decimal meanY = sumY.Div(Decimal::FromInt(n));

    Decimal numerator;
    Decimal denomX;
    Decimal denomY;
    for (int i = 0; i < n; i++) {
        Decimal dx = xValues[static_cast<size_t>(i)].Sub(meanX);
        Decimal dy = yValues[static_cast<size_t>(i)].Sub(meanY);
        numerator = numerator.Add(dx.Mul(dy));
        denomX = denomX.Add(dx.Mul(dx));
        denomY = denomY.Add(dy.Mul(dy));
    }
    if (denomX.IsZero() || denomY.IsZero()) return fn.Div0Error();

    // 分开开方，避免 denomX * denomY 溢出
    return Operand::Create(numerator.Div(denomX.Sqrt().Mul(denomY.Sqrt())));
}

/// SLOPE / INTERCEPT / FORECAST 共用的中心化两遍最小二乘。
/// 返回 false 时 error 为错误操作数。
bool FitLine(const FunctionBase& fn, const Operand& yArrayArg, const Operand& xArrayArg,
             Decimal& slope, Decimal& intercept, Operand& error) {
    std::vector<Decimal> yValues;
    for (const auto& item : yArrayArg.ArrayValue()) {
        if (item.IsNumber()) yValues.push_back(item.NumberValue());
    }
    std::vector<Decimal> xValues;
    for (const auto& item : xArrayArg.ArrayValue()) {
        if (item.IsNumber()) xValues.push_back(item.NumberValue());
    }

    if (yValues.size() != xValues.size() || yValues.size() < 2) {
        error = fn.FunctionError();
        return false;
    }

    const int n = static_cast<int>(yValues.size());
    Decimal sumX;
    Decimal sumY;
    for (int i = 0; i < n; i++) {
        sumX = sumX.Add(xValues[static_cast<size_t>(i)]);
        sumY = sumY.Add(yValues[static_cast<size_t>(i)]);
    }
    Decimal meanX = sumX.Div(Decimal::FromInt(n));
    Decimal meanY = sumY.Div(Decimal::FromInt(n));

    // 采用中心化两遍法，避免 n*sumX2 - sumX*sumX 的病态消减与溢出
    Decimal numerator;
    Decimal denominator;
    for (int i = 0; i < n; i++) {
        Decimal dx = xValues[static_cast<size_t>(i)].Sub(meanX);
        numerator = numerator.Add(dx.Mul(yValues[static_cast<size_t>(i)].Sub(meanY)));
        denominator = denominator.Add(dx.Mul(dx));
    }
    if (denominator.IsZero()) {
        error = fn.Div0Error();
        return false;
    }
    slope = numerator.Div(denominator);
    intercept = meanY.Sub(slope.Mul(meanX));
    return true;
}

/// STDEV / VAR 共用的 Welford 累加（数组先展开，非数值项尝试转数值）。
/// 返回 false 时 error 为错误操作数。
bool WelfordM2(const FunctionBase& fn, const std::vector<Operand>& args, Decimal& m2, int& count,
               Operand& error) {
    Decimal mean;
    m2 = Decimal::Zero;
    count = 0;
    auto add = [&](const Decimal& v) {
        count++;
        Decimal delta = v.Sub(mean);
        mean = mean.Add(delta.Div(Decimal::FromInt(count)));
        m2 = m2.Add(delta.Mul(v.Sub(mean)));
    };

    for (const auto& item : args) {
        if (item.IsArray()) {
            std::vector<Operand> array = item.ArrayValue();
            for (const auto& elem : array) {
                if (elem.IsNumber()) {
                    add(elem.NumberValue());
                } else if (elem.IsArray() || elem.IsJson()) {
                    std::vector<Decimal> list;
                    if (!FlattenToNumberList(elem, list)) {
                        error = fn.FunctionError();
                        return false;
                    }
                    for (const auto& v : list) add(v);
                } else {
                    Operand converted = elem.ToNumber();
                    if (converted.IsError()) {
                        error = fn.FunctionError();
                        return false;
                    }
                    add(converted.NumberValue());
                }
            }
        } else if (item.IsNumber()) {
            add(item.NumberValue());
        } else if (item.IsJson()) {
            std::vector<Decimal> list;
            if (!FlattenToNumberList(item, list)) {
                error = fn.FunctionError();
                return false;
            }
            for (const auto& v : list) add(v);
        } else {
            Operand converted = item.ToNumber();
            if (converted.IsError()) {
                error = fn.FunctionError();
                return false;
            }
            add(converted.NumberValue());
        }
    }
    return true;
}

/// MAX / MIN 共用实现（C# 两个类体仅比较方向不同）。
/// 返回 false 表示未找到任何可比较的数值。
bool ExtremumCore(const FunctionBase& fn, const std::vector<Operand>& args, bool isMax,
                  Decimal& out) {
    bool found = false;
    Decimal best = isMax ? Decimal::MinusMax : Decimal::Max;
    auto add = [&](const Decimal& v) {
        if (!found || (isMax ? v.Greater(best) : v.Less(best))) {
            best = v;
            found = true;
        }
    };

    for (const auto& item : args) {
        if (item.IsArray()) {
            std::vector<Operand> array = item.ArrayValue();
            for (const auto& elem : array) {
                if (elem.IsNumber()) {
                    add(elem.NumberValue());
                } else if (elem.IsArray() || elem.IsJson()) {
                    std::vector<Decimal> list;
                    if (!FlattenToNumberList(elem, list)) return false;
                    for (const auto& v : list) add(v);
                } else {
                    Operand converted = elem.ToNumber();
                    if (converted.IsError()) return false;
                    add(converted.NumberValue());
                }
            }
        } else if (item.IsNumber()) {
            add(item.NumberValue());
        } else if (item.IsJson()) {
            std::vector<Decimal> list;
            if (!FlattenToNumberList(item, list)) return false;
            for (const auto& v : list) add(v);
        } else {
            Operand converted = item.ToNumber();
            if (converted.IsError()) return false;
            add(converted.NumberValue());
        }
    }
    out = best;
    return found;
}

// ---------------------------------------------------------------------------
// AVEDEV
// ---------------------------------------------------------------------------
class Function_AVEDEV : public FunctionN {
public:
    explicit Function_AVEDEV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "AveDev", 1, -1);
    }

    std::string Name() const override { return "AveDev"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();
        // Excel 对空数据集返回 #NUM!
        if (list.empty()) return NumError();

        const Decimal count = Decimal::FromInt(static_cast<int64_t>(list.size()));
        Decimal sum;
        for (const auto& v : list) sum = sum.Add(v);
        Decimal avg = sum.Div(count);
        Decimal sum2;
        for (const auto& v : list) sum2 = sum2.Add(v.Sub(avg).Abs());
        return Operand::Create(sum2.Div(count));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// AVERAGE
// ---------------------------------------------------------------------------
class Function_AVERAGE : public FunctionN {
public:
    explicit Function_AVERAGE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Average", 1, -1);
    }

    std::string Name() const override { return "Average"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        Decimal sum;
        int count = 0;

        for (const auto& item : args) {
            if (item.IsArray()) {
                std::vector<Operand> array = item.ArrayValue();
                for (const auto& elem : array) {
                    if (elem.IsNumber()) {
                        sum = sum.Add(elem.NumberValue());
                        count++;
                    } else if (elem.IsArray() || elem.IsJson()) {
                        std::vector<Decimal> list;
                        if (FlattenToNumberList(elem, list)) {
                            for (const auto& v : list) sum = sum.Add(v);
                            count += static_cast<int>(list.size());
                        } else {
                            return FunctionError();
                        }
                    } else {
                        Operand converted = elem.ToNumber();
                        if (converted.IsError()) return FunctionError();
                        sum = sum.Add(converted.NumberValue());
                        count++;
                    }
                }
            } else if (item.IsNumber()) {
                sum = sum.Add(item.NumberValue());
                count++;
            } else if (item.IsJson()) {
                std::vector<Decimal> list;
                if (!FlattenToNumberList(item, list)) return FunctionError();
                for (const auto& v : list) sum = sum.Add(v);
                count += static_cast<int>(list.size());
            } else {
                Operand converted = item.ToNumber();
                if (converted.IsError()) return FunctionError();
                sum = sum.Add(converted.NumberValue());
                count++;
            }
        }

        if (count == 0) return Div0Error();
        return Operand::Create(sum.Div(Decimal::FromInt(count)));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// AVERAGEIF
// ---------------------------------------------------------------------------
class Function_AVERAGEIF : public FunctionN {
public:
    explicit Function_AVERAGEIF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "AverageIf", 2, 3);
    }

    std::string Name() const override { return "AverageIf"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = GetArray(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = EvalArg(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);

        std::vector<Decimal> sumdbs;
        if (ArgCount() > 2) {
            Operand args3 = GetArray(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            if (!FlattenToNumberList(args3, sumdbs)) return ParameterError(3);
        } else {
            sumdbs = list;
        }

        std::string op;
        Decimal value;
        if (!ParseCriteria(args2, op, value)) return ParameterError(2);

        int count = op.empty() ? GetCountIf(list, value) : GetCountIf(list, op, value);
        Decimal sum = op.empty() ? GetSumIf(list, value, sumdbs) : GetSumIf(list, op, value, sumdbs);

        if (count == 0) return Div0Error();
        return Operand::Create(sum.Div(Decimal::FromInt(count)));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        Arg(0)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        Arg(1)->GetParameterTypes(noneEngine, result, OperandType::NONE);
        if (ArgCount() > 2) {
            Arg(2)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        }
    }
};

// ---------------------------------------------------------------------------
// CORREL
// ---------------------------------------------------------------------------
class Function_CORREL : public FunctionN {
public:
    explicit Function_CORREL(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "CORREL", 2, 2);
    }

    std::string Name() const override { return "CORREL"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand array1Arg = GetArray(1, engine, tp);
        if (array1Arg.IsErrorOrNone()) return array1Arg;
        Operand array2Arg = GetArray(2, engine, tp);
        if (array2Arg.IsErrorOrNone()) return array2Arg;
        return Correlation(*this, array1Arg, array2Arg);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// COUNT
// ---------------------------------------------------------------------------
class Function_COUNT : public FunctionN {
public:
    explicit Function_COUNT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Count", 1, -1);
    }

    std::string Name() const override { return "Count"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Operand> list;
        if (!FlattenToList(args, list)) return FunctionError();

        // 与 Excel 一致：只统计可转为数值的项，文本、空值等一律忽略
        int count = 0;
        for (const auto& item : list) {
            if (!item.ToNumber().IsErrorOrNone()) count++;
        }
        return Operand::Create(count);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NONE);
    }
};

// ---------------------------------------------------------------------------
// COUNTIF
// ---------------------------------------------------------------------------
class Function_COUNTIF : public FunctionN {
public:
    explicit Function_COUNTIF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "CountIf", 2, 2);
    }

    std::string Name() const override { return "CountIf"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = GetArray(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = EvalArg(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);

        std::string op;
        Decimal value;
        if (!ParseCriteria(args2, op, value)) return ParameterError(2);

        int count = op.empty() ? GetCountIf(list, value) : GetCountIf(list, op, value);
        return Operand::Create(count);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::NONE});
    }
};

// ---------------------------------------------------------------------------
// COVAR
// ---------------------------------------------------------------------------
class Function_COVAR : public FunctionN {
public:
    explicit Function_COVAR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Covar", 2, 2);
    }

    std::string Name() const override { return "Covar"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = EvalArg(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = EvalArg(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list1;
        std::vector<Decimal> list2;
        bool o1 = FlattenToNumberList(args1, list1);
        bool o2 = FlattenToNumberList(args2, list2);
        if (!o1) return ParameterError(1);
        if (!o2) return ParameterError(2);
        if (list1.size() != list2.size()) return FunctionError();
        if (list1.empty()) return FunctionError();

        Decimal mean1;
        Decimal mean2;
        Decimal c;
        for (size_t i = 0; i < list1.size(); i++) {
            Decimal delta1 = list1[i].Sub(mean1);
            Decimal delta2 = list2[i].Sub(mean2);
            mean1 = mean1.Add(delta1.Div(Decimal::FromInt(static_cast<int64_t>(i) + 1)));
            mean2 = mean2.Add(delta2.Div(Decimal::FromInt(static_cast<int64_t>(i) + 1)));
            c = c.Add(delta1.Mul(list2[i].Sub(mean2)));
        }
        return Operand::Create(c.Div(Decimal::FromInt(static_cast<int64_t>(list1.size()))));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// COVARIANCES
// ---------------------------------------------------------------------------
class Function_COVARIANCES : public FunctionN {
public:
    explicit Function_COVARIANCES(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Covariances", 2, 2);
    }

    std::string Name() const override { return "Covariances"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = EvalArg(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = EvalArg(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list1;
        std::vector<Decimal> list2;
        bool o1 = FlattenToNumberList(args1, list1);
        bool o2 = FlattenToNumberList(args2, list2);
        if (!o1) return ParameterError(1);
        if (!o2) return ParameterError(2);
        if (list1.size() != list2.size()) return FunctionError();
        if (list1.size() < 2) return FunctionError();

        Decimal mean1;
        Decimal mean2;
        Decimal c;
        for (size_t i = 0; i < list1.size(); i++) {
            Decimal delta1 = list1[i].Sub(mean1);
            Decimal delta2 = list2[i].Sub(mean2);
            mean1 = mean1.Add(delta1.Div(Decimal::FromInt(static_cast<int64_t>(i) + 1)));
            mean2 = mean2.Add(delta2.Div(Decimal::FromInt(static_cast<int64_t>(i) + 1)));
            c = c.Add(delta1.Mul(list2[i].Sub(mean2)));
        }
        return Operand::Create(c.Div(Decimal::FromInt(static_cast<int64_t>(list1.size()) - 1)));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// DEVSQ
// ---------------------------------------------------------------------------
class Function_DEVSQ : public FunctionN {
public:
    explicit Function_DEVSQ(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "DevSq", 1, -1);
    }

    std::string Name() const override { return "DevSq"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();
        if (list.empty()) return FunctionError();

        Decimal mean;
        Decimal m2;
        for (size_t i = 0; i < list.size(); i++) {
            Decimal delta = list[i].Sub(mean);
            mean = mean.Add(delta.Div(Decimal::FromInt(static_cast<int64_t>(i) + 1)));
            m2 = m2.Add(delta.Mul(list[i].Sub(mean)));
        }
        return Operand::Create(m2);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// FORECAST
// ---------------------------------------------------------------------------
class Function_FORECAST : public FunctionN {
public:
    explicit Function_FORECAST(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "FORECAST", 3, 3);
    }

    std::string Name() const override { return "FORECAST"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand xArg = GetNumber(1, engine, tp);
        if (xArg.IsErrorOrNone()) return xArg;
        Decimal x = xArg.NumberValue();

        Operand yArrayArg = GetArray(2, engine, tp);
        if (yArrayArg.IsErrorOrNone()) return yArrayArg;
        Operand xArrayArg = GetArray(3, engine, tp);
        if (xArrayArg.IsErrorOrNone()) return xArrayArg;

        Decimal slope;
        Decimal intercept;
        Operand error;
        if (!FitLine(*this, yArrayArg, xArrayArg, slope, intercept, error)) return error;

        return Operand::Create(intercept.Add(slope.Mul(x)));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// GEOMEAN
// ---------------------------------------------------------------------------
class Function_GEOMEAN : public FunctionN {
public:
    explicit Function_GEOMEAN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "GeoMean", 1, -1);
    }

    std::string Name() const override { return "GeoMean"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return ParameterError(1);
        if (list.empty()) return ParameterError(1);

        Decimal product = Decimal::One;
        for (const auto& v : list) {
            if (v.LessOrEqual(Decimal::Zero)) return ParameterError(1);
            // 连乘可能溢出 decimal 范围，捕获并返回错误
            product = product.Mul(v);
            if (IsOverflow(product)) return FunctionError();
        }
        Decimal geoMean;
        if (!PowDecimal(product, Decimal::One.Div(Decimal::FromInt(static_cast<int64_t>(list.size()))),
                        geoMean)) {
            return FunctionError();
        }
        return Operand::Create(geoMean);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// HARMEAN
// ---------------------------------------------------------------------------
class Function_HARMEAN : public FunctionN {
public:
    explicit Function_HARMEAN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "HarMean", 1, -1);
    }

    std::string Name() const override { return "HarMean"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();
        if (list.empty()) return FunctionError();

        Decimal sum;
        for (const auto& v : list) {
            // 调和平均数要求所有数据为正数
            if (v.LessOrEqual(Decimal::Zero)) return FunctionError();
            sum = sum.Add(Decimal::One.Div(v));
        }
        if (sum.IsZero()) return FunctionError();
        return Operand::Create(Decimal::FromInt(static_cast<int64_t>(list.size())).Div(sum));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// INTERCEPT
// ---------------------------------------------------------------------------
class Function_INTERCEPT : public FunctionN {
public:
    explicit Function_INTERCEPT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "INTERCEPT", 2, 2);
    }

    std::string Name() const override { return "INTERCEPT"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand yArrayArg = GetArray(1, engine, tp);
        if (yArrayArg.IsErrorOrNone()) return yArrayArg;
        Operand xArrayArg = GetArray(2, engine, tp);
        if (xArrayArg.IsErrorOrNone()) return xArrayArg;

        Decimal slope;
        Decimal intercept;
        Operand error;
        if (!FitLine(*this, yArrayArg, xArrayArg, slope, intercept, error)) return error;
        return Operand::Create(intercept);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// LARGE
// ---------------------------------------------------------------------------
class Function_LARGE : public FunctionN {
public:
    explicit Function_LARGE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Large", 2, 2);
    }

    std::string Name() const override { return "Large"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = EvalArg(1, engine, tp);
        args1 = ConvertToArray(args1, 1);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = EvalArg(2, engine, tp);
        args2 = ConvertToNumber(args2, 2);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);
        if (list.empty()) return ParameterError(1);

        int k = args2.IntValue() - engine.ExcelIndex;
        if (k < 0 || k >= static_cast<int>(list.size())) return ParameterError(2);
        return Operand::Create(QuickSelect(list, k, true));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// MAX
// ---------------------------------------------------------------------------
class Function_MAX : public FunctionN {
public:
    explicit Function_MAX(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Max", 1, -1);
    }

    std::string Name() const override { return "Max"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        Decimal max;
        if (!ExtremumCore(*this, args, true, max)) return FunctionError();
        return Operand::Create(max);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ExtremumParameterTypes(*this, noneEngine, result);
    }
};

// ---------------------------------------------------------------------------
// MEDIAN
// ---------------------------------------------------------------------------
class Function_MEDIAN : public FunctionN {
public:
    explicit Function_MEDIAN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Median", 1, -1);
    }

    std::string Name() const override { return "Median"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();
        if (list.empty()) return FunctionError();

        const int n = static_cast<int>(list.size());
        if (n == 1) return Operand::Create(list[0]);

        const int mid = n / 2;
        if (n % 2 == 0) {
            Decimal lower = QuickSelect(list, mid - 1, false);
            Decimal upper = QuickSelect(list, mid, false);
            return Operand::Create(lower.Add(upper).Div(Decimal::FromInt(2)));
        }
        return Operand::Create(QuickSelect(list, mid, false));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// MIN
// ---------------------------------------------------------------------------
class Function_MIN : public FunctionN {
public:
    explicit Function_MIN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Min", 1, -1);
    }

    std::string Name() const override { return "Min"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        Decimal min;
        if (!ExtremumCore(*this, args, false, min)) return FunctionError();
        return Operand::Create(min);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ExtremumParameterTypes(*this, noneEngine, result);
    }
};

// ---------------------------------------------------------------------------
// MODE
// ---------------------------------------------------------------------------
class Function_MODE : public FunctionN {
public:
    explicit Function_MODE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Mode", 1, -1);
    }

    std::string Name() const override { return "Mode"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();
        if (list.empty()) return FunctionError();

        // 用去尾零文本作键；order 保留首次出现顺序，使并列时取最靠前的值
        std::map<std::string, int> counts;
        std::vector<std::pair<std::string, Decimal>> order;
        for (const auto& v : list) {
            std::string key = v.PlainStrip();
            auto it = counts.find(key);
            if (it == counts.end()) {
                counts[key] = 1;
                order.push_back(std::make_pair(key, v));
            } else {
                it->second++;
            }
        }
        Decimal modeKey;
        int maxCount = 1;
        for (const auto& kv : order) {
            int c = counts[kv.first];
            if (c > maxCount) {
                maxCount = c;
                modeKey = kv.second;
            }
        }
        if (maxCount == 1) return FunctionError();
        return Operand::Create(modeKey);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// PEARSON
// ---------------------------------------------------------------------------
class Function_PEARSON : public FunctionN {
public:
    explicit Function_PEARSON(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "PEARSON", 2, 2);
    }

    std::string Name() const override { return "PEARSON"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand array1Arg = GetArray(1, engine, tp);
        if (array1Arg.IsErrorOrNone()) return array1Arg;
        Operand array2Arg = GetArray(2, engine, tp);
        if (array2Arg.IsErrorOrNone()) return array2Arg;
        return Correlation(*this, array1Arg, array2Arg);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// PERCENTILE
// ---------------------------------------------------------------------------
class Function_PERCENTILE : public FunctionN {
public:
    explicit Function_PERCENTILE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Percentile", 2, 2);
    }

    std::string Name() const override { return "Percentile"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = GetArray(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);
        if (list.empty()) return ParameterError(1);

        Decimal k = args2.NumberValue();
        if (k.Less(Decimal::Zero) || k.Greater(Decimal::One)) return ParameterError(2);
        return Operand::Create(QuantileCustom(list, k));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// PERCENTRANK
// ---------------------------------------------------------------------------
class Function_PERCENTRANK : public FunctionN {
public:
    explicit Function_PERCENTRANK(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "PercentRank", 2, 3);
    }

    std::string Name() const override { return "PercentRank"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = GetArray(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);
        if (list.empty()) return ParameterError(1);

        Decimal v = QuantileRank(list, args2.NumberValue());
        int d = 3;
        if (ArgCount() > 2) {
            Operand args3 = GetNumber(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            d = args3.IntValue();
            // Math.Round(decimal,int) 最多支持 28 位小数，超出直接报参数错误
            if (d < 0 || d > 28) return ParameterError(3);
        }
        return Operand::Create(v.RoundHalfUp(d));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        Arg(0)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        Arg(1)->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        if (ArgCount() > 2) {
            Arg(2)->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }
};

// ---------------------------------------------------------------------------
// POISSON
// ---------------------------------------------------------------------------
class Function_POISSON : public FunctionN {
public:
    explicit Function_POISSON(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Poisson", 3, 3);
    }

    std::string Name() const override { return "Poisson"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Operand args3 = GetBoolean(3, engine, tp);
        if (args3.IsErrorOrNone()) return args3;

        int k = args1.IntValue();
        if (k < 0) return ParameterError(1);
        Decimal lambda = args2.NumberValue();
        if (!lambda.Greater(Decimal::Zero)) return ParameterError(2);
        bool state = args3.BooleanValue();

        double lam = lambda.Float64();
        double v = state ? mn::dist::Poisson::CDF(lam, static_cast<double>(k))
                         : mn::dist::Poisson::PMF(lam, k);
        return CheckedNumber(*this, v);
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
// QUARTILE
// ---------------------------------------------------------------------------
class Function_QUARTILE : public FunctionN {
public:
    explicit Function_QUARTILE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Quartile", 2, 2);
    }

    std::string Name() const override { return "Quartile"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = GetArray(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);
        if (list.empty()) return ParameterError(1);

        int quant = args2.IntValue();
        if (quant < 0 || quant > 4) return ParameterError(2);

        std::vector<Decimal> ordered = SortedCopy(list);
        switch (quant) {
            case 0:
                return Operand::Create(ordered.front());
            case 1:
                return Operand::Create(QuantileCustom(list, Decimal::FromString("0.25")));
            case 2:
                return Operand::Create(QuantileCustom(list, Decimal::FromString("0.5")));
            case 3:
                return Operand::Create(QuantileCustom(list, Decimal::FromString("0.75")));
            default:
                return Operand::Create(ordered.back());
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// RANK
// ---------------------------------------------------------------------------
class Function_RANK : public FunctionN {
public:
    explicit Function_RANK(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RANK", 2, 3);
    }

    std::string Name() const override { return "RANK"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        if (ArgCount() < 2) return ParameterError(1);

        Operand numArg = GetNumber(1, engine, tp);
        if (numArg.IsErrorOrNone()) return numArg;
        Decimal num = numArg.NumberValue();

        Operand arrayArg = GetArray(2, engine, tp);
        if (arrayArg.IsErrorOrNone()) return arrayArg;

        int order = 0;
        if (ArgCount() > 2) {
            Operand orderArg = GetNumber(3, engine, tp);
            if (orderArg.IsErrorOrNone()) return orderArg;
            order = orderArg.IntValue();
        }

        std::vector<Decimal> values;
        for (const auto& item : arrayArg.ArrayValue()) {
            if (item.IsNumber()) values.push_back(item.NumberValue());
        }
        if (values.empty()) return ParameterError(2);

        bool descending = (order == 0);
        int rank = GetRank(values, num, descending);
        if (rank == 0) {
            // 数值未出现在数组中，Excel 返回 #N/A，此处复用通用函数错误表示
            return FunctionError();
        }
        return Operand::Create(rank);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        Arg(0)->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        Arg(1)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        if (ArgCount() > 2) {
            Arg(2)->GetParameterTypes(noneEngine, result, OperandType::NUMBER);
        }
    }
};

// ---------------------------------------------------------------------------
// SERIESSUM
// ---------------------------------------------------------------------------
class Function_SERIESSUM : public FunctionN {
public:
    explicit Function_SERIESSUM(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SERIESSUM", 4, 4);
    }

    std::string Name() const override { return "SERIESSUM"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand xArg = GetNumber(1, engine, tp);
        if (xArg.IsErrorOrNone()) return xArg;
        Decimal x = xArg.NumberValue();

        Operand nArg = GetNumber(2, engine, tp);
        if (nArg.IsErrorOrNone()) return nArg;
        Decimal n = nArg.NumberValue();

        Operand mArg = GetNumber(3, engine, tp);
        if (mArg.IsErrorOrNone()) return mArg;
        Decimal m = mArg.NumberValue();

        Operand coefficientsArg = GetArray(4, engine, tp);
        if (coefficientsArg.IsErrorOrNone()) return coefficientsArg;

        Decimal result;
        std::vector<Operand> coefArray = coefficientsArg.ArrayValue();
        for (size_t i = 0; i < coefArray.size(); i++) {
            const Operand& coef = coefArray[i];
            if (!coef.IsNumber()) continue;
            Decimal power = n.Add(Decimal::FromInt(static_cast<int64_t>(i)).Mul(m));
            Decimal pow;
            // 负底数非整数幂等非法运算、溢出，与 Excel 返回 #NUM! 保持一致
            if (!PowDecimal(x, power, pow)) return FunctionError();
            Decimal term = coef.NumberValue().Mul(pow);
            result = result.Add(term);
            if (IsOverflow(term) || IsOverflow(result)) return FunctionError();
        }
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result,
                   {OperandType::NUMBER, OperandType::NUMBER, OperandType::NUMBER, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// SLOPE
// ---------------------------------------------------------------------------
class Function_SLOPE : public FunctionN {
public:
    explicit Function_SLOPE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SLOPE", 2, 2);
    }

    std::string Name() const override { return "SLOPE"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand yArrayArg = GetArray(1, engine, tp);
        if (yArrayArg.IsErrorOrNone()) return yArrayArg;
        Operand xArrayArg = GetArray(2, engine, tp);
        if (xArrayArg.IsErrorOrNone()) return xArrayArg;

        Decimal slope;
        Decimal intercept;
        Operand error;
        if (!FitLine(*this, yArrayArg, xArrayArg, slope, intercept, error)) return error;
        return Operand::Create(slope);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// SMALL
// ---------------------------------------------------------------------------
class Function_SMALL : public FunctionN {
public:
    explicit Function_SMALL(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Small", 2, 2);
    }

    std::string Name() const override { return "Small"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = EvalArg(1, engine, tp);
        args1 = ConvertToArray(args1, 1);
        if (args1.IsErrorOrNone()) return args1;

        Operand args2 = EvalArg(2, engine, tp);
        args2 = ConvertToNumber(args2, 2);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);
        if (list.empty()) return ParameterError(1);

        int k = args2.IntValue() - engine.ExcelIndex;
        if (k < 0 || k >= static_cast<int>(list.size())) return ParameterError(2);
        return Operand::Create(QuickSelect(list, k, false));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::NUMBER});
    }
};

// ---------------------------------------------------------------------------
// STDEV
// ---------------------------------------------------------------------------
class Function_STDEV : public FunctionN {
public:
    explicit Function_STDEV(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "StDev", 1, -1);
    }

    std::string Name() const override { return "StDev"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        Decimal m2;
        int count = 0;
        Operand evalError;
        if (!WelfordM2(*this, args, m2, count, evalError)) return evalError;

        if (count <= 1) return FunctionError();
        return Operand::Create(m2.Div(Decimal::FromInt(count - 1)).Sqrt());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// STDEVP
// ---------------------------------------------------------------------------
class Function_STDEVP : public FunctionN {
public:
    explicit Function_STDEVP(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "StDevP", 1, -1);
    }

    std::string Name() const override { return "StDevP"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();
        if (list.empty()) return FunctionError();

        Decimal mean;
        Decimal m2;
        for (size_t i = 0; i < list.size(); i++) {
            Decimal delta = list[i].Sub(mean);
            mean = mean.Add(delta.Div(Decimal::FromInt(static_cast<int64_t>(i) + 1)));
            m2 = m2.Add(delta.Mul(list[i].Sub(mean)));
        }
        return Operand::Create(m2.Div(Decimal::FromInt(static_cast<int64_t>(list.size()))).Sqrt());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// SUM
// ---------------------------------------------------------------------------
class Function_SUM : public FunctionN {
public:
    explicit Function_SUM(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Sum", 1, -1);
    }

    std::string Name() const override { return "Sum"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        Decimal sum;
        for (const auto& item : args) {
            if (item.IsArray()) {
                std::vector<Operand> array = item.ArrayValue();
                for (const auto& elem : array) {
                    if (elem.IsNumber()) {
                        sum = sum.Add(elem.NumberValue());
                    } else if (elem.IsArray() || elem.IsJson()) {
                        std::vector<Decimal> list;
                        if (FlattenToNumberList(elem, list)) {
                            for (const auto& v : list) sum = sum.Add(v);
                        } else {
                            return FunctionError();
                        }
                    } else {
                        Operand converted = elem.ToNumber();
                        if (converted.IsError()) return FunctionError();
                        sum = sum.Add(converted.NumberValue());
                    }
                }
            } else if (item.IsNumber()) {
                sum = sum.Add(item.NumberValue());
            } else if (item.IsJson()) {
                std::vector<Decimal> list;
                if (!FlattenToNumberList(item, list)) return FunctionError();
                for (const auto& v : list) sum = sum.Add(v);
            } else {
                Operand converted = item.ToNumber();
                if (converted.IsError()) return FunctionError();
                sum = sum.Add(converted.NumberValue());
            }
        }
        return Operand::Create(sum);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// SUMIF
// ---------------------------------------------------------------------------
class Function_SUMIF : public FunctionN {
public:
    explicit Function_SUMIF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SumIf", 2, 3);
    }

    std::string Name() const override { return "SumIf"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand args1 = GetArray(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Operand args2 = EvalArg(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args1, list)) return ParameterError(1);

        std::vector<Decimal> sumdbs;
        if (ArgCount() > 2) {
            Operand args3 = GetArray(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            if (!FlattenToNumberList(args3, sumdbs)) return ParameterError(3);
        } else {
            sumdbs = list;
        }

        std::string op;
        Decimal value;
        if (!ParseCriteria(args2, op, value)) return ParameterError(2);

        Decimal sum = op.empty() ? GetSumIf(list, value, sumdbs) : GetSumIf(list, op, value, sumdbs);
        return Operand::Create(sum);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        Arg(0)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        Arg(1)->GetParameterTypes(noneEngine, result, OperandType::NONE);
        if (ArgCount() > 2) {
            Arg(2)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        }
    }
};

// ---------------------------------------------------------------------------
// SUMPRODUCT
// ---------------------------------------------------------------------------
class Function_SUMPRODUCT : public FunctionN {
public:
    explicit Function_SUMPRODUCT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SUMPRODUCT", 1, -1);
    }

    std::string Name() const override { return "SUMPRODUCT"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<std::vector<Decimal>> arrays;
        for (size_t i = 0; i < ArgCount(); i++) {
            Operand arg = GetArray(static_cast<int>(i) + 1, engine, tp);
            if (arg.IsErrorOrNone()) return arg;
            std::vector<Decimal> list;
            for (const auto& item : arg.ArrayValue()) {
                // 与 Excel 一致：非数值项按 0 参与运算，保留占位以保证各数组元素位置对齐
                list.push_back(item.IsNumber() ? item.NumberValue() : Decimal::Zero);
            }
            arrays.push_back(std::move(list));
        }

        // 与 Excel 一致：各数组长度必须相同，否则返回错误
        const size_t length = arrays[0].size();
        for (size_t i = 1; i < arrays.size(); i++) {
            if (arrays[i].size() != length) return FunctionError();
        }

        Decimal result;
        for (size_t i = 0; i < length; i++) {
            Decimal product = Decimal::One;
            for (const auto& arr : arrays) {
                product = product.Mul(arr[i]);
            }
            result = result.Add(product);
        }
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        for (size_t i = 0; i < ArgCount(); i++) {
            Arg(i)->GetParameterTypes(noneEngine, result, OperandType::ARRAY);
        }
    }
};

// ---------------------------------------------------------------------------
// SUMSQ
// ---------------------------------------------------------------------------
class Function_SUMSQ : public FunctionN {
public:
    explicit Function_SUMSQ(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SumSq", 1, -1);
    }

    std::string Name() const override { return "SumSq"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        for (size_t i = 0; i < ArgCount(); i++) {
            Operand aa = EvalArg(static_cast<int>(i) + 1, engine, tp);
            if (aa.IsErrorOrNone()) return aa;
            args.push_back(aa);
        }

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();

        Decimal d;
        for (const auto& a : list) {
            Decimal term = a.Mul(a);
            // decimal 溢出时统一转为函数错误，避免向调用方抛出异常
            if (IsOverflow(term)) return FunctionError();
            d = d.Add(term);
            if (IsOverflow(d)) return FunctionError();
        }
        return Operand::Create(d);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// SUMX2MY2 / SUMX2PY2 / SUMXMY2 共用：取两个数组的数值项
// ---------------------------------------------------------------------------
bool PairedNumberArrays(const Operand& arrayXArg, const Operand& arrayYArg,
                        std::vector<Decimal>& arrayX, std::vector<Decimal>& arrayY) {
    for (const auto& item : arrayXArg.ArrayValue()) {
        if (item.IsNumber()) arrayX.push_back(item.NumberValue());
    }
    for (const auto& item : arrayYArg.ArrayValue()) {
        if (item.IsNumber()) arrayY.push_back(item.NumberValue());
    }
    // Excel 在数组长度不等时返回 #N/A，此处统一用函数错误表示
    return arrayX.size() == arrayY.size();
}

// ---------------------------------------------------------------------------
// SUMX2MY2
// ---------------------------------------------------------------------------
class Function_SUMX2MY2 : public FunctionN {
public:
    explicit Function_SUMX2MY2(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SUMX2MY2", 2, 2);
    }

    std::string Name() const override { return "SUMX2MY2"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand arrayXArg = GetArray(1, engine, tp);
        if (arrayXArg.IsErrorOrNone()) return arrayXArg;
        Operand arrayYArg = GetArray(2, engine, tp);
        if (arrayYArg.IsErrorOrNone()) return arrayYArg;

        std::vector<Decimal> arrayX;
        std::vector<Decimal> arrayY;
        if (!PairedNumberArrays(arrayXArg, arrayYArg, arrayX, arrayY)) return FunctionError();

        Decimal result;
        for (size_t i = 0; i < arrayX.size(); i++) {
            Decimal xx = arrayX[i].Mul(arrayX[i]);
            Decimal yy = arrayY[i].Mul(arrayY[i]);
            Decimal term = xx.Sub(yy);
            if (IsOverflow(xx) || IsOverflow(yy) || IsOverflow(term)) return FunctionError();
            result = result.Add(term);
            if (IsOverflow(result)) return FunctionError();
        }
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// SUMX2PY2
// ---------------------------------------------------------------------------
class Function_SUMX2PY2 : public FunctionN {
public:
    explicit Function_SUMX2PY2(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SUMX2PY2", 2, 2);
    }

    std::string Name() const override { return "SUMX2PY2"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand arrayXArg = GetArray(1, engine, tp);
        if (arrayXArg.IsErrorOrNone()) return arrayXArg;
        Operand arrayYArg = GetArray(2, engine, tp);
        if (arrayYArg.IsErrorOrNone()) return arrayYArg;

        std::vector<Decimal> arrayX;
        std::vector<Decimal> arrayY;
        if (!PairedNumberArrays(arrayXArg, arrayYArg, arrayX, arrayY)) return FunctionError();

        Decimal result;
        for (size_t i = 0; i < arrayX.size(); i++) {
            Decimal xx = arrayX[i].Mul(arrayX[i]);
            Decimal yy = arrayY[i].Mul(arrayY[i]);
            Decimal term = xx.Add(yy);
            if (IsOverflow(xx) || IsOverflow(yy) || IsOverflow(term)) return FunctionError();
            result = result.Add(term);
            if (IsOverflow(result)) return FunctionError();
        }
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// SUMXMY2
// ---------------------------------------------------------------------------
class Function_SUMXMY2 : public FunctionN {
public:
    explicit Function_SUMXMY2(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SUMXMY2", 2, 2);
    }

    std::string Name() const override { return "SUMXMY2"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand arrayXArg = GetArray(1, engine, tp);
        if (arrayXArg.IsErrorOrNone()) return arrayXArg;
        Operand arrayYArg = GetArray(2, engine, tp);
        if (arrayYArg.IsErrorOrNone()) return arrayYArg;

        std::vector<Decimal> arrayX;
        std::vector<Decimal> arrayY;
        if (!PairedNumberArrays(arrayXArg, arrayYArg, arrayX, arrayY)) return FunctionError();

        Decimal result;
        for (size_t i = 0; i < arrayX.size(); i++) {
            Decimal diff = arrayX[i].Sub(arrayY[i]);
            Decimal term = diff.Mul(diff);
            if (IsOverflow(term)) return FunctionError();
            result = result.Add(term);
            if (IsOverflow(result)) return FunctionError();
        }
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        ParamTypes(*this, noneEngine, result, {OperandType::ARRAY, OperandType::ARRAY});
    }
};

// ---------------------------------------------------------------------------
// VAR
// ---------------------------------------------------------------------------
class Function_VAR : public FunctionN {
public:
    explicit Function_VAR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Var", 1, -1);
    }

    std::string Name() const override { return "Var"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        Decimal m2;
        int count = 0;
        Operand evalError;
        if (!WelfordM2(*this, args, m2, count, evalError)) return evalError;

        if (count <= 1) return FunctionError();
        return Operand::Create(m2.Div(Decimal::FromInt(count - 1)));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

// ---------------------------------------------------------------------------
// VARP
// ---------------------------------------------------------------------------
class Function_VARP : public FunctionN {
public:
    explicit Function_VARP(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "VarP", 1, -1);
    }

    std::string Name() const override { return "VarP"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        Operand error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();
        if (list.empty()) return FunctionError();
        if (list.size() == 1) return Operand::Zero();

        Decimal mean;
        Decimal m2;
        for (size_t i = 0; i < list.size(); i++) {
            Decimal delta = list[i].Sub(mean);
            mean = mean.Add(delta.Div(Decimal::FromInt(static_cast<int64_t>(i) + 1)));
            m2 = m2.Add(delta.Mul(list[i].Sub(mean)));
        }
        return Operand::Create(m2.Div(Decimal::FromInt(static_cast<int64_t>(list.size()))));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result, OperandType,
                           const std::string& = std::string(),
                           const std::string& = std::string()) override {
        SingleOrMultiTypes(*this, noneEngine, result, OperandType::ARRAY, OperandType::NUMBER);
    }
};

}  // namespace

// ---------------------------------------------------------------------------
// 工厂
// ---------------------------------------------------------------------------

Function NewFunction_AVEDEV(std::vector<Function> args) {
    return std::make_shared<Function_AVEDEV>(std::move(args));
}
Function NewFunction_AVERAGE(std::vector<Function> args) {
    return std::make_shared<Function_AVERAGE>(std::move(args));
}
Function NewFunction_AVERAGEIF(std::vector<Function> args) {
    return std::make_shared<Function_AVERAGEIF>(std::move(args));
}
Function NewFunction_CORREL(std::vector<Function> args) {
    return std::make_shared<Function_CORREL>(std::move(args));
}
Function NewFunction_COUNT(std::vector<Function> args) {
    return std::make_shared<Function_COUNT>(std::move(args));
}
Function NewFunction_COUNTIF(std::vector<Function> args) {
    return std::make_shared<Function_COUNTIF>(std::move(args));
}
Function NewFunction_COVAR(std::vector<Function> args) {
    return std::make_shared<Function_COVAR>(std::move(args));
}
Function NewFunction_COVARIANCES(std::vector<Function> args) {
    return std::make_shared<Function_COVARIANCES>(std::move(args));
}
Function NewFunction_DEVSQ(std::vector<Function> args) {
    return std::make_shared<Function_DEVSQ>(std::move(args));
}
Function NewFunction_FORECAST(std::vector<Function> args) {
    return std::make_shared<Function_FORECAST>(std::move(args));
}
Function NewFunction_GEOMEAN(std::vector<Function> args) {
    return std::make_shared<Function_GEOMEAN>(std::move(args));
}
Function NewFunction_HARMEAN(std::vector<Function> args) {
    return std::make_shared<Function_HARMEAN>(std::move(args));
}
Function NewFunction_INTERCEPT(std::vector<Function> args) {
    return std::make_shared<Function_INTERCEPT>(std::move(args));
}
Function NewFunction_LARGE(std::vector<Function> args) {
    return std::make_shared<Function_LARGE>(std::move(args));
}
Function NewFunction_MAX(std::vector<Function> args) {
    return std::make_shared<Function_MAX>(std::move(args));
}
Function NewFunction_MEDIAN(std::vector<Function> args) {
    return std::make_shared<Function_MEDIAN>(std::move(args));
}
Function NewFunction_MIN(std::vector<Function> args) {
    return std::make_shared<Function_MIN>(std::move(args));
}
Function NewFunction_MODE(std::vector<Function> args) {
    return std::make_shared<Function_MODE>(std::move(args));
}
Function NewFunction_PEARSON(std::vector<Function> args) {
    return std::make_shared<Function_PEARSON>(std::move(args));
}
Function NewFunction_PERCENTILE(std::vector<Function> args) {
    return std::make_shared<Function_PERCENTILE>(std::move(args));
}
Function NewFunction_PERCENTRANK(std::vector<Function> args) {
    return std::make_shared<Function_PERCENTRANK>(std::move(args));
}
Function NewFunction_POISSON(std::vector<Function> args) {
    return std::make_shared<Function_POISSON>(std::move(args));
}
Function NewFunction_QUARTILE(std::vector<Function> args) {
    return std::make_shared<Function_QUARTILE>(std::move(args));
}
Function NewFunction_RANK(std::vector<Function> args) {
    return std::make_shared<Function_RANK>(std::move(args));
}
Function NewFunction_SERIESSUM(std::vector<Function> args) {
    return std::make_shared<Function_SERIESSUM>(std::move(args));
}
Function NewFunction_SLOPE(std::vector<Function> args) {
    return std::make_shared<Function_SLOPE>(std::move(args));
}
Function NewFunction_SMALL(std::vector<Function> args) {
    return std::make_shared<Function_SMALL>(std::move(args));
}
Function NewFunction_STDEV(std::vector<Function> args) {
    return std::make_shared<Function_STDEV>(std::move(args));
}
Function NewFunction_STDEVP(std::vector<Function> args) {
    return std::make_shared<Function_STDEVP>(std::move(args));
}
Function NewFunction_SUM(std::vector<Function> args) {
    return std::make_shared<Function_SUM>(std::move(args));
}
Function NewFunction_SUMIF(std::vector<Function> args) {
    return std::make_shared<Function_SUMIF>(std::move(args));
}
Function NewFunction_SUMPRODUCT(std::vector<Function> args) {
    return std::make_shared<Function_SUMPRODUCT>(std::move(args));
}
Function NewFunction_SUMSQ(std::vector<Function> args) {
    return std::make_shared<Function_SUMSQ>(std::move(args));
}
Function NewFunction_SUMX2MY2(std::vector<Function> args) {
    return std::make_shared<Function_SUMX2MY2>(std::move(args));
}
Function NewFunction_SUMX2PY2(std::vector<Function> args) {
    return std::make_shared<Function_SUMX2PY2>(std::move(args));
}
Function NewFunction_SUMXMY2(std::vector<Function> args) {
    return std::make_shared<Function_SUMXMY2>(std::move(args));
}
Function NewFunction_VAR(std::vector<Function> args) {
    return std::make_shared<Function_VAR>(std::move(args));
}
Function NewFunction_VARP(std::vector<Function> args) {
    return std::make_shared<Function_VARP>(std::move(args));
}

void Registermathsum() {
    RegisterFunction("AVEDEV", [](const std::vector<Function>& a) { return NewFunction_AVEDEV(a); });
    RegisterFunction("AVERAGE", [](const std::vector<Function>& a) { return NewFunction_AVERAGE(a); });
    RegisterFunction("AVERAGEIF",
                     [](const std::vector<Function>& a) { return NewFunction_AVERAGEIF(a); });
    RegisterFunction("CORREL", [](const std::vector<Function>& a) { return NewFunction_CORREL(a); });
    RegisterFunction("COUNT", [](const std::vector<Function>& a) { return NewFunction_COUNT(a); });
    RegisterFunction("COUNTIF", [](const std::vector<Function>& a) { return NewFunction_COUNTIF(a); });
    RegisterFunction("COVAR", [](const std::vector<Function>& a) { return NewFunction_COVAR(a); });
    RegisterFunction("COVARIANCES",
                     [](const std::vector<Function>& a) { return NewFunction_COVARIANCES(a); });
    RegisterFunction("DEVSQ", [](const std::vector<Function>& a) { return NewFunction_DEVSQ(a); });
    RegisterFunction("FORECAST",
                     [](const std::vector<Function>& a) { return NewFunction_FORECAST(a); });
    RegisterFunction("GEOMEAN", [](const std::vector<Function>& a) { return NewFunction_GEOMEAN(a); });
    RegisterFunction("HARMEAN", [](const std::vector<Function>& a) { return NewFunction_HARMEAN(a); });
    RegisterFunction("INTERCEPT",
                     [](const std::vector<Function>& a) { return NewFunction_INTERCEPT(a); });
    RegisterFunction("LARGE", [](const std::vector<Function>& a) { return NewFunction_LARGE(a); });
    RegisterFunction("MAX", [](const std::vector<Function>& a) { return NewFunction_MAX(a); });
    RegisterFunction("MEDIAN", [](const std::vector<Function>& a) { return NewFunction_MEDIAN(a); });
    RegisterFunction("MIN", [](const std::vector<Function>& a) { return NewFunction_MIN(a); });
    RegisterFunction("MODE", [](const std::vector<Function>& a) { return NewFunction_MODE(a); });
    RegisterFunction("PEARSON", [](const std::vector<Function>& a) { return NewFunction_PEARSON(a); });
    RegisterFunction("PERCENTILE",
                     [](const std::vector<Function>& a) { return NewFunction_PERCENTILE(a); });
    RegisterFunction("PERCENTRANK",
                     [](const std::vector<Function>& a) { return NewFunction_PERCENTRANK(a); });
    RegisterFunction("POISSON", [](const std::vector<Function>& a) { return NewFunction_POISSON(a); });
    RegisterFunction("QUARTILE",
                     [](const std::vector<Function>& a) { return NewFunction_QUARTILE(a); });
    RegisterFunction("RANK", [](const std::vector<Function>& a) { return NewFunction_RANK(a); });
    RegisterFunction("SERIESSUM",
                     [](const std::vector<Function>& a) { return NewFunction_SERIESSUM(a); });
    RegisterFunction("SLOPE", [](const std::vector<Function>& a) { return NewFunction_SLOPE(a); });
    RegisterFunction("SMALL", [](const std::vector<Function>& a) { return NewFunction_SMALL(a); });
    RegisterFunction("STDEV", [](const std::vector<Function>& a) { return NewFunction_STDEV(a); });
    RegisterFunction("STDEVP", [](const std::vector<Function>& a) { return NewFunction_STDEVP(a); });
    RegisterFunction("SUM", [](const std::vector<Function>& a) { return NewFunction_SUM(a); });
    RegisterFunction("SUMIF", [](const std::vector<Function>& a) { return NewFunction_SUMIF(a); });
    RegisterFunction("SUMPRODUCT",
                     [](const std::vector<Function>& a) { return NewFunction_SUMPRODUCT(a); });
    RegisterFunction("SUMSQ", [](const std::vector<Function>& a) { return NewFunction_SUMSQ(a); });
    RegisterFunction("SUMX2MY2",
                     [](const std::vector<Function>& a) { return NewFunction_SUMX2MY2(a); });
    RegisterFunction("SUMX2PY2",
                     [](const std::vector<Function>& a) { return NewFunction_SUMX2PY2(a); });
    RegisterFunction("SUMXMY2", [](const std::vector<Function>& a) { return NewFunction_SUMXMY2(a); });
    RegisterFunction("VAR", [](const std::vector<Function>& a) { return NewFunction_VAR(a); });
    RegisterFunction("VARP", [](const std::vector<Function>& a) { return NewFunction_VARP(a); });
}

}  // namespace toolgood
