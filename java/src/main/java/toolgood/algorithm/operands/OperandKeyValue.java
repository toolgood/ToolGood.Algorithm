package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

public final class OperandKeyValue extends Operand {
    private final KeyValue _value;

    public OperandKeyValue(KeyValue obj) {
        _value = obj;
    }

    @Override
    public boolean IsArrayJson() { return true; }

    @Override
    public OperandType Type() { return OperandType.ARRAYJSON; }

    public KeyValue Value() { return _value; }
}
