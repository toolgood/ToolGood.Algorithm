package toolgood.algorithm.internals.functions.mathtransformation;

import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.RegexHelper;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_BIN2OCT extends Function_2 {
    public Function_BIN2OCT(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter {1} is error!", "BIN2OCT", 1);
            if (args1.IsError()) {
                return args1;
            }
        }

        if (!RegexHelper.BinRegex.matcher(args1.TextValue()).matches()) {
            return Operand.Error("Function '{0}' parameter {1} is error!", "BIN2OCT", 1);
        }
        String num = Integer.toOctalString(Integer.parseInt(args1.TextValue(), 2));
        if (func2 != null) {
            Operand args2 = func2.Evaluate(work, tempParameter);
            if (args2.IsNotNumber()) {
                args2 = args2.ToNumber("Function '{0}' parameter {1} is error!", "BIN2OCT", 2);
                if (args2.IsError()) {
                    return args2;
                }
            }
            if (num.length() <= args2.IntValue()) {
                return Operand.Create(num);
            }
            return Operand.Error("Function '{0}' parameter {1} is error!", "BIN2OCT", 2);
        }
        return Operand.Create(num);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "BIN2OCT");
    }
}
