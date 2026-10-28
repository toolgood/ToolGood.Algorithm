using System;
using System.Globalization;
using System.Text;

namespace ToolGood.Algorithm.LitJson
{
	/// <summary>
	/// 手写 JSON 解析器，等价替代原来的 <c>Lexer</c>(28 状态委托状态机) + <c>JsonReader</c>(LL 自动机) + <c>JsonMapper</c> 三层结构。
	/// <para>
	/// 原实现每读一个字符要走一次 <c>StateHandler</c> 虚委托、每个 token 经 <c>StringBuilder</c> 拼一次再 <c>ToString()</c>，
	/// 每个符号还要在 <c>Stack&lt;int&gt;</c> 自动机上压/弹若干次，1 KB 文本实测 46 µs / 7.8 KB 分配。
	/// 这里改为对 string 直接下标扫描的递归下降解析：无委托调用、无 token 中转、字符串只在确认无转义时做一次 Substring。
	/// </para>
	/// <para>
	/// 可接受输入与原 Lexer 保持完全一致：允许 <c>//</c> 与 <c>/* */</c> 注释、允许单引号字符串、
	/// 数字严格按 <c>[-] (0 | [1-9][0-9]*) [.(digits)] [([eE][+-]?digits)]</c> 校验（前导零非法），
	/// 未知转义字符报错。差异仅在错误信息文本上（原为无上下文的状态机错误，这里带位置）。
	/// </para>
	/// </summary>
	internal sealed class JsonFastParser
	{
		/// <summary>
		/// 嵌套深度上限。原实现用递归 ReadValue 且无上限，深层嵌套会直接 StackOverflow 杀进程（无法 catch）
		/// </summary>
		private const int MaxDepth = 512;

		private readonly string _text;
		private int _pos;

		private JsonFastParser(string text)
		{
			_text = text;
		}

		internal static JsonData Parse(string text)
		{
			if(text == null) { throw new JsonException("Json string is null."); }
			var parser = new JsonFastParser(text);

			parser.SkipTrivia();
			JsonData root;
			var c = parser.Peek();
			if(c == '{') {
				root = parser.ParseObject(0);
			} else if(c == '[') {
				root = parser.ParseArray(0);
			} else {
				throw parser.Unexpected("the root must be an object or an array");
			}

			parser.SkipTrivia();
			if(!parser.Eof) { throw parser.Unexpected("unexpected trailing content"); }
			return root;
		}

		private bool Eof => _pos >= _text.Length;

		private char Peek()
		{
			return _pos < _text.Length ? _text[_pos] : '\0';
		}

		private static bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}

		private JsonException Unexpected(string expectation)
		{
			var found = _pos < _text.Length ? $"'{_text[_pos]}'" : "end of input";
			return new JsonException($"Invalid JSON at position {_pos}: {expectation}, but found {found}.");
		}

		/// <summary>跳过空白与注释（// 行注释、/* */ 块注释），与原 Lexer 的白名单一致</summary>
		private void SkipTrivia()
		{
			while(_pos < _text.Length) {
				var c = _text[_pos];
				// 原 Lexer: input_char == ' ' || input_char >= '\t' && input_char <= '\r'
				if(c == ' ' || (c >= '\t' && c <= '\r')) { _pos++; continue; }

				if(c != '/') { return; }

				if(_pos + 1 >= _text.Length) {
					throw new JsonException($"Invalid JSON at position {_pos}: '/' must start a comment.");
				}
				var next = _text[_pos + 1];
				if(next == '/') {
					_pos += 2;
					while(_pos < _text.Length && _text[_pos] != '\n') { _pos++; }
					continue;
				}
				if(next == '*') {
					var commentStart = _pos;
					_pos += 2;
					while(_pos + 1 < _text.Length && !(_text[_pos] == '*' && _text[_pos + 1] == '/')) { _pos++; }
					if(_pos + 1 >= _text.Length) {
						throw new JsonException($"Invalid JSON at position {commentStart}: unterminated block comment.");
					}
					_pos += 2;
					continue;
				}
				throw new JsonException($"Invalid JSON at position {_pos}: '/' must start a comment.");
			}
		}

		#region 容器

		private JsonData ParseObject(int depth)
		{
			if(depth > MaxDepth) { throw new JsonException($"Invalid JSON at position {_pos}: nested deeper than {MaxDepth} levels."); }

			_pos++; // '{'
			var instance = new JsonData();
			instance.SetJsonType(JsonType.Object);

			SkipTrivia();
			if(Peek() == '}') { _pos++; return instance; }

			while(true) {
				SkipTrivia();
				var quote = Peek();
				if(quote != '"' && quote != '\'') { throw Unexpected("expected a string property name"); }
				_pos++;
				var key = ParseString(quote);

				SkipTrivia();
				if(Peek() != ':') { throw Unexpected("expected ':'"); }
				_pos++;

				SkipTrivia();
				instance.Set(key, ParseValue(depth + 1));

				SkipTrivia();
				var c = Peek();
				if(c == ',') { _pos++; continue; }
				if(c == '}') { _pos++; return instance; }
				throw Unexpected("expected ',' or '}'");
			}
		}

		private JsonData ParseArray(int depth)
		{
			if(depth > MaxDepth) { throw new JsonException($"Invalid JSON at position {_pos}: nested deeper than {MaxDepth} levels."); }

			_pos++; // '['
			var instance = new JsonData();
			instance.SetJsonType(JsonType.Array);

			SkipTrivia();
			if(Peek() == ']') { _pos++; return instance; }

			while(true) {
				SkipTrivia();
				instance.Add(ParseValue(depth + 1));

				SkipTrivia();
				var c = Peek();
				if(c == ',') { _pos++; continue; }
				if(c == ']') { _pos++; return instance; }
				throw Unexpected("expected ',' or ']'");
			}
		}

		private JsonData ParseValue(int depth)
		{
			var c = Peek();
			if(c == '{') { return ParseObject(depth); }
			if(c == '[') { return ParseArray(depth); }
			if(c == '"' || c == '\'') {
				_pos++;
				var text = ParseString(c);
				var textData = new JsonData();
				textData.SetString(text);
				return textData;
			}
			if(c == 't') {
				Expect("true");
				var trueData = new JsonData();
				trueData.SetBoolean(true);
				return trueData;
			}
			if(c == 'f') {
				Expect("false");
				var falseData = new JsonData();
				falseData.SetBoolean(false);
				return falseData;
			}
			if(c == 'n') {
				Expect("null");
				var nullData = new JsonData();
				nullData.SetNull();
				return nullData;
			}
			if(c == '-' || IsDigit(c)) { return ParseNumber(); }
			throw Unexpected("expected a value");
		}

		private void Expect(string literal)
		{
			// 字面量只有 true/false/null（4~5 字符），逐字符比较比 string.CompareOrdinal 省掉一次跨方法调用
			if(_pos + literal.Length > _text.Length) { throw Unexpected($"expected '{literal}'"); }
			for(var i = 0; i < literal.Length; i++) {
				if(_text[_pos + i] != literal[i]) { throw Unexpected($"expected '{literal}'"); }
			}
			_pos += literal.Length;
		}

		#endregion 容器

		#region 标量

		private JsonData ParseNumber()
		{
			var start = _pos;

			if(Peek() == '-') { _pos++; }

			var c = Peek();
			if(c == '0') {
				_pos++;
				// 与原 FSM 一致：'0' 之后不允许再跟数字（前导零非法）
				if(IsDigit(Peek())) { throw Unexpected("leading zeros are not allowed"); }
			} else if(c >= '1' && c <= '9') {
				while(IsDigit(Peek())) { _pos++; }
			} else {
				throw Unexpected("expected a digit");
			}

			if(Peek() == '.') {
				_pos++;
				if(!IsDigit(Peek())) { throw Unexpected("expected a digit after '.'"); }
				while(IsDigit(Peek())) { _pos++; }
			}

			c = Peek();
			if(c == 'e' || c == 'E') {
				_pos++;
				c = Peek();
				if(c == '+' || c == '-') { _pos++; }
				if(!IsDigit(Peek())) { throw Unexpected("expected a digit in the exponent"); }
				while(IsDigit(Peek())) { _pos++; }
			}

			var span = _text.AsSpan(start, _pos - start);
			decimal number;
			// 与原 JsonReader.ProcessNumber 的分派顺序一致：带小数点/指数的先按 decimal 解析。
			// 这里用 Float 而非 Any：span 已由上面的扫描保证不含千分位、前导空白与 '+'，
			// Any 需要额外处理 AllowThousands，解析更慢，结果对合法 JSON 数字完全相同
			if(span.IndexOfAny('.', 'e', 'E') >= 0) {
				if(decimal.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out number)) {
					return CreateNumber(number);
				}
			} else {
				if(int.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i)) { return CreateNumber(i); }
				if(long.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var l)) { return CreateNumber(l); }
				if(ulong.TryParse(span, NumberStyles.Integer, CultureInfo.InvariantCulture, out var ul)) { return CreateNumber(ul); }
			}
			throw new JsonException("Number is too large or invalid: " + span.ToString());
		}

		private static JsonData CreateNumber(decimal value)
		{
			var data = new JsonData();
			data.SetDouble(value);
			return data;
		}

		/// <summary>
		/// 解析字符串（<paramref name="quote"/> 为开引号，调用时已越过开引号）。
		/// 无转义时直接 Substring 一次；有转义才走 StringBuilder 慢路径。
		/// </summary>
		private string ParseString(char quote)
		{
			var start = _pos;
			while(true) {
				if(_pos >= _text.Length) { throw new JsonException($"Invalid JSON at position {start}: unterminated string."); }
				var c = _text[_pos];
				if(c == quote) {
					var text = _text.Substring(start, _pos - start);
					_pos++;
					return text;
				}
				if(c == '\\') { return ParseStringSlow(start, quote); }
				_pos++;
			}
		}

		private string ParseStringSlow(int start, char quote)
		{
			var sb = new StringBuilder(_pos - start + 16);
			sb.Append(_text, start, _pos - start);

			while(true) {
				// 此处 _text[_pos] 恒为 '\\'
				_pos++;
				if(_pos >= _text.Length) { throw new JsonException($"Invalid JSON at position {start}: unterminated string."); }

				var esc = _text[_pos++];
				switch(esc) {
					case '"': sb.Append('"'); break;
					case '\'': sb.Append('\''); break;
					case '\\': sb.Append('\\'); break;
					case '/': sb.Append('/'); break;
					case 'b': sb.Append('\b'); break;
					case 'f': sb.Append('\f'); break;
					case 'n': sb.Append('\n'); break;
					case 'r': sb.Append('\r'); break;
					case 't': sb.Append('\t'); break;
					case 'u': {
							if(_pos + 4 > _text.Length) { throw new JsonException($"Invalid JSON at position {_pos}: incomplete \\u escape."); }
							var code = 0;
							for(var i = 0; i < 4; i++) {
								var hex = _text[_pos + i];
								int digit;
								if(hex >= '0' && hex <= '9') { digit = hex - '0'; }
								else if(hex >= 'a' && hex <= 'f') { digit = hex - 'a' + 10; }
								else if(hex >= 'A' && hex <= 'F') { digit = hex - 'A' + 10; }
								else { throw new JsonException($"Invalid JSON at position {_pos + i}: invalid hex digit '{hex}' in \\u escape."); }
								code = code * 16 + digit;
							}
							_pos += 4;
							sb.Append((char)code);
							break;
						}
					default:
						throw new JsonException($"Invalid JSON at position {_pos - 1}: invalid escape sequence '\\{esc}'.");
				}

				while(true) {
					if(_pos >= _text.Length) { throw new JsonException($"Invalid JSON at position {start}: unterminated string."); }
					var c = _text[_pos];
					if(c == quote) {
						_pos++;
						return sb.ToString();
					}
					if(c == '\\') { break; }
					sb.Append(c);
					_pos++;
				}
			}
		}

		#endregion 标量
	}
}
