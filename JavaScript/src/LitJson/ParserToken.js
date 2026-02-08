let ParserToken = {
    // Lexer tokens (see section A.1.1. of the manual)
    none: 65536, // System.Char.MaxValue + 1

    number: 65537,
    true: 65538,
    false: 65539,
    null: 65540,
    charSeq: 65541,

    // Single char
    char: 65542,

    // Parser Rules (see section A.2.1 of the manual)
    text: 65543,

    object: 65544,
    objectPrime: 65545,
    pair: 65546,
    pairRest: 65547,
    array: 65548,
    arrayPrime: 65549,
    value: 65550,
    valueRest: 65551,
    string: 65552,

    // End of input
    end: 65553,

    // The empty rule
    epsilon: 65554
};

export { ParserToken };