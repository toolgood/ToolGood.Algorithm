package toolgood.algorithm.internals.functions.dateTimes;

import java.math.BigDecimal;
import java.math.RoundingMode;
import java.util.List;
import org.joda.time.DateTime;
import org.joda.time.DateTimeZone;
import org.joda.time.Days;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.NoneEngine;
import toolgood.algorithm.internals.ParameterType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_3;

final class Function_YEARFRAC extends Function_3 {
    public Function_YEARFRAC(FunctionBase[] funcs) {
        super(funcs);
        if (funcs.length < 2 || funcs.length > 3) {
            throw new IllegalArgumentException("Function '" + Name() + "' requires 2 to 3 parameters.");
        }
    }

    @Override
    public String Name() {
        return "YEARFRAC";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        Operand startDateArg = GetDate_1(engine, tempParameter);
        if (startDateArg.IsErrorOrNone()) return startDateArg;
        DateTime startDate = startDateArg.DateValue().ToDateTime(DateTimeZone.UTC);

        Operand endDateArg = GetDate_2(engine, tempParameter);
        if (endDateArg.IsErrorOrNone()) return endDateArg;
        DateTime endDate = endDateArg.DateValue().ToDateTime(DateTimeZone.UTC);

        int basis = 0;
        if (func3 != null) {
            Operand basisArg = GetNumber_3(engine, tempParameter);
            if (basisArg.IsErrorOrNone()) return basisArg;
            basis = basisArg.IntValue();
            if (basis < 0 || basis > 4) {
                return ParameterError(3);
            }
        }

        BigDecimal result = CalculateYearFrac(startDate, endDate, basis);
        return Operand.Create(result);
    }

    private BigDecimal CalculateYearFrac(DateTime startDate, DateTime endDate, int basis) {
        if (startDate.isAfter(endDate)) {
            DateTime temp = startDate;
            startDate = endDate;
            endDate = temp;
        }

        switch (basis) {
            case 0:
                return Calculate30_360(startDate, endDate);
            case 1:
                return CalculateActualActual(startDate, endDate);
            case 2:
                return BigDecimal.valueOf(Days.daysBetween(startDate, endDate).getDays())
                        .divide(BigDecimal.valueOf(360), 10, RoundingMode.HALF_UP);
            case 3:
                return BigDecimal.valueOf(Days.daysBetween(startDate, endDate).getDays())
                        .divide(BigDecimal.valueOf(365), 10, RoundingMode.HALF_UP);
            case 4:
                return Calculate30_360E(startDate, endDate);
            default:
                return Calculate30_360(startDate, endDate);
        }
    }

    private BigDecimal Calculate30_360(DateTime startDate, DateTime endDate) {
        int d1 = Math.min(30, startDate.getDayOfMonth());
        int d2 = endDate.getDayOfMonth();

        if (d1 == 30) d2 = Math.min(30, d2);

        return BigDecimal.valueOf(360 * (endDate.getYear() - startDate.getYear())
                + 30 * (endDate.getMonthOfYear() - startDate.getMonthOfYear()) + (d2 - d1))
                .divide(BigDecimal.valueOf(360), 10, RoundingMode.HALF_UP);
    }

    private BigDecimal Calculate30_360E(DateTime startDate, DateTime endDate) {
        int d1 = Math.min(30, startDate.getDayOfMonth());
        int d2 = Math.min(30, endDate.getDayOfMonth());

        return BigDecimal.valueOf(360 * (endDate.getYear() - startDate.getYear())
                + 30 * (endDate.getMonthOfYear() - startDate.getMonthOfYear()) + (d2 - d1))
                .divide(BigDecimal.valueOf(360), 10, RoundingMode.HALF_UP);
    }

    private BigDecimal CalculateActualActual(DateTime startDate, DateTime endDate) {
        int startYear = startDate.getYear();
        int endYear = endDate.getYear();

        if (startYear == endYear) {
            int daysInYear = startDate.year().isLeap() ? 366 : 365;
            return BigDecimal.valueOf(Days.daysBetween(startDate, endDate).getDays())
                    .divide(BigDecimal.valueOf(daysInYear), 10, RoundingMode.HALF_UP);
        }

        BigDecimal result = BigDecimal.ZERO;
        int daysInStartYear = startDate.year().isLeap() ? 366 : 365;
        int daysInEndYear = endDate.year().isLeap() ? 366 : 365;

        DateTime yearEnd = new DateTime(startYear, 12, 31, 0, 0, 0, DateTimeZone.UTC);
        result = result.add(BigDecimal.valueOf(Days.daysBetween(startDate, yearEnd).getDays())
                .divide(BigDecimal.valueOf(daysInStartYear), 10, RoundingMode.HALF_UP));
        DateTime yearStart = new DateTime(endYear, 1, 1, 0, 0, 0, DateTimeZone.UTC);
        result = result.add(BigDecimal.valueOf(Days.daysBetween(yearStart, endDate).getDays())
                .divide(BigDecimal.valueOf(daysInEndYear), 10, RoundingMode.HALF_UP));
        result = result.add(BigDecimal.valueOf(endYear - startYear - 1));

        return result;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func3 != null) func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
