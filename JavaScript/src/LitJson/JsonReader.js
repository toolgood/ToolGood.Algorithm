import { Lexer } from './Lexer.js';
import { ParserToken } from './ParserToken.js';
import { JsonException } from './JsonException.js';
import { StringReader } from './StringReader.js';

let JsonToken = {
    None: 0,
    ObjectStart: 1,
    PropertyName: 2,
    ObjectEnd: 3,
    ArrayStart: 4,
    ArrayEnd: 5,
    Double: 6,
    String: 7,
    Boolean: 8,
    Null: 9
};

class JsonReader {
    constructor(json_text) {
        let reader = new StringReader(json_text);

        this.parser_in_string = false;
        this.parser_return = false;

        this.read_started = false;
        this.automaton_stack = [];
        this.automaton_stack.push(ParserToken.End);
        this.automaton_stack.push(ParserToken.Text);

        this.lexer = new Lexer(reader);

        this.end_of_input = false;
        this.end_of_json = false;
        this.token = JsonToken.None;
        this.token_value = null;
    }

    get Token() {
        return this.token;
    }

    get Value() {
        return this.token_value;
    }

    processNumber(number) {
        if (number.includes('.') || number.includes('e') || number.includes('E')) {
            let n_double = parseFloat(number);
            if (!isNaN(n_double)) {
                this.token_value = n_double;
                return;
            }
        } else {
            let n_int = parseInt(number, 10);
            if (!isNaN(n_int)) {
                this.token_value = n_int;
                return;
            }
        }
        this.token_value = 0;
    }

    processSymbol() {
        if (this.current_symbol === 91 /*'['.charCodeAt(0)*/) {
            this.token = JsonToken.ArrayStart;
            this.parser_return = true;
        } else if (this.current_symbol === 93 /*']'.charCodeAt(0)*/) {
            this.token = JsonToken.ArrayEnd;
            this.parser_return = true;
        } else if (this.current_symbol === 123 /*'{'.charCodeAt(0)*/) {
            this.token = JsonToken.ObjectStart;
            this.parser_return = true;
        } else if (this.current_symbol === 125 /*'}'.charCodeAt(0)*/) {
            this.token = JsonToken.ObjectEnd;
            this.parser_return = true;
        } else if (this.current_symbol === 34 /*'"'.charCodeAt(0)*/) {
            if (this.parser_in_string) {
                this.parser_in_string = false;
                this.parser_return = true;
            } else {
                if (this.token === JsonToken.None) {
                    this.token = JsonToken.String;
                }
                this.parser_in_string = true;
            }
        } else if (this.current_symbol === ParserToken.CharSeq) {
            this.token_value = this.lexer.StringValue;
        } else if (this.current_symbol === ParserToken.False) {
            this.token = JsonToken.Boolean;
            this.token_value = false;
            this.parser_return = true;
        } else if (this.current_symbol === ParserToken.Null) {
            this.token = JsonToken.Null;
            this.parser_return = true;
        } else if (this.current_symbol === ParserToken.Number) {
            this.processNumber(this.lexer.StringValue);
            this.token = JsonToken.Double;
            this.parser_return = true;
        } else if (this.current_symbol === ParserToken.Pair) {
            this.token = JsonToken.PropertyName;
        } else if (this.current_symbol === ParserToken.True) {
            this.token = JsonToken.Boolean;
            this.token_value = true;
            this.parser_return = true;
        }
    }

    readToken() {
        if (this.end_of_input) return false;

        let result = this.lexer.nextToken();

        if (this.lexer.EndOfInput) {
            this.close();
            return false;
        }

        this.current_input = this.lexer.Token;

        return result;
    }

    close() {
        if (this.end_of_input) {
            return;
        }

        this.end_of_input = true;
        this.end_of_json = true;
    }

    read() {
        if (this.end_of_input) {
            return false;
        }

        if (this.end_of_json) {
            this.end_of_json = false;
            this.automaton_stack.length = 0;
            this.automaton_stack.push(ParserToken.End);
            this.automaton_stack.push(ParserToken.Text);
        }

        this.parser_in_string = false;
        this.parser_return = false;

        this.token = JsonToken.None;
        this.token_value = null;

        if (!this.read_started) {
            this.read_started = true;

            if (!this.readToken()) {
                return false;
            }
        }

        let entry_symbols;

        while (true) {
            if (this.parser_return) {
                if (this.automaton_stack[this.automaton_stack.length - 1] === ParserToken.End) {
                    this.end_of_json = true;
                }
                return true;
            }

            this.current_symbol = this.automaton_stack.pop();

            this.processSymbol();

            if (this.current_symbol === this.current_input) {
                if (!this.readToken()) {
                    if (this.automaton_stack[this.automaton_stack.length - 1] !== ParserToken.End) {
                        throw new JsonException("Input doesn't evaluate to proper JSON text");
                    }

                    if (this.parser_return) {
                        return true;
                    }

                    return false;
                }
                continue;
            }

            try {
                entry_symbols = JsonReader.parse_table[this.current_symbol][this.current_input];
            } catch (e) {
                throw JsonException.createWithToken(this.current_input, e);
            }

            if (entry_symbols[0] === ParserToken.Epsilon) {
                continue;
            }

            for (let i = entry_symbols.length - 1; i >= 0; i--) {
                this.automaton_stack.push(entry_symbols[i]);
            }
        }
    }
}

// Parse table
JsonReader.parse_table = {
    [ParserToken.Array]: {
        [91 /*'['.charCodeAt(0)*/]: [91 /*'['.charCodeAt(0)*/, ParserToken.ArrayPrime]
    },
    [ParserToken.ArrayPrime]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.Value, ParserToken.ValueRest, 93 /*']'.charCodeAt(0)*/],
        [91 /*'['.charCodeAt(0)*/]: [ParserToken.Value, ParserToken.ValueRest, 93 /*']'.charCodeAt(0)*/],
        [93 /*']'.charCodeAt(0)*/]: [93 /*']'.charCodeAt(0)*/],
        [123 /*'{'.charCodeAt(0)*/]: [ParserToken.Value, ParserToken.ValueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.Number]: [ParserToken.Value, ParserToken.ValueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.True]: [ParserToken.Value, ParserToken.ValueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.False]: [ParserToken.Value, ParserToken.ValueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.Null]: [ParserToken.Value, ParserToken.ValueRest, 93 /*']'.charCodeAt(0)*/]
    },
    [ParserToken.Object]: {
        [123 /*'{'.charCodeAt(0)*/]: [123 /*'{'.charCodeAt(0)*/, ParserToken.ObjectPrime]
    },
    [ParserToken.ObjectPrime]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.Pair, ParserToken.PairRest, 125 /*'}'.charCodeAt(0)*/],
        [125 /*'}'.charCodeAt(0)*/]: [0 /*''.charCodeAt(0)*/]
    },
    [ParserToken.Pair]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.String, 58 /*':'.charCodeAt(0)*/, ParserToken.Value]
    },
    [ParserToken.PairRest]: {
        [44 /*','.charCodeAt(0)*/]: [44 /*','.charCodeAt(0)*/, ParserToken.Pair, ParserToken.PairRest],
        [125 /*'}'.charCodeAt(0)*/]: [ParserToken.Epsilon]
    },
    [ParserToken.String]: {
        [34 /*'"'.charCodeAt(0)*/]: [34 /*'"'.charCodeAt(0)*/, ParserToken.CharSeq, 34 /*'"'.charCodeAt(0)*/]
    },
    [ParserToken.Text]: {
        [91 /*'['.charCodeAt(0)*/]: [ParserToken.Array],
        [123 /*'{'.charCodeAt(0)*/]: [ParserToken.Object]
    },
    [ParserToken.Value]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.String],
        [91 /*'['.charCodeAt(0)*/]: [ParserToken.Array],
        [123 /*'{'.charCodeAt(0)*/]: [ParserToken.Object],
        [ParserToken.Number]: [ParserToken.Number],
        [ParserToken.True]: [ParserToken.True],
        [ParserToken.False]: [ParserToken.False],
        [ParserToken.Null]: [ParserToken.Null]
    },
    [ParserToken.ValueRest]: {
        [44 /*','.charCodeAt(0)*/]: [44 /*','.charCodeAt(0)*/, ParserToken.Value, ParserToken.ValueRest],
        [93 /*']'.charCodeAt(0)*/]: [ParserToken.Epsilon]
    }
};

export { JsonReader, JsonToken };