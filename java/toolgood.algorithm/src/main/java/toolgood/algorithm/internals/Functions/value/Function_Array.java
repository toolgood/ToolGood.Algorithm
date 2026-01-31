package toolgood.algorithm.internals.functions.value;

import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.internals.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_N;

public class Function_Array extends Function_N {
    public Function_Array(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>();
        for (FunctionBase item : funcs) {
            Operand aa = item.Evaluate(work, tempParameter);
            if (aa.IsError()) {
                return aa;
            }
            args.add(aa);
        }
        return Operand.Create(args);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "Array");
    }
}
