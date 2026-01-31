package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.Function_2;
import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_HASVALUE extends Function_2 {
    public Function_HASVALUE(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsError()) {
            return args1;
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "HasValue", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        if (args1.IsArrayJson()) {
            return Operand.Create(((toolgood.algorithm.internals.OperandKeyValueList) args1).ContainsValue(args2));
        } else if (args1.IsJson()) {
            Object json = args1.getJsonValue();
            // 这里需要根据实际的 JSON 实现进行调整
            // 假设使用了某种 JSON 库，如 Gson 或 Jackson
            // 此处为简化实现，实际需要根据具体的 JSON 处理方式进行修改
            return Operand.False();
        } else if (args1.IsArray()) {
            toolgood.algorithm.internals.OperandArray ar = (toolgood.algorithm.internals.OperandArray) args1;
            for (Operand item : ar.getArrayValue()) {
                Operand t = item.ToText();
                if (t.IsError()) {
                    continue;
                }
                if (t.getTextValue().equals(args2.getTextValue())) {
                    return Operand.True();
                }
            }
            return Operand.False();
        }
        return Operand.Error("Function '{0}' parameter {1} is error!", "HasValue", 1);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "HasValue");
    }
}
