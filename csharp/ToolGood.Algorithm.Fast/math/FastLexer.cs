using System;
using System.Runtime.CompilerServices;
using ToolGood.Algorithm.Internals.Visitors;

namespace ToolGood.Algorithm.math
{
	/// <summary>
	/// 词法单元。Start/End 均为原始字符串下标（End 为闭区间）。
	/// </summary>
	internal struct FastLexeme
	{
		public int Type;
		public int Start;
		public int End;
	}

	/// <summary>
	/// 手写词法分析器，等价替代 Antlr4 生成的 <c>mathLexer</c>。
	/// <para>
	/// 直接扫描 string，不做字符流包装、不产生 IToken 对象；大小写/全角归一化通过
	/// <see cref="CharUtil.StandardChar"/> 的查表完成，行为与原 Antlr 词法一致。
	/// 值类型实现，解析一条公式不产生额外对象分配。
	/// </para>
	/// </summary>
	internal struct FastLexer
	{
		private readonly string _text;
		private int _pos;
		private string _error;

		internal FastLexer(string text)
		{
			_text = text ?? string.Empty;
			_pos = 0;
			_error = null;
		}

		/// <summary>词法错误信息，null 表示无错误</summary>
		internal string ErrorMessage => _error;

		/// <summary>当前扫描位置</summary>
		internal int Position => _pos;

		/// <summary>
		/// 读取下一个词法单元。返回 false 表示词法错误（此时 <see cref="ErrorMessage"/> 非 null）。
		/// </summary>
		internal bool Next(ref FastLexeme token)
		{
			while(true) {
				if(_pos >= _text.Length) {
					token.Type = FastToken.EOF;
					token.Start = _pos;
					token.End = _pos - 1;
					return true;
				}
				int c = Ch(_pos);
				if(c >= '0' && c <= '9') {
					token.Start = _pos;
					ReadNumber(ref token);
					return true;
				}
				if(c == '\'' || c == '"' || c == '`') {
					token.Start = _pos;
					return ReadString(c, ref token);
				}
				if(IsIdentifierStart(c)) {
					token.Start = _pos;
					ReadIdentifier(ref token);
					return true;
				}
				if(c == ' ' || c == '\n' || c == '\t' || c == '\f') {
					_pos++;
					continue;
				}
				if(c == '/') {
					int c2 = Ch(_pos + 1);
					if(c2 == '*') {
						ConsumeBlockComment();
						continue;
					}
					if(c2 == '/') {
						ConsumeLineComment();
						continue;
					}
				}
				token.Start = _pos;
				return ReadOperator(c, ref token);
			}
		}

		/// <summary>返回指定下标的归一化字符，越界返回 -1</summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int Ch(int index)
		{
			if((uint)index >= (uint)_text.Length) { return -1; }
			return CharUtil.StandardChar(_text[index]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsDigit(int c)
		{
			return c >= '0' && c <= '9';
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsIdentifierStart(int c)
		{
			return (c >= 'A' && c <= 'Z') || c == '_';
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool IsIdentifierPart(int c)
		{
			return (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9');
		}

		/// <summary>从 from 开始连续吃掉的标识符字符，返回结束下标（不含）</summary>
		private int ScanIdentifierPart(int from)
		{
			int p = from;
			while(IsIdentifierPart(Ch(p))) { p++; }
			return p;
		}

		private void ConsumeBlockComment()
		{
			_pos += 2;
			while(true) {
				int c = Ch(_pos);
				if(c < 0) { return; }
				_pos++;
				if(c == '*' && Ch(_pos) == '/') {
					_pos++;
					return;
				}
			}
		}

		private void ConsumeLineComment()
		{
			_pos += 2;
			while(true) {
				int c = Ch(_pos);
				if(c < 0 || c == '\n') { return; }
				_pos++;
			}
		}

		private bool ReadString(int quote, ref FastLexeme token)
		{
			_pos++;  // 开引号
			while(true) {
				int c = Ch(_pos);
				if(c < 0) {
					_error = $"Unterminated string literal at position {token.Start}.";
					token.Type = FastToken.Invalid;
					return false;
				}
				_pos++;
				if(c == quote) { break; }
				if(c == '\\') {
					if(Ch(_pos) < 0) {
						_error = $"Unterminated string literal at position {token.Start}.";
						token.Type = FastToken.Invalid;
						return false;
					}
					_pos++;
				}
			}
			token.Type = FastToken.STRING;
			token.End = _pos - 1;
			return true;
		}

		private void ReadNumber(ref FastLexeme token)
		{
			_pos++;
			while(IsDigit(Ch(_pos))) { _pos++; }

			int c = Ch(_pos);
			if(c == '.' && IsDigit(Ch(_pos + 1))) {
				_pos += 2;
				while(IsDigit(Ch(_pos))) { _pos++; }
				c = Ch(_pos);
			}

			if(c == 'E') {
				int c2 = Ch(_pos + 1);
				if(IsDigit(c2) || c2 == '+' || c2 == '-') {
					_pos += 2;
					while(IsDigit(Ch(_pos))) { _pos++; }
				}
				token.Type = FastToken.NUM;
				token.End = _pos - 1;
				return;
			}

			// 单位后缀： [KDCM]? 'M' [23]? | 'M'? 'L' | 'K'? 'G' | 'T'
			if(c == 'C' || c == 'D') {
				if(Ch(_pos + 1) == 'M') {
					_pos += 2;
					int c3 = Ch(_pos);
					if(c3 == '2' || c3 == '3') { _pos++; }
				}
			} else if(c == 'M') {
				_pos++;
				int c2 = Ch(_pos);
				if(c2 == 'M') {
					_pos++;
					int c3 = Ch(_pos);
					if(c3 == '2' || c3 == '3') { _pos++; }
				} else if(c2 == '2' || c2 == '3' || c2 == 'L') {
					_pos++;
				}
			} else if(c == 'K') {
				int c2 = Ch(_pos + 1);
				if(c2 == 'M') {
					_pos += 2;
					int c3 = Ch(_pos);
					if(c3 == '2' || c3 == '3') { _pos++; }
				} else if(c2 == 'G') {
					_pos += 2;
				}
			} else if(c == 'G' || c == 'T' || c == 'L') {
				_pos++;
			}

			token.Type = FastToken.NUM;
			token.End = _pos - 1;
		}

		private void ReadIdentifier(ref FastLexeme token)
		{
			int start = _pos;
			int e0 = ScanIdentifierPart(start + 1);

			// 关键字允许带点，如 BETA.DIST / NORM.S.DIST：优先按最长形式匹配
			int e1 = -1, e2 = -1;
			if(Ch(e0) == '.' && IsIdentifierStart(Ch(e0 + 1))) {
				e1 = ScanIdentifierPart(e0 + 2);
				if(Ch(e1) == '.' && IsIdentifierStart(Ch(e1 + 1))) {
					e2 = ScanIdentifierPart(e1 + 2);
				}
			}
			if(e2 > 0) {
				int type2 = FindKeyword(start, e2);
				if(type2 != 0) {
					_pos = e2;
					token.Type = type2;
					token.End = e2 - 1;
					return;
				}
			}
			if(e1 > 0) {
				int type1 = FindKeyword(start, e1);
				if(type1 != 0) {
					_pos = e1;
					token.Type = type1;
					token.End = e1 - 1;
					return;
				}
			}
			_pos = e0;
			token.End = e0 - 1;
			int type0 = FindKeyword(start, e0);
			token.Type = type0 != 0 ? type0 : FastToken.PARAMETER;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int FindKeyword(int start, int end)
		{
			if(end <= start) { return 0; }
			var span = _text.AsSpan(start, end - start);
			return FastKeywordTable.Find(FastKeywordTable.Hash(span), span);
		}

		private bool ReadOperator(int c, ref FastLexeme token)
		{
			_pos++;
			switch(c) {
				case '(': token.Type = FastToken.LPAREN; break;
				case ')': token.Type = FastToken.RPAREN; break;
				case '.': token.Type = FastToken.DOT; break;
				case ',': token.Type = FastToken.COMMA; break;
				case '[': token.Type = FastToken.LBRACKET; break;
				case ']': token.Type = FastToken.RBRACKET; break;
				case '!':
					if(Ch(_pos) == '=') {
						_pos++;
						if(Ch(_pos) == '=') { _pos++; }
						token.Type = FastToken.OPNE;
					} else {
						token.Type = FastToken.BANG;
					}
					break;
				case '?': token.Type = FastToken.QUESTION; break;
				case ':': token.Type = FastToken.COLON; break;
				case '{': token.Type = FastToken.LBRACE; break;
				case '}': token.Type = FastToken.RBRACE; break;
				case '&':
					if(Ch(_pos) == '&') {
						_pos++;
						token.Type = FastToken.OPAND;
					} else {
						token.Type = FastToken.OPCAT;
					}
					break;
				case '|':
					if(Ch(_pos) == '|') {
						_pos++;
						token.Type = FastToken.OPOR;
						break;
					}
					_error = $"Unexpected character '|' at position {token.Start}.";
					token.Type = FastToken.Invalid;
					return false;
				case '+': token.Type = FastToken.OPADD; break;
				case '-': token.Type = FastToken.OPSUB; break;
				case '*': token.Type = FastToken.OPMUL; break;
				case '/': token.Type = FastToken.OPDIV; break;
				case '%': token.Type = FastToken.OPMOD; break;
				case '>':
					if(Ch(_pos) == '=') {
						_pos++;
						token.Type = FastToken.OPGE;
					} else {
						token.Type = FastToken.OPGT;
					}
					break;
				case '<':
					int c2 = Ch(_pos);
					if(c2 == '=') {
						_pos++;
						token.Type = FastToken.OPLE;
					} else if(c2 == '>') {
						_pos++;
						token.Type = FastToken.OPNE;
					} else {
						token.Type = FastToken.OPLT;
					}
					break;
				case '=':
					if(Ch(_pos) == '=') {
						_pos++;
						if(Ch(_pos) == '=') { _pos++; }
					}
					token.Type = FastToken.OPEQ;
					break;
				default:
					_error = $"Unexpected character '{_text[token.Start]}' at position {token.Start}.";
					token.Type = FastToken.Invalid;
					return false;
			}
			token.End = _pos - 1;
			return true;
		}
	}
}
