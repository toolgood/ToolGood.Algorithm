package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.Map;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.litJson.JsonData;

final class Function_HASVALUE extends Function_2 {

    public Function_HASVALUE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "HasValue";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        if (args1.IsArrayJson()) {
            return Operand.Create(((Operand.OperandKeyValueList) args1).ContainsValue(args2));
        } else if (args1.IsJson()) {
            JsonData json = args1.JsonValue();
            if (json.IsArray()) {
                for (int i = 0; i < json.Count(); i++) {
                    JsonData v = json.GetChild(i);
                    if (v.IsString()) {
                        if (v.StringValue().equals(args2.TextValue())) {
                            return Operand.True;
                        }
                    } else if (v.IsDouble()) {
                        if (v.NumberValue().toString().equals(args2.TextValue())) {
                            return Operand.True;
                        }
                    } else if (v.IsBoolean()) {
                        if (Boolean.toString(v.BooleanValue()).equalsIgnoreCase(args2.TextValue())) {
                            return Operand.True;
                        }
                    }
                }
            } else {
                for (Map.Entry<String, JsonData> item : json.inst_object.entrySet()) {
                    JsonData v = item.getValue();
                    if (v.IsString()) {
                        if (v.StringValue().equals(args2.TextValue())) {
                            return Operand.True;
                        }
                    } else if (v.IsDouble()) {
                        if (v.NumberValue().toString().equals(args2.TextValue())) {
                            return Operand.True;
                        }
                    } else if (v.IsBoolean()) {
                        if (Boolean.toString(v.BooleanValue()).equalsIgnoreCase(args2.TextValue())) {
                            return Operand.True;
                        }
                    }
                }
            }
            return Operand.False;
        } else if (args1.IsArray()) {
            Operand ar = args1;
            for (Operand item : ar.ArrayValue()) {
                Operand t = item.ToText(null);
                if (t.IsErrorOrNone()) {
                    continue;
                }
                if (t.TextValue().equals(args2.TextValue())) {
                    return Operand.True;
                }
            }
            return Operand.False;
        }
        return ParameterError(1);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.JSON);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
    }

}
