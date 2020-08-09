package toolgood.algorithm;

class OperandError extends Operand {
    public override OperandType Type => OperandType.ERROR;
    public override bool IsError => true;
    private final string _errorMsg;
    public override string ErrorMsg => _errorMsg;
    public OperandError(final string msg)
    {
        _errorMsg = msg;
    }
}