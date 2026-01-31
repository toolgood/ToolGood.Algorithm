package toolgood.algorithm.internals.functions.csharp;

import toolgood.algorithm.internals.FunctionBase;
import toolgood.algorithm.internals.Operand;
import toolgood.algorithm.AlgorithmEngine;

import java.util.UUID;
import java.util.function.Function;

public class Function_GUID extends FunctionBase {
    public Function_GUID() {
    }

    @Override
    public Operand Evaluate(AlgorithmEngine work, Function<String, Operand> tempParameter) {
        return Operand.Create(UUID.randomUUID().toString());
    }

    @Override
    public void ToString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append("GUID()");
    }
}
