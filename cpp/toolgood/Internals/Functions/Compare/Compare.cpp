// Compare 组实现，对应 csharp/.../Internals/Functions/Compare/*
#include "Compare.h"

#include <string>
#include <utility>

#include "../../../AlgorithmEngine.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

enum class CmpKind { EQ, NE, GT, GE, LT, LE };

bool CompareDecimals(const Decimal& a, const Decimal& b, CmpKind k) {
    switch (k) {
        case CmpKind::EQ: return a.Equal(b);
        case CmpKind::NE: return !a.Equal(b);
        case CmpKind::GT: return a.Greater(b);
        case CmpKind::GE: return a.GreaterOrEqual(b);
        case CmpKind::LT: return a.Less(b);
        case CmpKind::LE: return a.LessOrEqual(b);
    }
    return false;
}

bool CompareInts(int64_t a, int64_t b, CmpKind k) {
    switch (k) {
        case CmpKind::EQ: return a == b;
        case CmpKind::NE: return a != b;
        case CmpKind::GT: return a > b;
        case CmpKind::GE: return a >= b;
        case CmpKind::LT: return a < b;
        case CmpKind::LE: return a <= b;
    }
    return false;
}

bool CompareBools(bool a, bool b, CmpKind k) {
    switch (k) {
        case CmpKind::EQ: return a == b;
        case CmpKind::NE: return a != b;
        case CmpKind::GT: return a && !b;
        case CmpKind::GE: return a || !b;
        case CmpKind::LT: return !a && b;
        case CmpKind::LE: return !a || b;
    }
    return false;
}

class CompareFunction : public FunctionN {
public:
    CompareFunction(std::vector<Function> args, CmpKind kind)
        : FunctionN(std::move(args)), kind_(kind) {
        // 构造期不能调用虚函数 Name()，用符号表取名字
        RequireArgs(args_, SymbolOf(kind), 2, 2);
    }

    static const char* SymbolOf(CmpKind kind) {
        switch (kind) {
            case CmpKind::EQ: return "==";
            case CmpKind::NE: return "!=";
            case CmpKind::GT: return ">";
            case CmpKind::GE: return ">=";
            case CmpKind::LT: return "<";
            case CmpKind::LE: return "<=";
        }
        return "==";
    }

    std::string Name() const override {
        switch (kind_) {
            case CmpKind::EQ: return "==";
            case CmpKind::NE: return "!=";
            case CmpKind::GT: return ">";
            case CmpKind::GE: return ">=";
            case CmpKind::LT: return "<";
            case CmpKind::LE: return "<=";
        }
        return "==";
    }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a1 = EvalArg(1, engine, tp);
        if (a1.IsErrorOrNone()) return a1;
        Operand a2 = EvalArg(2, engine, tp);
        if (a2.IsErrorOrNone()) return a2;

        if (a1.Type() == a2.Type()) {
            if (a1.IsNumber()) {
                return Operand::Create(CompareDecimals(a1.NumberValue(), a2.NumberValue(), kind_));
            }
            if (a1.IsText()) {
                if (kind_ == CmpKind::EQ) return Operand::Create(a1.TextValue() == a2.TextValue());
                if (kind_ == CmpKind::NE) return Operand::Create(a1.TextValue() != a2.TextValue());
                const int r = a1.TextValue().compare(a2.TextValue());
                return Operand::Create(CompareInts(r, 0, kind_));
            }
            if (a1.IsBoolean()) {
                if (kind_ == CmpKind::EQ || kind_ == CmpKind::NE) {
                    return Operand::Create(CompareBools(a1.BooleanValue(), a2.BooleanValue(), kind_));
                }
                Operand n1 = a1.ToNumber();
                if (n1.IsError()) return n1;
                Operand n2 = a2.ToNumber();
                if (n2.IsError()) return n2;
                return Operand::Create(CompareDecimals(n1.NumberValue(), n2.NumberValue(), kind_));
            }
            if (a1.IsDate()) {
                return Operand::Create(
                    CompareInts(a1.DateValue().ToLong(), a2.DateValue().ToLong(), kind_));
            }
            if (a1.IsNull()) {
                // null 属于异常数据，两侧同为 null 时无法判断
                return CompareError();
            }
            return CompareError();
        }

        if (a1.IsNull() || a2.IsNull()) {
            // C# 的 != 在「一侧为 null」时返回 true，其余比较运算符返回 false
            return kind_ == CmpKind::NE ? Operand::True() : Operand::False();
        }
        if (a1.IsDate() || a2.IsDate() || a1.IsJson() || a2.IsJson() || a1.IsArray() ||
            a2.IsArray() || a1.IsArrayJson() || a2.IsArrayJson()) {
            return CompareError();
        }

        Operand n1 = ConvertToNumber(a1, 1);
        if (n1.IsErrorOrNone()) return n1;
        Operand n2 = ConvertToNumber(a2, 2);
        if (n2.IsErrorOrNone()) return n2;
        return Operand::Create(CompareDecimals(n1.NumberValue(), n2.NumberValue(), kind_));
    }

    OperandType GetResultType() const override { return OperandType::BOOLEAN; }

    void ToString(std::string& sb, bool) override {
        args_[0]->ToString(sb, true);
        sb += ' ';
        sb += Name();
        sb += ' ';
        args_[1]->ToString(sb, true);
    }

private:
    CmpKind kind_;
};

}  // namespace

Function NewFunction_EQ(std::vector<Function> args) {
    return std::make_shared<CompareFunction>(std::move(args), CmpKind::EQ);
}
Function NewFunction_NE(std::vector<Function> args) {
    return std::make_shared<CompareFunction>(std::move(args), CmpKind::NE);
}
Function NewFunction_GT(std::vector<Function> args) {
    return std::make_shared<CompareFunction>(std::move(args), CmpKind::GT);
}
Function NewFunction_GE(std::vector<Function> args) {
    return std::make_shared<CompareFunction>(std::move(args), CmpKind::GE);
}
Function NewFunction_LT(std::vector<Function> args) {
    return std::make_shared<CompareFunction>(std::move(args), CmpKind::LT);
}
Function NewFunction_LE(std::vector<Function> args) {
    return std::make_shared<CompareFunction>(std::move(args), CmpKind::LE);
}

}  // namespace toolgood
