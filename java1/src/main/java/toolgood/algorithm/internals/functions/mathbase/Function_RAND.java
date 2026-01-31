package toolgood.algorithm.internals.functions.mathbase;

import toolgood.algorithm.Internals.Operand;
import toolgood.algorithm.Internals.AlgorithmEngine;
import toolgood.algorithm.internals.FunctionBase;
import java.util.Random;

public class Function_RAND extends FunctionBase {
    public Function_RAND() {
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.Function<AlgorithmEngine, String, Operand> tempParameter) {
        Random rand = new Random();
        return Operand.Create(rand.nextDouble());
    }

    @Override
    public void ToString(java.lang.StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Rand()");
    }
}
