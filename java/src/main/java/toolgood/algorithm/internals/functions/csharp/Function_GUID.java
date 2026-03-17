package toolgood.algorithm.internals.functions.csharp;

import java.util.UUID;
import java.util.function.BiFunction;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_GUID extends Function_0 {
    public Function_GUID() {
    }

    @Override
    public String Name() {
        return "Guid";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return Operand.Create(UUID.randomUUID().toString());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("GUID()");
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.TEXT;
    }
}
