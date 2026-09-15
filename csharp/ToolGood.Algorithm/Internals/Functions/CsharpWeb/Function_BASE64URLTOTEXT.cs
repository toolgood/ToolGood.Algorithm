using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	/// <summary>
	/// Base64UrlToText：将 URL 安全型 Base64（RFC 4648 §5，'-'、'_' 分别等价于 '+'、'/'，结尾 '=' 可省略）按 UTF-8 解码为文本。
	/// 输入不符合 Base64 格式时返回参数错误。
	/// </summary>
	internal sealed class Function_BASE64URLTOTEXT : Function_1
	{
		public Function_BASE64URLTOTEXT(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Base64UrlToText";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			try {
				var base64Url = args1.TextValue.Replace('-', '+').Replace('_', '/');
				var padding = 4 - (base64Url.Length % 4);
				if (padding < 4) {
					base64Url += new string('=', padding);
				}
				var t = Encoding.UTF8.GetString(Convert.FromBase64String(base64Url));
				return Operand.Create(t);
			} catch {
				return ParameterError(1);
			}
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
