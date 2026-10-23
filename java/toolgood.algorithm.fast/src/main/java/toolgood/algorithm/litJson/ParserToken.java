package toolgood.algorithm.litJson;

public enum ParserToken {
    None(0x10000),
    Number(0x10001),
    True(0x10002),
    False(0x10003),
    Null(0x10004),
    CharSeq(0x10005),
    // Single char
    Char(0x10006),

    // Parser Rules (see section A.2.1 of the manual)
    Text(0x10007),
    Object(0x10008),
    ObjectPrime(0x10009),
    Pair(0x10010),
    PairRest(0x10011),
    Array(0x10012),
    ArrayPrime(0x10013),
    Value(0x10014),
    ValueRest(0x10015),
    String(0x10016),

    // End of input
    End(0x10017),

    // The empty rule
    Epsilon(0x10018);


    public int value;  
    // 构造方法  
    private ParserToken( int index) {  
         this.value = index;  
    }  
}
