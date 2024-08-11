﻿using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ToolGood.Algorithm.LitJson
{
    internal enum JsonToken
    {
        None,

        ObjectStart,
        PropertyName,
        ObjectEnd,

        ArrayStart,
        ArrayEnd,

        Double,

        String,

        Boolean,
        Null
    }

    internal sealed class JsonReader
    {
        #region Fields

        private static readonly IDictionary<int, IDictionary<int, int[]>> parse_table;

        private Stack<int> automaton_stack;
        private int current_input;
        private int current_symbol;
        private bool end_of_json;
        private bool end_of_input;
        private readonly Lexer lexer;
        private bool parser_in_string;
        private bool parser_return;
        private bool read_started;
        private object token_value;
        private JsonToken token;

        #endregion Fields

        #region Public Properties

        public JsonToken Token { get { return token; } }

        public object Value { get { return token_value; } }

        #endregion Public Properties

        #region Constructors

        static JsonReader()
        {
            parse_table = PopulateParseTable();
        }

        public JsonReader(string json_text)
        {
            var reader = new StringReader(json_text);

            parser_in_string = false;
            parser_return = false;

            read_started = false;
            automaton_stack = new Stack<int>();
            automaton_stack.Push((int)ParserToken.End);
            automaton_stack.Push((int)ParserToken.Text);

            lexer = new Lexer(reader);

            end_of_input = false;
            end_of_json = false;
        }

        #endregion Constructors

        #region Static Methods

        private static IDictionary<int, IDictionary<int, int[]>> PopulateParseTable()
        {
            // See section A.2. of the manual for details
            IDictionary<int, IDictionary<int, int[]>> parse_table = new Dictionary<int, IDictionary<int, int[]>>();

            TableAddRow(parse_table, ParserToken.Array);
            TableAddCol(parse_table, ParserToken.Array, '[', '[', (int)ParserToken.ArrayPrime);

            TableAddRow(parse_table, ParserToken.ArrayPrime);
            TableAddCol(parse_table, ParserToken.ArrayPrime, '"', (int)ParserToken.Value, (int)ParserToken.ValueRest, ']');
            TableAddCol(parse_table, ParserToken.ArrayPrime, '[', (int)ParserToken.Value, (int)ParserToken.ValueRest, ']');
            TableAddCol(parse_table, ParserToken.ArrayPrime, ']', ']');
            TableAddCol(parse_table, ParserToken.ArrayPrime, '{', (int)ParserToken.Value, (int)ParserToken.ValueRest, ']');
            TableAddCol(parse_table, ParserToken.ArrayPrime, (int)ParserToken.Number, (int)ParserToken.Value, (int)ParserToken.ValueRest, ']');
            TableAddCol(parse_table, ParserToken.ArrayPrime, (int)ParserToken.True, (int)ParserToken.Value, (int)ParserToken.ValueRest, ']');
            TableAddCol(parse_table, ParserToken.ArrayPrime, (int)ParserToken.False, (int)ParserToken.Value, (int)ParserToken.ValueRest, ']');
            TableAddCol(parse_table, ParserToken.ArrayPrime, (int)ParserToken.Null, (int)ParserToken.Value, (int)ParserToken.ValueRest, ']');

            TableAddRow(parse_table, ParserToken.Object);
            TableAddCol(parse_table, ParserToken.Object, '{', '{', (int)ParserToken.ObjectPrime);

            TableAddRow(parse_table, ParserToken.ObjectPrime);
            TableAddCol(parse_table, ParserToken.ObjectPrime, '"', (int)ParserToken.Pair, (int)ParserToken.PairRest, '}');
            TableAddCol(parse_table, ParserToken.ObjectPrime, '}', '}');

            TableAddRow(parse_table, ParserToken.Pair);
            TableAddCol(parse_table, ParserToken.Pair, '"', (int)ParserToken.String, ':', (int)ParserToken.Value);

            TableAddRow(parse_table, ParserToken.PairRest);
            TableAddCol(parse_table, ParserToken.PairRest, ',', ',', (int)ParserToken.Pair, (int)ParserToken.PairRest);
            TableAddCol(parse_table, ParserToken.PairRest, '}', (int)ParserToken.Epsilon);

            TableAddRow(parse_table, ParserToken.String);
            TableAddCol(parse_table, ParserToken.String, '"', '"', (int)ParserToken.CharSeq, '"');

            TableAddRow(parse_table, ParserToken.Text);
            TableAddCol(parse_table, ParserToken.Text, '[', (int)ParserToken.Array);
            TableAddCol(parse_table, ParserToken.Text, '{', (int)ParserToken.Object);

            TableAddRow(parse_table, ParserToken.Value);
            TableAddCol(parse_table, ParserToken.Value, '"', (int)ParserToken.String);
            TableAddCol(parse_table, ParserToken.Value, '[', (int)ParserToken.Array);
            TableAddCol(parse_table, ParserToken.Value, '{', (int)ParserToken.Object);
            TableAddCol(parse_table, ParserToken.Value, (int)ParserToken.Number, (int)ParserToken.Number);
            TableAddCol(parse_table, ParserToken.Value, (int)ParserToken.True, (int)ParserToken.True);
            TableAddCol(parse_table, ParserToken.Value, (int)ParserToken.False, (int)ParserToken.False);
            TableAddCol(parse_table, ParserToken.Value, (int)ParserToken.Null, (int)ParserToken.Null);

            TableAddRow(parse_table, ParserToken.ValueRest);
            TableAddCol(parse_table, ParserToken.ValueRest, ',', ',', (int)ParserToken.Value, (int)ParserToken.ValueRest);
            TableAddCol(parse_table, ParserToken.ValueRest, ']', (int)ParserToken.Epsilon);

            return parse_table;
        }

        private static void TableAddCol(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken row, int col, params int[] symbols)
        {
            parse_table[(int)row].Add(col, symbols);
        }

        private static void TableAddRow(IDictionary<int, IDictionary<int, int[]>> parse_table, ParserToken rule)
        {
            parse_table.Add((int)rule, new Dictionary<int, int[]>());
        }

        #endregion Static Methods

        #region Private Methods

        private void ProcessNumber(string number)
        {
            if (number.IndexOf('.') != -1 || number.IndexOf('e') != -1 || number.IndexOf('E') != -1) {
                if (decimal.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal n_double)) {
                    token_value = n_double;
                    return;
                }
            }
            if (int.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out int n_int32)) {
                token_value = (decimal)n_int32;
                return;
            }
            if (long.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out long n_int64)) {
                token_value = (decimal)n_int64;
                return;
            }
            if (ulong.TryParse(number, NumberStyles.Integer, CultureInfo.InvariantCulture, out ulong n_uint64)) {
                token_value = (decimal)n_uint64;
                return;
            }
            // Shouldn't happen, but just in case, return something
            token_value = 0;
        }

        private void ProcessSymbol()
        {
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
            } else if (current_symbol == (int)ParserToken.CharSeq) {
                token_value = lexer.StringValue;
            } else if (current_symbol == (int)ParserToken.False) {
                token = JsonToken.Boolean;
                token_value = false;
                parser_return = true;
            } else if (current_symbol == (int)ParserToken.Null) {
                token = JsonToken.Null;
                parser_return = true;
            } else if (current_symbol == (int)ParserToken.Number) {
                ProcessNumber(lexer.StringValue);
                token = JsonToken.Double;
                //if (double.TryParse(lexer.StringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double n_double))
                //{
                //    token_value = n_double;
                //}
                //else
                //{
                //    token_value = 0;
                //}
                parser_return = true;
            } else if (current_symbol == (int)ParserToken.Pair) {
                token = JsonToken.PropertyName;
            } else if (current_symbol == (int)ParserToken.True) {
                token = JsonToken.Boolean;
                token_value = true;
                parser_return = true;
            }
        }

        private bool ReadToken()
        {
            if (end_of_input) return false;

            lexer.NextToken();

            if (lexer.EndOfInput) {
                Close();

                return false;
            }

            current_input = lexer.Token;

            return true;
        }

        #endregion Private Methods

        public void Close()
        {
            if (end_of_input)
                return;

            end_of_input = true;
            end_of_json = true;
        }

        public bool Read()
        {
            if (end_of_input)
                return false;

            if (end_of_json) {
                end_of_json = false;
                automaton_stack.Clear();
                automaton_stack.Push((int)ParserToken.End);
                automaton_stack.Push((int)ParserToken.Text);
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
                    if (automaton_stack.Peek() == (int)ParserToken.End) end_of_json = true;

                    return true;
                }

                current_symbol = automaton_stack.Pop();

                ProcessSymbol();

                if (current_symbol == current_input) {
                    if (!ReadToken()) {
                        if (automaton_stack.Peek() != (int)ParserToken.End) throw new JsonException("Input doesn't evaluate to proper JSON text");

                        if (parser_return) return true;

                        return false;
                    }

                    continue;
                }

                try {
                    entry_symbols = parse_table[current_symbol][current_input];
                } catch (KeyNotFoundException e) {
                    throw new JsonException((ParserToken)current_input, e);
                }

                if (entry_symbols[0] == (int)ParserToken.Epsilon) continue;

                for (int i = entry_symbols.Length - 1; i >= 0; i--)
                    automaton_stack.Push(entry_symbols[i]);
            }
        }
    }
}