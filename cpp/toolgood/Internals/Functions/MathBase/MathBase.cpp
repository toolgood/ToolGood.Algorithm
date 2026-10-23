// MathBase.cpp - 对应 csharp/.../Internals/Functions/MathBase/
#include "MathBase.h"

#include <algorithm>
#include <cmath>
#include <limits>
#include <random>
#include <stdexcept>
#include <string>
#include <vector>

#include "../../../Decimal.h"
#include "../../../MathEx.h"
#include "../../../Operands/Operand.h"
#include "../../FunctionUtil.h"

namespace toolgood {

namespace {

const Decimal& IntMinValue() {
    static const Decimal v = Decimal::FromInt(std::numeric_limits<int>::min());
    return v;
}

const Decimal& IntMaxValue() {
    static const Decimal v = Decimal::FromInt(std::numeric_limits<int>::max());
    return v;
}

/// 是否为偶数（对应 C# 的 z % 2 == 0）
bool IsEvenNumber(const Decimal& z) { return z.Mod(Decimal::FromInt(2)).IsZero(); }

/// 是否为奇数（对应 C# 的 z % 2 == 1 || z % 2 == -1）
bool IsOddNumber(const Decimal& z) {
    Decimal r = z.Mod(Decimal::FromInt(2));
    return r.Equal(Decimal::One) || r.Equal(Decimal::FromInt(-1));
}

/// 10^n
Decimal Pow10(int n) { return Decimal::FromInt(10).Pow(n); }

/// random [0,1)
std::mt19937_64& RandomEngine() {
    static thread_local std::mt19937_64 engine{std::random_device{}()};
    return engine;
}

double NextDouble() {
    static thread_local std::uniform_real_distribution<double> dist(0.0, 1.0);
    return dist(RandomEngine());
}

/// .NET 的 "N{digits}" 格式化（InvariantCulture：千分位 ','，小数点 '.'）
std::string FormatNumberN(const Decimal& value, int digits) {
    std::string s = value.PlainStrip();
    bool neg = !s.empty() && s[0] == '-';
    if (neg) s.erase(0, 1);

    std::string intPart = s;
    std::string fracPart;
    size_t dot = s.find('.');
    if (dot != std::string::npos) {
        intPart = s.substr(0, dot);
        fracPart = s.substr(dot + 1);
    }
    if (intPart.empty()) intPart = "0";
    if (static_cast<int>(fracPart.size()) > digits) {
        fracPart.resize(static_cast<size_t>(digits));
    }
    fracPart.append(static_cast<size_t>(digits) - fracPart.size(), '0');

    std::string grouped;
    int count = 0;
    for (int i = static_cast<int>(intPart.size()) - 1; i >= 0; i--) {
        grouped.push_back(intPart[static_cast<size_t>(i)]);
        count++;
        if (count == 3 && i > 0) {
            grouped.push_back(',');
            count = 0;
        }
    }
    std::reverse(grouped.begin(), grouped.end());

    std::string result;
    if (neg) result.push_back('-');
    result += grouped;
    if (digits > 0) {
        result.push_back('.');
        result += fracPart;
    }
    return result;
}

// ---------------- Function_ABS ----------------

class Function_ABS : public FunctionN {
public:
    explicit Function_ABS(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Abs", 1, 1);
    }

    std::string Name() const override { return "Abs"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(args1.NumberValue().Abs());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_CEILING ----------------

class Function_CEILING : public FunctionN {
public:
    explicit Function_CEILING(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Ceiling", 1, 2);
    }

    std::string Name() const override { return "Ceiling"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        if (ArgCount() < 2) {
            return Operand::Create(args1.NumberValue().Ceil());
        }

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal b = args2.NumberValue();
        if (b.IsZero()) return Operand::Zero();

        Decimal a = args1.NumberValue();
        if (b.IsPositive()) {
            return Operand::Create(a.Div(b).Ceil().Mul(b));
        }
        if (a.IsPositive()) {
            return ParameterError(1);
        }
        return Operand::Create(a.Div(b).Ceil().Mul(b));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_COMBIN ----------------

class Function_COMBIN : public FunctionN {
public:
    explicit Function_COMBIN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Combin", 2, 2);
    }

    std::string Name() const override { return "Combin"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        // 先检查数值范围,避免 (int)decimal 强转超范围抛 OverflowException
        Decimal num1 = args1.NumberValue();
        Decimal num2 = args2.NumberValue();
        if (num1.Less(IntMinValue()) || num1.Greater(IntMaxValue())) return ParameterError(1);
        if (num2.Less(IntMinValue()) || num2.Greater(IntMaxValue())) return ParameterError(2);

        int total = num1.Int();
        int count = num2.Int();
        if (total < 0) return ParameterError(1);
        if (count < 0) return ParameterError(2);
        if (total < count) return ParameterError(2);

        // C(n,k) 具有对称性 C(n,k)=C(n,n-k)，取较小的 k 计算，避免大 count 时中间乘积溢出
        int k = std::min(count, total - count);
        Decimal result = Decimal::One;
        for (int i = 1; i <= k; i++) {
            // 边乘边除:每步结果都等于某个组合数，不会产生无谓的中间膨胀
            result = result.Mul(Decimal::FromInt(total - k + i)).Div(Decimal::FromInt(i));
        }
        return Operand::Create(result);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_DELTA ----------------

class Function_DELTA : public FunctionN {
public:
    explicit Function_DELTA(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Delta", 1, 2);
    }

    std::string Name() const override { return "Delta"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal num1 = args1.NumberValue();

        Decimal num2;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            num2 = args2.NumberValue();
        }

        return Operand::Create(num1.Equal(num2) ? 1 : 0);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_EVEN ----------------

class Function_EVEN : public FunctionN {
public:
    explicit Function_EVEN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Even", 1, 1);
    }

    std::string Name() const override { return "Even"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal z = args1.NumberValue();

        if (z.IsZero()) return Operand::Zero();

        if (z.IsPositive()) {
            if (IsEvenNumber(z)) return args1;
            z = z.Ceil();
            if (IsEvenNumber(z)) return Operand::Create(z);
            z = z.Add(Decimal::One);
            return Operand::Create(z);
        }
        if (IsEvenNumber(z)) return args1;
        z = z.Floor();
        if (IsEvenNumber(z)) return Operand::Create(z);
        z = z.Sub(Decimal::One);
        return Operand::Create(z);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_EXP ----------------

class Function_EXP : public FunctionN {
public:
    explicit Function_EXP(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Exp", 1, 1);
    }

    std::string Name() const override { return "Exp"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal z = args1.NumberValue();
        if (z.Greater(Decimal::FromInt(66))) return ParameterError(1);
        return Operand::Create(MathEx::Exp(z));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_FACT ----------------

class Function_FACT : public FunctionN {
public:
    explicit Function_FACT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Fact", 1, 1);
    }

    std::string Name() const override { return "Fact"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        // 先检查数值范围,避免 (int)decimal 强转超范围抛 OverflowException
        Decimal number = args1.NumberValue();
        if (number.Less(IntMinValue()) || number.Greater(IntMaxValue())) return ParameterError(1);
        int z = number.Int();
        if (z < 0) return ParameterError(1);
        if (z > 27) return ParameterError(1);

        Decimal d = Decimal::One;
        for (int i = 1; i <= z; i++) {
            d = d.Mul(Decimal::FromInt(i));
        }
        return Operand::Create(d);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_FACTDOUBLE ----------------

class Function_FACTDOUBLE : public FunctionN {
public:
    explicit Function_FACTDOUBLE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "FactDouble", 1, 1);
    }

    std::string Name() const override { return "FactDouble"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        // 先检查数值范围,避免 (int)decimal 强转超范围抛 OverflowException
        Decimal number = args1.NumberValue();
        if (number.Less(IntMinValue()) || number.Greater(IntMaxValue())) return ParameterError(1);
        int z = number.Int();
        if (z < 0) return ParameterError(1);
        // decimal 最大可容纳 45!!(≈2.537e28),46!! 溢出;与 FACT 的 28 限制不同
        if (z > 45) return ParameterError(1);

        Decimal d = Decimal::One;
        for (int i = z; i > 0; i -= 2) {
            d = d.Mul(Decimal::FromInt(i));
        }
        return Operand::Create(d);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_FIXED ----------------

class Function_FIXED : public FunctionN {
public:
    explicit Function_FIXED(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Fixed", 1, 3);
    }

    std::string Name() const override { return "Fixed"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        // 参数按从左到右的顺序求值,保证副作用与错误序号一致
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        int num = 2;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            Decimal dv = args2.NumberValue();
            if (!dv.FitsInt32()) return ParameterError(2);
            num = dv.Int();
            // Excel 支持负数 decimals(向左取整),如 FIXED(1234.567,-1)="1,230",范围与 ROUND 一致
            if (num < -15 || num > 15) return ParameterError(2);
        }

        Decimal s = args1.NumberValue();
        if (num >= 0) {
            s = s.RoundHalfUp(num);
        } else {
            // Math.Round(decimal, int) 只支持非负位数,负数位数(向左取整)改用先除后乘
            Decimal factor = Pow10(-num);
            s = s.Div(factor).RoundHalfUp(0).Mul(factor);
        }

        bool no = false;
        if (ArgCount() >= 3) {
            auto args3 = GetBoolean(3, engine, tp);
            if (args3.IsErrorOrNone()) return args3;
            no = args3.BooleanValue();
        }
        if (!no) {
            if (num < 0) {
                // 负数位数取整后无小数位,用 N0 保持千分位
                return Operand::Create(FormatNumberN(s, 0));
            }
            return Operand::Create(FormatNumberN(s, num));
        }
        return Operand::Create(s.ToString());
    }

    OperandType GetResultType() const override { return OperandType::TEXT; }
};

// ---------------- Function_FLOOR ----------------

class Function_FLOOR : public FunctionN {
public:
    explicit Function_FLOOR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Floor", 1, 2);
    }

    std::string Name() const override { return "Floor"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        if (ArgCount() < 2) {
            return Operand::Create(args1.NumberValue().Floor());
        }

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal b = args2.NumberValue();
        if (b.IsZero()) return Operand::Zero();

        Decimal a = args1.NumberValue();
        if (b.IsPositive()) {
            return Operand::Create(a.Div(b).Floor().Mul(b));
        }
        if (a.IsPositive()) {
            return ParameterError(1);
        }
        return Operand::Create(a.Div(b).Floor().Mul(b));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_GCD ----------------

class Function_GCD : public FunctionN {
public:
    explicit Function_GCD(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Gcd", 1, -1);
    }

    std::string Name() const override { return "Gcd"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        args.reserve(ArgCount());
        for (size_t i = 0; i < ArgCount(); i++) {
            auto aa = GetNumber(static_cast<int>(i) + 1, engine, tp);
            if (aa.IsErrorOrNone()) return aa;
            args.push_back(aa);
        }

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();

        for (size_t i = 0; i < list.size(); i++) {
            if (list[i].Less(Decimal::Zero)) {
                return ParameterError(static_cast<int>(i) + 1);
            }
        }

        return Operand::Create(GetGCD(list));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_GESTEP ----------------

class Function_GESTEP : public FunctionN {
public:
    explicit Function_GESTEP(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "GeStep", 1, 2);
    }

    std::string Name() const override { return "GeStep"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal number = args1.NumberValue();

        Decimal step;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            step = args2.NumberValue();
        }

        return Operand::Create(number.GreaterOrEqual(step) ? 1 : 0);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_INT ----------------

class Function_INT : public FunctionN {
public:
    explicit Function_INT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "INT", 1, 1);
    }

    std::string Name() const override { return "INT"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // Excel INT 语义:向下取整(Floor),且避免 (int)decimal 强转的溢出风险
        return Operand::Create(args1.NumberValue().Floor());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_LCM ----------------

class Function_LCM : public FunctionN {
public:
    explicit Function_LCM(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Lcm", 1, -1);
    }

    std::string Name() const override { return "Lcm"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        args.reserve(ArgCount());
        for (size_t i = 0; i < ArgCount(); i++) {
            auto aa = GetNumber(static_cast<int>(i) + 1, engine, tp);
            if (aa.IsErrorOrNone()) return aa;
            args.push_back(aa);
        }

        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();

        for (size_t i = 0; i < list.size(); i++) {
            if (list[i].Less(Decimal::Zero)) {
                return ParameterError(static_cast<int>(i) + 1);
            }
        }

        return Operand::Create(GetLCM(list));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_LN ----------------

class Function_LN : public FunctionN {
public:
    explicit Function_LN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Ln", 1, 1);
    }

    std::string Name() const override { return "Ln"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal z = args1.NumberValue();
        if (!z.IsPositive()) return ParameterError(1);
        return Operand::Create(MathEx::Log(z));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_LOG ----------------

class Function_LOG : public FunctionN {
public:
    explicit Function_LOG(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Log", 1, 2);
    }

    std::string Name() const override { return "Log"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        Decimal z = args1.NumberValue();
        if (!z.IsPositive()) return ParameterError(1);

        if (ArgCount() < 2) {
            return Operand::Create(MathEx::Log10(z));
        }

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal baseValue = args2.NumberValue();
        if (!baseValue.IsPositive() || baseValue.Equal(Decimal::One)) return ParameterError(2);
        return Operand::Create(MathEx::Log(z, baseValue));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_LOG10 ----------------

class Function_LOG10 : public FunctionN {
public:
    explicit Function_LOG10(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Log10", 1, 1);
    }

    std::string Name() const override { return "Log10"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        Decimal z = args1.NumberValue();
        if (!z.IsPositive()) return ParameterError(1);
        return Operand::Create(MathEx::Log10(z));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_MROUND ----------------

class Function_MROUND : public FunctionN {
public:
    explicit Function_MROUND(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Mround", 2, 2);
    }

    std::string Name() const override { return "Mround"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal multiple = args2.NumberValue();
        if (multiple.IsZero()) return ParameterError(2);

        Decimal number = args1.NumberValue();

        if ((number.IsPositive() && multiple.IsNegative()) ||
            (number.IsNegative() && multiple.IsPositive())) {
            return ParameterError(2);
        }

        Decimal r = number.Div(multiple).RoundHalfUp(0).Mul(multiple);
        return Operand::Create(r);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_MULTINOMIAL ----------------

class Function_MULTINOMIAL : public FunctionN {
public:
    explicit Function_MULTINOMIAL(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Multinomial", 1, -1);
    }

    std::string Name() const override { return "Multinomial"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        auto error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        std::vector<Decimal> list;
        for (size_t i = 0; i < args.size(); i++) {
            // 按参数逐个展平：展平后的下标与原始参数位置不再一一对应,
            // 逐个展平才能让错误消息给出真实的参数序号
            std::vector<Decimal> item;
            if (!FlattenToNumberList(args[i], item)) return FunctionError();
            for (size_t j = 0; j < item.size(); j++) {
                Decimal value = item[j];
                if (value.Less(IntMinValue()) || value.Greater(IntMaxValue())) {
                    return ParameterError(static_cast<int>(i) + 1);
                }
                if (value.Int() < 0) {
                    return ParameterError(static_cast<int>(i) + 1);
                }
            }
            list.insert(list.end(), item.begin(), item.end());
        }

        Decimal sum;
        Decimal n = Decimal::One;
        for (size_t i = 0; i < list.size(); i++) {
            int a = list[i].Int();
            n = n.Mul(GetFactorial(Decimal::FromInt(a)));
            sum = sum.Add(Decimal::FromInt(a));
        }

        Decimal r = GetFactorial(sum).Div(n);
        return Operand::Create(r);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_ODD ----------------

class Function_ODD : public FunctionN {
public:
    explicit Function_ODD(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Odd", 1, 1);
    }

    std::string Name() const override { return "Odd"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal z = args1.NumberValue();

        if (z.IsZero()) return Operand::Create(1);

        if (z.IsPositive()) {
            if (IsOddNumber(z)) return args1;
            z = z.Ceil();
            if (!IsEvenNumber(z)) return Operand::Create(z);
            z = z.Add(Decimal::One);
            return Operand::Create(z);
        }
        if (IsOddNumber(z)) return args1;
        z = z.Floor();
        if (!IsEvenNumber(z)) return Operand::Create(z);
        z = z.Sub(Decimal::One);
        return Operand::Create(z);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_PERMUT ----------------

class Function_PERMUT : public FunctionN {
public:
    explicit Function_PERMUT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Permut", 2, 2);
    }

    std::string Name() const override { return "Permut"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        // 先检查数值范围,避免 (int)decimal 强转超范围抛 OverflowException
        Decimal num1 = args1.NumberValue();
        Decimal num2 = args2.NumberValue();
        if (num1.Less(IntMinValue()) || num1.Greater(IntMaxValue())) return ParameterError(1);
        if (num2.Less(IntMinValue()) || num2.Greater(IntMaxValue())) return ParameterError(2);

        int total = num1.Int();
        int count = num2.Int();

        if (total < 0) return ParameterError(1);
        if (count < 0) return ParameterError(2);
        if (total < count) return ParameterError(2);

        Decimal sum = Decimal::One;
        for (int i = 0; i < count; i++) {
            sum = sum.Mul(Decimal::FromInt(total - i));
        }
        return Operand::Create(sum);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_POWER ----------------

class Function_POWER : public FunctionN {
public:
    explicit Function_POWER(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Power", 2, 2);
    }

    std::string Name() const override { return "Power"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        Decimal baseValue = args1.NumberValue();
        Decimal exponent = args2.NumberValue();

        if (baseValue.IsZero() && exponent.IsNegative()) return Div0Error();
        if (baseValue.IsNegative() && !exponent.IsInteger()) return ParameterError(1);

        try {
            // MathEx::Pow 对超大结果会抛 overflow_error，捕获并返回错误
            return Operand::Create(MathEx::Pow(baseValue, exponent));
        } catch (const std::overflow_error&) {
            return FunctionError();
        } catch (const std::domain_error&) {
            return ParameterError(1);
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_PRODUCT ----------------

class Function_PRODUCT : public FunctionN {
public:
    explicit Function_PRODUCT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Product", 1, -1);
    }

    std::string Name() const override { return "Product"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> args;
        auto error = EvaluateAll(engine, tp, args);
        if (error.IsErrorOrNone()) return error;

        // 直接求值并展平,避免对数组参数调用 GetNumber 导致报错
        std::vector<Decimal> list;
        if (!FlattenToNumberList(args, list)) return FunctionError();

        Decimal d = Decimal::One;
        for (size_t i = 0; i < list.size(); i++) {
            d = d.Mul(list[i]);
        }
        return Operand::Create(d);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_Percentage ----------------

class Function_Percentage : public FunctionN {
public:
    explicit Function_Percentage(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Percentage", 1, 1);
    }

    std::string Name() const override { return "Percentage"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(args1.NumberValue().Div(Decimal::FromInt(100)));
    }

    void ToString(std::string& sb, bool addBrackets) override {
        (void)addBrackets;
        args_[0]->ToString(sb, false);
        sb += '%';
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_QUOTIENT ----------------

class Function_QUOTIENT : public FunctionN {
public:
    explicit Function_QUOTIENT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Quotient", 2, 2);
    }

    std::string Name() const override { return "Quotient"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        if (args2.NumberValue().IsZero()) return Div0Error();

        // 商超 int 范围或 decimal 除法溢出时返回错误,避免异常穿透
        Decimal result = args1.NumberValue().Div(args2.NumberValue());
        if (result.Less(IntMinValue()) || result.Greater(IntMaxValue())) return FunctionError();
        return Operand::Create(result.Int());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_RAND ----------------

class Function_RAND : public FunctionN {
public:
    explicit Function_RAND(std::vector<Function> args) : FunctionN(std::move(args)) {}

    std::string Name() const override { return "Rand"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        (void)engine;
        (void)tp;
        return Operand::Create(NextDouble());
    }

    void ToString(std::string& sb, bool addBrackets) override {
        (void)addBrackets;
        sb += "Rand()";
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_RANDBETWEEN ----------------

class Function_RANDBETWEEN : public FunctionN {
public:
    explicit Function_RANDBETWEEN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RandBetween", 2, 2);
    }

    std::string Name() const override { return "RandBetween"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;

        Decimal bottom = args1.NumberValue();
        Decimal top = args2.NumberValue();
        if (bottom.Greater(top)) return ParameterError(1);

        Decimal r = Decimal::FromDouble(NextDouble())
                        .Mul(top.Sub(bottom).Add(Decimal::One))
                        .Add(bottom)
                        .Floor();
        return Operand::Create(r);
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_ROUND ----------------

class Function_ROUND : public FunctionN {
public:
    explicit Function_ROUND(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Round", 1, 2);
    }

    std::string Name() const override { return "Round"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        if (ArgCount() < 2) {
            return Operand::Create(args1.NumberValue().RoundHalfUp(0));
        }
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal dv = args2.NumberValue();
        if (!dv.FitsInt32()) return ParameterError(2);
        int digits = dv.Int();
        if (digits < -15 || digits > 15) return ParameterError(2);

        Decimal num = args1.NumberValue();
        if (digits >= 0) {
            return Operand::Create(num.RoundHalfUp(digits));
        }
        // Math.Round(decimal, int) 只支持非负位数,负数位数(向左取整)改用先除后乘
        Decimal factor = Pow10(-digits);
        return Operand::Create(num.Div(factor).RoundHalfUp(0).Mul(factor));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_ROUNDDOWN ----------------

class Function_ROUNDDOWN : public FunctionN {
public:
    explicit Function_ROUNDDOWN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RoundDown", 2, 2);
    }

    std::string Name() const override { return "RoundDown"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal dv = args2.NumberValue();
        if (!dv.FitsInt32()) return ParameterError(2);
        if (dv.Int() < -15 || dv.Int() > 15) return ParameterError(2);

        if (args1.NumberValue().IsZero()) return args1;

        Decimal num = args1.NumberValue();
        int digits = dv.Int();
        Decimal factor = Pow10(digits);
        if (num.IsPositive()) {
            return Operand::Create(num.Mul(factor).Floor().Div(factor));
        }
        return Operand::Create(num.Mul(factor).Ceil().Div(factor));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_ROUNDUP ----------------

class Function_ROUNDUP : public FunctionN {
public:
    explicit Function_ROUNDUP(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "RoundUp", 2, 2);
    }

    std::string Name() const override { return "RoundUp"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal dv = args2.NumberValue();
        if (!dv.FitsInt32()) return ParameterError(2);
        if (dv.Int() < -15 || dv.Int() > 15) return ParameterError(2);

        if (args1.NumberValue().IsZero()) return args1;
        Decimal b = args1.NumberValue();

        Decimal a = Pow10(dv.Int());
        Decimal t = b.Abs().Mul(a).Ceil().Div(a);
        if (b.IsPositive()) return Operand::Create(t);
        return Operand::Create(t.Neg());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_SIGN ----------------

class Function_SIGN : public FunctionN {
public:
    explicit Function_SIGN(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Sign", 1, 1);
    }

    std::string Name() const override { return "Sign"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(args1.NumberValue().Sign());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_SQRT ----------------

class Function_SQRT : public FunctionN {
public:
    explicit Function_SQRT(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Sqrt", 1, 1);
    }

    std::string Name() const override { return "Sqrt"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        if (args1.NumberValue().IsNegative()) return ParameterError(1);
        return Operand::Create(args1.NumberValue().Sqrt());
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_SQRTPI ----------------

class Function_SQRTPI : public FunctionN {
public:
    explicit Function_SQRTPI(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "SqrtPi", 1, 1);
    }

    std::string Name() const override { return "SqrtPi"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        if (args1.NumberValue().IsNegative()) return ParameterError(1);

        try {
            return Operand::Create(MathEx::Sqrt(args1.NumberValue().Mul(MathEx::PI())));
        } catch (const std::overflow_error&) {
            return NumError();
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_TRUNC ----------------

class Function_TRUNC : public FunctionN {
public:
    explicit Function_TRUNC(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Trunc", 1, 2);
    }

    std::string Name() const override { return "Trunc"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;

        int digits = 0;
        if (ArgCount() >= 2) {
            auto args2 = GetNumber(2, engine, tp);
            if (args2.IsErrorOrNone()) return args2;
            Decimal dv = args2.NumberValue();
            if (!dv.FitsInt32()) return ParameterError(2);
            digits = dv.Int();
            if (digits < -15 || digits > 15) return ParameterError(2);
        }

        if (args1.NumberValue().IsZero()) return args1;

        Decimal num = args1.NumberValue();
        Decimal factor = Pow10(digits);
        if (num.IsPositive()) {
            return Operand::Create(num.Mul(factor).Floor().Div(factor));
        }
        return Operand::Create(num.Mul(factor).Ceil().Div(factor));
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

}  // namespace

Function NewFunction_ABS(std::vector<Function> args) {
    return std::make_shared<Function_ABS>(std::move(args));
}

Function NewFunction_CEILING(std::vector<Function> args) {
    return std::make_shared<Function_CEILING>(std::move(args));
}

Function NewFunction_COMBIN(std::vector<Function> args) {
    return std::make_shared<Function_COMBIN>(std::move(args));
}

Function NewFunction_DELTA(std::vector<Function> args) {
    return std::make_shared<Function_DELTA>(std::move(args));
}

Function NewFunction_EVEN(std::vector<Function> args) {
    return std::make_shared<Function_EVEN>(std::move(args));
}

Function NewFunction_EXP(std::vector<Function> args) {
    return std::make_shared<Function_EXP>(std::move(args));
}

Function NewFunction_FACT(std::vector<Function> args) {
    return std::make_shared<Function_FACT>(std::move(args));
}

Function NewFunction_FACTDOUBLE(std::vector<Function> args) {
    return std::make_shared<Function_FACTDOUBLE>(std::move(args));
}

Function NewFunction_FIXED(std::vector<Function> args) {
    return std::make_shared<Function_FIXED>(std::move(args));
}

Function NewFunction_FLOOR(std::vector<Function> args) {
    return std::make_shared<Function_FLOOR>(std::move(args));
}

Function NewFunction_GCD(std::vector<Function> args) {
    return std::make_shared<Function_GCD>(std::move(args));
}

Function NewFunction_GESTEP(std::vector<Function> args) {
    return std::make_shared<Function_GESTEP>(std::move(args));
}

Function NewFunction_INT(std::vector<Function> args) {
    return std::make_shared<Function_INT>(std::move(args));
}

Function NewFunction_LCM(std::vector<Function> args) {
    return std::make_shared<Function_LCM>(std::move(args));
}

Function NewFunction_LN(std::vector<Function> args) {
    return std::make_shared<Function_LN>(std::move(args));
}

Function NewFunction_LOG(std::vector<Function> args) {
    return std::make_shared<Function_LOG>(std::move(args));
}

Function NewFunction_LOG10(std::vector<Function> args) {
    return std::make_shared<Function_LOG10>(std::move(args));
}

Function NewFunction_MROUND(std::vector<Function> args) {
    return std::make_shared<Function_MROUND>(std::move(args));
}

Function NewFunction_MULTINOMIAL(std::vector<Function> args) {
    return std::make_shared<Function_MULTINOMIAL>(std::move(args));
}

Function NewFunction_ODD(std::vector<Function> args) {
    return std::make_shared<Function_ODD>(std::move(args));
}

Function NewFunction_PERMUT(std::vector<Function> args) {
    return std::make_shared<Function_PERMUT>(std::move(args));
}

Function NewFunction_POWER(std::vector<Function> args) {
    return std::make_shared<Function_POWER>(std::move(args));
}

Function NewFunction_PRODUCT(std::vector<Function> args) {
    return std::make_shared<Function_PRODUCT>(std::move(args));
}

Function NewFunction_Percentage(std::vector<Function> args) {
    return std::make_shared<Function_Percentage>(std::move(args));
}

Function NewFunction_QUOTIENT(std::vector<Function> args) {
    return std::make_shared<Function_QUOTIENT>(std::move(args));
}

Function NewFunction_RAND(std::vector<Function> args) {
    return std::make_shared<Function_RAND>(std::move(args));
}

Function NewFunction_RANDBETWEEN(std::vector<Function> args) {
    return std::make_shared<Function_RANDBETWEEN>(std::move(args));
}

Function NewFunction_ROUND(std::vector<Function> args) {
    return std::make_shared<Function_ROUND>(std::move(args));
}

Function NewFunction_ROUNDDOWN(std::vector<Function> args) {
    return std::make_shared<Function_ROUNDDOWN>(std::move(args));
}

Function NewFunction_ROUNDUP(std::vector<Function> args) {
    return std::make_shared<Function_ROUNDUP>(std::move(args));
}

Function NewFunction_SIGN(std::vector<Function> args) {
    return std::make_shared<Function_SIGN>(std::move(args));
}

Function NewFunction_SQRT(std::vector<Function> args) {
    return std::make_shared<Function_SQRT>(std::move(args));
}

Function NewFunction_SQRTPI(std::vector<Function> args) {
    return std::make_shared<Function_SQRTPI>(std::move(args));
}

Function NewFunction_TRUNC(std::vector<Function> args) {
    return std::make_shared<Function_TRUNC>(std::move(args));
}

}  // namespace toolgood
