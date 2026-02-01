package toolgood.algorithm.internals.functions.flow;

import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public class Function_ISERROR extends Function_2 {
    public Function_ISERROR(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(work, tempParameter);
        if (func2 != null) {
            if (args1.IsError()) {
                return func2.Evaluate(work, tempParameter);
            }
            return args1;
        }
        if (args1.IsError()) {
            return Operand.TRUE;
        }
        return Operand.FALSE;
    }

    @Override
    public void toString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "IsError");
    }
}
