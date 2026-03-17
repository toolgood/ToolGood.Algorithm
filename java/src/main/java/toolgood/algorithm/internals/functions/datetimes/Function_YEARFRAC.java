package toolgood.algorithm.internals.functions.datetimes;

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

public final class Function_YEARFRAC extends Function_3 {
    public Function_YEARFRAC(FunctionBase[] funcs) {
        super(funcs);
    }

    @Override
    public String Name() {
        return "YEARFRAC";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        if (func1 == null || func2 == null)
            return ParameterError(1);

        Operand startDateArg = GetDate_1(engine, tempParameter);
        if (startDateArg.IsErrorOrNone())
            return startDateArg;

        Operand endDateArg = GetDate_2(engine, tempParameter);
        if (endDateArg.IsErrorOrNone())
            return endDateArg;

        int basis = 0;
        if (func3 != null) {
            Operand basisArg = GetNumber_3(engine, tempParameter);
            if (basisArg.IsErrorOrNone())
                return basisArg;
            basis = basisArg.IntValue();
            if (basis < 0 || basis > 4) {
                return ParameterError(3);
            }
        }

        double result = calculateYearFrac(startDateArg.DateValue(), endDateArg.DateValue(), basis);
        return Operand.Create(result);
    }

    private double calculateYearFrac(MyDate startDate, MyDate endDate, int basis) {
        if (startDate.ToDateTime().isAfter(endDate.ToDateTime())) {
            MyDate temp = startDate;
            startDate = endDate;
            endDate = temp;
        }

        switch (basis) {
            case 0:
                return calculate30_360(startDate, endDate);
            case 1:
                return calculateActualActual(startDate, endDate);
            case 2:
                return daysBetween(startDate, endDate) / 360.0;
            case 3:
                return daysBetween(startDate, endDate) / 365.0;
            case 4:
                return calculate30_360E(startDate, endDate);
            default:
                return calculate30_360(startDate, endDate);
        }
    }

    private double calculate30_360(MyDate startDate, MyDate endDate) {
        int d1 = Math.min(30, startDate.Day);
        int d2 = endDate.Day;
        if (d1 == 30)
            d2 = Math.min(30, d2);
        return (360.0 * (endDate.Year - startDate.Year) + 30.0 * (endDate.Month - startDate.Month) + (d2 - d1)) / 360.0;
    }

    private double calculate30_360E(MyDate startDate, MyDate endDate) {
        int d1 = Math.min(30, startDate.Day);
        int d2 = Math.min(30, endDate.Day);
        return (360.0 * (endDate.Year - startDate.Year) + 30.0 * (endDate.Month - startDate.Month) + (d2 - d1)) / 360.0;
    }

    private double calculateActualActual(MyDate startDate, MyDate endDate) {
        int startYear = startDate.Year;
        int endYear = endDate.Year;

        if (startYear == endYear) {
            int daysInYear = isLeapYear(startYear) ? 366 : 365;
            return daysBetween(startDate, endDate) / (double) daysInYear;
        }

        int daysInStartYear = isLeapYear(startYear) ? 366 : 365;
        int daysInEndYear = isLeapYear(endYear) ? 366 : 365;

        MyDate endOfStartYear = new MyDate(startYear, 12, 31, 0, 0, 0);
        double result = daysBetween(startDate, endOfStartYear) / (double) daysInStartYear;

        MyDate startOfEndYear = new MyDate(endYear, 1, 1, 0, 0, 0);
        result += daysBetween(startOfEndYear, endDate) / (double) daysInEndYear;

        result += endYear - startYear - 1;

        return result;
    }

    private long daysBetween(MyDate d1, MyDate d2) {
        long millis1 = d1.ToDateTime().withTimeAtStartOfDay().getMillis();
        long millis2 = d2.ToDateTime().withTimeAtStartOfDay().getMillis();
        return (millis2 - millis1) / (1000L * 60 * 60 * 24);
    }

    private boolean isLeapYear(int year) {
        return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType,
            String op, String val) {
        func1.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func2 != null)
            func2.GetParameterTypes(noneEngine, result, OperandType.DATE);
        if (func3 != null)
            func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
    }
}
