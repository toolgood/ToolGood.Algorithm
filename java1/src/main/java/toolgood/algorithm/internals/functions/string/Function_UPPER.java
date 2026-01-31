package toolgood.algorithm.internals.functions.string;

import toolgood.algorithm.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_1;

public class Function_UPPER extends Function_1 {
    public Function_UPPER(FunctionBase func1) {
        super(func1);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (args1.isNotText()) {
            args1 = args1.toText("Function '{0}' parameter is error!", "Upper");
            if (args1.isError()) {
                return args1;
            }
        }
        return Operand.create(args1.getTextValue().toUpperCase());
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        addFunction(stringBuilder, "Upper");
    }
}
