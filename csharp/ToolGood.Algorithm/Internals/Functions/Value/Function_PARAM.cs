using System;
using System.Text;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Value
{
	internal sealed class Function_Param : Function_2
	{
		public Function_Param(FunctionBase[] funcs) : base(funcs)
		{
		}

		

		public override string Name => "Param";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetText_1(engine, tempParameter);
			if (args1.IsError) { return args1; }
			if (tempParameter != null) {
				var r = tempParameter(engine, args1.TextValue);
				if (r != null) return r;
			}
			var result = engine.GetParameter(args1.TextValue);
			if (result.IsError) {
				if (func2 != null) {
					return func2.Evaluate(engine, tempParameter);
				}
			}
			return result;
		}
		public override OperandType GetResultType()
		{
			return OperandType.NONE;
		}
	}

}
