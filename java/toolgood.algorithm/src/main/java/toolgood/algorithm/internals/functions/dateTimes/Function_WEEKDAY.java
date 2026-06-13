package toolgood.algorithm.internals.functions.dateTimes;

import java.util.List;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_2;

public final class Function_WEEKDAY extends Function_2 {
    public Function_WEEKDAY(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Weekday";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand args1 = GetDate_1(engine, tempParameter);
        if (args1.IsErrorOrNone()) { return args1; }

        int type = 1;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) { return args2; }
            type = args2.IntValue();
            if (type != 1 && type != 2 && type != 3 && (type < 11 || type > 17)) {
                return ParameterError(2);
            }
        }

        int dayOfWeek = args1.DateValue().DayOfWeek() % 7;

        if (type == 1 || type == 17) {
            return Operand.Create(dayOfWeek + 1);
        } else if (type == 2 || type == 11) {
            if (dayOfWeek == 0) return Operand.Create(7);
            return Operand.Create(dayOfWeek);
        } else if (type == 3) {
            if (dayOfWeek == 0) return Operand.Create(6);
            return Operand.Create(dayOfWeek - 1);
        } else if (type == 12) {
            int[] mapping = { 6, 7, 1, 2, 3, 4, 5 };
            return Operand.Create(mapping[dayOfWeek]);
        } else if (type == 13) {
            int[] mapping = { 5, 6, 7, 1, 2, 3, 4 };
            return Operand.Create(mapping[dayOfWeek]);
        } else if (type == 14) {
            int[] mapping = { 4, 5, 6, 7, 1, 2, 3 };
            return Operand.Create(mapping[dayOfWeek]);
        } else if (type == 15) {
            int[] mapping = { 3, 4, 5, 6, 7, 1, 2 };
            return Operand.Create(mapping[dayOfWeek]);
        } else if (type == 16) {
            int[] mapping = { 2, 3, 4, 5, 6, 7, 1, 2 };
            return Operand.Create(mapping[dayOfWeek]);
        }

        return ParameterError(2);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func2 != null) {
            func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        }
    }
}
