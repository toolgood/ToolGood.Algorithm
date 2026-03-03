using System;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_SYD : Function_N
	{
		public Function_SYD(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "SYD";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 4) return ParameterError(1);

			var costArg = GetNumber(engine, tempParameter, 0);
			if (costArg.IsError) return costArg;
			var cost = costArg.NumberValue;

			var salvageArg = GetNumber(engine, tempParameter, 1);
			if (salvageArg.IsError) return salvageArg;
			var salvage = salvageArg.NumberValue;

			var lifeArg = GetNumber(engine, tempParameter, 2);
			if (lifeArg.IsError) return lifeArg;
			var life = lifeArg.NumberValue;

			var periodArg = GetNumber(engine, tempParameter, 3);
			if (periodArg.IsError) return periodArg;
			var period = periodArg.NumberValue;

			if (life == 0) return Div0Error();

			var syd = (cost - salvage) * (life - period + 1) * 2 / (life * (life + 1));
			return Operand.Create(syd);
		}
	}
}
