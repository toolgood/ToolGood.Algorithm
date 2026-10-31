using System;
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
	/// 解析途中把语法结构就地转换为可执行的 <see cref="FunctionBase"/> 节点。
	/// <para>
	/// 原实现先把语法树写成 <see cref="FastNode"/> 数组，再顺序扫一遍数组自底向上构建函数树
	/// （两次遍历、两套表示）。现在递归下降在返回时就地构造节点，构造阶段与解析阶段合并，
	/// 节点数组只在条件树 / 计算树 / DIY 名称收集时才需要，主路径完全不产生。
	/// </para>
	/// <para>
	/// 常量折叠也随构造就地完成：子节点返回时已带上「是否常量」标记，父节点据此决定是否折叠，
	/// 不再依赖节点数组的常量数组。
	/// </para>
	/// </summary>
	internal static class FastAstBuilder
	{
		/// <summary>无参调用的共享空参数表，避免每次为无参函数分配数组</summary>
		private static readonly FunctionBase[] EmptyArgs = new FunctionBase[0];

		/// <summary>
		/// 常量折叠探测用的共享引擎。
		/// 只用来求值「全部子节点均为字面量常量」的纯子树，这类子树不读参数、不改引擎状态
		/// （已核实：运算符与白名单函数的求值路径不写 engine.LastError、不调 GetParameter），
		/// 因此多线程并发解析共用一个实例是安全的。
		/// </summary>
		internal static readonly AlgorithmEngine FoldEngine = new AlgorithmEngine();

		#region 常量折叠

		/// <summary>
		/// 判断刚构建的节点是否本身就是常量（无需折叠）。
		/// 带单位数字（Function_Number2）的换算依赖引擎单位设置，不算常量。
		/// </summary>
		internal static bool IsConstant(FastNodeKind kind, int type, FunctionBase fn, FastHandle[] children, int offset)
		{
			switch(kind) {
				case FastNodeKind.Number: return fn is not Function_Number2;
				case FastNodeKind.Text: return true;
				case FastNodeKind.Bracket: return children[offset].Const;
				case FastNodeKind.Call:
					switch(type) {
						case FastToken.TRUE:
						case FastToken.FALSE:
						case FastToken.NULL:
						case FastToken.E:
						case FastToken.PI:
						case FastToken.ALGORITHMVERSION: return true;
						default: return false;
					}
				default: return false;
			}
		}

		/// <summary>
		/// 节点是否可以折叠：属于纯运算/白名单函数，且全部子节点都是常量。
		/// 白名单只收「求值确定、无副作用、不依赖引擎状态」的函数；
		/// 参数求值异常（Div 0 等）返回错误 Operand 而不是抛异常，由调用方据结果决定是否折叠。
		/// </summary>
		internal static bool CanFold(FastNodeKind kind, int type, FastHandle[] children, int offset, int count)
		{
			switch(kind) {
				case FastNodeKind.Mul:
				case FastNodeKind.Div:
				case FastNodeKind.Mod:
				case FastNodeKind.Add:
				case FastNodeKind.Sub:
				case FastNodeKind.Concat:
				case FastNodeKind.GreaterThan:
				case FastNodeKind.GreaterOrEqual:
				case FastNodeKind.LessThan:
				case FastNodeKind.LessOrEqual:
				case FastNodeKind.Equal:
				case FastNodeKind.NotEqual:
				case FastNodeKind.And:
				case FastNodeKind.Or:
				case FastNodeKind.Not:
				case FastNodeKind.Percentage:
				case FastNodeKind.If:
					break;
				case FastNodeKind.Call:
					// 注意：SUM/MAX/MIN 这类在求值时循环调用 funcs[i].Evaluate 的聚合函数
					// 不能进白名单——编译期探测会让 JIT 的分层 PGO 按「子节点全是常量节点」
					// 去虚化该调用点，生产路径传参数节点时守卫全部失配，实测求值慢 2 倍。
					switch(type) {
						case FastToken.CONCATENATE:
						case FastToken.ABS:
						case FastToken.ROUND:
						case FastToken.INT:
						case FastToken.IF:
						case FastToken.DATE:
							break;
						default: return false;
					}
					break;
				default: return false;
			}
			for(int i = 0; i < count; i++) {
				if(!children[offset + i].Const) { return false; }
			}
			return true;
		}

		/// <summary>把求值结果包装回常量函数节点；数组/json 等复杂结果不折叠</summary>
		internal static FunctionBase WrapConstant(Operand result)
		{
			if(result.IsNumber) { return new Function_Number(result); }
			if(result.IsText) { return new Function_ValueText(result); }
			if(result.IsBoolean) { return new Function_ValueBoolean(result.BooleanValue); }
			if(result.IsDate) { return new Function_Constant(result); }
			if(result.IsNull) { return new Function_NULL(); }
			return null;
		}

		#endregion

		#region 叶子

		internal static FunctionBase CreateNumber(string text, int start, int end)
		{
			var span = text.AsSpan(start, end - start + 1);
			// 快路径优先：绝大多数字面量是 [±]整数[.小数]，直接拼 64 位尾数构造 decimal。
			// 词法已保证 token 内不含空白/千分位/货币符号，NumberStyles.Any 里那些检查全是白付的，
			// 指数记法等少数形态退回 decimal.TryParse(Number)。
			if(TryParseDecimalFast(span, out decimal d)) {
				return new Function_Number(Operand.Create(d));
			}
			// Float = 空白 + 正负号 + 小数点 + 指数（Number 不含指数，"3E5" 会挂）
			if(decimal.TryParse(span, NumberStyles.Float, CultureInfo.InvariantCulture, out d)) {
				return new Function_Number(Operand.Create(d));
			}
			// 解析失败只可能是: 数值超出 decimal 范围, 或数值后带单位。
			// 仅当后缀确实命中单位字典时才按单位切分, 否则一律视为非法数值,
			// 避免把指数记法的 'E' 或超长数字的末位误当作单位。
			if(TrySplitUnit(span, out var txt, out var unit)) {
				if(!TryParseDecimalFast(txt, out var d2)
					&& !decimal.TryParse(txt, NumberStyles.Float, CultureInfo.InvariantCulture, out d2)) {
					throw new ArgumentException($"Number '{span.ToString()}' is invalid or out of range.");
				}
				return new Function_Number2(d2, unit);
			}
			throw new ArgumentException($"Number '{span.ToString()}' is invalid or out of range.");
		}

		internal static FunctionBase CreateText(string text, int start, int end)
		{
			return new Function_ValueText(Operand.Create(UnescapeString(text.AsSpan(start + 1, end - start - 1))));
		}

		internal static FunctionBase CreateParameter(string name)
		{
			return new Function_Parameter(name);
		}

		#endregion

		#region 组合节点

		/// <summary>
		/// 按节点种类构造函数节点。子节点已由调用方压入 <paramref name="children"/> 的 [offset, offset+count) 区间。
		/// 参数个数为 1/2 的结构直接走单参/双参构造，不额外分配参数数组。
		/// </summary>
		internal static FunctionBase Create(FastNodeKind kind, int type, FastHandle[] children, int offset, int count, string text)
		{
			switch(kind) {
				case FastNodeKind.Bracket: return children[offset].Fn;
				case FastNodeKind.Not: return new Function_NOT(children[offset].Fn);
				case FastNodeKind.Percentage: return new Function_Percentage(children[offset].Fn);

				case FastNodeKind.Mul: return new Function_Mul(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.Div: return new Function_Div(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.Mod: return new Function_Mod(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.Add: return new Function_Add(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.Sub: return new Function_Sub(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.Concat: return new Function_Connect(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.GreaterThan: return new Function_GT(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.GreaterOrEqual: return new Function_GE(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.LessThan: return new Function_LT(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.LessOrEqual: return new Function_LE(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.Equal: return new Function_EQ(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.NotEqual: return new Function_NE(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.And: return new Function_AND(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.Or: return new Function_OR(children[offset].Fn, children[offset + 1].Fn);

				case FastNodeKind.If: return new Function_IF(ToArgs(children, offset, count));
				case FastNodeKind.Call: return FastFunctionFactory.Create(type, ToArgs(children, offset, count), text);
				case FastNodeKind.Array: return new Function_ARRAY(ToArgs(children, offset, count));
				case FastNodeKind.ArrayJson: return new Function_ArrayJson(ToArgs(children, offset, count));
				case FastNodeKind.ArrayJsonItem: return new Function_ArrayJsonItem(UnescapeKey(text), children[offset].Fn);
				case FastNodeKind.GetJson: return new Function_GetJsonValue(children[offset].Fn, children[offset + 1].Fn);
				case FastNodeKind.GetJsonProperty: return new Function_GetJsonValue(children[offset].Fn, new Function_ValueText(Operand.Create(text)));

				default: throw new ArgumentException($"Node '{kind}' can't be built as a composite node.");
			}
		}

		private static FunctionBase[] ToArgs(FastHandle[] children, int offset, int count)
		{
			if(count == 0) { return EmptyArgs; }
			var list = new FunctionBase[count];
			for(int i = 0; i < count; i++) {
				list[i] = children[offset + i].Fn;
			}
			return list;
		}

		#endregion

		#region 数值解析

		/// <summary>
		/// 手写 decimal 快速解析：[−]整数[.小数]，尾数与舍入后总位数 ≤ 18（64 位内）、小数位 ≤ 28。
		/// 覆盖公式字面量的绝大多数形态；构造 decimal 不走字符串慢路径。
		/// 返回 false 时由调用方退回 decimal.TryParse，不是错误。
		/// </summary>
		private static bool TryParseDecimalFast(ReadOnlySpan<char> s, out decimal value)
		{
			var negative = false;
			int i = 0;
			if(i < s.Length && s[i] == '-') { negative = true; i++; }

			// 尾数防溢出上限：(2^64-1-9)/10，保证 *10+digit 不越 ulong
			const ulong MaxMantissa = 1844674407370955161UL;
			ulong mantissa = 0;
			int digits = 0;
			while(i < s.Length) {
				var c = s[i];
				if(c < '0' || c > '9') { break; }
				if(mantissa > MaxMantissa) { value = 0; return false; }
				mantissa = mantissa * 10 + (uint)(c - '0');
				digits++;
				i++;
			}
			if(digits == 0) { value = 0; return false; }

			var scale = 0;
			if(i < s.Length && s[i] == '.') {
				i++;
				while(i < s.Length) {
					var c = s[i];
					if(c < '0' || c > '9') { break; }
					if(mantissa > MaxMantissa) { value = 0; return false; }
					mantissa = mantissa * 10 + (uint)(c - '0');
					scale++;
					i++;
				}
			}
			if(i != s.Length || scale > 28) { value = 0; return false; }

			// mantissa < 2^64，低 64 位就是完整尾数，高位给 0
			value = new decimal((int)mantissa, (int)(mantissa >> 32), 0, negative, (byte)scale);
			return true;
		}

		/// <summary>
		/// 按 Function_Number2 实际支持的单位字典切分数值后缀。
		/// 必须校验完整后缀(而非仅首字母): 否则 "1E29" 这类超出 decimal 范围的指数记法
		/// 会被切成数值 1 + 单位 "E29", 报出"单位非法"这种与 "1E+30" 不一致的误导性错误。
		/// 单位集合固定（19 个、忽略大小写），按后缀长度分组后用 span 直接比对，
		/// 只有命中的那个候选才 ToString 一次——旧实现每个候选后缀都 ToString 一次（最多 3 次）。
		/// </summary>
		private static bool TrySplitUnit(ReadOnlySpan<char> span, out ReadOnlySpan<char> txt, out string unit)
		{
			for(int n = 3; n >= 1; n--) {  // 长后缀优先，与旧 Dictionary 版一致
				if(span.Length <= n) continue;
				var candidate = span.Slice(span.Length - n);
				var matched = MatchUnit(candidate, n);
				if(matched) {
					txt = span.Slice(0, span.Length - n);
					unit = candidate.ToString();  // 保留原文大小写，与旧行为一致
					return true;
				}
			}
			txt = span;
			unit = null;
			return false;
		}

		/// <summary>候选集与 Function_Number2.unitTypedict 的键一致（OrdinalIgnoreCase）</summary>
		private static bool MatchUnit(ReadOnlySpan<char> candidate, int length)
		{
			switch(length) {
				case 3:
					return candidate.Equals("KM2", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("KM3", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("DM2", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("CM2", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("MM2", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("DM3", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("CM3", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("MM3", StringComparison.OrdinalIgnoreCase);
				case 2:
					return candidate.Equals("KM", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("DM", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("CM", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("MM", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("M2", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("M3", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("ML", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("KG", StringComparison.OrdinalIgnoreCase);
				case 1:
					return candidate.Equals("M", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("L", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("T", StringComparison.OrdinalIgnoreCase)
						|| candidate.Equals("G", StringComparison.OrdinalIgnoreCase);
				default:
					return false;
			}
		}

		#endregion

		#region 字符串反转义

		/// <summary>json 的 key：字符串字面量需去掉首尾引号并反转义，其余形式直接用原文</summary>
		internal static string UnescapeKey(string keyText)
		{
			if(keyText != null && keyText.Length > 1 && (keyText[0] == '"' || keyText[0] == '\'' || keyText[0] == '`')) {
				return UnescapeString(keyText.AsSpan(1, keyText.Length - 2));
			}
			return keyText;
		}

		/// <summary>
		/// 把引号之间的内容(不含引号)反转义为实际文本值。
		/// 没有反斜杠时直接切片返回，不建 StringBuilder；有反斜杠时先把第一处之前的内容整段 Append，
		/// 不做「先 Substring 再整段重扫」。
		/// 必须与 OperandString.ToString / Function_ValueText.ToString 的转义输出保持对称,
		/// 否则字面量往返会静默失真(例如 "\u000b" 曾被解析成 "u000b")。
		/// </summary>
		internal static string UnescapeString(ReadOnlySpan<char> content)
		{
			var first = content.IndexOf('\\');
			if(first < 0) { return content.ToString(); }

			var sb = new StringBuilder(content.Length);
			sb.Append(content.Slice(0, first));
			var index = first;
			while(index < content.Length) {
				var c = content[index++];
				if(c != '\\') {
					sb.Append(c);
					continue;
				}
				var c2 = content[index++];
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
						if(index + 4 <= content.Length
							&& int.TryParse(content.Slice(index, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var code)) {
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

		#endregion
	}
}
