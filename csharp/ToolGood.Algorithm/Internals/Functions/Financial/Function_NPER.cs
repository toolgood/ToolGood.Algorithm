using System;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_NPER : Function_N
	{
		public Function_NPER(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "NPER";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var rateArg = GetNumber(engine, tempParameter, 0);
			if (rateArg.IsError) return rateArg;
			var rate = rateArg.NumberValue;

			var pmtArg = GetNumber(engine, tempParameter, 1);
			if (pmtArg.IsError) return pmtArg;
			var pmt = pmtArg.NumberValue;

			var pvArg = GetNumber(engine, tempParameter, 2);
			if (pvArg.IsError) return pvArg;
			var pv = pvArg.NumberValue;

			decimal fv = 0;
			if (funcs.Length > 3) {
				var fvArg = GetNumber(engine, tempParameter, 3);
				if (fvArg.IsError) return fvArg;
				fv = fvArg.NumberValue;
			}

			int type = 0;
			if (funcs.Length > 4) {
				var typeArg = GetNumber(engine, tempParameter, 4);
				if (typeArg.IsError) return typeArg;
				type = (int)typeArg.NumberValue;
			}

			if (rate == 0) {
				if (pmt == 0) return Div0Error();
				return Operand.Create(-(pv + fv) / pmt);
			}

			var factor = pmt;
			if (type == 1) {
				factor = pmt * (1 + rate);
			}

			var nper = Math.Log((double)(-fv * rate + factor) / (double)(pv * rate + factor)) / Math.Log((double)(1 + rate));
			return Operand.Create((decimal)nper);
		}
	}
}
