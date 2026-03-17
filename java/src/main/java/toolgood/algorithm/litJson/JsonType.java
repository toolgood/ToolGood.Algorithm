package toolgood.algorithm.litJson;

public enum JsonType {
    None(0),
    Object(1),
    Array(2),
    String(3),
    Double(4),
    Boolean(5),
    Null(6);

    public final int value;

    JsonType(int value) {
        this.value = value;
    }
}
