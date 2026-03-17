package toolgood.algorithm.litJson;

import java.io.StringReader;
import java.math.BigDecimal;
import java.util.HashMap;
import java.util.Map;
import java.util.Stack;

final class JsonReader {
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
        automaton_stack.push(ParserToken.End.value);
        automaton_stack.push(ParserToken.Text.value);

        lexer = new Lexer(reader);

        end_of_input = false;
        end_of_json = false;
    }

    static {
        PopulateParseTable();
    }

    private static void PopulateParseTable() {
        TableAddRow(parse_table, ParserToken.Array);
        TableAddCol(parse_table, ParserToken.Array, '[', '[', ParserToken.ArrayPrime.value);

        TableAddRow(parse_table, ParserToken.ArrayPrime);
        TableAddCol(parse_table, ParserToken.ArrayPrime, '"', ParserToken.Value.value, ParserToken.ValueRest.value, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, '[', ParserToken.Value.value, ParserToken.ValueRest.value, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, ']', ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, '{', ParserToken.Value.value, ParserToken.ValueRest.value, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, ParserToken.Number.value, ParserToken.Value.value, ParserToken.ValueRest.value, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, ParserToken.True.value, ParserToken.Value.value, ParserToken.ValueRest.value, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, ParserToken.False.value, ParserToken.Value.value, ParserToken.ValueRest.value, ']');
        TableAddCol(parse_table, ParserToken.ArrayPrime, ParserToken.Null.value, ParserToken.Value.value, ParserToken.ValueRest.value, ']');

        TableAddRow(parse_table, ParserToken.Object);
        TableAddCol(parse_table, ParserToken.Object, '{', '{', ParserToken.ObjectPrime.value);

        TableAddRow(parse_table, ParserToken.ObjectPrime);
        TableAddCol(parse_table, ParserToken.ObjectPrime, '"', ParserToken.Pair.value, ParserToken.PairRest.value, '}');
        TableAddCol(parse_table, ParserToken.ObjectPrime, '}', '}');

        TableAddRow(parse_table, ParserToken.Pair);
        TableAddCol(parse_table, ParserToken.Pair, '"', ParserToken.String.value, ':', ParserToken.Value.value);

        TableAddRow(parse_table, ParserToken.PairRest);
        TableAddCol(parse_table, ParserToken.PairRest, ',', ',', ParserToken.Pair.value, ParserToken.PairRest.value);
        TableAddCol(parse_table, ParserToken.PairRest, '}', ParserToken.Epsilon.value);

        TableAddRow(parse_table, ParserToken.String);
        TableAddCol(parse_table, ParserToken.String, '"', '"', ParserToken.CharSeq.value, '"');

        TableAddRow(parse_table, ParserToken.Text);
        TableAddCol(parse_table, ParserToken.Text, '[', ParserToken.Array.value);
        TableAddCol(parse_table, ParserToken.Text, '{', ParserToken.Object.value);

        TableAddRow(parse_table, ParserToken.Value);
        TableAddCol(parse_table, ParserToken.Value, '"', ParserToken.String.value);
        TableAddCol(parse_table, ParserToken.Value, '[', ParserToken.Array.value);
        TableAddCol(parse_table, ParserToken.Value, '{', ParserToken.Object.value);
        TableAddCol(parse_table, ParserToken.Value, ParserToken.Number.value, ParserToken.Number.value);
        TableAddCol(parse_table, ParserToken.Value, ParserToken.True.value, ParserToken.True.value);
        TableAddCol(parse_table, ParserToken.Value, ParserToken.False.value, ParserToken.False.value);
        TableAddCol(parse_table, ParserToken.Value, ParserToken.Null.value, ParserToken.Null.value);

        TableAddRow(parse_table, ParserToken.ValueRest);
        TableAddCol(parse_table, ParserToken.ValueRest, ',', ',', ParserToken.Value.value, ParserToken.ValueRest.value);
        TableAddCol(parse_table, ParserToken.ValueRest, ']', ParserToken.Epsilon.value);
    }

    private static void TableAddCol(Map<Integer, Map<Integer, int[]>> parse_table, ParserToken row, int col, int... symbols) {
        Map<Integer, int[]> rowMap = parse_table.get(row.value);
        if (rowMap == null) {
            rowMap = new HashMap<>();
            parse_table.put(row.value, rowMap);
        }
        rowMap.put(col, symbols);
    }

    private static void TableAddRow(Map<Integer, Map<Integer, int[]>> parse_table, ParserToken rule) {
        parse_table.put(rule.value, new HashMap<>());
    }

    private void ProcessNumber(String number) {
        String span = number;
        if (span.indexOf('.') >= 0 || span.indexOf('e') >= 0 || span.indexOf('E') >= 0) {
            try {
                token_value = new BigDecimal(span);
                return;
            } catch (NumberFormatException e) {
            }
        }
        try {
            token_value = new BigDecimal(Integer.parseInt(span));
            return;
        } catch (NumberFormatException e) {
        }
        try {
            token_value = new BigDecimal(Long.parseLong(span));
            return;
        } catch (NumberFormatException e) {
        }
        try {
            token_value = new BigDecimal(new java.math.BigInteger(span));
            return;
        } catch (NumberFormatException e) {
        }
        token_value = BigDecimal.ZERO;
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
        } else if (current_symbol == ParserToken.CharSeq.value) {
            token_value = lexer.StringValue();
        } else if (current_symbol == ParserToken.False.value) {
            token = JsonToken.Boolean;
            token_value = false;
            parser_return = true;
        } else if (current_symbol == ParserToken.Null.value) {
            token = JsonToken.Null;
            parser_return = true;
        } else if (current_symbol == ParserToken.Number.value) {
            ProcessNumber(lexer.StringValue());
            token = JsonToken.Double;
            parser_return = true;
        } else if (current_symbol == ParserToken.Pair.value) {
            token = JsonToken.PropertyName;
        } else if (current_symbol == ParserToken.True.value) {
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
            automaton_stack.push(ParserToken.End.value);
            automaton_stack.push(ParserToken.Text.value);
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
                if (automaton_stack.peek() == ParserToken.End.value) end_of_json = true;

                return true;
            }

            current_symbol = automaton_stack.pop();

            ProcessSymbol();

            if (current_symbol == current_input) {
                if (!ReadToken()) {
                    if (automaton_stack.peek() != ParserToken.End.value)
                        throw new JsonException("Input doesn't evaluate to proper JSON text");

                    if (parser_return) return true;

                    return false;
                }

                continue;
            }

            try {
                Map<Integer, int[]> row = parse_table.get(current_symbol);
                if (row == null) {
                    throw new JsonException("Parse error");
                }
                entry_symbols = row.get(current_input);
                if (entry_symbols == null) {
                    throw new JsonException("Parse error");
                }
            } catch (JsonException e) {
                throw e;
            } catch (Exception e) {
                throw new JsonException("Parse error", e);
            }

            if (entry_symbols[0] == ParserToken.Epsilon.value) continue;

            for (int i = entry_symbols.length - 1; i >= 0; i--)
                automaton_stack.push(entry_symbols[i]);
        }
    }
}
