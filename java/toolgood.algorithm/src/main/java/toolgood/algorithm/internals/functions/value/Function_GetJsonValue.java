package toolgood.algorithm.internals.functions.value;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.Operand.OperandKeyValueList;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.litJson.JsonData;

public final class Function_GetJsonValue extends Function_2 {

    public Function_GetJsonValue(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "GetJsonValue";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand obj = func1.Evaluate(engine, tempParameter);
        if (obj.IsErrorOrNone()) { return obj; }
        Operand op = func2.Evaluate(engine, tempParameter);
        if (op.IsErrorOrNone()) { return op; }

        if (obj.IsArray()) {
            op = ConvertToNumber(op, 2);
            if (op.IsErrorOrNone()) { return op; }
            int excelIndex = engine.ExcelIndex;
            int index = op.IntValue() - excelIndex;
            if (index < obj.ArrayValue().size() && index >= 0)
                return obj.ArrayValue().get(index);
            return Operand.Error(String.format("Function '%s' ARRAY index %d greater than maximum length!", "GetJsonValue", index));
        }
        if (obj.IsArrayJson()) {
            if (op.IsNumber()) {
                Operand operand = ((OperandKeyValueList)obj).GetValue(op.NumberValue().toString());
                if (operand != null) {
                    return operand;
                }
                return Operand.Error(String.format("Function '%s' Parameter name '%s' is missing!", "GetJsonValue", op.TextValue()));
            } else if (op.IsText()) {
                Operand operand = ((OperandKeyValueList)obj).GetValue(op.TextValue());
                if (operand != null) {
                    return operand;
                }
                return Operand.Error(String.format("Function '%s' Parameter name '%s' is missing!", "GetJsonValue", op.TextValue()));
            }
            return Operand.Error(String.format("Function '%s' Parameter name is missing!", "GetJsonValue"));
        }

        if (obj.IsJson()) {
            JsonData json = obj.JsonValue();
            if (json.IsArray()) {
                op = ConvertToNumber(op, 2);
                if (op.IsErrorOrNone()) { return op; }
                int excelIndex = engine.ExcelIndex;
                int index = op.IntValue() - excelIndex;
                if (index < json.Count() && index >= 0) {
                    return ConvertJsonDataToOperand(json.GetChild(index));
                }
                return Operand.Error(String.format("Function '%s' JSON index %d greater than maximum length!", "GetJsonValue", index));
            } else {
                op = ConvertToText(op, 2);
                if (op.IsErrorOrNone()) { return op; }
                JsonData v = json.GetChild(op.TextValue());
                if (v != null) {
                    return ConvertJsonDataToOperand(v);
                }
            }
        }
        return Operand.Error(String.format("Function '%s' Operator is error!", "GetJsonValue"));
    }

    private static Operand ConvertJsonDataToOperand(JsonData v) {
        if (v.IsString()) return Operand.Create(v.StringValue());
        if (v.IsBoolean()) return Operand.Create(v.BooleanValue());
        if (v.IsDouble()) return Operand.Create(v.NumberValue());
        if (v.IsObject()) return Operand.Create(v);
        if (v.IsArray()) return Operand.Create(v);
        if (v.IsNull()) return Operand.Null;
        return Operand.Create(v);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        func1.ToString(stringBuilder, false);
        stringBuilder.append('[');
        func2.ToString(stringBuilder, false);
        stringBuilder.append(']');
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
    }
}
