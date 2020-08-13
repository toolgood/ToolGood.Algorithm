package toolgood.algorithm;

class OperandNull extends Operand {
    // public override OperandType Type => OperandType.NULL;
    // public override bool IsNull => true;

    @Override
    public OperandType Type() {
        return OperandType.NULL;
    }

    @Override
    public boolean IsNull()   {
        return true;
    }

}