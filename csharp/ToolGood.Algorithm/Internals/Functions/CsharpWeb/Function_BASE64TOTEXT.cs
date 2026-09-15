using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.CsharpWeb
{
	/// <summary>
	/// Base64ToText：将标准 Base64 字符串按 UTF-8 解码为文本。
	/// 输入不符合 Base64 格式时返回参数错误。
	/// </summary>
	internal sealed class Function_BASE64TOTEXT : Function_1
	{
		public Function_BASE64TOTEXT(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Base64ToText";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			try {
				var t = Encoding.UTF8.GetString(Convert.FromBase64String(args1.TextValue));
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
