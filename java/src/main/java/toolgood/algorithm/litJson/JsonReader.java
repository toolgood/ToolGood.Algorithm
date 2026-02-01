package toolgood.algorithm.litJson;

import java.io.StringReader;
import java.math.BigDecimal;
import java.util.HashMap;
import java.util.Map;
import java.util.Stack;

public class JsonReader {
    private static Map<Integer, Map<Integer, int[]>> parse_table;

    private Stack<Integer> automaton_stack;
    private int current_input;
    private int current_symbol;
    private boolean end_of_json;
    private boolean end_of_input;
    private Lexer lexer;
    private boolean parser_in_string;
    private boolean parser_return;
    private boolean read_started;
    private Object token_value;
    private JsonToken token;

    public JsonToken Token() {
        return token;
    }

    public Object Value() {
        return token_value;
    }

    static {
        parse_table = PopulateParseTable();
    }

    public JsonReader(String json_text) {
        StringReader reader = new StringReader(json_text);

        parser_in_string = false;
        parser_return = false;

        read_started = false;
        automaton_stack = new Stack<Integer>();
        automaton_stack.push((int) ParserToken.End.value);
        automaton_stack.push((int) ParserToken.Text.value);

        lexer = new Lexer(reader);

        end_of_input = false;
        end_of_json = false;

    }

    private static Map<Integer, Map<Integer, int[]>> PopulateParseTable() {
        // See section A.2. of the manual for details
        Map<Integer, Map<Integer, int[]>> parse_table = new HashMap<Integer, Map<Integer, int[]>>();

        TableAddRow(parse_table, ParserToken.Array);
        TableAddCol(parse_table, ParserToken.Array, '[', new int[] { '[', (int) ParserToken.ArrayPrime.value });

        TableAddRow(parse_table, ParserToken.ArrayPrime);
        TableAddCol(parse_table, ParserToken.ArrayPrime, '"',
                new int[] { (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value, ']' });
        TableAddCol(parse_table, ParserToken.ArrayPrime, '[',
                new int[] { (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value, ']' });
        TableAddCol(parse_table, ParserToken.ArrayPrime, ']', new int[] { ']' });
        TableAddCol(parse_table, ParserToken.ArrayPrime, '{',
                new int[] { (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value, ']' });
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.Number.value,
                new int[] { (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value, ']' });
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.True.value,
                new int[] { (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value, ']' });
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.False.value,
                new int[] { (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value, ']' });
        TableAddCol(parse_table, ParserToken.ArrayPrime, (int) ParserToken.Null.value,
                new int[] { (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value, ']' });

        TableAddRow(parse_table, ParserToken.Object);
        TableAddCol(parse_table, ParserToken.Object, '{', new int[] { '{', (int) ParserToken.ObjectPrime.value });

        TableAddRow(parse_table, ParserToken.ObjectPrime);
        TableAddCol(parse_table, ParserToken.ObjectPrime, '"',
                new int[] { (int) ParserToken.Pair.value, (int) ParserToken.PairRest.value, '}' });
        TableAddCol(parse_table, ParserToken.ObjectPrime, '}', new int[] { '}' });

        TableAddRow(parse_table, ParserToken.Pair);
        TableAddCol(parse_table, ParserToken.Pair, '"',
                new int[] { (int) ParserToken.String.value, ':', (int) ParserToken.Value.value });

        TableAddRow(parse_table, ParserToken.PairRest);
        TableAddCol(parse_table, ParserToken.PairRest, ',',
                new int[] { ',', (int) ParserToken.Pair.value, (int) ParserToken.PairRest.value });
        TableAddCol(parse_table, ParserToken.PairRest, '}', new int[] { (int) ParserToken.Epsilon.value });

        TableAddRow(parse_table, ParserToken.String);
        TableAddCol(parse_table, ParserToken.String, '"', new int[] { '"', (int) ParserToken.CharSeq.value, '"' });

        TableAddRow(parse_table, ParserToken.Text);
        TableAddCol(parse_table, ParserToken.Text, '[', new int[] { (int) ParserToken.Array.value });
        TableAddCol(parse_table, ParserToken.Text, '{', new int[] { (int) ParserToken.Object.value });

        TableAddRow(parse_table, ParserToken.Value);
        TableAddCol(parse_table, ParserToken.Value, '"', new int[] { (int) ParserToken.String.value });
        TableAddCol(parse_table, ParserToken.Value, '[', new int[] { (int) ParserToken.Array.value });
        TableAddCol(parse_table, ParserToken.Value, '{', new int[] { (int) ParserToken.Object.value });
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.Number.value,
                new int[] { (int) ParserToken.Number.value });
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.True.value,
                new int[] { (int) ParserToken.True.value });
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.False.value,
                new int[] { (int) ParserToken.False.value });
        TableAddCol(parse_table, ParserToken.Value, (int) ParserToken.Null.value,
                new int[] { (int) ParserToken.Null.value });

        TableAddRow(parse_table, ParserToken.ValueRest);
        TableAddCol(parse_table, ParserToken.ValueRest, ',',
                new int[] { ',', (int) ParserToken.Value.value, (int) ParserToken.ValueRest.value });
        TableAddCol(parse_table, ParserToken.ValueRest, ']', new int[] { (int) ParserToken.Epsilon.value });

        return parse_table;
    }

    private static void TableAddCol(Map<Integer, Map<Integer, int[]>> parse_table, ParserToken row, int col,
            int[] symbols) {
        parse_table.get((int) row.value).put(col, symbols);
    }

    private static void TableAddRow(Map<Integer, Map<Integer, int[]>> parse_table, ParserToken rule) {
        parse_table.put((int) rule.value, new HashMap<Integer, int[]>());
    }

    private void ProcessNumber(String number)
    {
        try {
            BigDecimal n_double=new BigDecimal(number);
            token_value = n_double;
            return;
        }catch (Exception e){

        }
//        Double n_double=Double.valueOf(number);
//        if (n_double.isNaN()==false) {
//            token_value = n_double.doubleValue();
//            return;
//        }
        // if (number.indexOf('.') != -1 || number.indexOf('e') != -1 || number.indexOf('E') != -1) {
        //     Double n_double=Double.valueOf(number);
        //     if (n_double.isNaN()==false) {
        //         token_value = n_double.doubleValue();
        //         return;
        //     }
        // }
        // if (int.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out int n_int32)) {
        //     token_value = (double)n_int32;
        //     return;
        // }
        // if (long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out long n_int64)) {
        //     token_value = (double)n_int64;
        //     return;
        // }

        // Shouldn't happen, but just in case, return something
        token_value =new BigDecimal(0);
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
                if (token == JsonToken.None)
                    token = JsonToken.String;

                parser_in_string = true;
            }

        } else if (current_symbol == (int) ParserToken.CharSeq.value) {
            token_value = lexer.StringValue();

        } else if (current_symbol == (int) ParserToken.False.value) {
            token = JsonToken.Boolean;
            token_value = false;
            parser_return = true;

        } else if (current_symbol == (int) ParserToken.Null.value) {
            token = JsonToken.Null;
            parser_return = true;

        } else if (current_symbol == (int) ParserToken.Number.value) {
            ProcessNumber(lexer.StringValue());
            token = JsonToken.Double;
            // if (double.TryParse(lexer.StringValue, NumberStyles.Any,
            // CultureInfo.InvariantCulture, out double n_double))
            // {
            // token_value = n_double;
            // }
            // else
            // {
            // token_value = 0;
            // }
            parser_return = true;

        } else if (current_symbol == (int) ParserToken.Pair.value) {
            token = JsonToken.PropertyName;

        } else if (current_symbol == (int) ParserToken.True.value) {
            token = JsonToken.Boolean;
            token_value = true;
            parser_return = true;

        }
    }

    private boolean ReadToken() throws JsonException {
        if (end_of_input)
            return false;

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

    public boolean Read() throws JsonException {
        if (end_of_input)
            return false;

        if (end_of_json) {
            end_of_json = false;
            automaton_stack.clear();
            automaton_stack.push((int) ParserToken.End.value);
            automaton_stack.push((int) ParserToken.Text.value);
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
                if (automaton_stack.peek() == (int) ParserToken.End.value)
                    end_of_json = true;

                return true;
            }

            current_symbol = automaton_stack.pop();

            ProcessSymbol();

            if (current_symbol == current_input) {
                if (!ReadToken()) {
                    if (automaton_stack.peek() != (int) ParserToken.End.value) {
                        throw new JsonException("Input doesn't evaluate to proper JSON text");

                    }

                    if (parser_return)
                        return true;

                    return false;
                }

                continue;
            }

            try {
                entry_symbols =  parse_table.get(current_symbol).get(current_input);

            } catch (Exception e) {
                throw new JsonException(ParserToken.values()[current_input], e);
            }

            if (entry_symbols[0] == (int) ParserToken.Epsilon.value)
                continue;

            for (int i = entry_symbols.length - 1; i >= 0; i--)
                automaton_stack.push(entry_symbols[i]);
        }
    }

}
