using System;
using ToolGood.Algorithm.Enums;

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
			if (rateArg.IsErrorOrNone) return rateArg;
			var rate = rateArg.NumberValue;

			var pmtArg = GetNumber(engine, tempParameter, 1);
			if (pmtArg.IsErrorOrNone) return pmtArg;
			var pmt = pmtArg.NumberValue;

			var pvArg = GetNumber(engine, tempParameter, 2);
			if (pvArg.IsErrorOrNone) return pvArg;
			var pv = pvArg.NumberValue;

			decimal fv = 0;
			if (funcs.Length > 3) {
				var fvArg = GetNumber(engine, tempParameter, 3);
				if (fvArg.IsErrorOrNone) return fvArg;
				fv = fvArg.NumberValue;
			}

			int type = 0;
			if (funcs.Length > 4) {
				var typeArg = GetNumber(engine, tempParameter, 4);
				if (typeArg.IsErrorOrNone) return typeArg;
				type = typeArg.IntValue;
			}

			if (rate == 0) {
				if (pmt == 0) return Div0Error();
				return Operand.Create(-(pv + fv) / pmt);
			}

			var factor = pmt;
			if (type == 1) {
				factor = pmt * (1 + rate);
			}

			var nper = MathEx.Log((-fv * rate + factor) / (pv * rate + factor)) / MathEx.Log((1 + rate));
			return Operand.Create(nper);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
