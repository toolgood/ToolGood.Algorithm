// MathTrigonometric.cpp - 对应 csharp/.../Internals/Functions/MathTrigonometric/
#include "MathTrigonometric.h"

#include <stdexcept>
#include <string>
#include <vector>

#include "../../../Decimal.h"
#include "../../../MathEx.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

/// 单参数三角函数公共基类
class Trig1 : public FunctionN {
public:
    Trig1(std::vector<Function> args, std::string name)
        : FunctionN(std::move(args)), name_(std::move(name)) {
        RequireArgs(args_, name_, 1, 1);
    }

    std::string Name() const override { return name_; }

    OperandType GetResultType() const override { return OperandType::NUMBER; }

private:
    std::string name_;
};

// ---------------- Function_ACOS ----------------

class Function_ACOS : public Trig1 {
public:
    explicit Function_ACOS(std::vector<Function> args) : Trig1(std::move(args), "Acos") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        if (x.Less(Decimal::FromInt(-1)) || x.Greater(Decimal::One)) return NumError();
        return Operand::Create(MathEx::Acos(x));
    }
};

// ---------------- Function_ACOSH ----------------

class Function_ACOSH : public Trig1 {
public:
    explicit Function_ACOSH(std::vector<Function> args) : Trig1(std::move(args), "Acosh") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal z = args1.NumberValue();
        if (z.Less(Decimal::One)) return NumError();
        return Operand::Create(MathEx::Acosh(z));
    }
};

// ---------------- Function_ACOT ----------------

class Function_ACOT : public Trig1 {
public:
    explicit Function_ACOT(std::vector<Function> args) : Trig1(std::move(args), "Acot") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // acot(x)=π/2-atan(x)
        try {
            const Decimal halfPi = MathEx::Div(MathEx::PI(), Decimal::FromInt(2));
            return Operand::Create(halfPi.Sub(MathEx::Atan(args1.NumberValue())));
        } catch (const std::overflow_error&) {
            return NumError();
        }
    }
};

// ---------------- Function_ACOTH ----------------

class Function_ACOTH : public Trig1 {
public:
    explicit Function_ACOTH(std::vector<Function> args) : Trig1(std::move(args), "Acoth") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal d = args1.NumberValue();
        // 不能写成 Math.Abs(d) <= 1:decimal.MinValue 取绝对值会溢出抛异常
        if (d.GreaterOrEqual(Decimal::FromInt(-1)) && d.LessOrEqual(Decimal::One)) {
            return NumError();
        }
        // acoth(d)=atanh(1/d),用倒数形式可避免大 d 时 (d+1)/(d-1) 饱和为 1 导致结果恒为 0
        return Operand::Create(MathEx::Atanh(MathEx::Div(Decimal::One, d)));
    }
};

// ---------------- Function_ASIN ----------------

class Function_ASIN : public Trig1 {
public:
    explicit Function_ASIN(std::vector<Function> args) : Trig1(std::move(args), "Asin") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        if (x.Less(Decimal::FromInt(-1)) || x.Greater(Decimal::One)) return NumError();
        return Operand::Create(MathEx::Asin(x));
    }
};

// ---------------- Function_ASINH ----------------

class Function_ASINH : public Trig1 {
public:
    explicit Function_ASINH(std::vector<Function> args) : Trig1(std::move(args), "Asinh") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        try {
            return Operand::Create(MathEx::Asinh(args1.NumberValue()));
        } catch (const std::overflow_error&) {
            return NumError();
        }
    }
};

// ---------------- Function_ATAN ----------------

class Function_ATAN : public Trig1 {
public:
    explicit Function_ATAN(std::vector<Function> args) : Trig1(std::move(args), "Atan") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        try {
            return Operand::Create(MathEx::Atan(args1.NumberValue()));
        } catch (const std::overflow_error&) {
            return NumError();
        }
    }
};

// ---------------- Function_ATAN2 ----------------

class Function_ATAN2 : public FunctionN {
public:
    explicit Function_ATAN2(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Atan2", 2, 2);
    }

    std::string Name() const override { return "Atan2"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        auto args2 = GetNumber(2, engine, tp);
        if (args2.IsErrorOrNone()) return args2;
        Decimal x = args1.NumberValue();
        Decimal y = args2.NumberValue();
        if (x.IsZero() && y.IsZero()) return Div0Error();
        // C#: MathEx.Atan2(y, x)
        try {
            return Operand::Create(MathEx::Atan2(y, x));
        } catch (const std::overflow_error&) {
            // 两参数量级相差过大时 y/x 本身会溢出 decimal
            return NumError();
        }
    }

    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

// ---------------- Function_ATANH ----------------

class Function_ATANH : public Trig1 {
public:
    explicit Function_ATANH(std::vector<Function> args) : Trig1(std::move(args), "Atanh") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        if (x.GreaterOrEqual(Decimal::One) || x.LessOrEqual(Decimal::FromInt(-1))) {
            return NumError();
        }
        return Operand::Create(MathEx::Atanh(x));
    }
};

// ---------------- Function_COS ----------------

class Function_COS : public Trig1 {
public:
    explicit Function_COS(std::vector<Function> args) : Trig1(std::move(args), "Cos") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(MathEx::Cos(args1.NumberValue()));
    }
};

// ---------------- Function_COSH ----------------

class Function_COSH : public Trig1 {
public:
    explicit Function_COSH(std::vector<Function> args) : Trig1(std::move(args), "Cosh") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // |x| 超过约 67.235(即 ln(2*decimal.MaxValue))时结果本身超出 decimal 可表示范围
        try {
            return Operand::Create(MathEx::Cosh(args1.NumberValue()));
        } catch (const std::overflow_error&) {
            return NumError();
        }
    }
};

// ---------------- Function_COT ----------------

class Function_COT : public Trig1 {
public:
    explicit Function_COT(std::vector<Function> args) : Trig1(std::move(args), "Cot") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal d = MathEx::Tan(args1.NumberValue());
        if (d.IsZero()) return Div0Error();
        return Operand::Create(MathEx::Div(Decimal::One, d));
    }
};

// ---------------- Function_COTH ----------------

class Function_COTH : public Trig1 {
public:
    explicit Function_COTH(std::vector<Function> args) : Trig1(std::move(args), "Coth") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        // coth 在 |x| 较大时趋近 ±1,提前返回避免 e^|x| 溢出
        if (x.GreaterOrEqual(Decimal::FromInt(66))) return Operand::Create(Decimal::One);
        if (x.LessOrEqual(Decimal::FromInt(-66))) return Operand::Create(Decimal::FromInt(-1));
        Decimal s = MathEx::Sinh(x);
        if (s.IsZero()) return Div0Error();
        return Operand::Create(MathEx::Div(MathEx::Cosh(x), s));
    }
};

// ---------------- Function_CSC ----------------

class Function_CSC : public Trig1 {
public:
    explicit Function_CSC(std::vector<Function> args) : Trig1(std::move(args), "Csc") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal d = MathEx::Sin(args1.NumberValue());
        if (d.IsZero()) return Div0Error();
        return Operand::Create(MathEx::Div(Decimal::One, d));
    }
};

// ---------------- Function_CSCH ----------------

class Function_CSCH : public Trig1 {
public:
    explicit Function_CSCH(std::vector<Function> args) : Trig1(std::move(args), "Csch") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        // csch 在 |x| 较大时趋近 0,提前返回避免 e^|x| 溢出
        if (x.GreaterOrEqual(Decimal::FromInt(66)) || x.LessOrEqual(Decimal::FromInt(-66))) {
            return Operand::Zero();
        }
        Decimal d = MathEx::Sinh(x);
        if (d.IsZero()) return Div0Error();
        return Operand::Create(MathEx::Div(Decimal::One, d));
    }
};

// ---------------- Function_DEGREES ----------------

class Function_DEGREES : public Trig1 {
public:
    explicit Function_DEGREES(std::vector<Function> args) : Trig1(std::move(args), "Degrees") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // 弧度值超过约 1.38e26 时换算结果超出 decimal 可表示范围
        try {
            const Decimal z = args1.NumberValue();
            return Operand::Create(MathEx::Div(z, MathEx::PI()).Mul(Decimal::FromInt(180)));
        } catch (const std::overflow_error&) {
            return NumError();
        }
    }
};

// ---------------- Function_RADIANS ----------------

class Function_RADIANS : public Trig1 {
public:
    explicit Function_RADIANS(std::vector<Function> args) : Trig1(std::move(args), "Radians") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal z = args1.NumberValue();
        return Operand::Create(MathEx::Div(z, Decimal::FromInt(180)).Mul(MathEx::PI()));
    }
};

// ---------------- Function_SEC ----------------

class Function_SEC : public Trig1 {
public:
    explicit Function_SEC(std::vector<Function> args) : Trig1(std::move(args), "Sec") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal d = MathEx::Cos(args1.NumberValue());
        if (d.IsZero()) return Div0Error();
        return Operand::Create(MathEx::Div(Decimal::One, d));
    }
};

// ---------------- Function_SECH ----------------

class Function_SECH : public Trig1 {
public:
    explicit Function_SECH(std::vector<Function> args) : Trig1(std::move(args), "Sech") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        // sech 在 |x| 较大时趋近 0,提前返回避免 e^|x| 溢出
        if (x.GreaterOrEqual(Decimal::FromInt(66)) || x.LessOrEqual(Decimal::FromInt(-66))) {
            return Operand::Zero();
        }
        return Operand::Create(MathEx::Div(Decimal::One, MathEx::Cosh(x)));
    }
};

// ---------------- Function_SIN ----------------

class Function_SIN : public Trig1 {
public:
    explicit Function_SIN(std::vector<Function> args) : Trig1(std::move(args), "Sin") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        return Operand::Create(MathEx::Sin(args1.NumberValue()));
    }
};

// ---------------- Function_SINH ----------------

class Function_SINH : public Trig1 {
public:
    explicit Function_SINH(std::vector<Function> args) : Trig1(std::move(args), "Sinh") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        // |x| 超过约 67.235(即 ln(2*decimal.MaxValue))时结果本身超出 decimal 可表示范围
        try {
            return Operand::Create(MathEx::Sinh(args1.NumberValue()));
        } catch (const std::overflow_error&) {
            return NumError();
        }
    }
};

// ---------------- Function_TAN ----------------

class Function_TAN : public Trig1 {
public:
    explicit Function_TAN(std::vector<Function> args) : Trig1(std::move(args), "Tan") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        Decimal cosValue = MathEx::Cos(x);
        if (cosValue.IsZero()) return Div0Error();
        return Operand::Create(MathEx::Div(MathEx::Sin(x), cosValue));
    }
};

// ---------------- Function_TANH ----------------

class Function_TANH : public Trig1 {
public:
    explicit Function_TANH(std::vector<Function> args) : Trig1(std::move(args), "Tanh") {}

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto args1 = GetNumber(1, engine, tp);
        if (args1.IsErrorOrNone()) return args1;
        Decimal x = args1.NumberValue();
        // tanh 在 |x| 较大时趋近 ±1,提前返回避免 e^|x| 溢出
        if (x.GreaterOrEqual(Decimal::FromInt(66))) return Operand::Create(Decimal::One);
        if (x.LessOrEqual(Decimal::FromInt(-66))) return Operand::Create(Decimal::FromInt(-1));
        return Operand::Create(MathEx::Tanh(x));
    }
};

}  // namespace

Function NewFunction_ACOS(std::vector<Function> args) {
    return std::make_shared<Function_ACOS>(std::move(args));
}

Function NewFunction_ACOSH(std::vector<Function> args) {
    return std::make_shared<Function_ACOSH>(std::move(args));
}

Function NewFunction_ACOT(std::vector<Function> args) {
    return std::make_shared<Function_ACOT>(std::move(args));
}

Function NewFunction_ACOTH(std::vector<Function> args) {
    return std::make_shared<Function_ACOTH>(std::move(args));
}

Function NewFunction_ASIN(std::vector<Function> args) {
    return std::make_shared<Function_ASIN>(std::move(args));
}

Function NewFunction_ASINH(std::vector<Function> args) {
    return std::make_shared<Function_ASINH>(std::move(args));
}

Function NewFunction_ATAN(std::vector<Function> args) {
    return std::make_shared<Function_ATAN>(std::move(args));
}

Function NewFunction_ATAN2(std::vector<Function> args) {
    return std::make_shared<Function_ATAN2>(std::move(args));
}

Function NewFunction_ATANH(std::vector<Function> args) {
    return std::make_shared<Function_ATANH>(std::move(args));
}

Function NewFunction_COS(std::vector<Function> args) {
    return std::make_shared<Function_COS>(std::move(args));
}

Function NewFunction_COSH(std::vector<Function> args) {
    return std::make_shared<Function_COSH>(std::move(args));
}

Function NewFunction_COT(std::vector<Function> args) {
    return std::make_shared<Function_COT>(std::move(args));
}

Function NewFunction_COTH(std::vector<Function> args) {
    return std::make_shared<Function_COTH>(std::move(args));
}

Function NewFunction_CSC(std::vector<Function> args) {
    return std::make_shared<Function_CSC>(std::move(args));
}

Function NewFunction_CSCH(std::vector<Function> args) {
    return std::make_shared<Function_CSCH>(std::move(args));
}

Function NewFunction_DEGREES(std::vector<Function> args) {
    return std::make_shared<Function_DEGREES>(std::move(args));
}

Function NewFunction_RADIANS(std::vector<Function> args) {
    return std::make_shared<Function_RADIANS>(std::move(args));
}

Function NewFunction_SEC(std::vector<Function> args) {
    return std::make_shared<Function_SEC>(std::move(args));
}

Function NewFunction_SECH(std::vector<Function> args) {
    return std::make_shared<Function_SECH>(std::move(args));
}

Function NewFunction_SIN(std::vector<Function> args) {
    return std::make_shared<Function_SIN>(std::move(args));
}

Function NewFunction_SINH(std::vector<Function> args) {
    return std::make_shared<Function_SINH>(std::move(args));
}

Function NewFunction_TAN(std::vector<Function> args) {
    return std::make_shared<Function_TAN>(std::move(args));
}

Function NewFunction_TANH(std::vector<Function> args) {
    return std::make_shared<Function_TANH>(std::move(args));
}

}  // namespace toolgood
