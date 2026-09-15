using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;

namespace ToolGood.Algorithm.Internals.Functions.Financial
{
	internal sealed class Function_NPER : Function_5
	{
		public Function_NPER(FunctionBase[] funcs) : base(funcs) {
			if (funcs.Length < 3 || funcs.Length > 5) {
				throw new ArgumentException($"Function '{Name}' requires 3 to 5 parameters.");
			}
		}

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
				// 先校验再转换, 避免 typeArg.IntValue 截断小数或在大数值上溢出
				var typeValue = typeArg.NumberValue;
				if (typeValue != 0 && typeValue != 1) {
					return ParameterError(5);
				}
				type = (int)typeValue;
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

			// MathEx.Log 要求参数大于 0, 底数与真数非法时返回错误而非抛异常
			var logBase = 1 + rate;
			if (logBase <= 0) {
				return NumError();
			}
			var denominator = pv * rate + factor;
			if (denominator == 0) {
				return Div0Error();
			}
			var ratio = (-fv * rate + factor) / denominator;
			if (ratio <= 0) {
				return NumError();
			}

			var nper = MathEx.Log(ratio) / MathEx.Log(logBase);
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
