package toolgood.algorithm.internals.visitors;

public class AntlrErrorTextWriter {
    private boolean isError;
    private String errorMsg;

    public AntlrErrorTextWriter() {
    }

    public boolean IsError() {
        return isError;
    }

    public void setError(boolean error) {
        isError = error;
    }

    public String ErrorMsg() {
        return errorMsg;
    }

    public void setErrorMsg(String errorMsg) {
        this.errorMsg = errorMsg;
    }

    public void writeLine(String value) {
        isError = true;
        errorMsg = value;
    }
}
