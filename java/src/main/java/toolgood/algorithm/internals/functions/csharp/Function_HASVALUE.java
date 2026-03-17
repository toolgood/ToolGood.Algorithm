package toolgood.algorithm.internals.functions.csharp;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.OperandKeyValueList;
import toolgood.algorithm.internals.OperandArray;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_HASVALUE extends Function_2 {
    public Function_HASVALUE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "HasValue";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsError() || args1.IsNone()) {
            return args1;
        }

        Operand args2 = GetText_2(engine, tempParameter);
        if (args2.IsError() || args2.IsNone()) {
            return args2;
        }

        if (args1.IsArrayJson()) {
            return Operand.Create(((OperandKeyValueList) args1).ContainsValue(args2));
        } else if (args1.IsJson()) {
            Object json = args1.JsonValue();
            return Operand.FALSE;
        } else if (args1.IsArray()) {
            OperandArray ar = (OperandArray) args1;
            for (Operand item : ar.ArrayValue()) {
                Operand t = item.ToText();
                if (t.IsError() || t.IsNone()) {
                    continue;
                }
                if (t.TextValue().equals(args2.TextValue())) {
                    return Operand.TRUE;
                }
            }
            return Operand.FALSE;
        }
        return ParameterError(1);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.JSON, null, null);
        func2.GetParameterTypes(noneEngine, result, OperandType.TEXT, null, null);
    }
}
