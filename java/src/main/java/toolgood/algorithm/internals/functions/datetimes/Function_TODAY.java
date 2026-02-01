package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_TODAY extends FunctionBase {
    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
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