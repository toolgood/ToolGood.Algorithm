package toolgood.algorithm.internals.functions.operator;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_N;

public class Function_AND_N extends Function_N {
    public Function_AND_N(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        int index = 1;
        boolean b = true;
        for (FunctionBase item : funcs) {
            Operand a = item.Evaluate(work, tempParameter);
            if (a.IsNotBoolean()) {
                a = a.ToBoolean("Function '{0}' parameter {1} is error!", "AND", index++);
                if (a.IsError()) {
                    return a;
                }
            }
            if (a.getBooleanValue() == false) {
                b = false;
            }
        }
        return b ? Operand.True : Operand.False;
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "AND");
    }
}
