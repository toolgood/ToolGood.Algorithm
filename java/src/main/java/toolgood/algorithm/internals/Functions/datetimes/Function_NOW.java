package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.function.Function;

public class Function_NOW extends FunctionBase {
    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        toolgood.algorithm.internals.MyDate now = toolgood.algorithm.internals.MyDate.now();
        return Operand.create(now);
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Now()");
    }
}