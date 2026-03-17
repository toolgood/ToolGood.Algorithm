package toolgood.algorithm.internals.functions.financial;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.operands.MyDate;

public final class Function_XNPV extends Function_3 {
    public Function_XNPV(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "XNPV";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsErrorOrNone()) return rateArg;
        double rate = rateArg.DoubleValue();
        if (rate == -1) {
            return Div0Error();
        }

        Operand valuesArg = GetArray_2(engine, tempParameter);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        List<Operand> values = valuesArg.ArrayValue();

        Operand datesArg = GetArray_3(engine, tempParameter);
        if (datesArg.IsErrorOrNone()) return datesArg;
        List<Operand> dates = datesArg.ArrayValue();

        if (values.size() != dates.size()) return FunctionError();
        if (values.isEmpty()) return ParameterError(1);

        List<Long> dateMillis = new ArrayList<>();
        final long MILLIS_PER_DAY = 1000L * 60 * 60 * 24;
        for (Operand d : dates) {
            if (d.IsDate()) {
                dateMillis.add(d.DateValue().ToDateTime().getMillis());
            } else if (d.IsText()) {
                MyDate myDate = MyDate.parse(d.TextValue());
                if (myDate == null) return ParameterError(3);
                dateMillis.add(myDate.ToDateTime().getMillis());
            } else {
                return ParameterError(3);
            }
        }

        long baseMillis = dateMillis.get(0);
        double xnpv = 0;

        for (int i = 0; i < values.size(); i++) {
            double days = (dateMillis.get(i) - baseMillis) / (double) MILLIS_PER_DAY;
            xnpv += values.get(i).DoubleValue() / Math.pow(1 + rate, days / 365.0);
        }

        return Operand.Create(xnpv);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func3.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
