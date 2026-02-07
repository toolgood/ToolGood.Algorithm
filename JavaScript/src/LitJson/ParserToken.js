let ParserToken = {
    // Lexer tokens (see section A.1.1. of the manual)
    None: 65536, // System.Char.MaxValue + 1

    Number: 65537,
    True: 65538,
    False: 65539,
    Null: 65540,
    CharSeq: 65541,

    // Single char
    Char: 65542,

    // Parser Rules (see section A.2.1 of the manual)
    Text: 65543,

    Object: 65544,
    ObjectPrime: 65545,
    Pair: 65546,
    PairRest: 65547,
    Array: 65548,
    ArrayPrime: 65549,
    Value: 65550,
    ValueRest: 65551,
    String: 65552,

    // End of input
    End: 65553,

    // The empty rule
    Epsilon: 65554
};

export { ParserToken };