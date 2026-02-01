package toolgood.algorithm.internals.visitors;

public class AntlrErrorTextWriter {
    private boolean IsError;
    private String errorMsg;

    public AntlrErrorTextWriter() {
    }

    public boolean IsError() {
        return IsError;
    }

    public void setError(boolean error) {
        IsError = error;
    }

    public String ErrorMsg() {
        return errorMsg;
    }

    public void setErrorMsg(String errorMsg) {
        this.errorMsg = errorMsg;
    }

    public void writeLine(String value) {
        IsError = true;
        errorMsg = value;
    }
}
