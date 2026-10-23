// Value 组实现，对应 csharp/.../Internals/Functions/Value/*
#include "Value.h"

#include <map>
#include <stdexcept>
#include <string>
#include <utility>

#include "../../../AlgorithmEngine.h"
#include "../../../LitJson/JsonData.h"
#include "../../../Operands/Operand.h"

namespace toolgood {

namespace {

// ---------------- 常量节点 ----------------

class Function_Number_Impl : public FunctionBase {
public:
    explicit Function_Number_Impl(const Decimal& v) : value_(v) {}
    std::string Name() const override { return "Num"; }
    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override { return Operand::Create(value_); }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
    void ToString(std::string& sb, bool) override { sb += value_.ToString(); }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}

private:
    Decimal value_;
};

/// 数字单位换算（对应 Function_Number2.TransformationUnit）
Decimal TransformationUnit(const Decimal& src, NumberUnitType type1, DistanceUnitType distance,
                           AreaUnitType area, VolumeUnitType volume, MassUnitType mass) {
    auto mul = [&src](const char* factor) {
        return src.Mul(Decimal::FromString(factor));
    };
    if (type1 == NumberUnitType::MM) {
        switch (distance) {
            case DistanceUnitType::MM: return src;
            case DistanceUnitType::CM: return mul("0.1");
            case DistanceUnitType::DM: return mul("0.01");
            case DistanceUnitType::M: return mul("0.001");
            case DistanceUnitType::KM: return mul("0.000001");
        }
    } else if (type1 == NumberUnitType::CM) {
        switch (distance) {
            case DistanceUnitType::MM: return mul("10");
            case DistanceUnitType::CM: return src;
            case DistanceUnitType::DM: return mul("0.1");
            case DistanceUnitType::M: return mul("0.01");
            case DistanceUnitType::KM: return mul("0.00001");
        }
    } else if (type1 == NumberUnitType::DM) {
        switch (distance) {
            case DistanceUnitType::MM: return mul("100");
            case DistanceUnitType::CM: return mul("10");
            case DistanceUnitType::DM: return src;
            case DistanceUnitType::M: return mul("0.1");
            case DistanceUnitType::KM: return mul("0.0001");
        }
    } else if (type1 == NumberUnitType::M) {
        switch (distance) {
            case DistanceUnitType::MM: return mul("1000");
            case DistanceUnitType::CM: return mul("100");
            case DistanceUnitType::DM: return mul("10");
            case DistanceUnitType::M: return src;
            case DistanceUnitType::KM: return mul("0.001");
        }
    } else if (type1 == NumberUnitType::KM) {
        switch (distance) {
            case DistanceUnitType::MM: return mul("1000000");
            case DistanceUnitType::CM: return mul("100000");
            case DistanceUnitType::DM: return mul("10000");
            case DistanceUnitType::M: return mul("1000");
            case DistanceUnitType::KM: return src;
        }
    } else if (type1 == NumberUnitType::MM2) {
        switch (area) {
            case AreaUnitType::MM2: return src;
            case AreaUnitType::CM2: return mul("0.01");
            case AreaUnitType::DM2: return mul("0.0001");
            case AreaUnitType::M2: return mul("0.000001");
            case AreaUnitType::KM2: return mul("0.000000000001");
        }
    } else if (type1 == NumberUnitType::CM2) {
        switch (area) {
            case AreaUnitType::MM2: return mul("100");
            case AreaUnitType::CM2: return src;
            case AreaUnitType::DM2: return mul("0.01");
            case AreaUnitType::M2: return mul("0.0001");
            case AreaUnitType::KM2: return mul("0.0000000001");
        }
    } else if (type1 == NumberUnitType::DM2) {
        switch (area) {
            case AreaUnitType::MM2: return mul("10000");
            case AreaUnitType::CM2: return mul("100");
            case AreaUnitType::DM2: return src;
            case AreaUnitType::M2: return mul("0.01");
            case AreaUnitType::KM2: return mul("0.00000001");
        }
    } else if (type1 == NumberUnitType::M2) {
        switch (area) {
            case AreaUnitType::MM2: return mul("1000000");
            case AreaUnitType::CM2: return mul("10000");
            case AreaUnitType::DM2: return mul("100");
            case AreaUnitType::M2: return src;
            case AreaUnitType::KM2: return mul("0.000001");
        }
    } else if (type1 == NumberUnitType::KM2) {
        switch (area) {
            case AreaUnitType::MM2: return mul("1000000000000");
            case AreaUnitType::CM2: return mul("10000000000");
            case AreaUnitType::DM2: return mul("100000000");
            case AreaUnitType::M2: return mul("1000000");
            case AreaUnitType::KM2: return src;
        }
    } else if (type1 == NumberUnitType::MM3) {
        switch (volume) {
            case VolumeUnitType::MM3: return src;
            case VolumeUnitType::CM3: return mul("0.001");
            case VolumeUnitType::DM3: return mul("0.000001");
            case VolumeUnitType::M3: return mul("0.000000001");
            case VolumeUnitType::KM3: return mul("0.000000000000000001");
        }
    } else if (type1 == NumberUnitType::CM3) {
        switch (volume) {
            case VolumeUnitType::MM3: return mul("1000");
            case VolumeUnitType::CM3: return src;
            case VolumeUnitType::DM3: return mul("0.001");
            case VolumeUnitType::M3: return mul("0.000001");
            case VolumeUnitType::KM3: return mul("0.000000000000001");
        }
    } else if (type1 == NumberUnitType::DM3) {
        switch (volume) {
            case VolumeUnitType::MM3: return mul("1000000");
            case VolumeUnitType::CM3: return mul("1000");
            case VolumeUnitType::DM3: return src;
            case VolumeUnitType::M3: return mul("0.001");
            case VolumeUnitType::KM3: return mul("0.000000000001");
        }
    } else if (type1 == NumberUnitType::M3) {
        switch (volume) {
            case VolumeUnitType::MM3: return mul("1000000000");
            case VolumeUnitType::CM3: return mul("1000000");
            case VolumeUnitType::DM3: return mul("1000");
            case VolumeUnitType::M3: return src;
            case VolumeUnitType::KM3: return mul("0.000000001");
        }
    } else if (type1 == NumberUnitType::KM3) {
        switch (volume) {
            case VolumeUnitType::MM3: return mul("1000000000000000000");
            case VolumeUnitType::CM3: return mul("1000000000000000");
            case VolumeUnitType::DM3: return mul("1000000000000");
            case VolumeUnitType::M3: return mul("1000000000");
            case VolumeUnitType::KM3: return src;
        }
    } else if (type1 == NumberUnitType::G) {
        switch (mass) {
            case MassUnitType::G: return src;
            case MassUnitType::KG: return mul("0.001");
            case MassUnitType::T: return mul("0.000001");
        }
    } else if (type1 == NumberUnitType::KG) {
        switch (mass) {
            case MassUnitType::G: return mul("1000");
            case MassUnitType::KG: return src;
            case MassUnitType::T: return mul("0.001");
        }
    } else if (type1 == NumberUnitType::T) {
        switch (mass) {
            case MassUnitType::G: return mul("1000000");
            case MassUnitType::KG: return mul("1000");
            case MassUnitType::T: return src;
        }
    }
    throw std::runtime_error("Number unit is error!");
}

const std::map<std::string, NumberUnitType>& UnitDict() {
    static const std::map<std::string, NumberUnitType> dict = {
        {"KM", NumberUnitType::KM},   {"M", NumberUnitType::M},     {"DM", NumberUnitType::DM},
        {"CM", NumberUnitType::CM},   {"MM", NumberUnitType::MM},   {"KM2", NumberUnitType::KM2},
        {"M2", NumberUnitType::M2},   {"DM2", NumberUnitType::DM2}, {"CM2", NumberUnitType::CM2},
        {"MM2", NumberUnitType::MM2}, {"M3", NumberUnitType::M3},   {"DM3", NumberUnitType::DM3},
        {"CM3", NumberUnitType::CM3}, {"MM3", NumberUnitType::MM3}, {"KM3", NumberUnitType::KM3},
        {"L", NumberUnitType::DM3},   {"ML", NumberUnitType::CM3},  {"T", NumberUnitType::T},
        {"KG", NumberUnitType::KG},   {"G", NumberUnitType::G},
    };
    return dict;
}

class Function_Number2_Impl : public FunctionBase {
public:
    Function_Number2_Impl(const Decimal& d, std::string unit) : d_(d), unit_(std::move(unit)) {}
    std::string Name() const override { return "Num"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter*) override {
        auto it = UnitDict().find(unit_);
        if (it == UnitDict().end()) {
            return Operand::ErrorFormat("Number unit '{0}' is invalid!", {unit_});
        }
        Decimal d2 = TransformationUnit(d_, it->second, engine.DistanceUnit, engine.AreaUnit,
                                        engine.VolumeUnit, engine.MassUnit);
        if (d2.Abs().Greater(Decimal::Max)) return NumError();
        return Operand::Create(d2);
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
    void ToString(std::string& sb, bool) override {
        sb += d_.ToString();
        sb += unit_;
    }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}

private:
    Decimal d_;
    std::string unit_;
};

class Function_Parameter_Impl : public FunctionBase {
public:
    explicit Function_Parameter_Impl(std::string name) : name_(std::move(name)) {}
    std::string Name() const override { return "Parameter"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        if (tp != nullptr && *tp) {
            Operand r = (*tp)(engine, name_);
            if (r.IsValid()) return r;
        }
        return engine.GetParameter(name_);
    }
    OperandType GetResultType() const override { return OperandType::NONE; }
    void ToString(std::string& sb, bool) override { sb += name_; }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>& result, OperandType operandType,
                           const std::string& op, const std::string& val) override {
        ParameterType pt;
        pt.Name = name_;
        pt.Type = operandType;
        pt.Operator = op;
        pt.Value = val;
        result.push_back(pt);
    }

private:
    std::string name_;
};

class Function_ValueText_Impl : public FunctionBase {
public:
    explicit Function_ValueText_Impl(std::string text) : text_(std::move(text)) {}
    std::string Name() const override { return "Value"; }
    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override {
        return Operand::Create(text_);
    }
    OperandType GetResultType() const override { return OperandType::TEXT; }
    void ToString(std::string& sb, bool) override {
        sb += '"';
        for (char c : text_) {
            switch (c) {
                case '\\': sb += "\\\\"; break;
                case '"': sb += "\\\""; break;
                case '\r': sb += "\\r"; break;
                case '\n': sb += "\\n"; break;
                case '\t': sb += "\\t"; break;
                case '\0': sb += "\\0"; break;
                case '\v': sb += "\\v"; break;
                case '\a': sb += "\\a"; break;
                case '\b': sb += "\\b"; break;
                case '\f': sb += "\\f"; break;
                default: sb += c; break;
            }
        }
        sb += '"';
    }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}

private:
    std::string text_;
};

class Function_ValueBoolean_Impl : public FunctionBase {
public:
    explicit Function_ValueBoolean_Impl(bool v) : value_(v) {}
    std::string Name() const override { return value_ ? "True" : "False"; }
    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override {
        return value_ ? Operand::True() : Operand::False();
    }
    OperandType GetResultType() const override { return OperandType::BOOLEAN; }
    void ToString(std::string& sb, bool) override { sb += Name(); }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}

private:
    bool value_;
};

class Function_Version_Impl : public FunctionBase {
public:
    std::string Name() const override { return "ALGORITHMVERSION"; }
    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override { return Operand::Version(); }
    OperandType GetResultType() const override { return OperandType::TEXT; }
    void ToString(std::string& sb, bool) override { sb += Name(); }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}
};

class Function_NULL_Impl : public FunctionBase {
public:
    std::string Name() const override { return "NULL"; }
    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override { return Operand::Null(); }
    OperandType GetResultType() const override { return OperandType::NULL_VALUE; }
    void ToString(std::string& sb, bool) override { sb += "NULL"; }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}
};

// ---------------- 表达式节点（FunctionN） ----------------

class Function_ARRAY : public FunctionN {
public:
    explicit Function_ARRAY(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Array", 1, -1);
    }
    std::string Name() const override { return "Array"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> values;
        Operand error = EvaluateAll(engine, tp, values);
        if (!error.IsNull()) return error;
        return Operand::Create(std::move(values));
    }
    OperandType GetResultType() const override { return OperandType::ARRAY; }
};

class Function_ArrayJson : public FunctionN {
public:
    explicit Function_ArrayJson(std::vector<Function> args) : FunctionN(std::move(args)) {}
    std::string Name() const override { return "ArrayJson"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        auto list = std::make_shared<OperandKeyValueListImpl>();
        for (auto& f : args_) {
            Operand o = f->Evaluate(engine, tp);
            if (o.IsErrorOrNone()) return o;
            const KeyValue* kv = o.AsKeyValue();
            if (kv == nullptr) return ParameterError(1);
            list->AddValue(*kv);
        }
        return Operand(list);
    }
    OperandType GetResultType() const override { return OperandType::ARRAYJSON; }
    void ToString(std::string& sb, bool) override {
        sb += '{';
        for (size_t i = 0; i < args_.size(); i++) {
            if (i > 0) sb += ", ";
            args_[i]->ToString(sb, false);
        }
        sb += '}';
    }
};

class Function_ArrayJsonItem : public FunctionBase {
public:
    Function_ArrayJsonItem(std::string key, Function value)
        : key_(std::move(key)), value_(std::move(value)) {}
    std::string Name() const override { return "ArrayJsonItem"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand v = value_->Evaluate(engine, tp);
        if (v.IsErrorOrNone()) return v;
        KeyValue kv;
        kv.Key = key_;
        kv.Value = v;
        return CreateKeyValueOperand(std::move(kv));
    }
    OperandType GetResultType() const override { return OperandType::ARRAYJSON; }
    void ToString(std::string& sb, bool) override {
        sb += Operand::Create(key_).ToString();
        sb += ':';
        value_->ToString(sb, false);
    }
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        value_->GetParameterTypes(noneEngine, result, OperandType::NONE);
    }

private:
    std::string key_;
    Function value_;
};

class Function_DiyFunction : public FunctionN {
public:
    Function_DiyFunction(std::string name, std::vector<Function> args)
        : FunctionN(std::move(args)), funName_(std::move(name)) {}
    std::string Name() const override { return "DiyFunction"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        std::vector<Operand> values;
        Operand error = EvaluateAll(engine, tp, values);
        if (!error.IsNull()) return error;
        try {
            return engine.ExecuteDiyFunction(funName_, values);
        } catch (const std::exception& ex) {
            engine.LastError = ex.what();
            return Operand::Error(ex.what());
        }
    }
    OperandType GetResultType() const override { return OperandType::NONE; }
    void ToString(std::string& sb, bool) override {
        sb += funName_;
        sb += '(';
        for (size_t i = 0; i < args_.size(); i++) {
            if (i > 0) sb += ", ";
            args_[i]->ToString(sb, false);
        }
        sb += ')';
    }

private:
    std::string funName_;
};

class Function_ERROR : public FunctionN {
public:
    explicit Function_ERROR(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Error", 1, 1);
    }
    std::string Name() const override { return "Error"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a = GetText(1, engine, tp);
        if (a.IsErrorOrNone()) return a;
        return Operand::Error(a.TextValue());
    }
    OperandType GetResultType() const override { return OperandType::ERROR; }
};

class Function_JSON : public FunctionN {
public:
    explicit Function_JSON(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Json", 1, 1);
    }
    std::string Name() const override { return "Json"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a = EvalArg(1, engine, tp);
        if (a.IsErrorOrNone()) return a;
        if (a.IsJson()) return a;
        if (a.IsArrayJson()) a = a.ToText();
        if (!a.IsText()) return ParameterError(1);
        const std::string& txt = a.TextValue();
        if (txt.size() >= 2) {
            char c0 = txt[0], c1 = txt[txt.size() - 1];
            if ((c0 == '{' && c1 == '}') || (c0 == '[' && c1 == ']')) {
                JsonData json;
                if (JsonMapper::TryParse(txt, json)) return Operand::Create(json);
            }
        }
        return ParameterError(1);
    }
    OperandType GetResultType() const override { return OperandType::JSON; }
};

class Function_GetJsonValue : public FunctionN {
public:
    explicit Function_GetJsonValue(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "GetJsonValue", 2, 2);
    }
    std::string Name() const override { return "GetJsonValue"; }

    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand obj = EvalArg(1, engine, tp);
        if (obj.IsErrorOrNone()) return obj;
        Operand op = EvalArg(2, engine, tp);
        if (op.IsErrorOrNone()) return op;

        if (obj.IsArray()) {
            op = ConvertToNumber(op, 2);
            if (op.IsErrorOrNone()) return op;
            const int index = op.IntValue() - engine.ExcelIndex;
            std::vector<Operand> arr = obj.ArrayValue();
            if (index >= 0 && index < static_cast<int>(arr.size())) return arr[static_cast<size_t>(index)];
            return Operand::ErrorFormat("Function '{0}' ARRAY index {1} out of range!",
                                        {"GetJsonValue", std::to_string(index)});
        }
        if (obj.IsArrayJson()) {
            OperandKeyValueListImpl* kvl = obj.AsKeyValueList();
            if (kvl == nullptr) {
                return Operand::ErrorFormat("Function '{0}' Operator is error!", {"GetJsonValue"});
            }
            if (op.IsNumber()) {
                std::string key = op.NumberValue().ToString();
                Operand v;
                if (kvl->TryGetValue(key, v)) return v;
                return Operand::ErrorFormat("Function '{0}' Parameter name '{1}' is missing!",
                                            {"GetJsonValue", key});
            }
            if (op.IsText()) {
                Operand v;
                if (kvl->TryGetValue(op.TextValue(), v)) return v;
                return Operand::ErrorFormat("Function '{0}' Parameter name '{1}' is missing!",
                                            {"GetJsonValue", op.TextValue()});
            }
            return Operand::ErrorFormat("Function '{0}' Parameter name is missing!",
                                        {"GetJsonValue"});
        }
        if (obj.IsJson()) {
            const JsonData* json = obj.JsonValue();
            if (json == nullptr) {
                return Operand::ErrorFormat("Function '{0}' Operator is error!", {"GetJsonValue"});
            }
            if (json->IsArray()) {
                op = ConvertToNumber(op, 2);
                if (op.IsErrorOrNone()) return op;
                const int index = op.IntValue() - engine.ExcelIndex;
                if (index >= 0 && index < json->Count()) return JsonToOperand((*json)[index]);
                return Operand::ErrorFormat("Function '{0}' JSON index {1} out of range!",
                                            {"GetJsonValue", std::to_string(index)});
            }
            op = ConvertToText(op, 2);
            if (op.IsErrorOrNone()) return op;
            const JsonData* v = json->GetChild(op.TextValue());
            if (v != nullptr) return JsonToOperand(*v);
        }
        return Operand::ErrorFormat("Function '{0}' Operator is error!", {"GetJsonValue"});
    }

    OperandType GetResultType() const override { return OperandType::NONE; }
    void ToString(std::string& sb, bool) override {
        args_[0]->ToString(sb, false);
        sb += '[';
        args_[1]->ToString(sb, false);
        sb += ']';
    }

private:
    static Operand JsonToOperand(const JsonData& v) {
        if (v.IsString()) return Operand::Create(v.StringValue());
        if (v.IsBoolean()) return Operand::Create(v.BooleanValue());
        if (v.IsDouble()) return Operand::Create(v.NumberValue());
        if (v.IsObject() || v.IsArray()) return Operand::Create(v);
        if (v.IsNull()) return Operand::Null();
        return Operand::Create(v);
    }
};

class Function_PARAM : public FunctionN {
public:
    explicit Function_PARAM(std::vector<Function> args) : FunctionN(std::move(args)) {
        RequireArgs(args_, "Param", 1, 2);
    }
    std::string Name() const override { return "Param"; }
    Operand Evaluate(AlgorithmEngine& engine, const TempParameter* tp) override {
        Operand a = GetText(1, engine, tp);
        if (a.IsErrorOrNone()) return a;
        if (tp != nullptr && *tp) {
            Operand r = (*tp)(engine, a.TextValue());
            if (r.IsValid()) return r;
        }
        Operand result = engine.GetParameter(a.TextValue());
        if (result.IsErrorOrNone() && args_.size() >= 2) {
            return args_[1]->Evaluate(engine, tp);
        }
        return result;
    }
    OperandType GetResultType() const override { return OperandType::NONE; }
    void GetParameterTypes(NoneEngine& noneEngine, std::vector<ParameterType>& result,
                           OperandType, const std::string&, const std::string&) override {
        args_[0]->GetParameterTypes(noneEngine, result, OperandType::TEXT);
        if (args_.size() >= 2) args_[1]->GetParameterTypes(noneEngine, result, OperandType::NONE);
    }
};

class Function_Number_E_Impl : public FunctionBase {
public:
    std::string Name() const override { return "E"; }
    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override {
        static const Operand v = Operand::Create(Decimal::FromString("2.7182818284590452353602874714"));
        return v;
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
    void ToString(std::string& sb, bool) override { sb += "E()"; }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}
};

class Function_Number_PI_Impl : public FunctionBase {
public:
    std::string Name() const override { return "PI"; }
    Operand Evaluate(AlgorithmEngine&, const TempParameter*) override {
        static const Operand v = Operand::Create(Decimal::FromString("3.1415926535897932384626433833"));
        return v;
    }
    OperandType GetResultType() const override { return OperandType::NUMBER; }
    void ToString(std::string& sb, bool) override { sb += "PI()"; }
    void GetParameterTypes(NoneEngine&, std::vector<ParameterType>&, OperandType, const std::string&,
                           const std::string&) override {}
};

}  // namespace

// ---------------- 工厂 ----------------

Function NewFunction_ARRAY(std::vector<Function> args) {
    return std::make_shared<Function_ARRAY>(std::move(args));
}
Function NewFunction_ArrayJson(std::vector<Function> args) {
    return std::make_shared<Function_ArrayJson>(std::move(args));
}
Function NewFunction_ERROR(std::vector<Function> args) {
    return std::make_shared<Function_ERROR>(std::move(args));
}
Function NewFunction_GetJsonValue(std::vector<Function> args) {
    return std::make_shared<Function_GetJsonValue>(std::move(args));
}
Function NewFunction_JSON(std::vector<Function> args) {
    return std::make_shared<Function_JSON>(std::move(args));
}
Function NewFunction_PARAM(std::vector<Function> args) {
    return std::make_shared<Function_PARAM>(std::move(args));
}
Function NewFunction_ArrayJsonItem(const std::string& key, Function value) {
    return std::make_shared<Function_ArrayJsonItem>(key, std::move(value));
}
Function NewFunction_DiyFunction(const std::string& name, std::vector<Function> args) {
    return std::make_shared<Function_DiyFunction>(name, std::move(args));
}
Function NewFunction_Number(const Decimal& value) {
    return std::make_shared<Function_Number_Impl>(value);
}
Function NewFunction_Number2(const Decimal& value, const std::string& unit) {
    return std::make_shared<Function_Number2_Impl>(value, unit);
}
Function NewFunction_Number_E() { return std::make_shared<Function_Number_E_Impl>(); }
Function NewFunction_Number_PI() { return std::make_shared<Function_Number_PI_Impl>(); }
Function NewFunction_Parameter(const std::string& name) {
    return std::make_shared<Function_Parameter_Impl>(name);
}
Function NewFunction_ValueBoolean(bool value) {
    return std::make_shared<Function_ValueBoolean_Impl>(value);
}
Function NewFunction_ValueText(const std::string& text) {
    return std::make_shared<Function_ValueText_Impl>(text);
}
Function NewFunction_NULL() { return std::make_shared<Function_NULL_Impl>(); }
Function NewFunction_Version() { return std::make_shared<Function_Version_Impl>(); }

}  // namespace toolgood
