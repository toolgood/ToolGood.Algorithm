package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;
import toolgood.algorithm.internals.MyDate;
import toolgood.algorithm.litJson.JsonData;
import toolgood.algorithm.internals.functions.FunctionUtil;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

class OperandBigDecimal extends Operand {
    private final BigDecimal value;

    public OperandBigDecimal(BigDecimal obj) {
        value = obj;
    }

    @Override
    public boolean IsNumber() { return true; }

    @Override
    public boolean IsNotNumber() { return false; }

    @Override
    public OperandType Type() { return OperandType.NUMBER; }

    @Override
    public int IntValue() { return value.intValue(); }

    @Override
    public BigDecimal NumberValue() { return value; }

    @Override
    public long LongValue() { return value.longValue(); }

    @Override
    public double DoubleValue() { return value.doubleValue(); }


    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) { return value.compareTo(BigDecimal.ZERO) != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return value.compareTo(BigDecimal.ZERO) != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToText(String errorMessage) { return Create(value.toString()); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(value.toString()); }

    @Override
    public Operand ToMyDate(String errorMessage) { return Create(new MyDate(value.longValue())); }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return Create(new MyDate(value.longValue())); }

    @Override
    public String toString() { return NumberValue().toString(); }
}