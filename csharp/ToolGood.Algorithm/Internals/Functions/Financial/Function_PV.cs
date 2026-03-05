using System;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_PV : Function_N
	{
		public Function_PV(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "PV";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var rateArg = GetNumber(engine, tempParameter, 0);
			if (rateArg.IsError) return rateArg;
			var rate = rateArg.DoubleValue;

			var nperArg = GetNumber(engine, tempParameter, 1);
			if (nperArg.IsError) return nperArg;
			var nper = nperArg.DoubleValue;

			var pmtArg = GetNumber(engine, tempParameter, 2);
			if (pmtArg.IsError) return pmtArg;
			var pmt = pmtArg.DoubleValue;

			double fv = 0;
			if (funcs.Length > 3) {
				var fvArg = GetNumber(engine, tempParameter, 3);
				if (fvArg.IsError) return fvArg;
				fv = fvArg.DoubleValue;
			}

			int type = 0;
			if (funcs.Length > 4) {
				var typeArg = GetNumber(engine, tempParameter, 4);
				if (typeArg.IsError) return typeArg;
				type = typeArg.IntValue;
			}

			if (rate == 0) {
				return Operand.Create(-pmt * nper - fv);
			}

			var factor = Math.Pow((1 + rate), nper);
			var pv = -(fv + pmt * (factor - 1) / rate) / factor;
			if (type == 1) {
				pv = pv - pmt / (1 + rate) * ((factor - 1) / rate) / factor * (1 + rate);
				pv = -(fv + pmt * (1 + rate) * (factor - 1) / rate) / factor;
			}

			return Operand.Create(pv);
		}
	}
}
