package toolgood.algorithm;

class OperandBoolean extends OperandT<Boolean>  {
    public OperandBoolean(final Boolean obj)
    {
         super(obj);
    }

    @Override
    public OperandType Type() {
        return OperandType.BOOLEAN;
    }

    @Override
    public boolean BooleanValue(){
        return Value;
    }
}