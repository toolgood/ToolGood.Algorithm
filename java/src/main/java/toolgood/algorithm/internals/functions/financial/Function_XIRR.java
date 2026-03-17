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

public final class Function_XIRR extends Function_3 {
    public Function_XIRR(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "XIRR";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand valuesArg = GetArray_1(engine, tempParameter);
        if (valuesArg.IsErrorOrNone()) return valuesArg;

        List<Double> values = new ArrayList<>();
        for (Operand v : valuesArg.ArrayValue()) {
            values.add(v.DoubleValue());
        }

        Operand datesArg = GetArray_2(engine, tempParameter);
        if (datesArg.IsErrorOrNone()) return datesArg;

        List<Long> dateMillis = new ArrayList<>();
        for (Operand d : datesArg.ArrayValue()) {
            if (d.IsDate()) {
                dateMillis.add(d.DateValue().ToDateTime().getMillis());
            } else if (d.IsText()) {
                MyDate myDate = MyDate.parse(d.TextValue());
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
            if (guessArg.IsErrorOrNone()) return guessArg;
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
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        if (func3 != null) func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
