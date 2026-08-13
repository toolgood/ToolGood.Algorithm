using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_COTH : Function_1
	{
		public Function_COTH(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 1) {
				throw new ArgumentException($"Function '{Name}' requires exactly 1 parameter.");
			}
		}

		public override string Name => "Coth";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var x = args1.NumberValue;
			// coth 在 |x| 较大时趋近 ±1,提前返回避免 e^|x| 溢出
			if (x >= 66) { return Operand.Create(1m); }
			if (x <= -66) { return Operand.Create(-1m); }
			var d = MathEx.Sinh(x);
			if (d == 0) {
				return Div0Error();
			}
			return Operand.Create(MathEx.Cosh(x) / d);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
