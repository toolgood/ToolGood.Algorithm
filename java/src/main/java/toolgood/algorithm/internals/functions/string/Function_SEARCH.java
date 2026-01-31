package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_3;

public class Function_SEARCH extends Function_3 {
    public Function_SEARCH(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "Search", 1);
            if (args1.IsError()) {
                return args1;
            }
        }
        Operand args2 = func2.Evaluate(work, tempParameter);
        if (args2.IsNotText()) {
            args2 = args2.ToText("Function '{0}' parameter {1} is error!", "Search", 2);
            if (args2.IsError()) {
                return args2;
            }
        }

        if (func3 == null) {
            int p = args2.getTextValue().toLowerCase().indexOf(args1.getTextValue().toLowerCase()) + work.getExcelIndex();
            return Operand.Create(p);
        }
        Operand args3 = func3.Evaluate(work, tempParameter);
        if (args3.IsNotNumber()) {
            args3 = args3.ToNumber("Function '{0}' parameter {1} is error!", "Search", 3);
            if (args3.IsError()) {
                return args3;
            }
        }
        int startIndex = args3.getIntValue();
        if (startIndex >= args2.getTextValue().length()) {
            return Operand.Create(0);
        }
        int p2 = args2.getTextValue().substring(startIndex).toLowerCase().indexOf(args1.getTextValue().toLowerCase()) + startIndex + work.getExcelIndex();
        return Operand.Create(p2);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Search");
    }
}
