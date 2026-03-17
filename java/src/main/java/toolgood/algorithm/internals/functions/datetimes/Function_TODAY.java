package toolgood.algorithm.internals.functions.datetimes;

import java.lang.StringBuilder;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.internals.functions.Function_0;
import toolgood.algorithm.operands.MyDate;

public final class Function_TODAY extends Function_0 {
    @Override
    public String Name() {
        return "Today";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        MyDate now = MyDate.now();
        return Operand.Create(new MyDate(now.Year, now.Month, now.Day, 0, 0, 0));
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Today()");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.DATE;
    }
}
