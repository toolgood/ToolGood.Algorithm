import { FsmContext } from './FsmContext.js';
import { JsonException } from './JsonException.js';
import { ParserToken } from './ParserToken.js';

class Lexer {
    constructor(reader) {
        this.allow_comments = true;
        this.allow_single_quoted_strings = true;
        this.end_of_input = false;
        this.fsm_context = new FsmContext();
        this.input_buffer = 0;
        this.input_char = 0;
        this.reader = reader;
        this.state = 1;
        this.string_buffer = [];
        this.string_value = '';
        this.token = 0;
        this.unichar = 0;

        this.fsm_context.L = this;
    }

    get EndOfInput() {
        return this.end_of_input;
    }

    get Token() {
        return this.token;
    }

    get StringValue() {
        return this.string_value;
    }

    static HexValue(digit) {
        switch (digit) {
            case 'a':
            case 'A':
                return 10;
            case 'b':
            case 'B':
                return 11;
            case 'c':
            case 'C':
                return 12;
            case 'd':
            case 'D':
                return 13;
            case 'e':
            case 'E':
                return 14;
            case 'f':
            case 'F':
                return 15;
            default:
                return digit.charCodeAt(0) - '0'.charCodeAt(0);
        }
    }

    static ProcessEscChar(esc_char) {
        switch (esc_char) {
            case '"':
            case '\'':
            case '\\':
            case '/':
                return String.fromCharCode(esc_char.charCodeAt(0));
            case 'n':
                return '\n';
            case 't':
                return '\t';
            case 'r':
                return '\r';
            case 'b':
                return '\b';
            case 'f':
                return '\f';
            default:
                return '?';
        }
    }

    GetChar() {
        if ((this.input_char = this.NextChar()) !== -1) return true;
        this.end_of_input = true;
        return false;
    }

    NextChar() {
        if (this.input_buffer !== 0) {
            const tmp = this.input_buffer;
            this.input_buffer = 0;
            return tmp;
        }
        return this.reader.Read();
    }

    NextToken() {
        let handler;
        this.fsm_context.Return = false;

        while (true) {
            handler = Lexer.fsm_handler_table[this.state - 1];

            if (!handler(this.fsm_context)) {
                throw JsonException.createWithChar(this.input_char);
            }

            if (this.end_of_input) return false;

            if (this.fsm_context.Return) {
                this.string_value = this.string_buffer.join('');
                this.string_buffer.length = 0;
                this.token = Lexer.fsm_return_table[this.state - 1];

                if (this.token === ParserToken.Char) {
                    this.token = this.input_char;
                }

                this.state = this.fsm_context.NextState;

                return true;
            }

            this.state = this.fsm_context.NextState;
        }
    }

    UngetChar() {
        this.input_buffer = this.input_char;
    }
}

// FSM tables and state handlers
Lexer.fsm_handler_table = [
    Lexer.State1,
    Lexer.State2,
    Lexer.State3,
    Lexer.State4,
    Lexer.State5,
    Lexer.State6,
    Lexer.State7,
    Lexer.State8,
    Lexer.State9,
    Lexer.State10,
    Lexer.State11,
    Lexer.State12,
    Lexer.State13,
    Lexer.State14,
    Lexer.State15,
    Lexer.State16,
    Lexer.State17,
    Lexer.State18,
    Lexer.State19,
    Lexer.State20,
    Lexer.State21,
    Lexer.State22,
    Lexer.State23,
    Lexer.State24,
    Lexer.State25,
    Lexer.State26,
    Lexer.State27,
    Lexer.State28
];

Lexer.fsm_return_table = [
    ParserToken.Char,
    0,
    ParserToken.Number,
    ParserToken.Number,
    0,
    ParserToken.Number,
    0,
    ParserToken.Number,
    0,
    0,
    ParserToken.True,
    0,
    0,
    0,
    ParserToken.False,
    0,
    0,
    ParserToken.Null,
    ParserToken.CharSeq,
    ParserToken.Char,
    0,
    0,
    ParserToken.CharSeq,
    ParserToken.Char,
    0,
    0,
    0,
    0
];

// State handler methods
Lexer.State1 = function(ctx) {
    while (ctx.L.GetChar()) {
        if (ctx.L.input_char === ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
            continue;
        }

        if (ctx.L.input_char >= '1' && ctx.L.input_char <= '9') {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            ctx.NextState = 3;
            return true;
        }

        switch (ctx.L.input_char) {
            case '"':
                ctx.NextState = 19;
                ctx.Return = true;
                return true;

            case ',':
            case ':':
            case '[':
            case ']':
            case '{':
            case '}':
                ctx.NextState = 1;
                ctx.Return = true;
                return true;

            case '-':
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.NextState = 2;
                return true;

            case '0':
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.NextState = 4;
                return true;

            case 'f':
                ctx.NextState = 12;
                return true;

            case 'n':
                ctx.NextState = 16;
                return true;

            case 't':
                ctx.NextState = 9;
                return true;

            case '\'':
                if (!ctx.L.allow_single_quoted_strings) return false;
                ctx.L.input_char = '"'.charCodeAt(0);
                ctx.NextState = 23;
                ctx.Return = true;
                return true;

            case '/':
                if (!ctx.L.allow_comments) return false;
                ctx.NextState = 25;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.State2 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char >= '1' && ctx.L.input_char <= '9') {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.NextState = 3;
        return true;
    }

    if (ctx.L.input_char === '0') {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.NextState = 4;
        return true;
    }

    return false;
};

Lexer.State3 = function(ctx) {
    while (ctx.L.GetChar()) {
        if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            continue;
        }

        if (ctx.L.input_char === ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        switch (ctx.L.input_char) {
            case ',':
            case ']':
            case '}':
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            case '.':
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.NextState = 5;
                return true;

            case 'e':
            case 'E':
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.NextState = 7;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.State4 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
        ctx.Return = true;
        ctx.NextState = 1;
        return true;
    }

    switch (ctx.L.input_char) {
        case ',':
        case ']':
        case '}':
            ctx.L.UngetChar();
            ctx.Return = true;
            ctx.NextState = 1;
            return true;

        case '.':
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            ctx.NextState = 5;
            return true;

        case 'e':
        case 'E':
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            ctx.NextState = 7;
            return true;

        default:
            return false;
    }
};

Lexer.State5 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.NextState = 6;
        return true;
    }

    return false;
};

Lexer.State6 = function(ctx) {
    while (ctx.L.GetChar()) {
        if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            continue;
        }

        if (ctx.L.input_char === ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        switch (ctx.L.input_char) {
            case ',':
            case ']':
            case '}':
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            case 'e':
            case 'E':
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.NextState = 7;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.State7 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.NextState = 8;
        return true;
    }

    if (ctx.L.input_char === '+' || ctx.L.input_char === '-') {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.NextState = 8;
        return true;
    }

    return false;
};

Lexer.State8 = function(ctx) {
    while (ctx.L.GetChar()) {
        if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            continue;
        }

        if (ctx.L.input_char === ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
            ctx.Return = true;
            ctx.NextState = 1;
            return true;
        }

        switch (ctx.L.input_char) {
            case ',':
            case ']':
            case '}':
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.State9 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'r'.charCodeAt(0)) {
        ctx.NextState = 10;
        return true;
    }

    return false;
};

Lexer.State10 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'u'.charCodeAt(0)) {
        ctx.NextState = 11;
        return true;
    }

    return false;
};

Lexer.State11 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'e'.charCodeAt(0)) {
        ctx.Return = true;
        ctx.NextState = 1;
        return true;
    }

    return false;
};

Lexer.State12 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'a'.charCodeAt(0)) {
        ctx.NextState = 13;
        return true;
    }

    return false;
};

Lexer.State13 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'l'.charCodeAt(0)) {
        ctx.NextState = 14;
        return true;
    }

    return false;
};

Lexer.State14 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 's'.charCodeAt(0)) {
        ctx.NextState = 15;
        return true;
    }

    return false;
};

Lexer.State15 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'e'.charCodeAt(0)) {
        ctx.Return = true;
        ctx.NextState = 1;
        return true;
    }

    return false;
};

Lexer.State16 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'u'.charCodeAt(0)) {
        ctx.NextState = 17;
        return true;
    }

    return false;
};

Lexer.State17 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'l'.charCodeAt(0)) {
        ctx.NextState = 18;
        return true;
    }

    return false;
};

Lexer.State18 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'l'.charCodeAt(0)) {
        ctx.Return = true;
        ctx.NextState = 1;
        return true;
    }

    return false;
};

Lexer.State19 = function(ctx) {
    while (ctx.L.GetChar()) {
        switch (ctx.L.input_char) {
            case '"'.charCodeAt(0):
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 20;
                return true;

            case '\\'.charCodeAt(0):
                ctx.StateStack = 19;
                ctx.NextState = 21;
                return true;

            default:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                continue;
        }
    }
    return true;
};

Lexer.State20 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === '"'.charCodeAt(0)) {
        ctx.Return = true;
        ctx.NextState = 1;
        return true;
    }

    return false;
};

Lexer.State21 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === 'u'.charCodeAt(0)) {
        ctx.NextState = 22;
        return true;
    }

    switch (ctx.L.input_char) {
        case '"'.charCodeAt(0):
        case '\''.charCodeAt(0):
        case '/'.charCodeAt(0):
        case '\\'.charCodeAt(0):
        case 'b'.charCodeAt(0):
        case 'f'.charCodeAt(0):
        case 'n'.charCodeAt(0):
        case 'r'.charCodeAt(0):
        case 't'.charCodeAt(0):
            ctx.L.string_buffer.push(Lexer.ProcessEscChar(String.fromCharCode(ctx.L.input_char)));
            ctx.NextState = ctx.StateStack;
            return true;

        default:
            return false;
    }
};

Lexer.State22 = function(ctx) {
    let counter = 0;
    let mult = 4096;

    ctx.L.unichar = 0;

    while (ctx.L.GetChar()) {
        const c = String.fromCharCode(ctx.L.input_char);
        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')) {
            ctx.L.unichar += Lexer.HexValue(c) * mult;
            counter++;
            mult /= 16;

            if (counter === 4) {
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.unichar));
                ctx.NextState = ctx.StateStack;
                return true;
            }
            continue;
        }
        return false;
    }
    return true;
};

Lexer.State23 = function(ctx) {
    while (ctx.L.GetChar()) {
        switch (ctx.L.input_char) {
            case '\''.charCodeAt(0):
                ctx.L.UngetChar();
                ctx.Return = true;
                ctx.NextState = 24;
                return true;

            case '\\'.charCodeAt(0):
                ctx.StateStack = 23;
                ctx.NextState = 21;
                return true;

            default:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                continue;
        }
    }
    return true;
};

Lexer.State24 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === '\''.charCodeAt(0)) {
        ctx.L.input_char = '"'.charCodeAt(0);
        ctx.Return = true;
        ctx.NextState = 1;
        return true;
    }

    return false;
};

Lexer.State25 = function(ctx) {
    ctx.L.GetChar();

    if (ctx.L.input_char === '*'.charCodeAt(0)) {
        ctx.NextState = 27;
        return true;
    }

    if (ctx.L.input_char === '/'.charCodeAt(0)) {
        ctx.NextState = 26;
        return true;
    }

    return false;
};

Lexer.State26 = function(ctx) {
    while (ctx.L.GetChar()) {
        if (ctx.L.input_char === '\n'.charCodeAt(0)) {
            ctx.NextState = 1;
            return true;
        }
    }
    return true;
};

Lexer.State27 = function(ctx) {
    while (ctx.L.GetChar()) {
        if (ctx.L.input_char === '*'.charCodeAt(0)) {
            ctx.NextState = 28;
            return true;
        }
    }
    return true;
};

Lexer.State28 = function(ctx) {
    while (ctx.L.GetChar()) {
        if (ctx.L.input_char === '*'.charCodeAt(0)) continue;

        if (ctx.L.input_char === '/'.charCodeAt(0)) {
            ctx.NextState = 1;
            return true;
        }

        ctx.NextState = 27;
        return true;
    }
    return true;
};

export { Lexer };