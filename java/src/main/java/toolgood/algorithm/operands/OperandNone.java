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

/**
 * NONE 类型操作数，用于参数类型推断（NoneEngine 返回此值）
 */
class OperandNone extends Operand {

    @Override
    public OperandType Type() { return OperandType.NONE; }

    @Override
    public boolean IsError() { return true; }

    @Override
    public String ErrorMsg() { return "NONE"; }

    @Override
    public String toString() { return "NONE"; }
}