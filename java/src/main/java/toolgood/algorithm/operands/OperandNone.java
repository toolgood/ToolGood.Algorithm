package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

public final class OperandNone extends Operand {
    @Override
    public OperandType Type() { return OperandType.NONE; }

    @Override
    public boolean IsErrorOrNone() { return true; }

    @Override
    public boolean IsNone() { return true; }
}
