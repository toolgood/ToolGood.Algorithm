package toolgood.algorithm.internals.functions.financial;

import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

/**
 * XIRR: 返回一组不定期发生的现金流的内部收益率
 * XIRR(values, dates, [guess])
 */
public class Function_XIRR extends Function_3 {
    public Function_XIRR(FunctionBase func1, FunctionBase func2, FunctionBase func3) {
        super(func1, func2, func3);
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand valuesArg = GetArray_1(engine, tempParameter);
        if (valuesArg.IsError()) return valuesArg;

        List<Double> values = new ArrayList<>();
        for (Operand v : valuesArg.ArrayValue()) {
            values.add(v.IsNumber() ? v.DoubleValue() : 0.0);
        }

        Operand datesArg = GetArray_2(engine, tempParameter);
        if (datesArg.IsError()) return datesArg;

        List<Long> dateMillis = new ArrayList<>();
        for (Operand d : datesArg.ArrayValue()) {
            if (d.IsDate()) {
                dateMillis.add(d.DateValue().ToDateTime().getMillis());
            } else if (d.IsText()) {
                MyDate myDate = MyDate.Parse(d.TextValue());
                if (myDate == null) return ParameterError(2);
                dateMillis.add(myDate.ToDateTime().getMillis());
            } else {
                return ParameterError(2);
            }
        }

        if (values.size() != dateMillis.size() || values.size() < 2) return FunctionError();

        double guess = 0.1;
        if (func3 != null) {
            Operand guessArg = GetNumber_3(engine, tempParameter);
            if (guessArg.IsError()) return guessArg;
            guess = guessArg.DoubleValue();
        }

        double xirr = newtonRaphsonXIRR(values, dateMillis, guess);
        return Operand.Create(xirr);
    }

    private double newtonRaphsonXIRR(List<Double> values, List<Long> dateMillis, double rate) {
        long baseMillis = dateMillis.get(0);
        final long MILLIS_PER_DAY = 1000L * 60 * 60 * 24;

        for (int iter = 0; iter < 100; iter++) {
            double npv = 0;
            double dnpv = 0;

            for (int i = 0; i < values.size(); i++) {
                double days = (dateMillis.get(i) - baseMillis) / (double) MILLIS_PER_DAY;
                double exp = days / 365.0;
                double factor = Math.pow(1 + rate, exp);
                npv += values.get(i) / factor;
                dnpv -= values.get(i) * exp / (factor * (1 + rate));
            }

            if (Math.abs(dnpv) < 1e-12) break;
            double newRate = rate - npv / dnpv;

            if (Math.abs(newRate - rate) < 1e-10) {
                return newRate;
            }
            rate = newRate;
        }
        return rate;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        AddFunction(stringBuilder, "XIRR");
    }
}
