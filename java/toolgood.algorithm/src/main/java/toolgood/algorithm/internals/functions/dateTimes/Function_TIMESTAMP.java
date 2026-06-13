package toolgood.algorithm.internals.functions.dateTimes;

import java.math.BigDecimal;
import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.FunctionUtil;
import toolgood.algorithm.internals.functions.Function_2;

final class Function_TIMESTAMP extends Function_2 {
    public Function_TIMESTAMP(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 1 || funcs.length > 2) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 1 to 2 parameters.");
        }
    }

    @Override
    public String Name() {
        return "Timestamp";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        int type = 0;
        if (func2 != null) {
            Operand args2 = GetNumber_2(engine, tempParameter);
            if (args2.IsErrorOrNone()) { return args2; }
            type = args2.IntValue();
        }
        Operand args0 = GetDate_1(engine, tempParameter);
        if (args0.IsErrorOrNone()) { return args0; }
        DateTime args1;
        if (engine.UseLocalTime) {
            args1 = args0.DateValue().ToDateTime(DateTimeZone.getDefault()).withZone(DateTimeZone.UTC);
        } else {
            args1 = args0.DateValue().ToDateTime();
        }
        if (type == 0) {
            long ms = args1.getMillis() - FunctionUtil.StartDateUtc.getMillis();
            return Operand.Create(new BigDecimal(ms));
        } else if (type == 1) {
            long ms = args1.getMillis() - FunctionUtil.StartDateUtc.getMillis();
            double s = ms / 1000.0;
            return Operand.Create(s);
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
