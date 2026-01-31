package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_3;

public class Function_MID extends Function_3 {
    public Function_MID(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Mid", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotNumber()) {
            args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "Mid", 2);
            if (args2.IsError()) {
                return args2;
            }
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Mid", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        int start = args2.getIntValue() - work.getExcelIndex();
        int length = args3.getIntValue();
        int end = Math.min(start + length, args1.getTextValue().length());
        if (start < 0) {
            start = 0;
        }
        if (start >= args1.getTextValue().length()) {
            return Operand.Create("");
        }
        return Operand.Create(args1.getTextValue().substring(start, end));
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Mid");
    }
}
