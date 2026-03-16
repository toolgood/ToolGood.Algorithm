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

class OperandMyDate extends Operand {
    private final MyDate value;

    public OperandMyDate(MyDate obj) {
        value = obj;
    }

    @Override
    public boolean IsDate() { return true; }

    @Override
    public boolean IsNotDate() { return false; }

    @Override
    public OperandType Type() { return OperandType.DATE; }

    @Override
    public MyDate DateValue() { return value; }

    @Override
    public Operand ToNumber(String errorMessage) { return Create(value.ToNumber()); }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return Create(value.ToNumber()); }

    @Override
    public Operand ToBoolean(String errorMessage) { return value.ToNumber() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return value.ToNumber() != 0 ? TRUE : FALSE; }

    @Override
    public Operand ToText(String errorMessage) { return Create(value.toString()); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(value.toString()); }

    @Override
    public Operand ToMyDate(String errorMessage) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return this; }

    @Override
    public String toString() { return '"' + value.toString() + '"'; }
}