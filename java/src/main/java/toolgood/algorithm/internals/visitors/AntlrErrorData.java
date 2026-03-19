package toolgood.algorithm.internals.visitors;

public class AntlrErrorData {
    private boolean IsError;
    private String ErrorMsg;

    public boolean isError() {
        return IsError;
    }

    public void setError(boolean error) {
        IsError = error;
    }

    public String getErrorMsg() {
        return ErrorMsg;
    }

    public void setErrorMsg(String errorMsg) {
        ErrorMsg = errorMsg;
    }
}
