import { Lexer } from './Lexer.js';
import { ParserToken } from './ParserToken.js';
import { JsonException } from './JsonException.js';
import { StringReader } from './StringReader.js';
import { JsonToken } from './JsonToken.js';

class JsonReader {
    constructor(json_text) {
        let reader = new StringReader(json_text);

        this.parser_in_string = false;
        this.parser_return = false;

        this.read_started = false;
        this.automaton_stack = [];
        this.automaton_stack.push(ParserToken.end);
        this.automaton_stack.push(ParserToken.text);

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
        } else if (this.current_symbol === ParserToken.charSeq) {
            this.token_value = this.lexer.StringValue;
        } else if (this.current_symbol === ParserToken.false) {
            this.token = JsonToken.Boolean;
            this.token_value = false;
            this.parser_return = true;
        } else if (this.current_symbol === ParserToken.null) {
            this.token = JsonToken.Null;
            this.parser_return = true;
        } else if (this.current_symbol === ParserToken.number) {
            this.processNumber(this.lexer.StringValue);
            this.token = JsonToken.Double;
            this.parser_return = true;
        } else if (this.current_symbol === ParserToken.pair) {
            this.token = JsonToken.PropertyName;
        } else if (this.current_symbol === ParserToken.true) {
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
            this.automaton_stack.push(ParserToken.end);
            this.automaton_stack.push(ParserToken.text);
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
                if (this.automaton_stack[this.automaton_stack.length - 1] === ParserToken.end) {
                    this.end_of_json = true;
                }
                return true;
            }

            this.current_symbol = this.automaton_stack.pop();

            this.processSymbol();

            if (this.current_symbol === this.current_input) {
                if (!this.readToken()) {
                    if (this.automaton_stack[this.automaton_stack.length - 1] !== ParserToken.end) {
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

            if (entry_symbols[0] === ParserToken.epsilon) {
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
    [ParserToken.array]: {
        [91 /*'['.charCodeAt(0)*/]: [91 /*'['.charCodeAt(0)*/, ParserToken.arrayPrime]
    },
    [ParserToken.arrayPrime]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.value, ParserToken.valueRest, 93 /*']'.charCodeAt(0)*/],
        [91 /*'['.charCodeAt(0)*/]: [ParserToken.value, ParserToken.valueRest, 93 /*']'.charCodeAt(0)*/],
        [93 /*']'.charCodeAt(0)*/]: [93 /*']'.charCodeAt(0)*/],
        [123 /*'{'.charCodeAt(0)*/]: [ParserToken.value, ParserToken.valueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.number]: [ParserToken.value, ParserToken.valueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.true]: [ParserToken.value, ParserToken.valueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.false]: [ParserToken.value, ParserToken.valueRest, 93 /*']'.charCodeAt(0)*/],
        [ParserToken.null]: [ParserToken.value, ParserToken.valueRest, 93 /*']'.charCodeAt(0)*/]
    },
    [ParserToken.object]: {
        [123 /*'{'.charCodeAt(0)*/]: [123 /*'{'.charCodeAt(0)*/, ParserToken.objectPrime]
    },
    [ParserToken.objectPrime]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.pair, ParserToken.pairRest, 125 /*'}'.charCodeAt(0)*/],
        [125 /*'}'.charCodeAt(0)*/]: [0 /*''.charCodeAt(0)*/]
    },
    [ParserToken.pair]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.string, 58 /*':'.charCodeAt(0)*/, ParserToken.value]
    },
    [ParserToken.pairRest]: {
        [44 /*','.charCodeAt(0)*/]: [44 /*','.charCodeAt(0)*/, ParserToken.pair, ParserToken.pairRest],
        [125 /*'}'.charCodeAt(0)*/]: [ParserToken.epsilon]
    },
    [ParserToken.string]: {
        [34 /*'"'.charCodeAt(0)*/]: [34 /*'"'.charCodeAt(0)*/, ParserToken.charSeq, 34 /*'"'.charCodeAt(0)*/]
    },
    [ParserToken.text]: {
        [91 /*'['.charCodeAt(0)*/]: [ParserToken.array],
        [123 /*'{'.charCodeAt(0)*/]: [ParserToken.object]
    },
    [ParserToken.value]: {
        [34 /*'"'.charCodeAt(0)*/]: [ParserToken.string],
        [91 /*'['.charCodeAt(0)*/]: [ParserToken.array],
        [123 /*'{'.charCodeAt(0)*/]: [ParserToken.object],
        [ParserToken.number]: [ParserToken.number],
        [ParserToken.true]: [ParserToken.true],
        [ParserToken.false]: [ParserToken.false],
        [ParserToken.null]: [ParserToken.null]
    },
    [ParserToken.valueRest]: {
        [44 /*','.charCodeAt(0)*/]: [44 /*','.charCodeAt(0)*/, ParserToken.value, ParserToken.valueRest],
        [93 /*']'.charCodeAt(0)*/]: [ParserToken.epsilon]
    }
};

export { JsonReader };