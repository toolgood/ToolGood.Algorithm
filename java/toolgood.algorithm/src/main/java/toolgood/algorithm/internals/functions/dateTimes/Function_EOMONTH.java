package toolgood.algorithm.internals.functions.dateTimes;

import java.util.List;
import org.joda.time.DateTime;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_EOMONTH extends Function_2 {
    public Function_EOMONTH(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires exactly 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "EOMonth";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        Operand args2 = GetNumber_2(engine, tempParameter);
        if (args2.IsErrorOrNone()) { return args2; }
        DateTime dt = args1.DateValue().ToDateTime().plusMonths(args2.IntValue());
        DateTime dt2 = dt.dayOfMonth().withMaximumValue();
        return Operand.Create(dt2);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
