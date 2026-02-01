package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.UUID;


public class Function_GUID extends FunctionBase {
    public Function_GUID() {
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return Operand.Create(UUID.randomUUID().toString());
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("GUID()");
    }
}
