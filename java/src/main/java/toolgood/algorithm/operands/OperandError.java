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

class OperandError extends Operand {
    private final String errorMsg;

    public OperandError(String msg) {
        errorMsg = msg;
    }

    @Override
    public OperandType Type() { return OperandType.ERROR; }

    @Override
    public boolean IsError() { return true; }

    @Override
    public String ErrorMsg() { return errorMsg; }

    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToText(String errorMessage) { return this; }

    @Override
    public Operand ToText(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToArray(String errorMessage) { return this; }

    @Override
    public Operand ToArray(String errorMessage, Object... args) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage, Object... args) { return this; }
}