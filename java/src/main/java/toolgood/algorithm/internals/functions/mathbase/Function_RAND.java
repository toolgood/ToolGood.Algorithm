package toolgood.algorithm.internals.functions.mathbase;

import java.util.Random;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_RAND extends Function_0 {
    public Function_RAND() {
    }

    @Override
    public String Name() {
        return "Rand";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        Random rand = new Random();
        return Operand.Create(rand.nextDouble());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("Rand()");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }
}
