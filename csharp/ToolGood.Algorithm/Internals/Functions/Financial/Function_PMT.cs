using System;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_PMT : Function_N
	{
		public Function_PMT(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "PMT";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			if (funcs.Length < 3) return ParameterError(1);

			var rateArg = GetNumber(engine, tempParameter, 0);
			if (rateArg.IsError) return rateArg;
			var rate = rateArg.NumberValue;

			var nperArg = GetNumber(engine, tempParameter, 1);
			if (nperArg.IsError) return nperArg;
			var nper = nperArg.NumberValue;

			var pvArg = GetNumber(engine, tempParameter, 2);
			if (pvArg.IsError) return pvArg;
			var pv = pvArg.NumberValue;

			if (nper == 0) {
				return Div0Error();
			}

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
				type = typeArg.IntValue;
			}

			if (rate == 0) {
				return Operand.Create(-(pv + fv) / nper);
			}

			var factor = MathEx.Pow((1 + rate), nper);
			var pmt = -(pv * factor + fv) * rate / (factor - 1);
			if (type == 1) {
				pmt = pmt / (1 + rate);
			}

			return Operand.Create(pmt);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}
	}
}
