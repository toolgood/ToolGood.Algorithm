package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.functions.Function_0;

public final class Function_ValueBoolean extends Function_0 {
    private final boolean _value;

    public Function_ValueBoolean(boolean value) {
        this._value = value;
    }

    @Override
    public String Name() {
        return _value ? "True" : "False";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) throws Exception {
        return _value ? Operand.True : Operand.False;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(Name());
    }
}
