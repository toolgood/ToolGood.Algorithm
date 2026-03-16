package toolgood.algorithm.litJson;

import java.io.Reader;
import java.io.StringReader;

public class Lexer {
    private interface StateHandler {
        boolean run(FsmContext ctx);
    }

    private static final int[] fsm_return_table;
    private static final StateHandler[] fsm_handler_table;

    private boolean allow_comments = true;
    private boolean allow_single_quoted_strings = true;
    private boolean end_of_input = false;
    private final FsmContext fsm_context = new FsmContext();
    private int input_buffer = 0;
    private int input_char = 0;
    private final Reader reader;
    private int state = 1;
    private final StringBuilder string_buffer = new StringBuilder(128);
    private String string_value = "";
    private int token = 0;
    private int unichar = 0;

    public boolean EndOfInput() {
        return end_of_input;
    }

    public int Token() {
        return token;
    }

    public String StringValue() {
        return string_value;
    }

    public Lexer(Reader reader) {
        this.reader = reader;
    }

    public Lexer(String input) {
        this.reader = new StringReader(input);
    }

    static {
        fsm_handler_table = new StateHandler[]{
            Lexer::State1,
            Lexer::State2,
            Lexer::State3,
            Lexer::State4,
            Lexer::State5,
            Lexer::State6,
            Lexer::State7,
            Lexer::State8,
            Lexer::State9,
            Lexer::State10,
            Lexer::State11,
            Lexer::State12,
            Lexer::State13,
            Lexer::State14,
            Lexer::State15,
            Lexer::State16,
            Lexer::State17,
            Lexer::State18,
            Lexer::State19,
            Lexer::State20,
            Lexer::State21,
            Lexer::State22,
            Lexer::State23,
            Lexer::State24,
            Lexer::State25,
            Lexer::State26,
            Lexer::State27,
            Lexer::State28
        };

        fsm_return_table = new int[]{
            (int) ParserToken.Char,
            0,
            (int) ParserToken.Number,
            (int) ParserToken.Number,
            0,
            (int) ParserToken.Number,
            0,
            (int) ParserToken.Number,
            0,
            0,
            (int) ParserToken.True,
            0,
            0,
            0,
            (int) ParserToken.False,
            0,
            0,
            (int) ParserToken.Null,
            (int) ParserToken.CharSeq,
            (int) ParserToken.Char,
            0,
            0,
            (int) ParserToken.CharSeq,
            (int) ParserToken.Char,
            0,
            0,
            0,
            0
        };
    }

    private static int HexValue(int digit) {
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
                return digit - '0';
        }
    }

    private static char ProcessEscChar(int esc_char) {
        switch (esc_char) {
            case '"':
            case '\'':
            case '\\':
            case '/':
                return (char) esc_char;
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

    private static boolean State1(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            if (ctx.L.input_char == ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r'))
                continue;

            if (ctx.L.input_char >= '1' && ctx.L.input_char <= '9') {
                ctx.L.string_buffer.append((char) ctx.L.input_char);
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
                    ctx.L.string_buffer.append((char) ctx.L.input_char);
                    ctx.NextState = 2;
                    return true;

                case '0':
                    ctx.L.string_buffer.append((char) ctx.L.input_char);
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

                    ctx.L.input_char = '"';
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
    }

    private static boolean State2(FsmContext ctx) {
        ctx.L.GetChar();

        if (ctx.L.input_char >= '1' && ctx.L.input_char <= '9') {
            ctx.L.string_buffer.append((char) ctx.L.input_char);
            ctx.NextState = 3;
            return true;
        }

        switch (ctx.L.input_char) {
            case '0':
                ctx.L.string_buffer.append((char) ctx.L.input_char);
                ctx.NextState = 4;
                return true;

            default:
                return false;
        }
    }

    private static boolean State3(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                ctx.L.string_buffer.append((char) ctx.L.input_char);
                continue;
            }

            if (ctx.L.input_char == ' ' ||
                (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
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
                    ctx.L.string_buffer.append((char) ctx.L.input_char);
                    ctx.NextState = 5;
                    return true;

                case 'e':
                case 'E':
                    ctx.L.string_buffer.append((char) ctx.L.input_char);
                    ctx.NextState = 7;
                    return true;

                default:
                    return false;
            }
        }
        return true;
    }

    private static boolean State4(FsmContext ctx) {
        ctx.L.GetChar();

        if (ctx.L.input_char == ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
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
                ctx.L.string_buffer.append((char) ctx.L.input_char);
                ctx.NextState = 5;
                return true;

            case 'e':
            case 'E':
                ctx.L.string_buffer.append((char) ctx.L.input_char);
                ctx.NextState = 7;
                return true;

            default:
                return false;
        }
    }

    private static boolean State5(FsmContext ctx) {
        ctx.L.GetChar();

        if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
            ctx.L.string_buffer.append((char) ctx.L.input_char);
            ctx.NextState = 6;
            return true;
        }

        return false;
    }

    private static boolean State6(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                ctx.L.string_buffer.append((char) ctx.L.input_char);
                continue;
            }

            if (ctx.L.input_char == ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
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
                    ctx.L.string_buffer.append((char) ctx.L.input_char);
                    ctx.NextState = 7;
                    return true;

                default:
                    return false;
            }
        }

        return true;
    }

    private static boolean State7(FsmContext ctx) {
        ctx.L.GetChar();

        if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
            ctx.L.string_buffer.append((char) ctx.L.input_char);
            ctx.NextState = 8;
            return true;
        }

        switch (ctx.L.input_char) {
            case '+':
            case '-':
                ctx.L.string_buffer.append((char) ctx.L.input_char);
                ctx.NextState = 8;
                return true;

            default:
                return false;
        }
    }

    private static boolean State8(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9') {
                ctx.L.string_buffer.append((char) ctx.L.input_char);
                continue;
            }

            if (ctx.L.input_char == ' ' || (ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')) {
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
    }

    private static boolean State9(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'r':
                ctx.NextState = 10;
                return true;

            default:
                return false;
        }
    }

    private static boolean State10(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'u':
                ctx.NextState = 11;
                return true;

            default:
                return false;
        }
    }

    private static boolean State11(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'e':
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            default:
                return false;
        }
    }

    private static boolean State12(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'a':
                ctx.NextState = 13;
                return true;

            default:
                return false;
        }
    }

    private static boolean State13(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'l':
                ctx.NextState = 14;
                return true;

            default:
                return false;
        }
    }

    private static boolean State14(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 's':
                ctx.NextState = 15;
                return true;

            default:
                return false;
        }
    }

    private static boolean State15(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'e':
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            default:
                return false;
        }
    }

    private static boolean State16(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'u':
                ctx.NextState = 17;
                return true;

            default:
                return false;
        }
    }

    private static boolean State17(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'l':
                ctx.NextState = 18;
                return true;

            default:
                return false;
        }
    }

    private static boolean State18(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'l':
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            default:
                return false;
        }
    }

    private static boolean State19(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            switch (ctx.L.input_char) {
                case '"':
                    ctx.L.UngetChar();
                    ctx.Return = true;
                    ctx.NextState = 20;
                    return true;

                case '\\':
                    ctx.StateStack = 19;
                    ctx.NextState = 21;
                    return true;

                default:
                    ctx.L.string_buffer.append((char) ctx.L.input_char);
                    continue;
            }
        }

        return true;
    }

    private static boolean State20(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case '"':
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            default:
                return false;
        }
    }

    private static boolean State21(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case 'u':
                ctx.NextState = 22;
                return true;

            case '"':
            case '\'':
            case '/':
            case '\\':
            case 'b':
            case 'f':
            case 'n':
            case 'r':
            case 't':
                ctx.L.string_buffer.append(ProcessEscChar(ctx.L.input_char));
                ctx.NextState = ctx.StateStack;
                return true;

            default:
                return false;
        }
    }

    private static boolean State22(FsmContext ctx) {
        int counter = 0;
        int mult = 4096;

        ctx.L.unichar = 0;

        while (ctx.L.GetChar()) {
            if ((ctx.L.input_char >= '0' && ctx.L.input_char <= '9') ||
                (ctx.L.input_char >= 'A' && ctx.L.input_char <= 'F') ||
                (ctx.L.input_char >= 'a' && ctx.L.input_char <= 'f')) {
                ctx.L.unichar += HexValue(ctx.L.input_char) * mult;

                counter++;
                mult /= 16;

                if (counter == 4) {
                    ctx.L.string_buffer.append((char) ctx.L.unichar);
                    ctx.NextState = ctx.StateStack;
                    return true;
                }

                continue;
            }

            return false;
        }

        return true;
    }

    private static boolean State23(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            switch (ctx.L.input_char) {
                case '\'':
                    ctx.L.UngetChar();
                    ctx.Return = true;
                    ctx.NextState = 24;
                    return true;

                case '\\':
                    ctx.StateStack = 23;
                    ctx.NextState = 21;
                    return true;

                default:
                    ctx.L.string_buffer.append((char) ctx.L.input_char);
                    continue;
            }
        }

        return true;
    }

    private static boolean State24(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case '\'':
                ctx.L.input_char = '"';
                ctx.Return = true;
                ctx.NextState = 1;
                return true;

            default:
                return false;
        }
    }

    private static boolean State25(FsmContext ctx) {
        ctx.L.GetChar();

        switch (ctx.L.input_char) {
            case '*':
                ctx.NextState = 27;
                return true;

            case '/':
                ctx.NextState = 26;
                return true;

            default:
                return false;
        }
    }

    private static boolean State26(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            if (ctx.L.input_char == '\n') {
                ctx.NextState = 1;
                return true;
            }
        }

        return true;
    }

    private static boolean State27(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            if (ctx.L.input_char == '*') {
                ctx.NextState = 28;
                return true;
            }
        }

        return true;
    }

    private static boolean State28(FsmContext ctx) {
        while (ctx.L.GetChar()) {
            if (ctx.L.input_char == '*') continue;

            if (ctx.L.input_char == '/') {
                ctx.NextState = 1;
                return true;
            }

            ctx.NextState = 27;
            return true;
        }

        return true;
    }

    private boolean GetChar() {
        if ((input_char = NextChar()) != -1) return true;

        end_of_input = true;
        return false;
    }

    private int NextChar() {
        if (input_buffer != 0) {
            int tmp = input_buffer;
            input_buffer = 0;

            return tmp;
        }

        try {
            return reader.read();
        } catch (java.io.IOException e) {
            return -1;
        }
    }

    public boolean NextToken() {
        StateHandler handler;
        fsm_context.Return = false;
        fsm_context.L = this;

        while (true) {
            handler = fsm_handler_table[state - 1];

            if (!handler.run(fsm_context)) throw new JsonException(input_char);

            if (end_of_input) return false;

            if (fsm_context.Return) {
                string_value = string_buffer.toString();
                string_buffer.setLength(0);
                token = fsm_return_table[state - 1];

                if (token == (int) ParserToken.Char) token = input_char;

                state = fsm_context.NextState;

                return true;
            }

            state = fsm_context.NextState;
        }
    }

    private void UngetChar() {
        input_buffer = input_char;
    }
}
