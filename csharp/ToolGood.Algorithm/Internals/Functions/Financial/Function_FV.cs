using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_FV : Function_5
	{
		public Function_FV(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length < 3 || funcs.Length > 5) {
				throw new ArgumentException($"Function '{Name}' requires 3 to 5 parameters.");
			}
		}

		public override string Name => "FV";

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

			decimal pv = 0;
			if (func4 != null) {
				var pvArg = GetNumber_4(engine, tempParameter);
				if (pvArg.IsErrorOrNone) return pvArg;
				pv = pvArg.NumberValue;
			}

			int type = 0;
			if (func5 != null) {
				var typeArg = GetNumber_5(engine, tempParameter);
				if (typeArg.IsErrorOrNone) return typeArg;
				// 先校验再转换, 避免 typeArg.IntValue 截断小数或在大数值上溢出
				var typeValue = typeArg.NumberValue;
				if (typeValue != 0 && typeValue != 1) {
					return ParameterError(5);
				}
				type = (int)typeValue;
			}

			if (rate == 0) {
				return Operand.Create(-pmt * nper - pv);
			}

			if (!FinancialMath.TryPowPositiveBase((1 + rate), nper, out var factor)) return NumError();

			var fv = -pv * factor - pmt * (factor - 1) / rate;
			if (type == 1) {
				fv = -pv * factor - pmt * (1 + rate) * (factor - 1) / rate;
			}

			return Operand.Create(fv);
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
