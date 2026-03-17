package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

final class OperandMyDate extends Operand {
    private final MyDate _value;

    public OperandMyDate(MyDate obj) {
        _value = obj;
    }

    @Override
    public boolean IsDate() { return true; }

    @Override
    public OperandType Type() { return OperandType.DATE; }

    @Override
    public MyDate DateValue() { return _value; }

    @Override
    public Operand ToText(String errorMessage) { return Create(DateValue().toString()); }

    @Override
    public Operand ToText(String errorMessage, Object[] args) { return Create(DateValue().toString()); }

    @Override
    public Operand ToMyDate(String errorMessage) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage, Object[] args) { return this; }

    @Override
    public String toString() { return "\"" + DateValue().toString() + "\""; }
}
