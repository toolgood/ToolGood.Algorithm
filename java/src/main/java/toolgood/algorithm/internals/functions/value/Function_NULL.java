package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;

public class Function_NULL extends FunctionBase {

    public Function_NULL() {
    }

    @Override
    public String getName() {
        return "NULL";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return Operand.NULL_OPERAND;
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
