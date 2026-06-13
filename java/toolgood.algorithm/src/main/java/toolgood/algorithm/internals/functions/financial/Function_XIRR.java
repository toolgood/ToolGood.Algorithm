package toolgood.algorithm.internals.functions.financial;

import java.math.BigDecimal;
import java.math.MathContext;
import java.math.RoundingMode;
import java.util.ArrayList;
import java.util.List;
import java.util.function.BiFunction;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.joda.time.Days;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.MyDate;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

public final class Function_XIRR extends Function_3 {
    public Function_XIRR(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException(String.format("Function '%s' requires 2 to 3 parameters.", Name()));
        }
    }

    @Override
    public String Name() {
        return "XIRR";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand valuesArg = GetArray_1(engine, tempParameter);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        List<BigDecimal> values = new ArrayList<>();
        for (Operand v : valuesArg.ArrayValue()) {
            values.add(v.NumberValue());
        }

        Operand datesArg = GetArray_2(engine, tempParameter);
        if (datesArg.IsErrorOrNone()) return datesArg;
        List<DateTime> dates = new ArrayList<>();
        for (Operand d : datesArg.ArrayValue()) {
            if (d.IsDate()) {
                dates.add(d.DateValue().ToDateTime());
            } else if (d.IsText()) {
                MyDate myDate = MyDate.parse(d.TextValue());
                if (myDate == null) return ParameterError(2);
                dates.add(myDate.ToDateTime());
            } else {
                return ParameterError(2);
            }
        }

        if (values.size() != dates.size() || values.size() < 2) return FunctionError();

        BigDecimal guess = new BigDecimal("0.1");
        if (func3 != null) {
            Operand guessArg = GetNumber_3(engine, tempParameter);
            if (guessArg.IsErrorOrNone()) return guessArg;
            guess = guessArg.NumberValue();
        }

        try {
            BigDecimal xirr = NewtonRaphsonXIRR(values, dates, guess);
            return Operand.Create(xirr);
        } catch (Exception e) {
            return FunctionError();
        }
    }

    private BigDecimal NewtonRaphsonXIRR(List<BigDecimal> values, List<DateTime> dates, BigDecimal rate) {
        DateTime baseDate = dates.get(0);

        for (int iter = 0; iter < 100; iter++) {
            BigDecimal npv = BigDecimal.ZERO;
            BigDecimal dnpv = BigDecimal.ZERO;

            for (int i = 0; i < values.size(); i++) {
                int days = Days.daysBetween(baseDate, dates.get(i)).getDays();
                BigDecimal exp = BigDecimal.valueOf(days).divide(new BigDecimal("365.0"), MathContext.DECIMAL128);
                BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), exp.doubleValue()));
                npv = npv.add(values.get(i).divide(factor, MathContext.DECIMAL128));
                dnpv = dnpv.subtract(
                        values.get(i).multiply(exp)
                                .divide(factor.multiply(BigDecimal.ONE.add(rate)), MathContext.DECIMAL128));
            }

            if (dnpv.abs().compareTo(new BigDecimal("1e-12")) < 0) break;
            BigDecimal newRate = rate.subtract(npv.divide(dnpv, MathContext.DECIMAL128));

            if (newRate.subtract(rate).abs().compareTo(new BigDecimal("1e-10")) < 0) {
                return newRate;
            }
            rate = newRate;
        }
        throw new RuntimeException("XIRR calculation did not converge");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        if (func3 != null) func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
