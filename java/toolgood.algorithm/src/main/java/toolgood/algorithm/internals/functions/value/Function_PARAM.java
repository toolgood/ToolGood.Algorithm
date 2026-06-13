package toolgood.algorithm.internals.functions.value;

import java.util.List;
import java.util.function.BiFunction;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_PARAM extends Function_2 {

    public Function_PARAM(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException(String.format("Function '%s' requires 1 to 2 parameters.", Name()));
        }
    }

    @Override
    public String Name() {
        return "Param";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetText_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }
        if (tempParameter != null) {
            Operand r = tempParameter.apply(engine, args1.TextValue());
            if (r != null) return r;
        }
        if (func2 != null) {
            return func2.Evaluate(engine, tempParameter);
        }
        return Operand.Error("Parameter '" + args1.TextValue() + "' is missing.");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NONE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NONE);
        }
    }
}
