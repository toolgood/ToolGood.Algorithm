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

    static hexValue(digit) {
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
                return digit.charCodeAt(0) - 48 /*'0'.charCodeAt(0)*/;
        }
    }

    static processEscChar(esc_char) {
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

    getChar() {
        if ((this.input_char = this.nextChar()) !== -1) return true;
        this.end_of_input = true;
        return false;
    }

    nextChar() {
        if (this.input_buffer !== 0) {
            let tmp = this.input_buffer;
            this.input_buffer = 0;
            return tmp;
        }
        return this.reader.read();
    }

    nextToken() {
        let handler;
        this.fsm_context.ret = false;

        while (true) {
            handler = Lexer.fsm_handler_table[this.state - 1];

            if (!handler(this.fsm_context)) {
                throw JsonException.createWithChar(this.input_char);
            }

            if (this.end_of_input) return false;

            if (this.fsm_context.ret) {
                this.string_value = this.string_buffer.join('');
                this.string_buffer.length = 0;
                this.token = Lexer.fsm_return_table[this.state - 1];

                if (this.token === ParserToken.Char) {
                    this.token = this.input_char;
                }

                this.state = this.fsm_context.nextState;

                return true;
            }

            this.state = this.fsm_context.nextState;
        }
    }

    ungetChar() {
        this.input_buffer = this.input_char;
    }
}


// State handler methods
Lexer.state1 = function(ctx) {
    while (ctx.L.getChar()) {
        if (ctx.L.input_char === 32 /*' '.charCodeAt(0)*/ || (ctx.L.input_char >= 9 /*'\t'.charCodeAt(0)*/ && ctx.L.input_char <= 13 /*'\r'.charCodeAt(0)*/)) {
            continue;
        }

        if (ctx.L.input_char >= 49 /*'1'.charCodeAt(0)*/ && ctx.L.input_char <= 57 /*'9'.charCodeAt(0)*/) {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            ctx.nextState = 3;
            return true;
        }

        switch (ctx.L.input_char) {
            case 34 /*'"'.charCodeAt(0)*/:
                ctx.nextState = 19;
                ctx.ret = true;
                return true;

            case 44 /*','.charCodeAt(0)*/:
            case 58 /*':'.charCodeAt(0)*/:
            case 91 /*'['.charCodeAt(0)*/:
            case 93 /*']'.charCodeAt(0)*/:
            case 123 /*'{'.charCodeAt(0)*/:
            case 125 /*'}'.charCodeAt(0)*/:
                ctx.nextState = 1;
                ctx.ret = true;
                return true;

            case 45 /*'-'.charCodeAt(0)*/:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.nextState = 2;
                return true;

            case 48 /*'0'.charCodeAt(0)*/:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.nextState = 4;
                return true;

            case 102 /*'f'.charCodeAt(0)*/:
                ctx.nextState = 12;
                return true;

            case 110 /*'n'.charCodeAt(0)*/:
                ctx.nextState = 16;
                return true;

            case 116 /*'t'.charCodeAt(0)*/:
                ctx.nextState = 9;
                return true;

            case 39 /*'\''.charCodeAt(0)*/:
                if (!ctx.L.allow_single_quoted_strings) return false;
                ctx.L.input_char = 34 /*'"'.charCodeAt(0)*/;
                ctx.nextState = 23;
                ctx.ret = true;
                return true;

            case 47 /*'/'.charCodeAt(0)*/:
                if (!ctx.L.allow_comments) return false;
                ctx.nextState = 25;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.state2 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char >= 49 /*'1'.charCodeAt(0)*/ && ctx.L.input_char <= 57 /*'9'.charCodeAt(0)*/) {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.nextState = 3;
        return true;
    }

    if (ctx.L.input_char === 48 /*'0'.charCodeAt(0)*/) {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.nextState = 4;
        return true;
    }

    return false;
};

Lexer.state3 = function(ctx) {
    while (ctx.L.getChar()) {
        if (ctx.L.input_char >= 48 /*'0'.charCodeAt(0)*/ && ctx.L.input_char <= 57 /*'9'.charCodeAt(0)*/) {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            continue;
        }

        if (ctx.L.input_char === 32 /*' '.charCodeAt(0)*/ || (ctx.L.input_char >= 9 /*'\t'.charCodeAt(0)*/ && ctx.L.input_char <= 13 /*'\r'.charCodeAt(0)*/)) {
            ctx.ret = true;
            ctx.nextState = 1;
            return true;
        }

        switch (ctx.L.input_char) {
            case 44 /*','.charCodeAt(0)*/:
            case 93 /*']'.charCodeAt(0)*/:
            case 125 /*'}'.charCodeAt(0)*/:
                ctx.L.ungetChar();
                ctx.ret = true;
                ctx.nextState = 1;
                return true;

            case 46 /*'.'.charCodeAt(0)*/:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.nextState = 5;
                return true;

            case 101 /*'e'.charCodeAt(0)*/:
            case 69 /*'E'.charCodeAt(0)*/:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.nextState = 7;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.state4 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 32 /*' '.charCodeAt(0)*/ || (ctx.L.input_char >= 9 /*'\t'.charCodeAt(0)*/ && ctx.L.input_char <= 13 /*'\r'.charCodeAt(0)*/)) {
        ctx.ret = true;
        ctx.nextState = 1;
        return true;
    }

    switch (ctx.L.input_char) {
        case 44 /*','.charCodeAt(0)*/:
        case 93 /*']'.charCodeAt(0)*/:
        case 125 /*'}'.charCodeAt(0)*/:
            ctx.L.ungetChar();
            ctx.ret = true;
            ctx.nextState = 1;
            return true;

        case 46 /*'.'.charCodeAt(0)*/:
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            ctx.nextState = 5;
            return true;

        case 101 /*'e'.charCodeAt(0)*/:
        case 69 /*'E'.charCodeAt(0)*/:
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            ctx.nextState = 7;
            return true;

        default:
            return false;
    }
};

Lexer.state5 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char >= 48 /*'0'.charCodeAt(0)*/ && ctx.L.input_char <= 57 /*'9'.charCodeAt(0)*/) {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.nextState = 6;
        return true;
    }

    return false;
};

Lexer.state6 = function(ctx) {
    while (ctx.L.getChar()) {
        if (ctx.L.input_char >= 48 /*'0'.charCodeAt(0)*/ && ctx.L.input_char <= 57 /*'9'.charCodeAt(0)*/) {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            continue;
        }

        if (ctx.L.input_char === 32 /*' '.charCodeAt(0)*/ || (ctx.L.input_char >= 9 /*'\t'.charCodeAt(0)*/ && ctx.L.input_char <= 13 /*'\r'.charCodeAt(0)*/)) {
            ctx.ret = true;
            ctx.nextState = 1;
            return true;
        }

        switch (ctx.L.input_char) {
            case 44 /*','.charCodeAt(0)*/:
            case 93 /*']'.charCodeAt(0)*/:
            case 125 /*'}'.charCodeAt(0)*/:
                ctx.L.ungetChar();
                ctx.ret = true;
                ctx.nextState = 1;
                return true;

            case 101 /*'e'.charCodeAt(0)*/:
            case 69 /*'E'.charCodeAt(0)*/:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                ctx.nextState = 7;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.state7 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char >= 48 /*'0'.charCodeAt(0)*/ && ctx.L.input_char <= 57 /*'9'.charCodeAt(0)*/) {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.nextState = 8;
        return true;
    }

    if (ctx.L.input_char === 43 /*'+'.charCodeAt(0)*/ || ctx.L.input_char === 45 /*'-'.charCodeAt(0)*/) {
        ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
        ctx.nextState = 8;
        return true;
    }

    return false;
};

Lexer.state8 = function(ctx) {
    while (ctx.L.getChar()) {
        if (ctx.L.input_char >= 48 /*'0'.charCodeAt(0)*/ && ctx.L.input_char <= 57 /*'9'.charCodeAt(0)*/) {
            ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
            continue;
        }

        if (ctx.L.input_char === 32 /*' '.charCodeAt(0)*/ || (ctx.L.input_char >= 9 /*'\t'.charCodeAt(0)*/ && ctx.L.input_char <= 13 /*'\r'.charCodeAt(0)*/)) {
            ctx.ret = true;
            ctx.nextState = 1;
            return true;
        }

        switch (ctx.L.input_char) {
            case 44 /*','.charCodeAt(0)*/:
            case 93 /*']'.charCodeAt(0)*/:
            case 125 /*'}'.charCodeAt(0)*/:
                ctx.L.ungetChar();
                ctx.ret = true;
                ctx.nextState = 1;
                return true;

            default:
                return false;
        }
    }
    return true;
};

Lexer.state9 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 114 /*'r'.charCodeAt(0)*/) {
        ctx.nextState = 10;
        return true;
    }

    return false;
};

Lexer.state10 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 117 /*'u'.charCodeAt(0)*/) {
        ctx.nextState = 11;
        return true;
    }

    return false;
};

Lexer.state11 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 101 /*'e'.charCodeAt(0)*/) {
        ctx.ret = true;
        ctx.nextState = 1;
        return true;
    }

    return false;
};

Lexer.state12 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 97 /*'a'.charCodeAt(0)*/) {
        ctx.nextState = 13;
        return true;
    }

    return false;
};

Lexer.state13 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 108 /*'l'.charCodeAt(0)*/) {
        ctx.nextState = 14;
        return true;
    }

    return false;
};

Lexer.state14 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 115 /*'s'.charCodeAt(0)*/) {
        ctx.nextState = 15;
        return true;
    }

    return false;
};

Lexer.state15 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 101 /*'e'.charCodeAt(0)*/) {
        ctx.ret = true;
        ctx.nextState = 1;
        return true;
    }

    return false;
};

Lexer.state16 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 117 /*'u'.charCodeAt(0)*/) {
        ctx.nextState = 17;
        return true;
    }

    return false;
};

Lexer.state17 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 108 /*'l'.charCodeAt(0)*/) {
        ctx.nextState = 18;
        return true;
    }

    return false;
};

Lexer.state18 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 108 /*'l'.charCodeAt(0)*/) {
        ctx.ret = true;
        ctx.nextState = 1;
        return true;
    }

    return false;
};

Lexer.state19 = function(ctx) {
    while (ctx.L.getChar()) {
        switch (ctx.L.input_char) {
            case 34 /*'"'.charCodeAt(0)*/:
                ctx.L.ungetChar();
                ctx.ret = true;
                ctx.nextState = 20;
                return true;

            case 92 /*'\\'.charCodeAt(0)*/:
                ctx.stateStack = 19;
                ctx.nextState = 21;
                return true;

            default:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                continue;
        }
    }
    return true;
};

Lexer.state20 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 34 /*'"'.charCodeAt(0)*/) {
        ctx.ret = true;
        ctx.nextState = 1;
        return true;
    }

    return false;
};

Lexer.state21 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 117 /*'u'.charCodeAt(0)*/) {
        ctx.nextState = 22;
        return true;
    }

    switch (ctx.L.input_char) {
        case 34 /*'"'.charCodeAt(0)*/:
        case 39 /*'\''.charCodeAt(0)*/:
        case 47 /*'/'.charCodeAt(0)*/:
        case 92 /*'\\'.charCodeAt(0)*/:
        case 98 /*'b'.charCodeAt(0)*/:
        case 102 /*'f'.charCodeAt(0)*/:
        case 110 /*'n'.charCodeAt(0)*/:
        case 114 /*'r'.charCodeAt(0)*/:
        case 116 /*'t'.charCodeAt(0)*/:
            ctx.L.string_buffer.push(Lexer.processEscChar(String.fromCharCode(ctx.L.input_char)));
            ctx.nextState = ctx.stateStack;
            return true;

        default:
            return false;
    }
};

Lexer.state22 = function(ctx) {
    let counter = 0;
    let mult = 4096;

    ctx.L.unichar = 0;

    while (ctx.L.getChar()) {
        let c = String.fromCharCode(ctx.L.input_char);
        if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f')) {
                ctx.L.unichar += Lexer.hexValue(c) * mult;
            counter++;
            mult /= 16;

            if (counter === 4) {
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.unichar));
                ctx.nextState = ctx.stateStack;
                return true;
            }
            continue;
        }
        return false;
    }
    return true;
};

Lexer.state23 = function(ctx) {
    while (ctx.L.getChar()) {
        switch (ctx.L.input_char) {
            case 39 /*'\''.charCodeAt(0)*/:
                ctx.L.ungetChar();
                ctx.ret = true;
                ctx.nextState = 24;
                return true;

            case 92 /*'\\'.charCodeAt(0)*/:
                ctx.stateStack = 23;
                ctx.nextState = 21;
                return true;

            default:
                ctx.L.string_buffer.push(String.fromCharCode(ctx.L.input_char));
                continue;
        }
    }
    return true;
};

Lexer.state24 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 39 /*'\''.charCodeAt(0)*/) {
        ctx.L.input_char = 34 /*'"'.charCodeAt(0)*/;
        ctx.ret = true;
        ctx.nextState = 1;
        return true;
    }

    return false;
};

Lexer.state25 = function(ctx) {
    ctx.L.getChar();

    if (ctx.L.input_char === 42 /*'*'.charCodeAt(0)*/) {
        ctx.nextState = 27;
        return true;
    }

    if (ctx.L.input_char === 47 /*'/'.charCodeAt(0)*/) {
        ctx.nextState = 26;
        return true;
    }

    return false;
};

Lexer.state26 = function(ctx) {
    while (ctx.L.getChar()) {
        if (ctx.L.input_char === 10 /*'\n'.charCodeAt(0)*/) {
            ctx.nextState = 1;
            return true;
        }
    }
    return true;
};

Lexer.state27 = function(ctx) {
    while (ctx.L.getChar()) {
        if (ctx.L.input_char === 42 /*'*'.charCodeAt(0)*/) {
            ctx.nextState = 28;
            return true;
        }
    }
    return true;
};

Lexer.state28 = function(ctx) {
    while (ctx.L.getChar()) {
        if (ctx.L.input_char === 42 /*'*'.charCodeAt(0)*/) continue;

        if (ctx.L.input_char === 47 /*'/'.charCodeAt(0)*/) {
            ctx.nextState = 1;
            return true;
        }

        ctx.nextState = 27;
        return true;
    }
    return true;
};

// FSM tables and state handlers
Lexer.fsm_handler_table = [
    Lexer.state1,
    Lexer.state2,
    Lexer.state3,
    Lexer.state4,
    Lexer.state5,
    Lexer.state6,
    Lexer.state7,
    Lexer.state8,
    Lexer.state9,
    Lexer.state10,
    Lexer.state11,
    Lexer.state12,
    Lexer.state13,
    Lexer.state14,
    Lexer.state15,
    Lexer.state16,
    Lexer.state17,
    Lexer.state18,
    Lexer.state19,
    Lexer.state20,
    Lexer.state21,
    Lexer.state22,
    Lexer.state23,
    Lexer.state24,
    Lexer.state25,
    Lexer.state26,
    Lexer.state27,
    Lexer.state28
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
export { Lexer };