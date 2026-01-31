package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
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
            int p = args2.getTextValue().indexOf(args1.getTextValue()) + work.getExcelIndex();
            return Operand.Create(p);
        }
        Operand count = func3.Evaluate(work, tempParameter);
        if (count.IsNotNumber()) {
            count = count.ToNumber("Function '{0}' parameter {1} is error!", "Find", 3);
            if (count.IsError()) {
                return count;
            }
        }
        int p2 = args2.getTextValue().substring(count.getIntValue()).indexOf(args1.getTextValue()) + count.getIntValue() + work.getExcelIndex();
        return Operand.Create(p2);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Find");
    }
}
