package toolgood.algorithm.litJson;

import java.math.BigDecimal;

/**
 * 手写 JSON 解析器，等价替代原来的 Lexer(FSM 委托状态机) + JsonReader(LL 自动机) + JsonMapper 三层结构。
 * <p>
 * 原实现每读一个字符要走一次状态机委托、每个 token 经 StringBuilder 拼一次再 toString()，
 * 每个符号还要在自动机上压/弹若干次。这里改为对 string 直接下标扫描的递归下降解析：
 * 无委托调用、无 token 中转，字符串只在确认无转义时做一次 substring。
 * </p>
 * <p>
 * 可接受输入与原 Lexer 保持完全一致：允许 // 与 /* *&#47; 注释、允许单引号字符串、
 * 数字严格按 [-] (0 | [1-9][0-9]*) [.(digits)] [([eE][+-]?digits)] 校验（前导零非法），
 * 未知转义字符报错。差异仅在错误信息文本上（原为无上下文的状态机错误，这里带位置）。
 * </p>
 */
final class JsonFastParser {
    /** 嵌套深度上限。原实现递归无上限，深层嵌套会直接 StackOverflowError 杀进程（无法 catch） */
    private static final int MaxDepth = 512;

    private final String _text;
    private int _pos;

    private JsonFastParser(String text) {
        _text = text;
    }

    static JsonData Parse(String text) throws JsonException {
        if (text == null) {
            throw new JsonException("Json string is null.");
        }
        JsonFastParser parser = new JsonFastParser(text);

        parser.SkipTrivia();
        JsonData root;
        char c = parser.Peek();
        if (c == '{') {
            root = parser.ParseObject(0);
        } else if (c == '[') {
            root = parser.ParseArray(0);
        } else {
            throw parser.Unexpected("the root must be an object or an array");
        }

        parser.SkipTrivia();
        if (!parser.Eof()) {
            throw parser.Unexpected("unexpected trailing content");
        }
        return root;
    }

    private boolean Eof() {
        return _pos >= _text.length();
    }

    private char Peek() {
        return _pos < _text.length() ? _text.charAt(_pos) : '\0';
    }

    private static boolean IsDigit(char c) {
        return c >= '0' && c <= '9';
    }

    private JsonException Unexpected(String expectation) {
        String found = _pos < _text.length() ? "'" + _text.charAt(_pos) + "'" : "end of input";
        return new JsonException("Invalid JSON at position " + _pos + ": " + expectation + ", but found " + found + ".");
    }

    /** 跳过空白与注释（// 行注释、块注释），与原 Lexer 的白名单一致 */
    private void SkipTrivia() throws JsonException {
        while (_pos < _text.length()) {
            char c = _text.charAt(_pos);
            // 原 Lexer: input_char == ' ' || input_char >= '\t' && input_char <= '\r'
            if (c == ' ' || (c >= '\t' && c <= '\r')) {
                _pos++;
                continue;
            }

            if (c != '/') {
                return;
            }

            if (_pos + 1 >= _text.length()) {
                throw new JsonException("Invalid JSON at position " + _pos + ": '/' must start a comment.");
            }
            char next = _text.charAt(_pos + 1);
            if (next == '/') {
                _pos += 2;
                while (_pos < _text.length() && _text.charAt(_pos) != '\n') {
                    _pos++;
                }
                continue;
            }
            if (next == '*') {
                int commentStart = _pos;
                _pos += 2;
                while (_pos + 1 < _text.length() && !(_text.charAt(_pos) == '*' && _text.charAt(_pos + 1) == '/')) {
                    _pos++;
                }
                if (_pos + 1 >= _text.length()) {
                    throw new JsonException("Invalid JSON at position " + commentStart + ": unterminated block comment.");
                }
                _pos += 2;
                continue;
            }
            throw new JsonException("Invalid JSON at position " + _pos + ": '/' must start a comment.");
        }
    }

    // region 容器

    private JsonData ParseObject(int depth) throws JsonException {
        if (depth > MaxDepth) {
            throw new JsonException("Invalid JSON at position " + _pos + ": nested deeper than " + MaxDepth + " levels.");
        }

        _pos++; // '{'
        JsonData instance = new JsonData();
        instance.SetJsonType(JsonType.Object);

        SkipTrivia();
        if (Peek() == '}') {
            _pos++;
            return instance;
        }

        while (true) {
            SkipTrivia();
            char quote = Peek();
            if (quote != '"' && quote != '\'') {
                throw Unexpected("expected a string property name");
            }
            _pos++;
            String key = ParseString(quote);

            SkipTrivia();
            if (Peek() != ':') {
                throw Unexpected("expected ':'");
            }
            _pos++;

            SkipTrivia();
            instance.Set(key, ParseValue(depth + 1));

            SkipTrivia();
            char c = Peek();
            if (c == ',') {
                _pos++;
                continue;
            }
            if (c == '}') {
                _pos++;
                return instance;
            }
            throw Unexpected("expected ',' or '}'");
        }
    }

    private JsonData ParseArray(int depth) throws JsonException {
        if (depth > MaxDepth) {
            throw new JsonException("Invalid JSON at position " + _pos + ": nested deeper than " + MaxDepth + " levels.");
        }

        _pos++; // '['
        JsonData instance = new JsonData();
        instance.SetJsonType(JsonType.Array);

        SkipTrivia();
        if (Peek() == ']') {
            _pos++;
            return instance;
        }

        while (true) {
            SkipTrivia();
            instance.Add(ParseValue(depth + 1));

            SkipTrivia();
            char c = Peek();
            if (c == ',') {
                _pos++;
                continue;
            }
            if (c == ']') {
                _pos++;
                return instance;
            }
            throw Unexpected("expected ',' or ']'");
        }
    }

    private JsonData ParseValue(int depth) throws JsonException {
        char c = Peek();
        if (c == '{') {
            return ParseObject(depth);
        }
        if (c == '[') {
            return ParseArray(depth);
        }
        if (c == '"' || c == '\'') {
            _pos++;
            String text = ParseString(c);
            JsonData textData = new JsonData();
            textData.SetString(text);
            return textData;
        }
        if (c == 't') {
            Expect("true");
            JsonData trueData = new JsonData();
            trueData.SetBoolean(true);
            return trueData;
        }
        if (c == 'f') {
            Expect("false");
            JsonData falseData = new JsonData();
            falseData.SetBoolean(false);
            return falseData;
        }
        if (c == 'n') {
            Expect("null");
            JsonData nullData = new JsonData();
            nullData.SetNull();
            return nullData;
        }
        if (c == '-' || IsDigit(c)) {
            return ParseNumber();
        }
        throw Unexpected("expected a value");
    }

    private void Expect(String literal) throws JsonException {
        // 字面量只有 true/false/null（4~5 字符），逐字符比较省掉一次跨方法调用
        if (_pos + literal.length() > _text.length()) {
            throw Unexpected("expected '" + literal + "'");
        }
        for (int i = 0; i < literal.length(); i++) {
            if (_text.charAt(_pos + i) != literal.charAt(i)) {
                throw Unexpected("expected '" + literal + "'");
            }
        }
        _pos += literal.length();
    }

    // endregion 容器

    // region 标量

    private JsonData ParseNumber() throws JsonException {
        int start = _pos;

        if (Peek() == '-') {
            _pos++;
        }

        char c = Peek();
        if (c == '0') {
            _pos++;
            // 与原 FSM 一致：'0' 之后不允许再跟数字（前导零非法）
            if (IsDigit(Peek())) {
                throw Unexpected("leading zeros are not allowed");
            }
        } else if (c >= '1' && c <= '9') {
            while (IsDigit(Peek())) {
                _pos++;
            }
        } else {
            throw Unexpected("expected a digit");
        }

        if (Peek() == '.') {
            _pos++;
            if (!IsDigit(Peek())) {
                throw Unexpected("expected a digit after '.'");
            }
            while (IsDigit(Peek())) {
                _pos++;
            }
        }

        c = Peek();
        if (c == 'e' || c == 'E') {
            _pos++;
            c = Peek();
            if (c == '+' || c == '-') {
                _pos++;
            }
            if (!IsDigit(Peek())) {
                throw Unexpected("expected a digit in the exponent");
            }
            while (IsDigit(Peek())) {
                _pos++;
            }
        }

        String number = _text.substring(start, _pos);
        BigDecimal value;
        try {
            // 与 JsonReader.ProcessNumber 一致：数字统一按 BigDecimal 承载，保留精度
            value = new BigDecimal(number);
        } catch (NumberFormatException e) {
            throw new JsonException("Number is too large or invalid: " + number);
        }
        JsonData data = new JsonData();
        data.SetDouble(value);
        return data;
    }

    /**
     * 解析字符串（quote 为开引号，调用时已越过开引号）。
     * 无转义时直接 substring 一次；有转义才走 StringBuilder 慢路径。
     */
    private String ParseString(char quote) throws JsonException {
        int start = _pos;
        while (true) {
            if (_pos >= _text.length()) {
                throw new JsonException("Invalid JSON at position " + start + ": unterminated string.");
            }
            char c = _text.charAt(_pos);
            if (c == quote) {
                String text = _text.substring(start, _pos);
                _pos++;
                return text;
            }
            if (c == '\\') {
                return ParseStringSlow(start, quote);
            }
            _pos++;
        }
    }

    private String ParseStringSlow(int start, char quote) throws JsonException {
        StringBuilder sb = new StringBuilder(_pos - start + 16);
        sb.append(_text, start, _pos);

        while (true) {
            // 此处 _text[_pos] 恒为 '\\'
            _pos++;
            if (_pos >= _text.length()) {
                throw new JsonException("Invalid JSON at position " + start + ": unterminated string.");
            }

            char esc = _text.charAt(_pos++);
            switch (esc) {
                case '"': sb.append('"'); break;
                case '\'': sb.append('\''); break;
                case '\\': sb.append('\\'); break;
                case '/': sb.append('/'); break;
                case 'b': sb.append('\b'); break;
                case 'f': sb.append('\f'); break;
                case 'n': sb.append('\n'); break;
                case 'r': sb.append('\r'); break;
                case 't': sb.append('\t'); break;
                case 'u': {
                    if (_pos + 4 > _text.length()) {
                        throw new JsonException("Invalid JSON at position " + _pos + ": incomplete \\u escape.");
                    }
                    int code = 0;
                    for (int i = 0; i < 4; i++) {
                        char hex = _text.charAt(_pos + i);
                        int digit;
                        if (hex >= '0' && hex <= '9') {
                            digit = hex - '0';
                        } else if (hex >= 'a' && hex <= 'f') {
                            digit = hex - 'a' + 10;
                        } else if (hex >= 'A' && hex <= 'F') {
                            digit = hex - 'A' + 10;
                        } else {
                            throw new JsonException("Invalid JSON at position " + (_pos + i) + ": invalid hex digit '" + hex + "' in \\u escape.");
                        }
                        code = code * 16 + digit;
                    }
                    _pos += 4;
                    sb.append((char) code);
                    break;
                }
                default:
                    throw new JsonException("Invalid JSON at position " + (_pos - 1) + ": invalid escape sequence '\\" + esc + "'.");
            }

            while (true) {
                if (_pos >= _text.length()) {
                    throw new JsonException("Invalid JSON at position " + start + ": unterminated string.");
                }
                char c = _text.charAt(_pos);
                if (c == quote) {
                    _pos++;
                    return sb.toString();
                }
                if (c == '\\') {
                    break;
                }
                sb.append(c);
                _pos++;
            }
        }
    }

    // endregion 标量
}
