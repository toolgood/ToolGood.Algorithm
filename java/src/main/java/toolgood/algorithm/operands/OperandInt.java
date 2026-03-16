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

class OperandInt extends Operand {
    private final int value;

    public OperandInt(int obj) {
        value = obj;
    }

    @Override
    public boolean IsNumber() { return true; }

    @Override
    public boolean IsNotNumber() { return false; }

    @Override
    public OperandType Type() { return OperandType.NUMBER; }

    @Override
    public int IntValue() { return value; }

    @Override
    public BigDecimal NumberValue() { return BigDecimal.valueOf(value); }

    @Override
    public long LongValue() { return value; }

    @Override
    public double DoubleValue() { return value; }


    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) { return IntValue() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return IntValue() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToText(String errorMessage) { return Create(Integer.toString(value)); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(Integer.toString(value)); }

    @Override
    public Operand ToMyDate(String errorMessage) { return Create(new MyDate(value)); }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return Create(new MyDate(value)); }

    @Override
    public String toString() { return NumberValue().toString(); }
}