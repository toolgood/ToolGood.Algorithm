package toolgood.algorithm.litJson;

enum ParserToken {
    None(Character.MAX_VALUE + 1),
    Number(Character.MAX_VALUE + 2),
    True(Character.MAX_VALUE + 3),
    False(Character.MAX_VALUE + 4),
    Null(Character.MAX_VALUE + 5),
    CharSeq(Character.MAX_VALUE + 6),
    Char(Character.MAX_VALUE + 7),
    Text(Character.MAX_VALUE + 8),
    Object(Character.MAX_VALUE + 9),
    ObjectPrime(Character.MAX_VALUE + 10),
    Pair(Character.MAX_VALUE + 11),
    PairRest(Character.MAX_VALUE + 12),
    Array(Character.MAX_VALUE + 13),
    ArrayPrime(Character.MAX_VALUE + 14),
    Value(Character.MAX_VALUE + 15),
    ValueRest(Character.MAX_VALUE + 16),
    String(Character.MAX_VALUE + 17),
    End(Character.MAX_VALUE + 18),
    Epsilon(Character.MAX_VALUE + 19);

    public final int value;

    ParserToken(int value) {
        this.value = value;
    }
}
