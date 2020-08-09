package toolgood.algorithm;

class OperandString extends OperandT<String> {
    public OperandString(final String obj) : base(obj) { }

    public override OperandType Type=>OperandType.STRING;
    public override string TextValue=>Value;
}