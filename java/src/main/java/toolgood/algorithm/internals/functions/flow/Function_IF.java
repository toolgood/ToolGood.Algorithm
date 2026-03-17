package toolgood.algorithm.internals.functions.flow;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_IF extends Function_3 {
    public Function_IF(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "If";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
		Operand args1 = GetBoolean_1(engine, tempParameter);
		if(args1.IsErrorOrNone()) { return args1; }
        if (args1.BooleanValue()) {
            return func2.Evaluate(engine, tempParameter);
        }
        if (func3 == null) {
            return Operand.False;
        }
        return func3.Evaluate(engine, tempParameter);
    }

    @Override
    public OperandType GetResultType() {
        if (func2 != null) {
            OperandType t = func2.GetResultType();
            if (t != OperandType.NONE) {
                return t;
            }
        }
        if (func3 != null) {
            return func3.GetResultType();
        }
        return OperandType.BOOLEAN;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }
}
