using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	/// <summary>
	/// TextToBase64Url：将文本按 UTF-8 编码为 URL 安全型 Base64 字符串（'+'、'/' 分别替换为 '-'、'_'，并去掉结尾的 '=' 填充）。
	/// </summary>
	internal sealed class Function_TEXTTOBASE64URL : Function_1
	{
		public Function_TEXTTOBASE64URL(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "TextToBase64Url";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var bytes = Encoding.UTF8.GetBytes(args1.TextValue);
			// 单遍生成：一次 Base64 编码后就地替换并截断 padding，避免 Replace/Replace/TrimEnd 产生的多个中间字符串
			var len = ((bytes.Length + 2) / 3) * 4;
			var buf = new char[len];
			Convert.ToBase64CharArray(bytes, 0, bytes.Length, buf, 0);
			var end = len;
			for (int i = 0; i < len; i++) {
				var c = buf[i];
				if (c == '+') { buf[i] = '-'; }
				else if (c == '/') { buf[i] = '_'; }
				else if (c == '=') { end = i; break; }
			}
			return Operand.Create(new string(buf, 0, end));
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.TEXT);
		}

	}

}
