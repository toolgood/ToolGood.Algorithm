// Flow 组实现，对应 csharp/.../Internals/Functions/Flow/*
#include "Flow.h"

#include <stdexcept>
#include <string>
#include <utility>

#include "../../../AlgorithmEngine.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

/// 判断文本是否为空白（对应 string.IsNullOrWhiteSpace）
bool IsNullOrWhiteSpace(const std::string& s) {
    for (char c : s) {
        if (c != ' ' && c != '\t' && c != '\r' && c != '\n' && c != '\f' && c != '\v') return false;
    }
    return true;
}

class Function_IF : public FunctionN {
public:
    explicit Function_IF(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "If", 2, 3);
    }
    std::string Name() const override { return "If"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetBoolean(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (a1.BooleanValue()) return args_[1]->Evaluate(engine, tp);
        if (args_.size() < 3) return Operand::False();
        return args_[2]->Evaluate(engine, tp);
    }
    OperandType GetResultType() const override { return OperandType::NONE; }
};

class Function_IFERROR : public FunctionN {
public:
    explicit Function_IFERROR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IfError", 2, 3);
    }
    std::string Name() const override { return "IfError"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsNone()) return a1;
        if (a1.IsError()) return args_[1]->Evaluate(engine, tp);
        if (args_.size() < 3) return a1;
        return args_[2]->Evaluate(engine, tp);
    }
    OperandType GetResultType() const override {
        OperandType t2 = args_[1]->GetResultType();
        if (t2 != OperandType::NONE) return t2;
        if (args_.size() >= 3) return args_[2]->GetResultType();
        return OperandType::NONE;
    }
};

class Function_IFS : public FunctionN {
public:
    explicit Function_IFS(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Ifs", 4, -1);
        if (args_.size() % 2 != 0) {
            throw std::runtime_error("Function 'Ifs' requires an even number of parameters.");
        }
    }
    std::string Name() const override { return "Ifs"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        for (size_t i = 0; i + 1 < args_.size(); i += 2) {
            Operand condition = GetBoolean(static_cast<int>(i) + 1, engine, tp);
            if (condition.IsErrorOrNone()) return condition;
            if (condition.BooleanValue()) return args_[i + 1]->Evaluate(engine, tp);
        }
        return FunctionError();
    }
    OperandType GetResultType() const override { return OperandType::NONE; }
};

class Function_ISERROR : public FunctionN {
public:
    explicit Function_ISERROR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IsError", 1, 2);
    }
    std::string Name() const override { return "IsError"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsNone()) return a1;
        if (args_.size() >= 2) {
            if (a1.IsError()) return args_[1]->Evaluate(engine, tp);
            return a1;
        }
        return a1.IsError() ? Operand::True() : Operand::False();
    }
    OperandType GetResultType() const override {
        if (args_.size() >= 2) return args_[1]->GetResultType();
        return OperandType::BOOLEAN;
    }
};

class Function_ISNULL : public FunctionN {
public:
    explicit Function_ISNULL(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IsNull", 1, 2);
    }
    std::string Name() const override { return "IsNull"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (args_.size() >= 2) {
            if (a1.IsNull()) return args_[1]->Evaluate(engine, tp);
            return a1;
        }
        return a1.IsNull() ? Operand::True() : Operand::False();
    }
    OperandType GetResultType() const override {
        if (args_.size() >= 2) return args_[1]->GetResultType();
        return OperandType::BOOLEAN;
    }
};

class Function_ISNULLORERROR : public FunctionN {
public:
    explicit Function_ISNULLORERROR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IsNullOrError", 1, 2);
    }
    std::string Name() const override { return "IsNullOrError"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (args_.size() >= 2) {
            if (a1.IsNull() || a1.IsErrorOrNone()) return args_[1]->Evaluate(engine, tp);
            return a1;
        }
        return (a1.IsNull() || a1.IsErrorOrNone()) ? Operand::True() : Operand::False();
    }
    OperandType GetResultType() const override {
        if (args_.size() >= 2) return args_[1]->GetResultType();
        return OperandType::BOOLEAN;
    }
};

class Function_ISNULLOREMPTY : public FunctionN {
public:
    explicit Function_ISNULLOREMPTY(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IsNullOrEmpty", 1, 1);
    }
    std::string Name() const override { return "IsNullOrEmpty"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (a1.IsNull()) return Operand::True();
        Operand textArg = ConvertToText(a1, 1);
        if (textArg.IsErrorOrNone()) return textArg;
        return Operand::Create(textArg.TextValue().empty());
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_ISNULLORWHITESPACE : public FunctionN {
public:
    explicit Function_ISNULLORWHITESPACE(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "IsNullOrWhitespace", 1, 1);
    }
    std::string Name() const override { return "IsNullOrWhitespace"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (a1.IsNull()) return Operand::True();
        Operand textArg = ConvertToText(a1, 1);
        if (textArg.IsErrorOrNone()) return textArg;
        return Operand::Create(IsNullOrWhiteSpace(textArg.TextValue()));
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

/// 单参数布尔判定公共基类
class BoolUnary : public FunctionN {
public:
    explicit BoolUnary(std::vector<Function> args, const char* name)
        : FunctionN(std::move(args)), name_(name) {
        RequireArgs(args_, name_, 1, 1);
    }
    std::string Name() const override { return name_; }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }

protected:
    std::string name_;
};

class Function_ISEVEN : public BoolUnary {
public:
    explicit Function_ISEVEN(std::vector<Function> args) : BoolUnary(std::move(args), "IsEven") {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (a1.IsNumber()) {
            Decimal number = a1.NumberValue().Trunc();
            if (number.Mod(Decimal::FromInt(2)).IsZero()) return Operand::True();
        }
        return Operand::False();
    }
};

class Function_ISODD : public BoolUnary {
public:
    explicit Function_ISODD(std::vector<Function> args) : BoolUnary(std::move(args), "IsOdd") {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (a1.IsNumber()) {
            Decimal number = a1.NumberValue().Trunc();
            Decimal m = number.Abs().Mod(Decimal::FromInt(2));
            if (m.Equal(Decimal::One)) return Operand::True();
        }
        return Operand::False();
    }
};

class Function_ISLOGICAL : public BoolUnary {
public:
    explicit Function_ISLOGICAL(std::vector<Function> args)
        : BoolUnary(std::move(args), "IsLogical") {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        return a1.IsBoolean() ? Operand::True() : Operand::False();
    }
};

class Function_ISNONTEXT : public BoolUnary {
public:
    explicit Function_ISNONTEXT(std::vector<Function> args)
        : BoolUnary(std::move(args), "IsNonText") {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        return a1.IsText() ? Operand::False() : Operand::True();
    }
};

class Function_ISNUMBER : public BoolUnary {
public:
    explicit Function_ISNUMBER(std::vector<Function> args)
        : BoolUnary(std::move(args), "IsNumber") {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        return a1.IsNumber() ? Operand::True() : Operand::False();
    }
};

class Function_ISTEXT : public BoolUnary {
public:
    explicit Function_ISTEXT(std::vector<Function> args) : BoolUnary(std::move(args), "IsText") {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = args_[0]->Evaluate(engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        return a1.IsText() ? Operand::True() : Operand::False();
    }
};

class Function_NOT : public BoolUnary {
public:
    explicit Function_NOT(std::vector<Function> args) : BoolUnary(std::move(args), "Not") {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetBoolean(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        return a1.BooleanValue() ? Operand::False() : Operand::True();
    }
};

class Function_SWITCH : public FunctionN {
public:
    explicit Function_SWITCH(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Switch", 3, -1);
    }
    std::string Name() const override { return "Switch"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand exprValue = args_[0]->Evaluate(engine, tp);
        if (exprValue.IsErrorOrNone()) return exprValue;

        size_t i = 1;
        while (i + 1 < args_.size()) {
            Operand compareValue = args_[i]->Evaluate(engine, tp);
            if (compareValue.IsErrorOrNone()) return compareValue;
            if (EqualsOperand(exprValue, compareValue)) {
                return args_[i + 1]->Evaluate(engine, tp);
            }
            i += 2;
        }
        if (args_.size() % 2 == 0) {
            return args_.back()->Evaluate(engine, tp);
        }
        return FunctionError();
    }
    OperandType GetResultType() const override { return OperandType::NONE; }

private:
    static bool EqualsOperand(const Operand& a, const Operand& b) {
        if (a.IsNumber() && b.IsNumber()) return a.NumberValue().Equal(b.NumberValue());
        if (a.IsText() && b.IsText()) return a.TextValue() == b.TextValue();
        if (a.IsBoolean() && b.IsBoolean()) return a.BooleanValue() == b.BooleanValue();
        if (a.IsDate() && b.IsDate()) return a.DateValue().ToLong() == b.DateValue().ToLong();
        if (a.IsNull() && b.IsNull()) return true;
        if (a.IsNull() || b.IsNull()) return false;
        if (a.IsDate() || b.IsDate() || a.IsArray() || b.IsArray() || a.IsJson() || b.IsJson() ||
            a.IsArrayJson() || b.IsArrayJson()) {
            return false;
        }
        Operand na = a.ToNumber();
        if (na.IsErrorOrNone()) return false;
        Operand nb = b.ToNumber();
        if (nb.IsErrorOrNone()) return false;
        return na.NumberValue().Equal(nb.NumberValue());
    }
};

}  // namespace

Function NewFunction_IF(std::vector<Function> args) {
    return std::make_shared<Function_IF>(std::move(args));
}
Function NewFunction_IFERROR(std::vector<Function> args) {
    return std::make_shared<Function_IFERROR>(std::move(args));
}
Function NewFunction_IFS(std::vector<Function> args) {
    return std::make_shared<Function_IFS>(std::move(args));
}
Function NewFunction_ISERROR(std::vector<Function> args) {
    return std::make_shared<Function_ISERROR>(std::move(args));
}
Function NewFunction_ISEVEN(std::vector<Function> args) {
    return std::make_shared<Function_ISEVEN>(std::move(args));
}
Function NewFunction_ISLOGICAL(std::vector<Function> args) {
    return std::make_shared<Function_ISLOGICAL>(std::move(args));
}
Function NewFunction_ISNONTEXT(std::vector<Function> args) {
    return std::make_shared<Function_ISNONTEXT>(std::move(args));
}
Function NewFunction_ISNULL(std::vector<Function> args) {
    return std::make_shared<Function_ISNULL>(std::move(args));
}
Function NewFunction_ISNULLOREMPTY(std::vector<Function> args) {
    return std::make_shared<Function_ISNULLOREMPTY>(std::move(args));
}
Function NewFunction_ISNULLORERROR(std::vector<Function> args) {
    return std::make_shared<Function_ISNULLORERROR>(std::move(args));
}
Function NewFunction_ISNULLORWHITESPACE(std::vector<Function> args) {
    return std::make_shared<Function_ISNULLORWHITESPACE>(std::move(args));
}
Function NewFunction_ISNUMBER(std::vector<Function> args) {
    return std::make_shared<Function_ISNUMBER>(std::move(args));
}
Function NewFunction_ISODD(std::vector<Function> args) {
    return std::make_shared<Function_ISODD>(std::move(args));
}
Function NewFunction_ISTEXT(std::vector<Function> args) {
    return std::make_shared<Function_ISTEXT>(std::move(args));
}
Function NewFunction_NOT(std::vector<Function> args) {
    return std::make_shared<Function_NOT>(std::move(args));
}
Function NewFunction_SWITCH(std::vector<Function> args) {
    return std::make_shared<Function_SWITCH>(std::move(args));
}

}  // namespace toolgood
