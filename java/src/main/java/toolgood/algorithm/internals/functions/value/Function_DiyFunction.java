package toolgood.algorithm.internals.functions.value;

import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.internals.functions.Function_N;

public class Function_DiyFunction extends Function_N {
    private final String funName;

    public Function_DiyFunction(String name, FunctionBase[] funcs) {
        super(funcs);
        this.funName = name;
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        List<Operand> args = new ArrayList<>();
        for (FunctionBase item : funcs) {
            Operand aa = item.Evaluate(work, tempParameter);
            args.add(aa);
        }
        return work.ExecuteDiyFunction(funName, args);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, funName);
    }
}
