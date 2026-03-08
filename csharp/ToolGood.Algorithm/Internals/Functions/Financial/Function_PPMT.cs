using System;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_PPMT : Function_N
	{
		public Function_PPMT(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "PPMT";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 4) return ParameterError(1);

			var rateArg = GetNumber(engine, tempParameter, 0);
			if (rateArg.IsError) return rateArg;
			var rate = rateArg.NumberValue;

			var perArg = GetNumber(engine, tempParameter, 1);
			if (perArg.IsError) return perArg;
			var per = perArg.NumberValue;

			var nperArg = GetNumber(engine, tempParameter, 2);
			if (nperArg.IsError) return nperArg;
			var nper = nperArg.NumberValue;

			var pvArg = GetNumber(engine, tempParameter, 3);
			if (pvArg.IsError) return pvArg;
			var pv = pvArg.NumberValue;

			decimal fv = 0;
			if (funcs.Length > 4) {
				var fvArg = GetNumber(engine, tempParameter, 4);
				if (fvArg.IsError) return fvArg;
				fv = fvArg.NumberValue;
			}

			int type = 0;
			if (funcs.Length > 5) {
				var typeArg = GetNumber(engine, tempParameter, 5);
				if (typeArg.IsError) return typeArg;
				type = typeArg.IntValue;
			}

			var pmtResult = CalculatePMT(rate, nper, pv, fv, type);
			var ipmtResult = CalculateIPMT(rate, per, nper, pv, fv, type);
			return Operand.Create(pmtResult - ipmtResult);
		}

		private decimal CalculatePMT(decimal rate, decimal nper, decimal pv, decimal fv, int type)
		{
			if (rate == 0) {
				return -(pv + fv) / nper;
			}
			var factor = MathEx.Pow((1 + rate), nper);
			var pmt = -(pv * factor + fv) * rate / (factor - 1);
			if (type == 1) {
				pmt = pmt / (1 + rate);
			}
			return pmt;
		}

		private decimal CalculateIPMT(decimal rate, decimal per, decimal nper, decimal pv, decimal fv, int type)
		{
			if (rate == 0) {
				return 0;
			}
			var pmt = CalculatePMT(rate, nper, pv, fv, type);
			var factor = MathEx.Pow((1 + rate), (per - 1));
			var ipmt = -(pv * factor + pmt * (factor - 1) / rate) * rate;
			if (type == 1 && per == 1) {
				ipmt = 0;
			}
			return ipmt;
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
