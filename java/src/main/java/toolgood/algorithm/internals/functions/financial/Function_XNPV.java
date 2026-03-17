package toolgood.algorithm.internals.functions.financial;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;
import toolgood.algorithm.operands.MyDate;

/**
 * XNPV: 返回一组不定期发生的现金流的净现�?
 * XNPV(rate, values, dates)
 */
public class Function_XNPV extends Function_3 {
    public Function_XNPV(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsError()) return rateArg;
        double rate = rateArg.DoubleValue();
        if (rate == -1) return Div0Error();

        Operand valuesArg = GetArray_2(engine, tempParameter);
        if (valuesArg.IsError()) return valuesArg;
        List<Operand> values = valuesArg.ArrayValue();

        Operand datesArg = GetArray_3(engine, tempParameter);
        if (datesArg.IsError()) return datesArg;
        List<Operand> dates = datesArg.ArrayValue();

        if (values.size() != dates.size()) return FunctionError();
        if (values.isEmpty()) return ParameterError(1);

        List<Long> dateMillis = new ArrayList<>();
        final long MILLIS_PER_DAY = 1000L * 60 * 60 * 24;
        for (Operand d : dates) {
            if (d.IsDate()) {
                dateMillis.add(d.DateValue().ToDateTime().getMillis());
            } else if (d.IsText()) {
                MyDate myDate = MyDate.Parse(d.TextValue());
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
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "XNPV");
    }
}
