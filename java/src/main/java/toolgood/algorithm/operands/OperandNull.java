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

class OperandNull extends Operand {
    @Override
    public boolean IsNull() { return true; }

    @Override
    public boolean IsNotNull() { return false; }

    @Override
    public OperandType Type() { return OperandType.NULL; }

    @Override
    public String toString() { return "null"; }
}