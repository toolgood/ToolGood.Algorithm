package toolgood.algorithm.internals.functions.operator;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_N;

public class Function_OR_N extends Function_N {
    public Function_OR_N(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        int index = 1;
        boolean b = false;
        for (FunctionBase item : funcs) {
            Operand a = item.Evaluate(work, tempParameter);
            if (a.IsNotBoolean()) {
                a = a.ToBoolean("Function '{0}' parameter {1} is error!", "OR", index++);
                if (a.IsError()) {
                    return a;
                }
            }
            if (a.getBooleanValue()) {
                b = true;
            }
        }
        return b ? Operand.True : Operand.False;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "OR");
    }
}
