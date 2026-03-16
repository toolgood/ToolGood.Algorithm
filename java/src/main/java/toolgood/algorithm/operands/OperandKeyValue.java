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

class OperandKeyValue extends Operand {
    private final KeyValue value;

    public OperandKeyValue(KeyValue obj) {
        value = obj;
    }

    @Override
    public boolean IsArrayJson() { return true; }

    @Override
    public boolean IsNotArrayJson() { return false; }

    @Override
    public OperandType Type() { return OperandType.ARRARYJSON; }

    public KeyValue Value() { return value; }
}