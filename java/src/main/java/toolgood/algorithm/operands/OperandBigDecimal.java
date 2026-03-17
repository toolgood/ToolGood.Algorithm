package toolgood.algorithm.operands;

import java.math.BigDecimal;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

final class OperandBigDecimal extends Operand {
    private final BigDecimal _value;

    public OperandBigDecimal(BigDecimal obj) {
        _value = obj;
    }

    @Override
    public boolean IsNumber() { return true; }

    @Override
    public OperandType Type() { return OperandType.NUMBER; }

    @Override
    public int IntValue() { return _value.intValue(); }

    @Override
    public BigDecimal NumberValue() { return _value; }

    @Override
    public long LongValue() { return _value.longValue(); }

    @Override
    public double DoubleValue() { return _value.doubleValue(); }

    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) {
        if (_value.compareTo(BigDecimal.ZERO) == 0) {
            return False;
        } else if (_value.compareTo(BigDecimal.ONE) == 0) {
            return True;
        }
        return super.ToBoolean(errorMessage);
    }

    @Override
    public Operand ToBoolean(String errorMessage, Object[] args) {
        if (_value.compareTo(BigDecimal.ZERO) == 0) {
            return False;
        } else if (_value.compareTo(BigDecimal.ONE) == 0) {
            return True;
        }
        return super.ToBoolean(errorMessage);
    }

    @Override
    public Operand ToText(String errorMessage) { return Create(_value.toString()); }

    @Override
    public Operand ToText(String errorMessage, Object[] args) { return Create(_value.toString()); }

    @Override
    public String toString() { return _value.toString(); }
}
