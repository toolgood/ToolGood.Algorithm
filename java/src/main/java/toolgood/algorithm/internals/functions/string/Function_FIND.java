package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_3;

public class Function_FIND extends Function_3 {
    public Function_FIND(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Find", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Find", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        if (func3 == null) {
            int p = args2.TextValue().indexOf(args1.TextValue()) + work.ExcelIndex;
            return Operand.Create(p);
        }
        Operand count = func3.Evaluate(work, tempParameter);
        if (count.IsNotNumber()) {
            count = count.ToNumber("Function '{0}' parameter {1} is error!", "Find", 3);
            if (count.IsError()) {
                return count;
            }
        }
        int p2 = args2.TextValue().substring(count.IntValue()).indexOf(args1.TextValue()) + count.IntValue() + work.ExcelIndex;
        return Operand.Create(p2);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Find");
    }
}
