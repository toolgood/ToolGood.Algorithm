using System;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_FV : Function_N
	{
		public Function_FV(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "FV";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var rateArg = GetNumber(engine, tempParameter, 0);
			if (rateArg.IsError) return rateArg;
			var rate = rateArg.NumberValue;

			var nperArg = GetNumber(engine, tempParameter, 1);
			if (nperArg.IsError) return nperArg;
			var nper = nperArg.NumberValue;

			var pmtArg = GetNumber(engine, tempParameter, 2);
			if (pmtArg.IsError) return pmtArg;
			var pmt = pmtArg.NumberValue;

			decimal pv = 0;
			if (funcs.Length > 3) {
				var pvArg = GetNumber(engine, tempParameter, 3);
				if (pvArg.IsError) return pvArg;
				pv = pvArg.NumberValue;
			}

			int type = 0;
			if (funcs.Length > 4) {
				var typeArg = GetNumber(engine, tempParameter, 4);
				if (typeArg.IsError) return typeArg;
				type = typeArg.IntValue;
			}

			if (rate == 0) {
				return Operand.Create(-pmt * nper - pv);
			}

			var factor = MathEx.Pow((1 + rate), nper);
			var fv = -pv * factor - pmt * (factor - 1) / rate;
			if (type == 1) {
				fv = -pv * factor - pmt * (1 + rate) * (factor - 1) / rate;
			}

			return Operand.Create(fv);
		}
	}
}
