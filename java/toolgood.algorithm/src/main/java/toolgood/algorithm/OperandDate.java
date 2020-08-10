package toolgood.algorithm;

class OperandDate extends OperandT<MyDate> {
    public OperandDate(final MyDate obj) {
        super(obj);
    }
 
    @Override
    public OperandType Type() {
        return OperandType.DATE;
    }

    @Override
    public MyDate DateValue(){
        return Value;
    }
}