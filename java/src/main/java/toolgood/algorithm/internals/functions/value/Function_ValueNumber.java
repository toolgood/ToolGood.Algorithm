package toolgood.algorithm.internals.functions.value;

import toolgood.algorithm.internals.functions.FunctionBase;
import toolgood.algorithm.Operand;
import toolgood.algorithm.AlgorithmEngine;
import toolgood.algorithm.enums.OperandType;

public class Function_ValueNumber extends FunctionBase {
    private final Operand _value;
    private final String _showName;

    public Function_ValueNumber(Operand value, String showName) {
        this._value = value;
        this._showName = showName;
    }

    @Override
    public String getName() {
        return _showName;
    }

    @Override
    public Operand Evaluate(AlgorithmEngine engine, java.util.function.BiFunction<AlgorithmEngine, String, Operand> tempParameter) {
        return _value;
    }

    @Override
    public OperandType GetResultType() {
        return OperandType.NUMBER;
    }

    @Override
    public void toString(StringBuilder stringBuilder, boolean addBrackets) {
        stringBuilder.append(_showName);
    }
}
