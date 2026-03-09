using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_PV : Function_5
	{
		public Function_PV(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "PV";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var rateArg = GetNumber_1(engine, tempParameter);
			if (rateArg.IsErrorOrNone) return rateArg;
			var rate = rateArg.NumberValue;

			var nperArg = GetNumber_2(engine, tempParameter);
			if (nperArg.IsErrorOrNone) return nperArg;
			var nper = nperArg.NumberValue;

			var pmtArg = GetNumber_3(engine, tempParameter);
			if (pmtArg.IsErrorOrNone) return pmtArg;
			var pmt = pmtArg.NumberValue;

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
				return Operand.Create(-pmt * nper - fv);
			}

			var factor = MathEx.Pow((1 + rate), nper);
			var pv = -(fv + pmt * (factor - 1) / rate) / factor;
			if (type == 1) {
				pv = pv - pmt / (1 + rate) * ((factor - 1) / rate) / factor * (1 + rate);
				pv = -(fv + pmt * (1 + rate) * (factor - 1) / rate) / factor;
			}

			return Operand.Create(pv);
		}
		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func2 != null) func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func3 != null) func3.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func4 != null) func4.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			if(func5 != null) func5.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
