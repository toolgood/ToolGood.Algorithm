package toolgood.algorithm;

class OperandArray extends OperandT<java.util.List<Operand>> {
    public OperandArray(final List<Operand> obj)
    {
         super(obj);
    }

    @Override
    public OperandType Type() {
        return OperandType.ARRARY;
    }

    @Override
    public List<Operand> ArrayValue(){
        return Value;
    }
}