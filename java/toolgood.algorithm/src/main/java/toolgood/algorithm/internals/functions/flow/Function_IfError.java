package toolgood.algorithm.internals.functions.flow;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

final class Function_IFERROR extends Function_3 {

    public Function_IFERROR(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "IfError";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = func1.Evaluate(engine, tempParameter);
        if (args1.IsNone()) {
            return args1;
        }
        if (args1.IsError()) {
            return func2.Evaluate(engine, tempParameter);
        }
        if (func3 == null) {
            return args1;
        }
        return func3.Evaluate(engine, tempParameter);
    }

    @Override
    public OperandType GetResultType() {
        OperandType t2 = func2.GetResultType();
        if (t2 != OperandType.NONE) {
            return t2;
        }
        if (func3 != null) {
            OperandType t3 = func3.GetResultType();
            if (t3 != OperandType.NONE) {
                return t3;
            }
        }
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NONE);
        func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
        if (func3 != null) {
            func3.GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }

}
