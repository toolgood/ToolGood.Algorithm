package toolgood.algorithm.litJson;

public enum JsonToken {
    None,

    ObjectStart,
    PropertyName,
    ObjectEnd,

    ArrayStart,
    ArrayEnd,

    Double,

    String,

    Boolean,
    Null
}
