package toolgood.algorithm.litJson;

import java.io.IOException;
import java.io.StringReader;
import java.util.ArrayList;
import java.util.List;


public class Lexer {
    // private delegate boolean

    // StateHandler(FsmContext ctx);

    private static int[] fsm_return_table;
    private static List<Function<FsmContext, Boolean>> fsm_handler_table;

    private boolean allow_comments;
    private boolean allow_single_quoted_strings;
    private boolean end_of_input;
    private FsmContext fsm_context;
    private int input_buffer;
    private int input_char;
    private StringReader reader;
    private int state;
    private StringBuilder string_buffer;
    private String string_value;
    private int token;
    private int unichar;

    public boolean EndOfInput() {
        return end_of_input;
    }

    public int Token() {
        return token;
    }

    public String StringValue() {
        return string_value;
    }

    static {
        PopulateFsmTables();
    }

    public Lexer(StringReader _reader) {
        allow_comments = true;
        allow_single_quoted_strings = true;

        input_buffer = 0;
        string_buffer = new StringBuilder(128);
        state = 1;
        end_of_input = false;
        reader = _reader;

        fsm_context = new FsmContext();
        fsm_context.L = this;
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

    private static void PopulateFsmTables() {
        // See section A.1. of the manual for details of the finite state machine.
        fsm_handler_table = new ArrayList<Function<FsmContext, Boolean>>();//
        Function<FsmContext, Boolean> state1 = ctx -> State1(ctx);
        fsm_handler_table.add(state1);
        Function<FsmContext, Boolean> state2 = ctx -> State2(ctx);
        fsm_handler_table.add(state2);
        Function<FsmContext, Boolean> state3 = ctx -> State3(ctx);
        fsm_handler_table.add(state3);
        Function<FsmContext, Boolean> state4 = ctx -> State4(ctx);
        fsm_handler_table.add(state4);
        Function<FsmContext, Boolean> state5 = ctx -> State5(ctx);
        fsm_handler_table.add(state5);
        Function<FsmContext, Boolean> state6 = ctx -> State6(ctx);
        fsm_handler_table.add(state6);
        Function<FsmContext, Boolean> state7 = ctx -> State7(ctx);
        fsm_handler_table.add(state7);
        Function<FsmContext, Boolean> state8 = ctx -> State8(ctx);
        fsm_handler_table.add(state8);
        Function<FsmContext, Boolean> state9 = ctx -> State9(ctx);
        fsm_handler_table.add(state9);
        Function<FsmContext, Boolean> state10 = ctx -> State10(ctx);
        fsm_handler_table.add(state10);
        Function<FsmContext, Boolean> state11 = ctx -> State11(ctx);
        fsm_handler_table.add(state11);
        Function<FsmContext, Boolean> state12 = ctx -> State12(ctx);
        fsm_handler_table.add(state12);
        Function<FsmContext, Boolean> state13 = ctx -> State13(ctx);
        fsm_handler_table.add(state13);
        Function<FsmContext, Boolean> state14 = ctx -> State14(ctx);
        fsm_handler_table.add(state14);
        Function<FsmContext, Boolean> state15 = ctx -> State15(ctx);
        fsm_handler_table.add(state15);
        Function<FsmContext, Boolean> state16 = ctx -> State16(ctx);
        fsm_handler_table.add(state16);
        Function<FsmContext, Boolean> state17 = ctx -> State17(ctx);
        fsm_handler_table.add(state17);
        Function<FsmContext, Boolean> state18 = ctx -> State18(ctx);
        fsm_handler_table.add(state18);
        Function<FsmContext, Boolean> state19 = ctx -> State19(ctx);
        fsm_handler_table.add(state19);
        Function<FsmContext, Boolean> state20 = ctx -> State20(ctx);
        fsm_handler_table.add(state20);
        Function<FsmContext, Boolean> state21 = ctx -> State21(ctx);
        fsm_handler_table.add(state21);
        Function<FsmContext, Boolean> state22 = ctx -> State22(ctx);
        fsm_handler_table.add(state22);
        Function<FsmContext, Boolean> state23 = ctx -> State23(ctx);
        fsm_handler_table.add(state23);
        Function<FsmContext, Boolean> state24 = ctx -> State24(ctx);
        fsm_handler_table.add(state24);
        Function<FsmContext, Boolean> state25 = ctx -> State25(ctx);
        fsm_handler_table.add(state25);
        Function<FsmContext, Boolean> state26 = ctx -> State26(ctx);
        fsm_handler_table.add(state26);
        Function<FsmContext, Boolean> state27 = ctx -> State27(ctx);
        fsm_handler_table.add(state27);
        Function<FsmContext, Boolean> state28 = ctx -> State28(ctx);
        fsm_handler_table.add(state28);

        fsm_return_table = new int[] { (int) ParserToken.Char.value, 0, (int) ParserToken.Number.value,
                (int) ParserToken.Number.value, 0, (int) ParserToken.Number.value, 0, (int) ParserToken.Number.value, 0,
                0, (int) ParserToken.True.value, 0, 0, 0, (int) ParserToken.False.value, 0, 0,
                (int) ParserToken.Null.value, (int) ParserToken.CharSeq.value, (int) ParserToken.Char.value, 0, 0,
                (int) ParserToken.CharSeq.value, (int) ParserToken.Char.value, 0, 0, 0, 0 };
    }

    private static char ProcessEscChar(int esc_char) {
        switch (esc_char) {
            case '"':
            case '\'':
            case '\\':
            case '/':
                return (char) esc_char;// Convert.ToChar(esc_char);
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
            if (ctx.L.input_char == ' ' || ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r')
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
                    if (!ctx.L.allow_single_quoted_strings)
                        return false;

                    ctx.L.input_char = '"';
                    ctx.NextState = 23;
                    ctx.Return = true;
                    return true;

                case '/':
                    if (!ctx.L.allow_comments)
                        return false;

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

            if (ctx.L.input_char == ' ' || ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
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

        if (ctx.L.input_char == ' ' || ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
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

            if (ctx.L.input_char == ' ' || ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
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

            if (ctx.L.input_char == ' ' || ctx.L.input_char >= '\t' && ctx.L.input_char <= '\r') {
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

            if (ctx.L.input_char >= '0' && ctx.L.input_char <= '9' || ctx.L.input_char >= 'A' && ctx.L.input_char <= 'F'
                    || ctx.L.input_char >= 'a' && ctx.L.input_char <= 'f') {

                ctx.L.unichar += HexValue(ctx.L.input_char) * mult;

                counter++;
                mult /= 16;

                if (counter == 4) {
                    ctx.L.string_buffer.append((char) (ctx.L.unichar));
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
            if (ctx.L.input_char == '*')
                continue;

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
        try {
            if ((input_char = NextChar()) != -1)
            return true;
        } catch (Exception e) {
        }
        end_of_input = true;
        return false;
    }

    private int NextChar() throws IOException {
        if (input_buffer != 0) {
            int tmp = input_buffer;
            input_buffer = 0;

            return tmp;
        }

        return reader.read();
    }

    public boolean NextToken() throws JsonException {
        Function<FsmContext,Boolean> handler;
        fsm_context.Return = false;

        while (true) {
            handler =(Function<FsmContext,Boolean>) fsm_handler_table.get(state - 1);// [state - 1];
            
            if (!handler.apply(fsm_context))
                throw new JsonException(input_char);

            if (end_of_input)
                return false;

            if (fsm_context.Return) {
                string_value = string_buffer.toString();
                string_buffer.delete(0, string_buffer.length());
                // string_buffer.Remove(0, string_buffer.length());
                token = fsm_return_table[state - 1];

                if (token == (int) ParserToken.Char.value)
                    token = input_char;

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