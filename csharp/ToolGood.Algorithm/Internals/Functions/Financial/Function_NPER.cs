using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_NPER : Function_5
	{
		public Function_NPER(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "NPER";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var rateArg = GetNumber_1(engine, tempParameter);
			if (rateArg.IsErrorOrNone) return rateArg;
			var rate = rateArg.NumberValue;

			var pmtArg = GetNumber_2(engine, tempParameter);
			if (pmtArg.IsErrorOrNone) return pmtArg;
			var pmt = pmtArg.NumberValue;

			var pvArg = GetNumber_3(engine, tempParameter);
			if (pvArg.IsErrorOrNone) return pvArg;
			var pv = pvArg.NumberValue;

			decimal fv = 0;
			if (func4 != null) {
				var fvArg = GetNumber_4(engine, tempParameter);
				if (fvArg.IsErrorOrNone) return fvArg;
				fv = fvArg.NumberValue;
			}

			int type = 0;
			if (func5 != null) {
				var typeArg = GetNumber_5(engine, tempParameter);
				if (typeArg.IsErrorOrNone) return typeArg;
				type = typeArg.IntValue;
			}

			if (rate == 0) {
				if (pmt == 0) return Div0Error();
				return Operand.Create(-(pv + fv) / pmt);
			}
			if (rate == -1) {
				return Div0Error();
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

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
