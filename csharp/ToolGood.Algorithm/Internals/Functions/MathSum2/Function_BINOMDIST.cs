using System;
using System.Collections.Generic;
using ToolGood.Algorithm.Enums;
using ToolGood.Algorithm.MathNet.Numerics;

namespace ToolGood.Algorithm.Internals.Functions.MathSum2
{
	internal sealed class Function_BINOMDIST : Function_4
	{
		public Function_BINOMDIST(FunctionBase[] funcs) : base(funcs)
		{
			if (funcs.Length != 4) {
				throw new ArgumentException($"Function '{Name}' requires exactly 4 parameters.");
			}
		}

		public override string Name => "BinomDist";

		public override Operand Evaluate(AlgorithmEngine engine, Func<AlgorithmEngine, string, Operand> tempParameter)
		{
			var args1 = GetNumber_1(engine, tempParameter);
			if(args1.IsErrorOrNone) return args1;

			var args2 = GetNumber_2(engine, tempParameter);
			if(args2.IsErrorOrNone) return args2;

			var args3 = GetNumber_3(engine, tempParameter);
			if(args3.IsErrorOrNone) return args3;

			var args4 = GetBoolean_4(engine, tempParameter);
			if(args4.IsErrorOrNone) return args4;

			var n2 = args2.IntValue;
			if(n2 < 0) {
				return ParameterError(2);
			}
			var k = args1.IntValue;
			if(k < 0 || k > n2) {
				return ParameterError(1);
			}
			var n3 = args3.NumberValue;
			if(n3 < 0m || n3 > 1m) {
				return ParameterError(3);
			}
			return Operand.Create(ExcelFunctions.BinomDist(args1.IntValue, n2, n3, args4.BooleanValue));
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
			func4.GetParameterTypes(noneEngine, result, OperandType.BOOLEAN);
		}
	}

}
