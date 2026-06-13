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

public final class Function_XNPV extends Function_3 {
    public Function_XNPV(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length != 3) {
            throw new IllegalArgumentException(String.format("Function '%s' requires exactly 3 parameters.", Name()));
        }
    }

    @Override
    public String Name() {
        return "XNPV";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand rateArg = GetNumber_1(engine, tempParameter);
        if (rateArg.IsErrorOrNone()) return rateArg;
        BigDecimal rate = rateArg.NumberValue();
        if (rate.compareTo(BigDecimal.ONE.negate()) == 0) {
            return Div0Error();
        }

        Operand valuesArg = GetArray_2(engine, tempParameter);
        if (valuesArg.IsErrorOrNone()) return valuesArg;
        List<Operand> values = valuesArg.ArrayValue();

        Operand datesArg = GetArray_3(engine, tempParameter);
        if (datesArg.IsErrorOrNone()) return datesArg;
        List<Operand> dates = datesArg.ArrayValue();

        if (values.size() != dates.size()) return FunctionError();
        if (values.size() == 0) return ParameterError(1);

        List<DateTime> dateList = new ArrayList<>();
        for (Operand d : dates) {
            if (d.IsDate()) {
                dateList.add(d.DateValue().ToDateTime());
            } else if (d.IsText()) {
                MyDate myDate = MyDate.parse(d.TextValue());
                if (myDate == null) return ParameterError(3);
                dateList.add(myDate.ToDateTime());
            } else {
                return ParameterError(3);
            }
        }

        DateTime baseDate = dateList.get(0);
        BigDecimal xnpv = BigDecimal.ZERO;

        for (int i = 0; i < values.size(); i++) {
            int days = Days.daysBetween(baseDate, dateList.get(i)).getDays();
            BigDecimal daysDecimal = BigDecimal.valueOf(days);
            BigDecimal exponent = daysDecimal.divide(new BigDecimal("365.0"), MathContext.DECIMAL128);
            BigDecimal factor = BigDecimal.valueOf(Math.pow(BigDecimal.ONE.add(rate).doubleValue(), exponent.doubleValue()));
            xnpv = xnpv.add(values.get(i).NumberValue().divide(factor, MathContext.DECIMAL128));
        }

        return Operand.Create(xnpv);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
        func2.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
        func3.GetParameterTypes(noneEngine, result, OperandType.ARRAY);
    }
}
