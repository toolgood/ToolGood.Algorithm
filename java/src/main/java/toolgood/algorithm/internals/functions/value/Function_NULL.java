package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_NULL extends Function_0 {
    public Function_NULL() {
    }

    @Override
    public String Name() {
        return "NULL";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        return Operand.Null;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NULL;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("NULL");
    }
}
