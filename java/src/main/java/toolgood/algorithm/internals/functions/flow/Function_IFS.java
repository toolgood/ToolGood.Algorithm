package toolgood.algorithm.internals.functions.flow;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_N;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_IFS extends Function_N {
    public Function_IFS(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "IFS";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        for (int i = 0; i < funcs.length - 1; i += 2) {
            Operand condition = funcs[i].Evaluate(engine, tempParameter);
            if (condition.IsErrorOrNone()) {
                return condition;
            }
            if (condition.BooleanValue()) {
                return funcs[i + 1].Evaluate(engine, tempParameter);
            }
        }
        return FunctionError();
    }

    @Override
    public OperandType GetResultType() {
        for (int i = 0; i < funcs.length - 1; i += 2) {
            OperandType t = funcs[i + 1].GetResultType();
            if (t != OperandType.NONE) {
                return t;
            }
        }
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        for (int i = 0; i < funcs.length - 1; i += 2) {
            funcs[i].GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
            funcs[i + 1].GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }
}
