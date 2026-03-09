using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.MathTrigonometric
{
	internal sealed class Function_COTH : Function_1
	{
		public Function_COTH(FunctionBase func1) : base(func1)
		{
		}

		public override string Name => "Coth";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if (args1.IsErrorOrNone) { return args1; }
			var d = MathEx.Sinh(args1.NumberValue);
			if (d == 0) {
				return Div0Error();
			}
			return Operand.Create(MathEx.Cosh(args1.NumberValue) / d);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
