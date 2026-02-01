package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_GetJsonValue extends Function_2 {
    public Function_GetJsonValue(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand obj = func1.Evaluate(work, tempParameter);
        if (obj.IsError()) {
            return obj;
        }
        Operand op = func2.Evaluate(work, tempParameter);
        if (op.IsError()) {
            return op;
        }

        if (obj.IsArray()) {
            op = op.ToNumber("Function '{0}' parameter {1} is error!", "GetJsonValue", 2);
            if (op.IsError()) {
                return op;
            }
            int index = op.IntValue() - work.ExcelIndex;
            if (index < obj.ArrayValue().size()) {
                return obj.ArrayValue().get(index);
            }
            return Operand.Error("Function '{0}' ARRARY index {1} greater than maximum length!", "GetJsonValue", index);
        }
        if (obj.IsArrayJson()) {
            if (op.IsNumber()) {
                Operand operand;
                if (((OperandKeyValueList)obj).TryGetValue(op.getNumberValue().toString(), operand)) {
                    return operand;
                }
                return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue());
            } else if (op.IsText()) {
                Operand operand;
                if (((OperandKeyValueList)obj).TryGetValue(op.TextValue(), operand)) {
                    return operand;
                }
                return Operand.Error("Function '{0}' Parameter name '{1}' is missing!", "GetJsonValue", op.TextValue());
            }
            return Operand.Error("Function '{0}' Parameter name is missing!", "GetJsonValue");
        }

        if (obj.IsJson()) {
            JsonData json = obj.getJsonValue();
            if (json.IsArray()) {
                op = op.ToNumber("Function '{0}' parameter {1} is error!", "GetJsonValue", 2);
                if (op.IsError()) {
                    return op;
                }
                int index = op.IntValue() - work.ExcelIndex;
                if (index < json.size()) {
                    JsonData v = json.get(index);
                    if (v.isString()) {
                        return Operand.Create(v.getStringValue());
                    }
                    if (v.IsBoolean()) {
                        return Operand.Create(v.BooleanValue());
                    }
                    if (v.isDouble()) {
                        return Operand.Create(v.getNumberValue());
                    }
                    if (v.isObject()) {
                        return Operand.Create(v);
                    }
                    if (v.IsArray()) {
                        return Operand.Create(v);
                    }
                    if (v.isNull()) {
                        return Operand.CreateNull();
                    }
                    return Operand.Create(v);
                }
                return Operand.Error("Function '{0}' JSON index {1} greater than maximum length!", "GetJsonValue", index);
            } else {
                op = op.ToText("Function '{0}' parameter {1} is error!", "GetJsonValue", 2);
                if (op.IsError()) {
                    return op;
                }
                JsonData v = json.get(op.TextValue());
                if (v != null) {
                    if (v.isString()) {
                        return Operand.Create(v.getStringValue());
                    }
                    if (v.IsBoolean()) {
                        return Operand.Create(v.BooleanValue());
                    }
                    if (v.isDouble()) {
                        return Operand.Create(v.getNumberValue());
                    }
                    if (v.isObject()) {
                        return Operand.Create(v);
                    }
                    if (v.IsArray()) {
                        return Operand.Create(v);
                    }
                    if (v.isNull()) {
                        return Operand.CreateNull();
                    }
                    return Operand.Create(v);
                }
            }
        }
        return Operand.Error("Function '{0}' Operator is error!", "GetJsonValue");
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        func1.toString(stringBuilder, false);
        stringBuilder.append('[');
        func2.toString(stringBuilder, false);
        stringBuilder.append(']');
    }
}
