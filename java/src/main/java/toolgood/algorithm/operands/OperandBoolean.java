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

class OperandBoolean extends Operand {
    private final boolean value;

    public OperandBoolean(boolean obj) {
        value = obj;
    }

    @Override
    public boolean IsBoolean() { return true; }

    @Override
    public boolean IsNotBoolean() { return false; }

    @Override
    public OperandType Type() { return OperandType.BOOLEAN; }

    @Override
    public boolean BooleanValue() { return value; }

    @Override
    public Operand ToNumber(String errorMessage) { return value ? ONE : ZERO; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return value ? ONE : ZERO; }

    @Override
    public Operand ToBoolean(String errorMessage) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToText(String errorMessage) { return Create(value ? "TRUE" : "FALSE"); }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return Create(value ? "TRUE" : "FALSE"); }

    @Override
    public String toString() { return value ? "true" : "false"; }
}