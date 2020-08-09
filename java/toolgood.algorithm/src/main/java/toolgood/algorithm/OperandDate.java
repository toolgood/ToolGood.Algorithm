package toolgood.algorithm;

class OperandDate extends OperandT<Date> {
    public OperandDate(final Date obj) {
        super(obj);
    }

    public override OperandType Type=>OperandType.DATE;
    public override Date DateValue=>Value;
}