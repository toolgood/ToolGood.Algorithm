package toolgood.algorithm;

class OperandError extends Operand {
    private final String _errorMsg;
 
    public OperandError(final String msg)
    {
        _errorMsg = msg;
    }

    @Override
    public OperandType Type() {
        return OperandType.ERROR;
    }

    @Override
    public boolean IsError() {
        return true;
    }
    
    public String ErrorMsg(){
        return _errorMsg;
    }
}