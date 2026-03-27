using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_PPMT : Function_6
	{
		public Function_PPMT(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "PPMT";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var rateArg = GetNumber_1(engine, tempParameter);
			if (rateArg.IsErrorOrNone) return rateArg;
			var rate = rateArg.NumberValue;

			var perArg = GetNumber_2(engine, tempParameter);
			if (perArg.IsErrorOrNone) return perArg;
			var per = perArg.NumberValue;

			var nperArg = GetNumber_3(engine, tempParameter);
			if (nperArg.IsErrorOrNone) return nperArg;
			var nper = nperArg.NumberValue;

			var pvArg = GetNumber_4(engine, tempParameter);
			if (pvArg.IsErrorOrNone) return pvArg;
			var pv = pvArg.NumberValue;

			decimal fv = 0;
			if (func5 != null) {
				var fvArg = GetNumber_5(engine, tempParameter);
				if (fvArg.IsErrorOrNone) return fvArg;
				fv = fvArg.NumberValue;
			}

			int type = 0;
			if (func6 != null) {
				var typeArg = GetNumber_6(engine, tempParameter);
				if (typeArg.IsErrorOrNone) return typeArg;
				type = typeArg.IntValue;
				if (type != 0 && type != 1) {
					return ParameterError(6);
				}
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

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func6 != null) func6.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
