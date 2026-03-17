package toolgood.algorithm.operands;

import toolgood.algorithm.Operand;
import toolgood.algorithm.enums.OperandType;

final class OperandError extends Operand {
    private final String _errorMsg;
    private final Object[] _args;
    private String _formattedMsg;

    @Override
    public OperandType Type() { return OperandType.ERROR; }

    @Override
    public boolean IsErrorOrNone() { return true; }

    @Override
    public boolean IsError() { return true; }

    @Override
    public String ErrorMsg() { 
        if (_formattedMsg == null) {
            _formattedMsg = FormatMessage();
        }
        return _formattedMsg; 
    }

    public OperandError(String msg) {
        _errorMsg = msg;
        _args = null;
    }

    public OperandError(String msg, Object[] args) {
        _errorMsg = msg;
        _args = args;
    }

    private String FormatMessage() {
        if (_args == null || _args.length == 0) {
            return _errorMsg;
        }
        return String.format(_errorMsg, _args);
    }

    @Override
    public Operand ToNumber(String errorMessage) { return this; }

    @Override
    public Operand ToNumber(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage) { return this; }

    @Override
    public Operand ToBoolean(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToText(String errorMessage) { return this; }

    @Override
    public Operand ToText(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToArray(String errorMessage) { return this; }

    @Override
    public Operand ToArray(String errorMessage, Object[] args) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage) { return this; }

    @Override
    public Operand ToMyDate(String errorMessage, Object[] args) { return this; }
}
