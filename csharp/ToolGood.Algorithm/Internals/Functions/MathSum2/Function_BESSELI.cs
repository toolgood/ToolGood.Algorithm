using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_BESSELI : Function_2
	{
		public Function_BESSELI(FunctionBase func1, FunctionBase func2) : base(func1, func2)
		{
		}

		public Function_BESSELI(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 2) {
				throw new ArgumentException($"Function '{Name}' requires exactly 2 parameters.");
			}
		}

		public override string Name => "BesselI";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsErrorOrNone) { return args1; }
			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) { return args2; }

			var x = args1.NumberValue;
			var n = (int)Math.Truncate(args2.NumberValue);

			try {
				// 复用 SpecialFunctions 中基于 Chebyshev 展开的实现(移植自 MathNet.Numerics)
				return Operand.Create(SpecialFunctions.BesselI(n, x));
			} catch (OverflowException) {
				// x 过大时 MathEx.Exp 结果超出 decimal 范围,捕获并返回错误
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
