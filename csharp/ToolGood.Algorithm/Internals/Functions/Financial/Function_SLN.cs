using System;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_SLN : Function_N
	{
		public Function_SLN(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SLN";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var costArg = GetNumber(engine, tempParameter, 0);
			if (costArg.IsError) return costArg;
			var cost = costArg.NumberValue;

			var salvageArg = GetNumber(engine, tempParameter, 1);
			if (salvageArg.IsError) return salvageArg;
			var salvage = salvageArg.NumberValue;

			var lifeArg = GetNumber(engine, tempParameter, 2);
			if (lifeArg.IsError) return lifeArg;
			var life = lifeArg.NumberValue;

			if (life == 0) return Div0Error();

			return Operand.Create((cost - salvage) / life);
		}
	}
}
