package toolgood.algorithm.internals.functions.operator;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_XOR extends Function_N {
    public Function_XOR(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Xor";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        int trueCount = 0;
        for (int i = 0; i < funcs.length; i++) {
            Operand a = GetBoolean(engine, tempParameter, i);
            if (a.IsErrorOrNone()) { return a; }
            if (a.BooleanValue()) trueCount++;
        }
        return (trueCount % 2 == 1) ? Operand.True : Operand.False;
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
