package toolgood.algorithm.internals.functions.mathtransformation;

import toolgood.algorithm.internals.functions.Function_1;
import toolgood.algorithm.internals.RegexHelper;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;

public class Function_BIN2DEC extends Function_1 {
    public Function_BIN2DEC(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.IsNotText()) {
            args1 = args1.ToText("Function '{0}' parameter is error!", "BIN2DEC");
            if (args1.IsError()) {
                return args1;
            }
        }

        if (!RegexHelper.BinRegex.matcher(args1.TextValue()).matches()) {
            return Operand.Error("Function '{0}' parameter is error!", "BIN2DEC");
        }
        int num = Integer.parseInt(args1.TextValue(), 2);
        return Operand.Create(num);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "BIN2DEC");
    }
}
