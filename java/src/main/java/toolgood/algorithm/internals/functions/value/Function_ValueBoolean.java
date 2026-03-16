package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;

public class Function_ValueBoolean extends FunctionBase {
    private final boolean _value;

    public Function_ValueBoolean(boolean value) {
        this._value = value;
    }

    @Override
    public String getName() {
        return _value ? "True" : "False";
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return _value ? Operand.True : Operand.False;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.BOOLEAN;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(getName());
    }
}
