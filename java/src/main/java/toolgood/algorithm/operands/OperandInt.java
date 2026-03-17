package toolgood.algorithm.operands;

import java.math.BigDecimal;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

public final class OperandInt extends Operand {
    private final int _value;

    public OperandInt(int obj) {
        _value = obj;
    }

    @Override
    public boolean IsNumber() { return true; }

    @Override
    public OperandType Type() { return OperandType.NUMBER; }

    @Override
    public int IntValue() { return _value; }

    @Override
    public BigDecimal NumberValue() { return BigDecimal.valueOf(_value); }

    @Override
    public long LongValue() { return _value; }

    @Override
    public double DoubleValue() { return _value; }

    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) {
        if (_value == 0) {
            return False;
        } else if (_value == 1) {
            return True;
        }
        return super.ToBoolean(errorMessage);
    }

    @Override
    public Operand ToBoolean(String errorMessage, Object[] args) {
        if (_value == 0) {
            return False;
        } else if (_value == 1) {
            return True;
        }
        return super.ToBoolean(errorMessage);
    }

    @Override
    public Operand ToText(String errorMessage) { return Create(Integer.toString(_value)); }

    @Override
    public Operand ToText(String errorMessage, Object[] args) { return Create(Integer.toString(_value)); }

    @Override
    public String toString() { return Integer.toString(_value); }
}
