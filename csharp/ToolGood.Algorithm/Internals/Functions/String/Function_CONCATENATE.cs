using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.String
{
	internal sealed class Function_CONCATENATE : Function_N
	{
		public Function_CONCATENATE(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length < 1) {
				throw new ArgumentException($"Function '{Name}' requires at least 1 parameter.");
			}
		}

		public override string Name => "Concatenate";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length == 0) {
				return Operand.Create(string.Empty);
			}
			if (funcs.Length == 1) {
				var a = GetText(engine, tempParameter, 0);
				if (a.IsErrorOrNone) { return a; }
				return a; // 只有一个参数
			}
			var sb = new StringBuilder(funcs.Length * 16);
			for (int i = 0; i < funcs.Length; i++) {
				// 快路径：直接写入 StringBuilder，免去「Create(string) 造 OperandString → 再取 TextValue」
				// 每个参数省一次 Operand 装箱与一次中间 string；失败时回落原路径，错误消息与参数序号不变
				var a = funcs[i].Evaluate(engine, tempParameter);
				if (a.IsErrorOrNone) { return a; }
				if (a.TryAppendText(sb)) { continue; }
				var t = ConvertToText(a, i + 1);
				if (t.IsErrorOrNone) { return t; }
				sb.Append(t.TextValue);
			}
			return Operand.Create(sb.ToString());
		}
		public override OperandType GetResultType()
		{
			return OperandType.TEXT;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			for(int i = 0; i < funcs.Length; i++) {
				funcs[i].GetParameterTypes(noneEngine, result, OperandType.TEXT);
			}
		}
	}
}