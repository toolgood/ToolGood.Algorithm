package toolgood.algorithm;

class OperandNumber extends OperandT<Double> {

    public OperandNumber(Double obj) {
        super(obj);
        // TODO Auto-generated constructor stub
    }
    
    public OperandNumber(final double obj) : base(obj) { }
    public  OperandType Type => OperandType.NUMBER;
    public override int IntValue => (int) Value;
    public override double NumberValue => Value;
}