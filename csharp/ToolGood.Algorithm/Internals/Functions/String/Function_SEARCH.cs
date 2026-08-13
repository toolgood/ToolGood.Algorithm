using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
		/// </summary>
		private static int WildcardIndexOf(string text, string pattern, int startIndex)
		{
			var match = new Regex(WildcardToRegex(pattern), RegexOptions.IgnoreCase).Match(text, startIndex);
			if (!match.Success) { return -1; }
			return match.Index;
		}

		private static string WildcardToRegex(string pattern)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < pattern.Length; i++) {
				var c = pattern[i];
				if (c == '~') {
					if (i + 1 < pattern.Length && (pattern[i + 1] == '?' || pattern[i + 1] == '*' || pattern[i + 1] == '~')) {
						sb.Append(Regex.Escape(pattern[i + 1].ToString()));
						i++;
					} else {
						sb.Append(Regex.Escape("~"));
					}
				} else if (c == '*') {
					sb.Append("[\\s\\S]*");
				} else if (c == '?') {
					sb.Append("[\\s\\S]");
				} else {
					sb.Append(Regex.Escape(c.ToString()));
				}
			}
			return sb.ToString();
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
