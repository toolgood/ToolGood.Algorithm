package toolgood.algorithm.internals.functions.string;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;

final class Function_CONCATENATE extends Function_N {
    public Function_CONCATENATE(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires at least 1 parameter.");
        }
    }

    @Override
    public String Name() {
        return "Concatenate";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        if (funcs.length == 0) {
            return Operand.Create("");
        }
        if (funcs.length == 1) {
            Operand a = GetText(engine, tempParameter, 0);
            if (a.IsErrorOrNone()) { return a; }
            return a;
        }
        StringBuilder sb = new StringBuilder(funcs.length * 16);
        for (int i = 0; i < funcs.length; i++) {
            Operand a = GetText(engine, tempParameter, i);
            if (a.IsErrorOrNone()) { return a; }
            sb.append(a.TextValue());
        }
        return Operand.Create(sb.toString());
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }

    @Override
    void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.TEXT);
        }
    }
}
