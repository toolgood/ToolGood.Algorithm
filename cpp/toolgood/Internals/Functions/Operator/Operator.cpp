// Operator 组实现，对应 csharp/.../Internals/Functions/Operator/*
#include "Operator.h"

#include <stdexcept>
#include <string>
#include <utility>

#include "../../../AlgorithmEngine.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

/// 二元运算符公共基类：名字与符号在构造时传入，避免构造期调用纯虚函数。
class BinaryOp : public FunctionN {
public:
    BinaryOp(std::vector<Function> args, const char* name, const char* symbol, OperandType argType)
        : FunctionN(std::move(args)), name_(name), symbol_(symbol), argType_(argType) {
        RequireArgs(args_, name, 2, 2);
    }

    std::string Name() const override { return name_; }

    void ToString(std::string& sb, bool addBrackets) override {
        if (addBrackets) sb += '(';
        args_[0]->ToString(sb, true);
        sb += ' ';
        sb += symbol_;
        sb += ' ';
        args_[1]->ToString(sb, true);
        if (addBrackets) sb += ')';
    }

    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, argType_);
        args_[1]->GetParameterTypes(noneEngine, result, argType_);
    }

protected:
    const char* name_;
    const char* symbol_;
    OperandType argType_;
};

class Function_Add : public BinaryOp {
public:
    explicit Function_Add(std::vector<Function> args)
        : BinaryOp(std::move(args), "+", "+", OperandType::NUMBER) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetNumber(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        Operand a2 = GetNumber(2, engine, tp);
        if (a2.IsErrorOrNone()) return a2;
        Decimal r = a1.NumberValue().Add(a2.NumberValue());
        if (r.Abs().Greater(Decimal::Max)) return NumError();
        return Operand::Create(r);
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_Sub : public BinaryOp {
public:
    explicit Function_Sub(std::vector<Function> args)
        : BinaryOp(std::move(args), "-", "-", OperandType::NUMBER) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetNumber(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        Operand a2 = GetNumber(2, engine, tp);
        if (a2.IsErrorOrNone()) return a2;
        if (a2.NumberValue().IsZero()) return a1;
        Decimal r = a1.NumberValue().Sub(a2.NumberValue());
        if (r.Abs().Greater(Decimal::Max)) return NumError();
        return Operand::Create(r);
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_Mul : public BinaryOp {
public:
    explicit Function_Mul(std::vector<Function> args)
        : BinaryOp(std::move(args), "*", "*", OperandType::NUMBER) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetNumber(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        Operand a2 = GetNumber(2, engine, tp);
        if (a2.IsErrorOrNone()) return a2;
        if (a1.NumberValue().Equal(Decimal::One)) return a2;
        if (a2.NumberValue().Equal(Decimal::One)) return a1;
        Decimal r = a1.NumberValue().Mul(a2.NumberValue());
        if (r.Abs().Greater(Decimal::Max)) return NumError();
        return Operand::Create(r);
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_Div : public BinaryOp {
public:
    explicit Function_Div(std::vector<Function> args)
        : BinaryOp(std::move(args), "/", "/", OperandType::NUMBER) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetNumber(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        Operand a2 = GetNumber(2, engine, tp);
        if (a2.IsErrorOrNone()) return a2;
        if (a2.NumberValue().IsZero()) return Div0Error();
        if (a2.NumberValue().Equal(Decimal::One)) return a1;
        Decimal r = a1.NumberValue().Div(a2.NumberValue());
        if (r.Abs().Greater(Decimal::Max)) return NumError();
        return Operand::Create(r);
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

/// 对应 Function_Mod（%）：Excel MOD 语义
class Function_Mod : public BinaryOp {
public:
    explicit Function_Mod(std::vector<Function> args)
        : BinaryOp(std::move(args), "%", "%", OperandType::NUMBER) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetNumber(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        Operand a2 = GetNumber(2, engine, tp);
        if (a2.IsErrorOrNone()) return a2;
        if (a2.NumberValue().IsZero()) return Div0Error();
        Decimal n1 = a1.NumberValue();
        Decimal n2 = a2.NumberValue();
        Decimal r = n1.Mod(n2);
        if (!r.IsZero() && (r.IsNegative() != n2.IsNegative())) r = r.Add(n2);
        return Operand::Create(r);
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
};

class Function_Connect : public BinaryOp {
public:
    explicit Function_Connect(std::vector<Function> args)
        : BinaryOp(std::move(args), "&", "&", OperandType::TEXT) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetText(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        Operand a2 = GetText(2, engine, tp);
        if (a2.IsErrorOrNone()) return a2;
        return Operand::Create(a1.TextValue() + a2.TextValue());
    }
    OperandType GetResultType() const override { return OperandType::TEXT; }
};

class Function_AND : public BinaryOp {
public:
    explicit Function_AND(std::vector<Function> args)
        : BinaryOp(std::move(args), "And", "&&", OperandType::BOOLEAN) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetBoolean(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (!a1.BooleanValue()) {
            if (engine.UseStrictMode) {
                Operand a2 = GetBoolean(2, engine, tp);
                if (a2.IsErrorOrNone()) return a2;
            }
            return Operand::False();
        }
        return GetBoolean(2, engine, tp);
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_OR : public BinaryOp {
public:
    explicit Function_OR(std::vector<Function> args)
        : BinaryOp(std::move(args), "Or", "||", OperandType::BOOLEAN) {}
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = GetBoolean(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        if (a1.BooleanValue()) {
            if (engine.UseStrictMode) {
                Operand a2 = GetBoolean(2, engine, tp);
                if (a2.IsErrorOrNone()) return a2;
            }
            return Operand::True();
        }
        return GetBoolean(2, engine, tp);
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
};

class Function_AND_N : public FunctionN {
public:
    using FunctionN::FunctionN;
    std::string Name() const override { return "And"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        bool b = true;
        for (size_t i = 0; i < args_.size(); i++) {
            Operand a = GetBoolean(static_cast<int>(i) + 1, engine, tp);
            if (a.IsErrorOrNone()) return a;
            if (!a.BooleanValue()) {
                b = false;
                if (!engine.UseStrictMode) break;
            }
        }
        return b ? Operand::True() : Operand::False();
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        for (auto& f : args_) f->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
    }
};

class Function_OR_N : public FunctionN {
public:
    using FunctionN::FunctionN;
    std::string Name() const override { return "Or"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        bool b = false;
        for (size_t i = 0; i < args_.size(); i++) {
            Operand a = GetBoolean(static_cast<int>(i) + 1, engine, tp);
            if (a.IsErrorOrNone()) return a;
            if (a.BooleanValue()) {
                b = true;
                if (!engine.UseStrictMode) break;
            }
        }
        return b ? Operand::True() : Operand::False();
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        for (auto& f : args_) f->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
    }
};

class Function_XOR : public FunctionN {
public:
    using FunctionN::FunctionN;
    std::string Name() const override { return "Xor"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        int trueCount = 0;
        for (size_t i = 0; i < args_.size(); i++) {
            Operand a = GetBoolean(static_cast<int>(i) + 1, engine, tp);
            if (a.IsErrorOrNone()) return a;
            if (a.BooleanValue()) trueCount++;
        }
        return (trueCount % 2 == 1) ? Operand::True() : Operand::False();
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        for (auto& f : args_) f->GetParameterTypes(noneEngine, result, OperandType::BOOLEAN);
    }
};

}  // namespace

Function NewFunction_Add(std::vector<Function> args) {
    return std::make_shared<Function_Add>(std::move(args));
}
Function NewFunction_Sub(std::vector<Function> args) {
    return std::make_shared<Function_Sub>(std::move(args));
}
Function NewFunction_Mul(std::vector<Function> args) {
    return std::make_shared<Function_Mul>(std::move(args));
}
Function NewFunction_Div(std::vector<Function> args) {
    return std::make_shared<Function_Div>(std::move(args));
}
Function NewFunction_Mod(std::vector<Function> args) {
    return std::make_shared<Function_Mod>(std::move(args));
}
Function NewFunction_Connect(std::vector<Function> args) {
    return std::make_shared<Function_Connect>(std::move(args));
}
Function NewFunction_AND(std::vector<Function> args) {
    return std::make_shared<Function_AND>(std::move(args));
}
Function NewFunction_OR(std::vector<Function> args) {
    return std::make_shared<Function_OR>(std::move(args));
}
Function NewFunction_AND_N(std::vector<Function> args) {
    return std::make_shared<Function_AND_N>(std::move(args));
}
Function NewFunction_OR_N(std::vector<Function> args) {
    return std::make_shared<Function_OR_N>(std::move(args));
}
Function NewFunction_XOR(std::vector<Function> args) {
    return std::make_shared<Function_XOR>(std::move(args));
}

}  // namespace toolgood
