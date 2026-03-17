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
import toolgood.algorithm.operands.MyDate;

public final class Function_TIMESTAMP extends Function_2 {
    public Function_TIMESTAMP(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "Timestamp";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        int type = 0;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) {
                return args2;
            }
            type = args2.IntValue();
        }

        Operand args0 = GetDate_1(engine, tempParameter);
        if (args0.IsErrorOrNone()) {
            return args0;
        }

        MyDate myDate = args0.DateValue();
        long timestamp = myDate.ToDateTime().getTime();

        if (type == 0) {
            return Operand.Create(timestamp);
        } else if (type == 1) {
            return Operand.Create((double) timestamp / 1000);
        }
        return ParameterError(2);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
