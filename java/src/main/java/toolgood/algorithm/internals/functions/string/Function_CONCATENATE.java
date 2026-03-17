package toolgood.algorithm.internals.functions.string;

import java.util.List;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_CONCATENATE extends Function_N {
    public Function_CONCATENATE(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Concatenate";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        if (funcs.length == 0) {
            return Operand.Create("");
        }
        if (funcs.length == 1) {
            Operand a = GetText(engine, tempParameter, 0);
            if (a.IsErrorOrNone()) { return a; }
            return a;
        }
        StringBuilder sb = new StringBuilder();
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
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        for (int i = 0; i < funcs.length; i++) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.TEXT);
        }
    }
}
