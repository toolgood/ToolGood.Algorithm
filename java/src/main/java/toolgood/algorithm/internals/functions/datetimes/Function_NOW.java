package toolgood.algorithm.internals.functions.datetimes;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;



public class Function_NOW extends FunctionBase {
    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        toolgood.algorithm.internals.MyDate now = toolgood.algorithm.internals.MyDate.now();
        return Operand.Create(now);
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Now()");
    }
}