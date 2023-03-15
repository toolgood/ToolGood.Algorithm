package toolgood.algorithm;

public enum OperandType {
    NULL,
    ERROR,
    DATE,
    ARRARY,
    BOOLEAN,
    TEXT,
    DOUBLE_NUMBER,
    LONG_NUMBER,
    JSON;

    public boolean isNumber() {
        return this == DOUBLE_NUMBER || this == LONG_NUMBER;
    }
}