using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ToolGood.Algorithm.Internals.Functions;
using ToolGood.Algorithm.Internals.Functions.Compare;
using ToolGood.Algorithm.Internals.Functions.Flow;
using ToolGood.Algorithm.Internals.Functions.MathBase;
using ToolGood.Algorithm.Internals.Functions.Operator;
using ToolGood.Algorithm.Internals.Functions.Value;
using ToolGood.Algorithm.Operands;

namespace ToolGood.Algorithm.math
{
	/// <summary>
	/// 把手写语法树转换为可执行的 <see cref="FunctionBase"/> 树。
	/// 等价于原来的 <c>MathFunctionVisitor</c>，但省去了 Antlr 访问者分发。
	/// </summary>
	internal static class FastAstBuilder
	{
		internal static FunctionBase Build(string text, FastNode node)
		{
			switch(node.Kind) {
				case FastNodeKind.Number: return BuildNumber(text, node);
				case FastNodeKind.Text: return new Function_ValueText(Operand.Create(UnescapeString(Slice(text, node))));
				case FastNodeKind.Parameter: return new Function_Parameter(node.Text);
				case FastNodeKind.Bracket: return Build(text, node.Children[0]);
				case FastNodeKind.Not: return new Function_NOT(Build(text, node.Children[0]));
				case FastNodeKind.Percentage: return new Function_Percentage(Build(text, node.Children[0]));

				case FastNodeKind.Mul: return new Function_Mul(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.Div: return new Function_Div(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.Mod: return new Function_Mod(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.Add: return new Function_Add(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.Sub: return new Function_Sub(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.Concat: return new Function_Connect(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.GreaterThan: return new Function_GT(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.GreaterOrEqual: return new Function_GE(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.LessThan: return new Function_LT(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.LessOrEqual: return new Function_LE(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.Equal: return new Function_EQ(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.NotEqual: return new Function_NE(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.And: return new Function_AND(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.Or: return new Function_OR(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.If: return new Function_IF(BuildArguments(text, node));

				case FastNodeKind.Call: return FastFunctionFactory.Create(node.Type, BuildArguments(text, node), node.Text);
				case FastNodeKind.Array: return new Function_ARRAY(BuildArguments(text, node));
				case FastNodeKind.ArrayJson: return new Function_ArrayJson(BuildArguments(text, node));
				case FastNodeKind.ArrayJsonItem: return new Function_ArrayJsonItem(UnescapeKey(node.Text), Build(text, node.Children[0]));
				case FastNodeKind.GetJson: return new Function_GetJsonValue(Build(text, node.Children[0]), Build(text, node.Children[1]));
				case FastNodeKind.GetJsonProperty: return new Function_GetJsonValue(Build(text, node.Children[0]), new Function_ValueText(Operand.Create(node.Text)));
				default: throw new ArgumentException($"Node '{node.Kind}' is not implemented.");
			}
		}

		private static FunctionBase[] BuildArguments(string text, FastNode node)
		{
			var children = node.Children;
			if(children.Length == 0) { return FunctionBase_Empty; }
			var list = new FunctionBase[children.Length];
			for(int i = 0; i < children.Length; i++) {
				list[i] = Build(text, children[i]);
			}
			return list;
		}

		private static readonly FunctionBase[] FunctionBase_Empty = new FunctionBase[0];

		private static string Slice(string text, FastNode node)
		{
			return text.Substring(node.Start, node.End - node.Start + 1);
		}

		private static FunctionBase BuildNumber(string text, FastNode node)
		{
			var span = text.AsSpan(node.Start, node.End - node.Start + 1);
			if(decimal.TryParse(span, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) {
				return new Function_Number(Operand.Create(d));
			}
			// 解析失败只可能是: 数值超出 decimal 范围, 或数值后带单位。
			// 仅当后缀确实命中单位字典时才按单位切分, 否则一律视为非法数值,
			// 避免把指数记法的 'E' 或超长数字的末位误当作单位。
			if(TrySplitUnit(span, out var txt, out var unit)) {
				var d2 = decimal.TryParse(txt, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d3)
					? d3
					: throw new ArgumentException($"Number '{span.ToString()}' is invalid or out of range.");
				return new Function_Number2(d2, unit);
			}
			throw new ArgumentException($"Number '{span.ToString()}' is invalid or out of range.");
		}

		/// <summary>
		/// 按 Function_Number2 实际支持的单位字典切分数值后缀。
		/// 必须校验完整后缀(而非仅首字母): 否则 "1E29" 这类超出 decimal 范围的指数记法
		/// 会被切成数值 1 + 单位 "E29", 报出"单位非法"这种与 "1E+30" 不一致的误导性错误。
		/// </summary>
		private static bool TrySplitUnit(ReadOnlySpan<char> span, out ReadOnlySpan<char> txt, out string unit)
		{
			var dict = Function_Number2.GetUnitTypedict();
			for(int n = 3; n >= 1; n--) {
				if(span.Length <= n) continue;
				var candidate = span.Slice(span.Length - n).ToString();
				if(dict.ContainsKey(candidate)) {
					txt = span.Slice(0, span.Length - n);
					unit = candidate;
					return true;
				}
			}
			txt = span;
			unit = null;
			return false;
		}

		/// <summary>json 的 key：字符串字面量需去掉首尾引号并反转义，其余形式直接用原文</summary>
		private static string UnescapeKey(string keyText)
		{
			if(keyText != null && keyText.Length > 1 && (keyText[0] == '"' || keyText[0] == '\'' || keyText[0] == '`')) {
				return UnescapeString(keyText);
			}
			return keyText;
		}

		/// <summary>
		/// 将字符串字面量(含首尾引号)转为实际文本值。
		/// 必须与 OperandString.ToString / Function_ValueText.ToString 的转义输出保持对称,
		/// 否则字面量往返会静默失真(例如 "\u000b" 曾被解析成 "u000b")。
		/// </summary>
		internal static string UnescapeString(string text)
		{
			var sb = new StringBuilder(text.Length);
			int index = 1;
			var end = text.Length - 1;
			while(index < end) {
				var c = text[index++];
				if(c != '\\') {
					sb.Append(c);
					continue;
				}
				var c2 = text[index++];
				switch(c2) {
					case 'n': sb.Append('\n'); break;
					case 'r': sb.Append('\r'); break;
					case 't': sb.Append('\t'); break;
					case '0': sb.Append('\0'); break;
					case 'v': sb.Append('\v'); break;
					case 'a': sb.Append('\a'); break;
					case 'b': sb.Append('\b'); break;
					case 'f': sb.Append('\f'); break;
					case 'u':
						// \uXXXX 十六进制转义
						if(index + 4 <= end
							&& int.TryParse(text.AsSpan(index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var code)) {
							sb.Append((char)code);
							index += 4;
						} else {
							sb.Append(c2);
						}
						break;
					default: sb.Append(c2); break;
				}
			}
			return sb.ToString();
		}
	}
}
