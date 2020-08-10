package toolgood.algorithm;

class OperandString extends OperandT<String> {
 
    public OperandString(String obj) {
        super(obj);
    }

    @Override
    public OperandType Type() {
        return OperandType.STRING;
    }

    @Override
    public String TextValue(){
        return Value;
    }


}