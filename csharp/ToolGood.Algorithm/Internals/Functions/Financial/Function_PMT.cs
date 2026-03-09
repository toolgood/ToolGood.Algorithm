using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.Internals;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_PMT : Function_5
	{
		public Function_PMT(FunctionBase[] funcs) : base(funcs) { }

		public override string Name => "PMT";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var rateArg = GetNumber_1(engine, tempParameter);
			if (rateArg.IsErrorOrNone) return rateArg;
			var rate = rateArg.NumberValue;

			var nperArg = GetNumber_2(engine, tempParameter);
			if (nperArg.IsErrorOrNone) return nperArg;
			var nper = nperArg.NumberValue;

			var pvArg = GetNumber_3(engine, tempParameter);
			if (pvArg.IsErrorOrNone) return pvArg;
			var pv = pvArg.NumberValue;

			if (nper == 0) {
				return Div0Error();
			}

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
