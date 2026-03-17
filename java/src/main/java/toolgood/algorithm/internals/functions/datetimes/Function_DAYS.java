package toolgood.algorithm.internals.functions.datetimes;

import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;
import toolgood.algorithm.internals.functions.NoneEngine;

public final class Function_DAYS extends Function_2 {
    public Function_DAYS(FunctionBase func1, FunctionBase func2) {
        super(func1, func2);
    }

    public Function_DAYS(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Days";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) {
            return args1;
        }

        Operand args2 = GetDate_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) {
            return args2;
        }

        long endMillis = args1.DateValue().ToDateTime().withTimeAtStartOfDay().getMillis();
        long startMillis = args2.DateValue().ToDateTime().withTimeAtStartOfDay().getMillis();
        long days = (endMillis - startMillis) / (1000L * 60 * 60 * 24);

        return Operand.Create((int) days);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
    }
}
