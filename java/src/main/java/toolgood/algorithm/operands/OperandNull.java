package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

public final class OperandNull extends Operand {
    @Override
    public boolean IsNull() { return true; }

    @Override
    public OperandType Type() { return OperandType.NULL; }

    @Override
    public String toString() { return "null"; }
}
