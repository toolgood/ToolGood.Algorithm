using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_SEARCH : Function_3
	{
		public Function_SEARCH(FunctionBase[] funcs) : base(funcs)
		{
			// 与 Java 版一致:要求 2~3 个参数(find_text, within_text, [start_num])
			if (funcs.Length < 2 || funcs.Length > 3) {
				throw new ArgumentException($"Function '{Name}' requires 2 to 3 parameters.");
			}
		}

		public override string Name => "Search";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetText_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			if(func3 == null) {
				var index = WildcardIndexOf(args2.TextValue, args1.TextValue, 0);
				if(index < 0) {
					// 未找到:Excel 模式(索引从1开始)返回错误,C# 模式(索引从0开始)返回 -1
					return engine.ExcelIndex == 1 ? FunctionError() : Operand.Create(-1);
				}
				return Operand.Create(index + engine.ExcelIndex);
			}
			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsErrorOrNone) { return args3; }
			var startIndex = args3.IntValue - engine.ExcelIndex;
			if(startIndex < 0 || startIndex >= args2.TextValue.Length) {
				return ParameterError(3);
			}
			var p2 = WildcardIndexOf(args2.TextValue, args1.TextValue, startIndex);
			if(p2 < 0) {
				return engine.ExcelIndex == 1 ? FunctionError() : Operand.Create(-1);
			}
			return Operand.Create(p2 + engine.ExcelIndex);
		}

		/// <summary>
		/// 在指定起始位置起做大小写不敏感的通配符查找,支持 Excel 的 ? 与 * 及 ~ 转义。
		/// 使用手写匹配器替代正则,避免每次调用都编译正则的开销。
		/// </summary>
		private static int WildcardIndexOf(string text, string pattern, int startIndex)
		{
			if (startIndex < 0) { startIndex = 0; }
			if (startIndex > text.Length) { return -1; }

			var tokens = ParseWildcardPattern(pattern);
			if (tokens.Length == 0) { return startIndex; }

			for (int start = startIndex; start <= text.Length; start++) {
				if (MatchWildcard(text, tokens, start)) {
					return start;
				}
			}
			return -1;
		}

		private enum WildcardTokenKind { Literal, AnyOne, AnySeq }

		private struct WildcardToken
		{
			public WildcardTokenKind Kind;
			public char Value;
		}

		/// <summary>
		/// 将通配符 pattern 解析为 token 序列,字面字符统一转为大写以忽略大小写。
		/// </summary>
		private static WildcardToken[] ParseWildcardPattern(string pattern)
		{
			var list = new List<WildcardToken>(pattern.Length);
			for (int i = 0; i < pattern.Length; i++) {
				var c = pattern[i];
				if (c == '~') {
					if (i + 1 < pattern.Length && (pattern[i + 1] == '?' || pattern[i + 1] == '*' || pattern[i + 1] == '~')) {
						list.Add(new WildcardToken { Kind = WildcardTokenKind.Literal, Value = char.ToUpperInvariant(pattern[i + 1]) });
						i++;
					} else {
						list.Add(new WildcardToken { Kind = WildcardTokenKind.Literal, Value = '~' });
					}
				} else if (c == '*') {
					list.Add(new WildcardToken { Kind = WildcardTokenKind.AnySeq });
				} else if (c == '?') {
					list.Add(new WildcardToken { Kind = WildcardTokenKind.AnyOne });
				} else {
					list.Add(new WildcardToken { Kind = WildcardTokenKind.Literal, Value = char.ToUpperInvariant(c) });
				}
			}
			return list.ToArray();
		}

		/// <summary>
		/// 判断 pattern 是否从 text 的 start 位置开始完整匹配(贪心 + 回溯)。
		/// </summary>
		private static bool MatchWildcard(string text, WildcardToken[] tokens, int start)
		{
			int ti = 0;
			int si = start;
			int starToken = -1;
			int starMatch = 0;

			while (ti < tokens.Length) {
				if (si < text.Length) {
					var tok = tokens[ti];
					if (tok.Kind == WildcardTokenKind.Literal) {
						if (char.ToUpperInvariant(text[si]) == tok.Value) {
							ti++;
							si++;
						} else if (starToken != -1) {
							ti = starToken + 1;
							si = ++starMatch;
						} else {
							return false;
						}
					} else if (tok.Kind == WildcardTokenKind.AnyOne) {
						ti++;
						si++;
					} else {
						starToken = ti;
						starMatch = si;
						ti++;
					}
				} else {
					break;
				}
			}

			while (ti < tokens.Length && tokens[ti].Kind == WildcardTokenKind.AnySeq) {
				ti++;
			}
			return ti == tokens.Length;
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			func2.GetParameterTypes(noneEngine, result, OperandType.TEXT);
			if(func3 != null) {
				func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			}
		}
	}

}
