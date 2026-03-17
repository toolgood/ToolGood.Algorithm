package toolgood.algorithm.internals.functions.value;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.operands.OperandKeyValueList;

public final class Function_GetJsonValue extends Function_2 {

    public Function_GetJsonValue(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public String Name() {
        return "GetJsonValue";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand obj = func1.Evaluate(engine, tempParameter);
        if (obj.IsErrorOrNone()) {
            return obj;
        }
        Operand op = func2.Evaluate(engine, tempParameter);
        if (op.IsErrorOrNone()) {
            return op;
        }

        if (obj.IsArray()) {
            op = ConvertToNumber(op, 2);
            if (op.IsErrorOrNone()) {
                return op;
            }
            int index = op.IntValue() - engine.ExcelIndex;
            if (index < obj.ArrayValue().size() && index >= 0) {
                return obj.ArrayValue().get(index);
            }
            return Operand.Error("Function '{0}' ARRARY index {1} greater than maximum length!", "GetJsonValue", index);
        }
        if (obj.IsArrayJson()) {
            Operand[] operandArr = new Operand[1];
            if (op.IsNumber()) {
                if (((OperandKeyValueList) obj).TryGetValue(op.NumberValue().toString(), operandArr)) {
                    return operandArr[0];
                }
                return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue());
            } else if (op.IsText()) {
                if (((OperandKeyValueList) obj).TryGetValue(op.TextValue(), operandArr)) {
                    return operandArr[0];
                }
                return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue());
            }
            return Operand.Error("Function '{0}' Parameter name is missing!", "GetJsonValue");
        }

        if (obj.IsJson()) {
            JsonData json = obj.JsonValue();
            if (json.IsArray()) {
                op = ConvertToNumber(op, 2);
                if (op.IsErrorOrNone()) {
                    return op;
                }
                int index = op.IntValue() - engine.ExcelIndex;
                if (index < json.inst_array.size() && index >= 0) {
                    return ConvertJsonDataToOperand(json.inst_array.get(index));
                }
                return Operand.Error("Function '{0}' JSON index {1} greater than maximum length!", "GetJsonValue", index);
            } else {
                op = ConvertToText(op, 2);
                if (op.IsErrorOrNone()) {
                    return op;
                }
                JsonData v = json.get(op.TextValue());
                if (v != null) {
                    return ConvertJsonDataToOperand(v);
                }
            }
        }
        return Operand.Error("Function '{0}' Operator is error!", "GetJsonValue");
    }

    private static Operand ConvertJsonDataToOperand(JsonData v) {
        if (v.IsString()) {
            return Operand.Create(v.StringValue());
        }
        if (v.IsBoolean()) {
            return Operand.Create(v.BooleanValue());
        }
        if (v.IsDouble()) {
            return Operand.Create(v.NumberValue());
        }
        if (v.IsObject()) {
            return Operand.Create(v);
        }
        if (v.IsArray()) {
            return Operand.Create(v);
        }
        if (v.IsNull()) {
            return Operand.Null;
        }
        return Operand.Create(v);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        func1.toString(stringBuilder, false);
        stringBuilder.append('[');
        func2.toString(stringBuilder, false);
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
