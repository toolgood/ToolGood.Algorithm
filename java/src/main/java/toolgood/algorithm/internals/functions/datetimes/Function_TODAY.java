package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_TODAY extends FunctionBase {
    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        toolgood.algorithm.internals.MyDate now = toolgood.algorithm.internals.MyDate.now();
        // 创建一个只包含日期部分的新 MyDate 对象
        toolgood.algorithm.internals.MyDate today = new toolgood.algorithm.internals.MyDate(now.Year, now.Month, now.Day, 0, 0, 0);
        return Operand.Create(today);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Today()");
    }
}