package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

public final class OperandBoolean extends Operand {
    private final boolean _value;

    public OperandBoolean(boolean obj) {
        _value = obj;
    }

    @Override
    public boolean IsBoolean() { return true; }

    @Override
    public OperandType Type() { return OperandType.BOOLEAN; }

    @Override
    public boolean BooleanValue() { return _value; }

    @Override
    public Operand ToNumber(String errorMessage) { return _value ? One : Zero; }

    @Override
    public Operand ToNumber(String errorMessage, Object[] args) { return _value ? One : Zero; }

    @Override
    public Operand ToBoolean(String errorMessage) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToText(String errorMessage) { return Create(_value ? "TRUE" : "FALSE"); }

    @Override
    public Operand ToText(String errorMessage, Object[] args) { return Create(_value ? "TRUE" : "FALSE"); }

    @Override
    public String toString() { return _value ? "true" : "false"; }
}
