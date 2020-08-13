package toolgood.algorithm;

class OperandNumber extends OperandT<Double> {

    public OperandNumber(Double obj) {
        super(obj);
    }

    @Override
    public OperandType Type() {
        return OperandType.NUMBER;
    }

    @Override
    public int IntValue(){
        return (int)(double)Value;
    }
    @Override
    public double NumberValue(){
        return Value;
    }
 
}