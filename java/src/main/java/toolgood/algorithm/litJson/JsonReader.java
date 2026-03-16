package toolgood.algorithm.litJson;

import java.io.StringReader;
import java.util.HashMap;
import java.util.Map;
import java.util.Stack;

public class JsonReader {
    private static final Map<Integer, Map<Integer, int[]>> parse_table = new HashMap<>();

    private final Stack<Integer> automaton_stack = new Stack<>();
    private int current_input;
    private int current_symbol;
    private boolean end_of_json = false;
    private boolean end_of_input = false;
    private final Lexer lexer;
    private boolean parser_in_string = false;
    private boolean parser_return = false;
    private boolean read_started = false;
    private Object token_value = null;
    private JsonToken token = JsonToken.None;

    public JsonToken Token() {
        return token;
    }

    public Object Value() {
        return token_value;
    }

    public JsonReader(String json_text) {
        StringReader reader = new StringReader(json_text);

        parser_in_string = false;
        parser_return = false;

        read_started = false;
        automaton_stack.clear();
        automaton_stack.push((int) ParserToken.End);
        automaton_stack.push((int) ParserToken.Text);

        lexer = new Lexer(reader);

        end_of_input = false;
        end_of_json = false;
    }

    static {
        PopulateParseTable();
    }

    private static void PopulateParseTable() {
        TableAddRow(parse_table, ParserToken.Array);
        TableAddCol(parse_table, ParserToken.Array, '[', '[', (int) ParserToken.ArrayPrime);

        TableAddRow(parse_table, ParserToken.ArrayPrime);
        TableAddCol(parse_table, ParserToken.ArrayPrime, '"', (int) ParserToken.Value, (int) ParserToken.ValueRest, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, '[', (int) ParserToken.Value, (int) ParserToken.ValueRest, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, ']', ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, '{', (int) ParserToken.Value, (int) ParserToken.ValueRest, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.Number, (int) ParserToken.Value, (int) ParserToken.ValueRest, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.True, (int) ParserToken.Value, (int) ParserToken.ValueRest, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.False, (int) ParserToken.Value, (int) ParserToken.ValueRest, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.Null, (int) ParserToken.Value, (int) ParserToken.ValueRest, ']');

        TableAddRow(parse_table, ParserToken.Object);
        TableAddCol(parse_table, ParserToken.Object, '{', '{', (int) ParserToken.ObjectPrime);

        TableAddRow(parse_table, ParserToken.ObjectPrime);
        TableAddCol(parse_table, ParserToken.ObjectPrime, '"', (int) ParserToken.Pair, (int) ParserToken.PairRest, '}');
        TableAddCol(parse_table, ParserToken.ObjectPrime, '}', '}');

        TableAddRow(parse_table, ParserToken.Pair);
        TableAddCol(parse_table, ParserToken.Pair, '"', (int) ParserToken.String, ':', (int) ParserToken.Value);

        TableAddRow(parse_table, ParserToken.PairRest);
        TableAddCol(parse_table, ParserToken.PairRest, ',', ',', (int) ParserToken.Pair, (int) ParserToken.PairRest);
        TableAddCol(parse_table, ParserToken.PairRest, '}', (int) ParserToken.Epsilon);

        TableAddRow(parse_table, ParserToken.String);
        TableAddCol(parse_table, ParserToken.String, '"', '"', (int) ParserToken.CharSeq, '"');

        TableAddRow(parse_table, ParserToken.Text);
        TableAddCol(parse_table, ParserToken.Text, '[', (int) ParserToken.Array);
        TableAddCol(parse_table, ParserToken.Text, '{', (int) ParserToken.Object);

        TableAddRow(parse_table, ParserToken.Value);
        TableAddCol(parse_table, ParserToken.Value, '"', (int) ParserToken.String);
        TableAddCol(parse_table, ParserToken.Value, '[', (int) ParserToken.Array);
        TableAddCol(parse_table, ParserToken.Value, '{', (int) ParserToken.Object);
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.Number, (int) ParserToken.Number);
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.True, (int) ParserToken.True);
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.False, (int) ParserToken.False);
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.Null, (int) ParserToken.Null);

        TableAddRow(parse_table, ParserToken.ValueRest);
        TableAddCol(parse_table, ParserToken.ValueRest, ',', ',', (int) ParserToken.Value, (int) ParserToken.ValueRest);
        TableAddCol(parse_table, ParserToken.ValueRest, ']', (int) ParserToken.Epsilon);
    }

    private static void TableAddCol(Map<Integer, Map<Integer, int[]>> parse_table, ParserToken row, int col, int... symbols) {
        Map<Integer, int[]> rowMap = parse_table.get((int) row);
        if (rowMap == null) {
            rowMap = new HashMap<>();
            parse_table.put((int) row, rowMap);
        }
        rowMap.put(col, symbols);
    }

    private static void TableAddRow(Map<Integer, Map<Integer, int[]>> parse_table, ParserToken rule) {
        parse_table.put((int) rule, new HashMap<>());
    }

    private void ProcessNumber(String number) {
        if (number.indexOf('.') >= 0 || number.indexOf('e') >= 0 || number.indexOf('E') >= 0) {
            try {
                token_value = Double.parseDouble(number);
                return;
            } catch (NumberFormatException e) {
            }
        }
        try {
            token_value = Long.parseLong(number);
            return;
        } catch (NumberFormatException e) {
        }
        try {
            token_value = new java.math.BigDecimal(number);
            return;
        } catch (NumberFormatException e) {
        }
        token_value = 0;
    }

    private void ProcessSymbol() {
        if (current_symbol == '[') {
            token = JsonToken.ArrayStart;
            parser_return = true;
        } else if (current_symbol == ']') {
            token = JsonToken.ArrayEnd;
            parser_return = true;
        } else if (current_symbol == '{') {
            token = JsonToken.ObjectStart;
            parser_return = true;
        } else if (current_symbol == '}') {
            token = JsonToken.ObjectEnd;
            parser_return = true;
        } else if (current_symbol == '"') {
            if (parser_in_string) {
                parser_in_string = false;
                parser_return = true;
            } else {
                if (token == JsonToken.None) token = JsonToken.String;
                parser_in_string = true;
            }
        } else if (current_symbol == (int) ParserToken.CharSeq) {
            token_value = lexer.StringValue();
        } else if (current_symbol == (int) ParserToken.False) {
            token = JsonToken.Boolean;
            token_value = false;
            parser_return = true;
        } else if (current_symbol == (int) ParserToken.Null) {
            token = JsonToken.Null;
            parser_return = true;
        } else if (current_symbol == (int) ParserToken.Number) {
            ProcessNumber(lexer.StringValue());
            token = JsonToken.Double;
            parser_return = true;
        } else if (current_symbol == (int) ParserToken.Pair) {
            token = JsonToken.PropertyName;
        } else if (current_symbol == (int) ParserToken.True) {
            token = JsonToken.Boolean;
            token_value = true;
            parser_return = true;
        }
    }

    private boolean ReadToken() {
        if (end_of_input) return false;

        lexer.NextToken();

        if (lexer.EndOfInput()) {
            Close();
            return false;
        }

        current_input = lexer.Token();

        return true;
    }

    public void Close() {
        if (end_of_input)
            return;

        end_of_input = true;
        end_of_json = true;
    }

    public boolean Read() {
        if (end_of_input)
            return false;

        if (end_of_json) {
            end_of_json = false;
            automaton_stack.clear();
            automaton_stack.push((int) ParserToken.End);
            automaton_stack.push((int) ParserToken.Text);
        }

        parser_in_string = false;
        parser_return = false;

        token = JsonToken.None;
        token_value = null;

        if (!read_started) {
            read_started = true;

            if (!ReadToken())
                return false;
        }

        int[] entry_symbols;

        while (true) {
            if (parser_return) {
                if (automaton_stack.peek() == (int) ParserToken.End) end_of_json = true;

                return true;
            }

            current_symbol = automaton_stack.pop();

            ProcessSymbol();

            if (current_symbol == current_input) {
                if (!ReadToken()) {
                    if (automaton_stack.peek() != (int) ParserToken.End)
                        throw new JsonException("Input doesn't evaluate to proper JSON text");

                    if (parser_return) return true;

                    return false;
                }

                continue;
            }

            try {
                Map<Integer, int[]> row = parse_table.get(current_symbol);
                if (row == null) {
                    throw new JsonException((ParserToken) current_input, new Exception("Parse error"));
                }
                entry_symbols = row.get(current_input);
                if (entry_symbols == null) {
                    throw new JsonException((ParserToken) current_input, new Exception("Parse error"));
                }
            } catch (Exception e) {
                throw new JsonException((ParserToken) current_input, e);
            }

            if (entry_symbols[0] == (int) ParserToken.Epsilon) continue;

            for (int i = entry_symbols.length - 1; i >= 0; i--)
                automaton_stack.push(entry_symbols[i]);
        }
    }
}
