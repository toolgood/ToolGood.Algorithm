package toolgood.algorithm.internals.functions.operator;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

public final class Function_AND_N extends Function_N {

    public Function_AND_N(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "And";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        boolean b = true;
        for (int i = 0; i < funcs.length; i++) {
            Operand a = GetBoolean(engine, tempParameter, i);
            if (a.IsErrorOrNone()) { return a; }
            if (!a.BooleanValue()) b = false;
        }
        return b ? Operand.True : Operand.False;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        }
    }
}
