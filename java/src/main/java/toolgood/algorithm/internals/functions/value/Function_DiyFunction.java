package toolgood.algorithm.internals.functions.value;

import java.util.ArrayList;
import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_DiyFunction extends Function_N {
    private final String funName;

    public Function_DiyFunction(String name, FunctionBase[] funcs) {
        super(funcs);
        this.funName = name;
    }

    @Override
    public String Name() {
        return "DiyFunction";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        List<Operand> args = new ArrayList<>(funcs.length);
        for (FunctionBase item : funcs) {
            Operand aa = item.Evaluate(engine, tempParameter);
            args.add(aa);
        }
        return engine.ExecuteDiyFunction(funName, args);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, funName);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }
}
