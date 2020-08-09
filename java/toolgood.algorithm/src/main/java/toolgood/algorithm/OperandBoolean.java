package toolgood.algorithm;

class OperandBoolean extends OperandT<Boolean>  {
    public OperandBoolean(final Boolean obj) : base(obj) { }
    public override OperandType Type => OperandType.BOOLEAN;
    public override bool BooleanValue => Value;
}