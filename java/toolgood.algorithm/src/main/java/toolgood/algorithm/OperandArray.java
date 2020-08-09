package toolgood.algorithm;

class OperandArray extends OperandT<java.util.List<Operand>> {
    // public OperandArray(final List<Operand> obj) : base(obj) { }

    // public override OperandType Type=>OperandType.ARRARY;
    // public override List<Operand>ArrayValue=>Value;

    @Override
    public OperandType Type() {
        return OperandArray.ARRARY;
    }

}