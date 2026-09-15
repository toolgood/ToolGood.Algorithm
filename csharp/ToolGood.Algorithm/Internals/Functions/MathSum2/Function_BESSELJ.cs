using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_BESSELJ : Function_2
	{
		public Function_BESSELJ(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

		public override string Name => "BesselJ";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var x = args1.NumberValue;
			// n 取整数部分，且必须为非负整数(与 Excel BESSELJ 一致)
			if(!TryGetInt(args2, out var n) || n < 0) {
				return ParameterError(2);
			}

			try {
				return Operand.Create(SpecialFunctions.BesselJ(n, x));
			} catch (Exception ex) when (ex is OverflowException || ex is DivideByZeroException || ex is InvalidOperationException || ex is ArgumentException) {
				// x 过大时结果超出 decimal 范围,捕获并返回错误
				return FunctionError();
			}
		}

		public override OperandType GetResultType()
		{
			return OperandType.NUMBER;
		}

		internal override void GetParameterTypes(NoneEngine noneEngine, List<ParameterType> result, OperandType operandType, string op = null, string val = null)
		{
			func1.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
			func2.GetParameterTypes(noneEngine, result, OperandType.NUMBER);
		}
	}
}
